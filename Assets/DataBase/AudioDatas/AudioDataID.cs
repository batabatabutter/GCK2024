[System.Serializable]
public enum AudioDataID
{
    Dig,
    Bomb,

    //  プレイヤーダメージ回り
    GetDamage,
    GetDamageArmor,
    BreakArmor,
    DeadPlayer,

    //  システム
    Select,
    Correct,
    Change,
    
    // アイテム
    PIC_UP,

    // ツール
    ARMOR_USE,      // アーマー使用
    ARMOR_BREAK,    // アーマー破壊
    BOMB_PUT,       // 爆弾設置
    BOMB_BLOW,      // 爆破
    BOOST_USE,      // ブースト使用
    BOOST_EFFECT,   // ブースト効果
    HEALING_USE,    // 回復使用
    HEALING_EFFECT, // 回復効果

    OverID
}
