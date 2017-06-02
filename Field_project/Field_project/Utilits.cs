﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Field_project
{
    static class Utilits
    {
        public delegate string CheckUnit(int i, int j);

        public static event CheckUnit UnitEvent;

        //Метод переводящий матрицу состояния кораблей в строку, для отправки на сервер
        public static string ParseMatric(Unit[,] state)
        {
            string parsematrix = "";

            for (int i = 0; i < state.GetLength(0); i++)
            {
                for (int j = 0; j < state.GetLength(1); j++)
                {
                    if (state[i, j].Get_Unit_Type() == 0)
                    {
                        parsematrix = parsematrix + "0";
                    }
                    else
                    {
                        parsematrix = parsematrix + "1";
                    }
                }
            }
            return parsematrix;
        }

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

        //Обработка сообщения
        public static string ProcessingMessage(string message, Unit pressed_unit)
        {
            string ret = " ";
            
            if( message == "non" )
            {
                return ret;
            }

            if (message == "end")
            {
                return ret;
            }

            if (message[2]=='+' || message[2]=='*') //проверяем попали ли мы в корабль врага
            {
                //если мы попали надо отметить это на поле врага и подготовить сообщение к следующему ходу
                pressed_unit.Set_Unit_Type(unit_type.hit_ship);
                ret = " ";
            }
            else
            {

                int x = Int32.Parse(message[0].ToString());
                int y = Int32.Parse(message[1].ToString());
                //если мы не попали, отмечаем это на поле врага и проверяем, куда стрельнул враг и подготовить сообщение исходя из того, куда попал враг
                pressed_unit.Set_Unit_Type(unit_type.hit_sea);
                ret = UnitEvent(x, y); // Запили проверку на Null
            }
            return ret;
        }

        //Парсинг строки приходящих ударов от противника
        public static string ProcessingOnlineMessage(string message)
        {
            string ret = " ";

            if (message == "non")
            {
                return ret;
            }
            else
            {
                for (int i = 0; i < message.Length; i += 2)
                {
                    int x = Int32.Parse(message[i].ToString());
                    int y = Int32.Parse(message[i+1].ToString());

                    ret = UnitEvent(x, y); // Запили проверку на Null
                }
            }

            return ret;
        }

        // Достигли ли мы конца игры
        public static bool IsEndGame(Unit[,] matrix_state)
        {
            bool is_end = true;
            // рассматриваем матрицу состояния
            for(int line=0; line<matrix_state.GetLength(0); line++)
            {
                for (int column = 0; column < matrix_state.GetLength(1); column++)
                {
                    // ищем в ней целые кусочки кораблей
                    if (matrix_state[line, column].Get_Unit_Type() == unit_type.ship) is_end = false; // если нашли - значит еще не конец игры
                }
            }              
            return is_end;
        }
    }
}
