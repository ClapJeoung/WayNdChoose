using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapButton : ReturnButton
{
  public override void Clicked()
  {
    base.Clicked();
    if (UIManager.Instance.IsWorking) return;

    if (GameManager.Instance.MyGameData.CurrentSettlement != null)
    {
      if (GameManager.Instance.MyGameData.FirstRest)
      {
        UIManager.Instance.DialogueUI.OpenQuitAsk();
        return;
      }
     if(GameManager.Instance.MyGameData.Tendency_Head.Level == 2) GameManager.Instance.MyGameData.MovePoint += ConstValues.Tendency_Head_p2;
    }
    else
    {
      if (UIManager.Instance.DialogueUI.RemainReward)
      {
        UIManager.Instance.DialogueUI.OpenRewardAsk(this);
        return;
      }
    }

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
