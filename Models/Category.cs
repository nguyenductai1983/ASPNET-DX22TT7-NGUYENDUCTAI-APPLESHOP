﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppleShop.Models
{
    public class Category
    {
        public int Id { get; set; }


        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}