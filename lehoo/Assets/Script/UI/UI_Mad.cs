using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Mad : UI_default
{
  [SerializeField] private CanvasGroup PanelGroup = null;
  [SerializeField] private TextMeshProUGUI Name = null;
  [SerializeField] private TextMeshProUGUI Description = null;
  private Experience MadExp = null;
    public void OpenUI(Experience exp)
  {
    MadExp = exp;
    Name.text = exp.Name;
    Description.text = exp.TextData.SelectionSubDescription;
    StartCoroutine(UIManager.Instance.ChangeAlpha(MyGroup, 1.0f, true,false));
    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(PanelGroup,1.0f,false,UIFadeMoveDir.Down, false));
  }
  public override void CloseUI()
  {
    StartCoroutine(UIManager.Instance.ChangeAlpha(MyGroup,0.0f,true, false));
    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(PanelGroup, 0.0f, true, UIFadeMoveDir.Down, false));
  }
  public void AddMad()
  {
    GameManager.Instance.MadExpDic.Clear();
    GameManager.Instance.AddBadExp(MadExp);
  }
}
