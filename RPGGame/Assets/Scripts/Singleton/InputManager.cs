using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ゲーム内の入力を一元管理するシングルトン
/// </summary>
public class InputManager : MonoBehaviour
{
    //InputManager唯一のインスタンス
    public static InputManager Instance { get; private set; }

    //キーとアクション名の対応を定義するクラス
    [System.Serializable]
    public class KeyMapping
    {
        [Header("アクション名")]
        public GameActionType action;

        [Header("対応するキー")]
        public KeyCode key;
    }

    //インスペクターで設定するキーとアクションの配列
    public KeyMapping[] keyMappings = null;

    //アクション名をキー、KeyCodeを値とする辞書
    private Dictionary<GameActionType, KeyCode> keyMap;

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

        //辞書を新規作成
        keyMap = new Dictionary<GameActionType, KeyCode>();

        //配列のキーとアクション名を辞書に登録
        foreach (var km in keyMappings)
        {
            keyMap[km.action] = km.key;
        }
    }

    /// <summary>
    /// 水平・垂直方向の移動ベクトルを取得する処理
    /// </summary>
    public Vector2 GetMoveVector()
    {
        float x = 0f;
        float y = 0f;

        ///////////////
        ///キーボード入力
        ///////////////

        //左方向の入力をチェック
        //keyMapにMoveLeftが登録されているか確認
        //登録されていれば、そのキーが押されているかを確認
        //押されていればにして左方向へ移動させる
        if (GetKey(GameActionType.MoveLeft))
        {
            x -= 1f;
        }

        //右方向の入力をチェック
        //同様に押されていれば右方向へ移動
        if (GetKey(GameActionType.MoveRight))
        {
            x += 1f;
        }

        //上方向の入力をチェック
        ///同様に押されていれば上方向へ移動
        if (GetKey(GameActionType.MoveUp))
        {
            y += 1f;
        }

        //下方向の入力をチェック
        //同様に押されていれば下方向へ移動
        if (GetKey(GameActionType.MoveDown))
        {
            y -= 1f;
        }

        ///////////////
        ///ゲームパッド入力
        ///////////////

        x += Input.GetAxis("Horizontal");
        y += Input.GetAxis("Vertical");

        //入力結果をベクトルに変換
        Vector2 move = new Vector2(x, y);

        //斜め移動でも速度一定に
        if (move.magnitude > 1f)
        {
            move.Normalize();
        }

        return move.normalized;
    }

    /// <summary>
    /// 
    /// </summary>
    public bool GetKey(GameActionType action)
    {
        //
        if (keyMap.ContainsKey(action))
        {
            return Input.GetKey(keyMap[action]);
        }

        return false;
    }

    /// <summary>
    /// ボタンが押された瞬間の処理
    /// </summary>
    public bool GetButtonDown(GameActionType action)
    {
        //指定したアクション名がkeyMapに登録されているか確認
        //登録されている場合のみ入力をチェック
        if (keyMap.ContainsKey(action)) return Input.GetKeyDown(keyMap[action]);

        //アクション名が存在しなければfalseを返す
        return false;
    }

    /// <summary>
    /// ボタンが押されている間の処理
    /// </summary>
    public bool GetButton(GameActionType action)
    {
        //指定したアクション名がkeyMapに登録されているか確認
        //登録されている場合のみ入力をチェック
        if (keyMap.ContainsKey(action)) return Input.GetKey(keyMap[action]);

        //アクション名が存在しなければfalseを返す
        return false;
    }

    /// <summary>
    /// ボタンが離された瞬間の処理
    /// </summary>
    public bool GetButtonUp(GameActionType action)
    {
        //指定したアクション名がkeyMapに登録されているか確認
        //登録されている場合のみ入力をチェック
        if (keyMap.ContainsKey(action)) return Input.GetKeyUp(keyMap[action]);

        //アクション名が存在しなければfalseを返す
        return false;
    }
}