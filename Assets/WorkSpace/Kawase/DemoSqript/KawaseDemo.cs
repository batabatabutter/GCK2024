using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KawaseDemo : MonoBehaviour
{
    //�c�[������
    [SerializeField] List<Tool> tool;
    //�Ƃ肠����UI�ɑI�𒆂̃c�[����\������悤
    [SerializeField] Text text;

    [SerializeField] Camera cam;

    //�I�΂�Ă���c�[���ԍ�
    private int selectToolNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //���N���b�N
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);

            pos.z = 0.0f;

            tool[selectToolNum].Plant(pos);
        }

        //��]�̎擾
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        //�z�C�[���őI�����`�F���W
        if (scroll > 0 && selectToolNum < tool.Count - 1) selectToolNum++;
        if (scroll < 0 && selectToolNum > 0) selectToolNum--;

        string toolName = "";

        switch (selectToolNum)
        {
            case 0:
                toolName = "����";
                break;
            case 1:
                toolName = "���e";
                break;
            default:
                break;
        }


        text.text = "�I���c�[��:" + toolName;

    }
}
