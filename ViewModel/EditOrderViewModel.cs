using HandleHjelp.Data.Models;

namespace HandleHjelp.ViewModel
{
    public class EditOrderViewModel
    {
        public bool IsNew { get; set; } = true;

        public Order Order { get; set; } = new Order();

        public List<ProductType> ProductTypes { get; set; } = new List<ProductType>();

        public List<Country> Countries { get; set; }
    }
}
