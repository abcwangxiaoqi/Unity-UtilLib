using UnityEngine;
using SceneAirWall;

public class airwalltest : MonoBehaviour {

    void Start()
    {
        BlockItem item = new BlockItem();
        item.creat(Vector3.zero);

        BlockItem item1 = new BlockItem();
        item1.creat(new Vector3(-AirWallConst.blockSize,0f,0f));

        BlockItem item2 = new BlockItem();
        item2.creat(new Vector3(-AirWallConst.blockSize, 0f, AirWallConst.blockSize));

        BlockItem item3 = new BlockItem();
        item3.creat(new Vector3(AirWallConst.blockSize, 0f, 0f));

        BlockItem item4 = new BlockItem();
        item4.creat(new Vector3(AirWallConst.blockSize, 0f, AirWallConst.blockSize));

        BlockItem item5 = new BlockItem();
        item5.creat(new Vector3(0f, 0f, -AirWallConst.blockSize));

        BlockItem item6 = new BlockItem();
        item6.creat(new Vector3(AirWallConst.blockSize, 0f, -AirWallConst.blockSize));

        BlockItem item7 = new BlockItem();
        item7.creat(new Vector3(AirWallConst.blockSize*2, 0f, -AirWallConst.blockSize*2));

        AirWall wall0 = new AirWall(item.pos, "0");
        wall0.creat();

        AirWall wall1 = new AirWall(item1.pos, "1");
        wall1.creat();

        AirWall wall2 = new AirWall(item2.pos, "2");
        wall2.creat();

        AirWall wall3 = new AirWall(item3.pos, "3");
        wall3.creat();

        AirWall wall4 = new AirWall(item4.pos, "4");
        wall4.creat();

        AirWall wall5 = new AirWall(item5.pos, "5");
        wall5.creat();

        AirWall wall6 = new AirWall(item6.pos, "6");
        wall6.creat();

        AirWall wall7 = new AirWall(item7.pos, "7");
        wall7.creat();

        item.destroy();
        wall0.destroy();

        //BlockItem item8 = new BlockItem();
        //item8.creat(new Vector3(AirWallConst.blockSize * 3, 0f, -AirWallConst.blockSize * 2));
        //AirWall wall8 = new AirWall(item8.pos, "8");
        //wall8.creat();
    }
}
