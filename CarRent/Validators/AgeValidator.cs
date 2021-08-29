using CarRent.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace CarRent.Models
{
    public class AgeValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (value == null)
                return new ValidationResult("Date of birth is required.");

            var age = DateTime.Today.Year - ((DateTime)value).Year;

            return (age >= 18)
                ? ValidationResult.Success
                : new ValidationResult("Customer should be at least 18 years old to rent a car.");
        }
    }
}