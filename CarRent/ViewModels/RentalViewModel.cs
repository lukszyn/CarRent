using CarRent.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRent.ViewModels
{
    public class RentalViewModel
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateOfRental { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateOfReturn { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        public double Payment { get; set; }

        public bool isPaid { get; set; }

        public User User { get; set; }
        public Car Car { get; set; }

    }
}
