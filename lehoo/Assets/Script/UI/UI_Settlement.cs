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
  [SerializeField] private TextMeshProUGUI PlaceName = null;
  [SerializeField] private TextMeshProUGUI PlaceDescription = null;
  [SerializeField] private TextMeshProUGUI PlaceEffect = null;
  [SerializeField] private Button RestButton = null;
  [SerializeField] private TextMeshProUGUI RestButtonText = null;
  private Settlement CurrentSettlement = null;
  private PlaceType SelectedPlace = PlaceType.NULL;
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

    PlaceName.text = "";
    PlaceDescription.text = "";
    PlaceEffect.text = "";
    RestButton.interactable = false;
    RestButtonText.text = GameManager.Instance.GetTextData("SELECTPLACE");

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
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect(_rectname).Rect, GetPanelRect(_rectname).OutisdePos, GetPanelRect(_rectname).InsidePos, UIOpenMoveTime, UIManager.Instance.UIPanelOpenCurve));
    yield return LittleWait;
    UIManager.Instance.MapButton.Open(0, this);
  }
  public override void CloseUI() => UIManager.Instance.AddUIQueue(closeui());
  private IEnumerator closeui()
  {
    UIManager.Instance.MapButton.Close();

    string _rectname = "description";
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect(_rectname).Rect, GetPanelRect(_rectname).InsidePos, GetPanelRect(_rectname).OutisdePos, UICloseMoveTime, UIManager.Instance.UIPanelCLoseCurve));
    yield return LittleWait;
    _rectname = "name";
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect(_rectname).Rect, GetPanelRect(_rectname).InsidePos, GetPanelRect(_rectname).OutisdePos, UICloseMoveTime, UIManager.Instance.UIPanelCLoseCurve));
    yield return LittleWait;
    _rectname = "discomfort";
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect(_rectname).Rect, GetPanelRect(_rectname).InsidePos, GetPanelRect(_rectname).OutisdePos, UICloseMoveTime, UIManager.Instance.UIPanelCLoseCurve));
    yield return LittleWait;
    _rectname = "placepanel";
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect(_rectname).Rect, GetPanelRect(_rectname).InsidePos, GetPanelRect(_rectname).OutisdePos, UICloseMoveTime, UIManager.Instance.UIPanelCLoseCurve));
  }

  public void SelectPlace(int index)
  {
    SelectedPlace = (PlaceType)index;

    PlaceName.text = GameManager.Instance.GetTextData(SelectedPlace, 0);
    PlaceDescription.text = GameManager.Instance.GetTextData(SelectedPlace, 1);
    string _effect = GameManager.Instance.GetTextData(SelectedPlace, 3);
    switch (SelectedPlace)
    {
      case PlaceType.Residence:
        _effect = string.Format(_effect, ConstValues.PlaceEffect_residence);
        break;
      case PlaceType.Marketplace:
        _effect = string.Format(_effect, ConstValues.PlaceEffect_marketplace);
        break;
      case PlaceType.Temple:
        _effect = string.Format(_effect, ConstValues.PlaceEffect_temple);
        break;
      case PlaceType.Library:
        _effect = string.Format(_effect, ConstValues.PlaceDuration,CurrentSettlement.LibraryType, ConstValues.PlaceEffect_Library);
        break;
      case PlaceType.Theater:
        //서비스 종료다...!
        break;
      case PlaceType.Academy:
        _effect = string.Format(_effect,ConstValues.PlaceDuration, ConstValues.PlaceEffect_acardemy);
        break;
    }
    PlaceEffect.text = _effect;

    int _movepointvalue = 0;
    int _discomfortvalue = 0;
    switch (CurrentSettlement.Type)
    {
      case SettlementType.Town:
        _movepointvalue = ConstValues.RestMovePoint_Town;
        _discomfortvalue = ConstValues.RestDiscomfort_Town;
        break;
      case SettlementType.City: 
        _movepointvalue = ConstValues.RestMovePoint_City;
        _discomfortvalue = ConstValues.RestDiscomfort_City;
        break;
      case SettlementType.Castle: 
        _movepointvalue = ConstValues.RestMovePoint_Castle;
        _discomfortvalue = ConstValues.RestDiscomfort_Castle;
        break;
    }
    if (SelectedPlace == PlaceType.Residence) _movepointvalue++;
    RestButtonText.text = string.Format(GameManager.Instance.GetTextData("REST"),
      WNCText.GetSanityColor("-" + GameManager.Instance.MyGameData.SettleRestCost),
      WNCText.GetMovepointColor("+" + _movepointvalue),
      WNCText.GetDiscomfortColor("+" + _discomfortvalue));
    RestButton.interactable = true;

  }
  public void StartEvent()
    {
    GameManager.Instance.MyGameData.CurrentSanity -= GameManager.Instance.MyGameData.SettleRestCost;
    UIManager.Instance.UpdateSanityText();
    GameManager.Instance.MyGameData.AddDiscomfort(CurrentSettlement);


    Debug.Log("이벤트 시작인레후~~");
        CloseUI();
        EventManager.Instance.SetSettleEvent(SelectedPlace);
    }//시작 버튼 눌렀을 때 호출
}
