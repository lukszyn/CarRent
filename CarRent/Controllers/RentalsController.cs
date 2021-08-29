using System;
using System.Linq;
using System.Threading.Tasks;
using CarRent.Data;
using CarRent.Models;
using CarRent.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRent.Controllers
{
    [Authorize]
    public class RentalsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private AppDbContext _context;

        public RentalsController(UserManager<User> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Index(bool isValid)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var rentals = _context.Rentals.Include(r => r.User).Include(r => r.Car).Where(r => r.User.Id == user.Id).ToList();

            var model = new UserRentalsViewModel()
            {
                Rentals = rentals,
                IsValid = isValid
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllRentals()
        {
            var rentals = _context.Rentals.Include(r => r.User).Include(r => r.Car).ToList();

            return View(rentals);
        }

        [Authorize]
        public async Task<IActionResult> New(int id)
        {
            var car = _context.Cars.Find(id);

            if (car.isAvailable)
            {
                var newRental = new Rental()
                {
                    DateOfRental = DateTime.Now,
                    DateOfReturn = DateTime.Now.AddDays(7),
                    User = await _userManager.FindByNameAsync(User.Identity.Name),
                    Car = car,
                    Payment = car.PricePerDay * 7,
                    isPaid = false
                };

                car.isAvailable = false;
                _context.Entry(car).State = EntityState.Modified;
                _context.Rentals.Add(newRental);
                _context.SaveChanges();
            }
            else
            {
                return RedirectToAction("Index", "Cars", new { IsRented = true});
            }

            return RedirectToAction("Index", new { isValid = true });
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);

            var model = new EditRentalViewModel
            {
                Id = id,
                DateOfRental = rental.DateOfRental,
                DateOfReturn = rental.DateOfReturn
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(EditRentalViewModel model)
        {

            var rental = await _context.Rentals.FindAsync(model.Id);

            if (rental == null)
            {
                ViewBag.ErrorMessage = $"Rental with Id = {model.Id} cannot be found";
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    rental.DateOfReturn = model.DateOfReturn;
                    _context.Entry(rental).State = EntityState.Modified;
                    _context.SaveChanges();
                    return RedirectToAction("Index", new { isValid = true });
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Invalid Rental Edit Attempt");
                }
            }

            return View(model);
        }

        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var rental = _context.Rentals.Find(id);

            if (rental == null)
            {
                ViewBag.ErrorMessage = $"Rental with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                try
                {
                    _context.Remove(rental);
                    _context.SaveChanges();
                    return RedirectToAction("AllRentals");
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "The rental could not be deleted");
                }
                return View("AllRentals");
            }
        }

        [Authorize]
        public async Task<IActionResult> Return(int id)
        {
            var rental = await _context.Rentals.Include(c => c.Car).Include(u => u.User).FirstOrDefaultAsync(r => r.Id == id);

            if (DateTime.Now < rental.DateOfReturn && rental.isPaid)
            {
                rental.Car.isAvailable = true;
                rental.DateOfReturn = DateTime.Now;
                var car = rental.Car;

                _context.Entry(rental).State = EntityState.Modified;
                _context.Entry(car).State = EntityState.Modified;
                var result = await _context.SaveChangesAsync();

                if (result == 2)
                {
                    return RedirectToAction("Index", "Rentals", new { IsValid = true });
                }
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Pay(int id)
        {
            var rental = await _context.Rentals.Include(c => c.Car).Include(u => u.User).FirstOrDefaultAsync(r => r.Id == id);

            rental.isPaid = !rental.isPaid ? true : true;

            _context.Entry(rental).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Rentals", new { IsValid = true });
        }
    }
}
