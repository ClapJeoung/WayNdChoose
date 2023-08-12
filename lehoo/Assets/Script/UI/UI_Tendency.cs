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
    //�̼�, ��ü, ����, ����
    if (UIManager.Instance.IsWorking) return;
    if (IsOpen && CurrentTendencyType == _tendencytype) { CloseUI(); IsOpen = false; return; }
    //������ ���� �ٽ� Ŭ���ϸ� �ݱ�
    DefaultGroup.alpha = 1.0f;
    DefaultGroup.interactable = true;
    DefaultGroup.blocksRaycasts = true;
    BackButton.interactable = true;
    BackButton.blocksRaycasts = true;

    IsOpen = true;
    TouchBlock.enabled = true;
    Tendency _tendency = GetTendencyByType(_tendencytype);

    string _tendencyname = _tendency.Name;
    string _description = _tendency.Description;
    string _effect = GameManager.Instance.MyGameData.GetTendencyEffectString_long(_tendencytype);

    Illust.sprite = _tendency.Illust;
    Description.text = _description + "\n\n" + _effect;

    if (CurrentTendencyType.Equals(TendencyType.None))
    {
      UIManager.Instance.AddUIQueue(UIManager.Instance.OpenUI(DefaultRect, ClosePos,OpenPos, UIManager.Instance.LargePanelMoveTime,true));
    }
    CurrentTendencyType = _tendencytype;
  }
  public override void CloseUI()
  {
    IsOpen = false;
    BackButton.interactable = false;
    BackButton.blocksRaycasts = false;
    StartCoroutine(UIManager.Instance.ChangeAlpha(DefaultGroup, 0.0f, 0.1f, false));
    StartCoroutine( UIManager.Instance.CloseUI(DefaultRect, OpenPos,ClosePos, UIManager.Instance.LargePanelMoveTime,false));
    UIManager.Instance.CurrentTopUI = null;
        CurrentTendencyType = TendencyType.None;
  }
}
