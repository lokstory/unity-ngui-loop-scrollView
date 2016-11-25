using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 循環Grid控制
/// </summary>
public abstract class LoopGridControl : MonoBehaviour
{
    /// <summary>
    /// 預製物路徑
    /// </summary>
    public string PrefabPath = null;
    /// <summary>
    /// Loop Scroll View
    /// </summary>
    public LoopScrollView ScrollView = null;
    /// <summary>
    /// Panel
    /// </summary>
    public UIPanel ScrollViewPanel = null;
    /// <summary>
    /// UI Grid
    /// </summary>
    public UIGrid Grid = null;
    /// <summary>
    /// Panel初始位置
    /// </summary>
    protected Vector3 PanelOriginalPos;
    /// <summary>
    /// 循環Items
    /// </summary>
    protected List<LoopItem> Items = new List<LoopItem>();
    /// <summary>
    /// 目前顯示的第一個資料編號
    /// </summary>
    protected int CurrentFirstIndex = 0;
    /// <summary>
    /// Item的寬或高
    /// </summary>
    protected int ItemLength;
    /// <summary>
    /// 在畫面中的Item數量
    /// </summary>
    protected int VisibleCount = 0;
    /// <summary>
    /// 資料列表
    /// </summary>
    protected List<object> DataList = new List<object>();
    /// <summary>
    /// Grid最小第一個顯示的編號
    /// </summary>
    private int MinFirstIndex = 0;
    /// <summary>
    /// Grid最大第一個顯示的編號
    /// </summary>
    private int MaxFirstIndex = 0;

    /// <summary>
    /// 設定ScrollView位置
    /// </summary>
    protected abstract void SetScrollViewPosition(int index);

    /// <summary>
    /// 根據目前Panel位置 取得當前第一個Item ID
    /// </summary>
    protected abstract int GetCurrentIndex();

    /// <summary>
    /// 更新Item位置
    /// </summary>
    protected abstract void UpdateItemPosition(LoopItem item, int index);

    /// <summary>
    /// 初始化
    /// </summary>
    protected virtual void Awake()
    {
        PanelOriginalPos = ScrollView.transform.localPosition;
        ScrollView.Init(CheckUpdateGrid);

        bool isVertical = ScrollView.movement == UIScrollView.Movement.Vertical;
        ItemLength = isVertical ? (int)Grid.cellHeight : (int)Grid.cellWidth;

        VisibleCount = isVertical ? Mathf.CeilToInt(ScrollViewPanel.height / ItemLength) : Mathf.CeilToInt(ScrollViewPanel.width / ItemLength);
        // 頭尾各多一個
        InstantiateItems(VisibleCount + 2, Grid.transform, isVertical);
    }

    /// <summary>
    /// 生成Items
    /// </summary>
    protected void InstantiateItems(int count, Transform parent, bool isVertical)
    {
        if (string.IsNullOrEmpty(PrefabPath)) return;

        GameObject prefab = Resources.Load(PrefabPath) as GameObject;

        if (prefab == null) return;

        Vector3 hidePos = ScrollView.movement == UIScrollView.Movement.Vertical ? new Vector3(0, 5000, 0) : new Vector3(5000, 0, 0);

        for (int x = 0, length = count; x < length; x++)
        {
            GameObject go = Instantiate(prefab);
            go.transform.parent = parent;
            go.transform.localPosition = hidePos;
            go.transform.localScale = Vector3.one;

            LoopItem item = go.GetComponent<LoopItem>();

            go.SetActive(false);

            if (item == null) continue;

            Items.Add(item);
        }
    }

    /// <summary>
    /// 設定資料
    /// </summary>
    public void SetData<T>(List<T> list, bool resetPosition)
    {
        DataList.Clear();

        if (!list.isNullOrEmpty())
        {
            for (int x = 0, count = list.Count; x < count; x++)
            {
                DataList.Add(list[x]);
            }
        }

        if (resetPosition)
        {
            ResetGrid();
            return;
        }

        RefreshGrid();
    }

