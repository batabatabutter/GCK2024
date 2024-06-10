using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [Header("�v���C���[�̖�")]
    [SerializeField] private SpriteRenderer m_playerEye = null;

    [Header("�ڂ̃X�v���C�g")]
    [SerializeField] private List<Sprite> m_eyeSprites = new();

    [Header("�v���C���[�̍̌@���")]
    [SerializeField] private PlayerMining m_playerMining = null;

    [Header("�ʒu�̐U�ꕝ")]
    [SerializeField] private Vector2 m_amplitude = Vector2.one;



    // Start is called before the first frame update
    void Start()
    {
        // �̌@�̎擾
        if (m_playerMining == null)
        {
            m_playerMining = GetComponent<PlayerMining>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // �̌@���Ȃ���Ώ������Ȃ�
        if (m_playerMining == null)
            return;

        // �̌@����
        Vector2 miningVec = m_playerMining.GetMiningVector();
        // �̌@���a
        float miningRange = m_playerMining.MiningRange;
        // �ړ�����
        float rate = miningVec.magnitude / miningRange;

        // ���W�ݒ�
        m_playerEye.transform.position = m_playerMining.transform.position + (Vector3)(m_amplitude * miningVec.normalized * rate);

    }
}
