
/// <summary>
/// ダンジョン内で使用されるタイルの種類タイプ
/// </summary>
public enum DungeonTileType
{
    /// <summary>
    /// プレイヤーが歩ける床タイル
    /// </summary>
    Floor,

    /// <summary>
    /// 壁や障害物となるタイル
    /// </summary>
    Wall,

    /// <summary>
    /// ダンジョンの範囲外、または未使用領域
    /// </summary>
    Outside,

    /// <summary>
    /// 次の階層
    /// </summary>
    Stairs,

    /// <summary>
    /// 中継ポイント
    /// </summary>
    RelayPoint,

    /// <summary>
    /// イベント
    /// </summary>
    Event,

    /// <summary>
    /// 宝箱
    /// </summary>
    Treasure,

    /// <summary>
    /// 鍵付き宝箱
    /// </summary>
    TreasureWithKey,

    /// <summary>
    /// ショートカット
    /// </summary>
    Shortcut,

    /// <summary>
    /// 一方通行
    /// </summary>
    OneWay,

    /// <summary>
    /// ボス
    /// </summary>
    Boss,

    /// <summary>
    /// ダンジョン内に見える強敵
    /// </summary>
    FOE,
}