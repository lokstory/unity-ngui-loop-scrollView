using System;
/// <summary>
/// 測試Grid Item
/// </summary>
public class TestItem : LoopItem
{
    /// <summary>
    /// 編號
    /// </summary>
    public UILabel IDLabel;
    /// <summary>
    /// 模型
    /// </summary>
    public UILabel NameLabel;

    public override void SetData<T>(T data)
    {
        TestModel model = data as TestModel;

        if (model == null) return;

        IDLabel.text = model.id.ToString();

        NameLabel.text = model.name;
    }
}
