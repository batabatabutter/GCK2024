using System;
using UnityEngine;

// �J�X�^��Enum�^���������߂̃N���X
public class CustomEnumAttribute : PropertyAttribute
{
    public readonly Type Type;

    public CustomEnumAttribute(Type enumType) => Type = enumType;

    // �����Ɏw�肳��Ă���^��enum���ǂ������擾����
    // true: enum�ł��� / false: enum�łȂ�
    public bool IsEnum => Type != null && Type.IsEnum;
}