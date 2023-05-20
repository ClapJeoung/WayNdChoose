using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Mad : UI_default
{
  [SerializeField] private CanvasGroup PanelGroup = null;
  [SerializeField] private TextMeshProUGUI Name = null;
  [SerializeField] private Image Illust = null;
  [SerializeField] private TextMeshProUGUI Description = null;
    public void OpenUI(Experience exp)
  {
    Name.text = exp.Name;
    Illust.sprite = exp.Illust;
    Description.text = exp.TextData.SelectionSubDescription;
    StartCoroutine(UIManager.Instance.ChangeAlpha(MyGroup, 1.0f, true));
    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(PanelGroup,1.0f,false,UIFadeMoveDir.Down));
  }
  public override void CloseUI()
  {
    StartCoroutine(UIManager.Instance.ChangeAlpha(MyGroup,0.0f,true));
    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(PanelGroup, 0.0f, true, UIFadeMoveDir.Down));
  }
}
