using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebAppMVC.Models;
using DAL.Repository.Cliente;
using DAL.Repository.Producto;
using DAL.Repository.Carrito;
using DAL.Entities;
using Microsoft.VisualBasic;
namespace WebAppMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClienteRepository _clienteRepository;
        private readonly IProductoRepository _productoRepository;
        private readonly ICarritoRepository _carritoRepository;



        public HomeController(IClienteRepository clienteRep, IProductoRepository productoRep, ICarritoRepository carritoRep)
        {
            _clienteRepository = clienteRep;
            _productoRepository = productoRep;
            _carritoRepository = carritoRep;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // PRODUCTOS ---------------------------------------------------------------------------------------------------------------
        public async Task<IActionResult> DisplayProductos()
        {
            List<Producto> listaProductos = await _productoRepository.GetAllProductos();
            return View("Producto/DisplayProductos",listaProductos);
        }

        // mostrar producto por ID
        public async Task<IActionResult> DisplayProductoByID(int id)
        {
            Producto producto = await _productoRepository.GetProductoByID(id);
            return View("Producto/DisplayProductoByID",producto);
        }

        [HttpGet]
        public IActionResult CreateProducto()
        {
            return View("Producto/CreateProducto");
        }

        [HttpPost]
        public async Task<IActionResult> CreateProducto(Producto producto)
        {
            if (!ModelState.IsValid)
            {
                return View("Producto/CreateProducto", producto);
            }

            await _productoRepository.CreateProducto(producto.nombreProducto,producto.precio);

            return RedirectToAction(nameof(DisplayProductos));
        }


        //CLIENTES ---------------------------------------------------------------------------------------------------------------
        public async Task<IActionResult> DisplayClientes()
        {
            List<Cliente> listaClientes = await _clienteRepository.GetAllClientes();
            return View("Cliente/DisplayClientes",listaClientes);
        }

        [HttpGet]
        public IActionResult CreateCliente()
        {
            return View("Cliente/CreateCliente");
        }

        [HttpPost]
        public async Task<IActionResult> CreateCliente(Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return View("Cliente/CreateCliente",cliente);
            }

            await _clienteRepository.CreateCliente(cliente.nombreCliente, cliente.carritoID);

            return RedirectToAction(nameof(DisplayClientes));//wtf
        }

        public async Task<IActionResult> DisplayClienteByID(int clienteID)
        {
            Cliente cliente = await _clienteRepository.GetClienteByID(clienteID);
            return View("Cliente/DisplayClienteByID", cliente);
        }

        //CARRITOS ---------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public IActionResult CreateCarrito()
        {
            return View("Carrito/CreateCarrito");
        }

        [HttpPost]
        public async Task<IActionResult> CreateCarrito(Carrito carrito)
        {
            if (!ModelState.IsValid)
            {
                return View("Carrito/CreateCarrito",carrito);
            }

            await _carritoRepository.CreateCarrito(carrito.productoID);

            return RedirectToAction("Index"); //no hace falta retornar o si?
        }
        public async Task<IActionResult> DisplayCarritoByID(int carritoID)
        {
            Carrito carrito = await _carritoRepository.GetCarritoByID(carritoID);
            return View("Carrito/DisplayCarritoByID",carrito);
        }
        public async Task<IActionResult> DisplayCarritos()
        {
            List<Carrito> listaCarrito = await _carritoRepository.GetAllCarritos();
            return View("Carrito/DisplayCarritos", listaCarrito);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
