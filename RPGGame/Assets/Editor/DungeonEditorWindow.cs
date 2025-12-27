#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/// <summary>
/// ダンジョン編集用のエディタウィンドウ
/// </summary>
public class DungeonEditorWindow : EditorWindow
{
    //編集中のダンジョンデータ
    DungeonData dungeonData;

    //スクロール位置
    private Vector2 scrollPos;

    //1セルの表示サイズ
    private int cellSize = 20;

    //マップサイズ
    private int mapSize = 120;

    //現在選択しているタイル
    char currentTile = '.';

    /// <summary>
    /// メニューから開く処理
    /// </summary>
    [MenuItem("Tools/Dungeon Editor")]
    public static void Open()
    {
        //エディタウィンドウを生成し、最小サイズを設定
        GetWindow<DungeonEditorWindow>("Dungeon Editor").minSize = new Vector2(400, 300);
    }

    /// <summary>
    /// UnityのIMGUI描画ループ処理
    /// </summary>
    private void OnGUI()
    {
        //現在のイベント
        Event e = Event.current;

        //ペイントフラグ
        bool shouldPaint = false;
        int paintX = -1;
        int paintY = -1;

        //DungeonDataの参照を受け取る
        dungeonData = (DungeonData)EditorGUILayout.ObjectField(
            "Dungeon Data",
            dungeonData,
            typeof(DungeonData),
            false
        );

        //セルサイズ変更
        cellSize = EditorGUILayout.IntSlider("Cell Size (Zoom)", cellSize, 5, 60);

        //DungeonData未設定なら終了
        if (dungeonData == null)
        {
            EditorGUILayout.HelpBox("DungeonData をセットしてください", MessageType.Info);

            return;
        }

        //UI からマップサイズを入力させる
        int newSize = EditorGUILayout.IntField("Map Size", mapSize);

        //入力値が前と違っていて0より大きいならリサイズ実行
        if (newSize != mapSize && newSize > 0)
        {
            mapSize = newSize;
            dungeonData.mapSize = mapSize;
            ApplyMapResize();
        }

        //DungeonDataのmapSizeが変わったら同期
        if (mapSize != dungeonData.mapSize)
        {
            mapSize = dungeonData.mapSize;

            //rowsがまだ作られていないなら初期化
            if (dungeonData.rows == null || dungeonData.rows.Length == 0)
            {
                InitMap();
            }
        }

        //rowsが無いなら初期化ボタンだけ表示
        if (dungeonData.rows == null || dungeonData.rows.Length == 0)
        {
            //手動初期化ボタン
            if (GUILayout.Button("Initialize Map"))
            {
                InitMap();
            }

            return;
        }

        //ここからタイル選択UI
        EditorGUILayout.Space();

        //見出し
        EditorGUILayout.LabelField("タイル選択", EditorStyles.boldLabel);

        //横並びレイアウト開始
        GUILayout.BeginHorizontal();

        //各タイルの名称
        string[] tileNames =
        {
            "床","壁","次の階層","前の階層","中継地点","イベント",
            "宝箱","鍵付き宝箱","ショートカット","一方通行","ボス","強敵"
        };

        //実際に書き込まれる文字
        char[] tileChars = { '.', '#', 'U', 'D', 'R', 'E', 'T', 'K', 'S', 'O', 'B', 'F' };

        //ボタン形式トグル
        for (int i = 0; i < tileNames.Length; i++)
        {
            //選択されたらcurrentTileを更新
            if (GUILayout.Toggle(currentTile == tileChars[i], tileNames[i], "Button"))
            {
                currentTile = tileChars[i];
            }
        }

        //タイル選択行終了
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        //マップ描画領域をスクロール可能に
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        //縦方向ループ
        for (int y = 0; y < mapSize; y++)
        {
            //横一列開始
            GUILayout.BeginHorizontal();

            //横方向ループ
            for (int x = 0; x < mapSize; x++)
            {
                //現在タイル取得
                char tile = GetTile(x, y);

                //色取得
                Color c = GetTileColor(tile);

                //セル用Box
                GUILayout.Box("", GUILayout.Width(cellSize), GUILayout.Height(cellSize));

                //直前に描いたBoxのRectを取得
                Rect r = GUILayoutUtility.GetLastRect();

                //Boxの上に色塗り
                EditorGUI.DrawRect(r, c);

                //左クリック押下orドラッグ中&セルの上ならペイント
                if ((e.rawType == EventType.MouseDown || e.rawType == EventType.MouseDrag) &&
                e.button == 0 &&
                r.Contains(e.mousePosition))
                {
                    shouldPaint = true;
                    paintX = x;
                    paintY = y;
                }
            }

            //横一列終了
            GUILayout.EndHorizontal();
        }

        //スクロール終了
        EditorGUILayout.EndScrollView();

        //GUIカラーを念のためリセット
        GUI.backgroundColor = Color.white;

        //ペイント処理
        if (shouldPaint)
        {
            SetTile(paintX, paintY);
            Repaint();
        }

        //保存ボタン
        if (GUILayout.Button("保存"))
        {
            EditorUtility.SetDirty(dungeonData);
            AssetDatabase.SaveAssets();
        }
    }

