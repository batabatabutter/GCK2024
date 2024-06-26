using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [Header("ƒvƒŒƒCƒ„[‚Ì–Ú")]
    [SerializeField] private SpriteRenderer m_playerEye = null;

    [Header("–Ú‚ÌƒXƒvƒ‰ƒCƒg"), Tooltip("0->ŠJ‚¢‚Ä‚é, 1->•Â‚¶‚Ä‚é")]
    [SerializeField] private List<Sprite> m_eyeSprites = new();

    [Header("u‚«‚ÌŠÔ")]
    [SerializeField] private float m_blinkTime = 0.1f;
    private float m_blinkTimer = 0.0f;

    [Header("•½‹Ï“I‚Èu‚«ŠÔŠu(•b)")]
    [SerializeField] private float m_blinkInterval = 1.0f;

    [Header("ƒvƒŒƒCƒ„[‚ÌÌŒ@î•ñ")]
    [SerializeField] private PlayerMining m_playerMining = null;

    [Header("ˆÊ’u‚ÌU‚ê•")]
    [SerializeField] private Vector2 m_amplitude = Vector2.one;



    // Start is called before the first frame update
    void Start()
    {
        // ÌŒ@‚Ìæ“¾
        if (m_playerMining == null)
        {
            m_playerMining = GetComponent<PlayerMining>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ÌŒ@‚ª‚È‚¯‚ê‚Îˆ—‚µ‚È‚¢
        if (m_playerMining == null)
            return;

        // ÌŒ@•ûŒü
        Vector2 miningVec = m_playerMining.GetMiningVector();
        // ÌŒ@”¼Œa
        float miningRange = m_playerMining.MiningRange;
        // ˆÚ“®Š„‡
        float rate = miningVec.magnitude / miningRange;

        // À•Wİ’è
        m_playerEye.transform.position = m_playerMining.transform.position + (Vector3)(m_amplitude * miningVec.normalized * rate);

        // u‚«
        Blink();

    }



    // u‚«
    private void Blink()
    {
        // –Ú‚ğ•Â‚¶‚Ä‚é
        if (m_blinkTimer > 0.0f)
        {
            // ŠÔŒo‰ß
            m_blinkTimer -= Time.deltaTime;
            return;
        }

        float rand = Random.Range(0.0f, 1.0f);

        if (rand >= 1.0f / m_blinkInterval * Time.deltaTime)
        {
            m_playerEye.sprite = m_eyeSprites[0];
        }
        else
        {
            m_playerEye.sprite = m_eyeSprites[1];
            m_blinkTimer = m_blinkTime;
        }
    }
}
