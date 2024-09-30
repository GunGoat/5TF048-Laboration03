using Laboration03.Application.Common.Interfaces;
using Laboration03.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Laboration03.Controllers
{
    public class ActorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ActorController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var actors = _unitOfWork.Actors.GetAll();
            return View(actors);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Actor actor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (actor.Profile != null)
                    {
                        // Validate that the uploaded file is an image
                        if (!IsImageFile(actor.Profile))
                        {
                            ModelState.AddModelError("Profile", "Only image files (jpeg, jpg, png) are allowed.");
                            return View(actor);
                        }

                        actor.ProfileUrl = SaveProfileImage(actor.Profile);
                    }

                    _unitOfWork.Actors.Add(actor);
                    _unitOfWork.Commit();
                    TempData["success"] = $"Successfully added actor '{actor.Name}'";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _unitOfWork.Rollback();
                    TempData["error"] = $"Unable to add actor '{actor.Name}'. Error: {ex.Message}";
                }
            }
            else
            {
                TempData["error"] = $"Invalid data for actor '{actor.Name}'.";
            }
            return View(actor);
        }

        public IActionResult Update(int id)
        {
            var actor = _unitOfWork.Actors.GetById(id);
            if (actor != null)
            {
                return View(actor);
            }
            TempData["error"] = $"Unable to update actor with id '{id}'.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Update(Actor actor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var actorFromDb = _unitOfWork.Actors.GetById(actor.ActorID);


                    if (actor.Profile != null)
                    {
                        // Delete the old image if it exists
                        if (!string.IsNullOrEmpty(actorFromDb.ProfileUrl))
                        {
                            // Validate that the uploaded file is an image
                            if (!IsImageFile(actor.Profile))
                            {
                                ModelState.AddModelError("Profile", "Only image files (jpeg, jpg, png) are allowed.");
                                return View(actor);
                            }

                            DeleteProfileImage(actorFromDb.ProfileUrl);
                        }

                        // Save the new image
                        actor.ProfileUrl = SaveProfileImage(actor.Profile);
                    }

                    _unitOfWork.Actors.Update(actor);
                    _unitOfWork.Commit();
                    TempData["success"] = $"Successfully updated actor '{actor.Name}'.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _unitOfWork.Rollback();
                    TempData["error"] = $"Unable to update actor '{actor.Name}'. Error: {ex.Message}";
                }
            }
            else
            {
                TempData["error"] = $"Invalid data for actor '{actor.Name}'.";
            }
            return View(actor);
        }

        public IActionResult Delete(int id)
        {
            var actor = _unitOfWork.Actors.GetById(id);
            if (actor != null)
            {
                return View(actor);
            }
            TempData["error"] = $"Unable to delete actor with id '{id}'.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(Actor actor)
        {
            var actorFromDb = _unitOfWork.Actors.GetById(actor.ActorID);
            if (actorFromDb != null)
            {
                try
                {
                    // Delete the profile image if it exists
                    if (!string.IsNullOrEmpty(actorFromDb.ProfileUrl))
                    {
                        DeleteProfileImage(actorFromDb.ProfileUrl);
                    }

                    _unitOfWork.Actors.Delete(actorFromDb.ActorID);
                    _unitOfWork.Commit();
                    TempData["success"] = $"Successfully deleted actor '{actorFromDb.Name}'.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _unitOfWork.Rollback();
                    TempData["error"] = $"Unable to delete actor '{actorFromDb.Name}'. Error: {ex.Message}";
                }
            }
            else
            {
                TempData["error"] = $"Unable to delete actor with id '{actor.ActorID}'.";
            }
            return View(actor);
        }


        // Helper function to validate if the uploaded file is an image
        private bool IsImageFile(IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(extension);
        }

        // Helper function to save the profile image
        private string SaveProfileImage(IFormFile profileImage)
        {
            // Generate a unique file name
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + profileImage.FileName;

            // Get the path to the wwwroot/Images folder
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");

            // Combine the folder path with the unique file name
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save the file to the specified path
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                profileImage.CopyTo(fileStream);
            }

            // Return the relative path to be saved in the database
            return "/Images/" + uniqueFileName;
        }

        // Helper function to delete the profile image
        private void DeleteProfileImage(string profileUrl)
        {
            // Get the absolute path to the profile image file
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, profileUrl.TrimStart('/'));

            // Check if the file exists, and if so, delete it
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}
