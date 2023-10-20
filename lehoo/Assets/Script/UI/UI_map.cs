using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;
using System.Linq;
using UnityEngine.WSA;
using System.IO;

public class UI_map : UI_default
{
  [SerializeField] private RectTransform PlayerRect = null;
  [SerializeField] private Image Outline_Idle = null;
  [SerializeField] private Image Outline_Select = null;
  [SerializeField] private float MoveTime = 0.8f;
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
  [SerializeField] private AnimationCurve OpenFoldCurve = new AnimationCurve();
  [SerializeField] private AnimationCurve CloseFoldCurve = new AnimationCurve();
  public Vector2 OpenSize = new Vector2(1240, 750.0f);
  public Vector2 CloseSize = new Vector2(70.0f, 750.0f);
  public RectTransform PanelLastHolder = null;
  private Vector2 Left_Pivot = new Vector2(0.0f, 0.5f);
  public Vector2 Left_OutsidePos = new Vector2(-1000.0f, -0.0f);
  public Vector2 Left_InsidePos = new Vector2(-620.0f, 0.0f);
  private Vector2 Left_Anchor = new Vector2(1.0f, 0.5f);
  public Vector2 Left_LastHolderPos = new Vector2(620.0f, 0.0f);
  public Vector2 Right_Pivot = new Vector2(1.0f, 0.5f);
  public Vector2 Right_InsidePos = new Vector2(620.0f, 0.0f);
  public Vector2 Right_OutsidePos = new Vector2(1200.0f, -0.0f);
  private Vector2 Right_Anchor = new Vector2(0.0f, 0.5f);
  public Vector2 Right_LastHolderPos = new Vector2(-620.0f, 0.0f);
  public float UIOpenTime_Fold = 0.8f;
  public float UIOpenTime_Move = 0.6f;
  public float UICloseTime_Fold = 0.6f;
  public float UICloseTime_Move = 0.4f;

  [SerializeField] private AnimationCurve ZoomInCurve = null;
  [SerializeField] private RectTransform HolderRect = null;
 // [SerializeField] private RectTransform ScaleRect = null;
 // private Vector3 IdleScale = Vector3.one;
 // [SerializeField] private Vector3 ZoomInScale = Vector3.one* 1.5f;
  //[SerializeField] private float ZoomInTime = 1.2f;

  [SerializeField] private TextMeshProUGUI GuidText = null;
  [SerializeField] private RectTransform GuidRect = null;
  [SerializeField] private Vector2 GuidPos_Tile = new Vector2(316.0f, 281.0f);
  [SerializeField] private Vector2 GuidPos_Cost = new Vector2(316.0f, 16.0f);
  [SerializeField] private Image TilePreview_Bottom = null;
  [SerializeField] private Image TilePreview_Top = null;
  [SerializeField] private Image TilePreview_Landmark = null;
  [SerializeField] private TextMeshProUGUI TileInfoText = null;
  [SerializeField] private CanvasGroup MovecostButtonGroup = null;
  [SerializeField] private Onpointer_highlight SanityButton_Highlight = null;
  [SerializeField] private CanvasGroup SanitybuttonGroup = null;
  [SerializeField] private Onpointer_highlight GoldButton_Highlight = null;
  [SerializeField] private CanvasGroup GoldbuttonGroup = null;
 // private float MoveButtonDisableAlpha = 0.2f;
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
    SetOutline(Outline_Idle, tile.ButtonScript.Rect);

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
  /// <summary>
  /// true:왼쪽에서 등장 false:오른쪽에서 등장
  /// </summary>
  /// <param name="dir"></param>
  public void OpenUI(bool dir)
  {
    IsOpen = true;
    UIManager.Instance.AddUIQueue(openui(dir));
  }
  
