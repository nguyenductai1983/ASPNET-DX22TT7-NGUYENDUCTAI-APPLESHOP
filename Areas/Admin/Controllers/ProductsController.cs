using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AppleShop.Models;
using System.IO;
using PagedList; // Thêm using
using System.Text; // Thêm using
using Rotativa; // Thêm using

namespace AppleShop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // CẬP NHẬT ACTION INDEX ĐỂ THÊM TÌM KIẾM VÀ PHÂN TRANG
        // GET: Admin/Products
        public ActionResult Index(string searchString, int? page)
        {
            var products = db.Products.Include(p => p.Category);

            // Tìm kiếm theo tên sản phẩm
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Name.Contains(searchString));
            }

            products = products.OrderBy(p => p.Name);
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            ViewBag.CurrentSearch = searchString; // Lưu lại từ khóa tìm kiếm

            return View(products.ToPagedList(pageNumber, pageSize));
        }

        // THÊM ACTION XUẤT FILE CSV
        public FileResult ExportToCsv()
        {
            var products = db.Products.Include(p => p.Category).ToList();
            var sb = new StringBuilder();
            sb.AppendLine("Id,TenSanPham,MoTa,Gia,DanhMuc,NoiBat");

            foreach (var item in products)
            {
                string name = SanitizeCsvField(item.Name);
                string description = SanitizeCsvField(item.Description);
                sb.AppendLine($"{item.Id},{name},{description},{item.Price},{item.Category.Name},{item.IsFeatured}");
            }

            byte[] fileBytes = new UTF8Encoding(true).GetBytes(sb.ToString());
            return File(fileBytes, "text/csv", "DanhSachSanPham.csv");
        }

        // THÊM ACTION XUẤT FILE PDF
        public ActionResult ExportToPdf()
        {
            var products = db.Products.Include(p => p.Category).ToList();
            return new ViewAsPdf("ProductsPdf", products)
            {
                FileName = "DanhSachSanPham.pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                CustomSwitches = "--encoding utf-8"
            };
        }

        private string SanitizeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field)) return "";
            if (field.Contains(",")) return $"\"{field}\"";
            return field;
        }


        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Include(p => p.Category).SingleOrDefault(p => p.Id == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: Admin/Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Price,IsFeatured,CategoryId")] Product product, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Images/Products"), fileName);
                    var folder = Server.MapPath("~/Content/Images/Products");
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    ImageFile.SaveAs(path);
                    product.ImageUrl = "/Content/Images/Products/" + fileName;
                }
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Price,ImageUrl,IsFeatured,CategoryId")] Product product, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Images/Products"), fileName);
                    ImageFile.SaveAs(path);
                    product.ImageUrl = "/Content/Images/Products/" + fileName;
                }
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
