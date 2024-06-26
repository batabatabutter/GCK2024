using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FallRock : MonoBehaviour
{
    [Header("落下までの時間")]
    public float fallTime = 3.0f;
    [Header("攻撃力")]
    public int damage = 1;
    [Header("地面についた後の余韻")]
    [SerializeField] float extraTime = 0.5f;

    private GameObject m_highLight = null;

    Rigidbody2D rd;

    float scale = 0.0f;

    float keepX;

    bool ishitPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody2D>();

        rd.gravityScale = 0.0f;

        transform.localScale = Vector3.zero;

        keepX = transform.transform.position.x;

        ishitPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        fallTime -= Time.deltaTime;

        if(transform.lossyScale.x < 1.0f)
        {
            scale += Time.deltaTime * 0.5f;
        }

        if(fallTime < 0)
        {
            rd.gravityScale = 1.0f;

            transform.position = new Vector3(keepX,transform.position.y,transform.position.z);
        }
        else
        {
            transform.localScale = new Vector3(scale,scale,scale);

            transform.position = new Vector3(keepX + (Mathf.Sin(Time.time * 20) / 4), transform.position.y, transform.position.z);
        }


        if(ishitPlayer)
        {
            extraTime -= Time.deltaTime;
        }


        if(extraTime < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out FallRockHighlight highLight))
        {
            if (m_highLight == highLight.gameObject)
            {
				ishitPlayer = true;

				rd.AddForce(new Vector3(0.0f, 10.0f, 0.0f), ForceMode2D.Impulse);
			}
		}
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && ishitPlayer && !collision.isTrigger)
        {
            collision.GetComponent<Player>().AddDamage(damage);

            Destroy(gameObject);
        }
    }

    // ハイライト設定
    public void SetHighLight(GameObject highLight)
    {
        m_highLight = highLight;
    }


}
