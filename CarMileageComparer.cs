using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_5
{
    public class CarMileageComparer : IComparer<Car>
    {
        public int Compare(Car? x, Car? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            return x.Mileage.CompareTo(y.Mileage);
        }
    }
}
