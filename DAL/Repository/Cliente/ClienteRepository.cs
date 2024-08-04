using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.Cliente
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly string _connectionString;
        public ClienteRepository(IOptions<Settings.DbConnection> connection)
        {
            _connectionString = connection.Value.ConnectionString;
        }

        public async Task<DAL.Entities.Cliente> GetClienteByID(int clienteID)
        {
            DAL.Entities.Cliente? cliente = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_getClienteByID", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@clienteID", SqlDbType.Int)).Value = clienteID;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new DAL.Entities.Cliente
                            {
                                clienteID = (int)reader["clienteID"],
                                nombreCliente = (string)reader["nombreCliente"],
                                carritoID = (int)reader["carritoID"]
                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }


        public async Task<DAL.Entities.Cliente> CreateCliente(string nombreCliente, int carritoID)
        {
            DAL.Entities.Cliente? cliente = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_createCliente", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@nombreCliente", SqlDbType.NVarChar)).Value = nombreCliente;
                cmd.Parameters.Add(new SqlParameter("@carritoID", SqlDbType.Int)).Value = carritoID;

                await cmd.ExecuteNonQueryAsync();
            }
            return new DAL.Entities.Cliente
            {
                nombreCliente = nombreCliente,
                carritoID = carritoID
            };
        }
        public async Task<List<DAL.Entities.Cliente>> GetAllClientes()
        {
            DAL.Entities.Cliente? cliente = null; // this helps avoid nulll reference exceptions! read about it

            //all of this verbosity can be replaced with Dapper, must read about it! it simplifies data inserts / mapping by a lot

            List<DAL.Entities.Cliente> list = new List<DAL.Entities.Cliente>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_getClientes", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        list.Add(new Entities.Cliente()
                        {
                            clienteID = reader.GetInt32(reader.GetOrdinal("clienteID")),
                            nombreCliente = reader.GetString(reader.GetOrdinal("nombreCliente")),
                            carritoID = reader.GetInt32(reader.GetOrdinal("carritoID"))
                        });
                    }
                }
            }
            return list;
        }

        // namespace wrapper
    }
}
