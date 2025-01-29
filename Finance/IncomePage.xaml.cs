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
using LiveCharts;
using LiveCharts.Wpf;

namespace Finance
{
    /// <summary>
    /// Логика взаимодействия для IncomePage.xaml
    /// </summary>
    public partial class IncomePage : Page
    {
        public IncomePage()
        {
            InitializeComponent();

            pieChart.Series = new LiveCharts.SeriesCollection
            {
                new PieSeries
                {
                    Values = new ChartValues<double> { 20},
                DataLabels= true,
                Fill = new SolidColorBrush(Colors.Red),
                LabelPoint = ChartPoint => $"{ChartPoint.Y} ({ChartPoint.Participation:P})"
                },
                new PieSeries
                {
                    Values = new ChartValues<double> { 20},
                DataLabels= true,
                Fill = new SolidColorBrush(Colors.Green),
                LabelPoint = ChartPoint => $"{ChartPoint.Y} ({ChartPoint.Participation:P})"
                },
                new PieSeries
                {
                    Values = new ChartValues<double> { 20},
                DataLabels= true,
                Fill = new SolidColorBrush(Colors.Blue),
                LabelPoint = ChartPoint => $"{ChartPoint.Y} ({ChartPoint.Participation:P})"
                }
            };
            pieChart.LegendLocation = LegendLocation.Right;
        }
    }
}
