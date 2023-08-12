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
  [SerializeField] private CanvasGroup BackButton = null; 
  private Vector2 ClosePos =new Vector2(1500.0f,0.0f);
  private Vector2 OpenPos =new Vector2(309.0f,0.0f);
  public void OpenLongExpUI()
  {
    Experience _exp = GameManager.Instance.MyGameData.LongTermEXP;
    //나중에 인수도 받아야함
    if (UIManager.Instance.IsWorking || _exp == null) return;
    if (IsOpen && CurrentExp.Equals(_exp)) { CloseUI(); IsOpen = false; return; }
    DefaultGroup.alpha = 1.0f;
    DefaultGroup.interactable = true;
    DefaultGroup.blocksRaycasts = true;
    BackButton.interactable = true;
    BackButton.blocksRaycasts = true;

    TouchBlock.enabled = true;
    IsOpen = true;

    string _name = _exp.Name;
    Sprite _illust = _exp.Illust;
    string _description = _exp.Description;
    string _effect = _exp.EffectString;
    if (CurrentExp == null)
    {
      CurrentExp= _exp;
      ExpName.text = _name;
      ExpIllust.sprite = _illust;
      ExpEffect.text = _description + "\n\n" + _effect;
      ExpTurn.text = _exp.Duration.ToString();

      UIManager.Instance.AddUIQueue(UIManager.Instance.OpenUI(DefaultRect, ClosePos, OpenPos, UIManager.Instance.LargePanelMoveTime, true));
    }//최초는 아무 경험이나 클릭하면 열기
    else
    {
      if (CurrentExp == _exp) CloseUI();    //같은 경험 클릭하면 닫기
      else
      {
        CurrentExp = _exp;
        ExpName.text = _name;
        ExpIllust.sprite = _illust;
        ExpEffect.text = _description + "\n\n" + _effect;
        ExpTurn.text = _exp.Duration.ToString();

      }//다른 경험 클릭하면 해당 경험 정보로 대체
    }
  }
  public void OpenShortExpUI(int _index)
  {
    Experience _exp = GameManager.Instance.MyGameData.ShortTermEXP[_index];
    //나중에 인수도 받아야함
    if (UIManager.Instance.IsWorking || _exp == null) return;
    if (IsOpen && CurrentExp.Equals(_exp)) { CloseUI(); IsOpen = false; return; }
    DefaultGroup.alpha = 1.0f;
    DefaultGroup.interactable = true;
    DefaultGroup.blocksRaycasts = true;
    BackButton.interactable = true;
    BackButton.blocksRaycasts = true;

    TouchBlock.enabled = true;
    IsOpen = true;

    string _name = _exp.Name;
    Sprite _illust = _exp.Illust;
    string _description = _exp.Description;
    string _effect = _exp.EffectString;
    if (CurrentExp == null)
    {
      CurrentExp = _exp;
      ExpName.text = _name;
      ExpIllust.sprite = _illust;
      ExpEffect.text = _description + "\n\n" + _effect;
      ExpTurn.text = _exp.Duration.ToString();

      UIManager.Instance.AddUIQueue(UIManager.Instance.OpenUI(DefaultRect, ClosePos, OpenPos, UIManager.Instance.LargePanelMoveTime, true));
    }//최초는 아무 경험이나 클릭하면 열기
    else
    {
      if (CurrentExp == _exp) CloseUI();    //같은 경험 클릭하면 닫기
      else
      {
        CurrentExp = _exp;
        ExpName.text = _name;
        ExpIllust.sprite = _illust;
        ExpEffect.text = _description + "\n\n" + _effect;
        ExpTurn.text = _exp.Duration.ToString();

      }//다른 경험 클릭하면 해당 경험 정보로 대체
    }
  }
  public override void CloseUI()
  {
    IsOpen = false;
    BackButton.interactable = false;
    BackButton.blocksRaycasts = false;
    CurrentExp = null;
    UIManager.Instance.CurrentTopUI = null;

    TouchBlock.enabled = false;
    StartCoroutine(UIManager.Instance.ChangeAlpha(DefaultGroup, 0.0f, 0.1f, false));
    StartCoroutine( UIManager.Instance.CloseUI(DefaultRect, OpenPos,ClosePos, UIManager.Instance.LargePanelMoveTime,false));
  }
}
