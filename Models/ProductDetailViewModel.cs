using System.Collections.Generic;

namespace AppleShop.Models
{
    public class ProductDetailViewModel
    {
        public Models.Product Product { get; set; }
        public List<Models.Product> RelatedProducts { get; set; }
    }
}