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
using System.Windows.Shapes;

namespace Finance
{
    /// <summary>
    /// Логика взаимодействия для AddSpendingDialog.xaml
    /// </summary>
    public partial class AddSpendingDialog : Window
    {
        public double Amount { get; private set; }
        public string Category { get; private set; }
        public string Description { get; private set; }

        public AddSpendingDialog()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            double amount;

            if (!double.TryParse(AmountTextBox.Text, out amount) || amount <= 0)
            {
                MessageBox.Show("Введите корректную сумму.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Amount = amount;

            Category = CategoryTextBox.Text;
            Description = SpendingAmountTextBox.Text;

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}