using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �����z����C���X�y�N�^�[�ɕ\�����邽�߂̃N���X
/// </summary>
/// <typeparam name="TKey">�L�[</typeparam>
/// <typeparam name="TValue">�l</typeparam>
[Serializable]
public class SerializableKeyPair<TKey, TValue>
{
    [SerializeField] private TKey key;
    [SerializeField] private TValue value;

    //  �L�[
    public TKey Key => key;
    //  �l
    public TValue Value => value;

    //  ���X�g�������z��ɕϊ�
    public static Dictionary<TKey, TValue> ConvertToDictionaly(List<SerializableKeyPair<TKey, TValue>> list)
    {
        //  �Ԃ��p
        Dictionary<TKey, TValue> dic = list.ToDictionary(n => n.Key, n => n.Value);

        return dic;
    }
}

/// <summary>
/// �����z����C���X�y�N�^�[�ɕ\�����邽�߂̃N���X
/// </summary>
/// <typeparam name="TEnum">�L�[(�񋓌^)</typeparam>
/// <typeparam name="TValue">�l</typeparam>
[Serializable]
public class SerializableKeyPairCustomEnum<TEnum, TValue>
{
    [SerializeField][CustomEnum(typeof(ToolData.ToolType))] private string key;
    [SerializeField] private TValue value;

    //  �L�[
    public TEnum Key => SerializeUtil.Restore<TEnum>(key);
    //  �l
    public TValue Value => value;

    //  ���X�g�������z��ɕϊ�
    public static Dictionary<TEnum, TValue> ConvertToDictionaly(List<SerializableKeyPairCustomEnum<TEnum, TValue>> list)
    {
        //  �Ԃ��p
        Dictionary<TEnum, TValue> dic = list.ToDictionary(n => n.Key, n => n.Value);

        return dic;
    }
}