using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Tendency : UI_default
{
  [SerializeField] private Image TouchBlock = null;
  [SerializeField] private Image Illust = null;
  [SerializeField] private TextMeshProUGUI Name = null;
    [SerializeField] private TextMeshProUGUI Description = null;
  [SerializeField] private Image Icon = null;
    private TendencyTypeEnum CurrentTendencyType = TendencyTypeEnum.None;
    private Tendency GetTendencyByType(TendencyTypeEnum _type)
    {
        switch (_type)
        {
            case TendencyTypeEnum.Body:
                return GameManager.Instance.MyGameData.Tendency_Body;
            case TendencyTypeEnum.Head:
                return GameManager.Instance.MyGameData.Tendency_Head;
            default:return null;
        }
    }
  public void OpenUI(int _index)
  {
    TendencyTypeEnum _tendencytype = (TendencyTypeEnum)_index;
    if (GameManager.Instance.MyGameData.GetTendencyLevel(_tendencytype) == 0) return;
    if (UIManager.Instance.IsWorking) return;
    if (IsOpen && CurrentTendencyType == _tendencytype) { CloseUI(); IsOpen = false; return; }
    //동일한 성향 다시 클릭하면 닫기
    DefaultGroup.alpha = 1.0f;
    DefaultGroup.interactable = true;
    DefaultGroup.blocksRaycasts = true;

    UIManager.Instance.CloseOtherStatusPanels(this);

    IsOpen = true;
    TouchBlock.enabled = true;
    Tendency _tendency = GetTendencyByType(_tendencytype);

    string _tendencyname = _tendency.Name;
    Sprite _icon = _tendency.CurrentIcon;
    string _description = _tendency.Description;
    string _effect = _tendency.GetTendencyEffectString;

    Name.text= _tendencyname;
    Icon.sprite= _icon;
    Illust.sprite = _tendency.Illust;
    Description.text = _description + "<br><br>" + _effect;
    LayoutRebuilder.ForceRebuildLayoutImmediate(Description.transform as RectTransform);
    LayoutRebuilder.ForceRebuildLayoutImmediate(GetPanelRect("myrect").Rect);

    if (CurrentTendencyType.Equals(TendencyTypeEnum.None))
    {
   //   UIManager.Instance.AddUIQueue(UIManager.Instance.OpenUI(GetPanelRect("myrect").Rect, GetPanelRect("myrect").OutisdePos, GetPanelRect("myrect").InsidePos, UIManager.Instance.LargePanelMoveTime));
    }
    CurrentTendencyType = _tendencytype;
  }
  public void CloseUI()
  {
    TouchBlock.enabled = false;
    IsOpen = false;
    StartCoroutine(UIManager.Instance.ChangeAlpha(DefaultGroup, 0.0f, 0.1f));
   // StartCoroutine( UIManager.Instance.CloseUI(GetPanelRect("myrect").Rect, GetPanelRect("myrect").InsidePos, GetPanelRect("myrect").OutisdePos, UIManager.Instance.LargePanelMoveTime));
        CurrentTendencyType = TendencyTypeEnum.None;
  }
}
