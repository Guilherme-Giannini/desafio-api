using DesafioApi.Domain.Models;

namespace DesafioApi.Application.Interfaces;

public interface IProdutoRepository
{
    Task<IEnumerable<Produto>> GetAllAsync();
    Task SeedAsync();
}
