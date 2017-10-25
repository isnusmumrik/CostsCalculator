using System.ComponentModel.DataAnnotations;

namespace CostsCalculator.Models.ServieceModels
{
    public class LogModel
    {
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }
        [Required]
        [StringLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}