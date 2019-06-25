using UnityEditor;
using UnityEngine;

namespace KGame.ColorChannelMix
{
    public class ChannelMixCheckWindow : EditorWindow
    {
        [MenuItem("工具/通道混合计算验证")]
        static void open()
        {
            var window = GetWindow<ChannelMixCheckWindow>(true, "Transfer Color Window", true);
        }

        Color source;
        Color target;

        Color checkColor;

        Vector4 matrixCol1;
        Vector4 matrixCol2;
        Vector4 matrixCol3;

        float[][] matrix = null;

        private void OnGUI()
        {
            GUILayout.Label("原始色");
            source = EditorGUILayout.ColorField(source);

            GUILayout.Space(20);

            GUILayout.Label("目标色");
            target = EditorGUILayout.ColorField(target);

            GUILayout.Space(20);


            matrix = ChannelMixConst.calMixChannelParams(source, target);

            matrixCol1 = new Vector4(matrix[0][0], matrix[0][1], matrix[0][2], matrix[0][3]);
            matrixCol2 = new Vector4(matrix[1][0], matrix[1][1], matrix[1][2], matrix[1][3]);
            matrixCol3 = new Vector4(matrix[2][0], matrix[2][1], matrix[2][2], matrix[2][3]);

            GUILayout.Space(20);

            matrixCol1 = EditorGUILayout.Vector4Field("矩阵Row1", matrixCol1);
            matrixCol2 = EditorGUILayout.Vector4Field("矩阵Row2", matrixCol2);
            matrixCol3 = EditorGUILayout.Vector4Field("矩阵Row3", matrixCol3);

            GUILayout.Space(40);
            if (GUILayout.Button("验证计算"))
            {
                if (matrix == null)
                    return;

                float[][] sourceMatrix = new float[4][];
                sourceMatrix[0] = new float[] { source.r };
                sourceMatrix[1] = new float[] { source.g };
                sourceMatrix[2] = new float[] { source.b };
                sourceMatrix[3] = new float[] { source.a };

                float[][] res = ChannelMixConst.MatrixMult(matrix, sourceMatrix);
                checkColor = new Color(res[0][0], res[1][0], res[2][0], 0);
            }

            GUILayout.Space(20);

            EditorGUILayout.ColorField(checkColor);
        }
    }
}
