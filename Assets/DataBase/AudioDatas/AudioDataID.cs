[System.Serializable]
public enum AudioDataID
{
    Dig,
    Bomb,

    //  �v���C���[�_���[�W���
    GetDamage,
    GetDamageArmor,
    BreakArmor,
    DeadPlayer,

    //  �V�X�e��
    Select,
    Correct,
    Change,
    
    // �A�C�e��
    PIC_UP,

    // �c�[��
    ARMOR_USE,      // �A�[�}�[�g�p
    ARMOR_BREAK,    // �A�[�}�[�j��
    BOMB_PUT,       // ���e�ݒu
    BOMB_BLOW,      // ���j
    BOOST_USE,      // �u�[�X�g�g�p
    BOOST_EFFECT,   // �u�[�X�g����
    HEALING_USE,    // �񕜎g�p
    HEALING_EFFECT, // �񕜌���

    OverID
}
