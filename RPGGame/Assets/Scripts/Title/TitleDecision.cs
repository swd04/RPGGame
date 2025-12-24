using UnityEngine;

/// <summary>
/// 
/// </summary>
public class TitleDecision : MonoBehaviour
{
    [Header("")]
    [SerializeField]private TitleManager titleManager;

    /// <summary>
    /// 決定処理
    /// </summary>
    public void Decide()
    {
        //
        int index = titleManager.CurrentIndex;

        //
        switch (index)
        {
            case 0:
                Debug.Log("初めからスタート");

                SceneLoadManager.Instance.LoadScene(SceneType.DungeonScene);

                break;
            case 1:
                Debug.Log("続きからスタート");

                break;
            case 2:
                Debug.Log("設定");

                break;
            case 3:
                Debug.Log("ゲーム終了");

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
                break;
        }
    }
}