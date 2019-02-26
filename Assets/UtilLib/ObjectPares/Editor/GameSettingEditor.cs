using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#region 测试用例

public class GameSettingItemOne
{
    public string str1;
    public GameSettingEnum type;
    public int intV;
    public List<string> list = new List<string>();
    public GameSettingItemTwo two;
}

public class GameSettingItemTwo
{
    public string str1;
    public GameSettingEnum type;
    public int intV;
}

public enum GameSettingEnum
{
    One,
    Two
}

public class GameSettingConfig
{
    public string name;
    public int id;
    public float fvalue;
    public GameSettingEnum type;
    public GameSettingItemOne one { get; private set; }
    public string str1;
    public List<float> listf = new List<float>();
    public List<double> listd = new List<double>();
    public List<bool> listb = new List<bool>();
    public List<int> listi = new List<int>();
    public List<string> list = new List<string>();
    public List<List<GameSettingItemOne>> listlist = new List<List<GameSettingItemOne>>();
    public List<List<List<string>>> listlistlist = new List<List<List<string>>>();
    public bool flag = true;

}

#endregion

public class GameSettingEditor : EditorWindow
{
    [MenuItem("Tools/ObjectParse")]
    static void open()
    {
        ObjectParseWindow.OpenWindow();
    }
}