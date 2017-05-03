using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Field_project
{
    static class Utilits
    {
       
        //Проверка корректности расположения корабля
        public static bool Check_Ship(Point[] points, Unit[,] state, byte count_value_ship)
        {
            if (points.Length != count_value_ship) return false;
            if (count_value_ship == 1) return (Check_Ship_One(points, state));
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
                                if (state[(int)I, (int)J - 1].Get_Unit_Type() != unit_type.sea) return false; //Если соседние ячейки не пустые
                            }
                            catch { }
                            try
                            {
                                if (state[(int)I - 1, (int)J - 1].Get_Unit_Type() != unit_type.sea) return false;
                            }
                            catch { }
                            try
                            {
                                if (state[(int)I - 1, (int)J].Get_Unit_Type() != unit_type.sea) return false;
                            }
                            catch { }
                            try
                            {
                                if (state[(int)I - 1, (int)J + 1].Get_Unit_Type() != unit_type.sea) return false;
                            }
                            catch { }
                            try
                            {
                                if (state[(int)I, (int)J + 1].Get_Unit_Type() != unit_type.sea) return false;
                            }
                            catch { }
                        }
                        if (i != 0 && i != points.Length - 1)// для средних части корабля
                        {
                            try
                            {
                                if (state[(int)I, (int)J - 1].Get_Unit_Type() != unit_type.sea) return false; //Если соседние ячейки не пустые
                            }
                            catch { }
                            try
                            {
                                if (state[(int)I, (int)J + 1].Get_Unit_Type() != unit_type.sea) return false;
                            }
                            catch { }
                        }
                        if (i == points.Length - 1)// для нижней части корабля
                        {
                            try
                            {
                                if (state[(int)I, (int)J - 1].Get_Unit_Type() != unit_type.sea) return false; //Если соседние ячейки не пустые
                            }
                            catch { }
                            try
                            {
                                if (state[(int)I + 1, (int)J - 1].Get_Unit_Type() != unit_type.sea) return false;
                            }
                            catch { }
                            try
                            {
                                if (state[(int)I + 1, (int)J].Get_Unit_Type() != unit_type.sea) return false;
                            }
                            catch { }
                            try
                            {
                                if (state[(int)I + 1, (int)J + 1].Get_Unit_Type() != unit_type.sea) return false;
                            }
                            catch { }
                            try
                            {
                                if (state[(int)I, (int)J + 1].Get_Unit_Type() != unit_type.sea) return false;
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
                                if (state[(int)I - 1, (int)J - 1].Get_Unit_Type() != unit_type.sea) return false; //Если соседние ячейки не пустые
                            }
                            catch { }
                            try
                            {
                                if (state[(int)I + 1, (int)J - 1].Get_Unit_Type() != unit_type.sea) return false;
                            }
                            catch { }
                            try
                            {
                                if (state[(int)I + 1, (int)J].Get_Unit_Type() != unit_type.sea) return false;
                            }
                            catch { }
                            try
                            {
                                if (state[(int)I - 1, (int)J].Get_Unit_Type() != unit_type.sea) return false;
                            }
                            catch { }
                            try
                            {
                                if (state[(int)I, (int)J - 1].Get_Unit_Type() != unit_type.sea) return false;
                            }
                            catch { }
                        }
                        if (i != 0 && i != points.Length - 1)// для средней части корабля
                        {
                            try
                            {
                                if (state[(int)I - 1, (int)J].Get_Unit_Type() != unit_type.sea) return false; //Если соседние ячейки не пустые
                            }
                            catch { }
                            try
                            {
                                if (state[(int)I + 1, (int)J].Get_Unit_Type() != unit_type.sea) return false;
                            }
                            catch { }
                        }
                        if (i == points.Length - 1)// для правой части корабля
                        {
                            try
                            {
                                if (state[(int)I - 1, (int)J].Get_Unit_Type() != unit_type.sea) return false; //Если соседние ячейки не пустые
                            }
                            catch { }
                            try
                            {
                                if (state[(int)I - 1, (int)J + 1].Get_Unit_Type() != unit_type.sea) return false;
                            }
                            catch { }
                            try
                            {
                                if (state[(int)I, (int)J + 1].Get_Unit_Type() != unit_type.sea) return false;
                            }
                            catch { }
                            try
                            {
                                if (state[(int)I + 1, (int)J + 1].Get_Unit_Type() != unit_type.sea) return false;
                            }
                            catch { }
                            try
                            {
                                if (state[(int)I + 1, (int)J].Get_Unit_Type() != unit_type.sea) return false;
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
        private static bool Check_Ship_One(Point[] points, Unit[,] state)
        {
            double I = points[0].X;
            double J = points[0].Y;

            try
            {
                if (state[(int)I, (int)J - 1].Get_Unit_Type() != unit_type.sea) return false;
            }
            catch { }
            try
            {
                if (state[(int)I - 1, (int)J - 1].Get_Unit_Type() != unit_type.sea) return false;
            }
            catch { }
            try
            {
                if (state[(int)I - 1, (int)J].Get_Unit_Type() != unit_type.sea) return false;
            }
            catch { }
            try
            {
                if (state[(int)I - 1, (int)J + 1].Get_Unit_Type() != unit_type.sea) return false;
            }
            catch { }
            try
            {
                if (state[(int)I, (int)J + 1].Get_Unit_Type() != unit_type.sea) return false;
            }
            catch { }
            try
            {
                if (state[(int)I + 1, (int)J - 1].Get_Unit_Type() != unit_type.sea) return false;
            }
            catch { }
            try
            {
                if (state[(int)I + 1, (int)J].Get_Unit_Type() != unit_type.sea) return false;
            }
            catch { }
            try
            {
                if (state[(int)I + 1, (int)J + 1].Get_Unit_Type() != unit_type.sea) return false;
            }
            catch { }
            try
            {
                if (state[(int)I, (int)J + 1].Get_Unit_Type() != unit_type.sea) return false;
            }
            catch { }

            return true;
        }

        // Cортирует массив входящих точек для дальнейшей обработки в методе проверки расположения точек
        public static byte Sort_Points(Point[] points)
        {
            byte checktmp = 0; //переменная для определения ориентации корабля
            Point tmp = new Point();

            for (int i = 0; i < points.Length - 1; i++)
            {
                if (points[i].X == points[i + 1].X) checktmp++;
            }

            if (checktmp == points.Length - 1)//Если прокатило с Х
            {
                for (int j = 0; j < points.Length - 1; j++)
                    for (int i = 0; i < points.Length - 1 - j; i++)
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
                if (checktmp == points.Length - 1)//Если прокатило с Y
                {
                    for (int j = 0; j < points.Length - 1; j++)
                        for (int i = 0; i < points.Length - 1 - j; i++)
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

        // Выдает рандомную точку на матрице в диапазоне от 0 до max_сoordinate
        public static Point GetRandomPoint(int max_coordinate=10)
        {
            Point result = new Point(-1, -1);
            Random random_x = new Random();
            result.X = random_x.Next(max_coordinate);
            System.Threading.Thread.Sleep(50);
            Random random_y = new Random();
            result.Y = random_y.Next(max_coordinate);
            return result;
        }

        // Приводит точку point_change к соседней с good_point. Предварительно определяя ориентацию точек. Чтобы они шли в одну линию.
        public static Point NormalizedPoint(Point[] good_point, Point point_change)
        {
            Point result = new Point();
            bool IsHorizontal = (int)point_change.X/2==0; // Рандомное начальное значение

            if (good_point.Length!=1)
            {
                IsHorizontal = good_point[0].X==good_point[1].X; //Проверяем ориентацию корабля
            }

            if (!IsHorizontal) //Если горабль расположен горизонтально
            {
                if (good_point[good_point.Length-1].X != 9) //Если есть куда прибавлять
                {
                    result = good_point[good_point.Length - 1];
                    result.X += 1;
                }
                else
                {
                    result = good_point[0];
                    result.X -= 1;
                }                
            }
            else
            {
                if (good_point[good_point.Length-1].Y != 9)
                {
                    result = good_point[good_point.Length - 1];
                    result.Y += 1;
                }
                else
                {
                    result = good_point[0];
                    result.Y -= 1;
                }
            }

            return result;
        }

        //Сохранение матрицы состояния
        public static void Save_Matrix(Unit[,] matrix_state, Unit[,] saved_state)
        {
            for (int i = 0; i < matrix_state.GetLength(0); i++)
            {
                for (int j = 0; j < matrix_state.GetLength(1); j++)
                {
                    saved_state[i, j] = new Unit(matrix_state[i, j].Get_Unit_Type(), matrix_state[i, j].Get_Position_I(), matrix_state[i, j].Get_Position_J());
                }
            }
        }

        //Загрузка матрицы состояния
        public static void Load_Matrix(Unit[,] matrix_state, Unit[,] saved_state)
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
