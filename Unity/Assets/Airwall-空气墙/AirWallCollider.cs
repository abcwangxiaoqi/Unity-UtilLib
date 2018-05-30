using System.Collections.Generic;
using UnityEngine;

namespace SceneAirWall
{
    public enum EAirWallDirection
    {
        Up,
        Down,
        Left,
        Leftup,
        Leftdown,
        Right,
        Rightup,
        Rightdown
    }

    public class AirWallCollider
    {
        static GameObject root;
        GameObject gobject = null;
        float zoomRate = 1f;
        public AirWallCollider()
        {
            zoomRate = AirWallConst.blockSize / 10f;

            if(root==null)
            {
                root = new GameObject("airwallroot");
                root.transform.position = Vector3.zero;
                Object.DontDestroyOnLoad(root);
            }

            GameObject obj = Resources.Load<GameObject>("airwall/airwall");
            gobject = Object.Instantiate(obj);
            gobject.transform.SetParent(root.transform);
            up = gobject.FindInChildren("up");
            upleft = gobject.FindInChildren("upleft");
            upright = gobject.FindInChildren("upright");

            down = gobject.FindInChildren("down");
            downleft = gobject.FindInChildren("downleft");
            downright = gobject.FindInChildren("downright");

            left = gobject.FindInChildren("left");
            leftup = gobject.FindInChildren("leftup");
            leftdown = gobject.FindInChildren("leftdown");

            right = gobject.FindInChildren("right");
            rightup = gobject.FindInChildren("rightup");
            rightdown = gobject.FindInChildren("rightdown");
            gobject.transform.localScale = new Vector3(zoomRate, zoomRate, zoomRate);
        }

        bool upFlag = false;
        bool downFlag = false;
        bool leftFlag = false;
        bool leftupFlag = false;
        bool leftdownFlag = false;
        bool rightFlag = false;
        bool rightupFlag = false;
        bool rightdownFlag = false;

        #region items

        GameObject up;
        GameObject upleft;
        GameObject upright;

        GameObject down;
        GameObject downleft;
        GameObject downright;

        GameObject left;
        GameObject leftup;
        GameObject leftdown;

        GameObject right;
        GameObject rightup;
        GameObject rightdown;

        #endregion

        public void update()
        {
            up.SetActive(!upFlag);
            down.SetActive(!downFlag);
            left.SetActive(!leftFlag);
            right.SetActive(!rightFlag);

            leftup.SetActive(leftFlag && !(upFlag && leftupFlag));
            leftdown.SetActive(leftFlag && !(downFlag && leftdownFlag));
            rightup.SetActive(rightFlag && !(upFlag && rightupFlag));
            rightdown.SetActive(rightFlag && !(downFlag && rightdownFlag));

            upleft.SetActive(upFlag && !(leftFlag && leftupFlag));
            upright.SetActive(upFlag && !(rightFlag && rightupFlag));
            downleft.SetActive(downFlag && !(leftFlag && leftdownFlag));
            downright.SetActive(downFlag && !(rightFlag && rightdownFlag));

            //leftup.SetActive(!(upFlag && leftupFlag));
            //leftdown.SetActive(!(downFlag && leftdownFlag));
            //rightup.SetActive(!(upFlag && rightupFlag));
            //rightdown.SetActive(!(downFlag && rightdownFlag));

            //upleft.SetActive(!(leftFlag && leftupFlag));
            //upright.SetActive(!(rightFlag && rightupFlag));
            //downleft.SetActive(!(leftFlag && leftdownFlag));
            //downright.SetActive(!(rightFlag && rightdownFlag));
        }

        public void setState(EAirWallDirection dir)
        {
            switch (dir)
            {
                case EAirWallDirection.Down:
                    downFlag = true;
                    break;
                case EAirWallDirection.Up:
                    upFlag = true;
                    break;
                case EAirWallDirection.Left:
                    leftFlag = true;
                    break;
                case EAirWallDirection.Right:
                    rightFlag = true;
                    break;
                case EAirWallDirection.Leftup:
                    leftupFlag = true;
                    break;
                case EAirWallDirection.Leftdown:
                    leftdownFlag = true;
                    break;
                case EAirWallDirection.Rightup:
                    rightupFlag = true;
                    break;
                case EAirWallDirection.Rightdown:
                    rightdownFlag = true;
                    break;
            }
        }

        public void resetState()
        {
            upFlag = false;
            downFlag = false;
            leftFlag = false;
            leftupFlag = false;
            leftdownFlag = false;
            rightFlag = false;
            rightupFlag = false;
            rightdownFlag = false;
        }

        public void SetActive(bool active)
        {
            gobject.SetActive(active);
        }

        public void SetPosition(Vector3 vec)
        {
            gobject.transform.position = vec;
        }

        public void SetName(string name)
        {
            gobject.name = name;
        }

        public void destroy()
        {
            if (gobject != null)
            {
                Object.Destroy(gobject);
            }
        }
    }
}