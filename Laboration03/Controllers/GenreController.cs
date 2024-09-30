using Laboration03.Application.Common.Interfaces;
using Laboration03.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Laboration03.Controllers;

public class GenreController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public GenreController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        var genres = _unitOfWork.Genres.GetAll();
        return View(genres);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Genre genre)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _unitOfWork.Genres.Add(genre);
                _unitOfWork.Commit();
                TempData["success"] = $"Successfully added genre '{genre.GenreName}'";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                TempData["error"] = $"Unable to add genre '{genre.GenreName}'. Error: {ex.Message}";
            }
        }
        else
        {
            TempData["error"] = $"Invalid data for genre '{genre.GenreName}'";
        }
        return View(genre);
    }

    public IActionResult Update(int id)
    {
        var genre = _unitOfWork.Genres.GetById(id);
        if (genre != null)
        {
            return View(genre);
        }
        TempData["error"] = $"Unable to find genre with id '{id}'.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Update(Genre genre)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _unitOfWork.Genres.Update(genre);
                _unitOfWork.Commit();
                TempData["success"] = $"Successfully updated genre '{genre.GenreName}'";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                TempData["error"] = $"Unable to update genre '{genre.GenreName}'. Error: {ex.Message}";
            }
        }
        else
        {
            TempData["error"] = $"Invalid data for genre '{genre.GenreName}'";
        }
        return View(genre);
    }

    public IActionResult Delete(int id)
    {
        var genre = _unitOfWork.Genres.GetById(id);
        if (genre != null)
        {
            return View(genre);
        }
        TempData["error"] = $"Unable to find genre with id '{id}'.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Delete(Genre genre)
    {
        var genreFromDb = _unitOfWork.Genres.GetById(genre.GenreID);
        if (genreFromDb != null)
        {
            try
            {
                _unitOfWork.Genres.Delete(genreFromDb.GenreID);
                _unitOfWork.Commit();
                TempData["success"] = $"Successfully deleted genre '{genreFromDb.GenreName}'";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                TempData["error"] = $"Unable to delete genre '{genreFromDb.GenreName}'. Error: {ex.Message}";
            }
        }
        else
        {
            TempData["error"] = $"Unable to find genre with id '{genre.GenreID}'";
        }
        return View(genre);
    }
}
