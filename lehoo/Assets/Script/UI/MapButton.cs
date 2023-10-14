using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapButton : ReturnButton
{
  public override void Clicked()
  {
    base.Clicked();


    if (CurrentUI as UI_dialogue != null)
    {
      UI_dialogue _dialogue = CurrentUI as UI_dialogue;
      if (_dialogue.RemainReward == true && Warned == false)
      {
        WarningDescription.text = GameManager.Instance.GetTextData("NOREWARD");
        SetWarningButton();
        return;
      }
      else
      {
        UIManager.Instance.DialogueUI.CloseUI();
      }
    }
    else if (CurrentUI as UI_Settlement != null)
    {
      if (GameManager.Instance.MyGameData.MovePoint == 0 && Warned == false)
      {
        WarningDescription.text = GameManager.Instance.GetTextData("NOMOVEPOINT");
        SetWarningButton();
        return;
      }
      else
      {
        UIManager.Instance.MySettleUI.CloseUI(false);
        if (GameManager.Instance.MyGameData.Tendency_Head.Level < 0) GameManager.Instance.MyGameData.MovePoint += ConstValues.Tendency_Head_m1;
      }
    }

    switch (GameManager.Instance.MyGameData.QuestType)
    {
      case QuestType.Cult:
        switch (GameManager.Instance.MyGameData.Quest_Cult_Phase)
        {
          case 0:
            if (UIManager.Instance.CultUI.IsOpen) UIManager.Instance.CultUI.CloseUI_Auto();
            break;
          case 1:
            break;
          case 2:
            break;
        }
        break;
    }

    UIManager.Instance.AddUIQueue(UIManager.Instance.moverect(MyRect,MyRect.anchoredPosition, CenterPos, 0.4f,false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(MyGroup, 0.0f, UIManager.Instance.MapUI.UIOpenTime_Move));

    UIManager.Instance.MapUI.OpenUI();

  }
}
