using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Cliente
    {
        public int clienteID { get; set; }
        public string nombreCliente { get; set; }
        public int carritoID { get;set; }
    }
}
