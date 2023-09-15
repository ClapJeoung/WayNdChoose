using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics.Tracing;
using Google.Apis.Json;

public class UI_Settlement : UI_default
{
  private float UIOpenMoveTime = 0.6f;
  private WaitForSeconds LittleWait = new WaitForSeconds(0.2f);
  private float UICloseMoveTime = 0.5f;

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
  [SerializeField] private Button RestButton_Sanity = null;
  [SerializeField] private TextMeshProUGUI RestButtonText_Sanity = null;
  [SerializeField] private Button RestButton_Gold = null;
  [SerializeField] private TextMeshProUGUI RestButtonText_Gold = null;
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

    SelectedSector = SectorType.NULL;
    CurrentSettlement = GameManager.Instance.MyGameData.CurrentSettlement;
    SettlementNameText.text = CurrentSettlement.Name;
    DiscomfortText.text = CurrentSettlement.Discomfort.ToString();

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
    SectorName.text = "";
    SectorSelectDescription.text = "";
    SectorName.gameObject.SetActive(false);
    SectorSelectDescription.gameObject.SetActive(false);
    RestButton_Gold.interactable = false;
    RestButtonText_Gold.text = GameManager.Instance.GetTextData("SELECTPLACE");
    RestButton_Sanity.interactable = false;
    RestButtonText_Sanity.text = GameManager.Instance.GetTextData("SELECTPLACE");
    LayoutRebuilder.ForceRebuildLayoutImmediate(SectorName.transform.parent.transform as RectTransform);

    string _rectname = "nameholder";
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
    _rectname = "nameholder";
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
    _rectname = "nameholder";
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect(_rectname).Rect, GetPanelRect(_rectname).Rect.anchoredPosition, GetPanelRect(_rectname).OutisdePos, UICloseMoveTime, UIManager.Instance.UIPanelCLoseCurve));
    _rectname = "placepanel";
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect(_rectname).Rect, GetPanelRect(_rectname).Rect.anchoredPosition, GetPanelRect(_rectname).OutisdePos, UICloseMoveTime, UIManager.Instance.UIPanelCLoseCurve));
  }
  public void SelectPlace(int index)
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
          SectorSelectDescription.text = GameManager.Instance.GetTextData("Quest_Wolf_Cult_Blocked");
          RestButtonText_Gold.text = GameManager.Instance.GetTextData("REST");
          RestButtonText_Sanity.text= GameManager.Instance.GetTextData("REST");
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
              //서비스 종료다...!
              break;
            case SectorType.Academy:
              _effect = string.Format(_effect, ConstValues.SectorDuration, ConstValues.SectorEffect_acardemy);
              break;
          }
          string _progress = "";
          if (GameManager.Instance.MyGameData.Quest_Cult_Sabbat_TokenedSectors[SelectedSector] == 0)
          {
            _progress = string.Format(GameManager.Instance.GetTextData("Quest_Wolf_Cult_TokenedPlaceDescription"), ConstValues.Quest_Cult_Sabbat_Progress_TokenSector);
          }
          else
          {
            _progress = string.Format(GameManager.Instance.GetTextData("Quest_Wolf_Cult_NoTokenedPlaceDescription"), ConstValues.Quest_Cult_Sabbat_Progress_NoTokenSector);
          }
          SectorSelectDescription.text = _effect + "<br><br>" + _progress;

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

          RestButtonText_Sanity.text = string.Format(GameManager.Instance.GetTextData("REST"),
            GameManager.Instance.GetTextData(StatusType.Sanity, 2),
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
        break;
    }

    LayoutRebuilder.ForceRebuildLayoutImmediate(SectorName.transform.parent.transform as RectTransform);
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
