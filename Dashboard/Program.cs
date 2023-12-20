using Dashboard.Data;
using Dashboard.DataAccess.Repo;
using Dashboard.DataAccess.Repo.IRepository;
using Dashboard.DataAccess.UnitOfWork;
using Dashboard.Helpers;
using Dashboard.Mapping;
using Dashboard.Models.Models;
using Dashboard.Utillities.Helper;
using Dashboard.Utillities.Helper.Email;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File("Logs/dashboarLogs-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Add services to the container.
builder.Services.AddControllersWithViews();
    //.AddJsonOptions(options =>
    //{
    //    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    //});

builder.Services.AddDbContext<ProjectContext>( options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("connection"));
});

var hangfireConnection = builder.Configuration.GetConnectionString("hangfireconnection");

// for Configring Identity 




builder.Services.AddIdentity<Dashboard.Models.Models.CustomeUser, Microsoft.AspNetCore.Identity.IdentityRole>()
   .AddEntityFrameworkStores<ProjectContext>().AddDefaultTokenProviders();

// services

builder.Services.AddScoped<ICreateImage, CreateImage>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IHelper, Helper>();
builder.Services.AddScoped<IOathRepo, OathRepo>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IGenrateBillMonthly, GenrateBillMonthly>();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<CustomeUser>, CustomeUserClaimsPrincepleFactory>();
builder.Services.AddScoped<IUserService,UserService>();


// Hanfire Client Configration

builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(hangfireConnection)
    );

// Login Path Defined

builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = "/login";
    config.ExpireTimeSpan = TimeSpan.FromHours(1);
    config.AccessDeniedPath = "/unauth";

});

//builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
//               .AddCookie(options =>
//               {
//                   options.LoginPath = "/login";
//                   options.ExpireTimeSpan = new System.TimeSpan(0, 50, 0);
//                   options.SlidingExpiration = true;

//                   options.Cookie = new CookieBuilder
//                   {
//                       SameSite = SameSiteMode.Strict,
//                       SecurePolicy = CookieSecurePolicy.Always,
//                       IsEssential = true,
//                       HttpOnly = true
//                   };
//                   options.Cookie.Name = "MyCookie";
//               });
// Identity Configration 

// This was to check for confirmed emails only but asif sb told to not require this 

//builder.Services.Configure<IdentityOptions>(config =>
//    {
//        // Configration for Confirmed Emails Only
//        config.SignIn.RequireConfirmedEmail = true;
//    }
//);


// Hangfire Server 

builder.Services.AddHangfireServer();

QuestPDF.Settings.License = LicenseType.Community;
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


app.MapHangfireDashboard("/hanfire", new DashboardOptions
{
    // This is for Production to able to access Hangfire Dashboard by Admin only  which is specified in the AuthorzationFilter
    Authorization = new[] { new MyAuthorizationFilter() }
});
RecurringJob.AddOrUpdate("Add monthly Billing",() => builder.Services.BuildServiceProvider()
                                                    .GetRequiredService<IGenrateBillMonthly>().GerateBill() , Cron.Monthly(1));


app.MapControllerRoute(
    name: "default",
    //pattern: "{controller=UserForm}/{action=Clients}/{id?}");
    pattern: "{controller=oath}/{action=Index}/{id?}");



app.Run();
