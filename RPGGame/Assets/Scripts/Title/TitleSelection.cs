using UnityEngine;

/// <summary>
/// 
/// </summary>
public class TitleSelection : MonoBehaviour
{
    [Header("")]
    [SerializeField] private TitleManager titleManager = null;

    [Header("")]
    [SerializeField] private TitleDecision titleDecision = null;

    /// <summary>
    /// çXêVèàóù
    /// </summary>
    private void Update()
    {
        //
        if (InputManager.Instance.GetButtonDown(GameActionType.MoveUp))
        {
            titleManager.SelectUp();
        }

        //
        if (InputManager.Instance.GetButtonDown(GameActionType.MoveDown))
        {
            titleManager.SelectDown();
        }

        //
        if (InputManager.Instance.GetButtonDown(GameActionType.Confirm))
        {
            titleDecision.Decide();
        }
    }
}