using UnityEngine;

/// <summary>
/// 
/// </summary>
public class PlayerCameraController : MonoBehaviour
{
    [Header("追従するターゲット")]
    public Transform target;

    [Header("カメラのオフセット")]
    public Vector3 offset = new Vector3(5f, 7f, -5f);

    [Header("カメラ角度")]
    public Vector3 rotationEuler = new Vector3(45f, -40f, 0f);

    [Header("追従のスムーズ")]
    public float smoothSpeed = 100f;

    /// <summary>
    /// 
    /// </summary>
    void LateUpdate()
    {
        if (target == null) return;

        //追従位置
        Vector3 desiredPosition = target.position + offset;

        //スムーズ移動
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        //回転をEulerで指定
        transform.rotation = Quaternion.Euler(rotationEuler);
    }
}