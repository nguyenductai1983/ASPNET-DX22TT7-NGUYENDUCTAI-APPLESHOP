using System.ComponentModel.DataAnnotations;

namespace AppleShop.Models 
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [Display(Name = "Họ và Tên người nhận")]
        public string ShipName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        [Display(Name = "Địa chỉ giao hàng")]
        public string ShipAddress { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Display(Name = "Số điện thoại")]
        public string ShipPhoneNumber { get; set; }
    }
}