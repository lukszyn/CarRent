using CarRent.DTOs;
using System.Collections.Generic;

namespace CarRent.ViewModels
{
    public class CarsViewModel
    {
        public IEnumerable<CarDto> Cars { get; set; }
        public bool IsRented { get; set; }
    }
}
