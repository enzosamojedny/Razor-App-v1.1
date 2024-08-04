using DAL.Repository.Cliente;
using DAL.Repository.Producto;
using DAL.Repository.Carrito;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using DAL.Entities;
using System.Threading.Tasks;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IClienteRepository _clienteRepository;
        private readonly IProductoRepository _productoRepository;
        private readonly ICarritoRepository _carritoRepository;

        public IndexModel(ILogger<IndexModel> logger, IClienteRepository clienteRepository, IProductoRepository productoRepository, ICarritoRepository carritoRepository)
        {
            _logger = logger;
            _clienteRepository = clienteRepository;
            _productoRepository = productoRepository;
            _carritoRepository = carritoRepository;
        }

        public Cliente Cliente { get; set; }

        public async Task OnGetAsync(int id)
        {
            Cliente = await _clienteRepository.GetClienteByID(id);
        }

        public async Task<IActionResult> OnPostCreateAsync(string nombreCliente, int carritoID)
        {
            var newCliente = await _clienteRepository.CreateCliente(nombreCliente, carritoID);
            return RedirectToPage("./Index", new { id = newCliente.clienteID });
        }
    }
}
