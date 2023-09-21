using Dashboard.Data;
using Dashboard.DataAccess.Repo;
using Dashboard.DataAccess.Repo.IRepository;
using Dashboard.DataAccess.UnitOfWork;
using Dashboard.Mapping;
using Dashboard.Utillities.Helper;
using Dashboard.Utillities.Helper.Email;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

builder.Services.AddDbContext<ProjectContext>( options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("connection"));
});

var hangfireConnection = builder.Configuration.GetConnectionString("hangfireconnection");

// for Configring Identity 

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
   .AddEntityFrameworkStores<ProjectContext>();

builder.Services.AddScoped<ICreateImage, CreateImage>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IHelper, Helper>();
builder.Services.AddScoped<IOathRepo, OathRepo>();
builder.Services.AddScoped<IEmailService, EmailService>();


// Hanfire Client Configration

builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(hangfireConnection)
    );

// Hangfire Server 

builder.Services.AddHangfireServer();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// for Configring Identity 
app.UseAuthentication();

app.UseAuthorization();
app.UseHangfireDashboard();


app.MapHangfireDashboard("/hanfire");
//RecurringJob.AddOrUpdate(() => Console.WriteLine("Hello World"), "* * * * *");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=UserForm}/{action=Clients}/{id?}");



app.Run();
