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
        public game_mode mode_game = game_mode.offline_game; //Режим игры
        private Ships ships = new Ships(); //Объект обеспечивающий работу с методами по подсчету кораблей
        List<Point> ship_points = new List<Point>(); //Список, хранящий ячейки кораблей для их дальнейшей обработки
        private Unit[,] saved_state = new Unit[10, 10]; //Матрица для сохранения промежуточного состояния поля
        Computer comp_ii; //Компьютерный недоинтеллект
        string message = ""; // Строка для общения с ИИ и онлайном


        //Режимы игры
        public enum game_mode
        {
            offline_game=0,
            online_game=1
        }

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
            if(mode_game==game_mode.offline_game) //Если офлайн режим
            {
                comp_ii = new Computer();
            }

        }

        // Обрабатывает нажатия на ячейку в зависимости от режима поля (режимы: установка кораблей, поле врага, поле игрока)
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
                        if (Utilits.Check_Ship(ship_points.ToArray(), matrix_state, (byte)type_ship)) //Проверка корректности установки ячеек
                        {
                            Utilits.Save_Matrix(matrix_state, saved_state); //Сохраняем состояние поля
                        }
                        else //Если выставленные ячейки не корректны
                        {
                            ships.Back_Stage(); //Откатываем счетчик установки кораблей назад
                            Utilits.Load_Matrix(matrix_state, saved_state); //Загружаем предидущее состояние
                            Initinitialization_Field(Unit.Get_Size_Unit(), MyCanvas_MouseLeftButtonUp, matrix_state); //Перерисовываем поле
                        }
                        ship_points.Clear(); //Очищаем набор точек (временный набор точек, который заполняется при установке ячеек корабля)
                    }
                    else if (type_ship==-1) //Если установка закончена (нет больше доступных кораблей для установки)
                    {
                        Utilits.Load_Matrix(matrix_state, saved_state); //Загружаем матрицу состояния
                        ship_points.Clear(); //Очищаем набор точек
                        Initinitialization_Field(Unit.Get_Size_Unit(), MyCanvas_MouseLeftButtonUp, matrix_state); //Перерисовываем поле
                        field_type = type_field.user_field; // Меняем тип поля
                        grid.IsEnabled = false; // Блокируем поле, т.к. все уже выставлено и оно больше не будет изменяться игроком
                    }
                    break;

                case type_field.enemy_field: //Если тип поля "поле врага"
                    if (unit.Get_Unit_Type() != unit_type.sea) throw new Exception("Не туда ткнул");
                    if(mode_game==game_mode.offline_game)
                    {       
                        // отдать координаты по которым ткунули в класс ИИ
                        // получить ответ от класса ИИ
                    }
                    else
                    {
                        // Игра по сети
                    }
                    break;

                case type_field.user_field: //Если тип поля "поле игрока"
                    break;
            }
        }       

        // Авто-заполнение поля
        public void AutoSetShips()
        {
            Initinitialization_Field(Unit.Get_Size_Unit(), MyCanvas_MouseLeftButtonUp); // Инициализируем матрицу по новой, чтобы она была "чистой"
            AutoAction.AutoSetShips(ref matrix_state);
            Initinitialization_Field(Unit.Get_Size_Unit(), MyCanvas_MouseLeftButtonUp, matrix_state); // Отрисовка новой, заполненной матрицы
            grid.IsEnabled = false; // Блокируем поле, т.к. все уже выставлено и оно больше не будет изменяться игроком
        }
    }
}
