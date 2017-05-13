using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Field_project
{
    class Computer
    {
        Unit[,] Matrix_Com_My;
        Unit[,] Matrix_Com_Enemy;
        public Computer()
        {
            Unit[,] Matrix_Com_My = new Unit[10,10];
            AutoAction.AutoSetShips(ref Matrix_Com_My);

            Unit[,] Matrix_Com_Enemy = new Unit[10, 10];
            for (int i = 0; i < Matrix_Com_Enemy.GetLength(0); i++)
                for (int j = 0; j < Matrix_Com_Enemy.GetLength(1); j++)
                    Matrix_Com_Enemy[i, j].Set_Unit_Type(unit_type.sea);

        }

        public string GetCoordinat(string attach)
        {
            string answer = "";
            int x = 0, y = 0;
            x = (int)attach[0];
            y = (int)attach[1];


            if (Matrix_Com_My[x, y].Get_Unit_Type() == unit_type.sea)
            {
                Point tmp = AutoAction.AutoAttack(ref Matrix_Com_Enemy);
                answer = answer + tmp.X.ToString();
                answer = answer + tmp.Y.ToString();
                answer = answer + "-";
            }
            if (Matrix_Com_My[x, y].Get_Unit_Type() == unit_type.ship)
                answer = answer + "+++";

            return answer;
        }
    }
}
