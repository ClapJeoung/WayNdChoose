using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Settlement : UI_default
{
  private float UIOpenMoveTime = 0.8f;
  private WaitForSeconds LittleWait = new WaitForSeconds(0.2f);
  private float UICloseMoveTime = 0.6f;

  [SerializeField] private Image SettlementIcon = null;
  [SerializeField] private TextMeshProUGUI SettlementNameText = null;
  [SerializeField] private TextMeshProUGUI DiscomfortText = null;
  [SerializeField] private TextMeshProUGUI RestCostValueText = null;
  [SerializeField] private List<PlaceIconScript> SectorIcons=new List<PlaceIconScript>();
  private PlaceIconScript GetSectorIconScript(SectorTypeEnum sectortype)
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
  private SectorTypeEnum SelectedSector = SectorTypeEnum.NULL;
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
    QuestSectorInfo = 0;
    SelectedSector = SectorTypeEnum.NULL;
    CurrentSettlement = GameManager.Instance.MyGameData.CurrentSettlement;
    SettlementNameText.text = CurrentSettlement.Name;
    DiscomfortText.text = CurrentSettlement.Discomfort.ToString();
    RestCostValueText.text = string.Format(GameManager.Instance.GetTextData("RestCostValue"),
      (int)((ConstValues.MoveRest_Value_Deafult + ConstValues.MoveRest_Value_Ratio * CurrentSettlement.Discomfort)*100.0f));
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
    SectorName.text = GameManager.Instance.GetTextData("SELECTPLACE");
    if (SectorSelectDescription.gameObject.activeInHierarchy == false) SectorSelectDescription.gameObject.SetActive(true);
    SectorSelectDescription.text = "";
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
   yield return StartCoroutine(UIManager.Instance.moverect(GetPanelRect(_rectname).Rect, GetPanelRect(_rectname).InsidePos, GetPanelRect(_rectname).OutisdePos, UICloseMoveTime, false));
  }
  /// <summary>
  /// 0:일반 1:집회 2:페널티만
  /// </summary>
  public int QuestSectorInfo = 0;
  public int GoldCost = 0;
  public int SanityCost = 0;
  public int DiscomfortValue = 0;
  public int MovePointValue = 0;
  public void SelectPlace(int index)  //Sectortype은 0이 NULL임
  {
    if (SelectedSector == (SectorTypeEnum)index) return;

    if(SelectedSector!=SectorTypeEnum.NULL) GetSectorIconScript(SelectedSector).SetIdleColor();
    SelectedSector = (SectorTypeEnum)index;

    if(SectorName.gameObject.activeInHierarchy==false) SectorName.gameObject.SetActive(true);
    if(SectorSelectDescription.gameObject.activeInHierarchy==false) SectorSelectDescription.gameObject.SetActive(true);

    switch (GameManager.Instance.MyGameData.QuestType)
    {
      case QuestType.Cult:
        GetSectorIconScript(SelectedSector).SetSelectColor();

        QuestSectorInfo = GameManager.Instance.MyGameData.Cult_IsSabbat(SelectedSector);

        SectorName.text = GameManager.Instance.GetTextData(SelectedSector, 0);
        string _effect = GameManager.Instance.GetTextData(SelectedSector, 3);
        int _discomfort_default = (GameManager.Instance.MyGameData.FirstRest && GameManager.Instance.MyGameData.Tendency_Head.Level > 0) == true ?
          ConstValues.Rest_Discomfort :
          ConstValues.Tendency_Head_p1;
        switch (SelectedSector)
        {
          case SectorTypeEnum.Residence:
            _effect = string.Format(_effect, 
              WNCText.GetMovepointColor(ConstValues.SectorEffect_residence_movepoint),
              WNCText.GetDiscomfortColor(ConstValues.SectorEffect_residence_discomfort));

            GoldCost = GameManager.Instance.MyGameData.RestCost_Gold;
            SanityCost = GameManager.Instance.MyGameData.RestCost_Sanity;
            DiscomfortValue = _discomfort_default + ConstValues.SectorEffect_residence_discomfort;
            MovePointValue = ConstValues.Rest_MovePoint + ConstValues.SectorEffect_residence_movepoint;
            break;
          case SectorTypeEnum.Temple:
            _effect = string.Format(_effect, ConstValues.SectorEffect_temple);

            GoldCost = GameManager.Instance.MyGameData.RestCost_Gold;
            SanityCost = GameManager.Instance.MyGameData.RestCost_Sanity;
            DiscomfortValue = _discomfort_default;
            MovePointValue = ConstValues.Rest_MovePoint;
            break;
          case SectorTypeEnum.Marketplace:
            _effect = string.Format(_effect, ConstValues.SectorEffect_marketSector);

            GoldCost =Mathf.FloorToInt( GameManager.Instance.MyGameData.RestCost_Gold * (1.0f-ConstValues.SectorEffect_marketSector / 100.0f));
            SanityCost =Mathf.FloorToInt(GameManager.Instance.MyGameData.RestCost_Sanity * (1.0f-ConstValues.SectorEffect_marketSector / 100.0f));
            DiscomfortValue = _discomfort_default;
            MovePointValue = ConstValues.Rest_MovePoint;
            break;
          case SectorTypeEnum.Library:
            _effect = string.Format(_effect, ConstValues.SectorEffect_Library);

            GoldCost = GameManager.Instance.MyGameData.RestCost_Gold;
            SanityCost = GameManager.Instance.MyGameData.RestCost_Sanity;
            DiscomfortValue = _discomfort_default;
            MovePointValue = ConstValues.Rest_MovePoint;
            break;
          case SectorTypeEnum.Theater:
            //서비스 종료다...!
            break;
          case SectorTypeEnum.Academy:
            GoldCost = GameManager.Instance.MyGameData.RestCost_Gold;
            SanityCost = GameManager.Instance.MyGameData.RestCost_Sanity;
            DiscomfortValue = ConstValues.Rest_Discomfort;
            MovePointValue=ConstValues.Rest_MovePoint;
            //    _effect = string.Format(_effect, ConstValues.SectorDuration, ConstValues.SectorEffect_acardemy);
            break;
        }

        string _sabbatdescription = "";
        switch (QuestSectorInfo)
        {
          case 0:
            SectorSelectDescription.text = _effect;
            break;
          case 1:
            DiscomfortValue += ConstValues.Quest_Cult_SabbatDiscomfort;
            _sabbatdescription = "<br><br>" + string.Format(GameManager.Instance.GetTextData("Quest0_Progress_Sabbat_Effect"),
WNCText.GetDiscomfortColor(ConstValues.Quest_Cult_SabbatDiscomfort)) + "<br>" + string.Format(GameManager.Instance.GetTextData("Quest0_Progress_Sabbat"), ConstValues.Quest_Cult_Progress_Sabbat);
            SectorSelectDescription.text = _effect + _sabbatdescription;
            break;
          case 2:
            break;
        }

        RestButton_Sanity.interactable = true;
        RestButton_Gold.interactable = GameManager.Instance.MyGameData.Gold >= GoldCost?true:false;
        break;
    }

    if (RestbuttonHolder.activeInHierarchy == false) RestbuttonHolder.SetActive(true);
    RestDescription.text = "";
    LayoutRebuilder.ForceRebuildLayoutImmediate(RestbuttonHolder.transform as RectTransform);
    LayoutRebuilder.ForceRebuildLayoutImmediate(SectorName.transform.parent.transform as RectTransform);
  }
  public void OnPointerRestType(StatusTypeEnum type)
  {
    if (UIManager.Instance.IsWorking) return;

    switch (type)
    {
      case StatusTypeEnum.Sanity:

        RestDescription.text= string.Format(GameManager.Instance.GetTextData("Restbutton_Sanity"),
      WNCText.GetSanityColor("-"+ SanityCost),
      WNCText.GetMovepointColor("+" + MovePointValue),
      WNCText.GetDiscomfortColor("+" + DiscomfortValue));
        break;
      case StatusTypeEnum.Gold:

        RestDescription.text = string.Format(GameManager.Instance.GetTextData("Restbutton_Gold"),
  WNCText.GetGoldColor("-" + GoldCost),
  WNCText.GetMovepointColor("+" + MovePointValue),
  WNCText.GetDiscomfortColor("+" + DiscomfortValue));
        break;

    }
  }
  public void OnExitRestType(StatusTypeEnum type)
  {
    return;
  }
  public void StartRest_Sanity()
    {
    if (UIManager.Instance.IsWorking) return;
    CloseUI();
    GameManager.Instance.RestInSector(SelectedSector, StatusTypeEnum.Sanity,SanityCost,DiscomfortValue,MovePointValue, QuestSectorInfo);
  }
  public void StartRest_Gold()
  {
    if (UIManager.Instance.IsWorking) return;
    CloseUI();
    GameManager.Instance.RestInSector(SelectedSector, StatusTypeEnum.Gold,GoldCost, DiscomfortValue, MovePointValue, QuestSectorInfo);
  }
}
