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
    //�ٸ� UI�� �����̰� ������ �۵� ����

    if(GameManager.Instance.MyGameData.CanMove==false) return;
    //������ �� ���� ��Ȳ(�̺�Ʈ ���� ȭ��,�̺�Ʈ ����)�̶�� �� �����δٴ� UI ���
    if (SelectedSettle == null) return;
    //������ �� �ִ� ��Ȳ����, ���������� ����� �� ��ǥ �������� �������� ���� ��� ��ǥ ������ ���ٴ� UI ���
    UIManager.Instance.ResetEventPanels();
    UIManager.Instance.AddUIQueue(movemap());
  }
  private IEnumerator movemap()
  {
    //�̵� �����̴� �̵� ��ư�� ��Ȱ��ȭ
    MoveButton.interactable = false;
    Vector3 _playerrectpos = PlayerRect.anchoredPosition;          //���� ��ġ(UI)
    Vector3 _settlerectpos = SettleIcons[SelectedSettle.OriginName].GetComponent<RectTransform>().anchoredPosition;           //���� ��ġ(UI)

    Vector3 _playretilepos = GameManager.Instance.MyGameData.CurrentPos;//���� ��ġ(Ÿ��)
    Vector3 _settletilepos = (Vector3)SettleIcons[SelectedSettle.OriginName].SettlementData.VectorPos;//���� ��ġ(Ÿ��)

    float _currentprogress = GameManager.Instance.MyGameData.MoveProgress;//���� �̵� ���൵ 0.0f ~ 1.0f
    float _targetprogress = 0.0f;                                             //�̹� �̵����� ��ǥ�� �ϴ� �̵� ���൵
    if (_currentprogress == 0.0f) _targetprogress = Random.Range(0.3f, 0.7f);
    else _targetprogress = 1.0f;
    Vector3 _targetrectpos=Vector3.Lerp(_playerrectpos, _settlerectpos, _targetprogress);  //�̹� �̵����� ��ǥ�� �ϴ� ��ġ(UI)
    Vector3 _targettilepos=Vector3.Lerp(_playretilepos, _settletilepos, _targetprogress);    //�̹� �̵����� ��ǥ�� �ϴ� ��ġ(Ÿ��)
    if (_currentprogress == 0.0f)//������ ~ �߿�
    {
      GameManager.Instance.MyGameData.CurrentSanity -= GameManager.Instance.MyGameData.MoveSanityValue;
      UIManager.Instance.UpdateSanityText();
      GameManager.Instance.MyGameData.ClearBeforeEvents();
      GameManager.Instance.MyGameData.CurrentPos = _targettilepos;
      GameManager.Instance.MyGameData.MoveProgress = _targetprogress;
      GameManager.Instance.MyGameData.CurrentSettlement = null;
      //���� �������� �̺�Ʈ ���� ������ �ʱ�ȭ
      //currentprogress==0.0f�� ���������� �߰� �̺�Ʈ �������� �̵�
      yield return StartCoroutine(movecharacter(PlayerRect.anchoredPosition, _targetrectpos, _settlerectpos, _targetprogress));
      //ĳ���� �̵���Ŵ
      EventManager.Instance.SetOutsideEvent(MapCreater.GetSingleTileData(_targetrectpos));
            //ĳ���� ���� ��ġ ���� 1ĭ ��,��,���,��,�ٴ� ���� �ľ��ؼ� EventManager�� ������
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
      //�������� �ٷ� �� �ݱ�

      //���߿� ����Ŵϱ� �̵� ��ư Ȱ��ȭ�� ����
    }
    else//�߿� ~ ������
    {
      GameManager.Instance.MyGameData.CurrentEvent = null;
      GameManager.Instance.MyGameData.CurrentPos = _targettilepos;
      GameManager.Instance.MyGameData.MoveProgress = 0.0f;
      GameManager.Instance.MyGameData.CurrentSettlement = SelectedSettle;
      GameManager.Instance.MyGameData.CurrentSettlement.IsOpen = false;
      //��ǥ�� ������ ���������� �̵� ���� ������ �ʱ�ȭ

      //currentprogress!=0.0f�� �ܺο��� �̺�Ʈ Ŭ�����ϰ� ���� �������� ���� �ٽ� ���
      yield return StartCoroutine(movecharacter(_playerrectpos, _settlerectpos, _currentprogress));
      //ĳ���� �̵���Ų
      EventManager.Instance.SetSettleEvent(SelectedSettle.GetSettleTileEventData());
      //ĳ���� ��ǥ ���� ������ ���� ������
      GameManager.Instance.MyGameData.Turn++;
      UIManager.Instance.UpdateTurnIcon();
      //�������� ���������� ���ο� ��ǥ ��� 3�� Ȱ��ȭ
      GameManager.Instance.MyGameData.AvailableSettlement = GameManager.Instance.MyMapData.GetCloseSettles(GameManager.Instance.MyGameData.CurrentSettlement, 3);
      foreach (Settlement _settle in GameManager.Instance.MyGameData.AvailableSettlement) _settle.IsOpen = true;
      if (SelectedSettle != null) SettleIcons[SelectedSettle.OriginName].Selected = false;
      SelectedSettle = null;
      UpdateIcons(GameManager.Instance.MyGameData.AvailableSettlement);

            yield return StartCoroutine(UIManager.Instance.CloseUI(MyRect, MyGroup, MyDir));
            IsOpen = false;
            //�������� �ٷ� �� �ݱ�
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
    //0.0f ~ _targetprogress �������� ������

  }//ĳ���� �����̴� �ڷ�ƾ - ������ ~ �߿� �̺�Ʈ
  private IEnumerator movecharacter(Vector3 _currenrecttpos, Vector3 _endrectpos, float _currentprogress)
  {
    Vector3 _originpos =_currenrecttpos- ((_endrectpos - _currenrecttpos) / (1.0f - _currentprogress)) * _currentprogress;
    //��� ��ǥ, �� ��ǥ�� ���� ���൵(_progress)�� ����� 0.1ĭ ������ ����� 10�踦 ����
    //_progress 0.0f ����(������)�� �����Ѵ�
    float _time = MoveTime * _currentprogress;
    //���� _time�� 0.0~MoveTime�� _currentprogress��ŭ
    while (_time < MoveTime)
    {
      PlayerRect.anchoredPosition = Vector3.Lerp(_originpos, _endrectpos, _time / MoveTime);
      _time += Time.deltaTime; yield return null;
    }
    PlayerRect.anchoredPosition = _endrectpos;
    yield return new WaitForSeconds(UIManager.Instance.CharacterWaitTime);
    //targetprogress ~ 1.0f �������� �̵�

  }//ĳ���� �����̴� �ڷ�ƾ - �߿� �̺�Ʈ ~ ������

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

    Vector3 _unit = _onepos - _zeropos; //(0,0),(1,1)�� UI�� �ű� ��ǥ�� ���� UI �� �Ÿ� 1�� Rect ������ ����
    Vector3Int _intpos = new Vector3Int(Mathf.FloorToInt(_tilepos.x), Mathf.FloorToInt(_tilepos.y));
    Vector3 _else=new Vector3(_tilepos.x-_intpos.x,_tilepos.y-_intpos.y);
    //Ÿ�� ��ǥ�� ����, �������� �и�

    PlayerRect.anchoredPosition = MapCreater.Tilemap_bottom.CellToWorld(_intpos);
    PlayerRect.anchoredPosition = PlayerRect.anchoredPosition + new Vector2(_unit.x * _else.x, _unit.y * _else.y);
    PlayerRect.SetParent(_originparent);
    //���� ������ ������ CelltoWorld�� �̸� ���� ���س��� �Ǽ��� ���� ���� �̵�
  }//������ �ƴ� ��ǥ�� �޾Ƽ� �ű�� �ű�°�
}
