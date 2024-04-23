using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneratorBase : MonoBehaviour
{
    public virtual List<List<string>> GenerateDungeon(DungeonData data)
    {
        return new();
    }
    public virtual List<List<string>> GenerateDungeon(Vector2Int size)
    {
        return new();
    }
}
