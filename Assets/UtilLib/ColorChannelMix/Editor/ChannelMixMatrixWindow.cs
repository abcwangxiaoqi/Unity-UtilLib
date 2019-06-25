/*
 * 基础色 目标色 计算矩阵 进行混合染色
 * 基础色应尽量选取跟染色区域相近的颜色，这样计算出来的矩阵可以得到更相近目标色的颜色
 * 
 * 暂时因为默认基础色都为白色，暂时不用这种方案
 */
using UnityEditor;
using UnityEngine;

namespace KGame.ColorChannelMix
{
    public class ChannelMixMatrixWindow : EditorWindow
    {
        public Material mat;
        public Editor editor;
        
        Rect previewRect = new Rect(0, 200, 200, 200);

        public Color sourceCol = Color.white;
        public Color targetCol;

        Color checkColor;

        Vector4 matrixCol1;
        Vector4 matrixCol2;
        Vector4 matrixCol3;

        float[][] matrix = null;

        private void OnGUI()
        {
            GUILayout.Space(20);

            sourceCol = EditorGUILayout.ColorField(new GUIContent("基础色","尽量选取跟染色区域相近的颜色"), sourceCol, true, false, false,null);

            GUILayout.Space(10);

            targetCol = EditorGUILayout.ColorField(new GUIContent("混合色"),targetCol,true,false,false,null);            

            GUILayout.Space(20);

            matrix = ChannelMixConst.calMixChannelParams(sourceCol, targetCol);

            matrixCol1 = new Vector4(matrix[0][0], matrix[0][1], matrix[0][2], matrix[0][3]);
            matrixCol2 = new Vector4(matrix[1][0], matrix[1][1], matrix[1][2], matrix[1][3]);
            matrixCol3 = new Vector4(matrix[2][0], matrix[2][1], matrix[2][2], matrix[2][3]);

            GUILayout.Space(20);

            matrixCol1 = EditorGUILayout.Vector4Field("矩阵Row1", matrixCol1);
            matrixCol2 = EditorGUILayout.Vector4Field("矩阵Row2", matrixCol2);
            matrixCol3 = EditorGUILayout.Vector4Field("矩阵Row3", matrixCol3);

            if (mat == null)
                return;

            mat.SetVector("_ColorTransform0", matrixCol1);
            mat.SetVector("_ColorTransform1", matrixCol2);
            mat.SetVector("_ColorTransform2", matrixCol3);

            this.editor.Repaint();

            previewRect.x = 0;
            previewRect.y = this.position.height * 0.4f;
            previewRect.width = this.position.width;
            previewRect.height = this.position.height * (1-0.4f);

            GUI.backgroundColor = Color.gray;
            this.editor.OnPreviewGUI(previewRect, GUI.skin.GetStyle("ObjectPickerPreviewBackground"));
        }
    }
}