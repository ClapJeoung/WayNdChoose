using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapButton : ReturnButton
{
  public override void Clicked()
  {
    base.Clicked();
    UIManager.Instance.AddUIQueue(UIManager.Instance.moverect(MyRect,MyRect.anchoredPosition, CenterPos, 0.4f,UIManager.Instance.UIPanelCLoseCurve));

    switch (GameManager.Instance.MyGameData.QuestType)
    {
      case QuestType.Cult:
        switch (GameManager.Instance.MyGameData.Quest_Cult_Phase)
        {
          case 0:
            if (UIManager.Instance.QuestUI_Cult.IsOpen) UIManager.Instance.QuestUI_Cult.CloseUI_Prologue();
            break;
          case 1:
            break;
          case 2:
            break;
        }
        break;
    }
    UIManager.Instance.MyMap.OpenUI();

    StartCoroutine(UIManager.Instance.ChangeAlpha(MyGroup, 0.0f, UIManager.Instance.MyMap.AppearTime, false));
  }
}
