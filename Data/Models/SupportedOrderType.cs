namespace HandleHjelp.Data.Models
{
    public class SupportedOrderType : OrderType
    {
        public List<OrderTypeSubCategory> SubCategories { get; set; }
    }

    public class OrderTypeSubCategory
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
