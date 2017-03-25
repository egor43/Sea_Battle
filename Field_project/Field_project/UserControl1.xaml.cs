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
        //Переменные класса пользовательского элемента Field ("игровое поле")

        private Unit[,] matrix_state = new Unit [10,10]; //Матрица состояния игрового поля
        public type_field field_type = type_field.set_field; //Тип игрового поля
        private Ships ships = new Ships(); //Объект обеспечивающий работу с методами по подсчету кораблей
        List<Point> ship_points = new List<Point>(); //Список, хранящий ячейки кораблей для их дальнейшей обработки
        private Unit[,] saved_state = new Unit[10, 10]; //Матрица для сохранения промежуточного состояния поля

        //Перечисление типов поля set_field - поле установки кораблей, user_field - поле игрока, enemy_field - поле врага
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
                    saved_state[i,j]= new Unit(unit_type.sea, i, j);
                    matrix_state[i, j] = new Unit(unit_type.sea,i,j);
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

        //Метод заполняет, "расчерченные" методом Initinitialization_Grid, пустые ячейки элементами Canvas, которые содержат внутри себя объекты класса Unit из входящей матрицы unit_arr
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

        //Возвращает матрицу состояния
        private Unit[,] GetMatrixState()
        {
            return matrix_state;
        }

        public UserControl1()
        {
            InitializeComponent();
            window.Width = Unit.Get_Size_Unit() * 10; //Устанавливаем размер поля шириной равной 10 ячейкам
            window.Height = Unit.Get_Size_Unit() * 10; //Устанавливаем размер поля высотой равной 10 ячейкам
            Initinitialization_Grid(Unit.Get_Size_Unit()); //Проводим ""расчерчивание поля
            Initinitialization_Field(Unit.Get_Size_Unit(), MyCanvas_MouseLeftButtonUp); //Проводим инициализацию поля
        }

        private void MyCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Unit unit = (Unit)((Canvas)sender).Tag; //Вытаскиваем ячейку из вызвавшего элемента
            switch (field_type)
            {
                case type_field.set_field: //Если тип поля "установка кораблей."
                    ship_points.Add(new Point(unit.Get_Position_I(), unit.Get_Position_J())); //Добавляем нажатую ячейку в список кораблей для дальнейшей проверки
                    if (unit.Get_Unit_Type() != unit_type.sea) throw new Exception("Не туда ткнул");
                    unit.Set_Unit_Type(unit_type.ship); //Устанавливаем тип ячейки - "корабль"
                    ((Canvas)sender).Background = new ImageBrush(unit.Get_Image()); //Устанавливаем соответствующую картинку в ячейку
                    int type_ship = ships.Next_Stage(); //Получаем состояние кораблей
                    if (type_ship>0) //Если корабль заполнен
                    {
                        if (Check_Ship(ship_points.ToArray(), matrix_state, (byte)type_ship)) //Проверка корректности установки ячеек
                        {
                            Save_Matrix(matrix_state); //Сохраняем состояние поля
                        }
                        else //Если выставленные ячейки не корректны
                        {
                            ships.Back_Stage(); //Откатываем счетчик установки кораблей назад
                            Load_Matrix(); //Загружаем предидущее состояние
                            Initinitialization_Field(Unit.Get_Size_Unit(), MyCanvas_MouseLeftButtonUp, matrix_state); //Перерисовываем поле
                        }
                        ship_points.Clear(); //Очищаем набор точек (временный набор точек, который заполняется при установке ячеек корабля)
                    }
                    else if (type_ship==-1) //Если установка закончена (нет больше доступных кораблей для установки)
                    {
                        Load_Matrix(); //Загружаем матрицу состояния
                        ship_points.Clear(); //Очищаем набор точек
                        Initinitialization_Field(Unit.Get_Size_Unit(), MyCanvas_MouseLeftButtonUp, matrix_state); //Перерисовываем поле
                    }
                    break;

                case type_field.enemy_field: //Если тип поля "поле врага"
                    break;

                case type_field.user_field: //Если тип поля "поле игрока"
                    break;
            }
        }

        //Метод сортировки массива точек
        private byte Sort_Points(Point[] points)
        {
            byte checktmp = 0; //переменная для определения ориентации корабля
            Point tmp = new Point();

            for (int i = 0; i < points.Length - 1; i++)
            {
                if (points[i].X == points[i + 1].X) checktmp++;
            }

            if (checktmp == points.Length-1)//Если прокатило с Х
            {
                for (int j = 0; j < points.Length - 1; j++)
                    for (int i = 0; i < points.Length -1- j; i++)
                    {
                        if (points[i].Y > points[i + 1].Y)
                        {
                            tmp = points[i];
                            points[i] = points[i + 1];
                            points[i + 1] = tmp;
                        }
                    }
                //сортируем по X
                return 1; //Горизонтально
            }
            else
            {
                checktmp = 0;
                for (int i = 0; i < points.Length - 1; i++)
                {
                    if (points[i].Y == points[i + 1].Y) checktmp++;
                }
                if (checktmp == points.Length-1)//Если прокатило с Y
                {
                    for (int j = 0; j < points.Length - 1; j++)
                        for (int i = 0; i < points.Length -1- j; i++)
                        {
                            if (points[i].X > points[i + 1].X)
                            {
                                tmp = points[i];
                                points[i] = points[i + 1];
                                points[i + 1] = tmp;
                            }
                        }
                    //сортируем по Y
                    return 2;//Вертикально
                }
                else return 0;//Если не прокатило совсем
            }
        }

        //Проверка корректности расположения корабля
        private bool Check_Ship(Point[] points, Unit[,] state, byte count_value_ship)
        {
            if (points.Length != count_value_ship) return false;
            if (count_value_ship == 1) return(Check_Ship_One(points, state));
            byte tmp = Sort_Points(points);

            if (tmp != 0)
            {
                if (tmp == 2)//Вертикально
                {
                    if ((points[0].X + count_value_ship - 1) != (points[points.Length - 1].X)) return false; //Если корабля не имеет разрывов по оси X
                    for (int i = 0; i < points.Length; i++)
                    {
                        double I = points[i].X;
                        double J = points[i].Y;

                        if (i == 0)// для верхней части корабля
                        {
                            try
                            {
                                

                                if (state[(int)I, (int)J - 1].Get_Unit_Type() != unit_type.sea
                                    || state[(int)I - 1, (int)J - 1].Get_Unit_Type() != unit_type.sea
                                    || state[(int)I-1, (int)J].Get_Unit_Type() != unit_type.sea
                                    || state[(int)I- 1, (int)J+ 1].Get_Unit_Type() != unit_type.sea
                                    || state[(int)I, (int)J+1].Get_Unit_Type() != unit_type.sea) return false; //Если соседние ячейки не пустые
                            }
                            catch { }
                        }
                        if (i != 0 && i != points.Length - 1)// для средних части корабля
                        {
                            try
                            {
                              

                                if (state[(int)I, (int)J-1].Get_Unit_Type() != unit_type.sea
                                    || state[(int)I, (int)J+1].Get_Unit_Type() != unit_type.sea) return false; //Если соседние ячейки не пустые
                            }
                            catch { }
                        }
                        if (i == points.Length - 1)// для нижней части корабля
                        {
                            try
                            {
                               

                                if (state[(int)I, (int)J-1].Get_Unit_Type() != unit_type.sea
                                    || state[(int)I+1, (int)J-1].Get_Unit_Type() != unit_type.sea
                                    || state[(int)I+1 , (int)J].Get_Unit_Type() != unit_type.sea
                                    || state[(int)I+1, (int)J + 1].Get_Unit_Type() != unit_type.sea
                                    || state[(int)I, (int)J+1].Get_Unit_Type() != unit_type.sea) return false; //Если соседние ячейки не пустые
                            }
                            catch { }
                        }
                    }
                }
                else//Горизонтально
                {
                    if ((points[0].Y + count_value_ship - 1) != (points[points.Length - 1].Y)) return false; //Если корабля не имеет разрывов по оси Y
                    for (int i = 0; i < points.Length; i++)
                    {
                        double I = points[i].X;
                        double J = points[i].Y;

                        if (i == 0)// для левой части корабля
                        {
                            try
                            {
                                

                                if (state[(int)I-1, (int)J].Get_Unit_Type() != unit_type.sea
                                    || state[(int)I-1, (int)J - 1].Get_Unit_Type() != unit_type.sea
                                    || state[(int)I, (int)J - 1].Get_Unit_Type() != unit_type.sea
                                    || state[(int)I + 1, (int)J - 1].Get_Unit_Type() != unit_type.sea
                                    || state[(int)I + 1, (int)J].Get_Unit_Type() != unit_type.sea) return false; //Если соседние ячейки не пустые
                            }
                            catch { }
                        }
                        if (i != 0 && i != points.Length - 1)// для средней части корабля
                        {
                            try
                            {
                               

                                if (state[(int)I-1, (int)J].Get_Unit_Type() != unit_type.sea
                                    || state[(int)I+1, (int)J].Get_Unit_Type() != unit_type.sea) return false; //Если соседние ячейки не пустые
                            }
                            catch { }
                        }
                        if (i == points.Length - 1)// для правой части корабля
                        {
                            try
                            {
                               

                                if (state[(int)I-1, (int)J].Get_Unit_Type() != unit_type.sea
                                    || state[(int)I-1, (int)J + 1].Get_Unit_Type() != unit_type.sea
                                    || state[(int)I, (int)J + 1].Get_Unit_Type() != unit_type.sea
                                    || state[(int)I + 1, (int)J + 1].Get_Unit_Type() != unit_type.sea
                                    || state[(int)I + 1, (int)J].Get_Unit_Type() != unit_type.sea) return false; //Если соседние ячейки не пустые
                            }
                            catch { }
                        }
                    }
                }
            }
            else return false;

            return true;
        }

        //Проверка корректности установки однопалубного корабля
        private bool Check_Ship_One(Point[] points, Unit[,] state)
        {
            try
            {
                double I = points[0].X;
                double J = points[0].Y;

                if (state[(int)I, (int)J - 1].Get_Unit_Type() != unit_type.sea
                    || state[(int)I - 1, (int)J - 1].Get_Unit_Type() != unit_type.sea
                    || state[(int)I - 1, (int)J].Get_Unit_Type() != unit_type.sea
                    || state[(int)I - 1, (int)J + 1].Get_Unit_Type() != unit_type.sea
                    || state[(int)I, (int)J + 1].Get_Unit_Type() != unit_type.sea
                    || state[(int)I + 1, (int)J - 1].Get_Unit_Type() != unit_type.sea
                    || state[(int)I + 1, (int)J].Get_Unit_Type() != unit_type.sea
                    || state[(int)I + 1, (int)J + 1].Get_Unit_Type() != unit_type.sea
                    || state[(int)I, (int)J + 1].Get_Unit_Type() != unit_type.sea) return false; //Если соседние ячейки не пустые
            }
            catch { }
            return true;
        }

        //Сохранение матрицы состояния
        private void Save_Matrix(Unit[,] matrix_state)
        {
            for(int i=0; i<matrix_state.GetLength(0);i++)
            {
                for(int j=0; j<matrix_state.GetLength(1);j++)
                {
                    saved_state[i,j] = new Unit(matrix_state[i, j].Get_Unit_Type(), matrix_state[i, j].Get_Position_I(), matrix_state[i, j].Get_Position_J());
                }
            }
        }

        //Загрузка матрицы состояния
        private void Load_Matrix()
        {
            for (int i = 0; i < saved_state.GetLength(0); i++)
            {
                for (int j = 0; j < saved_state.GetLength(1); j++)
                {
                    matrix_state[i, j] = new Unit(saved_state[i, j].Get_Unit_Type(), saved_state[i, j].Get_Position_I(), saved_state[i, j].Get_Position_J());
                }
            }
        }
    }
}
