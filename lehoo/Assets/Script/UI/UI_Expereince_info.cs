using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_Expereince_info : UI_default
{

  private Experience CurrentExp = null;
  [SerializeField] private Image TouchBlock = null;
  [SerializeField] private TextMeshProUGUI ExpName = null;
  [SerializeField] private Image ExpIllust = null;
  [SerializeField] private TextMeshProUGUI ExpEffect = null;
  [SerializeField] private TextMeshProUGUI ExpTurn = null;
  [SerializeField] private Image MadnessIcon = null;
  [SerializeField] private CanvasGroup BackButton = null; 
  public void OpenLongExpUI()
  {
    Experience _exp = GameManager.Instance.MyGameData.LongTermEXP;
    if (UIManager.Instance.IsWorking || _exp == null) return;
    if (IsOpen && CurrentExp.Equals(_exp)) { CloseUI(); IsOpen = false; return; }

    Setup(_exp);
  }
  public void OpenShortExpUI(int _index)
  {
    Experience _exp = GameManager.Instance.MyGameData.ShortTermEXP[_index];
    //나중에 인수도 받아야함
    if (UIManager.Instance.IsWorking || _exp == null) return;
    if (IsOpen && CurrentExp.Equals(_exp)) { CloseUI(); IsOpen = false; return; }
    
    Setup(_exp);
  }
  public void Setup(Experience exp)
  {
    IsOpen = true;

    DefaultGroup.alpha = 1.0f;
    DefaultGroup.interactable = true;
    DefaultGroup.blocksRaycasts = true;
    BackButton.interactable = true;
    BackButton.blocksRaycasts = true;
    TouchBlock.enabled = true;

    UIManager.Instance.CloseOtherStatusPanels(this);

    IsOpen = true;

    string _name = exp.Name;
    Sprite _illust = exp.Illust;
    string _description = exp.Description;
    string _effect = exp.EffectString;
    if (CurrentExp == null)
    {
      CurrentExp = exp;
      ExpName.text = _name;
      ExpIllust.sprite = _illust;
      ExpEffect.text = _description + "<br><br>" + _effect;
      LayoutRebuilder.ForceRebuildLayoutImmediate(ExpEffect.transform as RectTransform);

      if (exp.ExpType == ExpTypeEnum.Mad)
      {
        if (MadnessIcon.enabled == false)
        {
          MadnessIcon.enabled = true;
          ExpTurn.text = "";
        }
      }
      else
      {
        if (MadnessIcon.enabled == true) MadnessIcon.enabled = false;
        ExpTurn.text = exp.Duration.ToString();
      }

      UIManager.Instance.AddUIQueue(UIManager.Instance.OpenUI(GetPanelRect("myrect").Rect, GetPanelRect("myrect").OutisdePos, GetPanelRect("myrect").InsidePos, UIManager.Instance.LargePanelMoveTime));
    }//최초는 아무 경험이나 클릭하면 열기
    else
    {
      if (CurrentExp == exp) CloseUI();    //같은 경험 클릭하면 닫기
      else
      {
        CurrentExp = exp;
        ExpName.text = _name;
        ExpIllust.sprite = _illust;
        ExpEffect.text = _description + "<br><br>" + _effect;
        ExpTurn.text = exp.Duration.ToString();
        LayoutRebuilder.ForceRebuildLayoutImmediate(ExpEffect.transform as RectTransform);

      }//다른 경험 클릭하면 해당 경험 정보로 대체
    }

  }
  public override void CloseUI()
  {
    IsOpen = false;
    BackButton.interactable = false;
    BackButton.blocksRaycasts = false;
    CurrentExp = null;
    TouchBlock.enabled = false;
    StartCoroutine(UIManager.Instance.ChangeAlpha(DefaultGroup, 0.0f, 0.1f));
    StartCoroutine( UIManager.Instance.CloseUI(GetPanelRect("myrect").Rect, GetPanelRect("myrect").InsidePos, GetPanelRect("myrect").OutisdePos, UIManager.Instance.LargePanelMoveTime));
  }
  public override void CloseForGameover()
  {
    IsOpen = false;
    BackButton.interactable = false;
    BackButton.blocksRaycasts = false;
    CurrentExp = null;
    TouchBlock.enabled = false;
    StartCoroutine(UIManager.Instance.ChangeAlpha(DefaultGroup, 0.0f, 0.1f));
    StartCoroutine(UIManager.Instance.CloseUI(GetPanelRect("myrect").Rect, GetPanelRect("myrect").Rect.anchoredPosition, GetPanelRect("myrect").OutisdePos, UIManager.Instance.LargePanelMoveTime));
  }
}
