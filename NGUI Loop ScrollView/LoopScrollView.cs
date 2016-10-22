using UnityEngine;
/// <summary>
/// 循環ScrollView
/// Make sure add virtual keyword to MoveRelative, Press and RestrictWithinBounds in UIScrollView before use.
/// </summary>
public class LoopScrollView : UIScrollView
{
    /// <summary>
    /// 更新Grid委派動作
    /// </summary>
    protected System.Action UpdateGridAction = null;
    /// <summary>
    /// 原本位置
    /// </summary>
    protected Vector3 OriginalLocalPos;
    /// <summary>
    /// Spring Panel
    /// </summary>
    protected SpringPanel SpringPanel = null;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="checkUpdateAction">移動時檢查更新Grid委派動作</param>
    public void Init(System.Action checkUpdateAction)
    {
        UpdateGridAction = checkUpdateAction;

        if (dragEffect == DragEffect.MomentumAndSpring)
        {
            OriginalLocalPos = transform.localPosition;
            SpringPanel = gameObject.AddComponent<SpringPanel>();
        }
    }

    /// <summary>
    /// Avoid use late update in LoopGridControl
    /// </summary>
    /// <param name="relative"></param>
    public override void MoveRelative(Vector3 relative)
    {
        base.MoveRelative(relative);
        UpdateGridAction();
    }

    /// <summary>
    /// When vertical scroll View items total height less than scroll view height, 
    /// drag down to bottom, item won't back to top. 
    /// So I overrode this function.
    /// If you have another better way, please let me know.
    /// 垂直ScrollView Item總高度少於ScrollView高度時 往上彈會置底 只好改寫
    /// </summary>
    public override void Press(bool pressed)
    {
        if (UICamera.currentScheme == UICamera.ControlScheme.Controller) return;

        if (smoothDragStart && pressed)
        {
            mDragStarted = false;
            mDragStartOffset = Vector2.zero;
        }

        if (enabled && NGUITools.GetActive(gameObject))
        {
            if (!pressed && mDragID == UICamera.currentTouchID) mDragID = -10;

            mCalculatedBounds = false;
            mShouldMove = shouldMove;
            if (!mShouldMove) return;
            mPressed = pressed;

            if (pressed)
            {
                mMomentum = Vector3.zero;
                mScroll = 0f;

                DisableSpring();

                mLastPos = UICamera.lastWorldPosition;

                mPlane = new Plane(mTrans.rotation * Vector3.back, mLastPos);

                Vector2 co = mPanel.clipOffset;
                co.x = Mathf.Round(co.x);
                co.y = Mathf.Round(co.y);
                mPanel.clipOffset = co;

                Vector3 v = mTrans.localPosition;
                v.x = Mathf.Round(v.x);
                v.y = Mathf.Round(v.y);
                mTrans.localPosition = v;

                if (!smoothDragStart)
                {
                    mDragStarted = true;
                    mDragStartOffset = Vector2.zero;
                    if (onDragStarted != null) onDragStarted();
                }
            }
            else if (centerOnChild)
            {
                centerOnChild.Recenter();
            }
            else
            {
                // 垂直彈回頂端
                if (movement == Movement.Vertical && dragEffect == DragEffect.MomentumAndSpring && mTrans.localPosition.y < OriginalLocalPos.y)
                {
                    SpringPanel.target = OriginalLocalPos;
                    SpringPanel.enabled = true;
                }
                else if (mDragStarted && restrictWithinPanel && mPanel.clipping != UIDrawCall.Clipping.None)
                {
                    RestrictWithinBounds(dragEffect == DragEffect.None, canMoveHorizontally, canMoveVertically);
                }

                if (mDragStarted && onDragFinished != null) onDragFinished();
                if (!mShouldMove && onStoppedMoving != null)
                    onStoppedMoving();
            }
        }
    }

    /// <summary>
    /// When Spring Panel enable, don't RestrictWithinBounds
    /// </summary>
    public override bool RestrictWithinBounds(bool instant, bool horizontal, bool vertical)
    {
        if (SpringPanel != null && SpringPanel.enabled) return false;

        return base.RestrictWithinBounds(instant, horizontal, vertical);
    }
}
