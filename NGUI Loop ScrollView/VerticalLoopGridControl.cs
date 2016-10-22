using UnityEngine;
using System.Collections.Generic;
using System;
/// <summary>
/// 循環Grid控制
/// </summary>
public abstract class VerticalLoopGridControl : LoopGridControl
{

    /// <summary>
    /// 初始化
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// 根據Panel位置 取得目前ID
    /// </summary>
    /// <returns></returns>
    protected override int GetCurrentIndex()
    {
        return Mathf.RoundToInt((ScrollViewPanel.transform.localPosition.y - PanelOriginalPos.y) / ItemLength);
    }

    /// <summary>
    /// 更新Item位置
    /// </summary>
    protected override void UpdateItemPosition(LoopItem item, int index)
    {
        Vector3 pos = item.Transform.localPosition;
        pos.y = -index * ItemLength;
        item.Transform.localPosition = pos;
    }

    /// <summary>
    /// 設定ScrollView位置
    /// </summary>
    protected override void SetScrollViewPosition(int index)
    {
        ScrollView.DisableSpring();
        Vector3 pos = PanelOriginalPos;
        pos.y = index * ItemLength;
        ScrollViewPanel.transform.localPosition = pos;

        pos.y = -pos.y;
        ScrollViewPanel.clipOffset = pos;
    }
}
