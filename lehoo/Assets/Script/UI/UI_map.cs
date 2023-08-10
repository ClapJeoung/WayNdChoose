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
  public GameObject GetSettlementIcon(Settlement settle)
  {
    switch (settle.Type)
    {
      case SettlementType.Town:
        foreach (var obj in TownIcons)
          if (obj.name.Contains(settle.OriginName)) return obj;
        break;
      case SettlementType.City:
        foreach (var obj in CityIcons)
          if (obj.name.Contains(settle.OriginName)) return obj;
        break;
      case SettlementType.Castle:
        return CastleIcon;
    }
    Debug.Log(settle.Type);
    return null;
  }
  private int SelectedSettleCost = 0;
  [SerializeField] private AnimationCurve ZoomInCurve = null;
  [SerializeField] private AnimationCurve ZoomOutCurve = null;
  [SerializeField] private RectTransform HolderRect = null;
  [SerializeField] private RectTransform ScaleRect = null;
  private Vector3 IdleScale = Vector3.one;
  [SerializeField] private Vector3 ZoomInScale = Vector3.one* 1.5f;
  [SerializeField] private float ZoomInTime = 1.2f;
  [SerializeField] private float ZoomOutTime = 0.4f;
  [SerializeField] private RectTransform PreviewRect = null;
  [SerializeField] private CanvasGroup PreviewGroup = null;
  [SerializeField] private GameObject SettleInfoHolder = null;
  [SerializeField] private TextMeshProUGUI SettleNameText = null;
  [SerializeField] private TextMeshProUGUI DiscomfortCountText = null;
  [SerializeField] private TextMeshProUGUI SanityCostText = null;
  [SerializeField] private TextMeshProUGUI ClickToMove = null;
  public Transform SelectTileHolder = null;
  private void Start()
  {
    ClickToMove.text = GameManager.Instance.GetTextData("CLICKTOMOVE");
  }
  public void OpenUI()
  {
    if (IsOpen) { CloseUI(); IsOpen = false; return; }
    IsOpen = true;
    UIManager.Instance.AddUIQueue(openui());
  }
  public void OpenUI(UI_default currentui)
  {
    if (IsOpen) { CloseUI(); IsOpen = false; return; }
    IsOpen = true;
    currentui.CloseUI();
    UIManager.Instance.AddUIQueue(openui());
  }
  private IEnumerator openui()
  {
    HolderRect.anchoredPosition = PlayerRect.anchoredPosition*-1.0f;
    if (MyGroup.alpha == 0.0f) MyGroup.alpha = 1.0f;
    //                                                                                                        퀘스트 기획 후 지도에 퀘스트 정보 표기 넣기

    if(ScaleRect.localScale!=Vector3.one) StartCoroutine(zoomoutview());

    yield return StartCoroutine(UIManager.Instance.moverect(GetPanelRect("myrect").Rect, GetPanelRect("myrect").OutisdePos, GetPanelRect("myrect").InsidePos,0.6f, UIManager.Instance.UIPanelOpenCurve));
    MyGroup.interactable = true;
    MyGroup.blocksRaycasts = true;
  }
  public override void CloseUI()
  {
    UIManager.Instance.AddUIQueue(closeui());
    IsOpen = false;
  }
  private IEnumerator closeui()
  {
    MyGroup.blocksRaycasts = false;
    yield return StartCoroutine(UIManager.Instance.moverect(GetPanelRect("myrect").Rect, GetPanelRect("myrect").InsidePos, GetPanelRect("myrect").OutisdePos, 0.3f, UIManager.Instance.UIPanelCLoseCurve));
    MyGroup.interactable = false;
  }

  public void SelectTile(TileData tiledata,Vector2 position)
  {
    if (SelectedTile != null) SelectedTile.ButtonScript.CancleTile();
    SelectedTile = tiledata;

    TileData _currenttile = GameManager.Instance.MyGameData.MyMapData.Tile(GameManager.Instance.MyGameData.Coordinate);
    int _length = GameManager.Instance.MyGameData.MyMapData.GetLength(_currenttile, SelectedTile);
    SelectedSettleCost = GameManager.Instance.MyGameData.GetMoveSanityCost(_length);

    SanityCostText.text = string.Format(GameManager.Instance.GetTextData("MOVESANITYCOST"), SelectedSettleCost);
    if (SelectedTile.TileSettle != null)
    {
      SettleNameText.text = _currenttile.TileSettle.Name;
      DiscomfortCountText.text = "<#5F04B4>" + _currenttile.TileSettle.Discomfort.ToString() + "</color>";
      if (SettleInfoHolder.activeInHierarchy == false) SettleInfoHolder.SetActive(true);
    }
    else
    {
        if (SettleInfoHolder.activeInHierarchy == true) SettleInfoHolder.SetActive(false);
    }
    PreviewRect.position = position;
    PreviewRect.anchoredPosition3D = new Vector3(PreviewRect.anchoredPosition.x, PreviewRect.anchoredPosition.y, 0.0f);
    PreviewGroup.alpha = 1.0f;
  }
  public void CancleTile()
  {
    SelectedTile = null;
    PreviewGroup.alpha = 0.0f;
  }
  public void MoveMap()
  {
    if (UIManager.Instance.IsWorking) return;
    //다른 UI가 움직이고 있으면 작동 안함
    PreviewGroup.alpha = 0.0f;
    MyGroup.blocksRaycasts = true;
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
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(MyGroup, 0.0f, ZoomInTime + 1.0f, false));
    yield return new WaitForSeconds(0.1f);
    GetPanelRect("myrect").Rect.anchoredPosition = GetPanelRect("myrect").OutisdePos;
    MyGroup.interactable = false;
    MyGroup.blocksRaycasts = false;
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
  }
}
