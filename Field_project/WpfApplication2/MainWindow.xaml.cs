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
using Field_project;
using System.Threading;

namespace WpfApplication2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Attack_Field.SetFieldType(UserControl1.type_field.enemy_field);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            game_field.AutoSetShips();
        }

        private void Game_Field_Filled()
        {
            Attack_Field.IsEnabled = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Attack_Field.IsEnabled = false;
            this.IsEnabled = false;
            game_field.FieldFilled += Game_Field_Filled;
            if( MessageBox.Show( "Вы будете играть в сетевом режиме?", "Режим игры",  MessageBoxButton.YesNo) == MessageBoxResult.Yes )
            {
                Attack_Field.SetModeType(UserControl1.game_mode.online_game);
                game_field.SetModeType(UserControl1.game_mode.online_game);
            }
            this.IsEnabled = true;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            game_field.Clear();
            Attack_Field.Clear();
            Attack_Field.SetFieldType(UserControl1.type_field.enemy_field);
            Attack_Field.IsEnabled = false;
        }

    }
}
