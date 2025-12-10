using System.Text.Json.Serialization;

namespace E_Commerce.Models
{
    public class Order
    {
        //[JsonIgnore]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        //public Customer Customer { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        [JsonIgnore]
        public decimal TotalAmount { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new();
    }
}
