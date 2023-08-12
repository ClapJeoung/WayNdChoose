using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettleButton : ReturnButton
{
  public Vector2 TopPos = Vector2.zero;
  public UI_Settlement SettlementUI = null;
  public override void Clicked()
  {
    base.Clicked();
    UIManager.Instance.AddUIQueue(UIManager.Instance.moverect(MyRect,MyRect.anchoredPosition, CenterPos, 0.25f, UIManager.Instance.UIPanelCLoseCurve));
    UIManager.Instance.AddUIQueue(UIManager.Instance.moverect(MyRect, MyRect.anchoredPosition, TopPos, 0.25f, UIManager.Instance.UIPanelCLoseCurve));
    SettlementUI.OpenUI();
  }
}
