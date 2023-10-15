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


    SettlementUI.OpenUI(true);
    Close();
  }
}
