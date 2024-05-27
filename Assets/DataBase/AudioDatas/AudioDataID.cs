[System.Serializable]
public enum AudioDataID
{
    Put,
    Dig,
    BlockDestroy,
    Bomb,

    //  プレイヤー使用周り
    UseTool,

    //  プレイヤーダメージ回り
    GetDamage,
    GetDamageArmor,
    BreakArmor,
    DeadPlayer,

    //  システム
    Select,
    Correct,
    Change,
    

    OverID
}
