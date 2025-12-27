using UnityEngine;

/// <summary>
/// ダンジョンの基本設定を持つScriptableObject
/// </summary>
[CreateAssetMenu(menuName = "Dungeon/DungeonData")]
public class DungeonData : ScriptableObject
{
    //========================
    // 基本情報s
    //========================

    [Header("階層の名前")]
    public string floorName = "";

    [Header("階層番号")]
    public int floorNumber = 0;

    [Header("マップサイズ")]
    public int mapSize = 120;

    [Header("マップデータ")]
    public string[] rows = null;

    //========================
    // プレイヤー関連
    //========================

    [Header("プレイヤーの初期位置")]
    public Vector3 playerStartPosition = Vector3.zero;

    //========================
    // 敵・ボス・FOE関連
    //========================

    [Header("出現する敵リスト")]
    public EnemyData[] enemyData = null;

    [Header("出現するボスリスト")]
    public BossEnemyData[] bossEnemyData = null;

    [Header("ダンジョン内を徘徊する強敵リスト")]
    public FOEData[] foeData = null;

    [Header("ランダムエンカウント率設定")]
    [Range(0f, 200f)]
    public float encounterRate = 0.0f;

    //========================
    // 宝箱・イベント
    //========================

    [Header("ダンジョン内の宝箱")]
    public TreasurePlacement[] treasures = null;

    [Header("ダンジョンのイベントリスト")]
    public EventData eventData = null;

    //========================
    // 階層のつながり
    //========================

    [Header("前の階層")]
    public DungeonData previousFloor = null;

    [Header("次の階層")]
    public DungeonData nextFloor = null;

    //========================
    // 演出関連
    //========================

    [Header("背景スカイボックス")]
    public Material backgroundSkyBox = null;

    [Header("Fogを使うかどうか")]
    public bool useFog = false;

    [Header("ダンジョンBGM")]
    public AudioClip dungeonBGM = null;
}