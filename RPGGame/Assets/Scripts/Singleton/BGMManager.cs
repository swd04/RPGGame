using UnityEngine;

/// <summary>
/// BGMを管理するシングルトン
/// </summary>
public class BGMManager : MonoBehaviour
{
    //BGMManager唯一のインスタンス
    public static BGMManager Instance { get; private set; }

    [Header("BGMの音量")]
    [SerializeField, Range(0f, 1f)]
    private float volume = 1f;

    //BGM再生用のAudioSource
    private AudioSource source = null;

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

        //シーンをまたいでも消えないように
        DontDestroyOnLoad(gameObject);

        //AudioSourceがなければ追加、あれば取得
        source = GetComponent<AudioSource>();
        if (source == null)
        {
            source = gameObject.AddComponent<AudioSource>();
        }

        //初期音量を設定
        source.volume = volume;

        //起動時に自動再生されないようにする
        source.playOnAwake = false;
    }

    /// <summary>
    /// BGMを再生する処理
    /// </summary>
    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        //nullチェック
        if (clip == null) return;

        //すでに同じBGMが再生中なら何もしない
        if (source.isPlaying && source.clip == clip) return;

        //再生するクリップとループ設定をセット
        source.clip = clip;
        source.loop = loop;

        //BGM再生開始
        source.Play();
    }

    /// <summary>
    /// BGMを停止する処理
    /// </summary>
    public void StopBGM()
    {
        //再生中の場合のみ停止
        if (source.isPlaying)
        {
            source.Stop();
        }
    }
}