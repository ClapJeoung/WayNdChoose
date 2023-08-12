using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics.Tracing;
using Google.Apis.Json;

public class UI_Settlement : UI_default
{
  private float UIOpenMoveTime = 0.2f;
  private float UIOpenFadeTime = 0.4f;
  private WaitForSeconds LittleWait = new WaitForSeconds(0.2f);
  private float UICloseMoveTime = 0.5f;
  private float UICloseFadeTime = 0.3f;

  [SerializeField] private TextMeshProUGUI SettleNameText = null;
  [SerializeField] private TextMeshProUGUI DiscomfortText = null;
  [SerializeField] private List<Button> PlaceIcons=new List<Button>();
  [SerializeField] private TextMeshProUGUI PlaceDescription = null;
  [SerializeField] private TextMeshProUGUI PlaceEffect = null;
  [SerializeField] private Button StartButton = null;
  [SerializeField] private TextMeshProUGUI StartButtonText = null;
  private Settlement CurrentSettlement = null;
  public void OpenUI() => UIManager.Instance.AddUIQueue(openui());
  private IEnumerator openui()
  {
    if(DefaultRect.anchoredPosition!=Vector2.zero)DefaultRect.anchoredPosition = Vector2.zero;

    CurrentSettlement = GameManager.Instance.MyGameData.CurrentSettlement;
    SettleNameText.text = CurrentSettlement.Name;
    DiscomfortText.text = CurrentSettlement.Discomfort.ToString();

    for(int i=0; i < PlaceIcons.Count; i++)
    {
      if (CurrentSettlement.Settlementplaces.Contains((PlaceType)i)) PlaceIcons[i].interactable = true;
      else PlaceIcons[i].interactable = false;
    }

    PlaceDescription.text = "";
    PlaceEffect.text = "";
    StartButton.interactable = false;
    StartButtonText.text = GameManager.Instance.GetTextData("SELECTPLACE");

    string _rectname = "name";
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect(_rectname).Rect, GetPanelRect(_rectname).OutisdePos, GetPanelRect(_rectname).InsidePos, UIOpenMoveTime, UIManager.Instance.UIPanelOpenCurve));
    yield return LittleWait;
    _rectname = "discomfort";
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect(_rectname).Rect, GetPanelRect(_rectname).OutisdePos, GetPanelRect(_rectname).InsidePos, UIOpenMoveTime, UIManager.Instance.UIPanelOpenCurve));
    yield return LittleWait;
    _rectname = "placepanel";
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect(_rectname).Rect, GetPanelRect(_rectname).OutisdePos, GetPanelRect(_rectname).InsidePos, UIOpenMoveTime, UIManager.Instance.UIPanelOpenCurve));
    yield return LittleWait;
    _rectname = "description";
    UIManager.Instance.MapButton.Open(0, this);
  }
  public override void CloseUI()
  {
    base.CloseUI();
  }

  public void StartEvent()
    {
    Debug.Log("이벤트 시작인레후~~");
        CloseUI();
        EventManager.Instance.SetSettleEvent(CurrentPlace);
    }//시작 버튼 눌렀을 때 호출
}
