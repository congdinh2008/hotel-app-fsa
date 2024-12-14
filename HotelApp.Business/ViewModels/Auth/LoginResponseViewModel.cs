namespace HotelApp.Business.ViewModels.Auth;

public class LoginResponseViewModel
{
    public required string UserInformation { get; set; }

    public required string Token { get; set; }

    public DateTime Expires { get; set; }
}
