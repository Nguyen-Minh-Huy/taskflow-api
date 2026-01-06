using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TaskFlow.Infrastructure;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TaskFlowDbContext>
{
    public TaskFlowDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TaskFlowDbContext>();

        optionsBuilder.UseSqlServer("Server=MSI_HUY;Database=TaskFlowDb;User Id=huynguyen;Password=11052003;TrustServerCertificate=True;");

        return new TaskFlowDbContext(optionsBuilder.Options);
    }
}
