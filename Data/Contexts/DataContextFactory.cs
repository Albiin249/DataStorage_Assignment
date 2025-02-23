using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Projects\\DataStorage_Assignment\\Data\\Databases\\local_db_API.mdf;Integrated Security=True");
        //För ConsoleApp använd connectionstring "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Projects\\DataStorage_Assignment\\Data\\Databases\\local_database.mdf;Integrated Security=True;Connect Timeout=30"
        //För Reactprojektet/APIt använd connectionstring "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Projects\\DataStorage_Assignment\\Data\\Databases\\local_db_API.mdf;Integrated Security=True"
        return new DataContext(optionsBuilder.Options);
    }
}
