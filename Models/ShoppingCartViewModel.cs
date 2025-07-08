using System.Collections.Generic;
using System.Linq;

namespace AppleShop.Models
{
    public class ShoppingCartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public decimal GrandTotal { get; set; }
    }
}