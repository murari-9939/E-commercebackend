using System.Text.Json.Serialization;

namespace E_Commerce.Models
{
    public class OrderItem
    {
        //OrderItem(Id, OrderId, ProductId, Quantity, UnitPrice)
        public int Id { get; set; }
        public int OrderId { get; set; }
        //public Order Order { get; set; } = null!;
        public int ProductId { get; set; }
        [JsonIgnore]
        public int Quantity { get; set; }
        [JsonIgnore]
        public decimal UnitPrice { get; set; }
    }
}
