using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.Carrito
{
    public interface ICarritoRepository
    {
            Task<DAL.Entities.Carrito> GetCarritoByID(int carritoID); 
            Task<DAL.Entities.Carrito> CreateCarrito(int productoID);
            Task<List<DAL.Entities.Carrito>> GetAllCarritos();
    }
}
