using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneratorBase : MonoBehaviour
{
    public virtual void SetDungeonData(DungeonData dungeonData)
    {

    }

    public virtual List<List<string>> GenerateDungeon(DungeonData data)
    {
        return new();
    }

}
