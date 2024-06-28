using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    [Header("UI情報")]
    //  リザルトキャンバス
    [SerializeField] private CanvasGroup m_resultGroop;
    //  リザルトテキスト
    [SerializeField] private Text m_resultText;
    //  リザルトボタン
    [SerializeField] private Button m_resultButton1;
    [SerializeField] private Button m_resultButton2;

    [Header("定数")]
    //  クリア時テキスト
    [SerializeField] private string m_clearText;
    //  失敗時テキスト
    [SerializeField] private string m_failedText;

    //  プレイシーンマネージャー
    private PlaySceneManager m_manager;

    //  プレイシーンマネージャー設定
    public void SetPlayScenemManager(PlaySceneManager manager) { m_manager = manager; }
    //  プレイシーンマネージャー取得
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
        //  状態によって更新
        switch(m_manager.GetGameState())
        {
            case PlaySceneManager.GameState.Clear:
                ////  透明度変化
                //m_resultGroop.alpha = Mathf.Min(m_resultGroop.alpha + Time.deltaTime, 1.0f);
                ////  テキスト変化
                //m_resultText.text = m_clearText;
                ////  表示
                //m_resultButton1.gameObject.SetActive(true);
                //m_resultButton2.gameObject.SetActive(true);

                break;
            case PlaySceneManager.GameState.Failed:
				////  透明度変化
				//m_resultGroop.alpha = Mathf.Min(m_resultGroop.alpha + Time.deltaTime, 1.0f);
				////  テキスト変化
				//m_resultText.text = m_failedText;
				////  表示
				//m_resultButton1.gameObject.SetActive(true);
				//m_resultButton2.gameObject.SetActive(true);

				// シーン切り替え
				SceneManager.LoadScene("StageSelectScene");

				break;
            default:
                break;
        }
    }

    /// <summary>
    /// もう一度
    /// </summary>
    public void RetryStage()
    {
        SceneManager.LoadScene("PlayScene");
    }

    /// <summary>
    /// ステージセレクトシーンへ戻る
    /// </summary>
    public void BackStageSelectScene()
    {
        SceneManager.LoadScene("StageSelectScene");
    }
}
