using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
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
using LiveCharts.Definitions.Charts;
using LiveCharts.Wpf;
using LiveCharts;
using System.Data.SqlClient;

namespace Finance
{
    /// <summary>
    /// Логика взаимодействия для ExpensesPage.xaml
    /// </summary>
    public partial class SpendingsPage : Page
    {
        private string connectionString = "Server=510EC16;Database=FINANCE;Trusted_Connection=True;";

        public SpendingsPage()
        {
            InitializeComponent();
            UpdateTotalExpenses();
            UpdatePieChart();
        }

        private void UpdateTotalExpenses()
        {
            string query = "SELECT SUM(Amount) FROM [Spending]";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    var total = command.ExecuteScalar();
                    TotalExpensesTextBlock.Text = total != DBNull.Value ? $"₽ {total}" : "₽ 0";
                }
            }
        }

        private int GetCategoryId(string categoryName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT Id FROM Categories WHERE Name = @Name";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", categoryName);
                    var result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        public void UpdatePieChart()
        {
            string query = @"
                SELECT c.Name AS Category, SUM(s.Amount) AS TotalAmount 
                FROM [Spending] s
                INNER JOIN [Categories] c ON s.CategoryId = c.Id
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
                Title = "Расходы",
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

                UpdateTotalExpenses(startDate, endDate);
                UpdatePieChart(startDate, endDate);
            }
        }

        private void UpdateTotalExpenses(DateTime startDate, DateTime endDate)
        {
            string query = "SELECT SUM(Amount) FROM [Spending] WHERE TransactionDate >= @StartDate AND TransactionDate <= @EndDate";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);
                    var total = command.ExecuteScalar();
                    TotalExpensesTextBlock.Text = total != DBNull.Value ? $"₽ {total}" : "₽ 0";
                }
            }
        }

        private void UpdatePieChart(DateTime startDate, DateTime endDate)
        {
            string query = @"
                SELECT c.Name AS Category, SUM(s.Amount) AS TotalAmount 
                FROM [Spending] s
                INNER JOIN [Categories] c ON s.CategoryId = c.Id
                WHERE s.TransactionDate >= @StartDate AND s.TransactionDate <= @EndDate
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
                Title = "Расходы",
                Values = values,
                DataLabels = true
            });

            pieChart.LegendLocation = LegendLocation.Right;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var addExpenseWindow = new AddExpenseWindow();
            if (addExpenseWindow.ShowDialog() == true)
            {
                // Обновляем данные после добавления расходов
                UpdateTotalExpenses();
                UpdatePieChart();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var addCategoryWindow = new AddCategoryWindow(this);
            if (addCategoryWindow.ShowDialog() == true)
            {
                // Обновляем данные после добавления категории
                UpdatePieChart();
            }
        }
    }
}