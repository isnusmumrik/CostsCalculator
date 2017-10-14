using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace CostsCalculator.Models
{
    public class Purchase
    {
        [Required]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Amount { get; set; }

        [Required]
        [StringLength(50)]
        public  string Unit { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public  double UnitCost { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public  DateTime Date { get; set; }

        [Required]
        [HiddenInput(DisplayValue = false)]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        [HiddenInput(DisplayValue = false)]
        public int CategoryId { get; set; }
        public virtual  Category Category { get; set; }
    }
}