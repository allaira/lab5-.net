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
            var sortedList = new List<Car>(Cars);
            sortedList.Sort(new CarMileageComparer());
            UpdateVisibleCollection(sortedList);
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
