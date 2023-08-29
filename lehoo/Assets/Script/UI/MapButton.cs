using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapButton : ReturnButton
{
  public override void Clicked()
  {
    base.Clicked();
    UIManager.Instance.AddUIQueue(UIManager.Instance.moverect(MyRect,MyRect.anchoredPosition, CenterPos, 0.4f,UIManager.Instance.UIPanelCLoseCurve));

    if (GameManager.Instance.MyGameData.CurrentQuest == QuestType.Wolf)
    {
      switch (GameManager.Instance.MyGameData.Quest_Wolf_Phase)
      {
        case 0:
          if (UIManager.Instance.QuestUI_Wolf.IsOpen) UIManager.Instance.QuestUI_Wolf.CloseUI_Prologue();
          break;
        case 1:
          break;
        case 2:
          break;
      }
    }
    UIManager.Instance.MyMap.OpenUI();

    StartCoroutine(UIManager.Instance.ChangeAlpha(MyGroup, 0.0f, UIManager.Instance.MyMap.AppearTime, false));
  }
}
