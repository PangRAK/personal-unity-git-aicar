using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAK_AI
{
    class Rak_Ai
    {
        
        static public float sigmoid(float z)
        {
            return Convert.ToSingle(1 / (1 + Math.Exp(-z)));
        }

        static public float[] softmax(float[] z)
        {
            return new float[] { z[0] / (z[0] + z[1]), z[1] / (z[0] + z[1]) };
        }

        static public float[] predict(float[] x, float[,] w1, float[,] w2, float[] b1, float[] b2)
        {
            float[] s1 = new float[4];
            float[] s2 = new float[2];
            float[] r1 = new float[4];
            float[] r2 = new float[2];

            // 입력층 -> 은닉층 계산
            for (int i = 0; i < 4; i++)
            {
                s1[i] = (w1[i,0] * x[0]) + (w1[i,1] * x[1]) + (w1[i,2] * x[2]) + (w1[i,3] * x[3]) + (w1[i,4] * x[4]) + b1[i];
                r1[i] = sigmoid(s1[i]);
            }
            // 은닉층 -> 출력층 계산
            for (int i = 0; i < 2; i++)
            {
                s2[i] = (w2[i,0] * r1[0]) + (w2[i,1] * r1[1]) + (w2[i,2] * r1[2]) + (w2[i,3] * r1[3]) + b2[i];
                
            }
            r2 = softmax(s2);

            return r2;
        }
    }
}
