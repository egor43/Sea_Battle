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

namespace Unit_wpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 


    public partial class MainWindow : Window
    {



        public MainWindow()
        {

            InitializeComponent();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<Unit> unit_arr = new List<Unit>();
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {
                     unit_arr.Add( new Unit(j*30, i * 30, unit_type.sea));                    
                }

            //Так рисуем картинку
            foreach (var v in unit_arr)
            {
                ImageBrush myImageBrush = new ImageBrush(v.Get_Image());
                Rectangle myCanvas = new Rectangle();
                myCanvas.Width = 30;
                myCanvas.Height = 30;
                myCanvas.Fill = myImageBrush;
                grid.Children.Add(myCanvas);
                Grid.SetColumn(myCanvas, v.Get_X() / 30);
                Grid.SetRow(myCanvas,v.Get_Y()/30);
            }
                   
        }
    }
}
