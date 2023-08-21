using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Zadanie5.DTOs
{
    public class ProductDTO
    {
        public int IdProduct { get; set; }
        public int IdWarehouse { get; set; }
        [Range(0, Int32.MaxValue, ErrorMessage = "Amount must be greater then 0")]
        public int Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}