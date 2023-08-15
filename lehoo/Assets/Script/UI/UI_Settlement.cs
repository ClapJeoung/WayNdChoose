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
  private WaitForSeconds LittleWait = new WaitForSeconds(0.2f);
  private float UICloseMoveTime = 0.5f;

  [SerializeField] private TextMeshProUGUI SettleNameText = null;
  [SerializeField] private TextMeshProUGUI DiscomfortText = null;
  [SerializeField] private List<Button> PlaceIcons=new List<Button>();
  [SerializeField] private TextMeshProUGUI PlaceName = null;
  [SerializeField] private TextMeshProUGUI PlaceDescription = null;
  [SerializeField] private TextMeshProUGUI PlaceEffect = null;
  [SerializeField] private Button RestButton_Sanity = null;
  [SerializeField] private TextMeshProUGUI RestButtonText_Sanity = null;
  [SerializeField] private Button RestButton_Gold = null;
  [SerializeField] private TextMeshProUGUI RestButtonText_Gold = null;
  private Settlement CurrentSettlement = null;
  private PlaceType SelectedPlace = PlaceType.NULL;
  public void OpenUI()
  {
    IsOpen = true;
    UIManager.Instance.AddUIQueue(openui());
  }
  private IEnumerator openui()
  {
    if (DefaultGroup.interactable == true) DefaultGroup.interactable = false;
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
    RestButton_Gold.interactable = false;
    RestButtonText_Gold.text = GameManager.Instance.GetTextData("SELECTPLACE");
    RestButton_Sanity.interactable = false;
    RestButtonText_Sanity.text = GameManager.Instance.GetTextData("SELECTPLACE");

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

    DefaultGroup.interactable = true;
  }
  public override void CloseUI()
  {
    IsOpen = false;
    UIManager.Instance.AddUIQueue(closeui());
  }
  private IEnumerator closeui()
  {
    DefaultGroup.interactable = false;

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
  public override void CloseForGameover()
  {
    IsOpen = false;

    UIManager.Instance.MapButton.CloseForGameover();

    string _rectname = "description";
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect(_rectname).Rect, GetPanelRect(_rectname).Rect.anchoredPosition, GetPanelRect(_rectname).OutisdePos, UICloseMoveTime, UIManager.Instance.UIPanelCLoseCurve));
    _rectname = "name";
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect(_rectname).Rect, GetPanelRect(_rectname).Rect.anchoredPosition, GetPanelRect(_rectname).OutisdePos, UICloseMoveTime, UIManager.Instance.UIPanelCLoseCurve));
    _rectname = "discomfort";
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect(_rectname).Rect, GetPanelRect(_rectname).Rect.anchoredPosition, GetPanelRect(_rectname).OutisdePos, UICloseMoveTime, UIManager.Instance.UIPanelCLoseCurve));
    _rectname = "placepanel";
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect(_rectname).Rect, GetPanelRect(_rectname).Rect.anchoredPosition, GetPanelRect(_rectname).OutisdePos, UICloseMoveTime, UIManager.Instance.UIPanelCLoseCurve));
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

    RestButtonText_Sanity.text = string.Format(GameManager.Instance.GetTextData("REST"),
      GameManager.Instance.GetTextData(StatusType.Sanity,2),
      WNCText.GetSanityColor("-" + GameManager.Instance.MyGameData.SettleRestCost_Sanity),
      WNCText.GetMovepointColor("+" + _movepointvalue),
      WNCText.GetDiscomfortColor("+" + _discomfortvalue));
    RestButton_Sanity.interactable = true;

    int _goldpayvalue = GameManager.Instance.MyGameData.SettleRestCost_Gold;

    RestButtonText_Gold.text = string.Format(GameManager.Instance.GetTextData("REST"),
      GameManager.Instance.GetTextData(StatusType.Gold, 2),
      WNCText.GetSanityColor("-" + _goldpayvalue),
      WNCText.GetMovepointColor("+" + _movepointvalue),
      WNCText.GetDiscomfortColor("+" + _discomfortvalue));

    if (GameManager.Instance.MyGameData.Gold >= _goldpayvalue) RestButton_Gold.interactable = true;
    else RestButton_Gold.interactable = false;
  }
  public void StartRest_Sanity()
    {
    if (UIManager.Instance.IsWorking) return;

    GameManager.Instance.MyGameData.CurrentSanity -= GameManager.Instance.MyGameData.SettleRestCost_Sanity;
    UIManager.Instance.UpdateSanityText();
    GameManager.Instance.MyGameData.AddDiscomfort(CurrentSettlement);

    CloseUI();
    EventManager.Instance.SetSettleEvent(SelectedPlace);
  }
  public void StartRest_Gold()
  {
    if (UIManager.Instance.IsWorking) return;

    GameManager.Instance.MyGameData.Gold -= GameManager.Instance.MyGameData.SettleRestCost_Gold;
    UIManager.Instance.UpdateGoldText();
    GameManager.Instance.MyGameData.AddDiscomfort(CurrentSettlement);

    CloseUI();
    EventManager.Instance.SetSettleEvent(SelectedPlace);
  }
}
