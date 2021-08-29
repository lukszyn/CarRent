using CarRent.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CarRent.DTOs
{
    public class CarDto
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Mileage { get; set; }

        [Display(Name = "Upload an image file")]
        public IFormFile ImageFile { get; set; }
        public string? ImageName { get; set; }

        [Display(Name = "Licence plate number")]
        public string LicensePlate { get; set; }

        public CarType Type { get; set; }

        [Display(Name = "Available")]
        public bool isAvailable { get; set; }

        [Display(Name = "Price per day")]
        public double PricePerDay { get; set; }

    }

}
