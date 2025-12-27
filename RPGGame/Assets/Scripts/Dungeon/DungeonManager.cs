using UnityEngine;

/// <summary>
/// ダンジョンの管理クラス
/// </summary>
public class DungeonManager : MonoBehaviour
{
    [Header("各階層のダンジョンデータ")]
    public DungeonData[] dungeonData = null;

    //階層ごとのタイル情報を保持
    public DungeonTileType[][,] tilesPerFloor = null;

    //現在の階層
    public int currentFloor = 0;
}