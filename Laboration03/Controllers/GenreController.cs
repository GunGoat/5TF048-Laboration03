using Laboration03.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Laboration03.Controllers
{
    public class GenreController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        // Constructor for injecting IUnitOfWork
        public GenreController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var genres = _unitOfWork.Genres.GetAll();
            return View(genres);
        }
    }
}
