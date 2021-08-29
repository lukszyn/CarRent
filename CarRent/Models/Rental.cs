using CarRent.Validators;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRent.Models
{
    public class Rental
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Date of rental")]
        public DateTime DateOfRental { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [RentalDateValidator]
        [Display(Name = "Date of return")]
        public DateTime DateOfReturn { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        public double Payment { get; set; }

        [Display(Name = "Is Paid")]
        public bool isPaid { get; set; }

        public User User { get; set; }
        public Car Car { get; set; }
    }
}
