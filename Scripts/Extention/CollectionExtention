using System.Collections.Generic;
/// <summary>
/// Collection擴充方法
/// </summary>
public static class CollectionExtention
{
    /// <summary>
    /// Array是否為空
    /// </summary>
    public static bool isNullOrEmpty<T>(this T[] list)
    {
        return list == null || list.Length == 0;
    }

    /// <summary>
    /// List是否為空
    /// </summary>
    public static bool isNullOrEmpty<T>(this List<T> list)
    {
        return list == null || list.Count == 0;
    }

    /// <summary>
    /// Queue是否為空
    /// </summary>
    public static bool isNullOrEmpty<T>(this Queue<T> list)
    {
        return list == null || list.Count == 0;
    }

    /// <summary>
    /// HashSet是否為空
    /// </summary>
    public static bool isNullOrEmpty<T>(this HashSet<T> list)
    {
        return list == null || list.Count == 0;
    }

    /// <summary>
    /// Dictionary是否為空
    /// </summary>
    public static bool isNullOrEmpty<K, V>(this Dictionary<K, V> dic)
    {
        return dic == null || dic.Count == 0;
    }

    /// <summary>
    /// Dictionary是否有該Key
    /// </summary>
    public static bool haveKey<K, V>(this Dictionary<K, V> dic, K key)
    {
        return dic != null && dic.ContainsKey(key);
    }

    /// <summary>
    /// 將Dictionary值轉換成List 並對資料做轉型委派
    /// </summary>
    public static List<R> CastList<T, R>(this List<T> list, System.Func<T, R> castAction)
    {
        if (list == null) return null;

        List<R> resultList = new List<R>();

        for (int x = 0, count = list.Count; x < count; x++)
        {
            resultList.Add(castAction(list[x]));
        }

        return resultList;
    }

    /// <summary>
    /// 將Dictionary值轉換成List
    /// </summary>
    /// <returns></returns>
    public static List<V> ConvertToList<K, V>(this Dictionary<K, V> dic)
    {
        if (dic == null) return null;

        Dictionary<K, V>.Enumerator enumarator = dic.GetEnumerator();
        List<V> list = new List<V>();

        while (enumarator.MoveNext())
        {
            list.Add(enumarator.Current.Value);
        }

        return list;
    }

    /// <summary>
    /// 將Dictionary值轉換成List 並對資料做轉型委派
    /// </summary>
    public static List<R> ConvertAndCastToList<K, V, R>(this Dictionary<K, V> dic, System.Func<V, R> castAction)
    {
        if (dic == null) return null;

        Dictionary<K, V>.Enumerator enumarator = dic.GetEnumerator();
        List<R> list = new List<R>();

        while (enumarator.MoveNext())
        {
            list.Add(castAction(enumarator.Current.Value));
        }

        return list;
    }
}
