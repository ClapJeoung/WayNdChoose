using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Tendency : UI_default
{
  [SerializeField] private Image TouchBlock = null;
  [SerializeField] private Image Illust = null;
    [SerializeField] private TextMeshProUGUI Description = null;
  private Vector2 ClosePos =new Vector2(1500.0f,0.0f);
  private Vector2 OpenPos =new Vector2(304.0f,0.0f);
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
  [SerializeField] private CanvasGroup BackButton = null;
  public void OpenUI(int _index)
  {
    TendencyType _tendencytype = (TendencyType)_index;
    //이성, 육체, 정신, 물질
    if (UIManager.Instance.IsWorking) return;
    if (IsOpen && CurrentTendencyType == _tendencytype) { CloseUI(); IsOpen = false; return; }
    //동일한 성향 다시 클릭하면 닫기
    MyGroup.alpha = 1.0f;
    MyGroup.interactable = true;
    MyGroup.blocksRaycasts = true;
    BackButton.interactable = true;
    BackButton.blocksRaycasts = true;

    IsOpen = true;
    TouchBlock.enabled = true;
    Tendency _tendency = GetTendencyByType(_tendencytype);

    TextData _textdata = _tendency.MyTextData;

    string _tendencyname = _textdata.Name;
    string _description = _textdata.Description;
    string _effect = GameManager.Instance.MyGameData.GetTendencyEffectString_long(_tendencytype);

    Illust.sprite = _tendency.Illust;
    Description.text = _description + "\n\n" + _effect;

    if (CurrentTendencyType.Equals(TendencyType.None))
    {
      UIManager.Instance.AddUIQueue(UIManager.Instance.OpenUI(MyRect, ClosePos,OpenPos, UIManager.Instance.LargePanelMoveTime,true));
    }
    CurrentTendencyType = _tendencytype;
  }
  public override void CloseUI()
  {
    IsOpen = false;
    BackButton.interactable = false;
    BackButton.blocksRaycasts = false;
    StartCoroutine(UIManager.Instance.ChangeAlpha(MyGroup, 0.0f, 0.1f, false));
    StartCoroutine( UIManager.Instance.CloseUI(MyRect, OpenPos,ClosePos, UIManager.Instance.LargePanelMoveTime,false));
    UIManager.Instance.CurrentTopUI = null;
        CurrentTendencyType = TendencyType.None;
  }
}
