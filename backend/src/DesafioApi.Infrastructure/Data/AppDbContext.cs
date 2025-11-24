using Microsoft.EntityFrameworkCore;
using DesafioApi.Domain.Models;

namespace DesafioApi.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Produto> Produtos { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Produto>().HasData(
            new Produto { Id = 1, Nome = "Caneta", Preco = 2.5M, Estoque = 100 },
            new Produto { Id = 2, Nome = "Caderno", Preco = 15.0M, Estoque = 50 }
        );
    }
}
