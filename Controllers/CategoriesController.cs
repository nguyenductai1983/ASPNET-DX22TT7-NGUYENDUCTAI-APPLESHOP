using AppleShop.Models;
using System.Linq;
using System.Web.Mvc;

namespace AppleShop.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        // GET: Categories
        public ActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }
    }
}