using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
/// <summary>
/// 継承順
/// Enemy -> EnemyDwell -> Dorinoko
/// </summary>
public class Enemy_Dorinoko : EnemyDwell
{
    // Start is called before the first frame update

    [Header("どりル")]
    [SerializeField] GameObject m_dori;
    protected override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (base.Player != null)
        {
            Transform target = base.Player.transform;
            // ターゲットの方向ベクトルを計算
            Vector3 direction = target.position - transform.position;

            // 方向ベクトルを正規化
            direction.Normalize();

            // 方向ベクトルから角度を計算
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // 角度を四捨五入して、0°、90°、180°、270°のいずれかに丸める
            angle = Mathf.Round(angle / 90) * 90 - 90;

            // オブジェクトを回転
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public override void Attack()
    {
        Instantiate(m_dori,gameObject.transform.position,Quaternion.identity,gameObject.transform);

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

    }

}
