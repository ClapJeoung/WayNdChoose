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
  [HideInInspector] public GameObject CastleIcon = null;
  [HideInInspector] public List<GameObject> CityIcons = new List<GameObject>();
  [HideInInspector] public List<GameObject> TownIcons = new List<GameObject>();
  public GameObject GetSettleIcon(Settlement settlement)
  {
    string _originname = settlement.OriginName;
    switch (settlement.Type)
    {
      case SettlementType.Castle:return CastleIcon;
      case SettlementType.City:
        foreach (GameObject city in CityIcons)
          if (city.name.Contains(_originname)) return city;
        return null;
      case SettlementType.Town:
        foreach(GameObject town in TownIcons)
          if(town.name.Contains(_originname)) return town;
        return null;
    }
    Debug.Log("뭔가 이상한 레후~");
    return null;
  }
  public float AppearTime = 0.5f;
  private int SelectedSettleCost = 0;
  [SerializeField] private AnimationCurve ZoomInCurve = null;
  [SerializeField] private AnimationCurve ZoomOutCurve = null;
  [SerializeField] private RectTransform HolderRect = null;
  [SerializeField] private RectTransform ScaleRect = null;
  private Vector3 IdleScale = Vector3.one;
  [SerializeField] private Vector3 ZoomInScale = Vector3.one* 1.5f;
  [SerializeField] private float ZoomInTime = 1.2f;
  [SerializeField] private float ZoomOutTime = 0.4f;

  [SerializeField] private TextMeshProUGUI ClickInfoText = null;

  [SerializeField] private GameObject MoveInfoHolder = null;
  [SerializeField] private Image MovePointImage = null;
  [SerializeField] private TextMeshProUGUI MovePointText = null;
  [SerializeField] private TextMeshProUGUI SanityCostText = null;

  [SerializeField] private RectTransform SettleInfoRect = null;
  [SerializeField] private CanvasGroup SettleInfoGroup = null;
  [SerializeField] private TextMeshProUGUI SettleNameText = null;
  [SerializeField] private TextMeshProUGUI DiscomfortCountText = null;
  public Transform SelectTileHolder = null;

  public Color NormalColor = Color.white;
  public Color DisableColor = Color.grey;
  private List<Settlement> ActiveSettles = new List<Settlement>();
  private List<TileData> ActiveTileData = new List<TileData>();
  private void ResetEnableTiles()
  {
    List<TileData> _currents = GameManager.Instance.MyGameData.MyMapData.GetAroundTile(GameManager.Instance.MyGameData.Coordinate, 2);
    List<Settlement> _currentsettles = new List<Settlement>();
    foreach (TileData _tile in _currents) //주위 2칸 타일 전부 가져오기
    {
      if (!ActiveTileData.Contains(_tile))
      {
        ActiveTileData.Add(_tile);
        _tile.ButtonScript.Button.interactable = true;
        if (_tile.ButtonScript.TopEnvirImage != null) _tile.ButtonScript.TopEnvirImage.color= NormalColor;
      }//기존 활성화 타일 목록에 없던 타일이라면 리스트에 추가하고 활성화

      if (_tile.TileSettle != null)
      {
        _currentsettles.Add(_tile.TileSettle);
        if (!ActiveSettles.Contains(_tile.TileSettle))
        {
          ActiveSettles.Add(_tile.TileSettle);
          StartCoroutine(UIManager.Instance.ChangeAlpha(GetSettleIcon(_tile.TileSettle).GetComponent<CanvasGroup>(), 1.0f, 0.8f,false));
        }
      }//기존 활성화 정착지 목록에 없던 타일이라면 리스트에 추가하고 활성화
    }

    List<TileData> _removetiles = new List<TileData>();
    List<Settlement> _removesettles = new List<Settlement>();

    foreach (TileData _tile in ActiveTileData)
    {
      if (!_currents.Contains(_tile))
      {
        _removetiles.Add(_tile);
         _tile.ButtonScript.Button.interactable = false;
        if (_tile.ButtonScript.TopEnvirImage != null) _tile.ButtonScript.TopEnvirImage.color = DisableColor;
      }//화면에서 사라진 타일들은 비활성화 상태로 전환
    }

    foreach (Settlement _settle in ActiveSettles)
    {
      if (!_currentsettles.Contains(_settle))
      {
        _removesettles.Add(_settle);
        StartCoroutine(UIManager.Instance.ChangeAlpha(GetSettleIcon(_settle).GetComponent<CanvasGroup>(), 0.0f, 0.3f, false));
      }//화면에서 사라진 정착지들은 투명화
    }

    foreach(TileData _tile in _removetiles)ActiveTileData.Remove(_tile);
    foreach(Settlement _settle in _removesettles)ActiveSettles.Remove(_settle);
  }
  
  private void Start()
  {
  }
  public void OpenUI()
  {
    if (IsOpen) { CloseUI(); IsOpen = false; return; }
    IsOpen = true;
    UIManager.Instance.AddUIQueue(openui());
  }
  
  private IEnumerator openui()
  {
    MoveInfoHolder.SetActive(false);
    MovePointImage.sprite=GameManager.Instance.MyGameData.MovePoint>0?GameManager.Instance.ImageHolder.MovePointIcon_Enable:GameManager.Instance.ImageHolder.MovePointIcon_Lack;
    MovePointText.text = GameManager.Instance.MyGameData.MovePoint > 0 ? GameManager.Instance.GetTextData("ENOUGHMOVEPOINT") : GameManager.Instance.GetTextData("NOTENOUGHMOVEPOINT");
    HolderRect.anchoredPosition = Vector2.zero;
    if (DefaultGroup.alpha == 1.0f) DefaultGroup.alpha = 0.0f;
    if (DefaultGroup.interactable == true) DefaultGroup.interactable = false;
    if (DefaultGroup.blocksRaycasts == true) DefaultGroup.blocksRaycasts = false;
    //                                                                                                        퀘스트 기획 후 지도에 퀘스트 정보 표기 넣기

    ClickInfoText.text = GameManager.Instance.GetTextData("CHOOSETILE");
    if (ScaleRect.localScale != Vector3.one) StartCoroutine(zoomoutview());

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
  public override void CloseUI()
  {
    UIManager.Instance.AddUIQueue(closeui());
    IsOpen = false;
  }
  private IEnumerator closeui()
  {
    DefaultGroup.blocksRaycasts = false;
    yield return StartCoroutine(UIManager.Instance.moverect(GetPanelRect("myrect").Rect, GetPanelRect("myrect").InsidePos, GetPanelRect("myrect").OutisdePos, 0.3f, UIManager.Instance.UIPanelCLoseCurve));
    DefaultGroup.interactable = false;
  }

  public void SelectTile(TileData tiledata,Vector2 position)
  {
    if (SelectedTile != null) SelectedTile.ButtonScript.CancleTile();
    SelectedTile = tiledata;
    ClickInfoText.text = GameManager.Instance.GetTextData("CHOSSETILEAGAIN");

    TileData _currenttile = GameManager.Instance.MyGameData.MyMapData.Tile(GameManager.Instance.MyGameData.Coordinate);
    int _length = GameManager.Instance.MyGameData.MyMapData.GetLength(_currenttile, SelectedTile);
    SelectedSettleCost = GameManager.Instance.MyGameData.GetMoveSanityCost(_length);

    SanityCostText.text = GameManager.Instance.MyGameData.MovePoint > 0 ? string.Format(GameManager.Instance.GetTextData("MOVESANITYCOST_ENOUGH"), "<#FFBF00>-1</color>", WNCText.GetSanityColor("-"+SelectedSettleCost)) :
    string.Format(GameManager.Instance.GetTextData("MOVESANITYCOST_ENOUGH"), "<#FFBF00>-1</color>", WNCText.GetSanityColor("-"+SelectedSettleCost));
    if (MoveInfoHolder.activeInHierarchy == false) MoveInfoHolder.SetActive(true);

    if (SelectedTile.TileSettle != null)
    {
      SettleInfoRect.position = position;
      SettleInfoRect.anchoredPosition3D = new Vector3(SettleInfoRect.anchoredPosition.x, SettleInfoRect.anchoredPosition.y, 0.0f);
      SettleInfoGroup.alpha = 1.0f;

      SettleNameText.text = _currenttile.TileSettle.Name;
      DiscomfortCountText.text = "<#5F04B4>" + _currenttile.TileSettle.Discomfort.ToString() + "</color>";
    }
    else
    {
      SettleInfoGroup.alpha = 0.0f;
    }
  }
  public void CancleTile()
  {
    SelectedTile = null;
    SettleInfoGroup.alpha = 0.0f;
  }
  public void MoveMap()
  {
    if (UIManager.Instance.IsWorking) return;
    //다른 UI가 움직이고 있으면 작동 안함
    SettleInfoGroup.alpha = 0.0f;
    DefaultGroup.blocksRaycasts = true;
    UIManager.Instance.ResetEventPanels();
    UIManager.Instance.AddUIQueue(movemap());
  }//이동 버튼을 눌렀으니 해당 정착지, 해당 화살표를 제외한 다른 정착지 버튼 비활성화 및 다른 화살표 비활성화
  private IEnumerator movemap()
  {

    Vector2 _playerrectpos = PlayerRect.anchoredPosition;                 //현재 위치(Rect)
    Vector2 _targettilepos = SelectedTile.Rect.anchoredPosition;            //종점 위치(Rect)

    Vector2 _startcoor = GameManager.Instance.MyGameData.Coordinate;//현재 위치(타일)
    Vector2 _endcoor = SelectedTile.Coordinate;//종점 위치(타일)

    GameManager.Instance.MyGameData.ClearBeforeEvents();
    GameManager.Instance.MyGameData.CurrentSanity -= SelectedSettleCost;

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
    UIManager.Instance.UpdateSanityText();

    GameManager.Instance.MyGameData.Coordinate = _endcoor;
    ResetEnableTiles();

    switch (SelectedTile.LandScape)
    {
      case LandscapeType.Outer:
        GameManager.Instance.MyGameData.CurrentSettlement = null;
        yield return new WaitForSeconds(1.0f);


        EventManager.Instance.SetOutsideEvent(GameManager.Instance.MyGameData.MyMapData.GetTileData(SelectedTile.Coordinate));
        //캐릭터 멈춘 위치 주위 1칸 강,숲,언덕,산,바다 유무 파악해서 EventManager에 던져줌
        IsOpen = false;
        //도중에 멈춘거니까 이동 버튼 활성화는 안함

        break;

      case LandscapeType.Settlement:
        GameManager.Instance.MyGameData.CurrentEvent = null;
        GameManager.Instance.MyGameData.CurrentSettlement = SelectedTile.TileSettle;
        GameManager.Instance.MyGameData.CurrentSettlement.SetAvailablePlaces();
        GameManager.Instance.MyGameData.CurrentSettlement.LibraryType = (SkillType)Random.Range(0, 4);

        yield return new WaitForSeconds(1.0f);

        EventManager.Instance.SetSettleEvent(GameManager.Instance.MyGameData.CurrentSettlement.TileInfoData);
        break;
    }

    GameManager.Instance.MyGameData.Turn++;
    UIManager.Instance.UpdateTurnIcon();

    StartCoroutine(zoominview());
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DefaultGroup, 0.0f, ZoomInTime + 1.0f, false));
    yield return new WaitForSeconds(0.1f);
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
  private IEnumerator zoomoutview()
  {
    float _time = 0.0f, _targettime = ZoomOutTime;
    Vector3 _startscale = ZoomInScale, _endscale = IdleScale;
    Vector2 _startposition = ScaleRect.anchoredPosition, _endposition = Vector2.zero;
 float _degree = 0.0f;
    while (_time < _targettime)
    {
      _degree = ZoomOutCurve.Evaluate(_time / _targettime);
      ScaleRect.localScale = Vector3.Lerp(_startscale, _endscale, _degree);
      ScaleRect.anchoredPosition = Vector2.Lerp(_startposition, _endposition, _degree);
      _time += Time.deltaTime;
      yield return null;
    }
    ScaleRect.localScale = _endscale;
    ScaleRect.anchoredPosition = _endposition;
  }//지도 켰을때 줌 인 된 상태라면 줌아웃 시키는 코루틴
  public void UpdateIcons(List<Settlement> _settles)
  {
    foreach (Settlement _settle in _settles)
    {
    }
  }
  public void SetPlayerPos(Vector2 coordinate)
  {
    Debug.Log("시작 플레이어 위치 설정");
    TileData _targettile = GameManager.Instance.MyGameData.MyMapData.Tile(coordinate);
    PlayerRect.anchoredPosition = _targettile.Rect.anchoredPosition;
    ScaleRect.localScale =IdleScale;
    HolderRect.anchoredPosition = PlayerRect.anchoredPosition * -1.0f;
    GameManager.Instance.MyGameData.Coordinate = coordinate;
    ResetEnableTiles();

  }
}
