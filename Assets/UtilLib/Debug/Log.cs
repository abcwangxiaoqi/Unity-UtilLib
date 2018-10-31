using System.Collections.Generic;
using UnityEngine;

namespace MyDebug
{
    public class LogItem
    {
        public ulong id;
        public string condition;
        public string stackTrace;
        public LogType type;
    }

    public static class Log
    {
        static Log()
        {
            allLog = new List<LogItem>();
            typeList = new Dictionary<DebugType, List<LogItem>>();
        }

        public static List<LogItem> allLog { get; private set; }

        public static Dictionary<DebugType, List<LogItem>> typeList { get; private set; }

        public static void Register()
        {
            UnityEngine.Application.logMessageReceivedThreaded += Application_logMessageReceived;
        }

        static ulong id=0;

        static void Application_logMessageReceived(string condition, string stackTrace, LogType type)
        {
            id += 1;

            LogItem tmpItem = new LogItem();
            tmpItem.id = id;
            tmpItem.condition = condition;
            tmpItem.stackTrace = stackTrace;
            tmpItem.type = type;

            allLog.Add(tmpItem);

            if (condition.StartsWith("["+DebugType.chenbo.ToString()+"]"))
            {
                if (!typeList.ContainsKey(DebugType.chenbo))
                {
                    typeList.Add(DebugType.chenbo, new List<LogItem>());
                }
                typeList[DebugType.chenbo].Add(tmpItem);
            }
            else if (condition.StartsWith("[" + DebugType.community.ToString() + "]"))
            {
                if (!typeList.ContainsKey(DebugType.community))
                {
                    typeList.Add(DebugType.community, new List<LogItem>());
                }
                typeList[DebugType.community].Add(tmpItem);
            }
            else if (condition.StartsWith("[" + DebugType.huangguoqing.ToString() + "]"))
            {
                if (!typeList.ContainsKey(DebugType.huangguoqing))
                {
                    typeList.Add(DebugType.huangguoqing, new List<LogItem>());
                }
                typeList[DebugType.huangguoqing].Add(tmpItem);
            }
            else if (condition.StartsWith("[" + DebugType.huangxu.ToString() + "]"))
            {
                if (!typeList.ContainsKey(DebugType.huangxu))
                {
                    typeList.Add(DebugType.huangxu, new List<LogItem>());
                }
                typeList[DebugType.huangxu].Add(tmpItem);
            }
            else if (condition.StartsWith("[" + DebugType.liyunfeng.ToString() + "]"))
            {
                if (!typeList.ContainsKey(DebugType.liyunfeng))
                {
                    typeList.Add(DebugType.liyunfeng, new List<LogItem>());
                }
                typeList[DebugType.liyunfeng].Add(tmpItem);
            }
            else if (condition.StartsWith("[" + DebugType.tangrui.ToString() + "]"))
            {
                if (!typeList.ContainsKey(DebugType.tangrui))
                {
                    typeList.Add(DebugType.tangrui, new List<LogItem>());
                }
                typeList[DebugType.tangrui].Add(tmpItem);
            }
            else if (condition.StartsWith("[" + DebugType.wangqi.ToString() + "]"))
            {
                if (!typeList.ContainsKey(DebugType.wangqi))
                {
                    typeList.Add(DebugType.wangqi, new List<LogItem>());
                }
                typeList[DebugType.wangqi].Add(tmpItem);
            }
            else if (condition.StartsWith("[" + DebugType.yichunguang.ToString() + "]"))
            {
                if (!typeList.ContainsKey(DebugType.yichunguang))
                {
                    typeList.Add(DebugType.yichunguang, new List<LogItem>());
                }
                typeList[DebugType.yichunguang].Add(tmpItem);
            }
        }

        public static void Dispose()
        {
            UnityEngine.Application.logMessageReceivedThreaded -= Application_logMessageReceived;
        }

        public static void clear()
        {
            allLog.Clear();
            typeList.Clear();
        }

        public static void sort(List<LogItem> list)
        {
            quickSort(list, 0, list.Count-1);
        }        

        static void quickSort(List<LogItem> list,int _left,int _right)
        {
            LogItem temp = null;
            int left = _left;
            int right = _right;

            if (left<=right)
            {
                temp = list[left];//基准元素
                while(left!=right)
                {
                    while(right>left && list[right].id>= temp.id)
                    {
                        right--;
                    }
                    list[left] = list[right];                  

                    while(left<right && list[left].id<=temp.id)
                    {
                        left++;
                    }
                    list[right] = list[left];
                }
                list[left] = temp;
                quickSort(list, _left, left - 1);
                quickSort(list, right+1, _right);
            }
        }
    }
}
