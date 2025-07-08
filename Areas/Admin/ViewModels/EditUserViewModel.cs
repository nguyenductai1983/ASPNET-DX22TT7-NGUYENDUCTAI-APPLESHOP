using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AppleShop.Areas.Admin.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        // Danh sách tất cả các vai trò có trong hệ thống
        public IEnumerable<SelectListItem> AllRoles { get; set; }

        // Danh sách các vai trò mà người dùng này đang có
        public IList<string> UserRoles { get; set; }

        public EditUserViewModel()
        {
            UserRoles = new List<string>();
        }
    }
}