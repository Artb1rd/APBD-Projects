using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Zadanie5.DTOs;
using Zadanie5.Utils;

namespace Zadanie5.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IConfiguration _configuration;

        public WarehouseService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> AddProduct(ProductDTO product)
        {
            using SqlConnection connection = initSqlConnection();
            var isProvidedDataExist = (int)isDataValid(product).Result;
            if (isProvidedDataExist != -1) return isProvidedDataExist;
            var isOrderExist = (int)this.isOrderExist(product).Result;
            if (isOrderExist != -1) return isOrderExist;
            return await Task.FromResult(1);
        }

        public async Task<RequestStatus> isDataValid(ProductDTO product)
        {
            var connection = new SqlConnection("Data Source=MSI;Initial Catalog=WarehouseDB;Integrated Security=True");
            await using var productCom =
                new SqlCommand("SELECT COUNT(*) AS 'X' FROM Product WHERE IdProduct=@IdProduct", connection);
            await using var wholesalerCom =
                new SqlCommand("SELECT COUNT(*) AS 'X' FROM Warehouse WHERE IdWarehouse=@IdWarehouse", connection);
            productCom.Parameters.AddWithValue("@IdProduct", product.IdProduct);
            wholesalerCom.Parameters.AddWithValue("@IdWarehouse", product.IdWarehouse);
            await connection.OpenAsync();
            using (var reader = await productCom.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (reader["X"].ToString() == "0") return RequestStatus.ERROR_PRODUCT_DOESNT_EXIST;
                }
            }

            using (var reader = await wholesalerCom.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (reader["X"].ToString() == "0") return RequestStatus.ERROR_WHOLESALE_DOESNT_EXISTS;
                }
            }

            return RequestStatus.SUCCESS;
        }

        public async void updateOrder(int orderId)
        {
            var connection = new SqlConnection("Data Source=MSI;Initial Catalog=WarehouseDB;Integrated Security=True");
            await using var orderCom =
                new SqlCommand("UPDATE [Order] SET FulfilledAt = @CurTime WHERE IdOrder = @IdOrder", connection);
            orderCom.Parameters.AddWithValue("@CurTime", DateTime.Now.ToString());
            orderCom.Parameters.AddWithValue("@IdOrder", orderId);
            await connection.OpenAsync();
            await orderCom.ExecuteNonQueryAsync();
        }

        public async Task<int> isOrderExist(ProductDTO product)
        {
            var connection = new SqlConnection("Data Source=MSI;Initial Catalog=WarehouseDB;Integrated Security=True");
            OrderDTO? order = null;
            ProductWarehouse? productWarehouse = null;
            await using var orderCom =
                new SqlCommand("SELECT * FROM [Order] WHERE IdProduct=@IdProduct AND Amount=@Amount", connection);
            await using var warehouseProductCom =
                new SqlCommand("SELECT Count(*) AS 'X' FROM Product_Warehouse WHERE IdOrder=@IdOrder", connection);
            await using var productOrderCom =
                new SqlCommand(
                    "SELECT * FROM Product INNER JOIN [Order] ON [Order].IdProduct = Product.IdProduct WHERE IdOrder=@IdOrder",
                    connection);
            await using var insertCom =
                new SqlCommand(
                    "INSERT INTO Product_Warehouse VALUES(@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @CreatedAt)",
                    connection);
            await using var finalResultCom =
                new SqlCommand("SELECT IdProductWarehouse FROM Product_Warehouse WHERE IdOrder=@IdOrder", connection);
            orderCom.Parameters.AddWithValue("@IdProduct", product.IdProduct);
            orderCom.Parameters.AddWithValue("@Amount", product.Amount);
            await connection.OpenAsync();
            using (var reader = await orderCom.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (reader.HasRows)
                    {
                        try
                        {
                            order = new OrderDTO();
                            order.IdProduct = Int32.Parse(reader["IdProduct"].ToString());
                            order.IdOrder = Int32.Parse(reader["IdOrder"].ToString());
                            order.Amount = Int32.Parse(reader["Amount"].ToString());
                            order.CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString());
                            var fulfTime = reader["FulfilledAt"].ToString();
                            if (fulfTime == "")
                                order.FulfilledAt = null;
                            else
                                order.FulfilledAt = DateTime.Parse(reader["FulfilledAt"].ToString());
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                    }
                }
            }

            if (order == null)
                return (int)RequestStatus.ERROR_ORDER_DOESNT_EXIST;
            if (convertDateTimeToMillis(product.CreatedAt) < convertDateTimeToMillis(order.CreatedAt))
                return (int)RequestStatus.ERROR_CREATED_AT_PARAMETER_GREATER_THEN_NATIVE;

            warehouseProductCom.Parameters.AddWithValue("@IdOrder", order.IdOrder);
            using (var reader = await warehouseProductCom.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (reader["X"].ToString() != "0") return (int)RequestStatus.ERROR_ORDER_ALREADY_DONE;
                }
            }

            updateOrder(order.IdOrder);
            productOrderCom.Parameters.AddWithValue("@IdOrder", order.IdOrder);
            using (var reader = await productOrderCom.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (reader.HasRows)
                    {
                        productWarehouse = new ProductWarehouse();
                        productWarehouse.IdOrder = Int32.Parse(reader["IdOrder"].ToString());
                        productWarehouse.Price = Double.Parse(reader["Price"].ToString());
                    }
                }
            }

            insertCom.Parameters.AddWithValue("@IdWarehouse", product.IdWarehouse);
            insertCom.Parameters.AddWithValue("@IdProduct", product.IdProduct);
            insertCom.Parameters.AddWithValue("@IdOrder", order.IdOrder);
            insertCom.Parameters.AddWithValue("@Price", productWarehouse.Price * product.Amount);
            insertCom.Parameters.AddWithValue("@Amount", order.Amount);
            insertCom.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
            insertCom.ExecuteNonQuery();
            finalResultCom.Parameters.AddWithValue("@IdOrder", order.IdOrder);
            using (var reader = await finalResultCom.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    return Int32.Parse(reader["IdProductWarehouse"].ToString());
                }
            }

            return (int)RequestStatus.SUCCESS;
        }

        double convertDateTimeToMillis(DateTime time)
        {
            return time.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            ).TotalMilliseconds;
        }

        SqlConnection initSqlConnection()
        {
            var connectionString =
                _configuration.GetConnectionString(
                    "Data Source=MSI;Initial Catalog=WarehouseDB;Integrated Security=True");
            return new SqlConnection(connectionString);
        }

        public async void Post(ProductDTO product)
        {
            await using var connection =
                new SqlConnection("Data Source=MSI;Initial Catalog=WarehouseDB;Integrated Security=True");
            await using var com = new SqlCommand("AddProductToWarehouse", connection);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@IdProduct", product.IdProduct);
            com.Parameters.AddWithValue("@IdWarehouse", product.IdWarehouse);
            com.Parameters.AddWithValue("@Amount", product.Amount);
            com.Parameters.AddWithValue("@CreatedAt", product.CreatedAt);
            await connection.OpenAsync();
            await com.ExecuteNonQueryAsync();
        }
    
        public async Task<int> getResultId(ProductDTO product)
        {
            using (SqlConnection con = new SqlConnection("Data Source=MSI;Initial Catalog=WarehouseDB;Integrated Security=True"))
            {
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                con.Open();
                com.CommandText = "SELECT IdProductWarehouse from Product_Warehouse Where IdProduct = @IdProduct and IdWarehouse = @IdWarehouse and Amount = @Amount and Price = ((SELECT Price FROM Product WHERE IdProduct = @IdProduct) * @Amount)";
                com.Parameters.AddWithValue("Amount", product.Amount);
                com.Parameters.AddWithValue("IdWarehouse", product.IdWarehouse);
                com.Parameters.AddWithValue("IdProduct", product.IdProduct);
                return ((int)com.ExecuteScalar());
            }
        }
        public string getResponse(int requestStatus)
        {
            switch (requestStatus)
            {
                case -2:
                    return "Product doesn't exist";
                case -3:
                    return "Wholesale doesn't exist";
                case -4:
                    return "Amount cannot be less then zero";
                case -5:
                    return "Order doesn't exist";
                case -6:
                    return "CreatedAt should be bigger then native parameter";
                case -7:
                    return "Order have already been done";
            }

            return "Success";
        }
    }
}