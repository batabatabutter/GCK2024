#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;

// stringのフィールドをenumリスト表示にするEditor拡張
[CustomPropertyDrawer(typeof(CustomEnumAttribute))]
public class EnumEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent content)
    {
        var att = attribute as CustomEnumAttribute;
        if (property.propertyType != SerializedPropertyType.String)
        {
            EditorGUI.PropertyField(position, property);
            Debug.LogWarning($"stringメンバー以外に{nameof(CustomEnumAttribute)}" +
                $"属性を指定しないでください。" +
                $"オブジェクト={property.serializedObject.targetObject}, 変数名={property.name}");
            return;
        }
        if (!att.IsEnum)
        {
            EditorGUI.PropertyField(position, property);
            Debug.LogWarning($"型情報にenumが指定されていません。" +
                $"オブジェクト={property.serializedObject.targetObject}, 変数名={property.name}");
            return;
        }

        // 文字列で保存されている値をenumに復元する
        Enum value = SerializeUtil.Restore(att.Type, property.stringValue);
        if (value == null)
        {
            value = (Enum)Enum.GetValues(att.Type).GetValue(0);
        }

        var label = new GUIContent(property.displayName);
        Enum selected = null; // 選択結果

#if ODIN_INSPECTOR
        // OdinInspector を使用しているときは拡張検索ボックスを表示する処理
        string typeName =
            $"Sirenix.OdinInspector.Editor.EnumSelector`1" +
            $"[[{att.Type.AssemblyQualifiedName}]], " +
            $"Sirenix.OdinInspector.Editor, Culture=neutral, PublicKeyToken=null";
        Type t = Type.GetType(typeName);

        var m = t.GetMethod("DrawEnumField", new[] { 
            typeof(Rect), typeof(GUIContent), 
            att.Type, typeof(GUIStyle) });
        selected = m.Invoke(null, new object[] { position, label, value, null }) as Enum;
#else
        selected = EditorGUI.EnumPopup(position, property.displayName, value);
#endif
        property.stringValue = SerializeUtil.Convert(selected);
    }
}
#endif