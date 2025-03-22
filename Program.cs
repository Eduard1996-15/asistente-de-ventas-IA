using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using asistenteventas.Data;
using Microsoft.Extensions.Azure;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    { option.LoginPath = "/Usuarios/Login";
      option.ExpireTimeSpan =  TimeSpan.FromHours(6);
      option.AccessDeniedPath = "/Usuarios/Login";
     });
    
builder.Services.AddDbContext<ASISTENTE_DE_VENTASContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
//app.UseSession();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuarios}/{action=Login}/{id?}");

app.Run();
