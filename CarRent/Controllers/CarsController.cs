using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CarRent.DTOs;
using CarRent.Models;
using CarRent.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRent.Data
{
    public class CarsController : Controller
    {
        private readonly ICarRepository _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        public CarsController(ICarRepository repository, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _repository = repository;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("{controller}/")]
        public ActionResult Index(bool isRented)
        {
            ViewBag.Title = "Our Cars";
            var cars = _repository.GetCars();
            var model = new CarsViewModel()
            {
                Cars = cars,
                IsRented = isRented
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult New()
        {
            ViewBag.CarTypes = Enum.GetNames(typeof(CarType)).ToList();
            ViewBag.Title = "Create";
            ViewBag.Action = "Create";

            var car = new CarDto();

            return View("CarForm", car);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarDto car, IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                if (car.ImageFile != null)
                {
                    string rootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(car.ImageFile.FileName);
                    string extension = Path.GetExtension(car.ImageFile.FileName);
                    car.ImageName = fileName + extension;
                    string path = Path.Combine(rootPath + "/images/", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await car.ImageFile.CopyToAsync(fileStream);
                    }
                }

                _repository.InsertCar(car);
                return RedirectToAction(nameof(Index));
            }

            return BadRequest();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var car = _repository.GetCarByID(id);

            if (car == null) return NotFound();

            ViewBag.CarTypes = Enum.GetNames(typeof(CarType)).ToList();
            ViewBag.Title = "Edit";
            ViewBag.Action = "Update";

            return View("CarForm", car);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CarDto car, IFormCollection collection)
        {
                if (ModelState.IsValid)
                {
                    if (car.ImageFile != null)
                    {
                        string rootPath = _webHostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(car.ImageFile.FileName);
                        string extension = Path.GetExtension(car.ImageFile.FileName);
                        car.ImageName = fileName + extension;
                        string path = Path.Combine(rootPath + "/images/", fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await car.ImageFile.CopyToAsync(fileStream);
                        }

                }
                    _repository.UpdateCar(car);
                }
                
                return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var car = _repository.GetCarByID(id);

            if (car == null) return NotFound();

            return View("Delete", car);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id, IFormCollection collection)
        {
            try
            {
                _repository.DeleteCar(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Index");
            }
        }
    }
}
