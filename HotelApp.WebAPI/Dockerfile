# Use the official .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set working directory inside the container
WORKDIR /app

# Copy the solution file and restore dependencies
COPY *.sln ./
COPY HotelApp.WebAPI/HotelApp.WebAPI.csproj HotelApp.WebAPI/
COPY HotelApp.Business/HotelApp.Business.csproj HotelApp.Business/
COPY HotelApp.Data/HotelApp.Data.csproj HotelApp.Data/
COPY HotelApp.UnitTesting/HotelApp.UnitTesting.csproj HotelApp.UnitTesting/

# Restore dependencies
RUN dotnet restore

# Copy the entire project files
COPY . ./

# Build the application
RUN dotnet build HotelApp.WebAPI/HotelApp.WebAPI.csproj -c Release -o /out

# Publish the application
RUN dotnet publish HotelApp.WebAPI/HotelApp.WebAPI.csproj -c Release -o /publish

# Use the official runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# Set working directory for runtime
WORKDIR /publish

# Copy the published application files
COPY --from=build /publish .

# Expose the application port
EXPOSE 5134

# Run the application
ENTRYPOINT ["dotnet", "HotelApp.WebAPI.dll"]
