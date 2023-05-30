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
    private Tendency CurrentTendency = null;
  private Vector2 ClosePos =new Vector2(1010.0f,0.0f);
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
    BackButton.interactable = true;
    BackButton.blocksRaycasts = true;
    TendencyType _tendencytype = (TendencyType)_index;
    //�̼�, ��ü, ����, ����
    if (UIManager.Instance.IsWorking) return;
    if (IsOpen && CurrentTendencyType == _tendencytype) { CloseUI(); IsOpen = false; return; }
    //������ ���� �ٽ� Ŭ���ϸ� �ݱ�

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
    BackButton.interactable = false;
    BackButton.blocksRaycasts = false;
    StartCoroutine( UIManager.Instance.CloseUI(MyRect, OpenPos,ClosePos, UIManager.Instance.LargePanelMoveTime,true));
    IsOpen = false;
    CurrentTendency = null;
        CurrentTendencyType = TendencyType.None;
  }
}
