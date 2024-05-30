using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBack : MonoBehaviour
{
    [System.Serializable]
    public struct BackBlocks
    {
        public GameObject block;
        [Range(0f, 1f)] public float rate;
    }

    [Header("�w�i�p")]
    [SerializeField] private List<BackBlocks> m_backdatas;

    [Header("�͈�")]
    [SerializeField] private Vector2 m_range;

    private List<GameObject> m_backs = new List<GameObject>();

    /// <summary>
    /// �ŏ��̍X�V��
    /// </summary>
    private void Start()
    {
        for (int x = (int)transform.position.x; x < m_range.x; x++)
        {
            for (int y = (int)transform.position.y; y < m_range.y; y++)
            {
                float rand = Random.Range(0.0f, 1.0f);
                float totalRand = 0.0f;
                GameObject gameObject = m_backdatas[0].block;
                foreach (BackBlocks ba in m_backdatas) 
                {
                    totalRand += ba.rate;
                    if(totalRand <= rand)
                    {
                        gameObject = ba.block;
                    }
                }
                m_backs.Add(Instantiate(gameObject, new Vector2(x, y), Quaternion.identity));
            }
        }
    }


    /// <summary>
    /// �X�V
    /// </summary>
    private void Update()
    {
        
    }
}
