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
using System.Data.SqlClient;

namespace Finance
{
    /// <summary>
    /// Логика взаимодействия для HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private string connectionString = "Server=510EC16;Database=FINANCE;Trusted_Connection=True;";

        public HomePage()
        {
            InitializeComponent();
            UpdateCharts();
            UpdateMainBalance();
        }

        private void UpdateCharts()
        {
            UpdateIncomeChart();
            UpdateExpenseChart();
        }

        private void UpdateIncomeChart()
        {
            string query = @"
            SELECT c.Name AS Category, SUM(t.Amount) AS TotalAmount 
            FROM [Transactions] t
            INNER JOIN [Categories] c ON t.CategoryId = c.Id
            WHERE c.Type = 'Income'
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
                DataLabels = true,
                LabelPoint = ChartPoint => $"{ChartPoint.Y} ({ChartPoint.Participation:P})"
            });

            pieChart.LegendLocation = LegendLocation.Right;
        }

        private void UpdateExpenseChart()
        {
            string query = @"
            SELECT c.Name AS Category, SUM(s.Amount) AS TotalAmount 
            FROM [Spending] s
            INNER JOIN [Categories] c ON s.CategoryId = c.Id
            WHERE c.Type = 'Expense'
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

            pieChart2.Series.Clear();
            pieChart2.Series.Add(new PieSeries
            {
                Title = "Расходы",
                Values = values,
                DataLabels = true,
                LabelPoint = ChartPoint => $"{ChartPoint.Y} ({ChartPoint.Participation:P})"
            });

            pieChart2.LegendLocation = LegendLocation.Right;
        }

        private void UpdateMainBalance()
        {
            decimal totalIncome = GetTotalIncome();
            decimal totalExpenses = GetTotalExpenses();
            decimal mainBalance = totalIncome - totalExpenses;

            MainBalanceTextBlock.Text = $"₽ {mainBalance}";
        }

        private decimal GetTotalIncome()
        {
            string query = "SELECT SUM(Amount) FROM [Transactions]";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    var result = command.ExecuteScalar();
                    return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                }
            }
        }

        private decimal GetTotalExpenses()
        {
            string query = "SELECT SUM(Amount) FROM [Spending]";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    var result = command.ExecuteScalar();
                    return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Логика для создания счета
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var addIncomeWindow = new AddIncomeWindow();
            if (addIncomeWindow.ShowDialog() == true)
            {
                UpdateCharts();
                UpdateMainBalance();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var addExpenseWindow = new AddExpenseWindow();
            if (addExpenseWindow.ShowDialog() == true)
            {
                UpdateCharts();
                UpdateMainBalance();
            }
        }
    }
}