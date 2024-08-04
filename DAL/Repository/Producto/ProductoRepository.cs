using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.Producto
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly string _connectionString;
        public ProductoRepository(IOptions<Settings.DbConnection> connection)
        {
            _connectionString = connection.Value.ConnectionString;
        }


        public async Task<List<DAL.Entities.Producto>> GetAllProductos()
        {
            DAL.Entities.Producto? producto = null; // this helps avoid nulll reference exceptions! read about it

            //all of this verbosity can be replaced with Dapper, must read about it! it simplifies data inserts / mapping by a lot

            List<DAL.Entities.Producto> list = new List<DAL.Entities.Producto>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_getProductos", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        list.Add(new Entities.Producto()
                        {
                            productoID = reader.GetInt32(reader.GetOrdinal("productoID")),
                            nombreProducto = reader.GetString(reader.GetOrdinal("nombreProducto")),
                            precio = reader.GetDecimal(reader.GetOrdinal("precio"))
                        });
                    }
                }
            }
            return list;
        }


        public async Task<DAL.Entities.Producto> CreateProducto(string nombreProducto, decimal precio)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_createProducto", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@nombreProducto", SqlDbType.NVarChar, 100)).Value = nombreProducto;
                cmd.Parameters.Add(new SqlParameter("@precio", SqlDbType.Decimal)).Value = precio;

                await cmd.ExecuteNonQueryAsync();
            }
            return new DAL.Entities.Producto
            {
                nombreProducto = nombreProducto,
                precio = precio,
            };
        }
        public async Task<DAL.Entities.Producto> GetProductoByID(int productoID)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_getProductoByID", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@productoID", SqlDbType.Int)).Value = productoID;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new DAL.Entities.Producto
                            {
                                productoID = (int)reader["productoID"],
                                precio = (decimal)reader["precio"],
                                nombreProducto = (string)reader["nombreProducto"]
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
    }
}

