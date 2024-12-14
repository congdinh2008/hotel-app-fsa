using HotelApp.Business.Services.Base;
using HotelApp.Data.Models;
using HotelApp.Data.UnitOfWork;

namespace HotelApp.Business.Services;

public class RoleService(IUnitOfWork unitOfWork) : 
    BaseService<Role>(unitOfWork), IRoleService
{
}
