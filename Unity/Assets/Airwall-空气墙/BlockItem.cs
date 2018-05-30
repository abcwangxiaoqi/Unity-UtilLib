using UnityEngine;
using System.Collections;

namespace SceneAirWall
{
    public class BlockItem
    {
        public GameObject obj { get; private set; }
        public Vector3 pos
        {
            get
            {
                return obj.transform.position;
            }
        }

        public void creat(Vector3 pos)//默认10米地块
        {
            GameObject go = Resources.Load<GameObject>("airwall/block");
            obj = Object.Instantiate(go);
            obj.transform.position = pos;
            obj.transform.localScale = new Vector3(AirWallConst.blockSize / 10f, 1, AirWallConst.blockSize / 10f);
        }

        public void destroy()
        {
            Object.Destroy(obj);
        }
    }
}
