using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;

public class UI_map : UI_default
{
  [SerializeField] private RectTransform PlayerRect = null;
  [SerializeField] private float MoveTime = 1.5f;
  [SerializeField] private Tilemap Tilemap_bottom = null;
  [SerializeField] private Tilemap Tilemap_top = null;
    [SerializeField] private TextMeshProUGUI SettleName = null;
    [SerializeField] private Image SettleIllust = null;
  [SerializeField] private TextMeshProUGUI SettleUnPleasant = null;
    [SerializeField] private Button MoveButton = null;
  [SerializeField] private maptext MapCreater = null;
  private Dictionary<string,SettlementIcon> SettleIcons = new Dictionary<string,SettlementIcon>();
  private Settlement SelectedSettle = null;

  public void AddSettleIcon(string _name, SettlementIcon _icon) => SettleIcons.Add(_name, _icon);
  public override void OpenUI(bool _islarge)
  {
    base.OpenUI(_islarge);
    if (GameManager.Instance.MyGameData.CanMove) MoveButton.interactable = true;
  }
  public override void CloseUI()
  {
    base.CloseUI();
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

    public void UpdatePanel(Settlement _settle)
    {
   if(SelectedSettle!=null)  SettleIcons[SelectedSettle.OriginName].Selected = false;
    SettleName.text = _settle.Name;
        SettleIllust.sprite = _settle.Illust;
    SettleUnPleasant.text = GameManager.Instance.MyGameData.AllSettleUnpleasant[_settle].ToString();
    MoveButton.interactable = true;
    SelectedSettle = _settle;
    SettleIcons[SelectedSettle.OriginName].Selected = true;
    }
  public void KeepMove()
  {
        if (UIManager.Instance.IsWorking) return;
        if (!GameManager.Instance.MyGameData.CanMove) return;
    base.OpenUI(true);
    UIManager.Instance.AddUIQueue(keepmove());
    UIManager.Instance.PreviewManager.ClosePreview();
  }
  private IEnumerator keepmove()
  {
    MoveButton.interactable = false;
        UIManager.Instance.ResetEventPanels();
        yield return StartCoroutine(movemap());
        yield return null;
  }
  public void MoveMap()
  {
    if (UIManager.Instance.IsWorking) return;
    //다른 UI가 움직이고 있으면 작동 안함

    if(GameManager.Instance.MyGameData.CanMove==false) return;
    //움직일 수 없는 상황(이벤트 선택 화면,이벤트 도중)이라면 못 움직인다는 UI 출력
    if (SelectedSettle == null) return;
    //움직일 수 있는 상황에서, 정착지에서 출발할 때 목표 정착지를 선택하지 않은 경우 목표 방향이 없다는 UI 출력
    UIManager.Instance.ResetEventPanels();
    UIManager.Instance.AddUIQueue(movemap());
  }
  private IEnumerator movemap()
  {
    //이동 도중이니 이동 버튼은 비활성화
    MoveButton.interactable = false;
    Vector3 _playerrectpos = PlayerRect.anchoredPosition;          //현재 위치(UI)
    Vector3 _settlerectpos = SettleIcons[SelectedSettle.OriginName].GetComponent<RectTransform>().anchoredPosition;           //종점 위치(UI)

    Vector3 _playretilepos = GameManager.Instance.MyGameData.CurrentPos;//현재 위치(타일)
    Vector3 _settletilepos = (Vector3)SettleIcons[SelectedSettle.OriginName].SettlementData.VectorPos;//종점 위치(타일)

    float _currentprogress = GameManager.Instance.MyGameData.MoveProgress;//현재 이동 진행도 0.0f ~ 1.0f
    float _targetprogress = 0.0f;                                             //이번 이동에서 목표로 하는 이동 진행도
    if (_currentprogress == 0.0f) _targetprogress = Random.Range(0.3f, 0.7f);
    else _targetprogress = 1.0f;
    Vector3 _targetrectpos=Vector3.Lerp(_playerrectpos, _settlerectpos, _targetprogress);  //이번 이동에서 목표로 하는 위치(UI)
    Vector3 _targettilepos=Vector3.Lerp(_playretilepos, _settletilepos, _targetprogress);    //이번 이동에서 목표로 하는 위치(타일)
    if (_currentprogress == 0.0f)//정착지 ~ 야외
    {
      GameManager.Instance.MyGameData.CurrentSanity -= GameManager.Instance.MyGameData.MoveSanityValue;
      UIManager.Instance.UpdateSanityText();
      GameManager.Instance.MyGameData.ClearBeforeEvents();
      GameManager.Instance.MyGameData.CurrentPos = _targettilepos;
      GameManager.Instance.MyGameData.MoveProgress = _targetprogress;
      GameManager.Instance.MyGameData.CurrentSettlement = null;
      //이전 정착지의 이벤트 관련 데이터 초기화
      //currentprogress==0.0f면 정착지에서 중간 이벤트 지점까지 이동
      yield return StartCoroutine(movecharacter(PlayerRect.anchoredPosition, _targetrectpos, _settlerectpos, _targetprogress));
      //캐릭터 이동시킴
      EventManager.Instance.SetOutsideEvent(MapCreater.GetSingleTileData(_targetrectpos));
            //캐릭터 멈춘 위치 주위 1칸 강,숲,언덕,산,바다 유무 파악해서 EventManager에 던져줌
            yield return StartCoroutine(UIManager.Instance.CloseUI(MyRect, MyGroup, MyDir));
            IsOpen = false;
      SettleIcons[SelectedSettle.OriginName].GetComponent<Button>().interactable = false;
      foreach (var _settle in GameManager.Instance.MyGameData.AvailableSettlement) 
      { 
        _settle.IsOpen = false;
        if (_settle.Equals(SelectedSettle)) continue;
        SettleIcons[_settle.OriginName].DeActiveButton(); 
      }
      GameManager.Instance.MyGameData.AvailableSettlement.Clear();
      GameManager.Instance.MyGameData.AvailableSettlement.Add(SelectedSettle);
      //멈췄으면 바로 맵 닫기

      //도중에 멈춘거니까 이동 버튼 활성화는 안함
    }
    else//야외 ~ 정착지
    {
      GameManager.Instance.MyGameData.CurrentEvent = null;
      GameManager.Instance.MyGameData.CurrentPos = _targettilepos;
      GameManager.Instance.MyGameData.MoveProgress = 0.0f;
      GameManager.Instance.MyGameData.CurrentSettlement = SelectedSettle;
      GameManager.Instance.MyGameData.CurrentSettlement.IsOpen = false;
      //목표에 완전히 도착했으니 이동 관련 정보는 초기화

      //currentprogress!=0.0f면 외부에서 이벤트 클리어하고 가던 정착지를 향해 다시 출발
      yield return StartCoroutine(movecharacter(_playerrectpos, _settlerectpos, _currentprogress));
      //캐릭터 이동시킨
      EventManager.Instance.SetSettleEvent(SelectedSettle.GetSettleTileEventData());
      //캐릭터 목표 지점 정착지 정보 보내줌
      GameManager.Instance.MyGameData.Turn++;
      UIManager.Instance.UpdateTurnIcon();
      //정착지에 도착했으면 새로운 목표 장소 3개 활성화
      GameManager.Instance.MyGameData.AvailableSettlement = GameManager.Instance.MyMapData.GetCloseSettles(GameManager.Instance.MyGameData.CurrentSettlement, 3);
      foreach (Settlement _settle in GameManager.Instance.MyGameData.AvailableSettlement) _settle.IsOpen = true;
      if (SelectedSettle != null) SettleIcons[SelectedSettle.OriginName].Selected = false;
      SelectedSettle = null;
      UpdateIcons(GameManager.Instance.MyGameData.AvailableSettlement);

            yield return StartCoroutine(UIManager.Instance.CloseUI(MyRect, MyGroup, MyDir));
            IsOpen = false;
            //멈췄으면 바로 맵 닫기
        }
    }
  private IEnumerator movecharacter(Vector3 _originrectpos,Vector3 _targetrectpos,Vector3 _maxpos, float _targetprogress)
  {
    float _time = 0.0f;
    float _targettime = MoveTime * _targetprogress;
    while (_time < _targettime)
    {
      PlayerRect.anchoredPosition=Vector3.Lerp(_originrectpos, _maxpos, _time/MoveTime);
      _time += Time.deltaTime;
      yield return null;
    }
    PlayerRect.anchoredPosition = _targetrectpos;
    yield return new WaitForSeconds(UIManager.Instance.CharacterWaitTime);
    //0.0f ~ _targetprogress 비율까지 움직임

  }//캐릭터 움직이는 코루틴 - 정착지 ~ 야외 이벤트
  private IEnumerator movecharacter(Vector3 _currenrecttpos, Vector3 _endrectpos, float _currentprogress)
  {
    Vector3 _originpos =_currenrecttpos- ((_endrectpos - _currenrecttpos) / (1.0f - _currentprogress)) * _currentprogress;
    //출발 좌표, 끝 좌표랑 현재 진행도(_progress)를 사용해 0.1칸 단위를 만들어 10배를 곱해
    //_progress 0.0f 기준(시작점)을 도출한다
    float _time = MoveTime * _currentprogress;
    //시작 _time은 0.0~MoveTime의 _currentprogress만큼
    while (_time < MoveTime)
    {
      PlayerRect.anchoredPosition = Vector3.Lerp(_originpos, _endrectpos, _time / MoveTime);
      _time += Time.deltaTime; yield return null;
    }
    PlayerRect.anchoredPosition = _endrectpos;
    yield return new WaitForSeconds(UIManager.Instance.CharacterWaitTime);
    //targetprogress ~ 1.0f 비율까지 이동

  }//캐릭터 움직이는 코루틴 - 야외 이벤트 ~ 정착지

  public void UpdateIcons(List<Settlement> _settles)
  {
    foreach (Settlement _settle in _settles)
    {
      if (_settle.IsOpen == true) SettleIcons[_settle.OriginName].ActiveButton();
      else SettleIcons[_settle.OriginName].DeActiveButton();
    }
  }
  public void SetPlayerPos(Settlement _targetsettle)
  {
    Vector3 _targetpos = SettleIcons[_targetsettle.OriginName].GetComponent<RectTransform>().anchoredPosition;
    PlayerRect.anchoredPosition= _targetpos;
  }
  public void SetPlayerPos(Vector3 _tilepos)
  {
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

    PlayerRect.anchoredPosition = MapCreater.Tilemap_bottom.CellToWorld(_intpos);
    PlayerRect.anchoredPosition = PlayerRect.anchoredPosition + new Vector2(_unit.x * _else.x, _unit.y * _else.y);
    PlayerRect.SetParent(_originparent);
    //정수 값으로 산출한 CelltoWorld랑 미리 값을 구해놓은 실수형 값을 더해 이동
  }//정수가 아닌 좌표를 받아서 거기로 옮기는거
}
