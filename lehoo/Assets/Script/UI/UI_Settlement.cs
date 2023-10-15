using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

public class UI_Settlement : UI_default
{
  private WaitForSeconds LittleWait = new WaitForSeconds(0.2f);
  public float OpenTime_Fold = 0.4f;
  public float OpenTime_Move = 0.8f;
  public float CloseTime_Fold = 0.4f;
  public float CloseTime_Move = 0.6f;
  [SerializeField] private AnimationCurve OpenFoldCurve = new AnimationCurve();
  [SerializeField] private AnimationCurve CloseFoldCurve = new AnimationCurve();
  [SerializeField] private RectTransform NameRect = null;
  private Vector2 NameHolderPos_Left = new Vector2(-1250.0f, 330.0f);
  private Vector2 NameHolderPos_Center = new Vector2(-110, 330.0f);
  private Vector2 NameHolderPos_Right = new Vector2(1450.0f, 330.0f);
  [SerializeField] private RectTransform PanelRect = null;
  private Vector2 OpenSize = new Vector2(740.0f, 450.0f);
  private Vector2 CloseSize = new Vector2(60.0f, 450.0f);
  public RectTransform PanelLastHolder = null;
  private Vector2 Left_Anchor = new Vector2(0.0f, 0.5f);
  private Vector2 Left_Pivot = new Vector2(0.0f, 0.5f);
  private Vector2 Left_PanelOutsidePos = new Vector2(-1000.0f, -0.0f);
  private Vector2 Left_PanelInsidePos = new Vector2(-480, 0.0f);
  private Vector2 Left_LastHolderPos = new Vector2(370.0f, 0.0f);
  private Vector2 Right_Anchor = new Vector2(1.0f, 0.5f);
  private Vector2 Right_Pivot = new Vector2(1.0f, 0.5f);
  private Vector2 Right_PanelInsidePos = new Vector2(260, 0.0f);
  private Vector2 Right_PanelOutsidePos = new Vector2(1150.0f, -0.0f);
  private Vector2 Right_LastHolderPos = new Vector2(-370.0f, 0.0f);

