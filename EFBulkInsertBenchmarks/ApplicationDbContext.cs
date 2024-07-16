using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFBulkInsertBenchmarks;

public class ApplicationDbContext:DbContext
{
    public const string CONNECTION_STRING = "Data Source=.;Initial Catalog=EFBulkInsertBenchmarks;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
    public ApplicationDbContext()
    {
            
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(CONNECTION_STRING);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<User>().HasKey(x=>x.Id);
      modelBuilder.Entity<User>().Property(x=>x.Id).ValueGeneratedOnAdd();
    }
    public DbSet<User> Users { get; set; }
}
