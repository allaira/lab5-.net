using lab_5;

namespace lab5_tests
{
    [TestClass]
    public class CarTests
    {
        [TestMethod]
        public void Constructor_WithValidArguments_CreatesCarObject()
        {
            var car = new Car("Toyota Camry", "Шевченко", 2021, 45000);

            Assert.AreEqual("Toyota Camry", car.Brand);
            Assert.AreEqual("Шевченко", car.Owner);
            Assert.AreEqual(2021, car.Year);
            Assert.AreEqual(45000, car.Mileage);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("   ")]
        public void Constructor_WithInvalidBrand_ThrowsArgumentException(string invalidBrand)
        {
            Assert.Throws<ArgumentException>(() =>
                new Car(invalidBrand, "Петренко", 2020, 10000));
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("   ")]
        public void Constructor_WithInvalidOwner_ThrowsArgumentException(string invalidOwner)
        {
            Assert.Throws<ArgumentException>(() =>
                new Car("Audi A6", invalidOwner, 2020, 10000));
        }

        [TestMethod]
        public void Constructor_WithFutureYear_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                new Car("BMW X5", "Коваль", DateTime.Now.Year + 1, 5000));
        }

        [TestMethod]
        public void Constructor_WithEarlyYear_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                new Car("BMW X5", "Коваль", 1899, 5000));
        }

        [TestMethod]
        public void Constructor_WithNegativeMileage_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                new Car("Skoda Octavia", "Сидоренко", 2018, -100));
        }

        [TestMethod]
        public void CompareTo_WithOtherCar_ReturnsCorrectValue()
        {
            var carA = new Car("Audi A4", "Власник 1", 2019, 80000);
            var carB = new Car("BMW 320", "Власник 2", 2019, 60000);
            var carC = new Car("audi a4", "Власник 3", 2022, 20000); // однаковий бренд без урахування регістру

            Assert.IsTrue(carA.CompareTo(carB) < 0, "Audi має бути перед BMW.");
            Assert.IsTrue(carB.CompareTo(carA) > 0, "BMW має бути після Audi.");
            Assert.AreEqual(0, carA.CompareTo(carC), "Порівняння має бути нечутливим до регістру.");
        }

        [TestMethod]
        public void CompareTo_WithNull_ReturnsPositiveValue()
        {
            var car = new Car("Ford Focus", "Мельник", 2017, 120000);
            Assert.IsTrue(car.CompareTo(null) > 0, "Порівняння об'єкта з null має повертати додатне число.");
        }

        [TestMethod]
        public void ToString_ReturnsCorrectlyFormattedString()
        {
            var car = new Car("Honda Civic", "Ткаченко", 2020, 30000);
            string expected = "Honda Civic (2020) — Власник: Ткаченко, Пробіг: 30000 км";
            Assert.AreEqual(expected, car.ToString());
        }
    }

    [TestClass]
    public class CarMileageComparerTests
    {
        [TestMethod]
        public void Compare_WhenMileageDifferent_ReturnsCorrectOrder()
        {
            var car1 = new Car("Mazda 6", "Іван", 2018, 50000);
            var car2 = new Car("Volvo XC90", "Олег", 2020, 110000);
            var comparer = new CarMileageComparer();

            Assert.IsTrue(comparer.Compare(car1, car2) < 0, "Авто з пробігом 50к має передувати 110к.");
            Assert.IsTrue(comparer.Compare(car2, car1) > 0, "Авто з пробігом 110к має йти після 50к.");
        }

        [TestMethod]
        public void Compare_WhenMileageIdentical_ReturnsZero()
        {
            var car1 = new Car("Renault Megane", "Максим", 2015, 90000);
            var car2 = new Car("Opel Astra", "Денис", 2016, 90000);
            var comparer = new CarMileageComparer();

            Assert.AreEqual(0, comparer.Compare(car1, car2), "Автомобілі з однаковим пробігом повинні бути рівними.");
        }

        [TestMethod]
        public void Compare_WhenFirstCarIsNull_ReturnsNegative()
        {
            var car = new Car("Tesla Model 3", "Богдан", 2022, 25000);
            var comparer = new CarMileageComparer();

            Assert.AreEqual(-1, comparer.Compare(null, car));
        }

        [TestMethod]
        public void Compare_WhenBothCarsAreNull_ReturnsZero()
        {
            var comparer = new CarMileageComparer();
            Assert.AreEqual(0, comparer.Compare(null, null));
        }
    }

    [TestClass]
    public class FileHandlerTests
    {
        private string _testFilePath = string.Empty;

        [TestInitialize]
        public void TestInitialize()
        {
            _testFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".json");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

        [TestMethod]
        public async Task SaveAsync_ShouldWriteDataToFile()
        {
            var cars = new List<Car>
            {
                new Car("Toyota Corolla", "Кравченко", 2019, 65000),
                new Car("Volkswagen Golf", "Лисенко", 2017, 140000)
            };
            var fileSaver = new JsonFileWriter();

            await fileSaver.SaveAsync(_testFilePath, cars);

            Assert.IsTrue(File.Exists(_testFilePath), "Файл не було створено на диску.");
            string fileContent = await File.ReadAllTextAsync(_testFilePath);
            Assert.IsFalse(string.IsNullOrWhiteSpace(fileContent), "Файл не повинен бути порожнім.");

            var fileReader = new JsonFileReader();
            var loadedCars = await fileReader.LoadAsync<Car>(_testFilePath);
            Assert.IsNotNull(loadedCars, "Десеріалізація повернула null.");
            Assert.AreEqual(2, loadedCars.Count, "Кількість десеріалізованих об'єктів не збігається.");
        }

        [TestMethod]
        public async Task LoadAsync_WhenFileDoesNotExist_ReturnsNull()
        {
            var fileReader = new JsonFileReader();
            var result = await fileReader.LoadAsync<Car>(_testFilePath);
            Assert.IsNull(result, "Повинен повертати null, якщо файлу не існує.");
        }
    }
}
