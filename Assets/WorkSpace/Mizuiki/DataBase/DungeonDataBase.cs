using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonDataBase", menuName = "CreateDataBase/Dungeon/DungeonDataBase")]
public class DungeonDataBase : ScriptableObject
{
    // ダンジョンの数
    static public int DUNGEON_COUNT = 5;

    public List<DungeonData> dungeonDatas;


}