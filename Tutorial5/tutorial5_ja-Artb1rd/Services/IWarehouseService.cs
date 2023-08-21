using Zadanie5.DTOs;
using Zadanie5.Utils;

namespace Zadanie5.Services
{
    public interface IWarehouseService
    {
        public Task<int> AddProduct(ProductDTO product);
        public string getResponse(int requestStatus);
        public void Post(ProductDTO product);
        public Task<int> getResultId(ProductDTO product);
    }
}
