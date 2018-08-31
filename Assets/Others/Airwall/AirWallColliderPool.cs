using System.Collections.Generic;

namespace SceneAirWall
{
    public sealed class AirWallColliderPool
    {
        static AirWallColliderPool instance = null;
        public static AirWallColliderPool Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AirWallColliderPool();
                }
                return instance;
            }
        }

        List<AirWallCollider> list = new List<AirWallCollider>();

        public void InPool(AirWallCollider wall)
        {
            wall.SetActive(false);
            list.Add(wall);
        }

        public AirWallCollider OutPool()
        {
            AirWallCollider item = null;
            if (list.Count == 0)
            {
                item = new AirWallCollider();
            }
            else
            {
                item = list[0];
                item.SetActive(true);
                list.RemoveAt(0);
            }
            return item;
        }

        public void Clear()
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].destroy();
            }

            list.Clear();
        }
    }
}
