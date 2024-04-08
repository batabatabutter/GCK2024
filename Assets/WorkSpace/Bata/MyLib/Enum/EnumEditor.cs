#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;

// string�̃t�B�[���h��enum���X�g�\���ɂ���Editor�g��
[CustomPropertyDrawer(typeof(CustomEnumAttribute))]
public class EnumEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent content)
    {
        var att = attribute as CustomEnumAttribute;
        if (property.propertyType != SerializedPropertyType.String)
        {
            EditorGUI.PropertyField(position, property);
            Debug.LogWarning($"string�����o�[�ȊO��{nameof(CustomEnumAttribute)}" +
                $"�������w�肵�Ȃ��ł��������B" +
                $"�I�u�W�F�N�g={property.serializedObject.targetObject}, �ϐ���={property.name}");
            return;
        }
        if (!att.IsEnum)
        {
            EditorGUI.PropertyField(position, property);
            Debug.LogWarning($"�^����enum���w�肳��Ă��܂���B" +
                $"�I�u�W�F�N�g={property.serializedObject.targetObject}, �ϐ���={property.name}");
            return;
        }

        // ������ŕۑ�����Ă���l��enum�ɕ�������
        Enum value = SerializeUtil.Restore(att.Type, property.stringValue);
        if (value == null)
        {
            value = (Enum)Enum.GetValues(att.Type).GetValue(0);
        }

        var label = new GUIContent(property.displayName);
        Enum selected = null; // �I������

#if ODIN_INSPECTOR
        // OdinInspector ���g�p���Ă���Ƃ��͊g�������{�b�N�X��\�����鏈��
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