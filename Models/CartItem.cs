using System.ComponentModel.DataAnnotations;

namespace AppleShop.Models
{
    public class CartItem
    {
        // Dùng ProductId làm key để đơn giản
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        // Tính tổng tiền cho một sản phẩm
        public decimal Total
        {
            get { return Quantity * Price; }
        }

        // Constructor để dễ dàng tạo đối tượng từ Product
        public CartItem()
        {
        }

        public CartItem(Product product)
        {
            ProductId = product.Id;
            ProductName = product.Name;
            Price = product.Price;
            Quantity = 1; // Mặc định khi thêm vào giỏ là 1
            ImageUrl = product.ImageUrl;
        }
    }
}