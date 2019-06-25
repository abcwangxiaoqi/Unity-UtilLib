using System;
using System.Collections.Generic;
using UnityEngine;

namespace KGame.ColorChannelMix
{
    public class ChannelMixConst
    {
        public static float[][] MatrixMult(float[][] matrix1, float[][] matrix2)
        {
            //matrix1是m*n矩阵，matrix2是n*p矩阵，则result是m*p矩阵   
            int m = matrix1.Length, n = matrix2.Length, p = matrix2[0].Length;
            float[][] result = new float[m][];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new float[p];
            }
            //矩阵乘法：c[i,j]=Sigma(k=1→n,a[i,k]*b[k,j])   
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < p; j++)
                {
                    //对乘加法则   
                    for (int k = 0; k < n; k++)
                    {
                        result[i][j] += (matrix1[i][k] * matrix2[k][j]);
                    }
                }
            }
            return result;
        }

        public static float[][] calMixChannelParams(Color source, Color target)
        {
            List<float> sColor = new List<float>() { source.r, source.g, source.b };
            List<float> tValue = new List<float>() { target.r, target.g, target.b };

            float[] ret1 = calMixChannelParam(sColor, tValue[0], 0);
            float[] ret2 = calMixChannelParam(sColor, tValue[1], 1);
            float[] ret3 = calMixChannelParam(sColor, tValue[2], 2);

            float[][] matrix = new float[3][];
            matrix[0] = ret1;
            matrix[1] = ret2;
            matrix[2] = ret3;

            return matrix;
        }

        public static float[] calMixChannelParam(List<float> sColor, float tValue, int targetidx)
        {
            List<float> ret = new List<float>() { 0, 0, 0 };
            ret[targetidx] = 1.0f;

            List<float> sColor2 = new List<float>(sColor);
            sColor2.Sort();

            float maxLimit = 0;

            foreach (var item in sColor2)
            {
                maxLimit += item;
            }
            maxLimit *= 2;

            if (tValue > maxLimit)
            {
                return new float[] { 2, 2, 2, tValue - maxLimit };
            }

            float currColor = sColor[targetidx];

            if (currColor < tValue)
            {
                for (int i = 0; i < sColor2.Count; i++)
                {
                    int retIdx = sColor.IndexOf(sColor2[2 - i]);
                    float diff = tValue - currColor;
                    float mRatio = 2f;

                    if (retIdx == targetidx)
                    {
                        mRatio = 1;
                    }

                    float r = diff / sColor2[2 - i];

                    if (r > mRatio)
                    {
                        currColor += (mRatio * sColor2[2 - 1]);
                        ret[retIdx] = 2;
                    }
                    else
                    {
                        ret[retIdx] += r;
                        break;
                    }
                }
            }
            else if (currColor > tValue)
            {
                float r = tValue / currColor;
                ret[targetidx] = r;
            }

            ret.Add(0);
            return ret.ToArray();
        }

        ///hex是没有alpha通道的
        public static Color HEX2RGB(int sColor)
        {
            Color c = new Color();
            c.r = ((sColor & 0x00FF0000) >> 16) / 255f;
            c.g = ((sColor & 0x0000FF00) >> 8) / 255f;
            c.b = (sColor & 0x000000FF) / 255f;
            return c;
        }

        /// <summary>
        /// HSV 转 RGB
        /// </summary>
        /// <param name="H">0~360</param>
        /// <param name="S">0~1</param>
        /// <param name="V">0~1</param>
        public static Color HSV2RGB(int H, float S, float V)
        {
            float C = V * S;
            float X = C * (1 - Math.Abs((H / 60) % 2 - 1));
            float m = V - C;

            float R, G, B;

            switch (H % 60)
            {
                case 0:
                    R = C;
                    G = X;
                    B = 0;
                    break;
                case 1:
                    R = X;
                    G = C;
                    B = 0;
                    break;
                case 2:
                    R = 0;
                    G = C;
                    B = X;
                    break;
                case 3:
                    R = 0;
                    G = X;
                    B = C;
                    break;
                case 4:
                    R = X;
                    G = 0;
                    B = C;
                    break;
                case 5:
                    R = C;
                    G = 0;
                    B = X;
                    break;
                default:
                    R = C;
                    G = X;
                    B = 0;
                    break;
            }

            R += m;
            G += m;
            B += m;

            return new Color(R, G, B);
        }
    }
}
