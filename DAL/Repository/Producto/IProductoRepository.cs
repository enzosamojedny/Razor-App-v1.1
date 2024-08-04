using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.Producto
{
   public interface IProductoRepository
    {
        Task<DAL.Entities.Producto> CreateProducto(string nombreProducto, decimal precio);
        Task<DAL.Entities.Producto> GetProductoByID(int id);
        Task<List<DAL.Entities.Producto>> GetAllProductos();
    }
}
