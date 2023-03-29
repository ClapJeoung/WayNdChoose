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
    //�ٸ� UI�� �����̰� ������ �۵� ����

    if(CanMove==false) return;
    //������ �� ���� ��Ȳ(�̺�Ʈ ���� ȭ��,�̺�Ʈ ����)�̶�� �� �����δٴ� UI ���
    if (SelectedSettle == null) return;
    //������ �� �ִ� ��Ȳ����, ���������� ����� �� ��ǥ �������� �������� ���� ��� ��ǥ ������ ���ٴ� UI ���
    StartCoroutine(movemap());
  }
  private IEnumerator movemap()
  {
    //�̵� �����̴� �̵� ��ư�� ��Ȱ��ȭ
    UIManager.Instance.IsWorking = true;
    Vector3 _currentpos = GameManager.Instance.MyGameData.PlayerPos;          //���� ��ġ
    Vector3 _settlepos = SettleIcons[SelectedSettle.Name].Position;           //���� ��ġ
    float _currentprogress = GameManager.Instance.MyGameData.PlayerPos_degree;//���� �̵� ���൵ 0.0f ~ 1.0f
    float _targetprogress = 0.0f;                                             //�̹� �̵����� ��ǥ�� �ϴ� �̵� ���൵
    if (_currentprogress == 0.0f) _targetprogress = Random.Range(0.3f, 0.7f);
    else _targetprogress = 1.0f;
    Vector3 _targetpos=Vector3.Lerp(_currentpos,_settlepos,_targetprogress);  //�̹� �̵����� ��ǥ�� �ϴ� ��ġ

    if (_currentprogress == 0.0f)
    {
      //currentprogress==0.0f�� ���������� �߰� �̺�Ʈ �������� �̵�
      yield return StartCoroutine(movecharacter(PlayerRect.anchoredPosition, _targetpos, _settlepos, _targetprogress));
      //ĳ���� �̵���Ŵ
      bool _isriver = false, _isforest = false, _ishighland = false, _ismountain = false, _issea = false;
      MapCreater.GetAroundData(_targetpos, ref _isriver, ref _isforest, ref _ishighland,ref _ismountain ,ref _issea);
      EventManager.Instance.SetNewEvent(_isriver, _isforest, _ishighland, _ismountain, _issea);
      //ĳ���� ���� ��ġ ���� 1ĭ ��,��,���,��,�ٴ� ���� �ľ��ؼ� EventManager�� ������
      //���߿� ����Ŵϱ� �̵� ��ư Ȱ��ȭ�� ����
    }
    else
    {
      //currentprogress!=0.0f�� �ܺο��� �̺�Ʈ Ŭ�����ϰ� ���� �������� ���� �ٽ� ���
      yield return StartCoroutine(movecharacter(_currentpos, _targetpos, _currentprogress));
      //ĳ���� �̵���Ų
      EventManager.Instance.SetNewEvent(SelectedSettle);
      //ĳ���� ��ǥ ���� ������ ���� ������
      SelectedSettle = null;
      GameManager.Instance.MyGameData.PlayerPos = _targetpos;
      GameManager.Instance.MyGameData.PlayerPos_degree = 0.0f;
      //��ǥ�� ������ ���������� �̵� ���� ������ �ʱ�ȭ
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
    //0.0f ~ _targetprogress �������� ������

  }//ĳ���� �����̴� �ڷ�ƾ - ������ ~ �߿� �̺�Ʈ
  private IEnumerator movecharacter(Vector3 _currentpos, Vector3 _endpos, float _currentprogress)
  {
    Vector3 _originpos = ((_endpos - _currentpos) / (1.0f - _currentprogress)) * 10.0f;
    //��� ��ǥ, �� ��ǥ�� ���� ���൵(_progress)�� ����� 0.1ĭ ������ ����� 10�踦 ����
    //_progress 0.0f ����(������)�� �����Ѵ�
    float _time = MoveTime * _currentprogress;
    //���� _time�� 0.0~MoveTime�� _currentprogress��ŭ
    while (_time < MoveTime)
    {
      PlayerRect.anchoredPosition = Vector3.Lerp(_originpos, _endpos, _time / MoveTime);
      _time += Time.deltaTime; yield return null;
    }
    PlayerRect.anchoredPosition = _endpos;
    SettleIcons[SelectedSettle.Name].Selected = false;
    SelectedSettle = null;
    //targetprogress ~ 1.0f �������� �̵�

  }//ĳ���� �����̴� �ڷ�ƾ - �߿� �̺�Ʈ ~ ������

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
