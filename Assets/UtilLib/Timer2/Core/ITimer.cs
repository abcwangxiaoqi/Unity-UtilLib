
public interface ITimer
{
    /// <summary>
    /// 是否在运行
    /// </summary>
    bool runing { get; }

    /// <summary>
    /// 启动计时器
    /// </summary>
    void Start();

    /// <summary>
    /// 停止后可以通过Start再次启动
    /// </summary>
    void Stop();

    /// <summary>
    /// 销毁后，该Timer无法再次启动
    /// </summary>
    void Destroy();

    /// <summary>
    /// 处理计时器处理
    /// </summary>
    void Cumulative();

}
