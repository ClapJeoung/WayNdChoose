using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics.Tracing;
using Google.Apis.Json;

public class UI_Settlement : UI_default
{
  private float UIOpenMoveTime = 0.8f;
  private WaitForSeconds LittleWait = new WaitForSeconds(0.2f);
  private float UICloseMoveTime = 0.6f;

  [SerializeField] private Image SettlementIcon = null;
  [SerializeField] private TextMeshProUGUI SettlementNameText = null;
  [SerializeField] private TextMeshProUGUI DiscomfortText = null;
  [SerializeField] private List<PlaceIconScript> SectorIcons=new List<PlaceIconScript>();
  private PlaceIconScript GetSectorIconScript(SectorType sectortype)
  {
    for(int i=0;i< SectorIcons.Count;i++)
    {
      if (SectorIcons[i].MyType == sectortype) return SectorIcons[i];
    }
    return null;
  }
  [SerializeField] private TextMeshProUGUI SectorName = null;
  [SerializeField] private TextMeshProUGUI SectorSelectDescription = null;
  [SerializeField] private GameObject RestbuttonHolder = null;
  [SerializeField] private Button RestButton_Sanity = null;
  [SerializeField] private Button RestButton_Gold = null;
  [SerializeField] private TextMeshProUGUI RestDescription = null;
  private Settlement CurrentSettlement = null;
  private SectorType SelectedSector = SectorType.NULL;
  public void OpenUI()
  {
    IsOpen = true;
    UIManager.Instance.AddUIQueue(openui());
  }
  private IEnumerator openui()
  {
    if (DefaultGroup.interactable == true) DefaultGroup.interactable = false;
    if(DefaultRect.anchoredPosition!=Vector2.zero)DefaultRect.anchoredPosition = Vector2.zero;

    if(RestbuttonHolder.activeInHierarchy==true) RestbuttonHolder.SetActive(false);
    SelectedSector = SectorType.NULL;
    CurrentSettlement = GameManager.Instance.MyGameData.CurrentSettlement;
    SettlementNameText.text = CurrentSettlement.Name;
    DiscomfortText.text = CurrentSettlement.Discomfort.ToString();
    RestDescription.text = "";

    Sprite _settlementicon = null;
    int _placecount = 0;
    switch (CurrentSettlement.SettlementType)
    {
      case SettlementType.Village: _placecount = 2;_settlementicon = GameManager.Instance.ImageHolder.VillageIcon_black; break;
      case SettlementType.Town:_placecount = 3; _settlementicon = GameManager.Instance.ImageHolder.TownIcon_black; break;
      case SettlementType.City:_placecount = 4; _settlementicon = GameManager.Instance.ImageHolder.CityIcon_black; break;
    }
    for (int i = 0; i < SectorIcons.Count; i++)
    {
      if (i < _placecount)
      {
        if (SectorIcons[i].gameObject.activeInHierarchy == false) SectorIcons[i].gameObject.SetActive(true);
        SectorIcons[i].OpenIcon();
      }
      else
      {
        if (SectorIcons[i].gameObject.activeInHierarchy == true) SectorIcons[i].gameObject.SetActive(false);
      }
    }

    SettlementIcon.sprite = _settlementicon;
    SectorName.gameObject.SetActive(false);
    SectorName.text = "";
    if (SectorSelectDescription.gameObject.activeInHierarchy == false) SectorSelectDescription.gameObject.SetActive(true);
    SectorSelectDescription.text = GameManager.Instance.GetTextData("SELECTPLACE");
    RestButton_Gold.interactable = false;
    RestButton_Sanity.interactable = false;
    LayoutRebuilder.ForceRebuildLayoutImmediate(SectorName.transform.parent.transform as RectTransform);
    LayoutRebuilder.ForceRebuildLayoutImmediate(SettlementIcon.transform.parent.transform as RectTransform);
    LayoutRebuilder.ForceRebuildLayoutImmediate(RestbuttonHolder.transform as RectTransform);

    string _rectname = "nameholder";
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect(_rectname).Rect, GetPanelRect(_rectname).OutisdePos, GetPanelRect(_rectname).InsidePos, UIOpenMoveTime, true));
    yield return LittleWait;
    _rectname = "placepanel";
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect(_rectname).Rect, GetPanelRect(_rectname).OutisdePos, GetPanelRect(_rectname).InsidePos, UIOpenMoveTime, true));
    yield return LittleWait;
    _rectname = "description";
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect(_rectname).Rect, GetPanelRect(_rectname).OutisdePos, GetPanelRect(_rectname).InsidePos, UIOpenMoveTime, true));
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
    if (UIManager.Instance.MapButton.IsOpen) UIManager.Instance.MapButton.Close();

    string _rectname = "description";
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect(_rectname).Rect, GetPanelRect(_rectname).InsidePos, GetPanelRect(_rectname).OutisdePos, UICloseMoveTime, false));
    yield return LittleWait;
    _rectname = "nameholder";
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect(_rectname).Rect, GetPanelRect(_rectname).InsidePos, GetPanelRect(_rectname).OutisdePos, UICloseMoveTime, false));
    yield return LittleWait;
    _rectname = "placepanel";
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect(_rectname).Rect, GetPanelRect(_rectname).InsidePos, GetPanelRect(_rectname).OutisdePos, UICloseMoveTime, false));
  }
  public override void CloseForGameover()
  {
    IsOpen = false;

    UIManager.Instance.MapButton.CloseForGameover();

    string _rectname = "description";
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect(_rectname).Rect, GetPanelRect(_rectname).Rect.anchoredPosition, GetPanelRect(_rectname).OutisdePos, UICloseMoveTime, false));
    _rectname = "nameholder";
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect(_rectname).Rect, GetPanelRect(_rectname).Rect.anchoredPosition, GetPanelRect(_rectname).OutisdePos, UICloseMoveTime, false));
    _rectname = "placepanel";
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect(_rectname).Rect, GetPanelRect(_rectname).Rect.anchoredPosition, GetPanelRect(_rectname).OutisdePos, UICloseMoveTime, false));
  }
  public void SelectPlace(int index)  //Sectortype�� 0�� NULL��
  {
    if (SelectedSector == (SectorType)index) return;

    if(SelectedSector!=SectorType.NULL) GetSectorIconScript(SelectedSector).SetIdleColor();
    SelectedSector = (SectorType)index;

    if(SectorName.gameObject.activeInHierarchy==false) SectorName.gameObject.SetActive(true);
    if(SectorSelectDescription.gameObject.activeInHierarchy==false) SectorSelectDescription.gameObject.SetActive(true);

    switch (GameManager.Instance.MyGameData.QuestType)
    {
      case QuestType.Cult:
        GetSectorIconScript(SelectedSector).SetSelectColor();
        if (GameManager.Instance.MyGameData.Quest_Cult_Sabbat_BlockedSectors.Contains(SelectedSector))
        {
          SectorName.text = "";
          SectorSelectDescription.text = GameManager.Instance.GetTextData("Quest0_Sabbat_Blocked");
          RestButton_Gold.interactable = false;
          RestButton_Sanity.interactable = false;
        }
        else
        {
          SectorName.text = GameManager.Instance.GetTextData(SelectedSector, 0);
          string _effect = GameManager.Instance.GetTextData(SelectedSector, 3);
          switch (SelectedSector)
          {
            case SectorType.Residence:
              _effect = string.Format(_effect, ConstValues.SectorEffect_residence);
              break;
            case SectorType.Temple:
              _effect = string.Format(_effect, ConstValues.SectorEffect_temple);
              break;
            case SectorType.Marketplace:
              _effect = string.Format(_effect, ConstValues.SectorEffect_marketSector);
              break;
            case SectorType.Library:
              _effect = string.Format(_effect, ConstValues.SectorEffect_Library);
              break;
            case SectorType.Theater:
              //���� �����...!
              break;
            case SectorType.Academy:
          //    _effect = string.Format(_effect, ConstValues.SectorDuration, ConstValues.SectorEffect_acardemy);
              break;
          }
          string _cultprogress = "";
          if (GameManager.Instance.MyGameData.Quest_Cult_Phase > 0 && GameManager.Instance.MyGameData.Quest_Cult_Type == 0)
          {
            if (GameManager.Instance.MyGameData.Quest_Cult_Sabbat_TokenedSectors[SelectedSector] == 0)
            {
              _cultprogress = string.Format(GameManager.Instance.GetTextData("Quest0_Sabbat_TokenedPlaceDescription"), ConstValues.Quest_Cult_Sabbat_Progress_TokenSector);
            }
            else
            {
              _cultprogress = string.Format(GameManager.Instance.GetTextData("Quest0_Sabbat_NoTokenedPlaceDescription"), ConstValues.Quest_Cult_Sabbat_Progress_NoTokenSector);
            }
          }
          if (_cultprogress != "") SectorSelectDescription.text = _effect + "<br>" + _cultprogress;
          else SectorSelectDescription.text = _effect;


          RestButton_Sanity.interactable = true;

          int _goldpayvalue = GameManager.Instance.MyGameData.SettleRestCost_Gold;

          if (GameManager.Instance.MyGameData.Gold >= _goldpayvalue) RestButton_Gold.interactable = true;
          else RestButton_Gold.interactable = false;
        }
        break;
    }

    if (RestbuttonHolder.activeInHierarchy == false) RestbuttonHolder.SetActive(true);
    LayoutRebuilder.ForceRebuildLayoutImmediate(RestbuttonHolder.transform as RectTransform);
    LayoutRebuilder.ForceRebuildLayoutImmediate(SectorName.transform.parent.transform as RectTransform);
  }
  public void OnPointerRestType(StatusType type)
  {
    if (UIManager.Instance.IsWorking) return;

    int _movepointvalue = 0;
    int _discomfortvalue = 0;
    switch (CurrentSettlement.SettlementType)
    {
      case SettlementType.Village:
        _movepointvalue = ConstValues.RestMovePoint_Village;
        _discomfortvalue = ConstValues.RestDiscomfort_Village;
        break;
      case SettlementType.Town:
        _movepointvalue = ConstValues.RestMovePoint_Town;
        _discomfortvalue = ConstValues.RestDiscomfort_Town;
        break;
      case SettlementType.City:
        _movepointvalue = ConstValues.RestMovePoint_City;
        _discomfortvalue = ConstValues.RestDiscomfort_City;
        break;
    }
    if (SelectedSector == SectorType.Residence) _movepointvalue++;

    switch (type)
    {
      case StatusType.Sanity:

        RestDescription.text= string.Format(GameManager.Instance.GetTextData("Restbutton_Sanity"),
      WNCText.GetSanityColor("-" + GameManager.Instance.MyGameData.SettleRestCost_Sanity),
      WNCText.GetMovepointColor("+" + _movepointvalue),
      WNCText.GetDiscomfortColor("+" + _discomfortvalue));
        break;
      case StatusType.Gold:
        int _goldpayvalue = GameManager.Instance.MyGameData.SettleRestCost_Gold;
        if (GameManager.Instance.MyGameData.Gold < _goldpayvalue) return;

        RestDescription.text = string.Format(GameManager.Instance.GetTextData("Restbutton_Gold"),
  WNCText.GetGoldColor("-" + _goldpayvalue),
  WNCText.GetMovepointColor("+" + _movepointvalue),
  WNCText.GetDiscomfortColor("+" + _discomfortvalue));
        break;

    }
  }
  public void OnExitRestType(StatusType type)
  {
    if (UIManager.Instance.IsWorking) return;

    switch (type)
    {
      case StatusType.Sanity:
        RestDescription.text = "";
        break;
      case StatusType.Gold:
        int _goldpayvalue = GameManager.Instance.MyGameData.SettleRestCost_Gold;
        if (GameManager.Instance.MyGameData.Gold < _goldpayvalue) return;
        RestDescription.text = "";
        break;
    }
  }
  public void StartRest_Sanity()
    {
    if (UIManager.Instance.IsWorking) return;
    CloseUI();
    GameManager.Instance.RestInSector(SelectedSector, true);
  }
  public void StartRest_Gold()
  {
    if (UIManager.Instance.IsWorking) return;
    CloseUI();
    GameManager.Instance.RestInSector(SelectedSector, false);
  }
}
