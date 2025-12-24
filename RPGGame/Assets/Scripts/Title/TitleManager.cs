using UnityEngine;


/// <summary>
/// 
/// </summary>
public class TitleManager : MonoBehaviour
{
    [Header("タイトルの選択対象")]
    [SerializeField] private GameObject[] titleMenuObjects = null;

    //
    public int CurrentIndex { get; private set; } = 0;

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        UpdateSelect();
    }

    /// <summary>
    /// 
    /// </summary>
    public void SelectUp()
    {
        //
        CurrentIndex--;

        //
        if (CurrentIndex < 0)
        {
            CurrentIndex = titleMenuObjects.Length - 1;
        }

        UpdateSelect();
    }

    /// <summary>
    /// 
    /// </summary>
    public void SelectDown()
    {
        //
        CurrentIndex++;

        //
        if (CurrentIndex >= titleMenuObjects.Length)
        {
            CurrentIndex = 0;
        }

        UpdateSelect();
    }

    /// <summary>
    /// 選択変更時処理
    /// </summary>
    private void UpdateSelect()
    {
        Debug.Log("選択中: " + titleMenuObjects[CurrentIndex].name);
    }
}