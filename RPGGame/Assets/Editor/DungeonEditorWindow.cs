#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/// <summary>
/// ダンジョン編集用のエディタウィンドウ
/// </summary>
public class DungeonEditorWindow : EditorWindow
{
    DungeonData dungeonData;
    private Vector2 scrollPos;
    private int cellSize = 20;
    private int mapSize = 120;
    char currentTile = '.';

    [MenuItem("Tools/Dungeon Editor")]
    public static void Open()
    {
        GetWindow<DungeonEditorWindow>("Dungeon Editor").minSize = new Vector2(400, 300);
    }

    private void OnGUI()
    {
        dungeonData = (DungeonData)EditorGUILayout.ObjectField(
            "Dungeon Data",
            dungeonData,
            typeof(DungeonData),
            false
        );

        cellSize = EditorGUILayout.IntSlider("Cell Size (Zoom)", cellSize, 5, 60);

        if (dungeonData == null)
        {
            EditorGUILayout.HelpBox("DungeonData をセットしてください", MessageType.Info);
            return;
        }

        if (mapSize != dungeonData.mapSize)
        {
            mapSize = dungeonData.mapSize;
            if (dungeonData.rows == null || dungeonData.rows.Length == 0)
                InitMap();
        }

        if (dungeonData.rows == null || dungeonData.rows.Length == 0)
        {
            if (GUILayout.Button("Initialize Map")) InitMap();
            return;
        }

        // タイル選択ボタン
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("タイル選択", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        string[] tileNames = { "床", "壁", "次の階層", "中継地点", "イベント", "宝箱", "鍵付き宝箱", "ショートカット", "一方通行", "ボス", "強敵" };
        char[] tileChars = { '.', '#', '>', 'R', 'E', 'T', 'K', 'S', 'O', 'B', 'F' };
        for (int i = 0; i < tileNames.Length; i++)
        {
            if (GUILayout.Toggle(currentTile == tileChars[i], tileNames[i], "Button"))
                currentTile = tileChars[i];
        }
        GUILayout.EndHorizontal();

        // マップ描画
        EditorGUILayout.Space();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        for (int y = 0; y < mapSize; y++)
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < mapSize; x++)
            {
                char tile = GetTile(x, y);
                GUI.backgroundColor = GetTileColor(tile);

                if (GUILayout.Button("", GUILayout.Width(cellSize), GUILayout.Height(cellSize)))
                    SetTile(x, y); // ← currentTile で置く
            }
            GUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();

        GUI.backgroundColor = Color.white;

        if (GUILayout.Button("保存"))
        {
            EditorUtility.SetDirty(dungeonData);
            AssetDatabase.SaveAssets();
        }
    }

    void InitMap()
    {
        mapSize = dungeonData.mapSize;
        dungeonData.rows = new string[mapSize];
        string emptyRow = new string('#', mapSize);
        for (int y = 0; y < mapSize; y++) dungeonData.rows[y] = emptyRow;
    }

    char GetTile(int x, int y)
    {
        if (dungeonData.rows == null || y >= dungeonData.rows.Length || x >= dungeonData.rows[y].Length) return '#';
        return dungeonData.rows[y][x];
    }

    void SetTile(int x, int y)
    {
        char[] row = dungeonData.rows[y].ToCharArray();
        row[x] = currentTile;
        dungeonData.rows[y] = new string(row);
    }

    Color GetTileColor(char tile)
    {
        return tile switch
        {
            '.' => Color.white,
            '#' => Color.black,
            '>' => new Color(1f, 1f, 0.5f),
            'R' => new Color(0.5f, 1f, 1f),
            'E' => Color.Lerp(new Color(0.5f, 0f, 0.5f), Color.red, 0.5f),
            'T' => new Color(0.5f, 1f, 0.5f),
            'K' => new Color(0f, 0.5f, 0f),
            'S' => Color.blue,
            'O' => Color.Lerp(Color.white, Color.blue, 0.5f),
            'B' => new Color(0.7f, 0.5f, 0.7f),
            'F' => new Color(1f, 0.5f, 0.5f),
            _ => Color.black
        };
    }
}
#endif