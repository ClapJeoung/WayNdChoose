using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapButton : ReturnButton
{
  public override void Clicked()
  {
    base.Clicked();

    if (GameManager.Instance.MyGameData.CurrentSettlement != null && GameManager.Instance.MyGameData.Tendency_Head.Level == -2)
      GameManager.Instance.MyGameData.MovePoint += ConstValues.Tendency_Head_m2;

      if (CurrentUI as UI_dialogue != null)
    {
      UI_dialogue _dialogue = CurrentUI as UI_dialogue;
      {
        UIManager.Instance.DialogueUI.CloseUI(false);
      }
    }

    switch (GameManager.Instance.MyGameData.QuestType)
    {
      case QuestType.Cult:
        if (UIManager.Instance.CultUI.IsOpen)
        {
          UIManager.Instance.CultUI.CloseUI_Auto();
          UIManager.Instance.MapUI.OpenUI(false);
          return;
        }
        break;
    }

    UIManager.Instance.MapUI.OpenUI(true);
  }
}
