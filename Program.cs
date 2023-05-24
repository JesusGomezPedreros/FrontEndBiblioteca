using Servicios;
using Servicios.Seguimiento;

var builder = WebApplication.CreateBuilder(args);
var mvc = builder.Services.AddRazorPages();
builder.Services.AddScoped<DBHelper>();
builder.Services.AddScoped<ProyectoSevices>();
builder.Services.AddScoped<MacService>();

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
// Agrega el uso de tu middleware personalizado aquí
//app.UseMiddleware<CustomMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
