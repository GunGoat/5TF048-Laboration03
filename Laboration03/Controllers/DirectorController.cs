using Laboration03.Application.Common.Interfaces;
using Laboration03.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Numerics;

namespace Laboration03.Controllers;

public class DirectorController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public DirectorController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        var directors = _unitOfWork.Directors.GetAll();
        return View(directors);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Director director)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _unitOfWork.Directors.Add(director);
                _unitOfWork.Commit(); 
                TempData["success"] = $"Successfully added director '{director.Name}'";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                TempData["error"] = $"Unable to add director '{director.Name}'. Error: {ex.Message}";
            }
        }
        else
        {
            TempData["error"] = $"Invalid data for director '{director.Name}'.";
        }
        return View(director);
    }


    public IActionResult Update(int id)
    {
        var director = _unitOfWork.Directors.GetById(id);
        if(director != null)
        {
            return View(director);
        }
        TempData["error"] = $"Unable to update director with id '{id}'.";
        return RedirectToAction(nameof(Index));
    }


    [HttpPost]
    public IActionResult Update(Director director)
    {
        if(director != null)
        {
            try
            {
                _unitOfWork.Directors.Update(director);
                _unitOfWork.Commit();
                TempData["success"] = $"Successfully updated director '{director.Name}'.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                TempData["error"] = $"Unable to update director '{director.Name}'. Error: {ex.Message}";
            }
        }
        else
        {
            TempData["error"] = $"Invalid data for director '{director?.Name}'.";
        }
        return View(director);
    }

    public IActionResult Delete(int id)
    {
        var director = _unitOfWork.Directors.GetById(id);
        if (director != null)
        {
            return View(director);
        }
        TempData["error"] = $"Unable to update director with id '{id}'.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Delete(Director director)
    {
        var directorFromDb = _unitOfWork.Directors.GetById(director.DirectorID);
        if (directorFromDb != null)
        {
            try
            {
                _unitOfWork.Directors.Delete(directorFromDb.DirectorID);
                _unitOfWork.Commit();
                TempData["success"] = $"Successfully deleted director '{directorFromDb.Name}'.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                TempData["error"] = $"Unable to delete director '{directorFromDb.Name}'. Error: {ex.Message}";
            }
        }
        else
        {
            TempData["error"] = $"Unable to update director with id '{director.DirectorID}'.";
        }
        return View(director);
    }
}
