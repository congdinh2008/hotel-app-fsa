namespace HotelApp.Business.ViewModels.Role;

public class RoleViewModel
{
    public Guid Id { get; set; }

    public required string Name { get; set; }
   
    public required string Description { get; set; }

    public bool IsActive { get; set; }
}