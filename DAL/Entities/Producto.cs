using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Producto
    {
        public int productoID {  get; set; }
        public string nombreProducto { get; set; }
        public decimal precio {  get; set; }
    }
}