  private IEnumerator openui(bool dir)
  {
    ResetEnableTiles();

    DisableOutline(Outline_Idle);
    DisableOutline(Outline_Select);

    IsRitual = false;
    GuidRect.anchoredPosition = GuidPos_Tile;
    GuidText.text = GameManager.Instance.GetTextData("CHOOSETILE_MAP");
    TileInfoText.text = "";
    MoveCostText.text = "";
    ProgressText.text = "";
    MovecostButtonGroup.alpha = 0.0f;
    MovecostButtonGroup.interactable = false;
    MovecostButtonGroup.blocksRaycasts = false;

    SelectedCostType = StatusTypeEnum.HP;

 //   ScaleRect.localScale = IdleScale;
 //   ScaleRect.anchoredPosition = Vector2.zero;

    DefaultGroup.interactable = false;
    DefaultGroup.blocksRaycasts = false;
    Vector2 _startpos= Vector2.zero,_endpos= Vector2.zero;
    if (dir == true)
    {
      DefaultRect.pivot = Left_Pivot;
      PanelLastHolder.anchorMin = Left_Anchor;
      PanelLastHolder.anchorMax= Left_Anchor;
      PanelLastHolder.anchoredPosition = Left_LastHolderPos;
      _startpos = Left_OutsidePos;
      _endpos = Left_InsidePos;
    }
    else
    {
      DefaultRect.pivot = Right_Pivot;
      PanelLastHolder.anchorMin = Right_Anchor;
      PanelLastHolder.anchorMax = Right_Anchor;
      PanelLastHolder.anchoredPosition = Right_LastHolderPos;
      _startpos = Right_OutsidePos;
      _endpos = Right_InsidePos;
    }
    DefaultRect.sizeDelta = CloseSize;
    yield return StartCoroutine(UIManager.Instance.moverect(DefaultRect, _startpos, _endpos, UIOpenTime_Move,UIManager.Instance.UIPanelOpenCurve));
    yield return new WaitForSeconds(0.1f);
    float _time = 0.0f;
    Vector2 _rect = DefaultRect.rect.size;
    while (_time < UIOpenTime_Fold)
    {
      _rect = Vector2.Lerp(CloseSize, OpenSize, OpenFoldCurve.Evaluate(_time / UIOpenTime_Fold));
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

    SetOutline(Outline_Select, selectedtiledata.ButtonScript.Rect);

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
              _progresstext += string.Format(GameManager.Instance.GetTextData("Cult_Progress_Ritual_Effect"),ConstValues.Quest_Cult_RitualMovepoint, ConstValues.Quest_Cult_Progress_Ritual);
            }
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

    SelectedCostType = StatusTypeEnum.HP;
    MoveCostText.text = "";

    SanitybuttonGroup.interactable = true;
    SanityButton_Highlight.Interactive = true;
    SanityButton_Highlight.SetInfo(HighlightEffectEnum.Sanity, -1*SanityCost);
    SanityButton_Highlight.SetInfo(HighlightEffectEnum.Movepoint,-1* MovePointCost);

    bool _goldable = GameManager.Instance.MyGameData.Gold >= GoldCost;
    GoldbuttonGroup.interactable = _goldable;
    GoldButton_Highlight.Interactive = _goldable;
    GoldButton_Highlight.SetInfo(HighlightEffectEnum.Gold, -1*GoldCost);
    GoldButton_Highlight.SetInfo(HighlightEffectEnum.Movepoint, -1*MovePointCost);
    GoldbuttonGroup.alpha = _goldable?1.0f:0.4f;

  }

  private int MovePointCost = 0;
  public void EnterPointerStatus(StatusTypeEnum type)
  {
    string _costtext = "";
    switch (type)
    {
      case StatusTypeEnum.Sanity:
        SelectedCostType = StatusTypeEnum.Sanity;
    //    SanitybuttonGroup.alpha = 1.0f;
     //   GoldbuttonGroup.alpha = MoveButtonDisableAlpha;
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
      //  SanitybuttonGroup.alpha = MoveButtonDisableAlpha;
      //  GoldbuttonGroup.alpha = 1.0f;

        _costtext = string.Format(GameManager.Instance.GetTextData("MAPCOSTTYPE_GOLD"), GoldCost);

        if (GameManager.Instance.MyGameData.MovePoint < MovePointCost)
          _costtext += string.Format(GameManager.Instance.GetTextData("LackofMovepoint"),
            WNCText.GetGoldColor("+" + $"{(int)(GameManager.Instance.MyGameData.MovePointAmplified * 100) - 100}%"));
        break;
    }
    MoveCostText.text = _costtext;
  }
  public void ExitPointerStatus(StatusTypeEnum type)
  {
    if (type==StatusTypeEnum.Gold&& GameManager.Instance.MyGameData.Gold < GoldCost) return;

   // SanitybuttonGroup.alpha = MoveButtonDisableAlpha;
   // GoldbuttonGroup.alpha = MoveButtonDisableAlpha;
    MoveCostText.text = "";
  }

