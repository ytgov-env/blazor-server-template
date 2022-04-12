using Microsoft.EntityFrameworkCore;

namespace BlazorServerTemplate.Data;

public class AppDbContext : DbContext
{
    public DbSet<ExampleEntity> ExampleEntities => Set<ExampleEntity>();
}
