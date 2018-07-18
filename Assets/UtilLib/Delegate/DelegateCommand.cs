
public enum DelegateCommand
{
    //系统事件
    ApplicationQuit,
    //角色瞬移
    RoleChangePosition,
    //摄像头视觉切换
    ChangeFollowType,
    //通知原生删掉缓存
    DeleteCache,
    //手势事件
    OnTap,
    OnDrag,
    OnTwist,
    OnPinch,
    OnDoubleTap,
    OnLongPress,
    /// <summary>
    /// 中心地块创建
    /// </summary>
    CenterBlockCreated,
    CityStreetInitCompleted,

    RequireMainActor,
    ReturnMainActor,

    AutoNavEnter,
    AutoNavTouchAirWall,
    //AutoNavStart,
    //AutoNavPause,
    //AutoNavContinue,
    //AutoNavEnd,
    AutoNavExit,
    AutoNavUpdatePosAndRot,
    AutoNavUpdateDirection,

    ExchangeCameraView,


    /// <summary>
    /// 用户信息更新
    /// </summary>
    UserInfoUpdate,
    SwitchStorePanorama,
    StoreRoom,


    //++++++++社交
    COMMUNITY_GAMEFRESH,
    COMMUNITY_NETBREAK,
    COMMUNITY_GAMETRIGGER_MJ,
    COMMUNITY_GAMETRIGGER_UI,
    COMMUNITY_CLICKPLAYER,
    COMMUNITY_CANCLECLICKPLAYER,
    COMMUNITY_LOGIN,
    COMMUNITY_ENTRY,
    COMMUNITY_NPCMOVE,
    COMMUNITY_GREET,
    COMMUNITY_BEREFUSEFOLLOW,
    COMMUNITY_FOLLOWLIST,
    COMMUNITY_OFFLINE,
    COMMUNITY_ROLESTATE,
    COMMUNITY_ROLEPOS,
    COMMUNITY_GREETTONPC,
    COMMUNITY_FOLLOW,
    COMMUNITY_FOLLOWSURE,
    COMMUNITY_FOLLOWSTATE,
    COMMUNITY_NPCDATA,
    COMMUNITY_ONNPCDESTROY,
    COMMUNITY_SWITCHUSER,
    COMMUNITY_NPCDESTROYACTION,
    COMMUNITY_REFUSESBFOLLOW,
    COMMUNITY_UPDATEINTERACTIONLIST,
    COMMUNITY_UPDATECLOTH,
    //+++


    Role_Enable,

    OnJoyBeginDrag,
    OnJoyMoveDrag,
    OnJoyEndDrag,
    //游戏图标
    GAME_LOAD_VISIABLE,
    GAME_LOAD_ICON,
    GAME_NOTICECLICK,
    //跟随
    FOLLOW_USERS,
    //FOLLOW_OTHER, //我是否跟随别人
    //是否点击玩家
    CLICK_PLAYER,
    CANEL_CLICK_PLAYER,
    //玩家自己
    ROLE_ELEMENT,
    //当前位置
    LOCATION_NAME,
    //当前显示红点
    MSG_RED_DOT,
    //游戏公告
    GAME_NOTICE,
    GAME_NOTICE_CALLBACK,


    //打招呼 发表情
    PLAYERINTERACTION,

    /// <summary>
    /// 刷新Token
    /// </summary>
    RefreshToken,

    //================================================================
    /// <summary>
    /// 进入店铺
    /// </summary>
    StoreEnter,
    /// <summary>
    /// 开启VR
    /// </summary>
    StoreOpenVR,
    /// <summary>
    /// 退出VR
    /// </summary>
    StoreExitVR,
    /// <summary>
    /// UI通知全景，Room改变
    /// </summary>
    StoreRoomChange,
    /// <summary>
    /// StoreLaunch启动通知
    /// </summary>
    StoreLaunchNotice,
    //================================================================

    //街道店铺UI相关
    HideAllStreetShopIcon,
    ShowAllStreetShopIcon,
    STORE_CHANGE,

    NotifyLowMomery,
}