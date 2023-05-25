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
    private Tendency CurrentTendency = null;
    private TendencyType CurrentTendencyType = TendencyType.None;
    private Tendency GetTendencyByType(TendencyType _type)
    {
        switch (_type)
        {
            case TendencyType.Body:
                return GameManager.Instance.MyGameData.Tendency_Body;
            case TendencyType.Head:
                return GameManager.Instance.MyGameData.Tendency_Head;
            default:return null;
        }
    }

  public void OpenUI(int _index)
  {
    TendencyType _tendencytype = (TendencyType)_index;
    //이성, 육체, 정신, 물질
    if (UIManager.Instance.IsWorking) return;
    if (IsOpen && CurrentTendencyType == _tendencytype) { CloseUI(); IsOpen = false; return; }
    //동일한 성향 다시 클릭하면 닫기

    IsOpen = true;
    TouchBlock.enabled = true;
    Tendency _tendency = GetTendencyByType(_tendencytype);

    TextData _textdata = _tendency.MyTextData;

    string _tendencyname = _textdata.Name;
    string _description = _textdata.Description;
    string _effect = GameManager.Instance.MyGameData.GetTendencyEffectString_long(_tendencytype);

    Illust.sprite = _tendency.Illust;
    Description.text = _description + "\n\n" + _effect;
    UIManager.Instance.AddUIQueue(UIManager.Instance.OpenUI(MyRect, MyDir, UIManager.Instance.LargePanelMoveTime));
  }
  public override void CloseUI()
  {
    TouchBlock.enabled = false;
    UIManager.Instance.AddUIQueue(UIManager.Instance.CloseUI(MyRect, MyDir, UIManager.Instance.LargePanelMoveTime));
    IsOpen = false;
    CurrentTendency = null;
        CurrentTendencyType = TendencyType.None;
  }
}
