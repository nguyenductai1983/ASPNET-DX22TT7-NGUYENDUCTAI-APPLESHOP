using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppleShop.Models;
using System.Data.Entity;
namespace AppleShop.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;
        public HomeController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            // Lấy danh sách sản phẩm từ DB
            // Include("Category") để lấy luôn thông tin danh mục liên quan
            var upcomingProducts = _context.Products
                .Include(p => p.Category)
                .ToList();

            return View(upcomingProducts);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}