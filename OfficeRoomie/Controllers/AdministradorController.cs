using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeRoomie.Helpers;
using OfficeRoomie.Repositories;
using OfficeRoomie.Models;
using OfficeRoomie.DTO;
using OfficeRoomie.ViewModels;

namespace OfficeRoomie.Controllers;

public class AdministradorController : Controller
{
    private readonly IAdministradorRepository _administradorRepository;
    private readonly ILogger<AdministradorController> _logger;

    public AdministradorController(IAdministradorRepository administradorRepository, ILogger<AdministradorController> logger)
    {
        _administradorRepository = administradorRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var administradores = await _administradorRepository.GetAdministradoresAsync();
        var viewModel = administradores.Select(u => new AdministradorViewModel
        {
            id = u.id,
            nome = u.nome,
            email = u.email,
            permissoes = u.permissoes,
        });

        return View(viewModel);
    }

    public async Task<IActionResult> Details(int id)
    {
        var administrador = await _administradorRepository.GetAdministradorByIdAsync(id);

        if (administrador == null)
        {
            _logger.LogWarning("Administrador com ID {Id} não encontrado.", id);
            return NotFound();
        }

        var viewModel = new AdministradorViewModel
        {
            id = administrador.id,
            nome = administrador.nome,
            email = administrador.email,
            permissoes = administrador.permissoes,
        };

        return View(viewModel);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(AdministradorDto dto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Modelo inválido ao criar administrador: {Errors}", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return View(dto);
        }

        var administrador = new Administrador
        {
            nome = dto.nome,
            email = dto.email,
            senha = PasswordHelper.HashPassword(dto.senha),
            permissoes = dto.permissoes,
        };

        await _administradorRepository.AddAdministradorAsync(administrador);
        _logger.LogInformation("Administrador {Nome} criado com sucesso.", dto.nome);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var administrador = await _administradorRepository.GetAdministradorByIdAsync(id);

        if (administrador == null)
        {
            _logger.LogWarning("Administrador com ID {Id} não encontrado para edição.", id);
            return NotFound();
        }

        var dto = new AdministradorDto
        {
            id = administrador.id,
            nome = administrador.nome,
            email = administrador.email,
            permissoes = administrador.permissoes,
        };

        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, AdministradorDto dto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Modelo inválido ao editar administrador: {Errors}", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return View(dto);
        }

        var administrador = new Administrador
        {
            id = dto.id,
            nome = dto.nome,
            email = dto.email,
            senha = dto.senha,
            permissoes = dto.permissoes,
        };

        var updatedAdministrador = await _administradorRepository.UpdateAdministradorAsync(id, administrador);

        if (updatedAdministrador == null)
        {
            _logger.LogWarning("Falha ao atualizar administrador com ID {Id}.", id);
            return NotFound();
        }

        _logger.LogInformation("Administrador com ID {Id} atualizado com sucesso.", id);
        return RedirectToAction(nameof(Index));
    }


    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var administrador = await _administradorRepository.DeleteAdministradorAsync(id);

        if (administrador == null)
        {
            _logger.LogWarning("Falha ao deletar administrador com ID {Id}.", id);
            return NotFound();
        }

        _logger.LogInformation("Administrador com ID {Id} deletado com sucesso.", id);
        return RedirectToAction(nameof(Index));
    }
}