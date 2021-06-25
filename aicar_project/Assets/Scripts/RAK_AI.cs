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
            return Convert.ToSingle(1 / (1 + Math.Exp(z * -2f)));   // 이 부분 나중엔 2를 빼는 거도 고려해보자. 데이터 증폭용
        }

        static public float[] softmax(float[] z)
        {
            float c = Math.Max(z[0], z[1]);
            float[] exp_z = new float[2];
            float sum_exp_z;

            exp_z[0] = (float)Math.Exp(z[0] - c);
            exp_z[1] = (float)Math.Exp(z[1] - c);
            sum_exp_z = exp_z[0] + exp_z[1];

            return new float[] { exp_z[0] / sum_exp_z, exp_z[1] / sum_exp_z };
        }

        static public float[] normalization(float[] x)
        {
            for (int i = 0; i < 5; i++)
            {
                if (x[i] != 0)
                    x[i] = 10f - x[i];
                x[i] /= 10f;
            }
            return x;
        }

        static public float[] predict(float[] x, float[,] w1, float[,] w2, float[] b1, float[] b2)
        {
            float[] s1 = new float[4];
            float[] s2 = new float[2];
            float[] r1 = new float[4];
            float[] r2 = new float[2];

            //UnityEngine.Debug.Log("x1 = " + x[0]);
            //UnityEngine.Debug.Log("x2 = " + x[1]);
            //UnityEngine.Debug.Log("x3 = " + x[2]);
            //UnityEngine.Debug.Log("x4 = " + x[3]);
            //UnityEngine.Debug.Log("x5 = " + x[4]);

            // 입력층 -> 은닉층 계산
            for (int i = 0; i < 4; i++)
            {
                s1[i] = (w1[i,0] * x[0]) + (w1[i,1] * x[1]) + (w1[i,2] * x[2]) + (w1[i,3] * x[3]) + (w1[i,4] * x[4]) - b1[i];
                r1[i] = sigmoid(s1[i]);
            }

            //UnityEngine.Debug.Log("s1 = " + s1[0]);
            //UnityEngine.Debug.Log("s2 = " + s1[1]);
            //UnityEngine.Debug.Log("s3 = " + s1[2]);
            //UnityEngine.Debug.Log("s4 = " + s1[3]);
            //UnityEngine.Debug.Log("r1 = " + r1[0]);
            //UnityEngine.Debug.Log("r2 = " + r1[1]);
            //UnityEngine.Debug.Log("r3 = " + r1[2]);
            //UnityEngine.Debug.Log("r4 = " + r1[3]);

            // 은닉층 -> 출력층 계산
            for (int i = 0; i < 2; i++)
            {
                s2[i] = (w2[i,0] * r1[0]) + (w2[i,1] * r1[1]) + (w2[i,2] * r1[2]) + (w2[i,3] * r1[3]) - b2[i];
            }
            r2 = softmax(s2);

            //UnityEngine.Debug.Log("s1 = " + s2[0]);
            //UnityEngine.Debug.Log("s2 = " + s2[1]);
            //UnityEngine.Debug.Log("r1 = " + r2[0]);
            //UnityEngine.Debug.Log("r2 = " + r2[1]);

            return r2;
        }
    }
}
