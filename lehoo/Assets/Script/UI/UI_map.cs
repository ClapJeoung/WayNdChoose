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
    [SerializeField] private Button MoveButton = null;
  [SerializeField] private maptext MapCreater = null;
  private Dictionary<string,SettlementIcon> SettleIcons = new Dictionary<string,SettlementIcon>();
  private Settlement SelectedSettle = null;
  private bool CanMove = false;
  public void AddSettleIcon(string _name, SettlementIcon _icon) => SettleIcons.Add(_name, _icon);
  public override void OpenUI()
  {
    base.OpenUI();
    StartCoroutine(tilemapalpha(UIManager.Instance.MoveInAlpha, 1.0f));
  }
  public override void CloseUI()
  {
    base.CloseUI();
    StartCoroutine(tilemapalpha(1.0f, 0.0f));
  }
  private IEnumerator tilemapalpha(float _startalpha,float _endalpha)
  {
    Color _color = Color.white;
    _color.a = _startalpha;
    Tilemap_bottom.color = _color;
    Tilemap_top.color= _color;
    float _time = 0.0f;
    float _maxtime = UIManager.Instance.UIActionDuration;
    while (_time < _maxtime)
    {
      _color.a = Mathf.Lerp(_startalpha,_endalpha,Mathf.Pow(_time/_maxtime,2.0f));
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
    SettleName.text = _settle.Name;
        SettleIllust.sprite = null;
    MoveButton.interactable = true;
    SelectedSettle = _settle;
    SettleIcons[SelectedSettle.Name].Selected = true;
    }
  public void MoveMap()
  {
    if (UIManager.Instance.IsWorking) return;
    //다른 UI가 움직이고 있으면 작동 안함

    if(CanMove==false) return;
    //움직일 수 없는 상황(이벤트 선택 화면,이벤트 도중)이라면 못 움직인다는 UI 출력
    if (SelectedSettle == null) return;
    //움직일 수 있는 상황에서, 정착지에서 출발할 때 목표 정착지를 선택하지 않은 경우 목표 방향이 없다는 UI 출력
    StartCoroutine(movemap());
  }
  private IEnumerator movemap()
  {
    //이동 도중이니 이동 버튼은 비활성화
    UIManager.Instance.IsWorking = true;
    Vector3 _currentpos = GameManager.Instance.MyGameData.PlayerPos;          //현재 위치
    Vector3 _settlepos = SettleIcons[SelectedSettle.Name].Position;           //종점 위치
    float _currentprogress = GameManager.Instance.MyGameData.PlayerPos_degree;//현재 이동 진행도 0.0f ~ 1.0f
    float _targetprogress = 0.0f;                                             //이번 이동에서 목표로 하는 이동 진행도
    if (_currentprogress == 0.0f) _targetprogress = Random.Range(0.3f, 0.7f);
    else _targetprogress = 1.0f;
    Vector3 _targetpos=Vector3.Lerp(_currentpos,_settlepos,_targetprogress);  //이번 이동에서 목표로 하는 위치

    if (_currentprogress == 0.0f)
    {
      //currentprogress==0.0f면 정착지에서 중간 이벤트 지점까지 이동
      yield return StartCoroutine(movecharacter(PlayerRect.anchoredPosition, _targetpos, _settlepos, _targetprogress));
      //캐릭터 이동시킴
      bool _isriver = false, _isforest = false, _ishighland = false, _ismountain = false, _issea = false;
      MapCreater.GetAroundData(_targetpos, ref _isriver, ref _isforest, ref _ishighland,ref _ismountain ,ref _issea);
      EventManager.Instance.SetNewEvent(_isriver, _isforest, _ishighland, _ismountain, _issea);
      //캐릭터 멈춘 위치 주위 1칸 강,숲,언덕,산,바다 유무 파악해서 EventManager에 던져줌
      //도중에 멈춘거니까 이동 버튼 활성화는 안함
    }
    else
    {
      //currentprogress!=0.0f면 외부에서 이벤트 클리어하고 가던 정착지를 향해 다시 출발
      yield return StartCoroutine(movecharacter(_currentpos, _targetpos, _currentprogress));
      //캐릭터 이동시킨
      EventManager.Instance.SetNewEvent(SelectedSettle);
      //캐릭터 목표 지점 정착지 정보 보내줌
      SelectedSettle = null;
      GameManager.Instance.MyGameData.PlayerPos = _targetpos;
      GameManager.Instance.MyGameData.PlayerPos_degree = 0.0f;
      //목표에 완전히 도착했으니 이동 관련 정보는 초기화
    }
  }
  private IEnumerator movecharacter(Vector3 _originpos,Vector3 _targetpos,Vector3 _maxpos, float _targetprogress)
  {
    float _time = 0.0f;
    float _targettime = MoveTime * _targetprogress;
    while (_time < _targettime)
    {
      PlayerRect.anchoredPosition=Vector3.Lerp(_originpos, _maxpos, _time/MoveTime);
      _time += Time.deltaTime;yield return null;
    }
    PlayerRect.anchoredPosition = _targetpos;
    //0.0f ~ _targetprogress 비율까지 움직임

  }//캐릭터 움직이는 코루틴 - 정착지 ~ 야외 이벤트
  private IEnumerator movecharacter(Vector3 _currentpos, Vector3 _endpos, float _currentprogress)
  {
    Vector3 _originpos = ((_endpos - _currentpos) / (1.0f - _currentprogress)) * 10.0f;
    //출발 좌표, 끝 좌표랑 현재 진행도(_progress)를 사용해 0.1칸 단위를 만들어 10배를 곱해
    //_progress 0.0f 기준(시작점)을 도출한다
    float _time = MoveTime * _currentprogress;
    //시작 _time은 0.0~MoveTime의 _currentprogress만큼
    while (_time < MoveTime)
    {
      PlayerRect.anchoredPosition = Vector3.Lerp(_originpos, _endpos, _time / MoveTime);
      _time += Time.deltaTime; yield return null;
    }
    PlayerRect.anchoredPosition = _endpos;
    SettleIcons[SelectedSettle.Name].Selected = false;
    SelectedSettle = null;
    //targetprogress ~ 1.0f 비율까지 이동

  }//캐릭터 움직이는 코루틴 - 야외 이벤트 ~ 정착지

  public void UpdateIcons(List<Settlement> _settles)
  {
    foreach (Settlement _settle in _settles)
    {
      if (_settle.IsOpen == true) SettleIcons[_settle.Name].ActiveButton();
      else SettleIcons[_settle.Name].DeActiveButton();
    }
  }
  public void SetPlayerPos(Settlement _targetsettle)
  {
    Vector3 _targetpos = SettleIcons[_targetsettle.Name].Position;
    PlayerRect.anchoredPosition= _targetpos;
  }
}
