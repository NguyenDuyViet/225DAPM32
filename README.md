# 225DAPM32
Dich Vu Mon Ngon Tai Nha

# Connect MySQL
"ConnectionStrings": {
    "MySqlConnection": "server=localhost;database=monngontainha;user=root;password=08052004;"
  },
# Connect SQLServer
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=MyDatabase;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
  }
    Add package: dotnet add package Microsoft.EntityFrameworkCore.SqlServer
                dotnet add package Microsoft.EntityFrameworkCore.Tools
    Change Program.cs: gỡ comment của DBConttext SQLServer

