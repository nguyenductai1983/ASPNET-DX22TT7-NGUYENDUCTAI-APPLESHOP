using AppleShop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AppleShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        // GET: ShoppingCart
        public ActionResult Index()
        {
            // Lấy giỏ hàng từ Session
            List<CartItem> cart = Session["Cart"] as List<CartItem>;
            if (cart == null)
            {
                // Nếu chưa có giỏ hàng, tạo mới
                cart = new List<CartItem>();
                Session["Cart"] = cart;
            }

            // Tính tổng tiền
            decimal grandTotal = cart.Sum(item => item.Total);

            // Tạo ViewModel
            var shoppingCartVM = new ShoppingCartViewModel
            {
                CartItems = cart,
                GrandTotal = grandTotal
            };

            return View(shoppingCartVM);
        }

        // Action thêm sản phẩm vào giỏ hàng
        public ActionResult AddToCart(int productId)
        {
            List<CartItem> cart = Session["Cart"] as List<CartItem>;
            if (cart == null)
            {
                cart = new List<CartItem>();
            }

            // Kiểm tra sản phẩm đã có trong giỏ chưa
            CartItem item = cart.FirstOrDefault(c => c.ProductId == productId);
            if (item == null)
            {
                // Nếu chưa có, tìm sản phẩm trong DB và thêm vào giỏ
                var productToAdd = _context.Products.Find(productId);
                if (productToAdd != null)
                {
                    cart.Add(new CartItem(productToAdd));
                }
            }
            else
            {
                // Nếu đã có, tăng số lượng lên 1
                item.Quantity++;
            }

            // Lưu lại giỏ hàng vào Session
            Session["Cart"] = cart;

            // Chuyển hướng về trang giỏ hàng
            return RedirectToAction("Index");
        }
    }
}