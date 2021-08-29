using CarRent.ViewModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace CarRent.Validators
{
    public class RentalDateValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var rental = (EditRentalViewModel)validationContext.ObjectInstance;

            if (rental.DateOfRental == null)
                return new ValidationResult("Date of rental is required.");

            var isDateValid = ((DateTime)value) > rental.DateOfRental;

            return (isDateValid)
                ? ValidationResult.Success
                : new ValidationResult("Date of return should be after the date of rental.");
        }
    }
}
