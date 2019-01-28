using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System;
using Object = UnityEngine.Object;
using System.Linq;

public static class GameObjectExpand
{
    public static void ClearChilds(this GameObject go)
    {
        if (go == null) return;

        Transform tran = go.transform;
        if (tran == null)
        {
            return;
        }

        foreach (Transform item in tran)
        {
            GameObject.Destroy(item.gameObject);
        }
    }

    /// <summary>
    /// 刷新shader
    /// </summary>
    /// <param name="srcRes"></param>
    public static void RefreshShader(this GameObject srcRes)
    {
        if (srcRes == null)
            return;

        ParticleSystem[] pss = srcRes.GetComponentsInChildren<ParticleSystem>();
        MeshRenderer[] mrs = srcRes.GetComponentsInChildren<MeshRenderer>();
        SkinnedMeshRenderer[] smrs = srcRes.GetComponentsInChildren<SkinnedMeshRenderer>();
        if (smrs == null && mrs == null && pss != null)
        {
            return;
        }
        int length = mrs.Length;
        for (int i = 0; i < length; i++)
        {
            Material[] ms = mrs[i].sharedMaterials;

            if (ms.Length == 0)
                continue;

            foreach (Object m in ms)
            {
                Material mat = m as Material;
                if (mat != null)
                {
                    if (mat.shader == null || !mat.shader.isSupported)
                    {
                        string shaderName = mat.shader.name;
                        Shader newShader = Shader.Find(shaderName);
                        if (newShader != null && newShader.isSupported)
                        {
                            mat.shader = newShader;
                        }
                        else
                        {
                            mat.shader = Shader.Find("Mobile/Diffuse");
                            Debug.LogWarning("unable to refresh shader: " + shaderName + " in material " + m.name);
                        }
                    }
                }
            }
        }
        length = smrs.Length;
        for (int i = 0; i < length; i++)
        {
            Material[] ms = smrs[i].sharedMaterials;

            if (ms.Length == 0)
                continue;

            foreach (Object m in ms)
            {
                Material mat = m as Material;

                if (mat != null)
                {
                    string shaderName = mat.shader.name;
                    Shader newShader = Shader.Find(shaderName);
                    if (newShader != null && newShader.isSupported)
                    {
                        mat.shader = newShader;
                    }
                    else
                    {
                        mat.shader = Shader.Find("Mobile/Diffuse");
                        Debug.LogWarning("unable to refresh shader: " + shaderName + " in material " + m.name);
                    }
                }
            }
        }
        length = pss.Length;
        for (int i = 0; i < length; i++)
        {
            ParticleSystemRenderer psr = pss[i].GetComponent<ParticleSystemRenderer>();
            Material[] ms = psr.sharedMaterials;

            if (ms.Length == 0)
                continue;

            foreach (Object m in ms)
            {
                Material mat = m as Material;

                if (mat != null)
                {
                    string shaderName = mat.shader.name;
                    Shader newShader = Shader.Find(shaderName);
                    if (newShader != null && newShader.isSupported)
                    {
                        mat.shader = newShader;
                    }
                    else
                    {
                        mat.shader = Shader.Find("Mobile/Diffuse");
                        Debug.LogWarning("unable to refresh shader: " + shaderName + " in material " + m.name);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 设置receiveShadows
    /// </summary>
    /// <param name="go"></param>
    /// <param name="receiveShadows"></param>
    public static void ReceiveShadows(this GameObject go, bool receiveShadows)
    {
        if (go == null)
            return;

        MeshRenderer[] renderers = go.GetComponentsInChildren<MeshRenderer>();
        if (renderers != null)
        {
            int length = renderers.Length;
            for (int i = 0; i < length; i++)
            {
                renderers[i].receiveShadows = receiveShadows;
            }
        }

        SkinnedMeshRenderer[] skinrenderers = go.GetComponentsInChildren<SkinnedMeshRenderer>();
        if (skinrenderers != null)
        {
            int length = skinrenderers.Length;
            for (int i = 0; i < length; i++)
            {
                skinrenderers[i].receiveShadows = receiveShadows;
            }
        }
    }

    /// <summary>
    /// 设置ShadowCastingMode
    /// </summary>
    /// <param name="go"></param>
    /// <param name="mode"></param>
    public static void ShadowCastingMode(this GameObject go, UnityEngine.Rendering.ShadowCastingMode mode)
    {
        if (go == null)
            return;

        MeshRenderer[] renderers = go.GetComponentsInChildren<MeshRenderer>();
        if (renderers == null)
            return;
        int length = renderers.Length;
        for (int i = 0; i < length; i++)
        {
            renderers[i].shadowCastingMode = mode;
        }
    }

    /// <summary>
    /// 设置UseLightProbes
    /// </summary>
    /// <param name="go"></param>
    /// <param name="useLightProbes"></param>
    public static void UseLightProbes(this GameObject go, bool useLightProbes)
    {
        if (go == null)
            return;

        MeshRenderer[] renderers = go.GetComponentsInChildren<MeshRenderer>();
        if (renderers != null)
        {
            int length = renderers.Length;
            for (int i = 0; i < length; i++)
            {
                renderers[i].useLightProbes = useLightProbes;
            }
        }

        SkinnedMeshRenderer[] skinrenderers = go.GetComponentsInChildren<SkinnedMeshRenderer>();
        if (skinrenderers != null)
        {
            int length = skinrenderers.Length;
            for (int i = 0; i < length; i++)
            {
                skinrenderers[i].useLightProbes = useLightProbes;
            }
        }
    }

    /// <summary>
    /// 设置ReflectionProbeUsage
    /// </summary>
    /// <param name="go"></param>
    /// <param name="usage"></param>
    public static void ReflectionProbeUsage(this GameObject go, UnityEngine.Rendering.ReflectionProbeUsage usage)
    {
        if (go == null)
            return;

        MeshRenderer[] renderers = go.GetComponentsInChildren<MeshRenderer>();
        if (renderers != null)
        {
            int length = renderers.Length;
            for (int i = 0; i < length; i++)
            {
                renderers[i].reflectionProbeUsage = usage;
            }
        }

        SkinnedMeshRenderer[] skinrenderers = go.GetComponentsInChildren<SkinnedMeshRenderer>();
        if (skinrenderers != null)
        {
            int length = skinrenderers.Length;
            for (int i = 0; i < length; i++)
            {
                skinrenderers[i].reflectionProbeUsage = usage;
            }
        }
    }

    /// <summary>
    /// 设置layer
    /// </summary>
    /// <param name="go"></param>
    /// <param name="layerName"></param>
    public static void SetLayer(this GameObject go, string layerName)
    {
        if (go == null)
            return;

        int layer = LayerMask.NameToLayer(layerName);
        if (layer < 0)
            return;

        Transform[] trans = go.GetComponentsInChildren<Transform>();

        foreach (Transform item in trans)
        {
            item.gameObject.layer = layer;
        }
    }

    /// <summary>
    /// 清除无效的animator
    /// </summary>
    /// <param name="go"></param>
    public static void ClearNoUseAnimator(this GameObject go)
    {
        if (go == null)
            return;

        Animator[] anims = go.GetComponentsInChildren<Animator>();
        if (anims == null)
            return;
        int length = anims.Length;
        for (int i = 0; i < length; i++)
        {
            if (anims[i].runtimeAnimatorController == null)
            {
                GameObject.DestroyImmediate(anims[i], true);
            }
        }
    }

    public static GameObject FindInChildren(this GameObject go, string name)
    {
        GameObject selectedGO = null;
        if (go != null && name != null)
        {
            if ((from x in go.GetComponentsInChildren<Transform>()
                 where x.gameObject.name == name
                 select x.gameObject).Any())
            {
                selectedGO = (from x in go.GetComponentsInChildren<Transform>()
                              where x.gameObject.name == name
                              select x.gameObject).First();
            }
        }
        return selectedGO;

    }
}

public static class TransformExpand
{
    public static void ClearChilds(this Transform tran)
    {
        if (tran == null)
        {
            return;
        }

        foreach (Transform item in tran)
        {
            GameObject.Destroy(item.gameObject);
        }
    }
}

public static class AssetBundleExpand
{
    /// <summary>
    /// refresh shader
    /// </summary>
    /// <param name="ab"></param>
    public static void RefreshShader(this AssetBundle ab)
    {
        if (ab == null)
            return;

        Material[] ms = ab.LoadAllAssets<Material>();
        int length = ms.Length;
        for (int i = 0; i < length; i++)
        {
            string shaderName = ms[i].shader.name;
            Shader newShader = Shader.Find(shaderName);
            if (newShader != null)
            {
                ms[i].shader = newShader;
            }
            else
            {
                Debug.LogWarning("unable to refresh shader: " + shaderName + " in material " + ms[i].name);
            }
        }
    }
}

public static class AnimatorExpand
{

    /// <summary>
    /// 后去动画运行normalizedTime 0~1
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="layerindex"></param>
    /// <param name="layerName"></param>
    /// <param name="clip">动画名</param>
    /// <param name="normalizedTime"></param>
    /// <returns></returns>
    public static bool GetAnimNormalizedTime(this Animator animator, string layerName, string clip, out float normalizedTime)
    {
        normalizedTime = 0f;
        if (animator == null || string.IsNullOrEmpty(layerName) || string.IsNullOrEmpty(clip))
        {
            return false;
        }

        int layerindex = animator.GetLayerIndex(layerName);

        if (layerindex < 0)
        {
            return false;
        }

        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(layerindex);
        string layename = string.Format("{0}.{1}", layerName, clip);
        if (info.IsName(layename))
        {
            normalizedTime = info.normalizedTime;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 当前播放动画
    /// </summary>
    /// <param name="animator"></param>
    /// <returns></returns>
    public static string GetCurrentClipName(this Animator animator, string layerName)
    {
        if (animator == null || string.IsNullOrEmpty(layerName))
            return null;

        int layerindex = animator.GetLayerIndex(layerName);
        if (layerindex < 0)
        {
            return null;
        }

        AnimatorClipInfo[] infos = animator.GetCurrentAnimatorClipInfo(layerindex);
        if (infos != null && infos.Length > 0)
        {
            return infos[0].clip.name;
        }
        return null;
    }
}

public static class CameraExpand
{

    /// <summary>
    /// 点击是否在相机视野内
    /// </summary>
    /// <param name="cam"></param>
    /// <returns></returns>
    public static bool TouchInCamView(this Camera cam)
    {
        if (cam == null)
            return false;

        Vector2 viewpoint =
#if UNITY_EDITOR
 cam.ScreenToViewportPoint(Input.mousePosition);
#else
 cam.ScreenToViewportPoint(Input.GetTouch(0).position);
#endif
        if (viewpoint.y < 0 || viewpoint.x == 0 || viewpoint.x == 1)
            return false;
        return true;
    }

    /// <summary>
    /// 点击 射线检测
    /// </summary>
    /// <param name="cam"></param>
    /// <param name="layer"></param>
    /// <param name="distance"></param>
    /// <param name="hit"></param>
    /// <returns></returns>
    public static bool ScreenToRay(this Camera cam, int layer, float distance, out RaycastHit hit)
    {
        if (cam == null)
        {
            hit = new RaycastHit();
            return false;
        }

        Ray ray =
#if (UNITY_STANDALONE || UNITY_EDITOR)
 cam.ScreenPointToRay(Input.mousePosition);
#else
 cam.ScreenPointToRay(Input.GetTouch(0).position);
#endif

        return Physics.Raycast(ray, out hit, distance, layer);
    }
}

public static class TExpand
{
    /// <summary>
    /// 对象转换为数据流存在本地
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <param name="path"></param>
    public static void SaveToLocal<T>(this T type, string path)
    {
        //.Rec为存储的文件后缀名可自己随便定义但读写的要一致！

        int index = path.LastIndexOf("/");
        if (index > 0)
        {
            string p = path.Substring(0, index);
            if (!Directory.Exists(p))
            {
                Directory.CreateDirectory(p);
            }
        }


        FileStream fs = new FileStream(path, FileMode.Create);
        fs = ToFileStream<T>(type, fs);
        fs.Close();
        fs.Dispose();
        Debug.Log("SaveToLocal Success!" + path);
    }

    /// <summary>
    /// 对象序列化为文件流
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <param name="fs"></param>
    /// <returns></returns>
    private static FileStream ToFileStream<T>(T type, FileStream fs)
    {
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, type);
        return fs;
    }

    /// <summary>
    /// 对象序列化为字节流
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <returns></returns>
    public static byte[] ToBytes<T>(this T type)
    {
        //BinaryFormatter bf = new BinaryFormatter();


        //MemoryStream ms = new MemoryStream();
        //bf.Serialize(ms, type);
        //return ms.GetBuffer();
        using (MemoryStream ms = new MemoryStream())
        {
            IFormatter formatter = new BinaryFormatter(); formatter.Serialize(ms, type);
            return ms.GetBuffer();
        }
    }
    /// <summary>
    /// 深层复制
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="RealObject"></param>
    /// <returns></returns>
    public static T Copy<T>(this T RealObject)
    {
        using (Stream objectStream = new MemoryStream())
        {
            //利用 System.Runtime.Serialization序列化与反序列化完成引用对象的复制     
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(objectStream, RealObject);
            objectStream.Seek(0, SeekOrigin.Begin);
            return (T)formatter.Deserialize(objectStream);
        }
    }

}

public static class StringExpand
{
    /// <summary>
    /// 本地数据流转换为对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public static T ReadFromLocal<T>(this string path)
    {
        FileStream fs = null;
        try
        {
            fs = new FileStream(path, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            T t = (T)bf.Deserialize(fs);
            fs.Close();
            fs.Dispose();
            return t;
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            if (fs != null)
            {
                fs.Close();
            }
        }

        return default(T);
    }


    /// <summary>
    /// 文件流反序列化为对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fs"></param>
    /// <returns></returns>
    private static T FileStreamTo<T>(FileStream fs)
    {
        BinaryFormatter bf = new BinaryFormatter();
        T t = (T)bf.Deserialize(fs);
        fs.Close();
        fs.Dispose();
        return t;
    }


    /// <summary>
    /// 将string转换成byte[]
    /// </summary>
    /// <param name="pXmlString"></param>
    /// <returns></returns>
    public static byte[] StringToUTF8ByteArray(this string pXmlString)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        byte[] byteArray = encoding.GetBytes(pXmlString);
        return byteArray;
    }

    public static string StringToUTF8(this string str)
    {
        UTF8Encoding utf8 = new UTF8Encoding();
        byte[] encodedBytes = utf8.GetBytes(str);
        string decodedString = utf8.GetString(encodedBytes);
        return decodedString;
    }


    public static byte[] ReadFileByIO(this string path)
    {
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        fileStream.Seek(0, SeekOrigin.Begin);
        //创建文件长度缓冲区
        byte[] bytes = new byte[fileStream.Length];
        //读取文件
        fileStream.Read(bytes, 0, (int)fileStream.Length);
        //释放文件读取流
        fileStream.Close();
        fileStream.Dispose();
        fileStream = null;
        return bytes;
    }

    public static float ToFloat(this string str)
    {
        return float.Parse(str);
    }
}

public static class ByteExpand
{
    /// <summary>
    /// 将byte[]转换成string
    /// </summary>
    /// <param name="characters"></param>
    /// <returns></returns>
    public static string UTF8ByteArrayToString(this byte[] mbyte)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        string constructedString = encoding.GetString(mbyte);
        return (constructedString);
    }


    /// <summary>
    /// 字节流反序列化为对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static T BytesTo<T>(this byte[] bytes)
    {
        using (MemoryStream ms = new MemoryStream(bytes))
        {
            IFormatter formatter = new BinaryFormatter();
            return (T)formatter.Deserialize(ms);
        }
    }
    
    public static void SaveToLocal(this byte[] info, string path)
    {
        if (info == null || info.Length == 0)
            return;

        int index = path.LastIndexOf("/");
        if (index > 0)
        {
            string p = path.Substring(0, index);
            if (!Directory.Exists(p))
            {
                Directory.CreateDirectory(p);
            }
        }

        var tmPath = path + ".tmp";
        try
        {
            FileInfo t = new FileInfo(tmPath);
            Stream sw = t.Create();
            sw.Write(info, 0, info.Length);
            sw.Close();
            sw.Dispose();

            t.MoveTo(path);
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}

public static class ObjectExpand
{
    public static void RefreshShader(this UnityEngine.Object obj)
    {
        GameObject o = obj as GameObject;
        o.RefreshShader();
    }

    public static void ClearNoUseAnimator(this UnityEngine.Object obj)
    {
        GameObject o = obj as GameObject;
        o.ClearNoUseAnimator();
    }
}

public static class DictionaryExpand
{
    /// <summary>
    /// 提供一个方法遍历所有项
    /// </summary>
    public static void Foreach<TKey, TValue>(this Dictionary<TKey, TValue> dic, CallBackWithParamsReturn<TKey, TValue, bool> action)
    {
        if (action == null) return;
        var enumerator = dic.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (!action(enumerator.Current.Key, enumerator.Current.Value))
            {
                break;
            }
        }
    }

    /// <summary>
    /// 提供一个方法遍历所有key值
    /// </summary>
    public static void ForeachKey<TKey, TValue>(this Dictionary<TKey, TValue> dic, CallBackWithParamsReturn<TKey, bool> action)
    {
        if (action == null) return;
        var enumerator = dic.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (!action(enumerator.Current.Key))
            {
                break;
            }

        }
    }

    /// <summary>
    /// 提供一个方法遍历所有value值
    /// </summary>
    public static void ForeachValue<TKey, TValue>(this Dictionary<TKey, TValue> dic, CallBackWithParamsReturn<TValue, bool> action)
    {
        if (action == null) return;
        var enumerator = dic.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (!action(enumerator.Current.Value))
            {
                break;
            }
        }
    }
}

public static class Util
{
    /// <summary>
    /// 获取物体LookAt后的旋转值
    /// </summary>
    /// <param name="original"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static Vector3 GetLookAtEuler(Vector3 original, Vector3 target)
    {
        //计算物体在朝向某个向量后的正前方
        Vector3 forwardDir = target - original;


        //计算朝向这个正前方时的物体四元数值
        Quaternion lookAtRot = Quaternion.LookRotation(forwardDir);


        //把四元数值转换成角度
        Vector3 resultEuler = lookAtRot.eulerAngles;

        return resultEuler;
    }

    /// <summary>
    /// 获取物体LookAt后的旋转值
    /// </summary>
    /// <param name="original"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static Quaternion GetLookAtRotate(Vector3 original, Vector3 target)
    {
        //计算物体在朝向某个向量后的正前方
        Vector3 forwardDir = target - original;


        //计算朝向这个正前方时的物体四元数值
        Quaternion lookAtRot = Quaternion.LookRotation(forwardDir);

        return lookAtRot;
    }

    /// <summary>
    /// 保留几位小数
    /// </summary>
    /// <param name="source"></param>
    /// <param name="num"></param>
    /// <returns></returns>
    public static float Parse(float source, int num)
    {
        int d = (int)UnityEngine.Mathf.Pow(10, num);
        int j = (int)(source * d);
        source = j / (float)d;
        return source;
    }

    /// <summary>  
    /// 获取时间戳  
    /// </summary>  
    /// <returns></returns>  
    public static long GetTimeStampSec()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalSeconds);
    }

    public static long GetTimeStampMsec()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalMilliseconds);
    }

    public static MaterialPropertyBlock GetMatPropertyBlock(this Renderer render)
    {
        MatPropertyBlock mpb = render.gameObject.GetComponent<MatPropertyBlock>();
        if (mpb == null)
        {
            mpb = render.gameObject.AddComponent<MatPropertyBlock>();
        }

        return mpb.block;
    }
}


