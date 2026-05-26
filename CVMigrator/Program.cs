using CVMigrator.Data;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

string connectionString =
    configuration.GetConnectionString("DefaultConnection")!;

var sqlHelper = new SqlClientHelper(connectionString);