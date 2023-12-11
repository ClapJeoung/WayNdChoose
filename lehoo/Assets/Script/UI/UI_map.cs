using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;
using System.Linq;
using System.IO;

public class RouteData
{
  public TileData Start, End;
  public List<TileData> Route=new List<TileData>();
  public List<Image> Outlines=new List<Image>();  //Start(X)  Route(O)  End(O)
  public List<Image> Arrows=new List<Image>();    //Start(O)  Route(O)  End(X)
  public int Length
  {
    get { return Route.Count+1; }
  }
  public int RequireSupply
  {
    get
    {
      int _sum = 0;
      _sum += Start.RequireSupply;
      _sum+= End.RequireSupply;
      foreach(var _tile in Route)
        _sum += _tile.RequireSupply;
      return _sum;
    }
  }
  public bool IsPart(TileData tile)
  {
    return Start==tile||End==tile||Route.Contains(tile);
  }
}

public class UI_map : UI_default
{
  [SerializeField] private RectTransform PlayerRect = null;
  [SerializeField] private TextMeshProUGUI DragDescription = null;
  [SerializeField] private Button CameraResetButton = null;
  public void CameraResetButtonClicked()
  {
    if (UIManager.Instance.IsWorking) return;
    if (!IsMoved) return;
    UIManager.Instance.AddUIQueue(resetholderpos());
    IsMoved = false;
    CameraResetButton.interactable = false;
  }

  [SerializeField] private Image Outline_Selecting = null;
  [SerializeField] private List<Image> Outlines = new List<Image>();
  private List<Image> CurrentOutlines
  {
    get
    {
      List<Image> _temp=new List<Image>();
      foreach (var _routedata in Routes)
        foreach (var _outline in _routedata.Outlines)
          _temp.Add(_outline);
      return _temp;
    }
  }
  private Image GetEnableOutline
  {
    get
    {
      foreach(Image img in Outlines)
        if(!img.enabled)return img;

     GameObject _newoutline=Instantiate(OutlinePrefab, Outlines[0].transform.parent);
      Outlines.Add(_newoutline.transform.GetComponent<Image>());
      return _newoutline.transform.GetComponent<Image>();
    }
  }
  [SerializeField] private GameObject OutlinePrefab = null;
  [SerializeField] private Color MadColor = new Color();
  [SerializeField] private Color LowColor = new Color();
  [SerializeField] private Color MiddleColor = new Color();
  [SerializeField] private Color HighColor = new Color();

  [SerializeField] private List<Image> Arrows = new List<Image>();
  private Image GetEnableArrow
  {
    get
    {
      foreach (Image image in Arrows)
        if (!image.enabled) return image;

      GameObject _newArrow = Instantiate(ArrowPrefab, Arrows[0].transform.parent);
      Arrows.Add(_newArrow.GetComponent<Image>());
      return _newArrow.GetComponent<Image>();
    }
  }
  [SerializeField] private GameObject ArrowPrefab = null;
  private List<Image> CurrentArrows
  {
    get
    {
      List<Image> _temp = new List<Image>();
      foreach (var _routedata in Routes)
        foreach (var _arrow in _routedata.Outlines)
          _temp.Add(_arrow);
      return _temp;
    }
  }
  private List<Image> Arrows_Selectingtemp= new List<Image>();
  private void SetArrowRotation(ref Image img, HexDir dir)
  {
    switch (dir)
    {
      case HexDir.TopRight:
        img.rectTransform.rotation = Quaternion.Euler(0.0f, 0.0f, 60.0f);
        img.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        break;
      case HexDir.Right:
        img.rectTransform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        img.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        break;
      case HexDir.BottomRight:
        img.rectTransform.rotation = Quaternion.Euler(0.0f, 0.0f, 300.0f);
        img.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        break;
      case HexDir.BottomLeft:
        img.rectTransform.rotation = Quaternion.Euler(0.0f, 0.0f, 240.0f);
        img.rectTransform.localScale = new Vector3(1.0f, -1.0f, 1.0f);
        break;
      case HexDir.Left:
        img.rectTransform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
        img.rectTransform.localScale = new Vector3(1.0f, -1.0f, 1.0f);
        break;
      case HexDir.TopLeft:
        img.rectTransform.rotation = Quaternion.Euler(0.0f, 0.0f, 120.0f);
        img.rectTransform.localScale = new Vector3(1.0f, -1.0f, 1.0f);
        break;
    }
    if (!img.enabled) img.enabled = true;
  }

