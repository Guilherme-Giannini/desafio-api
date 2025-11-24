using DesafioApi.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace DesafioApi.Api.Controllers;

public class HomeController : Controller
{
    private readonly IProdutoRepository _repo;
    private readonly ILockService _lockService;

    public HomeController(IProdutoRepository repo, ILockService lockService)
    {
        _repo = repo;
        _lockService = lockService;
    }

    public async Task<IActionResult> Index()
    {
        var produtos = await _repo.GetAllAsync();

        if (produtos == null || !produtos.Any())
            ViewBag.Mensagem = "Nenhum produto encontrado.";

        return View(produtos);
    }

    [HttpPost("/processar-relatorio")]
    public async Task<IActionResult> ProcessarRelatorio()
    {
        var key = "processar-relatorio-lock";

        if (!await _lockService.TryAcquireLockAsync(key, TimeSpan.FromSeconds(10)))
            return StatusCode(423, new { message = "Recurso ocupado" });

        try
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            return Ok(new { message = "Processo executado" });
        }
        finally
        {
            await _lockService.ReleaseLockAsync(key);
        }
    }
}