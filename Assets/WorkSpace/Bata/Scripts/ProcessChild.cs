using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProcessChild : MonoBehaviour
{
    //  スクリプト
    private List<MonoBehaviour> m_scripts = new List<MonoBehaviour>();
    //  衝突判定
    private List<Collider2D> m_colldier2Ds = new List<Collider2D>();

    //  処理軽減用
    public void Change(bool flag)
    {
        Scripts = new List<MonoBehaviour>(this.transform.GetComponentsInChildren<MonoBehaviour>().Skip(1));
        Collider2Ds = new List<Collider2D>(transform.GetComponentsInChildren<Collider2D>().Skip(1));

        foreach (var script in m_scripts) script.enabled = flag;
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
