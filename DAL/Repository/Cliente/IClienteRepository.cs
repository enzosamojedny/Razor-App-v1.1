using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Repository.Cliente
{
    public interface IClienteRepository
    {
        Task<DAL.Entities.Cliente> CreateCliente(string nombreCliente, int carritoID);
        Task<DAL.Entities.Cliente> GetClienteByID(int clienteID);
        Task<List<DAL.Entities.Cliente>> GetAllClientes();
    }
}
