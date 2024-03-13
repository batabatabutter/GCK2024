using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySceneManager : MonoBehaviour
{
    //  �v���C���[
    private GameObject m_player;
    //  �R�A
    private List<GameObject> m_cores;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //  �v���C���[�̐ݒ�
    public void SetPlayer(GameObject player) { m_player = player; }
    //  �v���C���[�̎擾
    public GameObject GetPlayer() { return m_player; }

    //  �R�A�̐ݒ�
    public void AddCore(GameObject core) { m_cores.Add(core); }
    //  �R�A�B�̎擾
    public List<GameObject> GetCores() { return m_cores; }
}