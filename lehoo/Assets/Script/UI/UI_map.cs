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
    [SerializeField] private Button MoveButton = null;
  [SerializeField] private TextMeshProUGUI MoveButtonText = null;
  [SerializeField] private Button CancleButton = null;
  [SerializeField] private TextMeshProUGUI CancleButtonText = null;
  /// <summary>
  /// 0: ������ ���� �� 1: �̵����� 2: �̵����̶� Ŭ������ 3:�̵��Ұ� 4:��� �̵�
  /// </summary>
  /// <param name="state"></param>
  public void SetMoveButton(int state)
  {
    switch (state)
    {
      case 0:
        Debug.Log("������ ���� ����");
        if (MoveButton.interactable.Equals(true)) MoveButton.interactable = false;
        if(CancleButton.interactable.Equals(true)) CancleButton.interactable = false;
        MoveButtonText.text = GameManager.Instance.GetTextData("choosesettle").Name;
        CancleButton.interactable = false;
        break;
      case 1:
        Debug.Log("�̵� ���� ����");
        if (MoveButton.interactable.Equals(false)) MoveButton.interactable = true;
        if (CancleButton.interactable.Equals(false)) CancleButton.interactable = true;
        MoveButtonText.text = GameManager.Instance.GetTextData("canmove").Name;
        CancleButton.interactable = true;
        break;
      case 2:
        Debug.Log("�̵� ��");
        if (MoveButton.interactable.Equals(true)) MoveButton.interactable = false;
        if (CancleButton.interactable.Equals(true)) CancleButton.interactable = false;
        MoveButtonText.text = GameManager.Instance.GetTextData("duringmoving").Name;
        CancleButton.interactable = false;
        break;
      case 3:
        Debug.Log("�̵� �Ұ� ����");
        if (MoveButton.interactable.Equals(true)) MoveButton.interactable = false;
        if (CancleButton.interactable.Equals(true)) CancleButton.interactable = false;
        MoveButtonText.text = GameManager.Instance.GetTextData("cannotmove").Name;
        CancleButton.interactable = false;
        break;
      case 4:
        Debug.Log("��� �̵� ���� ����");
        if (MoveButton.interactable.Equals(false)) MoveButton.interactable = true;
        if (CancleButton.interactable.Equals(true)) CancleButton.interactable = false;
        MoveButtonText.text = GameManager.Instance.GetTextData("keepgoing").Name;
        CancleButton.interactable = false;
        break;
    }
  }
  public maptext MapCreater = null;
  private List<SettlementIcon> SettleIcons = new List<SettlementIcon>();
  private SettlementIcon GetSettleIcon(string originname)
  {
    foreach(SettlementIcon icon in SettleIcons)
      if(icon.SettlementData.OriginName == originname) return icon;

    return null;
  }
  private SettlementIcon GetSettleIcon(Settlement settledata)
  {
    foreach (SettlementIcon icon in SettleIcons)
      if (icon.SettlementData==settledata) return icon;

    return null;
  }
  [SerializeField] private Image CurrentQuestProgress = null;
  [SerializeField] private TextMeshProUGUI CurrentQuestDescription = null;
  [SerializeField] private List<GameObject> EnvirIcons=new List<GameObject>();
  [SerializeField] private TextMeshProUGUI OuterInfoText = null;
  [SerializeField] private TextMeshProUGUI UnpText = null;
  [SerializeField] private CanvasGroup[] ArrowGroup = null;
  [SerializeField] private RectTransform[] ArrowRect = null;
  [SerializeField] private MapArrowButton[] ArrowButtonScript = null;
  [SerializeField] private MapArrowButton SelectedArrow = null;
  private SettlementIcon SelectedSettleIcon
  {
    get { return SelectedArrow.MySettlementIcon; }
  }
  private Settlement SelectedSettle
  {
    get
    {
      return SelectedSettleIcon.SettlementData;
    }
  }
  private int SelectedSettleCost = 0;
  [SerializeField] private AnimationCurve ZoomInCurve = null;
  [SerializeField] private AnimationCurve ZoomOutCurve = null;
  [SerializeField] private RectTransform ScaleChanger = null;
  [SerializeField] private RectTransform LastHolder = null;
  [SerializeField] private Vector3 IdleScale = Vector3.one * 1.8f;
  [SerializeField] private Vector3 ZoomInScale = Vector3.one* 2.3f;
  [SerializeField] private float ZoomInTime = 1.2f;
  [SerializeField] private float ZoomOutTime = 0.4f;

  [SerializeField] private AnimationCurve SettleMoveCurve = null;
  [SerializeField] private float SettleMoveTime = 1.4f;
  [SerializeField] private AnimationCurve FollowMoveCurve = null;
  [SerializeField] private AnimationCurve CancleMoveCurve = null;
  [SerializeField] private float CancleMoveTime = 0.5f;

  public void AddSettleIcon(SettlementIcon _icon) => SettleIcons.Add(_icon);
  public void OpenUI()
  {
    if (IsOpen) { CloseUI(); IsOpen = false; return; }
    IsOpen = true;
    UIManager.Instance.AddUIQueue(openui());
  }
  private IEnumerator openui()
  {
    ScaleChanger.anchoredPosition = GetScaleChangerPos(PlayerRect.anchoredPosition);
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
              if (!_ableenvir.Contains(_event.EnvironmentType))
              {
                _ableenvir.Add(_event.EnvironmentType);
              }
            }//�߿� �̺�Ʈ�� ȯ�� ������ ������
            else
            {
              foreach(var _data in SettleIcons)
              {
                if(_data.SettlementData.CheckAbleEvent(_event)&&!_ablesettles.Contains(_data))_ablesettles.Add(_data);
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
              if (_climaxevent.EnvironmentType!=EnvironmentType.NULL)
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
                if(_data.SettlementData.CheckAbleEvent(_climaxevent))_availableclimax.Add(_data);
              }
              SettlementIcon _selectclimax = _availableclimax[Random.Range(0, _availableclimax.Count)];
              _ablesettles.Add(_selectclimax);
              _currentquest.NextQuestSettlement = _selectclimax.SettlementData;
            }//������ �̺�Ʈ�� ������ �ϳ� ��������
          }//���� �������� �������� ���� ���¶�� ���� ���� �Է�
          else
          {
            if (_currentquest.NextQuestSettlement != null)
              _ablesettles.Add(GetSettleIcon(_currentquest.NextQuestSettlement.OriginName));
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
              if (_fallingevent.EnvironmentType!=EnvironmentType.NULL)
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
                if (_data.SettlementData.CheckAbleEvent(_fallingevent)) _availablefalling.Add(_data);
              }
              SettlementIcon _selectfalling = _availablefalling[Random.Range(0, _availablefalling.Count)];
              _ablesettles.Add(_selectfalling);
              _currentquest.NextQuestSettlement = _selectfalling.SettlementData;
            }//������ �̺�Ʈ�� ������ �ϳ� ��������
          }//���� �������� �������� ���� ���¶�� ���� ���� �Է�
          else
          {
            if (_currentquest.NextQuestSettlement != null)
              _ablesettles.Add(GetSettleIcon(_currentquest.NextQuestSettlement.OriginName));
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

    if(SelectedArrow!=null) UnpText.text = GameManager.Instance.MyGameData.AllSettleUnpleasant[SelectedSettle.OriginName].ToString();


    if (GameManager.Instance.MyGameData.CanMove)  //�̵� ������ ��Ȳ(�߿ܿ��� �̺�Ʈ�� �Ϸ��� ��Ȳ, ���������� �̺�Ʈ�� �Ϸ��� ��Ȳ)
    {
      if (GameManager.Instance.MyGameData.CurrentSettlement != null)//���� �������� ������ �ƴϴ�? ���������� ����ϴ� ��Ȳ
      {
        int _activearrowcount = 0;
        foreach (var _group in ArrowGroup)
          if (_group.alpha.Equals(1.0f)) _activearrowcount++;

        if (_activearrowcount <= 1)
        {
          SetArrow();
        }//���������� ������ �����µ� Ȱ��ȭ�� ȭ��ǥ�� �ϳ����̶�� ���� ���� ��������� �ϴ� ��Ȳ

        SetMoveButton(0);
      }
      else SetMoveButton(4);                                        //���� �������� ����? �߿� �̺�Ʈ�� ������ ��� �̵��ؾ� �ϴ� ��Ȳ
    }
    else  //�̵� �Ұ����� ��Ȳ(�߿� �̺�Ʈ ������ Ȥ�� ������ �̺�Ʈ ������)
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
    SelectedArrow.deactivecolor();
    SelectedArrow = null;
   yield return StartCoroutine(UIManager.Instance.ChangeAlpha(MyGroup,0.0f,0.2f, false));
    for(int i = 0; i < EnvirIcons.Count; i++)
    {
      if (EnvirIcons[i].activeInHierarchy.Equals(true)) EnvirIcons[i].SetActive(false);
    }
    var _settleicons = SettleIcons;
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
    List<SettlementIcon> _targetsettles=new List<SettlementIcon>();
    foreach (var _data in GameManager.Instance.MyGameData.AvailableSettles)
    {
      _targetsettles.Add(GetSettleIcon(_data.OriginName));
      Debug.Log(_data.OriginName);
    }

    for (int i = 0; i < _targetsettles.Count; i++)
    {

      Vector2 _targetpos = _targetsettles[i].GetComponent<RectTransform>().anchoredPosition;
      ArrowRect[i].anchoredPosition = PlayerRect.anchoredPosition;

      float _length = Mathf.Clamp(Vector3.Distance(PlayerRect.anchoredPosition,_targetpos), 0.0f, 5000.0f);
      ArrowRect[i].sizeDelta = new Vector2(100.0f, _length);

      Vector2 _dir = PlayerRect.anchoredPosition - _targetpos;
      Vector3 _angle = Vector3.back * (Mathf.Atan2(_dir.x, _dir.y) * Mathf.Rad2Deg + 180.0f);
      ArrowRect[i].rotation = Quaternion.Euler(_angle);

      ArrowButtonScript[i].Setup(PlayerRect.anchoredPosition, new Vector2(100.0f, _length), _angle, _targetsettles[i]);
      
    }
  }
  public void UpdatePanel(Settlement _settle,MapArrowButton _arrow)
  {
    UIManager.Instance.AddUIQueue(updatepanel(_settle, _arrow));
  }//�̵� ����ǥ�� ���� �������� ������ ��Ȳ
  private IEnumerator updatepanel(Settlement _settle, MapArrowButton _arrow)
  {
    if (_settle == null)
    {
      UIManager.Instance.IsWorking = false;
      Cancle();
      //���� �÷��̾�� ���ư��� ���, �̵� ��ư ��Ȱ��ȭ
    }//������ ���� ���
    else if (SelectedArrow != null&& SelectedArrow != _arrow)
    {
      SelectedArrow.deactivecolor();

      SelectedArrow = _arrow;
      SettleName.text = _settle.Name;
      UnpText.text = GameManager.Instance.MyGameData.AllSettleUnpleasant[_settle.OriginName].ToString();

      SelectedSettleCost = GameManager.Instance.MyGameData.GetMoveSanityValue(Vector2.Distance(GameManager.Instance.MyGameData.CurrentPos, SelectedSettle.VectorPos));
      Vector2 _settlerectpos = SelectedSettleIcon.GetComponent<RectTransform>().anchoredPosition;
      GameManager.Instance.MyGameData.MoveTargetPos = _settlerectpos;

      yield return StartCoroutine(movecameratosettle(GetScaleChangerPos(_settlerectpos)));
      SetMoveButton(1);
    }//���� ��ư�� ���� ī�޶� �̵��� ���¿��� �ٸ� ���� ��ư�� ���� ���
    else
    {
      SelectedArrow = _arrow;
      SettleName.text = _settle.Name;
      UnpText.text = GameManager.Instance.MyGameData.AllSettleUnpleasant[_settle.OriginName].ToString();
    
      SelectedSettleCost = GameManager.Instance.MyGameData.GetMoveSanityValue(Vector2.Distance(GameManager.Instance.MyGameData.CurrentPos, SelectedSettle.VectorPos));
      Vector2 _settlerectpos = SelectedSettleIcon.GetComponent<RectTransform>().anchoredPosition;
      GameManager.Instance.MyGameData.MoveTargetPos = _settlerectpos;

      yield return StartCoroutine(movecameratosettle(GetScaleChangerPos(_settlerectpos)));
      SetMoveButton(1);
      // ��ǥ �������� ī�޶� �̵�
    }//������ ����
  }
  public void Cancle()
  {
    if (UIManager.Instance.IsWorking) return;
    UIManager.Instance.AddUIQueue(cancle());
  }
  private IEnumerator cancle()
  {
    Debug.Log("���� ��ư ���");
    SelectedArrow.deactivecolor();
    SelectedArrow = null;
    SettleName.text = "";
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
  }//�÷��̾�->�������� ���� �̵�(ȭ��ǥ Ŭ������ ��)
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
  }//������->�÷��̾�� ���� �̵�(�̵� ������ ��, �̵� ������� ��)
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
    //�ٸ� UI�� �����̰� ������ �۵� ����

    if(GameManager.Instance.MyGameData.CanMove==false) return;
    //������ �� ���� ��Ȳ(�̺�Ʈ ���� ȭ��,�̺�Ʈ ����)�̶�� �� �����δٴ� UI ���
    if (SelectedSettle == null) return;
    //������ �� �ִ� ��Ȳ����, ���������� ����� �� ��ǥ �������� �������� ���� ��� ��ǥ ������ ���ٴ� UI ���
    SelectedArrow.MyGroup.interactable = false;
    UIManager.Instance.ResetEventPanels();
    UIManager.Instance.AddUIQueue(movemap());
  }//�̵� ��ư�� �������� �ش� ������, �ش� ȭ��ǥ�� ������ �ٸ� ������ ��ư ��Ȱ��ȭ �� �ٸ� ȭ��ǥ ��Ȱ��ȭ
  private IEnumerator movemap()
  {
    yield return StartCoroutine(movetoplayer());
   // Ÿ�ٵ� ���������� �÷��̾�� ī�޶� �̵�
    for(int i=0;i<SettleIcons.Count;i++)
      if (SettleIcons[i] != SelectedSettleIcon)
      {
        SettleIcons[i].DisableButton();
      }
    for (int i = 0; i < ArrowButtonScript.Length; i++)
      if (SelectedArrow != ArrowButtonScript[i])
      {
        StartCoroutine(UIManager.Instance.ChangeAlpha(ArrowGroup[i], 0.0f, 0.3f, false));
      }
    //�̵� �����̴� �̵� ��ư�� ��Ȱ��ȭ
    GameManager.Instance.MyGameData.VisitedPlaces.Clear();
    SetMoveButton(2);
    Vector3 _playerrectpos = PlayerRect.anchoredPosition;          //���� ��ġ(UI)
    Vector3 _settlerectpos = SelectedSettleIcon.GetComponent<RectTransform>().anchoredPosition;           //���� ��ġ(UI)

    Vector3 _playretilepos = GameManager.Instance.MyGameData.CurrentPos;//���� ��ġ(Ÿ��)
    Vector3 _settletilepos = (Vector3)SelectedSettleIcon.SettlementData.VectorPos;//���� ��ġ(Ÿ��)

    float _currentprogress = GameManager.Instance.MyGameData.MoveProgress;//���� �̵� ���൵ 0.0f ~ 1.0f
    float _targetprogress = 0.0f;                                             //�̹� �̵����� ��ǥ�� �ϴ� �̵� ���൵
    if (_currentprogress == 0.0f) _targetprogress = Random.Range(0.3f, 0.7f);
    else _targetprogress = 1.0f;
    Vector3 _targetrectpos = Vector3.Lerp(_playerrectpos, _settlerectpos, _targetprogress);  //�̹� �̵����� ��ǥ�� �ϴ� ��ġ(UI)
    Vector3 _targettilepos = Vector3.Lerp(_playretilepos, _settletilepos, _targetprogress);    //�̹� �̵����� ��ǥ�� �ϴ� ��ġ(Ÿ��)
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

      yield return new WaitForSeconds(1.0f);

      StartCoroutine(UIManager.Instance.CloseUI(MyGroup, true, false));
      //ĳ���� �̵���Ŵ
      //�̵�+����+����

      EventManager.Instance.SetOutsideEvent(MapCreater.GetSingleTileData(_targetrectpos));
      //ĳ���� ���� ��ġ ���� 1ĭ ��,��,���,��,�ٴ� ���� �ľ��ؼ� EventManager�� ������
      IsOpen = false;
      //���߿� ����Ŵϱ� �̵� ��ư Ȱ��ȭ�� ����
    }
    else//�߿� ~ ������
    {
      GameManager.Instance.MyGameData.CurrentEvent = null;
      GameManager.Instance.MyGameData.CurrentPos = _targettilepos;
      GameManager.Instance.MyGameData.MoveProgress = 0.0f;
      GameManager.Instance.MyGameData.CurrentSettlement = SelectedSettle;
      GameManager.Instance.MyGameData.CurrentSettlement.SetAvailablePlaces();
     GameManager.Instance.MyGameData.AvailableSettles = GameManager.Instance.MyMapData.GetCloseSettles(GameManager.Instance.MyGameData.CurrentSettlement, 3);

      SelectedSettle.LibraryType = (ThemeType)Random.Range(0, 4);
      //��ǥ�� ������ ���������� �̵� ���� ������ �ʱ�ȭ

      //currentprogress!=0.0f�� �ܺο��� �̺�Ʈ Ŭ�����ϰ� ���� �������� ���� �ٽ� ���
      yield return StartCoroutine(movecharacter(_playerrectpos, _settlerectpos, _currentprogress));

      yield return new WaitForSeconds(1.0f);

      yield return StartCoroutine(UIManager.Instance.CloseUI(MyGroup, true, false));
      //ĳ���� �̵���Ŵ(�̵�+����+����)
      IsOpen = false;
      //�������� �ٷ� �� �ݱ�

      EventManager.Instance.SetSettleEvent(SelectedSettle.TileData);
      //ĳ���� ��ǥ ���� ������ ���� ������
      GameManager.Instance.MyGameData.Turn++;
      UIManager.Instance.UpdateTurnIcon();
      //�������� ���������� ���ο� ��ǥ ��� 3�� Ȱ��ȭ
      if (SelectedSettle != null) SelectedArrow.DeActive();
      SelectedArrow = null;
    }
    Debug.Log("�̵� �ڷ�ƾ�� ���� ����~");
  }
  private IEnumerator movecharacter(Vector3 _originrectpos,Vector3 _targetrectpos,Vector3 _maxpos, float _targetprogress)
  {
    StartCoroutine(zoominview(_targetrectpos));
    float _time = 0.0f;
    while (_time < MoveTime)
    {
      PlayerRect.anchoredPosition=Vector3.Lerp(_originrectpos, _targetrectpos, UIManager.Instance.CharacterMoveCurve.Evaluate(_time / MoveTime));
      ScaleChanger.anchoredPosition= GetScaleChangerPos(Vector2.Lerp(_originrectpos,_targetrectpos,FollowMoveCurve.Evaluate(_time/ MoveTime)));
      _time += Time.deltaTime;
      yield return null;
    }
    PlayerRect.anchoredPosition = _targetrectpos;
    ScaleChanger.anchoredPosition = GetScaleChangerPos(_targetrectpos);
    //0.0f ~ _targetprogress �������� ������
  }//ĳ���� �����̴� �ڷ�ƾ - ������ ~ �߿� �̺�Ʈ
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
  }//������,�߿� �̵� �� ���� ���� �ϴ� �ڷ�ƾ
  private IEnumerator movecharacter(Vector3 _currenrecttpos, Vector3 _endrectpos, float _currentprogress)
  {
    StartCoroutine(zoominview(_endrectpos));
    //��� ��ǥ, �� ��ǥ�� ���� ���൵(_progress)�� ����� 0.1ĭ ������ ����� 10�踦 ����
    //_progress 0.0f ����(������)�� �����Ѵ�
    float _time =0.0f;
    //���� _time�� 0.0~MoveTime�� _currentprogress��ŭ
    while (_time < MoveTime)
    {
      PlayerRect.anchoredPosition = Vector3.Lerp(_currenrecttpos, _endrectpos, UIManager.Instance.CharacterMoveCurve.Evaluate(_time / MoveTime));
      ScaleChanger.anchoredPosition = GetScaleChangerPos(Vector2.Lerp(_currenrecttpos, _endrectpos, FollowMoveCurve.Evaluate(_time / MoveTime)));
      _time += Time.deltaTime; yield return null;
    }
    PlayerRect.anchoredPosition = _endrectpos;
    ScaleChanger.anchoredPosition = GetScaleChangerPos(_endrectpos);
    //targetprogress ~ 1.0f �������� �̵�

  }//ĳ���� �����̴� �ڷ�ƾ - �߿� �̺�Ʈ ~ ������
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
  }//���� ������ �� �� �� ���¶�� �ܾƿ� ��Ű�� �ڷ�ƾ
  public void UpdateIcons(List<Settlement> _settles)
  {
    foreach (Settlement _settle in _settles)
    {
    }
  }
  public void SetPlayerPos(Settlement _targetsettle)
  {
    ScaleChanger.localScale = Vector3.one;
    Vector3 _targetpos = GetSettleIcon(_targetsettle.OriginName).GetComponent<RectTransform>().anchoredPosition;
   // Debug.Log($"{_targetsettle.VectorPos}  {_targetsettle.Name}  {_targetpos}");
    PlayerRect.anchoredPosition= _targetpos;
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

    Vector3 _unit = _onepos - _zeropos; //(0,0),(1,1)�� UI�� �ű� ��ǥ�� ���� UI �� �Ÿ� 1�� Rect ������ ����
    Vector3Int _intpos = new Vector3Int(Mathf.FloorToInt(_tilepos.x), Mathf.FloorToInt(_tilepos.y));
    Vector3 _else=new Vector3(_tilepos.x-_intpos.x,_tilepos.y-_intpos.y);
    //Ÿ�� ��ǥ�� ����, �������� �и�

    PlayerRect.position = MapCreater.Tilemap_bottom.CellToWorld(_intpos);
    PlayerRect.position = PlayerRect.position + new Vector3(_unit.x * _else.x, _unit.y * _else.y);
    PlayerRect.SetParent(_originparent);
    //���� ������ ������ CelltoWorld�� �̸� ���� ���س��� �Ǽ��� ���� ���� �̵�
    ScaleChanger.localScale = IdleScale;*/
  }//������ �ƴ� ��ǥ�� �޾Ƽ� �ű�� �ű�°�
}
