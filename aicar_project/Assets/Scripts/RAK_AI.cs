using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAK_AI
{
    class Rak_Ai
    {
        static void Main(string[] args)
        {
        }


        static public float sigmoid(float z)
        {
            return Convert.ToSingle(1 / (1 + Math.Exp(-z)));
        }


    }
}