  [SerializeField] private float MoveTime = 0.8f;
  public maptext MapCreater = null;
  [HideInInspector] public List<GameObject> CityIcons = new List<GameObject>();
  [HideInInspector] public List<GameObject> TownIcons = new List<GameObject>();
  [HideInInspector] public List<GameObject> VillageIcons = new List<GameObject>();
  #region 열고닫기
  [SerializeField] private AnimationCurve OpenFoldCurve = new AnimationCurve();
  [SerializeField] private AnimationCurve CloseFoldCurve = new AnimationCurve();
  public Vector2 OpenSize = new Vector2(1240, 750.0f);
  public Vector2 CloseSize = new Vector2(70.0f, 750.0f);
  public RectTransform PanelLastHolder = null;
  private Vector2 Left_Pivot = new Vector2(0.0f, 0.5f);
  public Vector2 Left_OutsidePos = new Vector2(-1000.0f, -0.0f);
  public Vector2 Left_InsidePos = new Vector2(-620.0f, 0.0f);
  private Vector2 Left_Anchor = new Vector2(0.0F, 0.5f);
  public Vector2 Left_LastHolderPos = new Vector2(620.0f, 0.0f);
  public Vector2 Right_Pivot = new Vector2(1.0f, 0.5f);
  public Vector2 Right_InsidePos = new Vector2(620.0f, 0.0f);
  public Vector2 Right_OutsidePos = new Vector2(1200.0f, -0.0f);
  private Vector2 Right_Anchor = new Vector2(1.0F, 0.5f);
  public Vector2 Right_LastHolderPos = new Vector2(-620.0f, 0.0f);
  public float UIOpenTime_Fold = 0.8f;
  public float UIOpenTime_Move = 0.6f;
  public float UICloseTime_Fold = 0.6f;
  public float UICloseTime_Move = 0.4f;
  #endregion
  #region 지도 카메라
  [SerializeField] private RectTransform HolderRect = null;
  private float HolderPos_Min_x = 0.0f;
  private float HolderPos_Min_y = 0.0f;
  private float HolderPos_Max_x = 0.0f;
  private float HolderPos_Max_y = 0.0f;
  private bool IsMoved=false;
  private IEnumerator resetholderpos()
  {
    float _time = 0.0f, _targettime = 0.8f;
    Vector2 _originpos=HolderRect.anchoredPosition,_playerpos=PlayerRect.anchoredPosition*-1.0f;
    while(_time < _targettime)
    {
      Vector2 _newpos = Vector2.Lerp(_originpos, _playerpos, MoveAnimationCurve.Evaluate(_time / _targettime));
      _newpos = new Vector2(Mathf.Clamp(_newpos.x, HolderPos_Min_x, HolderPos_Max_x), Mathf.Clamp(_newpos.y, HolderPos_Min_y, HolderPos_Max_y));
      HolderRect.anchoredPosition = _newpos;
      _time+= Time.deltaTime; yield return null;
    }Vector2.Lerp(_originpos,_playerpos,MoveAnimationCurve.Evaluate(_time/_targettime));
    HolderRect.anchoredPosition = _playerpos;
    yield return null;
  }
  public void MoveHolderRect_mouse(Vector2 rawvector)
  {
    if (rawvector.sqrMagnitude <= 2.5f) return;
    IsMoved = true;
    CameraResetButton.interactable = true;
    Vector2 _newpos= HolderRect.anchoredPosition + rawvector;
    _newpos=new Vector2(Mathf.Clamp(_newpos.x,HolderPos_Min_x,HolderPos_Max_x),Mathf.Clamp(_newpos.y,HolderPos_Min_y,HolderPos_Max_y));
    HolderRect.anchoredPosition = _newpos;
  }
  #endregion
  #region 이동 조작 UI
  [SerializeField] private RectTransform TilePreviewRect = null;
  [SerializeField] private CanvasGroup TilePreviewGroup = null;
  [SerializeField] private Image TilePreview_Bottom = null;
  [SerializeField] private Image TilePreview_Top = null;
  [SerializeField] private Image TilePreview_IsEvent = null;
  [SerializeField] private Image TilePreview_Landmark = null;
  [SerializeField] private TextMeshProUGUI TileInfoText = null;
  [SerializeField] private TextMeshProUGUI BonusGoldText = null;
  [SerializeField] private TextMeshProUGUI MoveLengthText = null;
  [SerializeField] private TextMeshProUGUI MoveLengthCostText = null;
  [SerializeField] private CanvasGroup MovecostButtonGroup = null;
  [SerializeField] private Onpointer_highlight SanityButton_Highlight = null;
  [SerializeField] private CanvasGroup SanitybuttonGroup = null;
  [SerializeField] private Onpointer_highlight GoldButton_Highlight = null;
  [SerializeField] private CanvasGroup GoldbuttonGroup = null;
  public StatusTypeEnum SelectedCostType = StatusTypeEnum.HP;
  [SerializeField] private TextMeshProUGUI MoveCostText = null;
  public Image MadnessEffect = null;
  #endregion
  private List<TileData> ActiveTileData = new List<TileData>();
  private void ResetEnableTiles()
  {

    foreach(var tile in ActiveTileData)
    {
      tile.ButtonScript.Button.interactable = false;
      if (tile.ButtonScript.Preview != null) tile.ButtonScript.Preview.enabled = false;
      if (tile.Fogstate != 2) tile.SetFog(2);
    }
    ActiveTileData.Clear();

    List<TileData> _currents = GameManager.Instance.MyGameData.MyMapData.GetAroundTile(GameManager.Instance.MyGameData.Coordinate, ConstValues.ViewRange);
    foreach (TileData _tile in _currents) //새로운 주위 타일 전부 가져오기
    {
      _tile.ButtonScript.Button.interactable = true;
      if (_tile.ButtonScript.Preview != null) _tile.ButtonScript.Preview.enabled = true;
      ActiveTileData.Add(_tile);
      _tile.SetFog(2);
    }
  }

