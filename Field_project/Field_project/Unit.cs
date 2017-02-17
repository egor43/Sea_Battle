using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

//  Проект "Морской бой" в рамках предмета "Технология программирования" 
//  Мышко Е.В.  
//  Шамхалов Р.М. 
//  2017г. 
// 
//  Класс Unit представляет базовую единицу игрового поля - ячейку.
//  В данном классе реализуется вся логика поведения ячейки.
//
//  Перечисление unit_type представляет тип ячейки. Является открытым для  
//  предоставления возможности программисту при создании объекта класса
//  не использовать "магические константы", а наглядно видеть тип ячейки
//
//


public enum unit_type //Определяет тип ячейки (море, корабль, попадание в море, попадание в корабль)
{
    sea = 0,
    ship = 1,
    hit_sea = 2,
    hit_ship = 3
}

public class Unit
{

    //Перечисления класса:
    

    //Константы класса:
    private const int SIZE_UNIT = 30; //Размер ячейки (SIZE_UNIT x SIZE_UNIT)

    //Переменные класса:
    private BitmapImage image; //Картинка ячейки
    private unit_type type_unit; //Тип ячейки


    //Конструкторы:
    public Unit (unit_type type_unit)
    {
        try
        {
            this.type_unit = type_unit;
        }
        catch(Exception)
        {
            throw new ArgumentException("Invalid argument");
        }
        try
        {
            switch (type_unit) //В свитче устанавливаем ячейке картинку в зависимости от переданного аргумента type_unit
            {                  
                case unit_type.sea:
                    this.image = new BitmapImage(new Uri("sea.png", UriKind.Relative)); //Используем относитьельные пути к картинкам (т.е. они должны рядом с экзешником лежать)
                    break;
                case unit_type.ship:
                    this.image = new BitmapImage(new Uri("КАРТИНКА С КОРАБЛЕМ", UriKind.Relative)); //Добавить картинку с кораблем
                    break;
                case unit_type.hit_sea:
                    this.image = new BitmapImage(new Uri("КАРТИНКА С ДЫРКОЙ В МОРЕ", UriKind.Relative)); //Добавить картинку с попаданием в море
                    break;
                case unit_type.hit_ship:
                    this.image = new BitmapImage(new Uri("КАРТИНКА С ПОВРЕЖДЕНИЕМ КОРАБЛЯ", UriKind.Relative)); //Добавить картинку с попаданием в корабль
                    break;
            }
        }
        catch (Exception)
        {
            throw new UriFormatException("Missing image of the unit");
        }
    }

    //Public методы:
    public void Set_Image(BitmapImage image)
    {
        this.image = image;
    }

    public void Set_Image(Uri image)
    {
        this.image = new BitmapImage(image);
    }

    public BitmapImage Get_Image()
    {
        return this.image;
    }

    public static int Get_Size_Unit()
    {
        return SIZE_UNIT;
    }

    public unit_type Get_Unit_Type()
    {
        return type_unit;
    }

    public void Set_Unit_Type(unit_type type_unit)
    {
        this.type_unit = type_unit;
    }

    public void Treatment_Shot()
    {
        switch(type_unit)
        {
            case unit_type.sea:
                {

                    break;
                }
            case unit_type.ship:
                {

                    break;
                }
            default:
            {
                    throw new InvalidOperationException("Trying to change a modified cell");
            }


        }
        //Обработка попадания и соответсвующее изменение картинки для ячейки.
    }
}
