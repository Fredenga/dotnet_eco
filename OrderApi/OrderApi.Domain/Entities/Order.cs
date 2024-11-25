namespace OrderApi.Domain.Entities
{
    public class Order
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public int ClientID { get; set; }
        public int PurchaseQuantity { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    }
}
