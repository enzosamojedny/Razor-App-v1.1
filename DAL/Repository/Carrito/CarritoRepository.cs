using DAL.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.Carrito
{
    // to fix siem shit
    public class CarritoRepository : ICarritoRepository
    {
        private readonly string _connectionString;
        public CarritoRepository(IOptions<Settings.DbConnection> connection)
        {
            _connectionString = connection.Value.ConnectionString;
        }
        public async Task<DAL.Entities.Carrito> GetCarritoByID(int carritoID)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_getCarritoByID", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@carritoID", SqlDbType.Int)).Value = carritoID;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new DAL.Entities.Carrito
                            {
                                carritoID = (int)reader["carritoID"]
                            };
                        }
                        else
                        {
                            throw new Exception("Carrito not found");
                        }
                    }
                }
            }
        }

        public async Task<DAL.Entities.Carrito> CreateCarrito(int productoID)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_createCarrito", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@productoID", SqlDbType.Int)).Value = productoID;

                await cmd.ExecuteNonQueryAsync();
            }
            return new DAL.Entities.Carrito
            {
                productoID = productoID,
            };
        }

        public async Task<List<DAL.Entities.Carrito>> GetAllCarritos()
        {
            DAL.Entities.Carrito? cliente = null;

            List<DAL.Entities.Carrito> list = new List<DAL.Entities.Carrito>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_getCarritos", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        list.Add(new Entities.Carrito()
                        {
                            carritoID = reader.GetInt32(reader.GetOrdinal("carritoID")),
                            productoID = reader.GetInt32(reader.GetOrdinal("productoID")),
                        });
                    }
                }
            }
            return list;
        }
    }
}
        

