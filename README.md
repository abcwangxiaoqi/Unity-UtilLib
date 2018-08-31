# 记录Unity中，总结个人开发及封装的工具  

***Editor工具***    
对Unity的Editor接口进行了二次封装，许多功能聚合在一个对象中，提高了使用体验
+ **ObjectBase**  
Load本地资源,调用UnityApi处理资源，如文本，asset，material等，涵盖了大多数类型文件，特殊文件袋特殊功能会在其他对象上体现(如：设置texture的可读写功能)。    

**使用方法：**
```
    ObjectBase obj = new ObjectBase("Assets/1.txt");
    TextAsset txt = obj.Load<TextAsset>();
```
这样就可以加载出*Assets/1.txt*的文件，并以*TextAsset*的方式来读取他。文件路径可以是相对路径也可以是绝对路径。  
**接口说明**  
```
    Name //文件名
    Type //文件类型 如 txt

    T Load<T>() where T : Object; //从目标路径加载 根据T
    Object Load(); //从目标路径加载
    void Import();
    void CreatAsset(Object obj); //创建资源
    void SaveAsset(Object obj); //保存资源
    string[] GetDependencies(); //获取依赖资源列表
    string path { get; } //资源的相对路径
    void ImportAsset(); 
    void WriteImportSettingsIfDirty();
    void ImportAsset(ImportAssetOptions Options);
    void AddObjectToAsset(Object obj);
    void DeleteAsset();//删除资源
    AssetImporter importer { get; }//获取导入对象
    void SetAssetbundleName(string name); //设置bundlename
    void Save(); //importer save

```  

+ **Fbxitem** 
处理fbx资源的对象。继承自*ObjectBase*，所以ObjectBase该有的功能都有  

**接口说明**  
```
void SetReadable(bool readable);//设置是否可读写
```  

+ **EditorSprite** 
处理贴图资源的对象。继承自*ObjectBase*，所有ObjectBase该有的功能都有  

**接口说明**  
```
    Texture2D Tex { get; } //获取贴图
    void SetReadable(bool readable); //设置是否可读写
    void SetTextureType(TextureImporterType type); //设置类型
    void SetMaxTextureSize(int size); //
    void SetMipmapEnabled(bool b); //开启mipmap
    void SetNormalMap(bool normal); 
    void SetAphaIsTransparency(bool b);
    void SetPlatformTextureSettings(string platform, int maxSize, TextureImporterFormat format);
    void SetPlatformTextureSettings(string platform, int maxSize, TextureImporterFormat format, bool allowAphaSplit);
    void SetPlatformTextureSettings(string platform, int maxSize, TextureImporterFormat format, int compress, bool allowAphaSplit);
    bool HasAlphaChannel(); //是否有alpha通道
    bool IsSquare(); //是否是方图
```   

+ **PrefabItem**
创建Prefab。继承自*ObjectBase*，所有ObjectBase该有的功能都有  

**接口说明**  
```
    //obj: 创建对象
    //callback 创建回调 回调函数中 做在写入磁盘前，对object进行什么修改操作，如添加脚本等
    //回调参数 参数会跟着 callback一起返回

    void CreatPrefab(UnityEngine.Object obj, Action<UnityEngine.GameObject, Dictionary<string, object>> callback = null, Dictionary<string, object> parameters = null);
```  

+ **ScriptableItem**  
创建继承于Scriptable序列化为asset文件的对象。所有ObjectBase该有的功能都有  

**接口说明**  
```
    //T : 必须继承ScriptableObject
    //obj: 创建对象
    //callback 创建回调 回调函数中 做在写入磁盘前，对object进行什么修改操作，如添加脚本等
    //回调参数 参数会跟着 callback一起返回

    void Creat<T>(Action<T, Dictionary<string, object>> callback = null, Dictionary<string, object> parameters = null) where T : ScriptableObject;
```  

***Timer***  
1. 将update fixedupdate 等进行接口聚合
2. 定时器工具 隔帧 隔秒刷新

***Loader***  
Http工具
1. 多线程
2. 生产者 消费者模式

***Log***  
1. 系统打印
2. 可在终端查看
3. 初步筛选

***IMR***  
1. 指令 操作 数据层隔离

***IEnumeratorCollection***  
1. 控制协程 并发 或者 顺序执行  

***LitteMomery***  
1. 运行时缓存
2. gc机制

***SimpleWF*** 
1. 定制工作流
2. 方便逻辑串联 