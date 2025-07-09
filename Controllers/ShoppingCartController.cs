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
        
        [HttpPost]
        public ActionResult AddToCart(int productId, int quantity = 1)
        {
            List<CartItem> cart = Session["Cart"] as List<CartItem>;
            if (cart == null)
            {
                cart = new List<CartItem>();
            }

            CartItem item = cart.FirstOrDefault(c => c.ProductId == productId);
            if (item == null)
            {
                var productToAdd = _context.Products.Find(productId);
                if (productToAdd != null)
                {
                    var newItem = new CartItem(productToAdd);
                    newItem.Quantity = quantity; // Gán số lượng mới
                    cart.Add(newItem);
                }
            }
            else
            {
                item.Quantity += quantity; // Cộng thêm số lượng
            }

            Session["Cart"] = cart;
            return RedirectToAction("Index");
        }
        // POST: ShoppingCart/UpdateCart
        [HttpPost]
        public ActionResult UpdateCart(int productId, int quantity)
        {
            List<CartItem> cart = Session["Cart"] as List<CartItem>;
            CartItem itemToUpdate = cart.FirstOrDefault(i => i.ProductId == productId);

            if (itemToUpdate != null)
            {
                if (quantity > 0)
                {
                    itemToUpdate.Quantity = quantity;
                }
                else
                {
                    cart.Remove(itemToUpdate);
                }
            }
            Session["Cart"] = cart;

            // Tính toán các giá trị mới để trả về cho client
            decimal itemTotal = itemToUpdate?.Total ?? 0;
            decimal grandTotal = cart.Sum(i => i.Total);
            int cartCount = cart.Sum(i => i.Quantity);

            return Json(new
            {
                success = true,
                itemTotal = itemTotal.ToString("N0"),
                grandTotal = grandTotal.ToString("N0"),
                cartCount = cartCount
            });
        }
        // GET: ShoppingCart/RemoveFromCart
        public ActionResult RemoveFromCart(int productId)
        {
            List<CartItem> cart = Session["Cart"] as List<CartItem>;
            CartItem itemToRemove = cart.FirstOrDefault(i => i.ProductId == productId);

            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
            }
            Session["Cart"] = cart;

            return RedirectToAction("Index");
        }

        // Partial View cho summary giỏ hàng trên navbar
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            List<CartItem> cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();
            ViewBag.CartCount = cart.Sum(c => c.Quantity); // Đếm tổng số lượng sản phẩm
            return PartialView("_CartSummary");
        }
    }
}