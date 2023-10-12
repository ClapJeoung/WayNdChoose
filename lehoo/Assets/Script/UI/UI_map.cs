using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;
using System.Linq;

public class UI_map : UI_default
{
  [SerializeField] private RectTransform PlayerRect = null;
  [SerializeField] private float MoveTime = 1.5f;
  private TileData SelectedTile = null;
  public maptext MapCreater = null;
  [HideInInspector] public GameObject CityIcon = null;
  [HideInInspector] public GameObject TownIcon = null;
  [HideInInspector] public List<GameObject> VillageIcons = new List<GameObject>();
  public GameObject GetSettleIcon(Settlement settlement)
  {
    string _originname = settlement.OriginName;
    switch (settlement.SettlementType)
    {
      case SettlementType.City:return CityIcon;
      case SettlementType.Town:
        return TownIcon;
      case SettlementType.Village:
        foreach(GameObject village in VillageIcons)
          if(village.name.Contains(_originname)) return village;
        return null;
    }
    Debug.Log("뭔가 이상한 레후~");
    return null;
  }
  public float AppearTime = 1.2f;
  [SerializeField] private AnimationCurve ZoomInCurve = null;
  [SerializeField] private RectTransform HolderRect = null;
  [SerializeField] private RectTransform ScaleRect = null;
  private Vector3 IdleScale = Vector3.one;
  [SerializeField] private Vector3 ZoomInScale = Vector3.one* 1.5f;
  [SerializeField] private float ZoomInTime = 1.2f;

  [SerializeField] private CanvasGroup MoveInfoGroup = null;
  [SerializeField] private TextMeshProUGUI ProgressGuideText = null;
  [SerializeField] private Image TilePreview_Bottom = null;
  [SerializeField] private Image TilePreview_Top = null;
  [SerializeField] private Image TilePreview_Landmark = null;
  [SerializeField] private GameObject SettlementInfoHolder = null;
  [SerializeField] private TextMeshProUGUI SettlementNameText = null;
  [SerializeField] private TextMeshProUGUI DiscomfortText = null;
  [SerializeField] private TextMeshProUGUI MoveProgressInfoText = null;
  [SerializeField] private CanvasGroup MovecostButtonGroup = null;
  [SerializeField] private CanvasGroup SanitybuttonGroup = null;
  [SerializeField] private CanvasGroup GoldbuttonGroup = null;
  private float MoveButtonDisableAlpha = 0.2f;
  public StatusTypeEnum SelectedCostType = StatusTypeEnum.HP;
  [SerializeField] private TextMeshProUGUI MoveDescriptionText = null;
  private bool IsRitual = false;

  public Transform SelectTileHolder = null;

  public Color NormalColor = Color.white;
  public Color DisableColor = Color.grey;
  private List<TileData> ActiveTileData = new List<TileData>();

