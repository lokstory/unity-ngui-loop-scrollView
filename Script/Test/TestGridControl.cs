using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 測試控制器
/// </summary>
public class TestGridControl : VerticalLoopGridControl
{
    /// <summary>
    /// 測試資料
    /// </summary>
    List<TestModel> TestDataList = new List<TestModel>()
    {
        new TestModel { id = 6, name = "Lebron" },
        new TestModel { id = 23, name = "Kobe"},
        new TestModel { id = 3, name = "Wade" },
        new TestModel { id = 30, name = "Curry" },
        new TestModel { id = 7, name = "Lin" },
        new TestModel { id = 41, name = "Dirk" },
        new TestModel { id = 21, name = "Duncan" },
        new TestModel { id = 1, name = "McGee" }
    };

    /// <summary>
    /// 設定資料
    /// </summary>
    public void SetGridData()
    {
        SetData(TestDataList, true);
    }

    /// <summary>
    /// 更新資料
    /// </summary>
    public void UpdateData()
    {
        if (DataList.isNullOrEmpty()) return;

        DataList.Reverse();
        ResetGrid();
    }

    /// <summary>
    /// 新增資料
    /// </summary>
    public void AddData()
    {
        DataList.Add(TestDataList[Random.Range(0, TestDataList.Count)]);
        RefreshGrid();
    }

    /// <summary>
    /// 刪除資料
    /// </summary>
    public void DeleteData()
    {
        if (DataList.isNullOrEmpty()) return;

        DataList.RemoveAt(DataList.Count - 1);
        RefreshGrid();
    }
}
