using System;
using UnityEngine;

// �ۑ����������� �� enum �̑��ݕϊ����邽�߂̃��[�e�B���e�B
public static class SerializeUtil
{
    // enum �� �u�l:�V���{�����v�`���ɕϊ�����
    public static string Convert(Enum value)
    {
        return string.Format("{0}:{1}", (int)(object)value, value.ToString());
    }

    // �u�l:�V���{�����v�`�� �� enum�ɕϊ�����
    public static T Restore<T>(string value) => (T)(object)Restore(typeof(T), value);

    // �u�l:�V���{�����v�`�� �� enum�ɕϊ�����
    public static Enum Restore(Type enumType, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return default; // �ǂݎ��Ȃ��ꍇ�K��l��Ԃ�
        }

        // �񋓌^�̒l�Ɩ��O���擾���Ēl�𕜌����鏈��
        string valueText = value;

        // �r�����炱�̕����ɐ؂�ւ������ɐ��l���������ĂȂ����̑Ή�
        string[] splitTexts = valueText.Split(':');
        if (splitTexts.Length == 1 && TryParseAll(enumType, splitTexts[0], out Enum result1))
        {
            return result1;
        }

        // 2�ɕ����ł��鎞�̓V���{������D��ŕ�������
        if (Enum.TryParse(enumType, splitTexts[1], out object result2))
        {
            return (Enum)result2;
        }

        // ���̃V���{������enum�������ł��Ȃ�������O�̐�������v������̂�I������
        if (TryParseInt(enumType, splitTexts[0], out Enum result3))
        {
            return result3;
        }

        // �V���{�����Ɛ��l�𓯎��ɕύX�������A�V���{�����폜���Ė߂��Ȃ����Ȃǂɂ���
        Debug.LogWarningFormat($"{enumType.Name} �̕����Ɏ��s���܂����B Source={value}");
        return default;
    }

    // ���������� or enum������ �� enum �̕ϊ�
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
        result = default; // �ǂ�����Ă��ϊ��ł��Ȃ�
        return false;
    }

    // ����������("0") �� enum �̕ϊ�
    private static bool TryParseInt(Type enumType, string value, out Enum result)
    {
        if (int.TryParse(value, out int tmpInt) && Enum.IsDefined(enumType, tmpInt))
        {
            result = (Enum)Enum.Parse(enumType, tmpInt.ToString());
            return true;
        }
        result = default; // ��`����ĂȂ����l�̎w��͕ω��ł��Ȃ�����
        return false;
    }
}