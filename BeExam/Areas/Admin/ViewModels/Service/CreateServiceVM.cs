using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeExam.Areas.Admin.ViewModels
{
    public class CreateServiceVM
    {
        [Required]
        [MinLength(4)]
        [MaxLength(25)]
        public string Name { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
