using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Rokudan : EnemyMob
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

    }

    public override void Attack()
    {

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
