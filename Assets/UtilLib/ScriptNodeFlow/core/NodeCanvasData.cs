using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptNodeFlow
{
    [Serializable]
    public class NodeCanvasData : ScriptableObject
    {
        public string shareData;

        public List<NodeData> nodelist = new List<NodeData>();
        public List<RouterData> routerlist = new List<RouterData>();

        public DataBase Get(int id)
        {
            DataBase result = nodelist.Find((NodeData e) =>
            {
                return e.id == id;
            });

            if (result != null)
                return result;

            result = routerlist.Find((RouterData e) =>
            {
                return e.id == id;
            });

            return result;
        }

        public NodeData GetEntrance()
        {
            return nodelist.Find((NodeData e) =>
            {
                return e.isEntrance == true;
            });
        }
    }

    public enum NodeType
    {
        Node,
        Router
    }

    public class DataBase
    {
        public int id;
        public string name;
        public Vector2 position;

        public virtual NodeType type
        {
            get
            {
                return NodeType.Node;
            }
        }

    }

    [Serializable]
    public class NodeData : DataBase
    {
        public bool isEntrance;
        public string className;

        //just id
        public int next = -1;

        public override NodeType type
        {
            get
            {
                return NodeType.Node;
            }
        }
    }

    [Serializable]
    public class RouterData : DataBase
    {
        public List<RouterConditionData> conditions = new List<RouterConditionData>();
        public int defaultEntity = -1;

        public override NodeType type
        {
            get
            {
                return NodeType.Router;
            }
        }
    }

    [Serializable]
    public class RouterConditionData
    {
        public string className;
        public int entity = -1;
    }
}