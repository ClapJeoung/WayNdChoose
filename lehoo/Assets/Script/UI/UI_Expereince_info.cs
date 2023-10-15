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
  public void OpenLongExpUI()
  {
    Experience _exp = GameManager.Instance.MyGameData.LongExp;
    if (UIManager.Instance.IsWorking || _exp == null) return;
    if (IsOpen && CurrentExp.Equals(_exp)) { CloseUI(); IsOpen = false; return; }

    Setup(_exp);
  }
  public void OpenShortExpUI(int _index)
  {
    /*
    Experience _exp = GameManager.Instance.MyGameData.ShortTermEXP[_index];
    //���߿� �μ��� �޾ƾ���
    if (UIManager.Instance.IsWorking || _exp == null) return;
    if (IsOpen && CurrentExp.Equals(_exp)) { CloseUI(); IsOpen = false; return; }
    
    Setup(_exp);
    */
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

      LayoutRebuilder.ForceRebuildLayoutImmediate(ExpName.transform.parent.transform as RectTransform);
      LayoutRebuilder.ForceRebuildLayoutImmediate(ExpEffect.transform as RectTransform);

      ExpTurn.text = exp.Duration.ToString();

    //  UIManager.Instance.AddUIQueue(UIManager.Instance.OpenUI(GetPanelRect("myrect").Rect, GetPanelRect("myrect").OutisdePos, GetPanelRect("myrect").InsidePos, UIManager.Instance.LargePanelMoveTime));
    }//���ʴ� �ƹ� �����̳� Ŭ���ϸ� ����
    else
    {
      if (CurrentExp == exp) CloseUI();    //���� ���� Ŭ���ϸ� �ݱ�
      else
      {
        CurrentExp = exp;
        ExpName.text = _name;
        ExpIllust.sprite = _illust;
        ExpEffect.text = _description + "<br><br>" + _effect;
        ExpTurn.text = exp.Duration.ToString();
        LayoutRebuilder.ForceRebuildLayoutImmediate(ExpName.transform.parent.transform as RectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(ExpEffect.transform as RectTransform);

      }//�ٸ� ���� Ŭ���ϸ� �ش� ���� ������ ��ü
    }
    LayoutRebuilder.ForceRebuildLayoutImmediate(GetPanelRect("myrect").Rect);

  }
  public  void CloseUI()
  {
    IsOpen = false;
    BackButton.interactable = false;
    BackButton.blocksRaycasts = false;
    CurrentExp = null;
    TouchBlock.enabled = false;
    StartCoroutine(UIManager.Instance.ChangeAlpha(DefaultGroup, 0.0f, 0.1f));
   // StartCoroutine( UIManager.Instance.CloseUI(GetPanelRect("myrect").Rect, GetPanelRect("myrect").InsidePos, GetPanelRect("myrect").OutisdePos, UIManager.Instance.LargePanelMoveTime));
  }
}
