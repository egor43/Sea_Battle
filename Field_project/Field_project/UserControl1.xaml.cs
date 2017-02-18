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
        private Unit[,] matrix_state = new Unit [10,10];


        public enum type_field
        {
            set_field = 0,
            user_field = 1,
            enemy_field = 2
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
                    if (i == 2 && j == 1) matrix_state[2, 1] = new Unit(unit_type.ship);
                    if (i == 2 && j == 2) matrix_state[2, 2] = new Unit(unit_type.ship);
                    if (i == 2 && j == 3) matrix_state[2, 3] = new Unit(unit_type.ship);
                    if (i == 2 && j == 4) matrix_state[2, 4] = new Unit(unit_type.ship);
                                     
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

        private Unit[,] GetMatrixState()
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
                Point[] point = new Point[] { new Point(2, 1), new Point(2, 2), new Point(2, 3), new Point(2, 4) };
                //Меняем фон у ячейки и канваса в котором он лежит
                bool flag = Check_Ship(point, matrix_state, 4);
                unit.Treatment_Shot();
                ((Canvas)sender).Background = new ImageBrush(unit.Get_Image()); //Это шняга тестовая
            }
        }

        private byte Sort_Points(Point[] points)
        {
            byte checktmp = 0; //переменная для определения ориентации корабля
            Point tmp = new Point();

            for (int i = 0; i < points.Length - 1; i++)
            {
                if (points[i].X == points[i + 1].X) checktmp++;
            }

            if (checktmp == points.Length)//Если прокатило с Х
            {
                for (int j = 0; j < points.Length - 1; j++)
                    for (int i = 0; i < points.Length - j; i++)
                    {
                        if (points[i].Y > points[i + 1].Y)
                        {
                            tmp = points[i];
                            points[i] = points[i + 1];
                            points[i + 1] = tmp;
                        }
                    }
                //сортируем по X
                return 1;
            }
            else
            {
                checktmp = 0;
                for (int i = 0; i < points.Length - 1; i++)
                {
                    if (points[i].Y == points[i + 1].Y) checktmp++;
                }
                if (checktmp == points.Length)//Если прокатило с Y
                {
                    for (int j = 0; j < points.Length - 1; j++)
                        for (int i = 0; i < points.Length - j; i++)
                        {
                            if (points[i].X > points[i + 1].X)
                            {
                                tmp = points[i];
                                points[i] = points[i + 1];
                                points[i + 1] = tmp;
                            }
                        }
                    //сортируем по Y
                    return 2;
                }
                else return 0;//Если не прокатило совсем
            }
        }

        private bool Check_Ship(Point[] points, Unit[,] state, byte count_value_ship)
        {
            if (points.Length != count_value_ship) return false;
            byte tmp = Sort_Points(points);

            if (tmp != 0)
            {
                if (tmp == 1)//по X
                {
                    if ((points[0].X + count_value_ship - 1) != (points[points.Length - 1].X)) return false; //Если корабля не имеет разрывов по оси X
                    for (int i = 0; i < points.Length - 1; i++)
                    {
                        if (i == 0)// для левого части корабля
                        {
                            try
                            {
                                double X = points[i].X;
                                double Y = points[i].Y;

                                if (state[(int)X, (int)Y - 1].Get_Unit_Type() != unit_type.sea
                                    || state[(int)X - 1, (int)Y - 1].Get_Unit_Type() != unit_type.sea
                                    || state[(int)X - 1, (int)Y].Get_Unit_Type() != unit_type.sea
                                    || state[(int)X - 1, (int)Y + 1].Get_Unit_Type() != unit_type.sea
                                    || state[(int)X, (int)Y + 1].Get_Unit_Type() != unit_type.sea) return false; //Если соседние ячейки не пустые
                            }
                            catch { }
                        }
                        if (i != 0 && i != points.Length - 1)// для средних части корабля
                        {
                            try
                            {
                                double X = points[i].X;
                                double Y = points[i].Y;

                                if (state[(int)X, (int)Y - 1].Get_Unit_Type() != unit_type.sea
                                    || state[(int)X, (int)Y + 1].Get_Unit_Type() != unit_type.sea) return false; //Если соседние ячейки не пустые
                            }
                            catch { }
                        }
                        if (i == points.Length - 1)// для последней части корабля
                        {
                            try
                            {
                                double X = points[i].X;
                                double Y = points[i].Y;

                                if (state[(int)X, (int)Y - 1].Get_Unit_Type() != unit_type.sea
                                    || state[(int)X + 1, (int)Y - 1].Get_Unit_Type() != unit_type.sea
                                    || state[(int)X + 1, (int)Y].Get_Unit_Type() != unit_type.sea
                                    || state[(int)X + 1, (int)Y + 1].Get_Unit_Type() != unit_type.sea
                                    || state[(int)X, (int)Y + 1].Get_Unit_Type() != unit_type.sea) return false; //Если соседние ячейки не пустые
                            }
                            catch { }
                        }
                    }
                }
                else//по Y
                {
                    if ((points[0].Y + count_value_ship - 1) != (points[points.Length - 1].Y)) return false; //Если корабля не имеет разрывов по оси Y
                    for (int i = 0; i < points.Length - 1; i++)
                    {
                        if (i == 0)// для верхней части корабля
                        {
                            try
                            {
                                double X = points[i].X;
                                double Y = points[i].Y;

                                if (state[(int)X - 1, (int)Y].Get_Unit_Type() != unit_type.sea
                                    || state[(int)X - 1, (int)Y - 1].Get_Unit_Type() != unit_type.sea
                                    || state[(int)X, (int)Y - 1].Get_Unit_Type() != unit_type.sea
                                    || state[(int)X + 1, (int)Y - 1].Get_Unit_Type() != unit_type.sea
                                    || state[(int)X + 1, (int)Y].Get_Unit_Type() != unit_type.sea) return false; //Если соседние ячейки не пустые
                            }
                            catch { }
                        }
                        if (i != 0 && i != points.Length - 1)// для средней части корабля
                        {
                            try
                            {
                                double X = points[i].X;
                                double Y = points[i].Y;

                                if (state[(int)X + 1, (int)Y].Get_Unit_Type() != unit_type.sea
                                    || state[(int)X - 1, (int)Y].Get_Unit_Type() != unit_type.sea) return false; //Если соседние ячейки не пустые
                            }
                            catch { }
                        }
                        if (i == points.Length - 1)// для нижней части корабля
                        {
                            try
                            {
                                double X = points[i].X;
                                double Y = points[i].Y;

                                if (state[(int)X - 1, (int)Y].Get_Unit_Type() != unit_type.sea
                                    || state[(int)X - 1, (int)Y + 1].Get_Unit_Type() != unit_type.sea
                                    || state[(int)X, (int)Y + 1].Get_Unit_Type() != unit_type.sea
                                    || state[(int)X + 1, (int)Y + 1].Get_Unit_Type() != unit_type.sea
                                    || state[(int)X + 1, (int)Y].Get_Unit_Type() != unit_type.sea) return false; //Если соседние ячейки не пустые
                            }
                            catch { }
                        }
                    }
                }
            }
            else return false;

            return true;
        }

        private bool Check_Ship_One(Point[] points, Unit[,] state)//проверка для однопалубного корабля
        {
            try
            {
                double X = points[0].X;
                double Y = points[0].Y;

                if (state[(int)X, (int)Y - 1].Get_Unit_Type() != unit_type.sea
                    || state[(int)X - 1, (int)Y - 1].Get_Unit_Type() != unit_type.sea
                    || state[(int)X - 1, (int)Y].Get_Unit_Type() != unit_type.sea
                    || state[(int)X - 1, (int)Y + 1].Get_Unit_Type() != unit_type.sea
                    || state[(int)X, (int)Y + 1].Get_Unit_Type() != unit_type.sea
                    || state[(int)X + 1, (int)Y + 1].Get_Unit_Type() != unit_type.sea
                    || state[(int)X + 1, (int)Y].Get_Unit_Type() != unit_type.sea
                    || state[(int)X + 1, (int)Y - 1].Get_Unit_Type() != unit_type.sea) return false; 
            }
            catch { }
            return true;

        }


    }
}
