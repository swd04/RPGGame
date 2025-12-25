using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DungeonManagerからマップデータを受け取り
/// タイル配列の初期化、床・壁などのシーンオブジェクトを生成クラス
/// </summary>
public class DungeonBuilder : MonoBehaviour
{
    [Header("ダンジョン管理参照")]
    public DungeonManager dungeonManager = null;

    //1マスのサイズ
    private float tileSize = 1f;

    /// <summary>
    /// 
    /// </summary>
    private static readonly Dictionary<char, DungeonTileType> charToTile = new()
    {
        ////プレイヤーが歩ける床タイル
        ['.'] = DungeonTileType.Floor,
        //ダンジョンの壁、範囲外
        ['#'] = DungeonTileType.Wall,
        //次の階層
        ['>'] = DungeonTileType.Stairs,
        //中継ポイント
        ['R'] = DungeonTileType.RelayPoint,
        //イベント
        ['E'] = DungeonTileType.Event,
        //宝箱
        ['T'] = DungeonTileType.Treasure,
        //鍵付き宝箱
        ['K'] = DungeonTileType.TreasureWithKey,
        //ショートカット
        ['S'] = DungeonTileType.Shortcut,
        //一方通行
        ['O'] = DungeonTileType.OneWay,
        //ボス
        ['B'] = DungeonTileType.Boss,
        //ダンジョン内に見える強敵
        ['F'] = DungeonTileType.FOE,
    };

    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        //
        if (dungeonManager == null || dungeonManager.dungeonData == null) return;

        //
        BuildDungeon();
    }

    /// <summary>
    /// 
    /// </summary>
    void BuildDungeon()
    {
        //
        var data = dungeonManager.dungeonData;
        int size = data.mapSize;
        dungeonManager.tiles = new DungeonTileType[size, size];

        //
        float offsetX = (size * tileSize) / 2f;
        float offsetZ = (size * tileSize) / 2f;

        //
        for (int y = 0; y < size; y++)
        {
            //
            for (int x = 0; x < size; x++)
            {
                //
                char c = (y < data.rows.Length && x < data.rows[y].Length) ? data.rows[y][x] : '#';
                dungeonManager.tiles[x, y] = c == '#' ? DungeonTileType.Wall : DungeonTileType.Floor;

                //
                if (dungeonManager.tiles[x, y] == DungeonTileType.Floor && data.floorPrefab != null)
                {
                    Vector3 pos = new Vector3((x + 0.5f) * tileSize - offsetX, 0f, (y + 0.5f) * tileSize - offsetZ);
                    Instantiate(data.floorPrefab, pos, Quaternion.Euler(90f, 0f, 0f), transform);
                }
            }
        }
    }
}