using Laboration03.Application.Common.Interfaces;
using Laboration03.Domain.Entities;
using Laboration03.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Laboration03.Controllers;

public class MovieController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public MovieController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult Index(string titleSearch = null, string sortColumn = "Title", string sortOrder = "ASC")
    {
        var movies = _unitOfWork.Movies.GetMoviesWithDetails(titleSearch: titleSearch, sortColumn: sortColumn, sortOrder:sortOrder);
        return View(movies);
    }

    public IActionResult Create()
    {
        var model = new MovieVM
        {
            AvailableActors = _unitOfWork.Actors.GetAll(), 
            AvailableGenres = _unitOfWork.Genres.GetAll(),
            AvailableDirectors = _unitOfWork.Directors.GetAll()
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult Create(MovieVM movieVM)
    {
        if (ModelState.IsValid)
        {
            try
            {
                if (movieVM.Preview != null)
                {
                    // Validate that the uploaded file is an movie
                    if (!IsVideoFile(movieVM.Preview))
                    {
                        ModelState.AddModelError("Profile", "Only video files (mp4, avi, mov, wmv) are allowed.");
                        return View(movieVM);
                    }

                    movieVM.PreviewUrl = SavePreviewVideo(movieVM.Preview);
                }

                Movie movie = new Movie
                {
                    Title = movieVM.Title,
                    Description = movieVM.Description,
                    ReleaseDate = movieVM.ReleaseDate,
                    Rating = movieVM.Rating,
                    Duration = movieVM.Duration,
                    DirectorID = movieVM.DirectorID,
                    PreviewUrl = movieVM.PreviewUrl,
                };

                movie.MovieID = _unitOfWork.Movies.Add(movie);
                _unitOfWork.Movies.UpdateMovieActors(movie.MovieID, movieVM.SelectedActorIDs.ToArray());
                _unitOfWork.Movies.UpdateMovieGenres(movie.MovieID, movieVM.SelectedGenreIDs.ToArray());
                _unitOfWork.Commit();
                TempData["success"] = $"Successfully added movie '{movieVM.Title}'";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                TempData["error"] = $"Unable to add movie '{movieVM.Title}'. Error: {ex.Message}";
            }
        }
        else
        {
            TempData["error"] = $"Invalid data for movie '{movieVM.Title}'.";
        }
        return View(movieVM);
    }

    public IActionResult Update(int id)
    {
        var movieFromDb = _unitOfWork.Movies
            .GetMoviesWithDetails(movieIds: id)
            .FirstOrDefault();
        if (movieFromDb != null)
        {
            var movieVM = new MovieVM
            {
                MovieID = movieFromDb.MovieID,
                Title = movieFromDb.Title,
                ReleaseDate = movieFromDb.ReleaseDate,
                Rating = movieFromDb.Rating,
                Duration = movieFromDb.Duration,
                DirectorID = movieFromDb.DirectorID,
                PreviewUrl = movieFromDb.PreviewUrl,
                Description = movieFromDb.Description,
                AvailableActors = _unitOfWork.Actors.GetAll(),
                SelectedActorIDs = movieFromDb.Actors?.Select(a => a.ActorID).ToList() ?? new List<int>(),
                AvailableGenres = _unitOfWork.Genres.GetAll(),
                SelectedGenreIDs = movieFromDb?.Genres?.Select(g => g.GenreID).ToList() ?? new List<int>(),
                AvailableDirectors = _unitOfWork.Directors.GetAll(),
            };
            return View(movieVM);
        }
        TempData["error"] = $"Unable to find movie with id '{id}'.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Update(MovieVM movieVM)
    {
        if (ModelState.IsValid)
        {
            try
            {
                if (movieVM.Preview != null)
                {
                    // Delete the old video if it exists
                    if (!string.IsNullOrEmpty(movieVM.PreviewUrl))
                    {
                        // Validate that the uploaded file is a video
                        if (!IsVideoFile(movieVM.Preview))
                        {
                            ModelState.AddModelError("Profile", "Only video files (mp4, avi, mov, wmv) are allowed.");
                            return View(movieVM);
                        }

                        DeletePreviewVideo(movieVM.PreviewUrl);
                    }

                    movieVM.PreviewUrl = SavePreviewVideo(movieVM.Preview);
                }

                Movie movie = new Movie
                {
                    MovieID = movieVM.MovieID,
                    Title = movieVM.Title,
                    Description = movieVM.Description,
                    ReleaseDate = movieVM.ReleaseDate,
                    Rating = movieVM.Rating,
                    Duration = movieVM.Duration,
                    DirectorID = movieVM.DirectorID,
                    PreviewUrl = movieVM.PreviewUrl,
                };

                _unitOfWork.Movies.Update(movie);
                _unitOfWork.Movies.UpdateMovieActors(movie.MovieID, movieVM.SelectedActorIDs.ToArray());
                _unitOfWork.Movies.UpdateMovieGenres(movie.MovieID, movieVM.SelectedGenreIDs.ToArray());
                _unitOfWork.Commit();
                TempData["success"] = $"Successfully added movie '{movieVM.Title}'";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                TempData["error"] = $"Unable to add movie '{movieVM.Title}'. Error: {ex.Message}";
            }
        }
        else
        {
            TempData["error"] = $"Invalid data for movie '{movieVM.Title}'.";
        }
        return View(movieVM);
    }

    public IActionResult Delete(int id)
    {
        var movieFromDb = _unitOfWork.Movies
            .GetMoviesWithDetails(movieIds: id)
            .FirstOrDefault();
        if (movieFromDb != null)
        {
            var movieVM = new MovieVM
            {
                MovieID = movieFromDb.MovieID,
                Title = movieFromDb.Title,
                ReleaseDate = movieFromDb.ReleaseDate,
                Rating = movieFromDb.Rating,
                Duration = movieFromDb.Duration,
                DirectorID = movieFromDb.DirectorID,
                PreviewUrl = movieFromDb.PreviewUrl,
                Description = movieFromDb.Description,
                AvailableActors = movieFromDb.Actors,
                SelectedActorIDs = movieFromDb.Actors?.Select(a => a.ActorID).ToList() ?? new List<int>(),
                AvailableGenres = movieFromDb.Genres,
                SelectedGenreIDs = movieFromDb?.Genres?.Select(g => g.GenreID).ToList() ?? new List<int>(),
                AvailableDirectors = _unitOfWork.Directors.GetAll(),
            };
            return View(movieVM);
        }
        TempData["error"] = $"Unable to find movie with id '{id}'.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Delete(MovieVM movieVM)
    {
        var movieFromDb = _unitOfWork.Movies.GetById(movieVM.MovieID);
        if (movieFromDb != null)
        {
            try
            {
                // Delete the preview video if it exists
                if (!string.IsNullOrEmpty(movieFromDb.PreviewUrl))
                {

                }

                _unitOfWork.Movies.Delete(movieFromDb.MovieID);
                _unitOfWork.Commit();
                TempData["success"] = $"Successfully deleted movie '{movieFromDb.Title}'.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                TempData["error"] = $"Unable to delete movie '{movieFromDb.Title}'. Error: {ex.Message}";
            }
        }
        else
        {
            TempData["error"] = $"Unable to delete movie with id '{movieVM.Title}'.";
        }
        return View(movieVM);
    }

    // Helper function to validate if the uploaded file is a video
    private bool IsVideoFile(IFormFile file)
    {
        var allowedExtensions = new[] { ".mp4", ".avi", ".mov", ".wmv" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return allowedExtensions.Contains(extension);
    }

    // Helper function to save the preview video
    private string SavePreviewVideo(IFormFile previewVideo)
    {
        // Generate a unique file name
        string uniqueFileName = Guid.NewGuid().ToString() + "_" + previewVideo.FileName;

        // Get the path to the wwwroot/Videos folder
        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Videos");

        // Ensure the folder exists, create if necessary
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        // Combine the folder path with the unique file name
        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        // Save the video file to the specified path
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            previewVideo.CopyTo(fileStream);
        }

        // Return the relative path to be saved in the database
        return "/Videos/" + uniqueFileName;
    }

    // Helper function to delete the preview video
    private void DeletePreviewVideo(string previewUrl)
    {
        // Get the absolute path to the preview video file
        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, previewUrl.TrimStart('/'));

        // Check if the file exists, and if so, delete it
        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }
    }
}
