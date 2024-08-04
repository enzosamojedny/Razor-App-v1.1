using DAL.Repository.Cliente;
using DAL.Repository.Carrito;
using DAL.Repository.Producto;
using DAL.Settings;
using System.Data.SqlTypes;
using System.Data.SqlClient;
namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
            builder.Services.AddScoped<ICarritoRepository, CarritoRepository>();
            builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
            builder.Services.Configure<DbConnection>(builder.Configuration.GetSection("ConectionSettings"));
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // i have no idea what this shit does or if i need it or not
            app.MapControllerRoute(
               name: "default",
               pattern: "{controller=Home}/{action=Index}/{id?}");

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
