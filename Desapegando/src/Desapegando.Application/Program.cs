using Desapegando.Application.Controllers;
using Desapegando.Application.Data;
using Desapegando.Application.Extensions;
using Desapegando.Application.HostedService;
using Desapegando.Application.Services;
using Desapegando.Business.Interfaces.Notifications;
using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Interfaces.Services;
using Desapegando.Business.Notifications;
using Desapegando.Business.Services;
using Desapegando.Data.Context;
using Desapegando.Data.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);

});

builder.Services.AddDbContext<DesapegandoDbContext>(options =>
{
    options.UseSqlServer(connectionString);
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.LoginPath = "/Login";
//    //options.AccessDeniedPath = "/Identity/Account/AccessDenied"; /acesso-negado
//});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        //options.AccessDeniedPath = "/acesso-negado";
    });


// Dependcy Injection
//builder.Services.AddScoped<ApplicationDbContext>();
builder.Services.AddTransient<DesapegandoDbContext>();
builder.Services.AddScoped<ICampanhaRepository, CampanhaRepository>();
builder.Services.AddScoped<INotificador, Notificador>();

builder.Services.AddHostedService<CampanhaHostedService>();

builder.Services.Configure<EmailSender>(builder.Configuration.GetSection("EmailSender"));
builder.Services.AddTransient<IEmailSender, AuthMessageSender>();


// AutoMapper
builder.Services.AddAutoMapper(typeof(Program));



builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.Configure<AppSettings>(builder.Configuration);
builder.Services.AddHttpClient<RegisterController>(); // ????????????????????????? Pq a Register Controller?

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");
app.MapRazorPages();

var supportedCultures = new[] { new CultureInfo("pt-BR") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("pt-BR"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.Run();
