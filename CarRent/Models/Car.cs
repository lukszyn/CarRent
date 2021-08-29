using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRent.Models
{

    public enum CarType
    {
        PASSENGER,
        DELIVERY
    }
    public class Car
    {
        public int Id { get; set; }

        [MaxLength(64)]
        [Required]
        public string Brand { get; set; }

        [MaxLength(64)]
        [Required]
        public string Model { get; set; }

        [Display(Name = "Image name")]
        public string? ImageName { get; set; }

        [NotMapped]
        [Display(Name = "Upload file")]
        public IFormFile ImageFile { get; set; }

        [Required]
        [Range(2000, 2050)]
        public int Year { get; set; }

        [Range(0, 500000)]
        public int Mileage { get; set; }

        [Required]
        public string LicensePlate { get; set; }
        public CarType Type { get; set; }
        public bool isAvailable { get; set; }

        [Range(0, 500)]
        [Display(Name = "Price per day")]
        public double PricePerDay { get; set; }

        public ICollection<Rental> Rentals { get; set; }

    }
}
