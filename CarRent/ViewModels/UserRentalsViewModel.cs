using CarRent.Models;
using System.Collections.Generic;

namespace CarRent.ViewModels
{
    public class UserRentalsViewModel
    {
        public List<Rental> Rentals { get; set; }
        public bool IsValid { get; set; }
    }
}
