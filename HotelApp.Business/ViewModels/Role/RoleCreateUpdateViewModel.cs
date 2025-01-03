namespace HotelApp.Business.ViewModels.Role;

public class RoleCreateUpdateViewModel
{
    public required string Name { get; set; }

    public required string Description { get; set; }

    public bool IsActive { get; set; }
}
