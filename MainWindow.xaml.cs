using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lab_5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CarCatalog catalog;

        public MainWindow()
        {
            InitializeComponent();

            // зберігаємо файл у стандартній системній папці "документи"
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = System.IO.Path.Combine(documentsPath, "cars_database.json");

            //ініціалізація каталогу з передачею реалізацій інтерфейсів JSON
            catalog = new CarCatalog(new JsonFileWriter(), new JsonFileReader(), filePath);

            // прив'язуємо ObservableCollection до DataGrid
            CarsDataGrid.ItemsSource = catalog.Cars;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(YearTextBox.Text, out int year))
                    throw new ArgumentException("Введіть коректне числове значення для року придбання.");

                if (!int.TryParse(MileageTextBox.Text, out int mileage))
                    throw new ArgumentException("Введіть коректне числове значення для пробігу.");

                var car = new Car(BrandTextBox.Text, OwnerTextBox.Text, year, mileage);
                catalog.AddCar(car);

                // Очищення полів форми
                BrandTextBox.Clear();
                OwnerTextBox.Clear();
                YearTextBox.Clear();
                MileageTextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка валідації", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (CarsDataGrid.SelectedItem is Car selectedCar)
            {
                catalog.DeleteCar(selectedCar);
            }
            else
            {
                MessageBox.Show("Будь ласка, оберіть рядок з автомобілем для видалення.", "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SortByMileageButton_Click(object sender, RoutedEventArgs e)
        {
            catalog.SortByMileage();
        }
        private void MileageMin(object sender, RoutedEventArgs e)
        {
            int min = catalog.MileageMin();

            MessageBox.Show($"Мінімальний пробіг серед авто {min} км",
                            "Статистика", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                catalog.ResetFilter();
            }
            else
            {
                catalog.SearchCars(SearchTextBox.Text);
            }
        }

        private void UniqueBrandsButton_Click(object sender, RoutedEventArgs e)
        {
            catalog.SearchUniqueBrands();
        }

        private void NewestCarsButton_Click(object sender, RoutedEventArgs e)
        {
            catalog.FindThreeNewestCars();
        }

        private void AverageMileageButton_Click(object sender, RoutedEventArgs e)
        {
            double avgMileage = catalog.CalculateAverageMileage();
            MessageBox.Show($"Середній пробіг: {avgMileage:F1} км",
                            "Агрегація даних",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }

        private void GroupByYearButton_Click(object sender, RoutedEventArgs e)
        {
            catalog.GroupCarsByYear();
        }
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FilterYearTextBox.Text))
            {
                catalog.ResetFilter();
            }
            else if (int.TryParse(FilterYearTextBox.Text, out int year))
            {
                catalog.FilterByYearBefore(year);
            }
            else
            {
                MessageBox.Show("Будь ласка, введіть коректний рік для фільтрації.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ResetFilterButton_Click(object sender, RoutedEventArgs e)
        {
            FilterYearTextBox.Clear();
            catalog.ResetFilter();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await catalog.SaveAsync();
                MessageBox.Show("Дані успішно збережено у файл JSON.", "Збереження", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка збереження: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await catalog.LoadAsync();
                MessageBox.Show("Дані успішно завантажено з файлу.", "Завантаження", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}