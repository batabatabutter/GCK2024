using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonDataBase", menuName = "CreateDataBase/Dungeon/DungeonDataBase")]
public class DungeonDataBase : ScriptableObject
{
    // ƒ_ƒ“ƒWƒ‡ƒ“‚Ì”
    static public int DUNGEON_COUNT = 5;

    public List<DungeonData> dungeonDatas;


}