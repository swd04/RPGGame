using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// SEを管理するシングルトン
/// </summary>
public class SEManager : MonoBehaviour
{
    //SEManager唯一のインスタンス
    public static SEManager Instance { get; private set; }

    [Header("最大同時再生数")]
    [SerializeField] private int maxSources = 10;

    [Header("SEの音量")]
    [SerializeField, Range(0f, 1f)]
    private float volume = 1f;

    //次に使用するAudioSourceのインデックス
    private int currentIndex = 0;

    //SE再生用AudioSourceを格納するリスト
    private List<AudioSource> sources = new List<AudioSource>();

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Awake()
    {
        //すでにインスタンスが存在していたら自分を破棄
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        //このオブジェクトをシングルトンとして登録
        Instance = this;

        //シーンをまたいでも消えないように
        DontDestroyOnLoad(gameObject);

        //指定数だけAudioSourceコンポーネントを自動生成してリストに登録
        for (int i = 0; i < maxSources; i++)
        {
            AudioSource src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            sources.Add(src);
        }
    }

    /// <summary>
    /// 効果音を再生する処理
    /// </summary>
    public void PlaySE(AudioClip clip)
    {
        //再生するクリップがなければ処理しない
        if (clip == null) return;

        //現在のインデックスのAudioSourceを取得
        AudioSource src = sources[currentIndex];

        //再生音量を設定
        src.volume = volume;

        //効果音を再生
        src.PlayOneShot(clip);

        //次に使用するAudioSourceへインデックスを進める
        currentIndex = (currentIndex + 1) % sources.Count;
    }
}