using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 辞書配列をインスペクターに表示するためのクラス
/// </summary>
/// <typeparam name="TKey">キー</typeparam>
/// <typeparam name="TValue">値</typeparam>
[Serializable]
public class SerializableKeyPair<TKey, TValue>
{
    [SerializeField] private TKey key;
    [SerializeField] private TValue value;

    //  キー
    public TKey Key => key;
    //  値
    public TValue Value => value;

    //  リストを辞書配列に変換
    public static Dictionary<TKey, TValue> ConvertToDictionaly(List<SerializableKeyPair<TKey, TValue>> list)
    {
        //  返し用
        Dictionary<TKey, TValue> dic = list.ToDictionary(n => n.Key, n => n.Value);

        return dic;
    }
}

/// <summary>
/// 辞書配列をインスペクターに表示するためのクラス
/// </summary>
/// <typeparam name="TEnum">キー(列挙型)</typeparam>
/// <typeparam name="TValue">値</typeparam>
[Serializable]
public class SerializableKeyPairCustomEnum<TEnum, TValue>
{
    [SerializeField][CustomEnum(typeof(ToolData.ToolType))] private string key;
    [SerializeField] private TValue value;

    //  キー
    public TEnum Key => SerializeUtil.Restore<TEnum>(key);
    //  値
    public TValue Value => value;

    //  リストを辞書配列に変換
    public static Dictionary<TEnum, TValue> ConvertToDictionaly(List<SerializableKeyPairCustomEnum<TEnum, TValue>> list)
    {
        //  返し用
        Dictionary<TEnum, TValue> dic = list.ToDictionary(n => n.Key, n => n.Value);

        return dic;
    }
}