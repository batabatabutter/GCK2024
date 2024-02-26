using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KawaseDemo : MonoBehaviour
{
    //ツールたち
    [SerializeField] List<Tool> tool;
    //とりあえずUIに選択中のツールを表示するよう
    [SerializeField] Text text;

    [SerializeField] Camera cam;

    //選ばれているツール番号
    private int selectToolNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //左クリック
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);

            pos.z = 0.0f;

            tool[selectToolNum].Plant(pos);
        }

        //回転の取得
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        //ホイールで選択をチェンジ
        if (scroll > 0 && selectToolNum < tool.Count - 1) selectToolNum++;
        if (scroll < 0 && selectToolNum > 0) selectToolNum--;

        string toolName = "";

        switch (selectToolNum)
        {
            case 0:
                toolName = "松明";
                break;
            case 1:
                toolName = "爆弾";
                break;
            default:
                break;
        }


        text.text = "選択ツール:" + toolName;

    }
}
