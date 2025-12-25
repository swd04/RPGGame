using UnityEngine;

/// <summary>
/// ダンジョンの管理クラス
/// </summary>
public class DungeonManager : MonoBehaviour
{
    [Header("各階層のダンジョンデータ")]
    public DungeonData dungeonData = null;

    //タイル情報を保持
    public DungeonTileType[,] tiles;
}