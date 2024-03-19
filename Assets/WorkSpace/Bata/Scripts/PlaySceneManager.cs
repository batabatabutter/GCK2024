using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaySceneManager : MonoBehaviour
{
    //  �v���C���[
    private GameObject m_player;
    //  �R�A
    private GameObject m_core;
    private List<GameObject> m_cores;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //  �Q�[���J�ڊm�F
        //  �Q�[���N���A
        if(m_core == null || m_core.IsDestroyed() || !m_core.activeInHierarchy)
        {
            Debug.Log("Game Clear");
        }
        //  �Q�[���I�[�o�[
        else if(m_player.GetComponent<Player>().HitPoint <= 0)
        {
            Debug.Log("Game Over");
        }
    }

    //  �v���C���[�̐ݒ�
    public void SetPlayer(GameObject player) { m_player = player; }
    //  �v���C���[�̎擾
    public GameObject GetPlayer() { return m_player; }

    //  �R�A�̐ݒ�
    public void SetCore(GameObject core) { m_core = core; }
    //  �R�A�B�̎擾
    public GameObject GetCore() { return m_core; }
    //  �R�A�̒ǉ�
    public void AddCore(GameObject core) { m_cores.Add(core); }
    //  �R�A�B�̎擾
    public List<GameObject> GetCores() { return m_cores; }
}
