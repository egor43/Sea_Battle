using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Field_project
{
    static class AutoAction
    {
        private static List<Point> ship_points = new List<Point>(); //Список, хранящий ячейки кораблей для их дальнейшей обработки
        private static Ships ships = new Ships(); //Объект обеспечивающий работу с методами по подсчету кораблей
        private static Unit[,] saved_state = new Unit[10, 10]; //Матрица для сохранения промежуточного состояния поля


        public static void AutoSetShips(ref Unit[,] matrix_state)
        {
            ships.Reset();
            Utilits.Save_Matrix(matrix_state, saved_state);
            ship_points.Clear();
            Point random_point = Utilits.GetRandomPoint(); //ставим случайную ячейку
            while (true)
            {
                if (ship_points.Count>0) //Если нет точки для примера ("отправной точки")
                {
                    random_point = Utilits.NormalizedPoint(ship_points.ToArray(), random_point);
                }
                else
                {
                    random_point = Utilits.NormalizedPoint(new Point[] { random_point }, random_point);
                }
                if (matrix_state[(int)random_point.X, (int)random_point.Y].Get_Unit_Type() == unit_type.ship)
                {
                    ship_points.Clear();
                    random_point = Utilits.GetRandomPoint();
                    continue;
                }
                matrix_state[(int)random_point.X, (int)random_point.Y].Set_Unit_Type(unit_type.ship);
                ship_points.Add(random_point);
                int type_ship = ships.Next_Stage(); //Получаем состояние кораблей
                if (type_ship > 0) //Если корабль заполнен
                {
                    if (Utilits.Check_Ship(ship_points.ToArray(), matrix_state, (byte)type_ship)) //Проверка корректности установки ячеек
                    {
                        Utilits.Save_Matrix(matrix_state, saved_state); //Сохраняем состояние поля
                    }
                    else //Если выставленные ячейки не корректны
                    {
                        ships.Back_Stage(); //Откатываем счетчик установки кораблей назад
                        Utilits.Load_Matrix(matrix_state, saved_state); //Загружаем предидущее состояние
                    }
                    ship_points.Clear(); //Очищаем набор точек (временный набор точек, который заполняется при установке ячеек корабля)
                    random_point = Utilits.GetRandomPoint();
                }
                else if (type_ship == -1) //Если установка закончена (нет больше доступных кораблей для установки)
                {
                    Utilits.Load_Matrix(matrix_state, saved_state); //Загружаем матрицу состояния
                    ship_points.Clear(); //Очищаем набор точек
                    ships.Reset();
                    break;
                }
            }          
        }

        public static Point AutoAttack(ref Unit[,] matrix_state)
        {
            Point result = new Point(-1, -1);
            return result;
        
        }
        
    }
}
