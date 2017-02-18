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

namespace Field_project
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        private Unit[,] matrix_state;


        public enum type_field
        {
            set_field =0,
            user_field =1,
            enemy_field=2
        }

        //Метод "расчерчивает" грид на пустые ячейки.
        private void Initinitialization_Grid(double size_unit)
        {
            for (int i = 0; i < 10; i++) //В цикле создаем строки и колонки величиной size_unit*size_unit  (размер матрицы 10*10)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(size_unit, GridUnitType.Auto) });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(size_unit, GridUnitType.Auto) });
            }
        }

        //Метод заполняет, "расчерченные" методом Initinitialization_Grid, пустые ячейки элементами Canvas, которые содержат внутри себя объекты класса Unit
        private void Initinitialization_Field(double size_unit, MouseButtonEventHandler handler)
        {
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {
                    matrix_state[i, j] = new Unit(unit_type.sea);                   
                    Canvas my_canvas = new Canvas() {Width = size_unit, Height= size_unit };
                    my_canvas.Tag = matrix_state[i, j]; //Запихиваем в Tag канваса объект Unit
                    ImageBrush my_img_brush = new ImageBrush(matrix_state[i, j].Get_Image()); //Подгатавливаем изображением ячейки для использования в качестве фона для канваса
                    my_canvas.Background = my_img_brush;
                    my_canvas.MouseLeftButtonUp += handler;
                    grid.Children.Add(my_canvas);
                    Grid.SetColumn(my_canvas, j); //Установка канваса в нужную ячейку "расчерченной" матрицы.
                    Grid.SetRow(my_canvas, i);    //
                }
        }

        private void Initinitialization_Field(double size_unit, MouseButtonEventHandler handler, Unit[,] unit_arr)
        {
            matrix_state = unit_arr;
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {
                    Canvas my_canvas = new Canvas() { Width = size_unit, Height = size_unit };
                    my_canvas.Tag = matrix_state[i, j]; //Запихиваем в Tag канваса объект Unit
                    ImageBrush my_img_brush = new ImageBrush(matrix_state[i, j].Get_Image()); //Подгатавливаем изображением ячейки для использования в качестве фона для канваса
                    my_canvas.Background = my_img_brush;
                    my_canvas.MouseLeftButtonUp += handler;
                    grid.Children.Add(my_canvas);
                    Grid.SetColumn(my_canvas, j); //Установка канваса в нужную ячейку "расчерченной" матрицы.
                    Grid.SetRow(my_canvas, i);    //
                }
        }

        private Unit [,] GetMatrixState()
        {
            return matrix_state;
        }

        public UserControl1()
        {
            InitializeComponent();
            window.Width = Unit.Get_Size_Unit() * 10;
            window.Height = Unit.Get_Size_Unit() * 10;
            Initinitialization_Grid(Unit.Get_Size_Unit());
            Initinitialization_Field(Unit.Get_Size_Unit(), MyCanvas_MouseLeftButtonUp);
        }

        private void MyCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Unit unit = (Unit)((Canvas)sender).Tag;
            if ((unit.Get_Unit_Type() == unit_type.sea)  || (unit.Get_Unit_Type() == unit_type.ship))
            {
                // Меняем фон у ячейки и канваса в котором он лежит    
                unit.Treatment_Shot();
                ((Canvas)sender).Background = new ImageBrush(unit.Get_Image()); //Это шняга тестовая
            }
        }


    }
}
