
using System.Collections.Generic;
using UnityEngine;

namespace MyDebug
{
    public enum DebugType
    {
        wangqi,
        tangrui,
        chenbo,
        huangguoqing,
        liyunfeng,
        yichunguang,
        huangxu,
        community
    }

    public static class Debuger
    {
        static Debuger()
        {
            for (int i = 24; i < 70; i++)
            {
                StringPool.PreAlloc(i, 2);
            }
        }

        public static bool enable = true;

        public static readonly PrintLog WQ = new PrintLog(DebugType.wangqi, true);
        public static readonly PrintLog TR = new PrintLog(DebugType.tangrui, false);
        public static readonly PrintLog ChenBo = new PrintLog(DebugType.chenbo, false);
        public static readonly PrintLog HGQ = new PrintLog(DebugType.huangguoqing, false);
        public static readonly PrintLog LYF = new PrintLog(DebugType.liyunfeng, false);
        public static readonly PrintLog YCG = new PrintLog(DebugType.yichunguang, false);
        public static readonly PrintLog HX = new PrintLog(DebugType.huangxu, false);
        public static readonly PrintLog SOCKET = new PrintLog(DebugType.community, false);
    }
}
