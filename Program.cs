using Microsoft.AspNetCore.Authentication.Cookies;
using Servicios;
using Servicios.Seguimiento;



var builder = WebApplication.CreateBuilder(args);
var mvc = builder.Services.AddRazorPages();
builder.Services.AddScoped<DBHelper>();
builder.Services.AddScoped<ProyectoSevices>();
builder.Services.AddScoped<MacService>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();
// Agrega el uso de tu middleware personalizado aqu�
//app.UseMiddleware<CustomMiddleware>();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapGet("/Logout", (HttpContext context) =>
{
    // Aqu� puedes agregar la l�gica para cerrar la sesi�n del usuario
    // Por ejemplo, puedes eliminar la informaci�n de autenticaci�n o borrar las cookies relacionadas con la sesi�n

    // Redirecciona al inicio o a la p�gina de inicio de sesi�n

    context.Response.Redirect("Home/Index");
});


app.Run();
