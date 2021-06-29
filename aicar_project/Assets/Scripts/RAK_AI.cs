using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAK_AI
{
    class Rak_Ai
    {
        static System.Random rand = new System.Random();   // 난수 생성용

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

        static public int[] rankingSelection()
        {
            //30    0.0 ~ 0.3
            //20    0.3 ~ 0.5
            //15    0.5 ~ 0.65
            //10    0.65 ~ 0.75
            //8     0.75 ~ 0.83
            //6     0.83 ~ 0.89
            //5     0.89 ~ 0.94
            //4     0.94 ~ 0.98
            //2     0.98 ~ 1.0
            //0

            int[] selection = new int[4];           // 골라진 4개
            int count = 0;                          // 골라진 갯수
            float arrow = -1f;                      // 룰렛의 화살표
            bool[] flag = { false, false, false, false, false, false, false, false, false };
            while (true)
            {
                arrow = (float)rand.NextDouble(); // 룰렛의 화살표
                if (0f <= arrow && arrow < 0.3f)            // 1등
                {
                    if(flag[0] == false)
                    {
                        selection[count] = 1;
                        flag[0] = true;
                        count++;
                    }
                }
                else if (0.3f <= arrow && arrow < 0.5f)     // 2등
                {
                    if (flag[1] == false)
                    {
                        selection[count] = 2;
                        flag[1] = true;
                        count++;
                    }
                }
                else if (0.5f <= arrow && arrow < 0.65f)    // 3등
                {
                    if (flag[2] == false)
                    {
                        selection[count] = 3;
                        flag[2] = true;
                        count++;
                    }
                }
                else if (0.65f <= arrow && arrow < 0.75f)   // 4등
                {
                    if (flag[3] == false)
                    {
                        selection[count] = 4;
                        flag[3] = true;
                        count++;
                    }
                }
                else if (0.75f <= arrow && arrow < 0.83f)   // 5등
                {
                    if (flag[4] == false)
                    {
                        selection[count] = 5;
                        flag[4] = true;
                        count++;
                    }
                }
                else if (0.83f <= arrow && arrow < 0.89f)   // 6등
                {
                    if (flag[5] == false)
                    {
                        selection[count] = 6;
                        flag[5] = true;
                        count++;
                    }
                }
                else if (0.89f <= arrow && arrow < 0.94f)   // 7등
                {
                    if (flag[6] == false)
                    {
                        selection[count] = 7;
                        flag[6] = true;
                        count++;
                    }
                }
                else if (0.94f <= arrow && arrow < 0.98f)   // 8등
                {
                    if (flag[7] == false)
                    {
                        selection[count] = 8;
                        flag[7] = true;
                        count++;
                    }
                }
                else if (0.98f <= arrow && arrow < 1.0f)     // 9등
                {
                    if (flag[8] == false)
                    {
                        selection[count] = 9;
                        flag[8] = true;
                        count++;
                    }
                }
                else                                        // 10등 혹은 오류값
                {

                }
                if (count >= 4)
                    break;
            }

            return selection;
        }

        static public float[] normalization(float[] x)
        {
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] != 0)
                    x[i] = 10f - x[i];
                x[i] /= 10f;
            }
            return x;
        }

        static public float mutation(float w)
        {
            float randFloat = (float)rand.NextDouble();

            //if (randFloat < 0.01f)
            //{
            //    if (randFloat < 0.005f)
            //    {
            //        w += (1 - w) / 2;
            //    }
            //    else
            //    {
            //        w += (-1 - w) / 2;
            //    }
            //}

            if (randFloat < 0.01f)
            {
                if (randFloat < 0.005f)
                {
                    randFloat = (float)rand.NextDouble() / 2;
                    w += randFloat;
                    if (w >= 1f)
                        w = 0.9999f;
                }
                else
                {
                    randFloat = (float)rand.NextDouble() / 2;
                    w -= randFloat;
                    if (w < -1.0f)
                        w = -1.0f;
                }
            }
            return w;
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
            for (int i = 0; i < w1.GetLength(0); i++)
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
            for (int i = 0; i < w2.GetLength(0); i++)
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
