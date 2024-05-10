using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessChild : MonoBehaviour
{
    //  �X�N���v�g
    private List<MonoBehaviour> m_scripts = new List<MonoBehaviour>();
    //  �Փ˔���
    private List<Collider2D> m_colldier2Ds = new List<Collider2D>();

    //  �����y���p
    public void Change(bool flag)
    {
        foreach (var script in m_scripts)
        {
            //if(!script)
            //{
            //    m_scripts = new List<MonoBehaviour>(transform.GetComponentsInChildren<MonoBehaviour>().Skip(1));
            //}
            script.enabled = flag;
        }
        foreach (var coll in m_colldier2Ds) coll.enabled = flag;
    }

    public List<MonoBehaviour> Scripts
    {
        get { return m_scripts; }
        set { m_scripts = value; }
    }

    public List<Collider2D> Collider2Ds
    { 
        get { return m_colldier2Ds; }
        set { m_colldier2Ds = value; }
    }
}
