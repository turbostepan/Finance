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

namespace Finance
{
    /// <summary>
    /// Логика взаимодействия для CodePage.xaml
    /// </summary>
    public partial class CodePage : Page
    {
        public CodePage()
        {
            InitializeComponent();
        }

        private void RegistrationButtonClick(object sender, MouseButtonEventArgs e)
        {

            NavigationService.Navigate(new RegistrationPage());
        }

        private void ReturnButtonClick(object sender, MouseButtonEventArgs e)
        {

            NavigationService.Navigate(new EmailPage());
        }


        private void Enter_Button_Click(object sender, RoutedEventArgs e)
        { MessageBox.Show("Пока в разработке", "ОЙ-ОЙ", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
}