  [SerializeField] private UnityEngine.UI.Image SettlementIcon = null;
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
  [SerializeField] private TextMeshProUGUI SectorEffect = null;
  [SerializeField] private TextMeshProUGUI RestResult = null;
  [SerializeField] private RectTransform RestButtonHolder = null;
  [SerializeField] private Vector2 RestButtonRect_Hide = new Vector2(140.0f, 0.0f);
  [SerializeField] private Vector2 RestButtonRect_Open = new Vector2(370.0f, 0.0f);
  [SerializeField] private float RestHolderOpenTime = 0.8f;
  [SerializeField] private float RestButtonCloseTime = 0.5f;
  [SerializeField] private TextMeshProUGUI CostText = null;
  [SerializeField] private Onpointer_highlight CostHighlight_Sanity = null;
  [SerializeField] private UnityEngine.UI.Button Cost_Sanity = null;
  [SerializeField] private Onpointer_highlight CostHighlight_Gold = null;
  [SerializeField] private UnityEngine.UI.Button Cost_Gold = null;
  private Settlement CurrentSettlement = null;
  private SectorTypeEnum SelectedSector = SectorTypeEnum.NULL;
  /// <summary>
  /// true:좌측에서 등장 false:우측에서 등장
  /// </summary>
  /// <param name="type"></param>
  public void OpenUI(bool dir)
  {
    IsOpen = true;
    UIManager.Instance.AddUIQueue(openui(dir));
  }
  private IEnumerator openui(bool dir)
  {
    DefaultGroup.blocksRaycasts = false;

    IsSelectSector = false;
    QuestSectorInfo = 0;
    SelectedSector = SectorTypeEnum.NULL;
    CurrentSettlement = GameManager.Instance.MyGameData.CurrentSettlement;
    SettlementNameText.text = CurrentSettlement.Name;
    DiscomfortText.text = CurrentSettlement.Discomfort.ToString();
    RestCostValueText.text = string.Format(GameManager.Instance.GetTextData("RestCostValue"),
     (int)GameManager.Instance.MyGameData.GetDiscomfortValue(CurrentSettlement.Discomfort)*100);
    if (RestButtonHolder.gameObject.activeInHierarchy == true) RestButtonHolder.gameObject.SetActive(false);
    RestButtonHolder.anchoredPosition = RestButtonRect_Hide;

    Sprite _settlementicon = null;
    int _placecount = 0;
    switch (CurrentSettlement.SettlementType)
    {
      case SettlementType.Village: _placecount = 2;_settlementicon = GameManager.Instance.ImageHolder.VillageIcon_white; break;
      case SettlementType.Town:_placecount = 3; _settlementicon = GameManager.Instance.ImageHolder.TownIcon_white; break;
      case SettlementType.City:_placecount = 4; _settlementicon = GameManager.Instance.ImageHolder.CityIcon_white; break;
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
    SectorName.text = GameManager.Instance.GetTextData("SELECTPLACE");
    SectorEffect.text = "";

    RestResult.text = "";
    CostText.text = "";

    Vector2 _startpos_panel = Vector2.zero,_endpos_panel= Vector2.zero,_startpos_name= Vector2.zero,_endpos_name= Vector2.zero;
    if (dir == true)
    {
      PanelRect.pivot = Left_Pivot;
      PanelLastHolder.anchorMin = Left_Anchor;
      PanelLastHolder.anchorMax = Left_Anchor;
      PanelLastHolder.anchoredPosition = Left_LastHolderPos;
      _startpos_panel = Left_PanelOutsidePos;
      _endpos_panel = Left_PanelInsidePos;
      _startpos_name = NameHolderPos_Left;
      _endpos_name = NameHolderPos_Center;

    }
    else
    {
      PanelRect.pivot = Right_Pivot;

      PanelLastHolder.anchorMin = Right_Anchor;
      PanelLastHolder.anchorMax = Right_Anchor;
      PanelLastHolder.anchoredPosition = Right_LastHolderPos;

      _startpos_panel = Right_PanelOutsidePos;
      _endpos_panel = Right_PanelInsidePos;
      _startpos_name = NameHolderPos_Right;
      _endpos_name = NameHolderPos_Center;
    }

    PanelRect.sizeDelta = CloseSize;

    float _time = 0.0f;
    StartCoroutine(UIManager.Instance.moverect(NameRect, _startpos_name, _endpos_name,OpenTime_Move, UIManager.Instance.UIPanelOpenCurve));
    yield return LittleWait;
    yield return StartCoroutine(UIManager.Instance.moverect(PanelRect,_startpos_panel,_endpos_panel,OpenTime_Move,UIManager.Instance.UIPanelOpenCurve));
    while (_time < OpenTime_Fold)
    {
      PanelRect.sizeDelta = Vector2.Lerp(CloseSize, OpenSize, OpenFoldCurve.Evaluate(_time / OpenTime_Fold));
      _time += Time.deltaTime;
      yield return null;
    }
    PanelRect.sizeDelta = OpenSize;

    UIManager.Instance.MapButton.Open(0, this);

    DefaultGroup.blocksRaycasts = true;
  }
  /// <summary>
  /// true:좌측으로 이동 false:우측으로 이동
  /// </summary>
  /// <param name="dir"></param>
  public void CloseUI(bool dir)
  {
    IsOpen = false;
    UIManager.Instance.AddUIQueue(closeui(dir));
  }
  private IEnumerator closeui(bool dir)
  {
    DefaultGroup.blocksRaycasts = false;
    if (UIManager.Instance.MapButton.IsOpen) UIManager.Instance.MapButton.Close();

    if (RestButtonHolder.gameObject.activeInHierarchy == true)
    {
      yield return StartCoroutine(UIManager.Instance.moverect(RestButtonHolder, RestButtonRect_Open, RestButtonRect_Hide, RestButtonCloseTime, UIManager.Instance.UIPanelCLoseCurve));
      RestButtonHolder.gameObject.SetActive(false);
      yield return new WaitForSeconds(0.5f);
    }

    PanelRect.sizeDelta = OpenSize;
    Vector2 _startpos_panel = Vector2.zero, _endpos_panel = Vector2.zero, _startpos_name = Vector2.zero, _endpos_name = Vector2.zero;
    if (dir == true)
    {
      PanelRect.pivot = Left_Pivot;
      PanelLastHolder.anchorMin = Left_Anchor;
      PanelLastHolder.anchorMax = Left_Anchor;
      PanelLastHolder.anchoredPosition = Left_LastHolderPos;

      _startpos_panel = Left_PanelInsidePos;
      _endpos_panel = Left_PanelOutsidePos;
      _startpos_name = NameHolderPos_Center;
      _endpos_name = NameHolderPos_Left;
    }
    else
    {
      PanelRect.pivot = Right_Pivot;

      PanelLastHolder.anchorMin = Right_Anchor;
      PanelLastHolder.anchorMax = Right_Anchor;
      PanelLastHolder.anchoredPosition = Right_LastHolderPos;

      _startpos_panel = Right_PanelInsidePos;
      _endpos_panel = Right_PanelOutsidePos;
      _startpos_name = NameHolderPos_Center;
      _endpos_name = NameHolderPos_Right;
    }
    PanelRect.anchoredPosition = _startpos_panel;

    float _time = 0.0f;
    while (_time < CloseTime_Fold)
    {
      PanelRect.sizeDelta = Vector2.Lerp(OpenSize, CloseSize, CloseFoldCurve.Evaluate(_time / CloseTime_Fold));
      _time += Time.deltaTime;
      yield return null;
    }
    yield return LittleWait;
    PanelRect.sizeDelta = CloseSize;
    StartCoroutine(UIManager.Instance.moverect(NameRect, _startpos_name, _endpos_name, CloseTime_Move, UIManager.Instance.UIPanelCLoseCurve));
    yield return LittleWait;
    yield return StartCoroutine(UIManager.Instance.moverect(PanelRect, _startpos_panel, _endpos_panel, CloseTime_Move, UIManager.Instance.UIPanelCLoseCurve));

  }
  /// <summary>
  /// 0:일반 1:집회 2:페널티만
  /// </summary>
  public int QuestSectorInfo = 0;
  public int GoldCost = 0;
  public int SanityCost = 0;
  public int DiscomfortValue = 0;
  public int MovePointValue = 0;
  public bool IsSelectSector = false;
  
  public void OnPointerSector(SectorTypeEnum sector)
  {
    if (IsSelectSector == true) return;

    QuestSectorInfo = GameManager.Instance.MyGameData.Cult_IsSabbat(sector);

    SectorName.text = GameManager.Instance.GetTextData(sector, 0);
    string _effect = GameManager.Instance.GetTextData(sector, 3);
    int _discomfort_default = (GameManager.Instance.MyGameData.FirstRest && GameManager.Instance.MyGameData.Tendency_Head.Level > 0) == true ?
      ConstValues.Rest_Discomfort :
      ConstValues.Tendency_Head_p1;
    switch (sector)
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

        GoldCost = Mathf.FloorToInt(GameManager.Instance.MyGameData.RestCost_Gold * (1.0f - ConstValues.SectorEffect_marketSector / 100.0f));
        SanityCost = Mathf.FloorToInt(GameManager.Instance.MyGameData.RestCost_Sanity * (1.0f - ConstValues.SectorEffect_marketSector / 100.0f));
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
        MovePointValue = ConstValues.Rest_MovePoint;
        //    _effect = string.Format(_effect, ConstValues.SectorDuration, ConstValues.SectorEffect_acardemy);
        break;
    }

    string _sabbatdescription = "";
    switch (QuestSectorInfo)
    {
      case 0:
        SectorEffect.text = _effect;
        break;
      case 1:
        DiscomfortValue += ConstValues.Quest_Cult_SabbatDiscomfort;
        _sabbatdescription = "<br>" + string.Format(GameManager.Instance.GetTextData("Quest0_Progress_Sabbat_Effect"),
ConstValues.Quest_Cult_SabbatDiscomfort, ConstValues.Quest_Cult_Progress_Sabbat);
        SectorEffect.text = _effect + _sabbatdescription;
        break;
      case 2:
        SectorEffect.text = _effect;
        break;
    }
    RestResult.text = string.Format(GameManager.Instance.GetTextData("RestResult"), MovePointValue, DiscomfortValue);

  }
  public void OutPointerSector()
  {
    if (IsSelectSector == true) return;

    SectorName.text = "";
    SectorEffect.text = "";
    RestResult.text = "";
  }
  public void SelectPlace(int index)  //Sectortype은 0이 NULL임
  {
    if (SelectedSector == (SectorTypeEnum)index) return;

   if(SelectedSector!=SectorTypeEnum.NULL) GetSectorIconScript(SelectedSector).SetIdleColor();
    SelectedSector = (SectorTypeEnum)index;
    IsSelectSector = true;

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

        GoldCost = Mathf.FloorToInt(GameManager.Instance.MyGameData.RestCost_Gold * (1.0f - ConstValues.SectorEffect_marketSector / 100.0f));
        SanityCost = Mathf.FloorToInt(GameManager.Instance.MyGameData.RestCost_Sanity * (1.0f - ConstValues.SectorEffect_marketSector / 100.0f));
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
        MovePointValue = ConstValues.Rest_MovePoint;
        //    _effect = string.Format(_effect, ConstValues.SectorDuration, ConstValues.SectorEffect_acardemy);
        break;
    }

