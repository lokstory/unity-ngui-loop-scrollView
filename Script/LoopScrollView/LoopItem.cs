using UnityEngine;
/// <summary>
/// 循環Item控制
/// </summary>
public abstract class LoopItem : MonoBehaviour
{
  
    [HideInInspector]
    public Transform Transform = null;
    /// <summary>
    /// Grid編號
    /// </summary>
    private int Index;
    /// <summary>
    /// Grid編號存取子
    /// </summary>
    public int IndexProperty
    {
        get
        {
            return Index;
        }
        set
        {
            Index = value;
        }
    }

    protected virtual void Awake()
    {
        Transform = GetComponent<Transform>();
    }

    /// <summary>
    /// 開關物件
    /// </summary>
    public virtual void SetActive(bool active)
    {
        if (gameObject.activeSelf == active) return;

        gameObject.SetActive(active);
    }

    /// <summary>
    /// 設定資料
    /// </summary>
    public abstract void SetData<T>(T data);

    /// <summary>
    /// 清除資料
    /// </summary>
    public virtual void ClearData()
    {
    }
}
