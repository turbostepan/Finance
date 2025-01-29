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
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
        public partial class SettingsPage : Page
        {
            public SettingsPage()
            {
                InitializeComponent();
                App.ApplyTheme(this);
            }

            private void ApplyTheme()
            {
                ToggleThemeButton.Content = App.IsDarkTheme ? "Включить светлую тему" : "Включить темную тему";

                var stackPanel = this.FindName("MainStackPanel") as StackPanel;
                foreach (var child in stackPanel.Children)
                {
                    if (child is Button button)
                    {
                        button.Background = App.IsDarkTheme
                            ? (SolidColorBrush)Application.Current.Resources["SecondColor"]
                            : (SolidColorBrush)Application.Current.Resources["ElementsColor"];
                    }
                }
            }

            private void ExitButton_Click(object sender, RoutedEventArgs e)
            {
                Application.Current.Shutdown();
            }

            private void ToggleThemeButton_Click(object sender, RoutedEventArgs e)
            {

                App.IsDarkTheme = !App.IsDarkTheme;

                foreach (Window window in Application.Current.Windows)
                {
                    App.ApplyTheme(window);
                }

                ApplyTheme();
            }
        }
    }


