using AutoMapper;
using CarRent.DTOs;
using CarRent.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CarRent.Data
{
    public interface ICarRepository
    {
        IEnumerable<CarDto> GetCars();
        CarDto GetCarByID(int carId);
        void InsertCar(CarDto car);
        void DeleteCar(int carId);
        void UpdateCar(CarDto car);
        void Save();
        void AddFile(CarDto car, string rootPath);
    }

    public class CarRepository : ICarRepository
    {
        private AppDbContext _context; 
        private readonly IMapper _mapper;

        public CarRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<CarDto> GetCars()
        {
            return _context.Cars.ToList().Select(car => _mapper.Map<Car, CarDto>(car));
        }

        public CarDto GetCarByID(int carId)
        {
            return _mapper.Map<Car, CarDto>(_context.Cars.SingleOrDefault(car => car.Id == carId));
        }

        public void InsertCar(CarDto car)
        {
            _context.Cars.Add(_mapper.Map<CarDto, Car>(car));
            Save();
        }

        public void DeleteCar(int carId)
        {
            Car car = _context.Cars.Find(carId);
            _context.Cars.Remove(car);
            Save();
        }

        public void UpdateCar(CarDto carDto)
        {
            var car = _context.Cars.FirstOrDefault(car => car.Id == carDto.Id);
            _context.Entry(_mapper.Map(carDto, car)).State = EntityState.Modified;
            Save();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async void AddFile(CarDto car, string rootPath)
        {
            string fileName = Path.GetFileNameWithoutExtension(car.ImageFile.FileName);
            string extension = Path.GetExtension(car.ImageFile.FileName);
            car.ImageName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            string path = Path.Combine(rootPath + "/images/", fileName);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await car.ImageFile.CopyToAsync(fileStream);
            }
        }
    }
}