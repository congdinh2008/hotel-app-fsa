namespace HotelApp.Business.ViewModels.User;

public class UserViewModel
{
    public Guid Id { get; set; }
    
    public required string Email { get; set; }
    
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }
    
    public required string DisplayName { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    public string? Avatar { get; set; }
    
    public bool IsActive { get; set; }
}