    /// <summary>
    /// マップ初期化処理
    /// </summary>
    void InitMap()
    {
        //サイズ同期
        mapSize = dungeonData.mapSize;

        //行配列を生成
        dungeonData.rows = new string[mapSize];

        //初期状態：全部壁
        string emptyRow = new string('#', mapSize);

        //全行を同じ文字列で埋める
        for (int y = 0; y < mapSize; y++)
        {
            dungeonData.rows[y] = emptyRow;
        }
    }

    /// <summary>
    /// タイル取得処理
    /// </summary>
    char GetTile(int x, int y)
    {
        //安全チェック
        if (dungeonData.rows == null ||
            y >= dungeonData.rows.Length ||
            x >= dungeonData.rows[y].Length)
        {
            return '#';
        }

        return dungeonData.rows[y][x];
    }

    /// <summary>
    /// タイル設定処理
    /// </summary>
    void SetTile(int x, int y)
    {
        //stringはimmutableのため一度char[]に変換
        char[] row = dungeonData.rows[y].ToCharArray();

        //対象セルを書き換える
        row[x] = currentTile;

        //再度stringに戻す
        dungeonData.rows[y] = new string(row);
    }

    /// <summary>
    /// タイルごとの表示色処理
    /// </summary>
    Color GetTileColor(char tile)
    {
        return tile switch
        {
            '.' => Color.white,
            '#' => Color.black,
            'U' => new Color(1f, 1f, 0.5f),
            'D' => new Color(0.5f, 0.8f, 0.9f),
            'R' => new Color(0.5f, 1f, 1f),
            'E' => Color.Lerp(new Color(0.5f, 0f, 0.5f), Color.red, 0.5f),
            'T' => new Color(0.5f, 1f, 0.5f),
            'K' => new Color(0f, 0.5f, 0f),
            'S' => Color.blue,
            'O' => Color.Lerp(Color.white, Color.blue, 0.5f),
            'B' => new Color(0.6f, 0f, 0.8f),
            'F' => new Color(1f, 0.5f, 0.5f),
            _ => Color.black
        };
    }

    /// <summary>
    /// マップサイズ変更処理
    /// </summary>
    void ApplyMapResize()
    {
        //まだrowsが無いなら初期化
        if (dungeonData.rows == null || dungeonData.rows.Length == 0)
        {
            InitMap();

            return;
        }

        //元のサイズ
        int oldSize = dungeonData.rows.Length;

        //新しい行配列を作る
        string[] newRows = new string[mapSize];

        //1行ずつ処理
        for (int y = 0; y < mapSize; y++)
        {
            //既存行がまだある範囲
            if (y < oldSize)
            {
                //元の行を取得
                string oldRow = dungeonData.rows[y];

                //横が大きすぎる→切る
                if (oldRow.Length > mapSize)
                {
                    newRows[y] = oldRow.Substring(0, mapSize);
                }
                //横が足りない→壁で埋める
                else if (oldRow.Length < mapSize)
                {
                    newRows[y] = oldRow + new string('#', mapSize - oldRow.Length);
                }
                //同じならそのまま
                else
                {
                    newRows[y] = oldRow;
                }
            }
            //新しく増えた行
            else
            {
                newRows[y] = new string('#', mapSize);
            }
        }

        //置き換え
        dungeonData.rows = newRows;
    }
}
#endif