  public void MoveMap()
  {
    if (UIManager.Instance.IsWorking) return;
    if (SelectedCostType == StatusTypeEnum.Gold && GameManager.Instance.MyGameData.Gold < GoldCost) return;

    DefaultGroup.interactable = false;
    DefaultGroup.blocksRaycasts = false;
    SanityButton_Highlight.Interactive = false;
    GoldButton_Highlight.Interactive = false;

    //  UIManager.Instance.ResetEventPanels();
    UIManager.Instance.AddUIQueue(movemap());
  }
  public AnimationCurve MoveAnimationCurve = new AnimationCurve();
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

      Debug.Log("자연 광기 발동");
      UIManager.Instance.HighlightManager.HighlightAnimation(HighlightEffectEnum.Madness);

      Length = GameManager.Instance.GetLength(GameManager.Instance.MyGameData.CurrentTile, SelectedTile);
    }

    switch (SelectedCostType)
    {
      case StatusTypeEnum.Sanity:
        if (GameManager.Instance.MyGameData.MovePoint >= 0)
        {
       //   StartCoroutine(UIManager.Instance.SetIconEffect(true, StatusTypeEnum.Sanity, PlayerRect));
          yield return StartCoroutine(UIManager.Instance.SetIconEffect_movepoint(true, MovePointCost, PlayerRect));
        }
        else
        {
        //  yield return StartCoroutine(UIManager.Instance.SetIconEffect(true, StatusTypeEnum.Sanity, PlayerRect));
        }
        break;
      case StatusTypeEnum.Gold:
        if (GameManager.Instance.MyGameData.MovePoint >= 0)
        {
        //  StartCoroutine(UIManager.Instance.SetIconEffect(true, StatusTypeEnum.Gold, PlayerRect));
          yield return StartCoroutine(UIManager.Instance.SetIconEffect_movepoint(true, MovePointCost, PlayerRect));
        }
        else
        {
       //   yield return StartCoroutine(UIManager.Instance.SetIconEffect(true, StatusTypeEnum.Gold, PlayerRect));
        }
        break;
    }

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

    Debug.Log($"시작 타일 {GameManager.Instance.MyGameData.Coordinate} 목표 타일 {SelectedTile.Coordinate}");

    List<Vector2> _path= new List<Vector2>();
    _path.Add(PlayerRect.anchoredPosition);
    MapData _map = GameManager.Instance.MyGameData.MyMapData;
    TileData _currenttile = GameManager.Instance.MyGameData.CurrentTile;
    for(int i = 0; i < Length.Count; i++)
    {
      _currenttile = _map.GetNextTile(_currenttile, Length[i]);
      _path.Add(_currenttile.ButtonScript.Rect.anchoredPosition);
    }

    float _time = 0.0f;             //x
    int _pathcount = _path.Count-1; //길 개수-1 (마지막 좌표는 current가 되면 안되니까)   n
    int _currentindex = 0;          //y를 개수로 나눈 값(현재 start가 될 index)
    float _value = 0.0f;            //커브에 따른 이동 값(y)                              0.0f ~ 1.0f
    float _valuedegree = 1.0f / _pathcount;
    float _currentvalue = 0.0f;     //
    Vector2 _current = Vector2.zero,_next= Vector2.zero;
    float _movetime = MoveTime * _pathcount;
    while (_time < _movetime)
    {
      _value = MoveAnimationCurve.Evaluate(_time / _movetime);

      _currentindex = Mathf.FloorToInt(_value / _valuedegree);
      if (_currentindex == _pathcount) break;
      _current = _path[_currentindex];
      _next = _path[_currentindex+1];
      _currentvalue = (_value % _valuedegree) * _pathcount;

      PlayerRect.anchoredPosition = Vector3.Lerp(_current,_next,_currentvalue);
      HolderRect.anchoredPosition = PlayerRect.anchoredPosition * -1.0f;
      _time += Time.deltaTime;
      yield return null;
    }

      PlayerRect.anchoredPosition = _path[_path.Count-1];
    HolderRect.anchoredPosition = PlayerRect.anchoredPosition * -1.0f;

    GameManager.Instance.MyGameData.Coordinate = _currenttile.Coordinate;

 //   StartCoroutine(zoominview());

    //CloseUI 안 쓰고 여기서 닫기 실행
    yield return new WaitForSeconds(0.7f);

    DefaultRect.pivot = Left_Pivot;
    DefaultRect.anchoredPosition = Left_InsidePos;
    PanelLastHolder.anchorMin = Left_Anchor;
    PanelLastHolder.anchorMax = Left_Anchor;
    PanelLastHolder.anchoredPosition = Left_LastHolderPos;

    _time = 0.0f;
    Vector2 _rect = DefaultRect.rect.size;
    while (_time < UICloseTime_Fold)
    {
      _rect = Vector2.Lerp(OpenSize, CloseSize,CloseFoldCurve.Evaluate(_time / UICloseTime_Fold));
      DefaultRect.sizeDelta = _rect;

      _time += Time.deltaTime;
      yield return null;
    }
    DefaultRect.sizeDelta = CloseSize;
    yield return new WaitForSeconds(0.2f);
    yield return StartCoroutine(UIManager.Instance.moverect(DefaultRect, Left_InsidePos,Left_OutsidePos , UIOpenTime_Move, UIManager.Instance.UIPanelCLoseCurve));

    switch (SelectedTile.Landmark)
    {
      case LandmarkType.Outer:
        if (GameManager.Instance.MyGameData.QuestType == QuestType.Cult && GameManager.Instance.MyGameData.Quest_Cult_Phase > 0)
          GameManager.Instance.MyGameData.Quest_Cult_Progress += ConstValues.Quest_Cult_Progress_Ritual;

        GameManager.Instance.MyGameData.CurrentSettlement = null;
        GameManager.Instance.MyGameData.DownAllDiscomfort(ConstValues.DiscomfortDownValue);
        EventManager.Instance.SetOutsideEvent(GameManager.Instance.MyGameData.MyMapData.GetTileData(SelectedTile.Coordinate));
        break;

      case LandmarkType.Village:
      case LandmarkType.Town:
      case LandmarkType.City:
        GameManager.Instance.MyGameData.FirstRest = true;
        GameManager.Instance.EnterSettlement(SelectedTile.TileSettle);
        break;
      case LandmarkType.Ritual:
        GameManager.Instance.MyGameData.Quest_Cult_Progress += ConstValues.Quest_Cult_Progress_Ritual;

        GameManager.Instance.MyGameData.CurrentSettlement = null;
        EventManager.Instance.SetOutsideEvent(GameManager.Instance.MyGameData.MyMapData.GetTileData(SelectedTile.Coordinate));

        if (GameManager.Instance.MyGameData.Quest_Cult_Phase == 2) GameManager.Instance.MyGameData.Cult_RitualTile_CoolDown = ConstValues.Quest_Cult_CoolDown;
        break;
    }
    if (IsRitual) UIManager.Instance.CultUI.AddProgress(4);
    IsOpen = false;
    SelectedTile = null;
    Debug.Log("이동 코루틴이 끝난 레후~");
  }
  /*
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
  */
  public void SetPlayerPos(Vector2 coordinate)
  {
    TileData _targettile = GameManager.Instance.MyGameData.MyMapData.Tile(coordinate);
    PlayerRect.anchoredPosition = _targettile.ButtonScript.Rect.anchoredPosition;
 //   ScaleRect.localScale =IdleScale;
    HolderRect.anchoredPosition = PlayerRect.anchoredPosition * -1.0f;
   // Debug.Log($"({coordinate.x},{coordinate.y}) -> {PlayerRect.anchoredPosition}");
  }

}
