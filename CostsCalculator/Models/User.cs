using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace CostsCalculator.Models
{
    public class User
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Incorrect format of email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public ICollection<Category> Categories { get; set; }

        public ICollection<Purchase> Purchases { get; set; }
    }
}