using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// </summary>
public class PlayerManager : MonoBehaviour
{
    [Header("")]
    public float moveDuration = 0.25f;

    //
    private Vector3 targetPos;

    //
    private bool isMoving = false;

    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        targetPos = transform.position;
    }

    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        if (!isMoving)
        {
            Vector2 input = InputManager.Instance.GetMoveVector();

            Vector3 direction = Vector3.zero;
            if (Mathf.Abs(input.x) > 0.1f) direction.x = Mathf.Sign(input.x);
            else if (Mathf.Abs(input.y) > 0.1f) direction.z = Mathf.Sign(input.y);
            if (direction != Vector3.zero)
            {
                Vector3 nextPos = targetPos + direction;

                if (CanMoveTo(nextPos))
                {
                    StartCoroutine(MoveCoroutine(nextPos));
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private IEnumerator MoveCoroutine(Vector3 destination)
    {
        isMoving = true;

        Vector3 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            transform.position = Vector3.Lerp(startPos, destination, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = destination;
        targetPos = destination;
        isMoving = false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private bool CanMoveTo(Vector3 pos)
    {
        DungeonManager dungeon = FindObjectOfType<DungeonManager>();
        if (dungeon == null) return false;

        int x = Mathf.FloorToInt(pos.x + dungeon.tiles.GetLength(0) * 0.5f);
        int y = Mathf.FloorToInt(pos.z + dungeon.tiles.GetLength(1) * 0.5f);

        if (x < 0 || x >= dungeon.tiles.GetLength(0) || y < 0 || y >= dungeon.tiles.GetLength(1))
            return false;

        DungeonTileType tile = dungeon.tiles[x, y];
        return tile == DungeonTileType.Floor;
    }
}