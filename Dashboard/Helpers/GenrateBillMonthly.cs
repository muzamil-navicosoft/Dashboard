using Dashboard.DataAccess.UnitOfWork;
using Dashboard.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Helpers;

public class GenrateBillMonthly : IGenrateBillMonthly
{
    private readonly IUnitOfWork unitOfWork;

    public GenrateBillMonthly(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public void GerateBill()
    {
        var result = unitOfWork.User.CustomeGetAll().Where(x => x.isBilledMonthly.Value == true && x.isActive).ToList();
        foreach (var item in result)
        {
            var bill = unitOfWork.billing.CustomeGetAll()
                                .Where(x => x.ClientFormId == item.Id && x.Month == DateTime.Now.AddDays(-1).ToString("MMMM")).FirstOrDefault();
            BillingInfo newbill = new BillingInfo();
            newbill.ClientFormId = bill.Id;
            newbill.DueDate = bill.DueDate.AddMonths(1);
            newbill.Amount = bill.Amount;
            newbill.Month = newbill.DueDate.ToString("MMMM");
            unitOfWork.billing.Add(newbill);
            unitOfWork.Save();
        }
    }
}
