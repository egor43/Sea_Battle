using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//  Проект "Морской бой" в рамках предмета "Технология программирования" 
//  Мышко Е.В.  
//  Шамхалов Р.М. 
//  2017г. 
// 
//  Класс Ships представляет методы для работы с игровым полем
//  в случае установки кораблей.
//
//

namespace Field_project
{
    class Ships
    {
        //Переменные класса:
        private int count_ship_4 = 1;
        private int count_ship_3 = 2;
        private int count_ship_2 = 3;
        private int count_ship_1 = 4;

        private int elements_ship_4 = 4;
        private int elements_ship_3 = 3;
        private int elements_ship_2 = 2;
        private int elements_ship_1 = 1;

        //Конструкторы:
        public Ships() { }

        //Public методы:

        //Следующий шаг установки корабля. Метод отсчитывает количество кораблей доступных для установки.
        //Начинает отсчет с самого большого 4-х клеточного корабля.
        //Возвращает: (-1) - если больше нет кораблей, доступных для установки;
        //            (0) - если корабль, доступный для установки, установлен не до конца;
        //            (1) - если корабль, доступный для установки, установлен до конца;
        public int Next_Stage()
        {
            if (count_ship_4 > 0)
            {
                elements_ship_4--;
                if (elements_ship_4 == 0)
                {
                    count_ship_4--;
                    elements_ship_4 = 4;
                    return 1;
                }
                return 0;
            }
            if (count_ship_3 > 0)
            {
                elements_ship_3--;
                if (elements_ship_3 == 0)
                {
                    count_ship_3--;
                    elements_ship_3 = 3;
                    return 1;
                }
                return 0;
            }
            if (count_ship_2 > 0)
            {
                elements_ship_2--;
                if (elements_ship_2 == 0)
                {
                    count_ship_2--;
                    elements_ship_2 = 2;
                    return 1;
                }
                return 0;
            }
            if (count_ship_1 > 0)
            {
                elements_ship_1--;
                if (elements_ship_1 == 0)
                {
                    count_ship_1--;
                    elements_ship_1 = 1;
                    return 1;
                }
                return 0;
            }
            return -1;
        }

        //Восстанавливает состояние счетчиков кораблей и их элементов
        public void Reset()
        {
            count_ship_4 = 1;
            count_ship_3 = 2;
            count_ship_2 = 3;
            count_ship_1 = 4;
            elements_ship_4 = 4;
            elements_ship_3 = 3;
            elements_ship_2 = 2;
            elements_ship_1 = 1;

    }

        //Возвращает состояние кораблей к предыдущему шагу (восстанавливает последний заполненный корабль)
        public void Back_Stage()
        {
            if (count_ship_1 < 4)
            {
                count_ship_1++;
                elements_ship_1 = 1;
            }
            else
            {
                if (count_ship_2 < 3)
                {
                    count_ship_2++;
                    elements_ship_2 = 2;
                }
                else
                {
                    if (count_ship_3 < 2)
                    {
                        count_ship_3++;
                        elements_ship_3 = 3;
                    }
                    else
                    {
                        if (count_ship_4 < 1)
                        {
                            count_ship_4++;
                            elements_ship_4 = 4;
                        }
                    }
                }
            }
        }

    //Private методы:
}
}
