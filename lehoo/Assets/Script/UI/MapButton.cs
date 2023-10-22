using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapButton : ReturnButton
{
  public override void Clicked()
  {
    base.Clicked();

    if (GameManager.Instance.MyGameData.CurrentSettlement != null && GameManager.Instance.MyGameData.Tendency_Head.Level < 0)
      GameManager.Instance.MyGameData.MovePoint++;

      if (CurrentUI as UI_dialogue != null)
    {
      UI_dialogue _dialogue = CurrentUI as UI_dialogue;
 /*     if (_dialogue.RemainReward == true && Warned == false)
      {
        WarningDescription.text = GameManager.Instance.GetTextData("NOREWARD");
        SetWarningButton();
        return;
      }
      else*/
      {
        UIManager.Instance.DialogueUI.CloseUI(false);
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

    UIManager.Instance.MapUI.OpenUI(Dir == 0 );

    Close();
  }
}
