using UnityEngine;

/// <summary>
/// 
/// </summary>
public class DungeonGenerator : MonoBehaviour
{
    [Header("")]
    public GameObject floorPrefab = null;

    [Header("")]
    public int width = 100;
    public int height = 100;

    //
    private float tileSize = 1f;

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        Generate();
    }

    /// <summary>
    /// 
    /// </summary>
    void Generate()
    {
        //
        for (int y = 0; y < height; y++)
        {
            //
            for (int x = 0; x < width; x++)
            {
                Vector3 pos = new Vector3(x * tileSize, 0, y * tileSize);
                Quaternion rot = Quaternion.Euler(90f, 0f, 0f);
                Instantiate(floorPrefab, pos, rot, transform);
            }
        }
    }
}