    /// <summary>
    /// 重置位置與Item資料
    /// </summary>
    protected void ResetGrid()
    {
        CurrentFirstIndex = 0;
        UpdateMaxFirstIndex();

        ScrollView.DisableSpring();
        ScrollViewPanel.clipOffset = Vector2.zero;
        ScrollViewPanel.transform.localPosition = PanelOriginalPos;

        ResetItems();
    }

    /// <summary>
    /// 更新Grid
    /// </summary>
    public void RefreshGrid()
    {
        int count = DataList == null ? MinFirstIndex : DataList.Count;

        if (CurrentFirstIndex > 0 && CurrentFirstIndex >= count)
        { 
            CurrentFirstIndex = count - 1;
            SetScrollViewPosition(CurrentFirstIndex);
        }

        UpdateMaxFirstIndex();
        ResetItems();
    }

    /// <summary>
    /// 更新Index最大值
    /// </summary>
    private void UpdateMaxFirstIndex()
    {
        MaxFirstIndex = DataList == null ? MinFirstIndex : DataList.Count - 1;
        MaxFirstIndex = MaxFirstIndex > VisibleCount ? MaxFirstIndex - VisibleCount : MinFirstIndex;
    }

    /// <summary>
    /// 根據目前ID 重設Item
    /// </summary>
    protected void ResetItems()
    {
        int id = CurrentFirstIndex - 1;

        for (int x = 0, length = Items.Count; x < length; x++)
        {
            UpdateItem(Items[x], id + x);
        }
    }

    /// <summary>
    /// 檢查是否需要更新Grid
    /// </summary>
    public void CheckUpdateGrid()
    {
        // 目前第一個Item ID
        int index = GetCurrentIndex();

        // 限制在最大最小範圍
        index = index < MinFirstIndex ? MinFirstIndex : index > MaxFirstIndex ? MaxFirstIndex : index;
        // 拖曳限制
        ScrollView.restrictWithinPanel = index <= MinFirstIndex || index >= MaxFirstIndex;

        // 不需更新
        if (index == CurrentFirstIndex) return;

        // ID差異
        int diff = index - CurrentFirstIndex;
        // 位移量
        int movement = Mathf.CeilToInt((float)Mathf.Abs(diff) / Items.Count) * Items.Count;

        if (diff > 0)
        {
            MoveItemToEnd(CurrentFirstIndex + diff - 2, movement);
        }
        else
        {
            MoveItemToStart(CurrentFirstIndex + VisibleCount + diff + 1, movement);
        }

        CurrentFirstIndex = index;
    }

    /// <summary>
    /// 重置ScrollView位置回起點
    /// </summary>
    protected void ResetScrollViewPosition()
    {
        ScrollView.DisableSpring();
        ScrollViewPanel.clipOffset = Vector2.zero;
        ScrollViewPanel.transform.localPosition = PanelOriginalPos;
    }

    /// <summary>
    /// 將Item移至頂端
    /// </summary>
    protected virtual void MoveItemToStart(int needChangeID, int movement)
    {
        for (int x = 0, count = Items.Count; x < count; x++)
        {
            LoopItem item = Items[x];

            if (item.IndexProperty < needChangeID) continue;

            UpdateItem(item, item.IndexProperty - movement);
        }
    }

    /// <summary>
    /// 將Item移至尾端
    /// </summary>
    protected virtual void MoveItemToEnd(int needChangeID, int movement)
    {
        for (int x = 0, count = Items.Count; x < count; x++)
        {
            LoopItem item = Items[x];

            if (item.IndexProperty > needChangeID) continue;

            UpdateItem(item, item.IndexProperty + movement);
        }
    }

    /// <summary>
    /// 更新Item資料與位置
    /// </summary>
    protected virtual void UpdateItem(LoopItem item, int index)
    {
        item.IndexProperty = index;

        bool isNull = DataList == null || index < 0 || index >= DataList.Count;
        item.SetData(isNull ? null : DataList[index]);
        item.SetActive(!isNull);

        UpdateItemPosition(item, index);
    }

    /// <summary>
    /// 清除Item資料 關閉介面時用
    /// </summary>
    protected virtual void ClearItemData()
    {
        if (Items.isNullOrEmpty()) return;

        for (int x = 0, count = Items.Count; x < count; x++)
        {
            Items[x].ClearData();
        }
    }
}
