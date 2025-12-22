using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン遷移を一元管理するシングルトン
/// </summary>
public class SceneLoadManager : MonoBehaviour
{
    //SceneLoadManager唯一のインスタンス
    public static SceneLoadManager Instance { get; private set; }

    //多重ロード防止フラグ
    private bool isLoading = false;

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Awake()
    {
        //すでにインスタンスが存在していたら破棄
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        //このオブジェクトをシングルトンとして登録
        Instance = this;

        //シーンをまたいでも消えないようにする
        DontDestroyOnLoad(gameObject);

        //シーンロード完了イベントを登録
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// 破棄時処理
    /// </summary>
    private void OnDestroy()
    {
        //自分自身がシングルトンである場合のみイベント登録の解除
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    /// <summary>
    /// シーンロード完了時に呼ばれるコールバック処理
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //ロード完了後に多重ロード防止フラグを解除する
        isLoading = false;
    }

    /// <summary>
    /// シーン読み込み処理
    /// </summary>
    public void LoadScene(SceneType sceneType)
    {
        //すでにロード中の場合は処理しない
        if (isLoading) return;

        //enum名をシーン名として使用
        string sceneName = sceneType.ToString();

        //ビルド設定に存在しないシーンの場合は処理中断
        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError($"{sceneName}はビルド設定で見つかりません");
            return;
        }

        //ロード開始フラグを立てる
        isLoading = true;

        //シーン遷移
        SceneManager.LoadScene(sceneName);
    }
}