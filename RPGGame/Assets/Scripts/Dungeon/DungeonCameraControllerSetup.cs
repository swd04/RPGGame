using UnityEngine;

/// <summary>
/// ダンジョンでプレイヤー生成後カメラをセットする処理
/// </summary>
public class DungeonCameraControllerSetup : MonoBehaviour
{
    [Header("")]
    public PlayerCameraController playerCameraController;

    [Header("")]
    public Transform player;

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        //
        if (player && playerCameraController)
        {
            playerCameraController.target = player;
        }
    }
}