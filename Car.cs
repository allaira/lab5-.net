using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_5
{
    public class Car : IComparable<Car>
    {
        private string brand = string.Empty;
        private string owner = string.Empty;
        private int year;
        private int mileage;

        public string Brand
        {
            get => brand;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Марка автомобіля не може бути порожньою", nameof(value));
                }
                brand = value;
            }
        }

        public string Owner
        {
            get => owner;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Прізвище власника не може бути порожнім", nameof(Owner));
                owner = value.Trim();
            }
        }

        public int Year
        {
            get => year;
            set
            {
                if (value < 1900 || value > DateTime.Now.Year)
                    throw new ArgumentException($"Рік придбання має бути в межах від 1900 до {DateTime.Now.Year}", nameof(Year));
                year = value;
            }
        }

        public int Mileage
        {
            get => mileage;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Пробіг не може бути від'ємним", nameof(Mileage));
                mileage = value;
            }
        }

        public Car(string brand, string owner, int year, int mileage)
        {
            Brand = brand;
            Owner = owner;
            Year = year;
            Mileage = mileage;
        }

        //  сортування за алфавітом (марка)
        public int CompareTo(Car? other)
        {
            if (other == null) return 1;
            return string.Compare(Brand, other.Brand, StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return $"{Brand} ({Year}) — Власник: {Owner}, Пробіг: {Mileage} км";
        }
    }
}
