using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// プロジェクト内のスクリプト、シーン、プレハブを一覧表示するエディタ拡張ウィンドウ
/// Editorファイルに入れるように
/// </summary>
public class ProjectAssetBrowser : EditorWindow
{
    /// <summary>
    /// タブの種類
    /// </summary>
    private enum TabType
    {
        //スクリプト
        Scripts,

        //シーン
        Scenes,

        //プレハブ
        Prefabs
    }

    //現在選択中のタブ
    private TabType currentTab = TabType.Scripts;

    //スクロール位置
    private Vector2 scrollPos;

    //検索テキスト
    private string searchText = "";

    //スクリプトのGUIDリスト
    private List<string> scriptGuids;

    //シーンのGUIDリスト
    private List<string> sceneGuids;

    //プレハブのGUIDリスト
    private List<string> prefabGuids;

    /// <summary>
    /// メニューからエディタウィンドウを開く処理
    /// </summary>
    [MenuItem("Tools/ProjectAssetBrowser")]
    public static void Open()
    {
        //このスクリプトを基にエディタウィンドウを開く
        GetWindow<ProjectAssetBrowser>("ProjectBrowser");
    }

    /// <summary>
    /// ウィンドウがフォーカスされた時に呼ばれる処理
    /// </summary>
    private void OnFocus()
    {
        LoadAssets();
    }

    /// <summary>
    /// ウィンドウが有効になった時に呼ばれる処理
    /// </summary>
    private void OnEnable()
    {
        LoadAssets();
    }

    /// <summary>
    /// プロジェクト内のアセットを読み込む処理
    /// </summary>
    private void LoadAssets()
    {
        //"Assets/Scripts"フォルダ内のスクリプトを検索してGUIDリスト化
        scriptGuids = AssetDatabase.FindAssets("t:Script", new[] { "Assets/Scripts" }).ToList();

        // "Assets/Scenes"フォルダ内のシーンを検索してGUIDリスト化
        sceneGuids = AssetDatabase.FindAssets("t:Scene", new[] { "Assets/Scenes" }).ToList();

        //"Assets/Prefabs"フォルダ内のプレハブを検索してGUIDリスト化
        prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefabs" }).ToList();
    }

    /// <summary>
    /// ウィンドウのGUI描画処理
    /// </summary>
    private void OnGUI()
    {
        //タブ部分を描画
        DrawTabs();

        //検索バーを描画
        DrawSearchBar();

        //アセット一覧を描画
        DrawList();
    }

    /// <summary>
    /// タブを描画する処理
    /// </summary>
    private void DrawTabs()
    {
        //少しスペースを空ける
        GUILayout.Space(5);

        //タブを描画して選択されたタブをcurrentTabに反映
        currentTab = (TabType)GUILayout.Toolbar(
            (int)currentTab, new string[]
            { "Scripts", "Scenes", "Prefabs" }, GUILayout.Height(25));
    }

    /// <summary>
    /// 検索バーを描画する処理
    /// </summary>
    private void DrawSearchBar()
    {
        //少しスペース
        GUILayout.Space(5);

        //水平レイアウト開始
        EditorGUILayout.BeginHorizontal();

        //「Search:」ラベル
        GUILayout.Label("Search:", GUILayout.Width(60));

        //テキストフィールドに入力された値をsearchTextに格納
        searchText = GUILayout.TextField(searchText);

        //水平レイアウト終了
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// アセットリストを描画する処理
    /// </summary>
    private void DrawList()
    {
        //上に少しスペース
        GUILayout.Space(5);

        //現在のタブに対応するGUIDリストを取得
        List<string> targetList = GetTargetList();

        //小文字で検索用に変換
        var lower = string.IsNullOrEmpty(searchText) ? "" : searchText.ToLower();

        //リストがnullの場合は描画しない
        if (targetList == null) return;

        //GUIDからパス・名前を取得して、検索フィルターで絞り込み、名前順にソート
        var sorted = targetList
            .Select(guid => new
            {
                guid,

                //GUIDからパスを取得
                path = AssetDatabase.GUIDToAssetPath(guid),

                //拡張子を除いた名前
                name = System.IO.Path.GetFileNameWithoutExtension(
                    AssetDatabase.GUIDToAssetPath(guid))
            })
            .Where(a => string.IsNullOrEmpty(lower) || a.name.ToLower().Contains(lower))
            .OrderBy(a => a.name)
            .ToList();

        //スクロールビューを開始
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        //各アセットを描画
        foreach (var asset in sorted)
        {
            DrawAssetItem(asset.guid, asset.name);
        }

        //スクロールビュー終了
        EditorGUILayout.EndScrollView();
    }

    /// <summary>
    /// アセット1件分の描画処理
    /// </summary>
    private void DrawAssetItem(string guid, string name)
    {
        //GUIDからパスを取得
        var path = AssetDatabase.GUIDToAssetPath(guid);

        //パスからオブジェクトを取得
        var obj = AssetDatabase.LoadAssetAtPath<Object>(path);

        //アセットを描画するための矩形
        Rect rect = GUILayoutUtility.GetRect(0, 22, GUILayout.ExpandWidth(true));

        //背景描画
        GUI.Box(rect, "", EditorStyles.helpBox);

        //アセットのアイコンを取得
        Texture2D icon = AssetDatabase.GetCachedIcon(path) as Texture2D;

        //アイコンを描画する矩形
        var iconRect = new Rect(rect.x + 4, rect.y + 2, 18, 18);

        //アイコンがあれば描画
        if (icon != null)
        {
            GUI.DrawTexture(iconRect, icon, ScaleMode.ScaleToFit);
        }

        //ラベル描画用の矩形
        var labelRect = new Rect(rect.x + 26, rect.y + 3, rect.width - 30, rect.height);
        EditorGUI.LabelField(labelRect, name);

        //クリック処理
        if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
        {
            //シングルクリックで選択
            if (Event.current.clickCount == 1)
            {
                Selection.activeObject = obj;
                EditorGUIUtility.PingObject(obj);
            }
            //ダブルクリックで開く
            else if (Event.current.clickCount == 2)
            {
                OpenAsset(obj, path);
            }
        }
    }

    /// <summary>
    /// アセットを開く処理
    /// </summary>
    private void OpenAsset(Object obj, string path)
    {
        switch (currentTab)
        {
            case TabType.Scripts:
                //スクリプトは開く
                AssetDatabase.OpenAsset(obj);
                break;
            case TabType.Scenes:
                //シーンは保存確認後に開く
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    EditorSceneManager.OpenScene(path);
                }
                break;
            case TabType.Prefabs:
                //プレハブは選択してプロジェクトウィンドウでハイライト
                Selection.activeObject = obj;
                EditorGUIUtility.PingObject(obj);
                break;
        }
    }

    /// <summary>
    /// 現在のタブに対応するGUIDリストを返す処理
    /// </summary>
    private List<string> GetTargetList()
    {
        //currentTabの値に応じて返すリストを切り替える
        switch (currentTab)
        {
            //スクリプトタブが選択されている場合
            case TabType.Scripts: return scriptGuids;

            //シーンタブが選択されている場合
            case TabType.Scenes: return sceneGuids;

            //プレハブタブが選択されている場合
            case TabType.Prefabs: return prefabGuids;
        }

        //タブに対応するリストがない場合はnullを返す
        return null;
    }
}