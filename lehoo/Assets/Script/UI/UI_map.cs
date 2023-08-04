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
  [SerializeField] private Tilemap Tilemap_bottom = null;
  [SerializeField] private Tilemap Tilemap_top = null;
    [SerializeField] private Button MoveButton = null;
  [SerializeField] private TextMeshProUGUI MoveButtonText = null;
  [SerializeField] private Button CancleButton = null;
  [SerializeField] private TextMeshProUGUI CancleButtonText = null;
  private TileData SelectedTile = null;
  /// <summary>
  /// 0: 정착지 선택 전 1: 이동가능 2: 이동중이라 클릭못함 3:이동불가 4:계속 이동
  /// </summary>
  /// <param name="state"></param>
  public void SetMoveButton(int state)
  {
    switch (state)
    {
      case 0:
        Debug.Log("정착지 선택 상태");
        if (MoveButton.interactable.Equals(true)) MoveButton.interactable = false;
        if(CancleButton.interactable.Equals(true)) CancleButton.interactable = false;
        MoveButtonText.text = GameManager.Instance.GetTextData("choosesettle");      //지도 상태 문구 기획 완료 후 변경 요망
        CancleButton.interactable = false;
        break;
      case 1:
        Debug.Log("이동 가능 상태");
        if (MoveButton.interactable.Equals(false)) MoveButton.interactable = true;
        if (CancleButton.interactable.Equals(false)) CancleButton.interactable = true;
        MoveButtonText.text = GameManager.Instance.GetTextData("canmove");
        CancleButton.interactable = true;
        break;
      case 2:
        Debug.Log("이동 중");
        if (MoveButton.interactable.Equals(true)) MoveButton.interactable = false;
        if (CancleButton.interactable.Equals(true)) CancleButton.interactable = false;
        MoveButtonText.text = GameManager.Instance.GetTextData("duringmoving");
        CancleButton.interactable = false;
        break;
      case 3:
        Debug.Log("이동 불가 상태");
        if (MoveButton.interactable.Equals(true)) MoveButton.interactable = false;
        if (CancleButton.interactable.Equals(true)) CancleButton.interactable = false;
        MoveButtonText.text = GameManager.Instance.GetTextData("cannotmove");
        CancleButton.interactable = false;
        break;
      case 4:
        Debug.Log("계속 이동 중인 상태");
        if (MoveButton.interactable.Equals(false)) MoveButton.interactable = true;
        if (CancleButton.interactable.Equals(true)) CancleButton.interactable = false;
        MoveButtonText.text = GameManager.Instance.GetTextData("keepgoing");
        CancleButton.interactable = false;
        break;
    }
  }
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

  [SerializeField] private Image CurrentQuestProgress = null;
  [SerializeField] private TextMeshProUGUI CurrentQuestDescription = null;
  private int SelectedSettleCost = 0;
  [SerializeField] private AnimationCurve ZoomInCurve = null;
  [SerializeField] private AnimationCurve ZoomOutCurve = null;
  [SerializeField] private RectTransform ScaleChanger = null;
  [SerializeField] private Vector3 IdleScale = Vector3.one * 1.0f;
  [SerializeField] private Vector3 ZoomInScale = Vector3.one* 1.5f;
  [SerializeField] private float ZoomInTime = 1.2f;
  [SerializeField] private float ZoomOutTime = 0.4f;

  [SerializeField] private AnimationCurve SettleMoveCurve = null;
  [SerializeField] private float SettleMoveTime = 1.4f;
  [SerializeField] private AnimationCurve FollowMoveCurve = null;
  [SerializeField] private AnimationCurve CancleMoveCurve = null;
  [SerializeField] private float CancleMoveTime = 0.5f;

  public Transform SelectTileHolder = null;
  public void OpenUI()
  {
    if (IsOpen) { CloseUI(); IsOpen = false; return; }
    IsOpen = true;
    UIManager.Instance.AddUIQueue(openui());
  }
  private IEnumerator openui()
  {
    ScaleChanger.anchoredPosition = GetScaleChangerPos(PlayerRect.anchoredPosition);

    //                                                                                                        퀘스트 기획 후 지도에 퀘스트 정보 표기 넣기


    if (GameManager.Instance.MyGameData.CanMove)  //이동 가능한 상황(야외에서 이벤트를 완료한 상황, 정착지에서 이벤트를 완료한 상황)
    {
      if (GameManager.Instance.MyGameData.CurrentSettlement != null)//현재 정착지가 공백이 아니다? 정착지에서 출발하는 상황
      {
        SetMoveButton(0);
      }
      else SetMoveButton(4);                                        //현재 정착지가 없다? 야외 이벤트를 끝내고 계속 이동해야 하는 상황
    }
    else  //이동 불가능한 상황(야외 이벤트 진행중 혹은 정착지 이벤트 진행중)
    {
      SetMoveButton(3);
    }
    if (ScaleChanger.localScale.Equals(ZoomInScale)) StartCoroutine(zoomoutview());

    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(MyGroup,1.0f,0.4f, false));
  }
  public override void CloseUI()
  {
    UIManager.Instance.AddUIQueue(closeui());
    IsOpen = false;
  }
  private IEnumerator closeui()
  {
   yield return StartCoroutine(UIManager.Instance.ChangeAlpha(MyGroup,0.0f,0.2f, false));
  }
  private IEnumerator tilemapalpha(float _startalpha,float _endalpha)
  {
    Color _color = Color.white;
    _color.a = _startalpha;
    Tilemap_bottom.color = _color;
    Tilemap_top.color= _color;
    float _time = 0.0f;
    float _maxtime = UIManager.Instance.LargePanelFadeTime;
    while (_time < _maxtime)
    {
      _color.a = Mathf.Lerp(_startalpha,_endalpha,Mathf.Pow(_time/_maxtime,0.5f));
      Tilemap_bottom.color = _color;
      Tilemap_top.color = _color;
      _time += Time.deltaTime;
      yield return null;
    }
    _color.a = _endalpha;
    Tilemap_bottom.color = _color;
    Tilemap_top.color = _color;
  }

  public void SelectTile(TileData tiledata)
  {
    if (SelectedTile != null) SelectedTile.ButtonScript.CancleTile();
    SelectedTile = tiledata;
  }
  public void CancleTile()
  {
    SelectedTile = null;
  }

  private IEnumerator updatepanel(TileData targettile)
  {
    if (targettile == null)
    {
      UIManager.Instance.IsWorking = false;
      Cancle();
      //시점 플레이어로 돌아가고 취소, 이동 버튼 비활성화
    }//정착지 선택 취소
    else if (SelectedTile != null&&targettile.TileSettle!=null)
    {

      //   SelectedSettleCost = GameManager.Instance.MyGameData.GetMoveSanityValue(Vector2.Distance(GameManager.Instance.MyGameData.CurrentPos, SelectedSettle.Coordinate));
      Vector2 _targetrecrpos = targettile.Rect.anchoredPosition;

      yield return StartCoroutine(movecameratosettle(GetScaleChangerPos(_targetrecrpos)));
      SetMoveButton(1);
    }//방향 버튼을 눌러 카메라가 이동한 상태에서 다른 방향 버튼을 누른 경우
  }
  public void Cancle()
  {
    if (UIManager.Instance.IsWorking) return;
    UIManager.Instance.AddUIQueue(cancle());
  }
  private IEnumerator cancle()
  {
    Debug.Log("방향 버튼 취소");
    SetMoveButton(0);
    yield return StartCoroutine(movetoplayer());
  }
  private Vector2 GetScaleChangerPos(Vector2 pos)
  {
    return new Vector2(pos.x*ScaleChanger.localScale.x*-1.0f,pos.y*ScaleChanger.localScale.y*-1.0f);
  }
  private IEnumerator movecameratosettle(Vector2 settlepos)
  {
    float _time = 0.0f, _targettime = SettleMoveTime;
    Vector2 _startpos = ScaleChanger.anchoredPosition;
    while (_time < _targettime)
    {
      ScaleChanger.anchoredPosition = Vector2.Lerp(_startpos, settlepos, SettleMoveCurve.Evaluate(_time / _targettime));
      _time += Time.deltaTime;
      yield return null;
    }
    ScaleChanger.anchoredPosition = settlepos;
  }//플레이어->정착지로 시점 이동(화살표 클릭했을 때)
  private IEnumerator movetoplayer()
  {
    float _time = 0.0f, _targettime = CancleMoveTime;
    Vector2 _startpos = ScaleChanger.anchoredPosition;
    Vector2 _endpos = GetScaleChangerPos(PlayerRect.anchoredPosition);
    while (_time < _targettime)
    {
      ScaleChanger.anchoredPosition = Vector2.Lerp(_startpos, _endpos, CancleMoveCurve.Evaluate(_time / _targettime));
      _time += Time.deltaTime;
      yield return null;
    }
    ScaleChanger.anchoredPosition = _endpos;
    yield return null;
  }//정착지->플레이어로 시점 이동(이동 시작할 때, 이동 취소했을 때)
  public void KeepMove()
  {
        if (UIManager.Instance.IsWorking) return;
        if (!GameManager.Instance.MyGameData.CanMove) return;
    UIManager.Instance.AddUIQueue(keepmove());
    UIManager.Instance.PreviewManager.ClosePreview();
  }
  private IEnumerator keepmove()
  {
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(MyGroup, 1.0f, 0.4f, false));
    MoveButton.interactable = false;
        UIManager.Instance.ResetEventPanels();
    yield return StartCoroutine(zoomoutview());
    yield return new WaitForSeconds(0.1f);
    yield return StartCoroutine(movemap());
        yield return null;
  }
  public void MoveMap()
  {
    if (UIManager.Instance.IsWorking) return;
    //다른 UI가 움직이고 있으면 작동 안함

    if(GameManager.Instance.MyGameData.CanMove==false) return;
    //움직일 수 없는 상황(이벤트 선택 화면,이벤트 도중)이라면 못 움직인다는 UI 출력
    if (SelectedTile == null) return;
    //움직일 수 있는 상황에서, 정착지에서 출발할 때 목표 정착지를 선택하지 않은 경우 목표 방향이 없다는 UI 출력
    UIManager.Instance.ResetEventPanels();
    UIManager.Instance.AddUIQueue(movemap());
  }//이동 버튼을 눌렀으니 해당 정착지, 해당 화살표를 제외한 다른 정착지 버튼 비활성화 및 다른 화살표 비활성화
  private IEnumerator movemap()
  {
    yield return StartCoroutine(movetoplayer());
   // 타겟된 정착지에서 플레이어로 카메라 이동

    //이동 도중이니 이동 버튼은 비활성화
    SetMoveButton(2);
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
      ScaleChanger.anchoredPosition = GetScaleChangerPos(Vector2.Lerp(_playerrectpos, _targettilepos, FollowMoveCurve.Evaluate(_time / MoveTime)));
      _time += Time.deltaTime;
      yield return null;
    }
    PlayerRect.anchoredPosition = _targettilepos;
    ScaleChanger.anchoredPosition = GetScaleChangerPos(_targettilepos);
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
    StartCoroutine(UIManager.Instance.CloseUI(MyGroup, true, false));
        //캐릭터 이동시킴
        //이동+줌인+종료

    Debug.Log("이동 코루틴이 끝난 레후~");
  }
  private IEnumerator zoominview(Vector2 targetpos)
  {
    float _time = 0.0f, _targettime = ZoomInTime;
    Vector3 _startscale = IdleScale, _endscale = ZoomInScale;
    Vector2 _startpos = Vector3.zero, _endpos = targetpos;
    float _degree = 0.0f;
    while (_time < _targettime)
    {
      _degree = ZoomInCurve.Evaluate(_time / _targettime);
      ScaleChanger.localScale = Vector3.Lerp(_startscale, _endscale, _degree);
      _time += Time.deltaTime;
      yield return null;
    }
    ScaleChanger.localScale = _endscale;
  }//정착지,야외 이동 후 지도 줌인 하는 코루틴
  private IEnumerator movecharacter(Vector3 _currenrecttpos, Vector3 _endrectpos, float _currentprogress)
  {
    StartCoroutine(zoominview(_endrectpos));
    //출발 좌표, 끝 좌표랑 현재 진행도(_progress)를 사용해 0.1칸 단위를 만들어 10배를 곱해
    //_progress 0.0f 기준(시작점)을 도출한다
    float _time =0.0f;
    //시작 _time은 0.0~MoveTime의 _currentprogress만큼
    while (_time < MoveTime)
    {
      PlayerRect.anchoredPosition = Vector3.Lerp(_currenrecttpos, _endrectpos, UIManager.Instance.CharacterMoveCurve.Evaluate(_time / MoveTime));
      ScaleChanger.anchoredPosition = GetScaleChangerPos(Vector2.Lerp(_currenrecttpos, _endrectpos, FollowMoveCurve.Evaluate(_time / MoveTime)));
      _time += Time.deltaTime; yield return null;
    }
    PlayerRect.anchoredPosition = _endrectpos;
    ScaleChanger.anchoredPosition = GetScaleChangerPos(_endrectpos);
    //targetprogress ~ 1.0f 비율까지 이동

  }//캐릭터 움직이는 코루틴 - 야외 이벤트 ~ 정착지
  private IEnumerator zoomoutview()
  {
    float _time = 0.0f, _targettime = ZoomOutTime;
    Vector3 _startscale = ZoomInScale, _endscale = IdleScale;
    Vector2 _startpos = ScaleChanger.anchoredPosition, _endpos = Vector3.zero;
    float _degree = 0.0f;
    while (_time < _targettime)
    {
      _degree = ZoomOutCurve.Evaluate(_time / _targettime);
      ScaleChanger.localScale = Vector3.Lerp(_startscale, _endscale, _degree);
      _time += Time.deltaTime;
      yield return null;
    }
    ScaleChanger.localScale = _endscale;
  }//지도 켰을때 줌 인 된 상태라면 줌아웃 시키는 코루틴
  public void UpdateIcons(List<Settlement> _settles)
  {
    foreach (Settlement _settle in _settles)
    {
    }
  }
  public void SetPlayerPos(Vector2 coordinate)
  {
    ScaleChanger.localScale = Vector3.one;
    TileData _targettile = GameManager.Instance.MyGameData.MyMapData.Tile(coordinate);
    PlayerRect.anchoredPosition = _targettile.Rect.anchoredPosition;
    ScaleChanger.localScale =IdleScale;
  }
  public void SetPlayerPos(Vector3 _tilepos)
  {
    ScaleChanger.localScale = Vector3.one;
    Vector3Int _intpos_lower =new Vector3Int(Mathf.FloorToInt(_tilepos.x), Mathf.FloorToInt(_tilepos.y));
    Vector3Int _intpos_upper = new Vector3Int(Mathf.CeilToInt(_tilepos.x), Mathf.FloorToInt(_tilepos.y));

    PlayerRect.position = MapCreater.Tilemap_bottom.CellToWorld(_intpos_lower);
    Vector2 _loweranchorpos = PlayerRect.anchoredPosition;
    PlayerRect.position = MapCreater.Tilemap_bottom.CellToWorld(_intpos_upper);
    Vector2 _upperanchorpos=PlayerRect.anchoredPosition;
    float _length =Vector3.Distance(_intpos_lower,_tilepos)/ Vector3Int.Distance(_intpos_lower, _intpos_upper);

    PlayerRect.position = Vector3.Lerp(_loweranchorpos, _upperanchorpos, _length);
    return;

    /*
    Transform _originparent = PlayerRect.parent;
    PlayerRect.SetParent(null);

    PlayerRect.anchoredPosition= MapCreater.Tilemap_bottom.CellToWorld(Vector3Int.zero);
    Vector3 _zeropos = new Vector3(PlayerRect.anchoredPosition.x, PlayerRect.anchoredPosition.y);
    PlayerRect.anchoredPosition = MapCreater.Tilemap_bottom.CellToWorld(Vector3Int.one);
    Vector3 _onepos= new Vector3(PlayerRect.anchoredPosition.x, PlayerRect.anchoredPosition.y);

    Vector3 _unit = _onepos - _zeropos; //(0,0),(1,1)을 UI로 옮긴 좌표를 비교해 UI 상 거리 1의 Rect 단위를 산출
    Vector3Int _intpos = new Vector3Int(Mathf.FloorToInt(_tilepos.x), Mathf.FloorToInt(_tilepos.y));
    Vector3 _else=new Vector3(_tilepos.x-_intpos.x,_tilepos.y-_intpos.y);
    //타일 좌표를 정수, 나머지로 분리

    PlayerRect.position = MapCreater.Tilemap_bottom.CellToWorld(_intpos);
    PlayerRect.position = PlayerRect.position + new Vector3(_unit.x * _else.x, _unit.y * _else.y);
    PlayerRect.SetParent(_originparent);
    //정수 값으로 산출한 CelltoWorld랑 미리 값을 구해놓은 실수형 값을 더해 이동
    ScaleChanger.localScale = IdleScale;*/
  }//정수가 아닌 좌표를 받아서 거기로 옮기는거
}
