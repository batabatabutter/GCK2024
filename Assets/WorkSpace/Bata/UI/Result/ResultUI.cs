using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    [Header("UI���")]
    //  ���U���g�L�����o�X
    [SerializeField] private CanvasGroup m_resultGroop;
    //  ���U���g�e�L�X�g
    [SerializeField] private Text m_resultText;
    //  ���U���g�{�^��
    [SerializeField] private Button m_resultButton1;
    [SerializeField] private Button m_resultButton2;

    [Header("�萔")]
    //  �N���A���e�L�X�g
    [SerializeField] private string m_clearText;
    //  ���s���e�L�X�g
    [SerializeField] private string m_failedText;

    //  �v���C�V�[���}�l�[�W���[
    private PlaySceneManager m_manager;

    //  �v���C�V�[���}�l�[�W���[�ݒ�
    public void SetPlayScenemManager(PlaySceneManager manager) { m_manager = manager; }
    //  �v���C�V�[���}�l�[�W���[�擾
    public PlaySceneManager GetPlaySceneManager() { return m_manager; }


    // Start is called before the first frame update
    void Start()
    {
        m_resultButton1.onClick.AddListener(RetryStage);
        m_resultButton2.onClick.AddListener(BackStageSelectScene);
    }

    // Update is called once per frame
    void Update()
    {
        //  ��Ԃɂ���čX�V
        switch(m_manager.GetGameState())
        {
            case PlaySceneManager.GameState.Clear:
                ////  �����x�ω�
                //m_resultGroop.alpha = Mathf.Min(m_resultGroop.alpha + Time.deltaTime, 1.0f);
                ////  �e�L�X�g�ω�
                //m_resultText.text = m_clearText;
                ////  �\��
                //m_resultButton1.gameObject.SetActive(true);
                //m_resultButton2.gameObject.SetActive(true);

                break;
            case PlaySceneManager.GameState.Failed:
				////  �����x�ω�
				//m_resultGroop.alpha = Mathf.Min(m_resultGroop.alpha + Time.deltaTime, 1.0f);
				////  �e�L�X�g�ω�
				//m_resultText.text = m_failedText;
				////  �\��
				//m_resultButton1.gameObject.SetActive(true);
				//m_resultButton2.gameObject.SetActive(true);

				// �V�[���؂�ւ�
				SceneManager.LoadScene("StageSelectScene");

				break;
            default:
                break;
        }
    }

    /// <summary>
    /// ������x
    /// </summary>
    public void RetryStage()
    {
        SceneManager.LoadScene("PlayScene");
    }

    /// <summary>
    /// �X�e�[�W�Z���N�g�V�[���֖߂�
    /// </summary>
    public void BackStageSelectScene()
    {
        SceneManager.LoadScene("StageSelectScene");
    }
}
