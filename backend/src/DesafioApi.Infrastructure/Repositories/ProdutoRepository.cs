using Microsoft.EntityFrameworkCore;
using DesafioApi.Application.Interfaces;
using DesafioApi.Domain.Models;
using DesafioApi.Infrastructure.Data;

namespace DesafioApi.Infrastructure.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _ctx;
    public ProdutoRepository(AppDbContext ctx) => _ctx = ctx;
    public async Task<IEnumerable<Produto>> GetAllAsync() => await _ctx.Produtos.ToListAsync();
    public Task SeedAsync()
    {
        _ctx.Database.EnsureCreated();
        return Task.CompletedTask;
    }
}