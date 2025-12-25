using UnityEngine;

/// <summary>
/// ダンジョンのマップデータと基本設定を持つScriptableObject
/// </summary>
[CreateAssetMenu(menuName = "Dungeon/DungeonData")]
public class DungeonData : ScriptableObject
{
    [Header("床として配置するプレハブ")]
    public GameObject floorPrefab = null;

    [Header("マップサイズ")]
    public int mapSize = 120;

    [Header("マップデータ")]
    public string[] rows;
}