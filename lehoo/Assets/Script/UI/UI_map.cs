using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;
using System.Linq;
using UnityEngine.WSA;

public class UI_map : UI_default
{
  [SerializeField] private RectTransform PlayerRect = null;
  [SerializeField] private Image Outline_Idle = null;
  [SerializeField] private Image Outline_Select = null;
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
  public Vector2 OpenPos = new Vector2(0.0f, 375.0f);
  public Vector2 ClosePos = new Vector2(0.0f, 700.0f);
  public Vector2 OpenSize = new Vector2(1100.0f, 750.0f);
  public Vector2 CloseSize = new Vector2(1100.0f, 60.0f);
  public float UIOpenTime_Fold = 0.7f;
  public float UIOpenTime_Move = 0.4f;
  public float UICloseTime_Fold = 0.5f;
  public float UICloseTime_Move = 0.4f;

  [SerializeField] private AnimationCurve ZoomInCurve = null;
  [SerializeField] private RectTransform HolderRect = null;
  [SerializeField] private RectTransform ScaleRect = null;
  private Vector3 IdleScale = Vector3.one;
  [SerializeField] private Vector3 ZoomInScale = Vector3.one* 1.5f;
  [SerializeField] private float ZoomInTime = 1.2f;

  [SerializeField] private CanvasGroup MoveInfoGroup = null;
  [SerializeField] private TextMeshProUGUI GuidText = null;
  [SerializeField] private RectTransform GuidRect = null;
  [SerializeField] private Vector2 GuidPos_Tile = new Vector2(316.0f, 281.0f);
  [SerializeField] private Vector2 GuidPos_Cost = new Vector2(316.0f, 16.0f);
  [SerializeField] private Image TilePreview_Bottom = null;
  [SerializeField] private Image TilePreview_Top = null;
  [SerializeField] private Image TilePreview_Landmark = null;
  [SerializeField] private TextMeshProUGUI TileInfoText = null;
  [SerializeField] private CanvasGroup MovecostButtonGroup = null;
  [SerializeField] private CanvasGroup SanitybuttonGroup = null;
  [SerializeField] private CanvasGroup GoldbuttonGroup = null;
  private float MoveButtonDisableAlpha = 0.2f;
  public StatusTypeEnum SelectedCostType = StatusTypeEnum.HP;
  [SerializeField] private TextMeshProUGUI MoveCostText = null;
  [SerializeField] private TextMeshProUGUI ProgressText = null;
  private bool IsRitual = false;

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
        UIManager.Instance.MapUI.MapCreater.MyTiles.GetTile(GameManager.Instance.MyGameData.Cult_RitualTile.landmarkSprite);

    }
  }
  public void SetOutline_Idle(TileData tile)
  {
    SetOutline(Outline_Idle, tile.Rect);

    if (SelectedTile == null)
    {
      TilePreview_Bottom.sprite = tile.ButtonScript.BottomImage.sprite;
      TilePreview_Bottom.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -60.0f * tile.Rotation));
      TilePreview_Top.sprite = tile.ButtonScript.TopImage.sprite;
      TilePreview_Landmark.sprite = tile.ButtonScript.LandmarkImage.sprite;
    }
  }
  public void DisableOutline_Idle() => DisableOutline(Outline_Idle);
  public void SetOutline(Image outline, RectTransform tilerect)
  {
    if (outline.enabled == false) outline.enabled = true;
    outline.rectTransform.position = tilerect.position;
    outline.rectTransform.anchoredPosition3D = new Vector3(outline.rectTransform.anchoredPosition3D.x, outline.rectTransform.anchoredPosition3D.y, 0.0f);
  }
  public void DisableOutline(Image outline)
  {
    outline.enabled = false;
  }
  public void OpenUI()
  {
    IsOpen = true;
    UIManager.Instance.AddUIQueue(openui());
  }
  
  private IEnumerator openui()
  {
    ResetEnableTiles();

    DisableOutline(Outline_Idle);
    DisableOutline(Outline_Select);

    IsRitual = false;
    MoveInfoGroup.interactable = true;
    GuidRect.anchoredPosition = GuidPos_Tile;
    GuidText.text = GameManager.Instance.GetTextData("CHOOSETILE_MAP");
    TileInfoText.text = "";
    MoveCostText.text = "";
    ProgressText.text = "";
    MovecostButtonGroup.alpha = 0.0f;
    MovecostButtonGroup.interactable = false;
    MovecostButtonGroup.blocksRaycasts = false;

    SelectedCostType = StatusTypeEnum.HP;

    ScaleRect.localScale = IdleScale;
    ScaleRect.anchoredPosition = Vector2.zero;

    DefaultGroup.interactable = false;
    DefaultGroup.blocksRaycasts = false;
    DefaultRect.sizeDelta = CloseSize;
    yield return StartCoroutine(UIManager.Instance.moverect(DefaultRect, ClosePos, OpenPos, UIOpenTime_Move, UIManager.Instance.UIPanelOpenCurve));
    yield return new WaitForSeconds(0.3f);
    float _time = 0.0f;
    Vector2 _rect = DefaultRect.rect.size;
    while (_time < UIOpenTime_Fold)
    {
      _rect = Vector2.Lerp(CloseSize, OpenSize, _time / UIOpenTime_Fold);
      DefaultRect.sizeDelta = _rect;

      _time+= Time.deltaTime;
      yield return null;
    }
    DefaultRect.sizeDelta = OpenSize;
    DefaultGroup.interactable = true;
    DefaultGroup.blocksRaycasts = true;

  }
  public List<HexDir> Length=new List<HexDir>();
  public int SanityCost = 0, GoldCost = 0;
  [HideInInspector] public int QuestInfo = 0;
  public void SelectTile(TileData selectedtiledata)
  {
    //동일한 좌표면 호출되지 않게 이미 거름
    if (selectedtiledata.Coordinate==GameManager.Instance.MyGameData.Coordinate||( SelectedTile != null && selectedtiledata == SelectedTile)) return;

    SetOutline(Outline_Select, selectedtiledata.Rect);

    TileData _currenttile = GameManager.Instance.MyGameData.MyMapData.Tile(GameManager.Instance.MyGameData.Coordinate);
    Length = GameManager.Instance.GetLength(_currenttile, selectedtiledata);

    SelectedTile = selectedtiledata;

    TilePreview_Bottom.sprite = SelectedTile.ButtonScript.BottomImage.sprite;
    TilePreview_Bottom.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -60.0f * SelectedTile.Rotation));
    TilePreview_Top.sprite = SelectedTile.ButtonScript.TopImage.sprite;
    TilePreview_Landmark.sprite = SelectedTile.ButtonScript.LandmarkImage.sprite;


    GuidText.text = GameManager.Instance.GetTextData("CHOOSECOSTTYPE_MAP");
    GuidRect.anchoredPosition = GuidPos_Cost;

    if (SelectedTile.TileSettle != null)
    {
      TileInfoText.text = GameManager.Instance.GetTextData("MoveDescription_Settlement");
    }
    else
    {
      TileInfoText.text = GameManager.Instance.GetTextData("MoveDescription_Outer");
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
              _progresstext += string.Format(GameManager.Instance.GetTextData("Quest0_Progress_Ritual_Effect"),ConstValues.Quest_Cult_RitualMovepoint, ConstValues.Quest_Cult_Progress_Ritual);
            }
            break;
          case 2:
            if (SelectedTile.Landmark == LandmarkType.Ritual&&GameManager.Instance.MyGameData.Cult_RitualTile_CoolDown==0)
            {
              _progresstext += string.Format(GameManager.Instance.GetTextData("Quest0_Progress_Ritual_Effect"), ConstValues.Quest_Cult_RitualMovepoint, ConstValues.Quest_Cult_Progress_Ritual);
            }
            else _progresstext = "";
            break;
        }
        ProgressText.text = _progresstext;
        break;
    }

    MovecostButtonGroup.alpha = 1.0f;
    MovecostButtonGroup.interactable = true;
    MovecostButtonGroup.blocksRaycasts = true;
    
    SanityCost = GameManager.Instance.MyGameData.GetMoveSanityCost(Length.Count, MovePointCost);
    GoldCost = GameManager.Instance.MyGameData.GetMoveGoldCost(Length.Count, MovePointCost);

    SanitybuttonGroup.interactable = true;
  }

  private int MovePointCost = 0;
  public void EnterPointerStatus(StatusTypeEnum type)
  {
    string _costtext = "";
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

        _costtext = string.Format(GameManager.Instance.GetTextData("MAPCOSTTYPE_SANITY"), SanityCost);

        if (GameManager.Instance.MyGameData.MovePoint < MovePointCost)
          _costtext += string.Format(GameManager.Instance.GetTextData("LackofMovepoint"),
            WNCText.GetSanityColor("+" + $"{(int)(GameManager.Instance.MyGameData.MovePointAmplified * 100) - 100}%"));
        break;
      case StatusTypeEnum.Gold:
        if (GameManager.Instance.MyGameData.Gold < GoldCost) return;

        SelectedCostType = StatusTypeEnum.Gold;
        SanitybuttonGroup.alpha = MoveButtonDisableAlpha;
        GoldbuttonGroup.alpha = 1.0f;

        _costtext = string.Format(GameManager.Instance.GetTextData("MAPCOSTTYPE_GOLD"), GoldCost);

        if (GameManager.Instance.MyGameData.MovePoint < MovePointCost)
          _costtext += string.Format(GameManager.Instance.GetTextData("LackofMovepoint"),
            WNCText.GetGoldColor("+" + $"{(int)(GameManager.Instance.MyGameData.MovePointAmplified * 100) - 100}%"));
        break;
    }
    MoveCostText.text = _costtext;

    LayoutRebuilder.ForceRebuildLayoutImmediate(MoveCostText.transform.parent.transform as RectTransform);
  }
  public void ExitPointerStatus(StatusTypeEnum type)
  {
    if (type==StatusTypeEnum.Gold&& GameManager.Instance.MyGameData.Gold < GoldCost) return;

    SanitybuttonGroup.alpha = MoveButtonDisableAlpha;
    GoldbuttonGroup.alpha = MoveButtonDisableAlpha;
    MoveCostText.text = "";
  }

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

      Length = GameManager.Instance.GetLength(GameManager.Instance.MyGameData.CurrentTile, SelectedTile);
    }
    GameManager.Instance.MyGameData.ClearBeforeEvents();
    IsRitual = SelectedTile.Landmark == LandmarkType.Ritual;

    GameManager.Instance.MyGameData.MovePoint -= MovePointCost;
    switch (SelectedCostType)
    {
      case StatusTypeEnum.Sanity:
        GameManager.Instance.MyGameData.Sanity -= SanityCost;
        break;
      case StatusTypeEnum.Gold:
        GameManager.Instance.MyGameData.Gold -= GoldCost;
        break;
    }

    Debug.Log(GameManager.Instance.MyGameData.Coordinate);
    Debug.Log(GameManager.Instance.MyGameData.CurrentSettlement);
    Debug.Log(GameManager.Instance.MyGameData.CurrentTile.Coordinate);
    List<Vector2> _path= new List<Vector2>();
    _path.Add(PlayerRect.anchoredPosition);
    MapData _map = GameManager.Instance.MyGameData.MyMapData;
    TileData _currenttile = GameManager.Instance.MyGameData.CurrentTile;
    for(int i = 0; i < Length.Count; i++)
    {
      _currenttile = _map.GetNextTile(_currenttile, Length[i]);
      _path.Add(_currenttile.Rect.anchoredPosition);
    }
    //_path : (최초 플레이어 좌표,~~~,마지막 좌표)

    float _time = 0.0f;
    float _time_split=MoveTime/(_path.Count-1);

    for (int i = 0; i < _path.Count - 1; i++)
    {
      while(_time<_time_split)
      {
        PlayerRect.anchoredPosition = Vector3.Lerp(_path[i], _path[i + 1], _time / _time_split);
        HolderRect.anchoredPosition = PlayerRect.anchoredPosition * -1.0f;
        _time += Time.deltaTime;
        yield return null;
      }
      _time = 0.0f;
      yield return null;
    }

    PlayerRect.anchoredPosition = _path[_path.Count-1];
    HolderRect.anchoredPosition = PlayerRect.anchoredPosition * -1.0f;

    GameManager.Instance.MyGameData.Coordinate = _currenttile.Coordinate;

    switch (SelectedTile.Landmark)
    {
      case LandmarkType.Outer:
        if (GameManager.Instance.MyGameData.QuestType == QuestType.Cult && GameManager.Instance.MyGameData.Quest_Cult_Phase > 0)
          GameManager.Instance.MyGameData.Quest_Cult_Progress += ConstValues.Quest_Cult_Progress_Ritual;

        GameManager.Instance.MyGameData.CurrentSettlement = null;
        GameManager.Instance.MyGameData.DownAllDiscomfort(ConstValues.DiscomfortDownValue);
        yield return new WaitForSeconds(0.5f);
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
        yield return new WaitForSeconds(0.5f);
        EventManager.Instance.SetOutsideEvent(GameManager.Instance.MyGameData.MyMapData.GetTileData(SelectedTile.Coordinate));

        if (GameManager.Instance.MyGameData.Quest_Cult_Phase == 2) GameManager.Instance.MyGameData.Cult_RitualTile_CoolDown = ConstValues.Quest_Cult_CoolDown;
        break;
    }

    StartCoroutine(zoominview());

    //CloseUI 안 쓰고 여기서 닫기 실행
    DefaultGroup.interactable = false;
    DefaultGroup.blocksRaycasts = false;
    _time = 0.0f;
    Vector2 _rect = DefaultRect.rect.size;
    while (_time < UIOpenTime_Fold)
    {
      _rect = Vector2.Lerp(OpenSize, CloseSize, _time / UIOpenTime_Fold);
      DefaultRect.sizeDelta = _rect;

      _time += Time.deltaTime;
      yield return null;
    }
    DefaultRect.sizeDelta = CloseSize;
    yield return new WaitForSeconds(0.3f);
    yield return StartCoroutine(UIManager.Instance.moverect(DefaultRect, OpenPos, ClosePos, UIOpenTime_Move, UIManager.Instance.UIPanelCLoseCurve));


    yield return new WaitForSeconds(0.1f);
    if (IsRitual) UIManager.Instance.CultUI.AddProgress(4);
    IsOpen = false;
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
    TileData _targettile = GameManager.Instance.MyGameData.MyMapData.Tile(coordinate);
    PlayerRect.anchoredPosition = _targettile.Rect.anchoredPosition;
    ScaleRect.localScale =IdleScale;
    HolderRect.anchoredPosition = PlayerRect.anchoredPosition * -1.0f;
    Debug.Log($"({coordinate.x},{coordinate.y}) -> {PlayerRect.anchoredPosition}");
  }

}