  public List<TileData> Destinations= new List<TileData>();   //좌클릭으로 찍은 타일들
  private TileData LastDestination
  {
    get
    {
      return Destinations.Count == 0?GameManager.Instance.MyGameData.CurrentTile: Destinations[Destinations.Count - 1];
    }
  }
  public List<RouteData> Routes= new List<RouteData>();
  private int RouteLength
  {
    get
    {
      int _sum = 0;
      foreach(var _route in Routes)
        _sum += _route.Length;
      return _sum;
    }
  }
  public int GetlengthAsRoute(TileData tile)
  {
    if (Destinations.Count > 0) 
      return RouteLength + tile.HexGrid.GetDistance(Destinations[Destinations.Count-1]);
    else 
      return tile.HexGrid.GetDistance(GameManager.Instance.MyGameData.CurrentTile);
  }
  public void AddDestination(TileData tile)
  {
    foreach (var _route in Routes)
      if (_route.IsPart(tile)) return;

    RouteData _newroute= new RouteData();
    _newroute.Start = LastDestination;
    _newroute.End = tile;
    List<HexDir> _grid = tile.HexGrid.GetRoute(LastDestination);
    for (int i = 0; i < _grid.Count - 1; i++)
      _newroute.Route.Add(GameManager.Instance.MyGameData.MyMapData.GetNextTile(i == 0 ? _newroute.Start : _newroute.Route[_newroute.Route.Count - 1], _grid[i]));
    for(int i = 0; i < _newroute.Length; i++)
    {
      TileData _targettile = i == _newroute.Length - 1 ? _newroute.End : _newroute.Route[i];
      int _length = GetlengthAsRoute(_targettile);
      Color _currentcolor = new Color();
      if (IsMad) _currentcolor = MadColor;
      else if (_length < ConstValues.MoveLength_Low) _currentcolor = LowColor;
      else if (_length < ConstValues.MoveLength_Middle) _currentcolor = MiddleColor;
      else _currentcolor = HighColor;
      _currentcolor.a = i == _newroute.Length - 1 ? 1.0f : 0.4f;
      Image _enableoutline = GetEnableOutline;
      SetOutline(_enableoutline, _targettile.ButtonScript.Rect, _currentcolor);
      _newroute.Outlines.Add(_enableoutline);
    }
    foreach(var _arrow in Arrows_Selectingtemp)
      _newroute.Arrows.Add(_arrow);
    Arrows_Selectingtemp.Clear();

    DisableOutline(Outline_Selecting);
    Destinations.Add(tile);
    Routes.Add(_newroute);

    UIManager.Instance.PreviewManager.ClosePreview();
  }
  public void RemoveDestination(TileData tile)
  {
    if (!Destinations.Contains(tile)) return;

    RouteData _fixroute = null, _removeroute= null;
    TileData _newnexttile = null;
    for (int i = 0; i < Routes.Count; i++)
    {
      if (Routes[i].End == tile)
      {
        if (i == Routes.Count - 1)
        {
          _removeroute = Routes[i];
          break;
        }
        else
        {
          _fixroute = Routes[i];
        }
      }
      else if (Routes[i].Start == tile)
      {
        _removeroute = Routes[i];
        _newnexttile =GameManager.Instance.MyGameData.MyMapData.Tile(_removeroute.End.Coordinate);
        break;
      }
    }

    if (_removeroute !=null)
    {
      foreach (var _outline in _removeroute.Outlines)
        _outline.enabled = false;
      foreach (var _arrow in _removeroute.Arrows)
        _arrow.enabled = false;
      Routes.Remove(_removeroute);
    }
    Destinations.Remove(tile);

    if (_fixroute != null)
    {
      _fixroute.End = _newnexttile;
      foreach (var _outline in _fixroute.Outlines)
        _outline.enabled = false;
      foreach (var _arrow in _fixroute.Arrows)
        _arrow.enabled = false;
      _fixroute.Route.Clear();
      List<HexDir> _grid = _fixroute.End.HexGrid.GetRoute(_fixroute.Start);
      for (int i = 0; i < _grid.Count - 1; i++)
        _fixroute.Route.Add(GameManager.Instance.MyGameData.MyMapData.GetNextTile(i == 0 ? _fixroute.Start : _fixroute.Route[_fixroute.Route.Count - 1], _grid[i]));
      for (int i = 0; i < _fixroute.Length; i++)
      {
        TileData _targettile = i == _fixroute.Length - 1 ? _fixroute.End : _fixroute.Route[i];
        Color _currentcolor = new Color();
        if (IsMad) _currentcolor = MadColor;
        else if (i < ConstValues.MoveLength_Low) _currentcolor = LowColor;
        else if (i < ConstValues.MoveLength_Middle) _currentcolor = MiddleColor;
        else _currentcolor = HighColor;
        _currentcolor.a = i == _fixroute.Length - 1 ? 1.0f : 0.4f;
        Image _enableoutline = GetEnableOutline;
        SetOutline(_enableoutline, _targettile.ButtonScript.Rect, _currentcolor);
        _fixroute.Outlines.Add(_enableoutline);

        if (i == _fixroute.Route.Count) continue;
        Vector3 _arrowpos = (i == 0 ? (_fixroute.Start.ButtonScript.Rect.anchoredPosition3D+ _fixroute.Route[i].ButtonScript.Rect.anchoredPosition3D)/2.0f :
          _fixroute.Route[i-1].ButtonScript.Rect.anchoredPosition3D +_fixroute.Route[i].ButtonScript.Rect.anchoredPosition3D) / 2.0f;
        Image _arrow = GetEnableArrow;
        _arrow.rectTransform.anchoredPosition = _arrowpos;
        SetArrowRotation(ref _arrow, i==0? _fixroute.Route[i].HexGrid.GetRoute(_fixroute.Start)[0] :
          _fixroute.Route[i].HexGrid.GetRoute(_fixroute.Route[i-1].HexGrid)[0]);
        _fixroute.Arrows.Add(_arrow);
      }
    }

  }
  private void ResetRoute()
  {
    foreach (var _outline in Outlines)
      if (_outline.enabled) _outline.enabled = false;
    foreach (var _arrow in Arrows)
      if (_arrow.enabled) _arrow.enabled = false;
    Destinations.Clear();
    Routes.Clear();
  }
  public void PointerEnterTile(TileData tile)
  {
    if (Route_Tile.Contains(tile)) return;

    if (!IsMad)
    {
      TilePreview_Bottom.sprite = tile.ButtonScript.BottomImage.sprite;
      TilePreview_Bottom.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -60.0f * tile.Rotation));
      TilePreview_Top.sprite = tile.ButtonScript.TopImage.sprite;
      TilePreview_IsEvent.enabled = tile.IsEvent;
      TilePreview_Landmark.sprite = tile.ButtonScript.LandmarkImage.sprite;
    }
    switch (GameManager.Instance.MyGameData.Quest_Cult_Phase)
    {
      case 0:
        UIManager.Instance.SidePanelCultUI.SetSettlementEffect(SettlementType.Village, tile.TileSettle == null ? false : tile.TileSettle.SettlementType == SettlementType.Village);
        break;
      case 1:
        UIManager.Instance.SidePanelCultUI.SetSettlementEffect(SettlementType.Town, tile.TileSettle == null ? false : tile.TileSettle.SettlementType == SettlementType.Town);
        break;
      case 2:
        UIManager.Instance.SidePanelCultUI.SetSettlementEffect(SettlementType.City, tile.TileSettle == null ? false : tile.TileSettle.SettlementType == SettlementType.City);
        break;
    }

    int _length = GetlengthAsRoute(tile);
    Color _currentcolor = new Color();
    if (IsMad) _currentcolor = MadColor;
    else if (_length < ConstValues.MoveLength_Low) _currentcolor = LowColor;
    else if (_length < ConstValues.MoveLength_Middle) _currentcolor = MiddleColor;
    else _currentcolor = HighColor;

    _currentcolor.a = 1.0f;

    SetOutline(Outline_Selecting, tile.ButtonScript.Rect,_currentcolor);

    List<HexDir> _routetemp= tile.HexGrid.GetRoute(LastDestination);
    TileData _currenttile= LastDestination, _nexttile = null;
    for(int i=0;i<_routetemp.Count;i++)
    {
      _nexttile = GameManager.Instance.MyGameData.MyMapData.GetNextTile(_currenttile, _routetemp[i]);
      Vector3 _arrowpos = (_currenttile.ButtonScript.Rect.anchoredPosition3D + _nexttile.ButtonScript.Rect.anchoredPosition3D) / 2.0f;
      Image _arrow = GetEnableArrow;
      _arrow.rectTransform.anchoredPosition = _arrowpos;
      SetArrowRotation(ref _arrow, _routetemp[i]);

      Arrows_Selectingtemp.Add(_arrow);
      _currenttile = _nexttile;
    }
  }
  public void ExitTile()
  {
    foreach (var _arrow in Arrows_Selectingtemp)
      _arrow.GetComponent<Image>().enabled = false;
    Arrows_Selectingtemp.Clear();

    DisableOutline(Outline_Selecting); }

  public void SetOutline(Image outline, RectTransform tilerect,Color color)
  {
    if (!outline.enabled) outline.enabled = true;
    outline.color=color;
    outline.rectTransform.position = tilerect.position;
    outline.rectTransform.anchoredPosition3D = new Vector3(outline.rectTransform.anchoredPosition3D.x, outline.rectTransform.anchoredPosition3D.y, 0.0f);
  }
  public void DisableOutline(Image outline) { if(outline.enabled) outline.enabled = false; }

  public bool IsMad = false;
  /// <summary>
  /// true:왼쪽에서 등장 false:오른쪽에서 등장
  /// </summary>
  /// <param name="dir"></param>
  public void OpenUI(bool dir)
  {
    if (HolderPos_Min_x == 0.0f)
    {
      float _size = GameManager.Instance.MyGameData.MyMapData.TileDatas[0, 0].ButtonScript.Rect.sizeDelta.x;
      float _length = 2.5f;
      Vector2 _mintile = GameManager.Instance.MyGameData.MyMapData.TileDatas[0, 0].ButtonScript.Rect.anchoredPosition;
      HolderPos_Max_x = -1.0f*_mintile.x- _size* _length;
      HolderPos_Max_y = -1.0f * _mintile.y - _size * _length;
      Vector2 _maxtile = GameManager.Instance.MyGameData.MyMapData.TileDatas[ConstValues.MapSize-1, ConstValues.MapSize - 1].ButtonScript.Rect.anchoredPosition;
      HolderPos_Min_x = -1.0f * _maxtile.x + _size * _length;
      HolderPos_Min_y = -1.0f * _maxtile.y + _size * _length;
    }

    IsOpen = true;
    UIManager.Instance.AddUIQueue(openui(dir));
  }
  
  private IEnumerator openui(bool dir)
  {
    if (PlayerPrefs.GetInt("Tutorial_Map") == 0) UIManager.Instance.TutorialUI.OpenTutorial_Map();
    if (DragDescription.text == "") DragDescription.text = GameManager.Instance.GetTextData("MapDragDescription");
    CameraResetButton.interactable = false;
    if (GameManager.Instance.MyGameData.Madness_Wild && (GameManager.Instance.MyGameData.TotalMoveCount % ConstValues.MadnessEffect_Wild == ConstValues.MadnessEffect_Wild - 1))
    {
      Debug.Log("자연 광기 발동");
      UIManager.Instance.HighlightManager.HighlightAnimation(HighlightEffectEnum.Madness, SkillTypeEnum.Wild);
      UIManager.Instance.AudioManager.PlaySFX(27, "madness");
      if (!MadnessEffect.enabled) MadnessEffect.enabled = true;
      IsMad = true;
      TilePreview_Bottom.transform.rotation = Quaternion.Euler(Vector3.zero);
      TilePreview_Bottom.sprite = GameManager.Instance.ImageHolder.MadnessActive;
      TilePreview_Top.sprite = GameManager.Instance.ImageHolder.Transparent;
      TilePreview_IsEvent.enabled = false;
      TilePreview_Landmark.sprite = GameManager.Instance.ImageHolder.Transparent;
    }
    else
    {
      if (MadnessEffect.enabled) MadnessEffect.enabled = false;
      IsMad = false;
      TilePreview_Bottom.transform.rotation = Quaternion.Euler(Vector3.zero);
      TilePreview_Bottom.sprite = GameManager.Instance.ImageHolder.Transparent;
      TilePreview_Top.sprite = GameManager.Instance.ImageHolder.Transparent;
      TilePreview_IsEvent.enabled = false;
      TilePreview_Landmark.sprite = GameManager.Instance.ImageHolder.Transparent;
    }

    ResetEnableTiles();
    ResetRoute();

    DisableOutline(Outline_Selecting);

    for(int i = 0; i < GameManager.Instance.MyGameData.MyMapData.AllSettles.Count; i++)
    {
      GameManager.Instance.MyGameData.MyMapData.AllSettles[i].Tile.ButtonScript.DiscomfortOutline.alpha =
        Mathf.Lerp(0.0f, 1.0f, GameManager.Instance.MyGameData.MyMapData.AllSettles[i].Discomfort / ConstValues.MaxDiscomfortForUI);
    }

    TileInfoText.text =IsMad?GameManager.Instance.GetTextData("Madness_Wild_Description"): GameManager.Instance.GetTextData("CHOOSETILE_MAP");
    BonusGold = 0;
    BonusGoldText.text = "";
    MoveLengthText.text = "";
    MoveLengthCostText.text = "";
    MoveCostText.text = "";
    MovecostButtonGroup.alpha = 0.0f;
    MovecostButtonGroup.interactable = false;
    MovecostButtonGroup.blocksRaycasts = false;

    SelectedCostType = StatusTypeEnum.HP;

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

    UIManager.Instance.AudioManager.PlaySFX(3);
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

    if (IsMoved)
    {
      CameraResetButton.interactable = false;
      IsMoved = false;
      yield return StartCoroutine(resetholderpos());
    }

    foreach(var _tile in ActiveTileData)
    {
      _tile.SetFog(2);
    }

    if (DoHighlight)
    {
      List<RectTransform> _highlightlist = new List<RectTransform>();
      List<TileData> _targettiles= new List<TileData>();
      switch (GameManager.Instance.MyGameData.Quest_Cult_Phase)
      {
        case 0:
          for (int i = 0; i < VillageIcons.Count; i++)
          {
            _highlightlist.Add(VillageIcons[i].GetComponent<RectTransform>());
            _targettiles.Add(GameManager.Instance.MyGameData.MyMapData.Villages[i].Tile);
          }
          break;
        case 1:
          for (int i = 0; i < TownIcons.Count; i++)
          {
            _highlightlist.Add(TownIcons[i].GetComponent<RectTransform>());
            _targettiles.Add(GameManager.Instance.MyGameData.MyMapData.Towns[i].Tile);
          }
          break;
        case 2:
          for (int i = 0; i < CityIcons.Count; i++)
          {
            _highlightlist.Add(CityIcons[i].GetComponent<RectTransform>());
            _targettiles.Add(GameManager.Instance.MyGameData.MyMapData.Citys[i].Tile);
          }
          break;
        case 3:
          switch (GameManager.Instance.MyGameData.Cult_SabbatSector)
          {
            case SectorTypeEnum.Residence:
              for (int i = 0; i < VillageIcons.Count; i++)
              {
                _highlightlist.Add(VillageIcons[i].GetComponent<RectTransform>());
                _targettiles.Add(GameManager.Instance.MyGameData.MyMapData.Villages[i].Tile);
              }
              break;
            case SectorTypeEnum.Temple:
              for (int i = 0; i < VillageIcons.Count; i++)
              {
                _highlightlist.Add(VillageIcons[i].GetComponent<RectTransform>());
                _targettiles.Add(GameManager.Instance.MyGameData.MyMapData.Villages[i].Tile);
              }
              for (int i = 0; i < TownIcons.Count; i++)
              {
                _highlightlist.Add(TownIcons[i].GetComponent<RectTransform>());
                _targettiles.Add(GameManager.Instance.MyGameData.MyMapData.Towns[i].Tile);
              }
              break;
            case SectorTypeEnum.Marketplace:
              for (int i = 0; i < TownIcons.Count; i++)
              {
                _highlightlist.Add(TownIcons[i].GetComponent<RectTransform>());
                _targettiles.Add(GameManager.Instance.MyGameData.MyMapData.Towns[i].Tile);
              }
              for (int i = 0; i < CityIcons.Count; i++)
              {
                _highlightlist.Add(CityIcons[i].GetComponent<RectTransform>());
                _targettiles.Add(GameManager.Instance.MyGameData.MyMapData.Citys[i].Tile);
              }
              break;
            case SectorTypeEnum.Library:
              for (int i = 0; i < CityIcons.Count; i++)
              {
                _highlightlist.Add(CityIcons[i].GetComponent<RectTransform>());
                _targettiles.Add(GameManager.Instance.MyGameData.MyMapData.Citys[i].Tile);
              }
              break;
          }
          break;
        case 4:
          _targettiles.Add(GameManager.Instance.MyGameData.Cult_RitualTile);
          _highlightlist.Add(GameManager.Instance.MyGameData.Cult_RitualTile.ButtonScript.LandmarkImage.rectTransform);
          break;
      }
      Vector3 _pos = Vector2.zero;
      float _targettime = 0.0f;
      TileData _highlighttarget = null;
      int _min = 100;
      foreach(var _tile in _targettiles)
      {
        int _newmin = GameManager.Instance.MyGameData.CurrentTile.HexGrid.GetDistance(_tile);
        if (_newmin < _min)
        {
          _min = _newmin;
          _highlighttarget = _tile;
        }
      }

      foreach (var _tile in GameManager.Instance.MyGameData.MyMapData.GetAroundTile(_highlighttarget, 1))
        if (_tile.Fogstate == 0) _tile.SetFog(1);
      _time = 0.0f;
      _pos = Vector2.zero;
      _startpos = HolderRect.anchoredPosition;
      _endpos = _highlighttarget.ButtonScript.Rect.anchoredPosition * -1.0f;
      _targettime = DoHighlight ? HighlightMovetime_First : HighlightMovetime_Else;
      while (_time < _targettime)
      {
        _pos = Vector3.Lerp(_startpos, _endpos, SettlementAnimationCurve.Evaluate(_time / _targettime));
        HolderRect.anchoredPosition3D = new Vector3(_pos.x, _pos.y, 0.0f);
        _time += Time.deltaTime;
        yield return null;
      }
      HolderRect.anchoredPosition3D = _endpos;

      _time = 0.0f;
      _targettime = DoHighlight ? HighlightSizeTime_First : HighlightSizeTime_Else;
      Vector3 _highlightscale = DoHighlight ? HighlightSize_First : HighlightSize_Second;
      while (_time < _targettime)
      {
        _highlighttarget.ButtonScript.LandmarkImage.rectTransform.localScale = Vector3.Lerp(Vector3.one, _highlightscale, SettlementIconCurve.Evaluate(_time / _targettime));
        _time += Time.deltaTime;
        yield return null;
      }
      _highlighttarget.ButtonScript.LandmarkImage.rectTransform.localScale = Vector3.one;

      _time = 0.0f;
      _pos = Vector2.zero;
      _startpos = HolderRect.anchoredPosition;
      _endpos = PlayerRect.anchoredPosition * -1.0f;
      _targettime = DoHighlight ? HighlightMovetime_First : HighlightMovetime_Else;
      while (_time < _targettime)
      {
        _pos = Vector3.Lerp(_startpos, _endpos, SettlementAnimationCurve.Evaluate(_time / _targettime));
        HolderRect.anchoredPosition3D = new Vector3(_pos.x, _pos.y, 0.0f);
        _time += Time.deltaTime;
        yield return null;
      }
      HolderRect.anchoredPosition3D = _endpos;
      DoHighlight = false;
    }

    DefaultGroup.interactable = true;
    DefaultGroup.blocksRaycasts = true;

  }
  public float HighlightMovetime_First = 1.5f;
  public float HighlightMovetime_Else = 0.8f;
  public Vector3 HighlightSize_First = new Vector3(1.5f, 1.5f, 1.5f);
  public Vector3 HighlightSize_Second = new Vector3(1.3f, 1.3f, 1.3f);
  public float HighlightSizeTime_First = 1.25f;
  public float HighlightSizeTime_Else = 1.25f;
  public bool DoHighlight = true;
  public AnimationCurve SettlementAnimationCurve = new AnimationCurve();
  public AnimationCurve SettlementIconCurve = new AnimationCurve();
  [SerializeField] private Vector2 TilePreviewDownPos = new Vector2(-235.0f, 23.0f);
  private float TilePreviewStartAlpha = 0.5f;
  private List<TileData> Route_Tile
  {
    get
    {
      List<TileData> _temp=new List<TileData>();
      foreach(var _route in Routes)
      {
        foreach (var _tile in _route.Route)
          _temp.Add(_tile);
        _temp.Add(_route.End);
      }
      return _temp;
    }
  }
  private int MovePointCost
  {
    get
    {
      int _count = 0;
      for (int i = 1; i < Route_Tile.Count; i++)
        _count += Route_Tile[i].RequireSupply;
      return _count;
    }
  }
  private TileData SelectedTile = null;
  public int SanityCost = 0, GoldCost = 0, BonusGold = 0;
  public void SelectTile(TileData selectedtile)
  {
    if (Destinations.Contains(selectedtile))
    {
      RemoveDestination(selectedtile);
      return;
    }
    //동일한 좌표면 호출되지 않게 이미 거름
    if (selectedtile.Coordinate == GameManager.Instance.MyGameData.Coordinate || Route_Tile.Contains(selectedtile)) return;


    AddDestination(selectedtile);

    UIManager.Instance.AudioManager.PlaySFX(5);

    SelectedTile = selectedtile;

    TilePreviewRect.anchoredPosition = TilePreviewDownPos;
    TilePreviewGroup.alpha = TilePreviewStartAlpha;
    if (!IsMad)
    {
      TilePreview_Bottom.sprite = SelectedTile.ButtonScript.BottomImage.sprite;
      TilePreview_Bottom.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -60.0f * SelectedTile.Rotation));
      TilePreview_IsEvent.enabled = SelectedTile.IsEvent;
      TilePreview_Top.sprite = SelectedTile.ButtonScript.TopImage.sprite;
      TilePreview_IsEvent.enabled = SelectedTile.IsEvent;
      TilePreview_Landmark.sprite = SelectedTile.ButtonScript.LandmarkImage.sprite;
    }
    StopAllCoroutines();
    StartCoroutine(UIManager.Instance.moverect(TilePreviewRect, TilePreviewDownPos, new Vector2(-235.0f,57.0f), 0.5f, UIManager.Instance.UIPanelOpenCurve));
    StartCoroutine(UIManager.Instance.ChangeAlpha(TilePreviewGroup, TilePreviewStartAlpha, 1.0f, 0.5f));

    if (IsMad)
    {
      TileInfoText.text = GameManager.Instance.GetTextData("Madness_Wild_Description");
    }
     else if (SelectedTile.TileSettle != null)
    {
      TileInfoText.text =  GameManager.Instance.GetTextData("MoveDescription_Settlement");
    }
    else if(SelectedTile.IsEvent)
    {
      TileInfoText.text = GameManager.Instance.GetTextData("MoveDescription_Event");
    }
    else
    {
      TileInfoText.text = GameManager.Instance.GetTextData("MoveDescription_Outer");
    }

    switch (GameManager.Instance.MyGameData.QuestType)
    {
      case QuestType.Cult:
        if (!IsMad)
        {
          string _progresstext = "";
          switch (GameManager.Instance.MyGameData.Quest_Cult_Phase)
          {
            case 0:
              if (SelectedTile.TileSettle != null && SelectedTile.TileSettle.SettlementType == SettlementType.Village)
              {
                _progresstext +=string.Format(GameManager.Instance.GetTextData("Cult_Progress_Settlement"),ConstValues.Quest_Cult_Progress_Village);
              }
              else _progresstext = "";
              break;
            case 1:
              if (SelectedTile.TileSettle != null && SelectedTile.TileSettle.SettlementType == SettlementType.Town)
              {
                _progresstext += string.Format(GameManager.Instance.GetTextData("Cult_Progress_Settlement"), ConstValues.Quest_Cult_Progress_Town);
              }
              else _progresstext = "";
              break;
            case 2:
              if (SelectedTile.TileSettle != null && SelectedTile.TileSettle.SettlementType == SettlementType.City)
              {
                _progresstext += string.Format(GameManager.Instance.GetTextData("Cult_Progress_Settlement"), ConstValues.Quest_Cult_Progress_City);
              }
              else _progresstext = "";
              break;
            case 4:
              if (CheckRitual)
              {
                _progresstext += string.Format(GameManager.Instance.GetTextData("Cult_Progress_Ritual_Effect"), ConstValues.Quest_Cult_Progress_Ritual);
                UIManager.Instance.SidePanelCultUI.SetRitualEffect(true);
              }
              else
              {
                _progresstext = "";
                UIManager.Instance.SidePanelCultUI.SetRitualEffect(false);
              }
              break;

          }
          TileInfoText.text += _progresstext;
        }
        break;
    }

    switch (GameManager.Instance.MyGameData.Quest_Cult_Phase)
    {
      case 0:
          UIManager.Instance.SidePanelCultUI.SetSettlementEffect(SettlementType.Village, SelectedTile.TileSettle==null?false: SelectedTile.TileSettle.SettlementType == SettlementType.Village);
        break;
      case 1:
        UIManager.Instance.SidePanelCultUI.SetSettlementEffect(SettlementType.Town, SelectedTile.TileSettle == null ? false : SelectedTile.TileSettle.SettlementType == SettlementType.Town);
        break;
      case 2:
        UIManager.Instance.SidePanelCultUI.SetSettlementEffect(SettlementType.City, SelectedTile.TileSettle == null ? false : SelectedTile.TileSettle.SettlementType == SettlementType.City);
        break;
    }

    MovecostButtonGroup.alpha = 1.0f;
    MovecostButtonGroup.interactable = true;
    MovecostButtonGroup.blocksRaycasts = true;
    
    SanityCost = GameManager.Instance.MyGameData.GetMoveSanityCost(MovePointCost);
    GoldCost = GameManager.Instance.MyGameData.GetMoveGoldCost(MovePointCost);
    if (SelectedTile.TileSettle == null)
    {
      BonusGold = SelectedTile.RequireSupply>1?(int)((SelectedTile.RequireSupply) * ConstValues.GoldPerSupplies * GameManager.Instance.MyGameData.GetGoldGenModify(true)):0;
      BonusGold += (Route_Tile.Count > ConstValues.Tendency_Head_p1_length && GameManager.Instance.MyGameData.Tendency_Head.Level >= 1) ? ConstValues.Tendency_Head_p1_value : 0;
    }
    SelectedCostType = StatusTypeEnum.HP;
    MoveCostText.text = "";

    SanitybuttonGroup.interactable = true;
    SanityButton_Highlight.Interactive = true;
    SanityButton_Highlight.SetInfo(HighlightEffectEnum.Sanity);
    SanityButton_Highlight.SetInfo(HighlightEffectEnum.Movepoint);

    bool _goldable = GameManager.Instance.MyGameData.Gold >= GoldCost;
    GoldbuttonGroup.interactable = _goldable;
    GoldButton_Highlight.Interactive = _goldable;
    GoldButton_Highlight.SetInfo(HighlightEffectEnum.Gold);
    GoldButton_Highlight.SetInfo(HighlightEffectEnum.Movepoint);
    GoldbuttonGroup.alpha = _goldable ? 1.0f : 0.4f;

    string[] _bonusgoldtext = null;
    if (!IsMad)
    switch (BonusGold)
    {
      case 0:
        _bonusgoldtext = GameManager.Instance.GetTextData("BonusGold_0").Split('@');
        break;
        case 1: case 2:
        _bonusgoldtext = GameManager.Instance.GetTextData("BonusGold_2").Split('@');
        break;
      case 3: case 4:
        _bonusgoldtext = GameManager.Instance.GetTextData("BonusGold_4").Split('@');
        break;
      default:
        _bonusgoldtext = GameManager.Instance.GetTextData("BonusGold_over").Split('@');
        break;
    }
    BonusGoldText.text =IsMad? GameManager.Instance.GetTextData(StatusTypeEnum.Gold, 2) + " +?":
      SelectedTile.TileSettle!=null?"":
      ((Route_Tile.Count>2 && GameManager.Instance.MyGameData.Tendency_Head.Level >= 1) ? 
      "<sprite=104>":
      "") + _bonusgoldtext[Random.Range(0,_bonusgoldtext.Length)]+" "+ GameManager.Instance.GetTextData(StatusTypeEnum.Gold, 2)+" +" + BonusGold.ToString();
    MoveLengthText.text = IsMad ? "<sprite=100> ?" :
      string.Format(GameManager.Instance.GetTextData("MoveLength"),
      GameManager.Instance.MyGameData.Supply,
      WNCText.PercentageColor(MovePointCost.ToString(), MovePointCost <= GameManager.Instance.MyGameData.Supply ? 1.0f : 0.0f));
    if (GameManager.Instance.MyGameData.Supply < 0)
    {
      MoveLengthCostText.text = GameManager.Instance.GetTextData("Movepoint_NoSupplies");
    }
    else if (MovePointCost > GameManager.Instance.MyGameData.Supply)
    {
      MoveLengthCostText.text = string.Format(GameManager.Instance.GetTextData("LackofMovepoint"),
     MovePointCost - GameManager.Instance.MyGameData.Supply);
    }
    else MoveLengthCostText.text = "";

  }
  private bool CheckRitual
  {
    get
    {
      foreach (var _tile in Route_Tile)
        if (_tile.Landmark == LandmarkType.Ritual) return true;
      return false;
    }
  }
  public void EnterPointerStatus(StatusTypeEnum type)
  {
    string _costtext = "";
    switch (type)
    {
      case StatusTypeEnum.Sanity:
        SelectedCostType = StatusTypeEnum.Sanity;

        _costtext = string.Format(GameManager.Instance.GetTextData("MAPCOSTTYPE_SANITY"),
          MovePointCost <= GameManager.Instance.MyGameData.Supply?"" : string.Format(GameManager.Instance.GetTextData("AmplifiedValues"),
        MovePointCost - GameManager.Instance.MyGameData.Supply, (GameManager.Instance.MyGameData.Tendency_Head.Level <= -1 ? "<sprite=103>" : "") + (int)(GameManager.Instance.MyGameData.MovePointAmplified * 100)),
        !IsMad ?SanityCost:"?");
        break;
      case StatusTypeEnum.Gold:
        if (GameManager.Instance.MyGameData.Gold < GoldCost) return;

        SelectedCostType = StatusTypeEnum.Gold;

        _costtext = string.Format(GameManager.Instance.GetTextData("MAPCOSTTYPE_GOLD"),
                    MovePointCost <= GameManager.Instance.MyGameData.Supply ? "" : string.Format(GameManager.Instance.GetTextData("AmplifiedValues"),
        MovePointCost - GameManager.Instance.MyGameData.Supply, (GameManager.Instance.MyGameData.Tendency_Head.Level <= -1 ? "<sprite=103>" : "") + (int)(GameManager.Instance.MyGameData.MovePointAmplified * 100)),
!IsMad ?GoldCost:"?");
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
    if (IsMad)
    {
      List<TileData> _availabletiles = new List<TileData>();
      foreach (var _tile in GameManager.Instance.MyGameData.MyMapData.GetAroundTile(SelectedTile, 1))
      {
        if (_tile == SelectedTile) continue;
        if (_tile == GameManager.Instance.MyGameData.CurrentTile) continue;
        if (_tile.Interactable == false) continue;
        if (!ActiveTileData.Contains(_tile)) continue;
        _availabletiles.Add(_tile);
      }
      if (_availabletiles.Count == 0)
      {
        _availabletiles = new List<TileData>();
        foreach (var _tile in GameManager.Instance.MyGameData.MyMapData.GetAroundTile(SelectedTile, 2))
        {
          if (_tile == SelectedTile) continue;
          if (_tile == GameManager.Instance.MyGameData.CurrentTile) continue;
          if (_tile.Interactable == false) continue;
          if (!ActiveTileData.Contains(_tile)) continue;
          _availabletiles.Add(_tile);
        }
      }
      SelectedTile = _availabletiles[Random.Range(0, _availabletiles.Count)];

      SanityCost = GameManager.Instance.MyGameData.GetMoveSanityCost(MovePointCost);
      GoldCost = GameManager.Instance.MyGameData.GetMoveGoldCost(MovePointCost);

      GameManager.Instance.MyGameData.TotalMoveCount++;
      UIManager.Instance.SetWildMadCount();
    }
    if (SelectedCostType == StatusTypeEnum.Sanity && GameManager.Instance.MyGameData.Sanity < SanityCost && !GameManager.Instance.MyGameData.MadnessSafe)
    {
      GameManager.Instance.GameOver();
      yield break;
    }

    if (IsMoved)
    {
      IsMoved = false;
      CameraResetButton.interactable = false;
      yield return StartCoroutine(resetholderpos());
    }

    Dictionary<TileData, int> _movepointicondata = new Dictionary<TileData, int>();
    int _totalmp = 0;
    for(int i = 1; i < Route_Tile.Count; i++)
    {
      int _mp = Route_Tile[i].RequireSupply;
      _totalmp += _mp;
      if (GameManager.Instance.MyGameData.Supply >= _totalmp)
      {
        _movepointicondata.Add(Route_Tile[i],_mp);
      }
      else
      {
        _movepointicondata.Add(Route_Tile[i],Mathf.Clamp(GameManager.Instance.MyGameData.Supply - (_totalmp - _mp),0,100));
      }
    }
    yield return StartCoroutine(UIManager.Instance.SetIconEffect_movepoint_using(_movepointicondata,SelectedCostType));

 //   yield return StartCoroutine(UIManager.Instance.statusgainanimation(PlayerRect));

    bool _iswalking = false;
    if (GameManager.Instance.MyGameData.Supply >= MovePointCost)
      _iswalking = true;
    else _iswalking = false;


    GameManager.Instance.MyGameData.Supply -= MovePointCost;
    switch (SelectedCostType)
    {
      case StatusTypeEnum.Sanity:
        GameManager.Instance.MyGameData.Sanity -= SanityCost;
        break;
      case StatusTypeEnum.Gold:
        GameManager.Instance.MyGameData.Gold -= GoldCost;
        break;
    }

 //   Debug.Log($"시작 타일 {GameManager.Instance.MyGameData.Coordinate} 목표 타일 {SelectedTile.Coordinate}");

    List<Vector2> _path= new List<Vector2>();
    _path.Add(PlayerRect.anchoredPosition);
    MapData _map = GameManager.Instance.MyGameData.MyMapData;
    for(int i = 0; i < Route_Tile.Count; i++)
    {
      _path.Add(Route_Tile[i].ButtonScript.Rect.anchoredPosition);
    }
    if(_iswalking)
      UIManager.Instance.AudioManager.PlayWalking();
    else UIManager.Instance.AudioManager.PlaySFX(29);


    float _time = 0.0f;             //x
    int _pathcount = _path.Count-1; //길 개수-1 (마지막 좌표는 current가 되면 안되니까)   n
    int _currentindex = 0;          //y를 개수로 나눈 값(현재 start가 될 index)
    int _lastindex = 0;
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

      if (_lastindex!=_currentindex)
      {
        if(Route_Tile[_currentindex].Landmark == LandmarkType.Ritual)
          UIManager.Instance.CultUI.AddProgress(4, null);

        List<TileData> _newarounds = GameManager.Instance.MyGameData.MyMapData.GetAroundTile(Route_Tile[_currentindex], ConstValues.ViewRange);
        foreach (var _tile in _newarounds)
        {
          ActiveTileData.Add(_tile);
          _tile.SetFog(2);
        }

        _lastindex = _currentindex;
      }

      _time += Time.deltaTime;
      yield return null;
    }
    if(SelectedTile.TileSettle==null) GameManager.Instance.MyGameData.Gold += BonusGold;
    UIManager.Instance.AudioManager.StopWalking();

    PlayerRect.anchoredPosition = _path[_path.Count-1];
    HolderRect.anchoredPosition = PlayerRect.anchoredPosition * -1.0f;

    GameManager.Instance.MyGameData.Coordinate = SelectedTile.Coordinate;

 //   StartCoroutine(zoominview());

    //CloseUI 안 쓰고 여기서 닫기 실행
    yield return new WaitForSeconds(0.7f);

    UIManager.Instance.AudioManager.PlaySFX(4);

    UIManager.Instance.SidePanelCultUI.SetRitualEffect(false);

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
      case LandmarkType.Ritual:
        GameManager.Instance.MyGameData.Turn++;

        GameManager.Instance.MyGameData.CurrentSettlement = null;
        GameManager.Instance.MyGameData.DownAllDiscomfort(ConstValues.DiscomfortDownValue);
        EventManager.Instance.SetOutsideEvent(GameManager.Instance.MyGameData.MyMapData.GetTileData(SelectedTile.Coordinate));
        break;

      case LandmarkType.Village:
      case LandmarkType.Town:
      case LandmarkType.City:
        GameManager.Instance.MyGameData.FirstRest = true;
        GameManager.Instance.EnterSettlement(SelectedTile.TileSettle);

        GameManager.Instance.MyGameData.Turn++;
        GameManager.Instance.SaveData();
        break;
    }
    IsOpen = false;
    SelectedTile = null;
    //Debug.Log("이동 코루틴이 끝난 레후~");
  }
  public void SetPlayerPos(Vector2 coordinate)
  {
    TileData _targettile = GameManager.Instance.MyGameData.MyMapData.Tile(coordinate);
    PlayerRect.anchoredPosition = _targettile.ButtonScript.Rect.anchoredPosition;
 //   ScaleRect.localScale =IdleScale;
    HolderRect.anchoredPosition = PlayerRect.anchoredPosition * -1.0f;
   // Debug.Log($"({coordinate.x},{coordinate.y}) -> {PlayerRect.anchoredPosition}");
  }

}
