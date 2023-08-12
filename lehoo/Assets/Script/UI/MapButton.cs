using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapButton : ReturnButton
{
  public override void Clicked()
  {
    base.Clicked();
    UIManager.Instance.AddUIQueue(UIManager.Instance.moverect(MyRect,MyRect.anchoredPosition, CenterPos, 0.4f,UIManager.Instance.UIPanelCLoseCurve));
    UIManager.Instance.MyMap.OpenUI();
    StartCoroutine(UIManager.Instance.ChangeAlpha(MyGroup, 0.0f, UIManager.Instance.MyMap.AppearTime, false));
  }
}
