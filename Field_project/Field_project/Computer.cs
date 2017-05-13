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
        Unit[,] Matrix_Com_My = new Unit[10, 10];
        Unit[,] Matrix_Com_Enemy = new Unit[10, 10];
        public Computer()
        {
            for (int i = 0; i < Matrix_Com_Enemy.GetLength(0); i++)
                for (int j = 0; j < Matrix_Com_Enemy.GetLength(1); j++)
                {
                    Matrix_Com_Enemy[i, j]=new Unit(unit_type.sea,i,j);
                    Matrix_Com_My[i, j]=new Unit(unit_type.sea, i, j);
                }
            AutoAction.AutoSetShips(ref Matrix_Com_My);
            

        }

        public string GetCoordinat(string attach)
        {
            string answer = "";
            int x = 0, y = 0;
            if (attach == "+++" || attach == "***")
            {
               
                Point tmp = AutoAction.AutoAttack(ref Matrix_Com_Enemy);
                answer = answer + tmp.X.ToString();
                answer = answer + tmp.Y.ToString();
                answer = answer + "-";
            }
            else
            {
                x = Int32.Parse(attach[0].ToString());
                y = Int32.Parse(attach[1].ToString());
                if (Matrix_Com_My[x, y].Get_Unit_Type() == unit_type.sea)
                {
                    Point tmp = AutoAction.AutoAttack(ref Matrix_Com_Enemy);
                    answer = answer + tmp.X.ToString();
                    answer = answer + tmp.Y.ToString();
                    answer = answer + "-";
                }
                if (Matrix_Com_My[x, y].Get_Unit_Type() == unit_type.ship)
                    answer = answer + "+++";
            }       
            return answer;
        }
    }
}
