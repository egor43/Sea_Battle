using System;
using System.Drawing;

//  Проект "Морской бой" в рамках предмета "Технология программирования" 
//  Мышко Е.В.  
//  Шамхалов Р.М. 
//  2017г. 
// 
//  Класс Unit представляет базовую единицу игрового поля - ячейку.
//  В данном классе реализуется вся логика поведения ячейки.
//  Основными данными, которыми оперирует данный класс являются координаты.
// 



public class Unit
{

    //Перечисления класса:
    private enum unit_type //Определяет тип ячейки (море, корабль, попадание в море, попадание в корабль)
    {
        sea,
        ship,
        hit_sea,
        hit_ship
    }

    //Константы класса:
    private const int SIZE_UNIT = 10; //Размер ячейки (SIZE_UNIT x SIZE_UNIT)

    //Переменные класса:
    private int x; //Позиция ячейки по координате Х
    private int y; //Позиция ячейки по координате У
    private Bitmap image; //Картинка ячейки


    //Конструкторы:
    public Unit (int position_x, int position_y, unit_type type_unit)
    {
        //Реализация конструктора
    }

    //Public методы:
    public void Set_X(int x)
    {
        //Установка координаты Х
    }

    public void Set_Y(int y)
    {
        //Установка координаты Y
    }

    public int Get_X()
    {
        //Возврат координаты Х
    }

    public int Get_Y()
    {
        //Возврат координаты Y
    }

    public void Set_Image(Bitmam image)
    {
        //Установка картинки
    }

    public Bitmam Get_Image()
    {
        //Возврат картинки
    }

    public int Get_Size_Unit()
    {
        //Возвращает размер ячейки
    }

    public string Get_Unit_Type_Str()
    {
        //Возвращает тип ячейки в строковом представлении
    }

    public void Set_Unit_Type_Str(unit_type type_unit)
    {
        //Получает тип ячейки
    }

    public bool Treatment_Shot(int shot_x, int shot_y)
    {
        //Обработка координат и соответсвующее изменение картинки для ячейки в которую попали данные координаты
        //Вернуть true - если попали в корабль, иначе - false
    }
}
