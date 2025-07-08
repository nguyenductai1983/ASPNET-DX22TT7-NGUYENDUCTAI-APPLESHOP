using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppleShop.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}