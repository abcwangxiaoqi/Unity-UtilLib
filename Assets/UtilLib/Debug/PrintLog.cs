using UnityEngine;

namespace MyDebug
{
    public class PrintLog
    {
        string type = null;

        public PrintLog(DebugType _type, bool _enable)
        {
            type = _type.ToString();
            enable = _enable;
        }

        //static CString sb = new CString(256);

        public bool enable = true;

        string GetLogFormat(string str, LogType logType)
        {
            CString sb = new CString(256);
            //sb.Clear();
            if (!string.IsNullOrEmpty(type))
            {
                sb.Append("[");
                sb.Append(type);
                sb.Append("]");
                sb.Append("[");
                sb.Append(logType.ToString());
                sb.Append("]");
                sb.Append(":");
            }
            sb.Append(str);
            /*string dest = StringPool.Alloc(sb.Length);
            sb.CopyToString(dest);
            return dest;*/
            return sb.ToString();
        }

        public static object _lock = new object();

        public void Log(string msg)
        {
            if (!enable || !Debuger.enable)
                return;

            lock(_lock)
            {
                msg = GetLogFormat(msg, LogType.Log);
                Debug.Log(msg);
                StringPool.Collect(msg);
            }
        }

        public void Log(object message)
        {
            Log(message == null ? "" : message.ToString());
        }

        public void LogError(string msg)
        {
            if (!enable || !Debuger.enable)
                return;
            lock (_lock)
            {
                msg = GetLogFormat(msg, LogType.Error);
                Debug.LogError(msg);
                StringPool.Collect(msg);
            }
        }

        public void LogError(object message)
        {
            LogError(message == null ? "" : message.ToString());
        }

        public void LogWarning(string msg)
        {
            if (!enable || !Debuger.enable)
                return;

            lock (_lock)
            {
                msg = GetLogFormat(msg, LogType.Warning);
                Debug.LogWarning(msg);
                StringPool.Collect(msg);
            }
        }

        public void LogWarning(object message)
        {
            LogWarning(message == null ? "" : message.ToString());
        }
    }
}
