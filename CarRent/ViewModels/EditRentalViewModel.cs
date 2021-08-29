using CarRent.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace CarRent.ViewModels
{
    public class EditRentalViewModel
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
    }
}
