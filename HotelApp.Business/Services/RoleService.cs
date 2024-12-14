using HotelApp.Business.Services.Base;
using HotelApp.Data.Models;
using HotelApp.Data.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace HotelApp.Business.Services;

public class RoleService(IUnitOfWork unitOfWork, ILogger<RoleService> logger) : 
    BaseService<Role>(unitOfWork, logger), IRoleService
{
}
