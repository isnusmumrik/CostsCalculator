using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CostsCalculator.Models
{
    public class Category
    {
        [Required]
        [HiddenInput(DisplayValue = false)]
        public int Id{ get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(6,MinimumLength = 6)]
        public string ColorForDiagram { get; set; }

        [Required]
        [HiddenInput(DisplayValue = false)]
        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Purchase> Purchases { get; set; }
    }
}