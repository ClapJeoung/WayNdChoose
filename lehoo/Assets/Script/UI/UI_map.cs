using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;
using System.Linq;
using System.IO;
using System.Drawing.Text;
using UnityEngine.Rendering;

public class UI_map : UI_default
{
  [SerializeField] private RectTransform PlayerRect = null;
  [SerializeField] private Image Outline_Idle = null;
  [SerializeField] private Image Outline_Select = null;
  [SerializeField] private List<Image> Outline_Routes = new List<Image>();
  [SerializeField] private List<Image> Outline_Routes_temps= new List<Image>();
  [SerializeField] private float MoveTime = 0.8f;
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

  [SerializeField] private RectTransform HolderRect = null;
  [SerializeField] private RectTransform TilePreviewRect = null;
  [SerializeField] private CanvasGroup TilePreviewGroup = null;
  [SerializeField] private Image TilePreview_Bottom = null;
  [SerializeField] private Image TilePreview_Top = null;
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
  private List<TileData> ActiveTileData = new List<TileData>();

  /// <summary>
  /// 주위 2칸 타일 업데이트
  /// </summary>
  private void ResetEnableTiles()
  {

    foreach(var tile in ActiveTileData)
    {
      tile.ButtonScript.Button.interactable = false;
      if (tile.ButtonScript.Preview != null) tile.ButtonScript.Preview.enabled = false;
    }
    ActiveTileData.Clear();

    List<TileData> _currents = GameManager.Instance.MyGameData.MyMapData.GetAroundTile(GameManager.Instance.MyGameData.Coordinate, 3);
    foreach (TileData _tile in _currents) //새로운 주위 2칸 타일 전부 가져오기
    {
      _tile.ButtonScript.Button.interactable = true;
      if (_tile.ButtonScript.Preview != null) _tile.ButtonScript.Preview.enabled = true;
      ActiveTileData.Add(_tile);
    }
  }
  public List<TileData> SetRouteTemp(TileData tile)
  {
    if (Route_Temp.Count > 0)
    {
      for (int i = 1; i < Route_Temp.Count; i++)
      {
        DisableOutline(Outline_Routes_temps[i]);
      }
    }

    //GetLength로 방향들 산출
    TileData _currenttile = GameManager.Instance.MyGameData.MyMapData.Tile(GameManager.Instance.MyGameData.Coordinate);
    List<HexDir> _dirtemp = MapData.GetLength(_currenttile, tile);
    //배열들 만들기
    List<List<int>> _results = new List<List<int>>();
    if (_dirtemp.Count == 1) { _results.Add(new List<int> { 0 }); }
    else
    {
      int _elementindex = -1, _inputindex = 0;// _failcount = 0;
      int _maxcount = _dirtemp.Count;
      List<int> _elements = new List<int>();
      for (int i = 0; i < _dirtemp.Count; i++) _elements.Add(i);
      Dictionary<int, List<int>> _disableindex = new Dictionary<int, List<int>>();
      List<int> _current = null;

      _results.Add(new List<int>());
      while (true)
      {

        if (_disableindex.ContainsKey(0) && _disableindex[0].Count == _maxcount) break;
        _elementindex++;

        _current = _results[_results.Count - 1];

        int _temp = 0;
        try
        {
          _temp = _elements[_elementindex];
        }
        catch (System.Exception)
        {
          throw;
        }
        if (_current.Contains(_temp) || (_disableindex.ContainsKey(_inputindex) && _disableindex[_inputindex].Contains(_temp)))
        {
          AddBan(_inputindex, _temp);

          if (_disableindex[_inputindex].Count == _maxcount)
          {
            _disableindex[_inputindex].Clear();
            _inputindex = _inputindex > 0 ? _inputindex - 1 : 0;
            _elementindex = -1;
            AddBan(_inputindex, _current[_current.Count - 1]);
            if (_current.Count > 0) _current.RemoveAt(_current.Count - 1);
          }
          continue;
        }
        _current.Add(_temp);

        if (_inputindex == _maxcount - 1)
        {
          bool _isokay = true;
          for (int i = 0; i < _results.Count - 1; i++)
          {
            List<bool> _colaspeelement = new List<bool>();
            for (int j = 0; j < _current.Count; j++)
            {
              if (_current[j] == _results[i][j]) { _colaspeelement.Add(true); }
              else _colaspeelement.Add(false);
            }
            if (!_colaspeelement.Contains(false)) { _isokay = false; break; }
          }
          if (_isokay)
          {
            _inputindex = -1;
            _disableindex.Clear();

            _results.Add(new List<int>());
          }
          else
          {
            AddBan(_inputindex, _temp);
            _current.RemoveAt(_current.Count - 1);

            if (_disableindex[_inputindex].Count == _maxcount)
            {
              _disableindex[_inputindex].Clear();
              _inputindex = _inputindex > 0 ? _inputindex - 1 : 0;
              _elementindex = -1;
              AddBan(_inputindex, _current[_current.Count - 1]);
              if (_current.Count > 0) _current.RemoveAt(_current.Count - 1);
              continue;
            }
            else
            {
              continue;
            }
          }
        }
        _inputindex++;
        _elementindex = -1;
      }
      if (_results[_results.Count - 1].Count == 0) _results.RemoveAt(_results.Count - 1);

      void AddBan(int inputindex, int value)
      {
        int _index = inputindex > 0 ? inputindex : 0;
        if (!_disableindex.ContainsKey(_index))
        {
          int _banindex = _index;
          _disableindex.Add(_banindex, new List<int>());
        }
        if (!_disableindex[_index].Contains(value)) _disableindex[_index].Add(value);
      }
    }
    //배열대로 집어넣은 HexDir들
    List<List<HexDir>> _routedirs = new List<List<HexDir>>();
    foreach (var _list in _results)
    {
      List<HexDir> _dirs = new List<HexDir>();
      foreach (var _value in _list) _dirs.Add(_dirtemp[_value]);
      _routedirs.Add(_dirs);
    }
    //겹치는게 있으면 제거
    bool _iscollaspe = true;
    while (_iscollaspe == true)
    {
      if (_routedirs.Count == 1) break;

      for (int i = 0; i < _routedirs.Count; i++)
      {
        for (int j = i + 1; j < _routedirs.Count; j++)
        {
          _iscollaspe = true;
          for (int k = 0; k < _routedirs[i].Count; k++)
            if (_routedirs[i][k] != _routedirs[j][k]) { _iscollaspe = false; break; }

          if (_iscollaspe)
          {
            _routedirs.RemoveAt(j);
            break;
          }
        }
        if (_iscollaspe) break;
      }
      if (_iscollaspe) continue;
    }
    //Tile로 변환
    List<List<TileData>> _routes = new List<List<TileData>>();
    foreach (var _dirlist in _routedirs)
    {
      List<TileData> _newroute = new List<TileData>();
      _newroute.Add(GameManager.Instance.MyGameData.CurrentTile);
      for (int i = 0; i < _dirlist.Count; i++)
        _newroute.Add(GameManager.Instance.MyGameData.MyMapData.GetNextTile(_newroute[_newroute.Count - 1], _dirlist[i]));
      _routes.Add(_newroute);
    }
    //제일 짧은 거리 산출
    int _minMP = 100, _targetindex = 0;
    for (int i = 0; i < _routes.Count; i++)
    {
      int _MP = 0;
      foreach (TileData _tile in _routes[i])
        _MP += _tile.MovePoint;
      if (_MP < _minMP)
      {
        int _tempindex = i;
        _targetindex = _tempindex;
        _minMP = _MP;
      }
    }
    //Temp에 넣음
    Route_Temp = _routes[_targetindex];

    for (int i = 1; i < Route_Temp.Count; i++)
    {
      SetOutline(Outline_Routes_temps[i], Route_Temp[i].ButtonScript.Rect);
    }

    return Route_Temp;
  }
  public void EnterTile(TileData tile)
  {
    SetOutline(Outline_Idle, tile.ButtonScript.Rect);
    if (Route_Temp.Count > 0)
    {
      for (int i = 1; i < Route_Temp.Count; i++)
      {
        DisableOutline(Outline_Routes_temps[i]);
      }
    }

    if (SelectedTile == null)
    {
      if (!IsMad)
      {
        TilePreview_Bottom.sprite = tile.ButtonScript.BottomImage.sprite;
        TilePreview_Bottom.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -60.0f * tile.Rotation));
        TilePreview_Top.sprite = tile.ButtonScript.TopImage.sprite;
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
    }
  }
  public void ExitTile()
  {
    if (Route_Temp.Count > 0)
    {
      for (int i = 1; i < Route_Temp.Count; i++)
      {
        DisableOutline(Outline_Routes_temps[i]);
      }
    }
    DisableOutline(Outline_Idle); }

  public void SetOutline(Image outline, RectTransform tilerect)
  {
    if (outline.enabled == false) outline.enabled = true;
    outline.rectTransform.position = tilerect.position;
    outline.rectTransform.anchoredPosition3D = new Vector3(outline.rectTransform.anchoredPosition3D.x, outline.rectTransform.anchoredPosition3D.y, 0.0f);
  }
  public void DisableOutline(Image outline) => outline.enabled = false;

  public bool IsMad = false;
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
    if (PlayerPrefs.GetInt("Tutorial_Map") == 0) UIManager.Instance.TutorialUI.OpenTutorial_Map();

    if (GameManager.Instance.MyGameData.Madness_Wild && (GameManager.Instance.MyGameData.TotalMoveCount % ConstValues.MadnessEffect_Wild == ConstValues.MadnessEffect_Wild - 1))
    {
      Debug.Log("자연 광기 발동");
      UIManager.Instance.HighlightManager.HighlightAnimation(HighlightEffectEnum.Madness, SkillTypeEnum.Wild);
      UIManager.Instance.AudioManager.PlaySFX(27, 5);
      if (!MadnessEffect.enabled) MadnessEffect.enabled = true;
      IsMad = true;
      TilePreview_Bottom.transform.rotation = Quaternion.Euler(Vector3.zero);
      TilePreview_Bottom.sprite = GameManager.Instance.ImageHolder.MadnessActive;
      TilePreview_Top.sprite = GameManager.Instance.ImageHolder.Transparent;
      TilePreview_Landmark.sprite = GameManager.Instance.ImageHolder.Transparent;
    }
    else
    {
      if (MadnessEffect.enabled) MadnessEffect.enabled = false;
      IsMad = false;
      TilePreview_Bottom.transform.rotation = Quaternion.Euler(Vector3.zero);
      TilePreview_Bottom.sprite = GameManager.Instance.ImageHolder.UnknownTile;
      TilePreview_Top.sprite = GameManager.Instance.ImageHolder.Transparent;
      TilePreview_Landmark.sprite = GameManager.Instance.ImageHolder.Transparent;
    }

    ResetEnableTiles();

    if (Route_Tile.Count > 0)
    {
      for (int i = 1; i < Route_Tile.Count; i++)
      {
        DisableOutline(Outline_Routes[i]);
      }
    }
    if (Route_Temp.Count > 0)
    {
      for (int i = 1; i < Route_Temp.Count; i++)
      {
        DisableOutline(Outline_Routes_temps[i]);
      }
    }

    DisableOutline(Outline_Idle);
    DisableOutline(Outline_Select);

    QuestInfo = false;
   // GuidRect.anchoredPosition = GuidPos_Tile;
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

    List<RectTransform> _highlightlist=new List<RectTransform>();
    switch (GameManager.Instance.MyGameData.Quest_Cult_Phase)
    {
      case 0:
        _highlightlist.Add(VillageIcons[0].GetComponent<RectTransform>() );
        _highlightlist.Add(VillageIcons[1].GetComponent<RectTransform>() );
        break;
      case 1:
        _highlightlist.Add(TownIcon.GetComponent<RectTransform>());
        break;
      case 2:
        _highlightlist.Add(CityIcon.GetComponent<RectTransform>());
        break;
      case 3:
        switch (GameManager.Instance.MyGameData.Cult_SabbatSector)
        {
          case SectorTypeEnum.Residence:
            _highlightlist.Add(VillageIcons[0].GetComponent<RectTransform>());
            _highlightlist.Add(VillageIcons[1].GetComponent<RectTransform>());
            break;
          case SectorTypeEnum.Temple:
            _highlightlist.Add(VillageIcons[0].GetComponent<RectTransform>());
            _highlightlist.Add(VillageIcons[1].GetComponent<RectTransform>());
            _highlightlist.Add(TownIcon.GetComponent<RectTransform>());
            break;
          case SectorTypeEnum.Marketplace:
            _highlightlist.Add(TownIcon.GetComponent<RectTransform>());
            _highlightlist.Add(CityIcon.GetComponent<RectTransform>());
            break;
          case SectorTypeEnum.Library:
            _highlightlist.Add(CityIcon.GetComponent<RectTransform>());
            break;
        }
        break;
      case 4:
        _highlightlist.Add(GameManager.Instance.MyGameData.Cult_RitualTile.ButtonScript.LandmarkImage.rectTransform);
        break;
    }
    Vector3 _pos = Vector2.zero;
    float _targettime = 0.0f;
    for (int i = 0; i < _highlightlist.Count ; i++)
    {
      _time = 0.0f;
      _pos = Vector2.zero; 
      _startpos = HolderRect.anchoredPosition;
      _endpos = _highlightlist[i].anchoredPosition * -1.0f;
      _targettime = FirstHighlight ? HighlightMovetime_First : HighlightMovetime_Else;
      while (_time < _targettime)
      {
        _pos = Vector3.Lerp(_startpos, _endpos, SettlementAnimationCurve.Evaluate(_time / _targettime));
        HolderRect.anchoredPosition3D = new Vector3(_pos.x, _pos.y, 0.0f);
        _time += Time.deltaTime;
        yield return null;
      }
      HolderRect.anchoredPosition3D = _endpos;

      _time = 0.0f;
      _targettime = FirstHighlight ? HighlightSizeTime_First : HighlightSizeTime_Else;
      Vector3 _highlightscale = FirstHighlight ? HighlightSize_First : HighlightSize_Second;
      while (_time < _targettime)
      {
        _highlightlist[i].localScale = Vector3.Lerp(Vector3.one, _highlightscale, SettlementIconCurve.Evaluate(_time / _targettime));
        _time += Time.deltaTime;
        yield return null;
      }
      _highlightlist[i].localScale = Vector3.one;
    }

    _time = 0.0f;
    _pos = Vector2.zero;
    _startpos = HolderRect.anchoredPosition;
    _endpos = PlayerRect.anchoredPosition * -1.0f;
    _targettime = FirstHighlight ? HighlightMovetime_First : HighlightMovetime_Else;
    while (_time < _targettime)
    {
      _pos = Vector3.Lerp(_startpos, _endpos, SettlementAnimationCurve.Evaluate(_time / _targettime));
      HolderRect.anchoredPosition3D = new Vector3(_pos.x, _pos.y, 0.0f);
      _time += Time.deltaTime;
      yield return null;
    }
    HolderRect.anchoredPosition3D = _endpos;


    FirstHighlight = false;
    DefaultGroup.interactable = true;
    DefaultGroup.blocksRaycasts = true;

  }
  public float HighlightMovetime_First = 1.5f;
  public float HighlightMovetime_Else = 0.8f;
  public Vector3 HighlightSize_First = new Vector3(1.5f, 1.5f, 1.5f);
  public Vector3 HighlightSize_Second = new Vector3(1.3f, 1.3f, 1.3f);
  public float HighlightSizeTime_First = 1.25f;
  public float HighlightSizeTime_Else = 1.25f;
  public bool FirstHighlight = true;
  public AnimationCurve SettlementAnimationCurve = new AnimationCurve();
  public AnimationCurve SettlementIconCurve = new AnimationCurve();
  [SerializeField] private Vector2 TilePreviewDownPos = new Vector2(-235.0f, 23.0f);
  private float TilePreviewStartAlpha = 0.5f;
  private List<TileData> Route_Tile = new List<TileData>();
  private List<TileData> Route_Temp= new List<TileData>();
  private int MovePointCost
  {
    get
    {
      int _count = 0;
      for (int i = 1; i < Route_Tile.Count; i++)
        _count += Route_Tile[i].MovePoint;
      return _count;
    }
  }
  private TileData SelectedTile = null;
  public List<HexDir> Route_Dir=new List<HexDir>();
  public int SanityCost = 0, GoldCost = 0, BonusGold = 0;
  [HideInInspector] public bool QuestInfo = false;
  public void SelectTile(TileData selectedtiledata)
  {
    //동일한 좌표면 호출되지 않게 이미 거름
    if (selectedtiledata.Coordinate == GameManager.Instance.MyGameData.Coordinate || (SelectedTile != null && selectedtiledata == SelectedTile)) return;

    if (Route_Tile.Count > 0)
    {
      for(int i=0;i< Route_Tile.Count;i++)
      {
        DisableOutline(Outline_Routes[i]);
      }
    }

    UIManager.Instance.AudioManager.PlaySFX(5);
    SetOutline(Outline_Select, selectedtiledata.ButtonScript.Rect);


    Route_Tile.Clear();
    foreach (var _tile in Route_Temp) Route_Tile.Add(_tile);
    for(int i=1;i<Route_Tile.Count;i++)
    {
      SetOutline(Outline_Routes[i], Route_Tile[i].ButtonScript.Rect);
    }

    SelectedTile = selectedtiledata;

    TilePreviewRect.anchoredPosition = TilePreviewDownPos;
    TilePreviewGroup.alpha = TilePreviewStartAlpha;
    if (!IsMad)
    {
      TilePreview_Bottom.sprite = SelectedTile.ButtonScript.BottomImage.sprite;
      TilePreview_Bottom.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -60.0f * SelectedTile.Rotation));
      TilePreview_Top.sprite = SelectedTile.ButtonScript.TopImage.sprite;
      TilePreview_Landmark.sprite = SelectedTile.ButtonScript.LandmarkImage.sprite;
    }
    StopAllCoroutines();
    StartCoroutine(UIManager.Instance.moverect(TilePreviewRect, TilePreviewDownPos, new Vector2(-235.0f,57.0f), 0.5f, UIManager.Instance.UIPanelOpenCurve));
    StartCoroutine(UIManager.Instance.ChangeAlpha(TilePreviewGroup, TilePreviewStartAlpha, 1.0f, 0.5f));

    if (SelectedTile.TileSettle != null)
    {
      TileInfoText.text = IsMad ? GameManager.Instance.GetTextData("Madness_Wild_Description") : GameManager.Instance.GetTextData("MoveDescription_Settlement");
    }
    else
    {
      TileInfoText.text = IsMad ? GameManager.Instance.GetTextData("Madness_Wild_Description") : GameManager.Instance.GetTextData("MoveDescription_Outer");
    }

    switch (GameManager.Instance.MyGameData.QuestType)
    {
      case QuestType.Cult:
        string _progresstext = "";
        QuestInfo = CheckRitual;
        switch (QuestInfo)
        {
          case false:
            _progresstext = "";
            UIManager.Instance.SidePanelCultUI.SetRitualEffect(false);
            break;
          case true:
            _progresstext += string.Format(GameManager.Instance.GetTextData("Cult_Progress_Ritual_Effect") ,ConstValues.Quest_Cult_Progress_Ritual);
            UIManager.Instance.SidePanelCultUI.SetRitualEffect(true);
            break;
        }
        TileInfoText.text += _progresstext;
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
    
    SanityCost = GameManager.Instance.MyGameData.GetMoveSanityCost(Route_Tile.Count, MovePointCost);
    GoldCost = GameManager.Instance.MyGameData.GetMoveGoldCost(Route_Tile.Count, MovePointCost);
    BonusGold = (SelectedTile.MovePoint) * ConstValues.GoldPerMovepoint;
   // BonusGold = SelectedTile.MovePoint > 1 ? (SelectedTile.MovePoint) * ConstValues.GoldPerMovepoint+1 : ConstValues.DefaultBonusGold;
    BonusGold =(int)(BonusGold* GameManager.Instance.MyGameData.GetGoldGenModify(true));
    BonusGold += (Route_Tile.Count>2&&GameManager.Instance.MyGameData.Tendency_Head.Level == 1) ? ConstValues.Tendency_Head_p1 : 0;

    SelectedCostType = StatusTypeEnum.HP;
    MoveCostText.text = "";

    SanitybuttonGroup.interactable = true;
    SanityButton_Highlight.Interactive = true;
    SanityButton_Highlight.SetInfo(HighlightEffectEnum.Sanity, -1 * SanityCost);
    SanityButton_Highlight.SetInfo(HighlightEffectEnum.Movepoint, -1 * MovePointCost);

    bool _goldable = GameManager.Instance.MyGameData.Gold >= GoldCost;
    GoldbuttonGroup.interactable = _goldable;
    GoldButton_Highlight.Interactive = _goldable;
    GoldButton_Highlight.SetInfo(HighlightEffectEnum.Gold, -1 * GoldCost);
    GoldButton_Highlight.SetInfo(HighlightEffectEnum.Movepoint, -1 * MovePointCost);
    GoldbuttonGroup.alpha = _goldable ? 1.0f : 0.4f;

    string[] _bonusgoldtext = null;
    if (!IsMad)
    switch (SelectedTile.MovePoint)
    {
      case 1:
        _bonusgoldtext = GameManager.Instance.GetTextData("BonusGold_1").Split('@');
        break;
      case 2:
        _bonusgoldtext = GameManager.Instance.GetTextData("BonusGold_2").Split('@');
        break;
      case 3:
        _bonusgoldtext = GameManager.Instance.GetTextData("BonusGold_3").Split('@');
        break;
      default:
        _bonusgoldtext = GameManager.Instance.GetTextData("BonusGold_over").Split('@');
        break;
    }
    BonusGoldText.text =IsMad? GameManager.Instance.GetTextData(StatusTypeEnum.Gold, 2) + " +?":
      ((Route_Tile.Count>2 && GameManager.Instance.MyGameData.Tendency_Head.Level == 1) ? "<sprite=104>":"") + _bonusgoldtext[Random.Range(0,_bonusgoldtext.Length-1)]+" "+ GameManager.Instance.GetTextData(StatusTypeEnum.Gold, 2)+" +" + BonusGold.ToString();
    MoveLengthText.text = IsMad ? "<sprite=100> ?" :
      string.Format(GameManager.Instance.GetTextData("MoveLength"),
      GameManager.Instance.MyGameData.MovePoint,
      WNCText.PercentageColor(MovePointCost.ToString(), MovePointCost <= GameManager.Instance.MyGameData.MovePoint ? 1.0f : 0.0f));
    if (GameManager.Instance.MyGameData.MovePoint < 0)
    {
      MoveLengthCostText.text = GameManager.Instance.GetTextData("Movepoint_NoSupplies");
    }
    else if(MovePointCost > GameManager.Instance.MyGameData.MovePoint)
    {
      MoveLengthCostText.text = string.Format(GameManager.Instance.GetTextData("LackofMovepoint"),
     MovePointCost - GameManager.Instance.MyGameData.MovePoint);
    }
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
          MovePointCost <= GameManager.Instance.MyGameData.MovePoint?"" : string.Format(GameManager.Instance.GetTextData("AmplifiedValues"),
        MovePointCost - GameManager.Instance.MyGameData.MovePoint, (GameManager.Instance.MyGameData.Tendency_Head.Level == -1 ? "<sprite=103>" : "") + (int)(GameManager.Instance.MyGameData.MovePointAmplified * 100)),
        !IsMad ?SanityCost:"?");
        break;
      case StatusTypeEnum.Gold:
        if (GameManager.Instance.MyGameData.Gold < GoldCost) return;

        SelectedCostType = StatusTypeEnum.Gold;

        _costtext = string.Format(GameManager.Instance.GetTextData("MAPCOSTTYPE_GOLD"),
                    MovePointCost <= GameManager.Instance.MyGameData.MovePoint ? "" : string.Format(GameManager.Instance.GetTextData("AmplifiedValues"),
        MovePointCost - GameManager.Instance.MyGameData.MovePoint, (GameManager.Instance.MyGameData.Tendency_Head.Level == -1 ? "<sprite=103>" : "") + (int)(GameManager.Instance.MyGameData.MovePointAmplified * 100)),
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
    if (GameManager.Instance.MyGameData.Madness_Wild)
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

        Route_Dir = MapData.GetLength(GameManager.Instance.MyGameData.CurrentTile, SelectedTile);
        Route_Tile = new List<TileData>();
        Route_Tile.Add(GameManager.Instance.MyGameData.CurrentTile);
        for (int i = 0; i < Route_Dir.Count; i++)
        {
          Route_Tile.Add(GameManager.Instance.MyGameData.MyMapData.GetNextTile(Route_Tile[i], Route_Dir[i]));
        }
        QuestInfo = CheckRitual;
      }
      GameManager.Instance.MyGameData.TotalMoveCount++;
    }

    Dictionary<TileData, int> _movepointicondata = new Dictionary<TileData, int>();
    int _totalmp = 0;
    for(int i = 1; i < Route_Tile.Count; i++)
    {
      int _mp = Route_Tile[i].MovePoint;
      _totalmp += _mp;
      if (GameManager.Instance.MyGameData.MovePoint >= _totalmp)
      {
        _movepointicondata.Add(Route_Tile[i],_mp);
      }
      else
      {
        _movepointicondata.Add(Route_Tile[i],Mathf.Clamp(GameManager.Instance.MyGameData.MovePoint - (_totalmp - _mp),0,100));
      }
    }
    yield return StartCoroutine(UIManager.Instance.SetIconEffect_movepoint_using(_movepointicondata,SelectedCostType));

 //   yield return StartCoroutine(UIManager.Instance.statusgainanimation(PlayerRect));

    bool _iswalking = false;
    if (GameManager.Instance.MyGameData.MovePoint >= MovePointCost)
      _iswalking = true;
    else _iswalking = false;


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


    bool _enterritual = false;
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

      if (!_enterritual && Route_Tile[_currentindex].Landmark == LandmarkType.Ritual)
      {
        UIManager.Instance.CultUI.AddProgress(4);
        _enterritual = true;
      }
      _time += Time.deltaTime;
      yield return null;
    }
    GameManager.Instance.MyGameData.Gold += BonusGold;
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

        GameManager.Instance.MyGameData.Turn++;
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
