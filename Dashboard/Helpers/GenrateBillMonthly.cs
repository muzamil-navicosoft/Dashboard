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

            var compareDate = DateTime.Now.AddDays(-1).ToString("MMMM");
            var bill = unitOfWork.billing.CustomeGetAll()
                                .Where(x => x.ClientFormId == item.Id && x.Month == compareDate).FirstOrDefault();
            if( bill != null )
            {
                BillingInfo newbill = new BillingInfo();
                //newbill.ClientFormId = bill.Id;
                newbill.ClientFormId = bill.ClientFormId;
                newbill.DueDate = bill.DueDate.AddMonths(1);
                newbill.Amount = bill.Amount;
                newbill.Month = newbill.DueDate.ToString("MMMM");
                newbill.IsPaid = false;
                unitOfWork.billing.Add(newbill);
                unitOfWork.Save();
            }

        }
    }
}
