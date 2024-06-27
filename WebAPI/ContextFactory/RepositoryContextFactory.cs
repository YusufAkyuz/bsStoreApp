using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repositories.EFCore;

namespace WebAPI.ContextFactory;

public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
{
    public RepositoryContext CreateDbContext(string[] args)
    {
        //Configuration oluşturacaz bu sayede 
        var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json").Build();


        //DbContextOprtionsBuilder yazıcaz
        var builder =
            new DbContextOptionsBuilder<RepositoryContext>().UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                prj => prj.MigrationsAssembly("WebAPI"));

        return new RepositoryContext(builder.Options);
    }
}