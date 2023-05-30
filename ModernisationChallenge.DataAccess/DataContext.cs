using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace ModernisationChallenge.DataAccess;

public class DataContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options)
         : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        if (!optionsBuilder.IsConfigured)
        {
            // connect to sql server with connection string from app settings
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("ModernisationChallenge"));
        }
    }

    public virtual DbSet<Task> Tasks { get; set; }
}