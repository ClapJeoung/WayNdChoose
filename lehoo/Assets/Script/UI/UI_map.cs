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
    [SerializeField] private TextMeshProUGUI SettleName = null;
    [SerializeField] private Image SettleIllust = null;
    [SerializeField] private Button MoveButton = null;
  [SerializeField] private TextMeshProUGUI MoveButtonText = null;
  /// <summary>
  /// 0: ������ ���� �� 1: �̵����� 2: �̵����̶� Ŭ������ 3:�̵��Ұ� 4:��� �̵�
  /// </summary>
  /// <param name="state"></param>
  public void SetMoveButton(int state)
  {
    switch (state)
    {
      case 0:
        if (MoveButton.interactable.Equals(true)) MoveButton.interactable = false;
        MoveButtonText.text = GameManager.Instance.GetTextData("choosesettle").Name;
        break;
      case 1:
        if (MoveButton.interactable.Equals(false)) MoveButton.interactable = true;
        MoveButtonText.text = string.Format(GameManager.Instance.GetTextData("canmove").Name, SelectedSettleCost);
        break;
      case 2:
        if (MoveButton.interactable.Equals(true)) MoveButton.interactable = false;
        MoveButtonText.text = GameManager.Instance.GetTextData("duringmoving").Name;
        break;
      case 3:
        if (MoveButton.interactable.Equals(true)) MoveButton.interactable = false;
        MoveButtonText.text = GameManager.Instance.GetTextData("cannotmove").Name;
        break;
      case 4:
        if (MoveButton.interactable.Equals(false)) MoveButton.interactable = true;
        MoveButtonText.text = GameManager.Instance.GetTextData("keepgoing").Name;
        break;
    }
  }
  [SerializeField] private maptext MapCreater = null;
  private Dictionary<string,SettlementIcon> SettleIcons = new Dictionary<string,SettlementIcon>();
  [SerializeField] private Image CurrentQuestProgress = null;
  [SerializeField] private TextMeshProUGUI CurrentQuestDescription = null;
  [SerializeField] private List<GameObject> EnvirIcons=new List<GameObject>();
  [SerializeField] private TextMeshProUGUI OuterInfoText = null;
  [SerializeField] private TextMeshProUGUI UnpText = null;
  [SerializeField] private CanvasGroup ArrowGroup = null;
  [SerializeField] private RectTransform ArrowRect = null;
  //<OriginName,SettlementIcon>
  private Settlement SelectedSettle = null;
  private int SelectedSettleCost = 0;

  public void AddSettleIcon(string _name, SettlementIcon _icon) => SettleIcons.Add(_name, _icon);
  public void OpenUI()
  {
    if (IsOpen) { CloseUI(); IsOpen = false; return; }
    IsOpen = true;
    UIManager.Instance.AddUIQueue(openui());
  }
  private IEnumerator openui()
  {
    if (GameManager.Instance.MyGameData.CurrentQuest != null)
    {
      List<SettlementIcon> _ablesettles = new List<SettlementIcon>();
      List<EnvironmentType> _ableenvir = new List<EnvironmentType>();
      QuestHolder _currentquest = GameManager.Instance.MyGameData.CurrentQuest;
      CurrentQuestDescription.text = _currentquest.CurrentDescription;
      switch (_currentquest.CurrentSequence)
      {
        case QuestSequence.Rising:
          CurrentQuestProgress.sprite = GameManager.Instance.ImageHolder.Quest_risig;
          var _risings = _currentquest.EventList_Rising_Available;
          foreach(var _event in _risings)
          {
            if (_event.SettlementType.Equals(SettlementType.Outer))
            {
              if (_event.TileCheckType.Equals(1) && !_ableenvir.Contains(_event.EnvironmentType))
              {
                _ableenvir.Add(_event.EnvironmentType);
              }
            }//�߿� �̺�Ʈ�� ȯ�� ������ ������
            else
            {
              foreach(var _data in SettleIcons)
              {
                if(_data.Value.SettlementData.CheckAbleEvent(_event)&&!_ablesettles.Contains(_data.Value))_ablesettles.Add(_data.Value);
              }
            }//�� �� ��������� �̺�Ʈ�� ���� ����� ������
          }
          break;
        case QuestSequence.Climax:
          CurrentQuestProgress.sprite = GameManager.Instance.ImageHolder.Quest_climax;
          if (_currentquest.NextQuestSettlement == null&&_currentquest.NextQuestEnvir.Equals(EnvironmentType.NULL))
          {
            QuestEventData _climaxevent = _currentquest.Event_Climax;
            if (_climaxevent.SettlementType.Equals(SettlementType.Outer))
            {
              if (_climaxevent.TileCheckType.Equals(1))
              {
                _ableenvir.Add(_climaxevent.EnvironmentType);
                _currentquest.NextQuestEnvir = _climaxevent.EnvironmentType;
              }
            }//�ܺ� �̺�Ʈ�� ȯ�游
            else
            {
              List<SettlementIcon> _availableclimax = new List<SettlementIcon>();
              foreach(var _data in SettleIcons)
              {
                if(_data.Value.SettlementData.CheckAbleEvent(_climaxevent))_availableclimax.Add(_data.Value);
              }
              SettlementIcon _selectclimax = _availableclimax[Random.Range(0, _availableclimax.Count)];
              _ablesettles.Add(_selectclimax);
              _currentquest.NextQuestSettlement = _selectclimax.SettlementData;
            }//������ �̺�Ʈ�� ������ �ϳ� ��������
          }//���� �������� �������� ���� ���¶�� ���� ���� �Է�
          else
          {
            if (_currentquest.NextQuestSettlement != null)
              _ablesettles.Add(SettleIcons[_currentquest.NextQuestSettlement.OriginName]);
            else _ableenvir.Add(_currentquest.NextQuestEnvir);
          }//���� �������� ������ ���¶�� �״�� �ҷ�����
          break;
        case QuestSequence.Falling:
          CurrentQuestProgress.sprite = GameManager.Instance.ImageHolder.Quest_fall;
          if (_currentquest.NextQuestSettlement == null && _currentquest.NextQuestEnvir.Equals(EnvironmentType.NULL))
          {
            QuestEventData _fallingevent = _currentquest.Event_Falling;
            if (_fallingevent.SettlementType.Equals(SettlementType.Outer))
            {
              if (_fallingevent.TileCheckType.Equals(1))
              {
                _ableenvir.Add(_fallingevent.EnvironmentType);
                _currentquest.NextQuestEnvir = _fallingevent.EnvironmentType;
              }
            }//�ܺ� �̺�Ʈ�� ȯ�游
            else
            {
              List<SettlementIcon> _availablefalling = new List<SettlementIcon>();
              foreach (var _data in SettleIcons)
              {
                if (_data.Value.SettlementData.CheckAbleEvent(_fallingevent)) _availablefalling.Add(_data.Value);
              }
              SettlementIcon _selectfalling = _availablefalling[Random.Range(0, _availablefalling.Count)];
              _ablesettles.Add(_selectfalling);
              _currentquest.NextQuestSettlement = _selectfalling.SettlementData;
            }//������ �̺�Ʈ�� ������ �ϳ� ��������
          }//���� �������� �������� ���� ���¶�� ���� ���� �Է�
          else
          {
            if (_currentquest.NextQuestSettlement != null)
              _ablesettles.Add(SettleIcons[_currentquest.NextQuestSettlement.OriginName]);
            else _ableenvir.Add(_currentquest.NextQuestEnvir);
          }//���� �������� ������ ���¶�� �״�� �ҷ�����
          break;
      }

      if (_ableenvir.Count > 0)
      {
        foreach (var _envir in _ableenvir)
          if (EnvirIcons[(int)_envir].activeInHierarchy.Equals(false)) EnvirIcons[(int)_envir].SetActive(true);
        OuterInfoText.text = GameManager.Instance.GetTextData("outereventinfo").Name;
      }
      else OuterInfoText.text = "";

      if (_ablesettles.Count>0)
      {
        for (int i = 0; i < _ablesettles.Count; i++)
        {
          _ablesettles[i].SetQuestIcon((int)_currentquest.CurrentSequence);
        }
      }

    }

    if(SelectedSettle!=null) UnpText.text = GameManager.Instance.MyGameData.AllSettleUnpleasant[SelectedSettle].ToString();

    if (SelectedSettle==null) ArrowGroup.alpha = 0.0f;
    else
    {
      if(ArrowGroup.alpha.Equals(0.0f)) ArrowGroup.alpha = 1.0f;
      SetArrow();
    }

    if (GameManager.Instance.MyGameData.CanMove)  //�̵� ������ ��Ȳ(�߿ܿ��� �̺�Ʈ�� �Ϸ��� ��Ȳ, ���������� �̺�Ʈ�� �Ϸ��� ��Ȳ)
    {
      if (GameManager.Instance.MyGameData.CurrentSettlement != null)//���� �������� ������ �ƴϴ�? ���������� ����ϴ� ��Ȳ
      {
        SetMoveButton(0);
      }
      else SetMoveButton(4);                                        //���� �������� ����? �߿� �̺�Ʈ�� ������ ��� �̵��ؾ� �ϴ� ��Ȳ
    }
    else  //�̵� �Ұ����� ��Ȳ(�߿� �̺�Ʈ ������ Ȥ�� ������ �̺�Ʈ ������)
    {
      SetMoveButton(3);
    }
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(MyGroup,1.0f,0.4f));
  }
  public override void CloseUI()
  {
    UIManager.Instance.AddUIQueue(closeui());
    IsOpen = false;
  }
  private IEnumerator closeui()
  {
   yield return StartCoroutine(UIManager.Instance.ChangeAlpha(MyGroup,0.0f,0.2f));
    for(int i = 0; i < EnvirIcons.Count; i++)
    {
      if (EnvirIcons[i].activeInHierarchy.Equals(true)) EnvirIcons[i].SetActive(false);
    }
    var _settleicons = SettleIcons.Values.ToArray();
    for (int i = 0; i < SettleIcons.Count; i++)
      _settleicons[i].SetQuestIcon(0);
    yield return null;
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

  public void SetArrow()
  {
    if(ArrowGroup.alpha.Equals(0.0f))ArrowGroup.alpha = 1.0f;
    float _length = Mathf.Clamp(Vector3.Distance(PlayerRect.anchoredPosition, GameManager.Instance.MyGameData.MoveTargetPos), 60.0f, 5000.0f);
    ArrowRect.sizeDelta = new Vector2(100.0f, _length);
  ArrowRect.anchoredPosition = PlayerRect.anchoredPosition;
    Vector2 _dir = PlayerRect.anchoredPosition - GameManager.Instance.MyGameData.MoveTargetPos;
   // _dir.Normalize();
  //  Debug.Log(_dir);
    Vector3 _angle =Vector3.back*( Mathf.Atan2(_dir.x,_dir.y)*Mathf.Rad2Deg + 180.0f);
    ArrowRect.rotation = Quaternion.Euler(_angle);
  }
    public void UpdatePanel(Settlement _settle)
    {
    if (SelectedSettle!=null)  SettleIcons[SelectedSettle.OriginName].Selected = false;
    SelectedSettle = _settle;
    SettleName.text = _settle.Name;
        SettleIllust.sprite = _settle.Illust;
    UnpText.text = GameManager.Instance.MyGameData.AllSettleUnpleasant[_settle].ToString();
    SelectedSettleCost = GameManager.Instance.MyGameData.GetMoveSanityValue(Vector2.Distance(GameManager.Instance.MyGameData.CurrentPos, SelectedSettle.VectorPos));
    SetMoveButton(1);
    SettleIcons[SelectedSettle.OriginName].Selected = true;
    GameManager.Instance.MyGameData.MoveTargetPos = SettleIcons[SelectedSettle.OriginName].GetComponent<RectTransform>().anchoredPosition;
    SetArrow();
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
        GameManager.Instance.MyGameData.VisitedPlaces.Clear();
    SetMoveButton(2);
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
      GameManager.Instance.MyGameData.CurrentSanity -= SelectedSettleCost;
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
        if (_settle.Equals(SelectedSettle)) continue;
      }
      GameManager.Instance.MyGameData.AvailableSettlement.Clear();
      GameManager.Instance.MyGameData.AvailableSettlement.Add(SelectedSettle);
      //���߿� ����Ŵϱ� �̵� ��ư Ȱ��ȭ�� ����
    }
    else//�߿� ~ ������
    {
      GameManager.Instance.MyGameData.CurrentEvent = null;
      GameManager.Instance.MyGameData.CurrentPos = _targettilepos;
      GameManager.Instance.MyGameData.MoveProgress = 0.0f;
      GameManager.Instance.MyGameData.CurrentSettlement = SelectedSettle;
      SelectedSettle.LibraryType = (ThemeType)Random.Range(0, 4);
      //��ǥ�� ������ ���������� �̵� ���� ������ �ʱ�ȭ

      //currentprogress!=0.0f�� �ܺο��� �̺�Ʈ Ŭ�����ϰ� ���� �������� ���� �ٽ� ���
      yield return StartCoroutine(movecharacter(_playerrectpos, _settlerectpos, _currentprogress));
            //ĳ���� �̵���Ų
            CloseUI();
            IsOpen = false;
            //�������� �ٷ� �� �ݱ�

            EventManager.Instance.SetSettleEvent(SelectedSettle.TileData);
      //ĳ���� ��ǥ ���� ������ ���� ������
      GameManager.Instance.MyGameData.Turn++;
      UIManager.Instance.UpdateTurnIcon();
      //�������� ���������� ���ο� ��ǥ ��� 3�� Ȱ��ȭ
      GameManager.Instance.MyGameData.AvailableSettlement = GameManager.Instance.MyMapData.GetCloseSettles(GameManager.Instance.MyGameData.CurrentSettlement, 3);
      if (SelectedSettle != null) SettleIcons[SelectedSettle.OriginName].Selected = false;
      SelectedSettle = null;
      UpdateIcons(GameManager.Instance.MyGameData.AvailableSettlement);

        }
    }
  private IEnumerator movecharacter(Vector3 _originrectpos,Vector3 _targetrectpos,Vector3 _maxpos, float _targetprogress)
  {
    float _time = 0.0f;
    float _targettime = MoveTime * _targetprogress;
    while (_time < _targettime)
    {
      PlayerRect.anchoredPosition=Vector3.Lerp(_originrectpos, _maxpos, Mathf.Pow(_time / MoveTime,0.4f));
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
      PlayerRect.anchoredPosition = Vector3.Lerp(_originpos, _endrectpos, Mathf.Pow(_time / MoveTime, 0.4f));
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
