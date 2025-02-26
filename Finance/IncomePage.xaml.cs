using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;

namespace Finance
{
    /// <summary>
    /// Логика взаимодействия для IncomePage.xaml
    /// </summary>
    public partial class IncomePage : Page
    {
        private string connectionString = "Server=510EC16;Database=FINANCE;Trusted_Connection=True;";

        public IncomePage()
        {
            InitializeComponent();
            UpdateTotalIncome();
            UpdatePieChart();
        }
        private void UpdateTotalIncome()
        {
            string query = "SELECT SUM(Amount) FROM [Transactions]";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    var total = command.ExecuteScalar();
                    TotalIncomeTextBlock.Text = total != DBNull.Value ? $"₽ {total}" : "₽ 0";
                }
            }
        }
        private int GetCategoryId(string categoryName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT Id FROM Category WHERE Name = @Name";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", categoryName);
                    var result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }
        public void UpdatePieChart() // Метод должен быть public
        {
            string query = @"
            SELECT c.Name AS Category, SUM(t.Amount) AS TotalAmount 
            FROM [Transactions] t
            INNER JOIN [Categories] c ON t.CategoryId = c.Id
            GROUP BY c.Name";

            var values = new ChartValues<double>();
            var labels = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            labels.Add(reader["Category"].ToString());
                            values.Add(Convert.ToDouble(reader["TotalAmount"]));
                        }
                    }
                }
            }

            pieChart.Series.Clear();
            pieChart.Series.Add(new PieSeries
            {
                Title = "Доходы",
                Values = values,
                DataLabels = true
            });

            pieChart.LegendLocation = LegendLocation.Right;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var periodWindow = new PeriodSelectionWindow();
            if (periodWindow.ShowDialog() == true)
            {
                DateTime startDate = periodWindow.StartDate;
                DateTime endDate = periodWindow.EndDate;

                UpdateTotalIncome(startDate, endDate);
                UpdatePieChart(startDate, endDate);
            }
        }

        private void UpdateTotalIncome(DateTime startDate, DateTime endDate)
        {
            string query = "SELECT SUM(Amount) FROM [Transactions] WHERE TransactionDate >= @StartDate AND TransactionDate <= @EndDate";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);
                    var total = command.ExecuteScalar();
                    TotalIncomeTextBlock.Text = total != DBNull.Value ? $"₽ {total}" : "₽ 0";
                }
            }
        }

        private void UpdatePieChart(DateTime startDate, DateTime endDate)
        {
            string query = @"
        SELECT c.Name AS Category, SUM(t.Amount) AS TotalAmount 
        FROM [Transactions] t
        INNER JOIN [Categories] c ON t.CategoryId = c.Id
        WHERE t.TransactionDate >= @StartDate AND t.TransactionDate <= @EndDate
        GROUP BY c.Name";

            var values = new ChartValues<double>();
            var labels = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            labels.Add(reader["Category"].ToString());
                            values.Add(Convert.ToDouble(reader["TotalAmount"]));
                        }
                    }
                }
            }

            pieChart.Series.Clear();
            pieChart.Series.Add(new PieSeries
            {
                Title = "Доходы",
                Values = values,
                DataLabels = true
            });

            pieChart.LegendLocation = LegendLocation.Right;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var addIncomeWindow = new AddIncomeWindow();
            if (addIncomeWindow.ShowDialog() == true)
            {
                // Обновляем данные после добавления доходов
                UpdateTotalIncome();
                UpdatePieChart();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var addCategoryWindow = new AddCategoryWindow(this);
            if (addCategoryWindow.ShowDialog() == true)
            {
              
            }
        }
    }
}