  /// <summary>
  /// 주위 2칸 타일 업데이트
  /// </summary>
  private void ResetEnableTiles()
  {

    foreach(var tile in ActiveTileData)
    {
      tile.ButtonScript.Button.interactable = false;
    }
    ActiveTileData.Clear();

    List<TileData> _currents = GameManager.Instance.MyGameData.MyMapData.GetAroundTile(GameManager.Instance.MyGameData.Coordinate, 3);
    foreach (TileData _tile in _currents) //새로운 주위 2칸 타일 전부 가져오기
    {
      _tile.ButtonScript.Button.interactable = true;
      ActiveTileData.Add(_tile);
    }

    switch (GameManager.Instance.MyGameData.QuestType)
    {
      case QuestType.Cult:
        switch (GameManager.Instance.MyGameData.Quest_Cult_Phase)
        {
          case 0:
            break;
          case 1:
            SetRitualTile();
            break;
          case 2:
            if (GameManager.Instance.MyGameData.Cult_RitualTile_CoolDown == 0) SetRitualTile();
            break;
        }
        break;
    }
    void SetRitualTile()
    {
      if (GameManager.Instance.MyGameData.Cult_RitualTile != null)
      {
        GameManager.Instance.MyGameData.Cult_RitualTile.Landmark = LandmarkType.Outer;
        GameManager.Instance.MyGameData.Cult_RitualTile.ButtonScript.LandmarkImage.sprite = GameManager.Instance.ImageHolder.Transparent;
        GameManager.Instance.MyGameData.Cult_RitualTile = null;
      }

      int _mincount = 10;
      List<TileData> _availabletiles=new List<TileData>();
      foreach (var _tile in _currents)
      {
        if (_tile.Interactable == false) continue;
        if (_tile.Landmark != LandmarkType.Outer) continue;
        if (_tile.Coordinate == GameManager.Instance.MyGameData.Coordinate) continue;

        int _count = 0;
        foreach (var _aroundtile in GameManager.Instance.MyGameData.MyMapData.GetAroundTile(_tile, 2))
        {
          if (_aroundtile.TileSettle != null) _count++;
        }
        if (_count < _mincount)
        {
          _mincount = _count;
          _availabletiles.Clear();

          _availabletiles.Add(_tile);
        }
        else
        {
          _availabletiles.Add(_tile);
        }
      }

      GameManager.Instance.MyGameData.Cult_RitualTile = _availabletiles[Random.Range(0, _availabletiles.Count)];
      GameManager.Instance.MyGameData.Cult_RitualTile.Landmark = LandmarkType.Ritual;
      GameManager.Instance.MyGameData.Cult_RitualTile.ButtonScript.LandmarkImage.sprite =
        UIManager.Instance.MyMap.MapCreater.MyTiles.GetTile(GameManager.Instance.MyGameData.Cult_RitualTile.landmarkSprite);

    }
  }
  
  public void OpenUI()
  {
    IsOpen = true;
    UIManager.Instance.AddUIQueue(openui());
  }
  
