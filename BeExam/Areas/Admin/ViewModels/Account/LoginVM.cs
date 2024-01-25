namespace BeExam.Areas.Admin.ViewModels;

public class LoginVM
{
    public string UsernameOrEmail { get; set; }
    public string Password { get; set; }
    public bool IsRemembered { get; set; }
}
