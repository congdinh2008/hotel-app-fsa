<!-- Add package to HotelApp.WebAPI -->
dotnet add HotelApp.WebAPI package Microsoft.EntityFrameworkCore
dotnet add HotelApp.WebAPI package Microsoft.EntityFrameworkCore.SqlServer
dotnet add HotelApp.WebAPI package Microsoft.EntityFrameworkCore.Design
dotnet add HotelApp.WebAPI package Microsoft.AspNetCore.Identity.EntityFrameworkCore


<!-- Add package to HotelApp.Data -->
dotnet add HotelApp.Data package Microsoft.EntityFrameworkCore
dotnet add HotelApp.Data package Microsoft.EntityFrameworkCore.SqlServer
dotnet add HotelApp.Data package Microsoft.AspNetCore.Identity.EntityFrameworkCore

<!-- Add package to HotelApp.Business -->
dotnet add HotelApp.Business package Microsoft.AspNetCore.Identity

<!-- Add package to HotelApp.UnitTesting -->
dotnet add HotelApp.UnitTesting package Moq