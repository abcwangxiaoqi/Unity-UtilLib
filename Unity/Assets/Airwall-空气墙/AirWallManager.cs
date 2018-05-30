using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SceneAirWall
{
    public class AirWallManager
    {
        static AirWallManager instance;
        public static AirWallManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AirWallManager();
                }
                return instance;
            }
        }

        List<AirWall> walls = new List<AirWall>();

        public void Add(AirWall wall)
        {
            walls.Add(wall);
            updaterelation(wall);
        }

        public void Remove(AirWall wall)
        {
            if (!walls.Contains(wall))
                return;

            walls.Remove(wall);
            updaterelation(wall);
        }

        void updaterelation(AirWall wall)
        {
            List<AirWall> relations=walls.FindAll((AirWall item) =>
            {
                return distance2D(wall, item) < 2 * AirWallConst.blockSize;
            });

            if (relations==null)
            {
                return;
            }

            for (int i = 0; i < relations.Count; i++)
            {
                signUpdate(relations[i]);
            }
        }

        void signUpdate(AirWall wall)
        {
            wall.airwallCollider.resetState();
            for (int i = 0; i < walls.Count; i++)
            {
                if (walls[i].ID == wall.ID)
                {
                    continue;
                }
                float dis = distance2D(walls[i], wall);
                if (dis >= 2 * AirWallConst.blockSize)
                {
                    continue;
                }

                EAirWallDirection dir = direction(wall, walls[i]);
                wall.airwallCollider.setState(dir);
            }
            wall.airwallCollider.update();
        }

        EAirWallDirection direction(AirWall org, AirWall tar)
        {
            Vector2 Org = new Vector2(org.Pos.x, org.Pos.z);
            Vector2 Tar = new Vector2(tar.Pos.x, tar.Pos.z);

            Vector2 v1 = Tar - Org;

            float dot1 = Vector2.Dot(v1.normalized, Vector2.right);
            float dot2 = Vector2.Dot(v1.normalized, Vector2.up);
            if (System.Math.Abs(dot1) < 0.0001f)
            {
                //up or down
                if (System.Math.Abs(dot2 - 1) < 0.0001f)
                {
                    //up
                    return EAirWallDirection.Up;
                }
                else
                {
                    //down
                    return EAirWallDirection.Down;
                }
            }
            else if (dot1 > 0)
            {
                if (System.Math.Abs(dot1 - 1f) < 0.0001f)
                {
                    //right
                    return EAirWallDirection.Right;
                }
                else
                {
                    if (dot2 > 0)
                    {
                        //up right
                        return EAirWallDirection.Rightup;
                    }
                    else
                    {
                        //down right
                        return EAirWallDirection.Rightdown;
                    }
                }
            }
            else
            {
                if (System.Math.Abs(dot1 - -1f) < 0.0001f)
                {
                    //left
                    return EAirWallDirection.Left;
                }
                else
                {
                    if (dot2 > 0)
                    {
                        //left up
                        return EAirWallDirection.Leftup;
                    }
                    else
                    {
                        //left down
                        return EAirWallDirection.Leftdown;
                    }
                }
            }
        }

        float distance2D(AirWall block1, AirWall block2)
        {
            Vector2 v1 = new Vector2(block1.Pos.x, block1.Pos.z);
            Vector2 v2 = new Vector2(block2.Pos.x, block2.Pos.z);
            return Vector2.Distance(v1, v2);
        }
    }
}
