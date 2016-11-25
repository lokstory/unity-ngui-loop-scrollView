
## Make a good game :)


![](/Screen.png) ![](/Hierarchy.png)




## Quick Start

1. Import [**NGUI**](https://www.assetstore.unity3d.com/en/#!/content/2413) before use.

2. Download, unzip, then copy files to Unity **Assets** folder.

3. Modify **UIScrollView.cs** , add **virtual** keyword to **Press** and **RestrictWithinBounds** void like below.

	> public virtual void Press (bool pressed)

	> public virtual bool RestrictWithinBounds (bool instant, bool horizontal, bool vertical)
    
4. Open **TestScene** and play it!




## Summary

1. Use **LoopScrollView.cs** instead of **UIScrollView.cs**.

2. Set **UIGrid** position topmost and center horizontal(**↑**)  for **vertical** scroll view, 
	leftmost and center vertical(**←**)  for **horizontal** scroll view.
    
3. Your custom **controller** must **inherit** VerticalLoopGridControl or HorizontalLoopGridControl.
	Don't forget **drag** components to **inspector** and set prefab **path**.
