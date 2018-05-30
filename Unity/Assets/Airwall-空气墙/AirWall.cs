using UnityEngine;

namespace SceneAirWall
{    
    public static class AirWallConst
    {
        public const float blockSize = 200f;//地块大小(米)
    }

    public class AirWall
    { 
        public Vector3 Pos { get; private set; }

        public string ID { get; private set; }

        public AirWallCollider airwallCollider { get; private set; }

        public AirWall(Vector3 pos, string blockid)
        {
            this.Pos = pos;
            this.ID = blockid;
        }

        public void creat()
        {
            airwallCollider = AirWallColliderPool.Instance.OutPool();
            airwallCollider.SetPosition(Pos);
            airwallCollider.SetName(ID);

            AirWallManager.Instance.Add(this);
        }

        public void destroy()
        {
            if (airwallCollider != null)
            {
                AirWallColliderPool.Instance.InPool(airwallCollider);
                airwallCollider = null;
            }
            AirWallManager.Instance.Remove(this);
        }
    }
}
