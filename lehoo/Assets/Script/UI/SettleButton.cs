using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettleButton : ReturnButton
{
  [SerializeField] private Vector2 TopPos = Vector2.zero;
  [SerializeField] private UI_dialogue DialogueUI = null;
  public override void Clicked()
  {
    base.Clicked();
    if (UIManager.Instance.IsWorking) return;

    if (UIManager.Instance.DialogueUI.RemainReward)
    {
      UIManager.Instance.DialogueUI.OpenRewardAsk(this);
      return;
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
    UIManager.Instance.AddUIQueue(DialogueUI.openui_settlement(true));
  }
}
