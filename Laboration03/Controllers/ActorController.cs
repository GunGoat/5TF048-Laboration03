using Laboration03.Application.Common.Interfaces;
using Laboration03.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Laboration03.Controllers;

public class ActorController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public ActorController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Create()
    {
        return View();
    }

    public IActionResult Create(Actor actor)
    {
        return View();
    }

    public IActionResult Update(int id)
    {
        return View();
    }

    [HttpPost]
    public IActionResult Update(Actor actor)
    {
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int id)
    {
        return View();
    }

    [HttpPost]
    public IActionResult Delete(Actor actor)
    {
        return RedirectToAction(nameof(Index));
    }
}
