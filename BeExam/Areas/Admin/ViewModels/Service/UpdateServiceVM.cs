namespace BeExam.Areas.Admin.ViewModels
{
    public class UpdateServiceVM
    {
        public string Name { get; set; }
        public string? Image { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
