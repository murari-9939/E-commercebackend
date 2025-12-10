using System.Text.Json.Serialization;

namespace E_Commerce.Models
{
    public class Product
    {
        //public int Id { get; set; }
        public int Id { get; private set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Category { get; set; } = string.Empty;
    }
   
}
