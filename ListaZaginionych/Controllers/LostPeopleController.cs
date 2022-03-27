using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ListaZaginionych.Data.Models;
using ListaZaginionych.Logic;
using ListaZaginionych.Models;
using System.Collections.Generic;
using System.IO;
using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace ListaZaginionych.Controllers
{
    public class LostPeopleController : Controller
    {
        private readonly ILostPeopleManager _managerLostPeople;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public LostPeopleController(ILostPeopleManager lostPeopleManager, IWebHostEnvironment hostEnvironment)
        {
            _managerLostPeople = lostPeopleManager;
            _webHostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            IList<LostPeopleModel> list = _managerLostPeople.GetAll();
            return View(list);
        }

        public IActionResult Details(int id)
        {
            var el = _managerLostPeople.Get(id);
            return View(el);
        }

        [Authorize(Policy = "writepolicy")]
        public IActionResult Delete(int id)
        {
            var item = _managerLostPeople.Get(id);
            
            return View(item);
        }

        [HttpPost]
        [Authorize(Policy = "writepolicy")]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmedDelete(LostPeopleModel modelPost)
        {
            RemoveFile(modelPost.Image);
            _managerLostPeople.Remove(modelPost.Id);
            
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "writepolicy")]
        public IActionResult Edit(int id)
        {
            var item = _managerLostPeople.Get(id);
            LostPeopleViewModel model = new LostPeopleViewModel()
            {
                FirstName = item.FirstName,
                LastName = item.LastName,
                Age = item.Age,
                Gender = item.Gender,
                LastSeenDate = item.LastSeenDate,
                LastSeenLocation = item.LastSeenLocation,
                ImagePath = item.Image
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "writepolicy")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(LostPeopleViewModel model)
        {
            if (ModelState.IsValid)
            {
                LostPeopleModel dbModel = new LostPeopleModel()
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Age = model.Age,
                    Gender = model.Gender,
                    LastSeenDate = model.LastSeenDate,
                    LastSeenLocation = model.LastSeenLocation,
                    Image = (model.Image != null) ? UploadFile(model) : model.ImagePath
                };
                _managerLostPeople.Update(dbModel);

                if (model.ImagePath != dbModel.Image)
                {
                    RemoveFile(model.ImagePath);
                }

                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        [Authorize(Policy = "readpolicy")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "readpolicy")]
        public IActionResult Add(LostPeopleViewModel model)
        {
            if(ModelState.IsValid)
            {
                string uniqueFileName = UploadFile(model);

                LostPeopleModel lostPeopleModel = new LostPeopleModel()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Age = model.Age,
                    Gender = model.Gender,
                    LastSeenLocation = model.LastSeenLocation,
                    LastSeenDate = model.LastSeenDate,
                    Image = uniqueFileName,
                };

                _managerLostPeople.Add(lostPeopleModel);

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void RemoveFile(string fileName)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            string filePath = Path.Combine(uploadsFolder, fileName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            } 
        }
        private string UploadFile(LostPeopleViewModel model)
        {
            string uniqueFileName = null;

            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using(var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
