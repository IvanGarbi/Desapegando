using Desapegando.Application.Data;
using Desapegando.Application.Services;
using Desapegando.Business.Interfaces.Notifications;
using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Interfaces.Services;
using Desapegando.Business.Notifications;
using Desapegando.Business.Services;
using Desapegando.Data.Context;
using Desapegando.Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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


// Dependcy Injection
//builder.Services.AddScoped<ApplicationDbContext>();
builder.Services.AddTransient<DesapegandoDbContext>();
builder.Services.AddScoped<ICondominoService, CondominoService>();
builder.Services.AddScoped<ICondominoRepository, CondominoRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IProdutoImagemRepository, ProdutoImagemRepository>();
builder.Services.AddScoped<IProdutoImagemService, ProdutoImagemService>();
builder.Services.AddScoped<ICampanhaRepository, CampanhaRepository>();
builder.Services.AddScoped<ICampanhaService, CampanhaService>();
builder.Services.AddScoped<ICampanhaImagemRepository, CampanhaImagemRepository>();
builder.Services.AddScoped<ICampanhaImagemService, CampanhaImagemService>();
builder.Services.AddScoped<INotificador, Notificador>();
builder.Services.AddScoped<IProdutoCurtidaRepository, ProdutoCurtidaRepository>();
builder.Services.AddScoped<IProdutoCurtidaService, ProdutoCurtidaService>();

builder.Services.Configure<EmailSender>(builder.Configuration.GetSection("EmailSender"));
builder.Services.AddTransient<IEmailSender, AuthMessageSender>();


// AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
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

app.Run();