  private IEnumerator openui()
  {
    ResetEnableTiles();

    IsRitual = false;
    MoveInfoGroup.interactable = true;
    MoveProgressInfoText.text = "";
    MoveDescriptionText.text = "";
    if (SettlementInfoHolder.activeInHierarchy == true) SettlementInfoHolder.SetActive(false);
    MovecostButtonGroup.alpha = 0.0f;
    MovecostButtonGroup.interactable = false;
    MovecostButtonGroup.blocksRaycasts = false;

    SelectedCostType = StatusTypeEnum.HP;

    if (DefaultGroup.alpha == 1.0f) DefaultGroup.alpha = 0.0f;
    if (DefaultGroup.interactable == true) DefaultGroup.interactable = false;
    if (DefaultGroup.blocksRaycasts == true) DefaultGroup.blocksRaycasts = false;

    if (GetPanelRect("myrect").Rect.anchoredPosition == GetPanelRect("myrect").OutisdePos)
      GetPanelRect("myrect").Rect.anchoredPosition = GetPanelRect("myrect").InsidePos;

    ScaleRect.localScale = IdleScale;
    ScaleRect.anchoredPosition = Vector2.zero;

    float _time = 0.0f;
    while (_time < AppearTime)
    {
      DefaultGroup.alpha = Mathf.Lerp(0.0f, 1.0f, _time / AppearTime);
      _time += Time.deltaTime;
      yield return null;
    }

    DefaultGroup.alpha = 1.0f;
    DefaultGroup.interactable = true;
    DefaultGroup.blocksRaycasts = true;
  }
  public int SanityCost = 0, GoldCost = 0;
  [HideInInspector] public int QuestInfo = 0;
  public void SelectTile(TileData selectedtiledata,Vector2 position)
  {
    //동일한 좌표면 호출되지 않게 이미 거름

    if (SelectedTile != null) SelectedTile.ButtonScript.CancleTile();
    TileData _currenttile = GameManager.Instance.MyGameData.MyMapData.Tile(GameManager.Instance.MyGameData.Coordinate);
    int _length = GameManager.Instance.MyGameData.MyMapData.GetLength(_currenttile, selectedtiledata);

    SelectedTile = selectedtiledata;

    ProgressGuideText.text = GameManager.Instance.GetTextData("CHOOSECOSTTYPE_MAP");

    TilePreview_Bottom.sprite = SelectedTile.ButtonScript.BottomImage.sprite;
    TilePreview_Bottom.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -60.0f * SelectedTile.Rotation));
    TilePreview_Top.sprite = SelectedTile.ButtonScript.TopImage.sprite;
    TilePreview_Landmark.sprite = SelectedTile.ButtonScript.TopImage.sprite;

    if (SelectedTile.TileSettle != null)
    {
      SettlementInfoHolder.SetActive(true);
      SettlementNameText.text=SelectedTile.TileSettle.Name;
      DiscomfortText.text = SelectedTile.TileSettle.Discomfort.ToString();
    }
    else
    {
      if (SettlementInfoHolder.activeInHierarchy == true) SettlementInfoHolder.SetActive(false);
    }
    switch (GameManager.Instance.MyGameData.QuestType)
    {
      case QuestType.Cult:
        string _progresstext = "";
        QuestInfo = GameManager.Instance.MyGameData.Cult_IsRitual(SelectedTile);
        switch (QuestInfo)
        {
          case 0:
            _progresstext = "";
            break;
          case 1:
            if (SelectedTile.Landmark == LandmarkType.Ritual)
            {
              _progresstext += string.Format(GameManager.Instance.GetTextData("Quest0_Progress_Ritual"), ConstValues.Quest_Cult_Progress_Ritual) +
                "<br>" +
                string.Format(GameManager.Instance.GetTextData("Quest0_Progress_Ritual_Effect"), ConstValues.Quest_Cult_RitualMovepoint);
            }
            break;
          case 2:
            _progresstext = "";
            break;
        }
        MoveProgressInfoText.text = _progresstext;
        break;
    }

    MovecostButtonGroup.alpha = 1.0f;
    MovecostButtonGroup.interactable = true;
    MovecostButtonGroup.blocksRaycasts = true;
    SanityCost =GameManager.Instance.MyGameData.MovePoint>=MovePointCost?
      GameManager.Instance.MyGameData.MoveSanityCost:
      Mathf.FloorToInt(GameManager.Instance.MyGameData.MoveSanityCost*GameManager.Instance.MyGameData.MovePointAmplified);
    GoldCost = GameManager.Instance.MyGameData.MovePoint >= MovePointCost ?
      GameManager.Instance.MyGameData.MoveGoldCost :
      Mathf.FloorToInt(GameManager.Instance.MyGameData.MoveGoldCost * GameManager.Instance.MyGameData.MovePointAmplified);
    SanitybuttonGroup.interactable = true;
  }

  private int MovePointCost = 0;
  public void EnterPointerStatus(StatusTypeEnum type)
  {
    switch (type)
    {
      case StatusTypeEnum.Sanity:
        SelectedCostType = StatusTypeEnum.Sanity;
        SanitybuttonGroup.alpha = 1.0f;
        GoldbuttonGroup.alpha = MoveButtonDisableAlpha;
        switch (GameManager.Instance.MyGameData.QuestType)
        {
          case QuestType.Cult:
            switch (QuestInfo)
            {
              case 0:
                MovePointCost = 1;
                break;
              case 1:
                MovePointCost = 1 + ConstValues.Quest_Cult_RitualMovepoint;
                break;
              case 2:
                MovePointCost = 1;
                break;
            }
            break;
        }

        MoveDescriptionText.text =GameManager.Instance.GetTextData("MAPCOSTTYPE_SANITY")+"<br>"+ (GameManager.Instance.MyGameData.MovePoint > 0 ?
    string.Format(GameManager.Instance.GetTextData("MOVECOST_ENOUGH"), $"<#FFBF00>-{MovePointCost}</color>", GameManager.Instance.GetTextData(StatusTypeEnum.Sanity, 2), WNCText.GetGoldColor("-" + SanityCost)) :
    string.Format(GameManager.Instance.GetTextData("MOVECOST_NOTENOUGH"), GameManager.Instance.GetTextData(StatusTypeEnum.Sanity, 2), WNCText.GetGoldColor("-" + SanityCost)));
        break;
      case StatusTypeEnum.Gold:
        if (GameManager.Instance.MyGameData.Gold < GoldCost) return;

        SelectedCostType = StatusTypeEnum.Gold;
        SanitybuttonGroup.alpha = MoveButtonDisableAlpha;
        GoldbuttonGroup.alpha = 1.0f;

        MoveDescriptionText.text =GameManager.Instance.GetTextData("MAPCOSTTYPE_GOLD")+"<br>"+(GameManager.Instance.MyGameData.MovePoint > 0 ?
    string.Format(GameManager.Instance.GetTextData("MOVECOST_ENOUGH"), $"<#FFBF00>-{MovePointCost}</color>", GameManager.Instance.GetTextData(StatusTypeEnum.Gold, 2), WNCText.GetGoldColor("-" + GoldCost)) :
    string.Format(GameManager.Instance.GetTextData("MOVECOST_NOTENOUGH"), GameManager.Instance.GetTextData(StatusTypeEnum.Gold, 2), WNCText.GetGoldColor("-" + GoldCost)));
        break;
    }
    LayoutRebuilder.ForceRebuildLayoutImmediate(MoveDescriptionText.transform.parent.transform as RectTransform);
  }
  public void ExitPointerStatus(StatusTypeEnum type)
  {
    if (type==StatusTypeEnum.Gold&& GameManager.Instance.MyGameData.Gold < GoldCost) return;

    SanitybuttonGroup.alpha = MoveButtonDisableAlpha;
    GoldbuttonGroup.alpha = MoveButtonDisableAlpha;
    MoveDescriptionText.text = "";
  }

  /// <summary>
  /// 0:정신력 1:골드
  /// </summary>
  /// <param name="index"></param>
  public void MoveMap()
  {
    if (UIManager.Instance.IsWorking) return;
    if (SelectedCostType == StatusTypeEnum.Gold && GameManager.Instance.MyGameData.Gold < GoldCost) return;

    MoveInfoGroup.interactable = false;
    DefaultGroup.blocksRaycasts = true;
    UIManager.Instance.ResetEventPanels();
    UIManager.Instance.AddUIQueue(movemap());
  }
  private IEnumerator movemap()
  {
    GameManager.Instance.MyGameData.Turn++;

    Vector2 _playerrectpos = PlayerRect.anchoredPosition;                 //현재 위치(Rect)

    if (GameManager.Instance.MyGameData.Madness_Wild == true && Random.Range(0, 100) < ConstValues.MadnessEffect_Wild)
    {
      List<TileData> _availabletiles = new List<TileData>();
      foreach(var _tile in GameManager.Instance.MyGameData.MyMapData.GetAroundTile(SelectedTile, 1))
      {
        if (_tile == SelectedTile) continue;
        if(_tile.Interactable == false) continue;
        if (!ActiveTileData.Contains(_tile)) continue;
        _availabletiles.Add(_tile);
      }
      if (_availabletiles.Count == 0)
      {
        _availabletiles = new List<TileData>();
        foreach (var _tile in GameManager.Instance.MyGameData.MyMapData.GetAroundTile(SelectedTile, 2))
        {
          if (_tile == SelectedTile) continue;
          if (_tile.Interactable == false) continue;
          if (!ActiveTileData.Contains(_tile)) continue;
          _availabletiles.Add(_tile);
        }
      }
      SelectedTile = _availabletiles[Random.Range(0,_availabletiles.Count)];
    }
    Vector2 _targettilepos = SelectedTile.Rect.anchoredPosition;            //종점 위치(Rect)

    Vector2 _endcoor = SelectedTile.Coordinate;//종점 위치(타일)

    GameManager.Instance.MyGameData.ClearBeforeEvents();

    switch (SelectedCostType)
    {
      case StatusTypeEnum.Sanity:
        GameManager.Instance.MyGameData.Sanity -= SanityCost;
        break;
      case StatusTypeEnum.Gold:
        GameManager.Instance.MyGameData.Gold -= GoldCost;
        break;
    }

    GameManager.Instance.MyGameData.MovePoint -= MovePointCost;

    float _time = 0.0f;
    while (_time < MoveTime)
    {
      PlayerRect.anchoredPosition = Vector3.Lerp(_playerrectpos, _targettilepos, UIManager.Instance.CharacterMoveCurve.Evaluate(_time / MoveTime));
      HolderRect.anchoredPosition = PlayerRect.anchoredPosition * -1.0f;
      _time += Time.deltaTime;
      yield return null;
    }
    PlayerRect.anchoredPosition = _targettilepos;
    HolderRect.anchoredPosition = PlayerRect.anchoredPosition * -1.0f;

    GameManager.Instance.MyGameData.Coordinate = _endcoor;

    switch (SelectedTile.Landmark)
    {
      case LandmarkType.Outer:
        if (GameManager.Instance.MyGameData.QuestType == QuestType.Cult && GameManager.Instance.MyGameData.Quest_Cult_Phase > 0)
          GameManager.Instance.MyGameData.Quest_Cult_Progress += ConstValues.Quest_Cult_Progress_Ritual;

        GameManager.Instance.MyGameData.CurrentSettlement = null;
        yield return new WaitForSeconds(1.0f);
        EventManager.Instance.SetOutsideEvent(GameManager.Instance.MyGameData.MyMapData.GetTileData(SelectedTile.Coordinate));
        break;

      case LandmarkType.Village:
      case LandmarkType.Town:
      case LandmarkType.City:
        GameManager.Instance.EnterSettlement(SelectedTile.TileSettle);
        break;
      case LandmarkType.Ritual:
        GameManager.Instance.MyGameData.Quest_Cult_Progress += ConstValues.Quest_Cult_Progress_Ritual;

        GameManager.Instance.MyGameData.CurrentSettlement = null;
        yield return new WaitForSeconds(1.0f);
        EventManager.Instance.SetOutsideEvent(GameManager.Instance.MyGameData.MyMapData.GetTileData(SelectedTile.Coordinate));

        if (GameManager.Instance.MyGameData.Quest_Cult_Phase == 2) GameManager.Instance.MyGameData.Cult_RitualTile_CoolDown = ConstValues.Quest_Cult_CoolDown;
        break;
    }

    StartCoroutine(zoominview());
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DefaultGroup, 0.0f, ZoomInTime + 0.6f));
    yield return new WaitForSeconds(0.1f);
    if (IsRitual) UIManager.Instance.QuestUI_Cult.AddProgress(4);
    IsOpen = false;
    GetPanelRect("myrect").Rect.anchoredPosition = GetPanelRect("myrect").OutisdePos;
    DefaultGroup.interactable = false;
    DefaultGroup.blocksRaycasts = false;
    SelectedTile.ButtonScript.CancleTile();
    SelectedTile = null;
    Debug.Log("이동 코루틴이 끝난 레후~");
  }
  private IEnumerator zoominview()
  {
    float _time = 0.0f, _targettime = ZoomInTime;
    Vector3 _startscale = IdleScale, _endscale = ZoomInScale;
    Vector2 _startposition = ScaleRect.anchoredPosition, 
      _endposition = new Vector2(SelectedTile.Rect.anchoredPosition.x*-1.0f*ZoomInScale.x,SelectedTile.Rect.anchoredPosition.y*-1.0f*ZoomInScale.y)-HolderRect.anchoredPosition;
    float _degree = 0.0f;
    while (_time < _targettime)
    {
      _degree = ZoomInCurve.Evaluate(_time / _targettime);
      ScaleRect.localScale = Vector3.Lerp(_startscale, _endscale, _degree);
      ScaleRect.anchoredPosition=Vector2.Lerp(_startposition,_endposition,_degree);
      _time += Time.deltaTime;
      yield return null;
    }
    ScaleRect.localScale = _endscale;
    ScaleRect.anchoredPosition = _endposition;
  }//정착지,야외 이동 후 지도 줌인 하는 코루틴
  public void SetPlayerPos(Vector2 coordinate)
  {
    Debug.Log("시작 플레이어 위치 설정");
    TileData _targettile = GameManager.Instance.MyGameData.MyMapData.Tile(coordinate);
    PlayerRect.anchoredPosition = _targettile.Rect.anchoredPosition;
    ScaleRect.localScale =IdleScale;
    HolderRect.anchoredPosition = PlayerRect.anchoredPosition * -1.0f;
    GameManager.Instance.MyGameData.Coordinate = coordinate;
  }
  public override void CloseUI()
  {
    UIManager.Instance.AddUIQueue(closeui());
    IsOpen = false;
  }
  private IEnumerator closeui()
  {
    DefaultGroup.blocksRaycasts = false;
    yield return StartCoroutine(UIManager.Instance.moverect(GetPanelRect("myrect").Rect, GetPanelRect("myrect").InsidePos, GetPanelRect("myrect").OutisdePos, 0.3f, false));
    DefaultGroup.interactable = false;
  }

}