    string _sabbatdescription = "";
    switch (QuestSectorInfo)
    {
      case 0:
        SectorEffect.text = _effect;
        break;
      case 1:
        DiscomfortValue += ConstValues.Quest_Cult_SabbatDiscomfort;
        _sabbatdescription = "<br>" + string.Format(GameManager.Instance.GetTextData("Quest0_Progress_Sabbat_Effect"),
ConstValues.Quest_Cult_SabbatDiscomfort, ConstValues.Quest_Cult_Progress_Sabbat);
        SectorEffect.text = _effect + _sabbatdescription;
        break;
      case 2:
        SectorEffect.text = _effect;
        break;
    }
    RestResult.text = string.Format(GameManager.Instance.GetTextData("RestResult"), MovePointValue, DiscomfortValue);

    CostText.text = "";
    if (RestButtonHolder.gameObject.activeInHierarchy == false)
    {
      RestButtonHolder.gameObject.SetActive(true);
      StartCoroutine(UIManager.Instance.moverect(RestButtonHolder, RestButtonRect_Hide, RestButtonRect_Open, RestHolderOpenTime, UIManager.Instance.UIPanelOpenCurve));
    }

    CostHighlight_Sanity.Interactive = true;
    Cost_Gold.interactable = GameManager.Instance.MyGameData.Gold >= GoldCost;
    CostHighlight_Gold.Interactive = GameManager.Instance.MyGameData.Gold >= GoldCost;

  }
  public void OnPointerRestType(StatusTypeEnum type)
  {
    if (UIManager.Instance.IsWorking) return;

    switch (type)
    {
      case StatusTypeEnum.Sanity:

        CostText.text = string.Format(GameManager.Instance.GetTextData("Restbutton_Sanity"), SanityCost);
        break;
      case StatusTypeEnum.Gold:

        CostText.text = string.Format(GameManager.Instance.GetTextData("Restbutton_Gold"), GoldCost);
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

    CostHighlight_Sanity.Interactive = false;
    CostHighlight_Gold.Interactive = false;

    StartCoroutine(restclose_sanity());
  }
  private IEnumerator restclose_sanity()
  {
    IsOpen = false;
    yield return StartCoroutine(closeui(true));
    GameManager.Instance.RestInSector(SelectedSector, StatusTypeEnum.Sanity, SanityCost, DiscomfortValue, MovePointValue, QuestSectorInfo);
  }
  public void StartRest_Gold()
  {
    if (UIManager.Instance.IsWorking) return;

    CostHighlight_Sanity.Interactive = false;
    CostHighlight_Gold.Interactive = false;

    StartCoroutine(restclose_gold());
  }
  private IEnumerator restclose_gold()
  {
    IsOpen = false;
    yield return StartCoroutine(closeui(true));
    GameManager.Instance.RestInSector(SelectedSector, StatusTypeEnum.Gold, GoldCost, DiscomfortValue, MovePointValue, QuestSectorInfo);
  }

}
