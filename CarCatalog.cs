using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_5
{
    public class CarCatalog
    {
        private readonly IFileSaver fileSaver;
        private readonly IFileLoader fileLoader;
        private readonly string filePath;

        public ObservableCollection<Car> Cars { get; }//колекція для WPF

        private List<Car> allCars;

        public CarCatalog(IFileSaver fileSaver, IFileLoader fileLoader, string filePath)
        {
            this.fileSaver = fileSaver;
            this.fileLoader = fileLoader;
            this.filePath = filePath;
            Cars = new ObservableCollection<Car>();
            allCars = new List<Car>();
        }

        public void AddCar(Car car)
        {
            Cars.Add(car);
            allCars.Add(car);
        }
        public void DeleteCar(Car car)
        {
            Cars.Remove(car);
            allCars.Remove(car);
        }

        public void SortByMileage()
        {
            var sortedCars = Cars.OrderBy(c => c.Mileage).ToList();
            UpdateVisibleCollection(sortedCars);
        }

        public void SortByBrand()
        {
            var sortedCars = Cars.OrderBy(c => c.Brand).ToList();
            UpdateVisibleCollection(sortedCars);
        }

        public void SearchCars(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                ResetFilter();
                return;
            }

            string lowerTerm = searchTerm.ToLowerInvariant();
            var searchResult = allCars.Where(c =>
            c.Brand.ToLowerInvariant().Contains(lowerTerm) || 
            c.Owner.ToLowerInvariant().Contains(lowerTerm)).ToList();

            UpdateVisibleCollection(searchResult);
        }
        public void SearchUniqueBrands()
        {
            var uniqueBrands = allCars.Select(c => c.Brand).Distinct().OrderBy(b => b).ToList();
            var brandCars = uniqueBrands.Select(brand => new Car(brand, "-", 2000, 0)).ToList();
            UpdateVisibleCollection(brandCars);
        }
        public void FindThreeNewestCars()
        {
            var newestCars = allCars.OrderByDescending(c => c.Year).Take(3).ToList();
            UpdateVisibleCollection(newestCars);
        }
        public double CalculateAverageMileage()
        {
            if (allCars.Count == 0)
            {
                return 0;
            }
            return allCars.Average(c => c.Mileage);
        }
        public void GroupCarsByYear()
        {
            var grouped = allCars.GroupBy(c => c.Year).OrderByDescending(g => g.Key).SelectMany(g => g).ToList();
            UpdateVisibleCollection(grouped); 
        }

        public void FilterByYearBefore(int year)
        {
            var filtered = allCars.Where(c => c.Year < year).ToList();
            UpdateVisibleCollection(filtered);
        }

        public void ResetFilter()
        {
            UpdateVisibleCollection(allCars);
        }

        private void UpdateVisibleCollection(List<Car> newList)
        {
            Cars.Clear();
            foreach (var car in newList)
            {
                Cars.Add(car);
            }
        }

        public int MileageMin()
        {
            int min = Cars[0].Mileage;

            foreach (var car in Cars)
            {
                if (car.Mileage < min)
                {
                    min = car.Mileage;
                }
            }
            return min;
        }


        public async Task SaveAsync()
        {
            await fileSaver.SaveAsync(filePath, allCars);
        }

        public async Task LoadAsync()
        {
            var loaded = await fileLoader.LoadAsync<Car>(filePath);
            if (loaded != null)
            {
                allCars = loaded;
                UpdateVisibleCollection(allCars);
            }
        }
    }
}
