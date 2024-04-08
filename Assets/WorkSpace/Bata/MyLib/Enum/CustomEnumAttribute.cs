using System;
using UnityEngine;

// カスタムEnum型を扱うためのクラス
public class CustomEnumAttribute : PropertyAttribute
{
    public readonly Type Type;

    public CustomEnumAttribute(Type enumType) => Type = enumType;

    // 属性に指定されている型がenumかどうかを取得する
    // true: enumである / false: enumでない
    public bool IsEnum => Type != null && Type.IsEnum;
}