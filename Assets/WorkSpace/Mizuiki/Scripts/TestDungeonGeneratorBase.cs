using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDungeonGeneratorBase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public virtual List<List<string>> GenerateDungeon(Vector2Int size)
    {
        return new();
    }

}
