﻿using System;
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
            Attack_Field.SetFieldType( UserControl1.type_field.enemy_field );
            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            game_field.AutoSetShips();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            if( MessageBox.Show( "Вы будете играть в сетевом режиме?", "Режим игры",  MessageBoxButton.YesNo) == MessageBoxResult.Yes )
            {
                Attack_Field.SetModeType(UserControl1.game_mode.online_game);
            }
            this.IsEnabled = true;
        }
    }
}
