using UnityEngine;

/// <summary>
/// プレイヤーをダンジョンに生成クラス
/// </summary>
public class DungeonPlayerSpawner : MonoBehaviour
{
    [Header("ダンジョン管理参照")]
    public DungeonManager dungeonManager = null;

    [Header("プレイヤープレハブ")]
    public GameObject playerPrefab = null;

    /// <summary>
    /// 
    /// </summary>
    public void SpawnPlayer()
    {
        //
        if (!playerPrefab || dungeonManager?.tiles == null) return;

        //
        int size = dungeonManager.dungeonData.mapSize;

        //
        float offsetX = (size) / 2f;
        float offsetZ = (size) / 2f;

        //
        int centerX = size / 2;
        int centerY = size / 2;

        //
        Vector3 spawnPos = Vector3.zero;
        bool found = false;

        //
        for (int r = 0; r < size && !found; r++)
        {
            //
            for (int dx = -r; dx <= r && !found; dx++)
            {
                //
                for (int dy = -r; dy <= r && !found; dy++)
                {
                    //
                    int x = centerX + dx;
                    int y = centerY + dy;

                    //
                    if (x < 0 || x >= size || y < 0 || y >= size) continue;

                    //
                    if (dungeonManager.tiles[x, y] == DungeonTileType.Floor)
                    {
                        spawnPos = new Vector3(x - offsetX + 0.5f, 0f, y - offsetZ + 0.5f);
                        found = true;
                    }
                }
            }
        }

        //
        Instantiate(playerPrefab, spawnPos, Quaternion.identity);
    }
}