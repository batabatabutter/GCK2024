using System;
using UnityEngine;

// 保存した文字列 ⇔ enum の相互変換するためのユーティリティ
public static class SerializeUtil
{
    // enum → 「値:シンボル名」形式に変換する
    public static string Convert(Enum value)
    {
        return string.Format("{0}:{1}", (int)(object)value, value.ToString());
    }

    // 「値:シンボル名」形式 → enumに変換する
    public static T Restore<T>(string value) => (T)(object)Restore(typeof(T), value);

    // 「値:シンボル名」形式 → enumに変換する
    public static Enum Restore(Type enumType, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return default; // 読み取れない場合規定値を返す
        }

        // 列挙型の値と名前を取得して値を復元する処理
        string valueText = value;

        // 途中からこの方式に切り替えた時に数値しか入ってない時の対応
        string[] splitTexts = valueText.Split(':');
        if (splitTexts.Length == 1 && TryParseAll(enumType, splitTexts[0], out Enum result1))
        {
            return result1;
        }

        // 2つに分割できる時はシンボル名を優先で復元する
        if (Enum.TryParse(enumType, splitTexts[1], out object result2))
        {
            return (Enum)result2;
        }

        // 後ろのシンボルからenumが復元できなかったら前の数字が一致するものを選択する
        if (TryParseInt(enumType, splitTexts[0], out Enum result3))
        {
            return result3;
        }

        // シンボル名と数値を同時に変更した時、シンボルを削除して戻せない時などにくる
        Debug.LogWarningFormat($"{enumType.Name} の復元に失敗しました。 Source={value}");
        return default;
    }

    // 整数文字列 or enum文字列 → enum の変換
    private static bool TryParseAll(Type enumType, string value, out Enum result)
    {
        if (Enum.TryParse(enumType, value, out object tmpValue1))
        {
            result = (Enum)tmpValue1;
            return true;
        }
        else if (TryParseInt(enumType, value, out Enum tmpValue2))
        {
            result = tmpValue2;
            return true;
        }
        result = default; // どうやっても変換できない
        return false;
    }

    // 整数文字列("0") → enum の変換
    private static bool TryParseInt(Type enumType, string value, out Enum result)
    {
        if (int.TryParse(value, out int tmpInt) && Enum.IsDefined(enumType, tmpInt))
        {
            result = (Enum)Enum.Parse(enumType, tmpInt.ToString());
            return true;
        }
        result = default; // 定義されてない数値の指定は変化できない扱い
        return false;
    }
}