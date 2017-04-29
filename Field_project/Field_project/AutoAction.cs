using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Field_project
{
    class AutoAction
    {
        private int dimension = 0; // Размерность поля

        public AutoAction (int dimension)
        {
            if(0<= dimension)
            {
                this.dimension = dimension;
            }
            else
            {
                dimension = 0;
                throw new ArgumentException("Передана неверная размерность");
            }
        }

        public void AutoSetShips(ref Unit[,] matrix_state)
        {

        }

        
    }
}
