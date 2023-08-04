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
        MoveButtonText.text = GameManager.Instance.GetTextData("choosesettle");      //���� ���� ���� ��ȹ �Ϸ� �� ���� ���
        CancleButton.interactable = false;
        break;
      case 1:
        Debug.Log("�̵� ���� ����");
        if (MoveButton.interactable.Equals(false)) MoveButton.interactable = true;
        if (CancleButton.interactable.Equals(false)) CancleButton.interactable = true;
        MoveButtonText.text = GameManager.Instance.GetTextData("canmove");
        CancleButton.interactable = true;
        break;
      case 2:
        Debug.Log("�̵� ��");
        if (MoveButton.interactable.Equals(true)) MoveButton.interactable = false;
        if (CancleButton.interactable.Equals(true)) CancleButton.interactable = false;
        MoveButtonText.text = GameManager.Instance.GetTextData("duringmoving");
        CancleButton.interactable = false;
        break;
      case 3:
        Debug.Log("�̵� �Ұ� ����");
        if (MoveButton.interactable.Equals(true)) MoveButton.interactable = false;
        if (CancleButton.interactable.Equals(true)) CancleButton.interactable = false;
        MoveButtonText.text = GameManager.Instance.GetTextData("cannotmove");
        CancleButton.interactable = false;
        break;
      case 4:
        Debug.Log("��� �̵� ���� ����");
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

    //                                                                                                        ����Ʈ ��ȹ �� ������ ����Ʈ ���� ǥ�� �ֱ�


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
      //���� �÷��̾�� ���ư��� ���, �̵� ��ư ��Ȱ��ȭ
    }//������ ���� ���
    else if (SelectedTile != null&&targettile.TileSettle!=null)
    {

      //   SelectedSettleCost = GameManager.Instance.MyGameData.GetMoveSanityValue(Vector2.Distance(GameManager.Instance.MyGameData.CurrentPos, SelectedSettle.Coordinate));
      Vector2 _targetrecrpos = targettile.Rect.anchoredPosition;

      yield return StartCoroutine(movecameratosettle(GetScaleChangerPos(_targetrecrpos)));
      SetMoveButton(1);
    }//���� ��ư�� ���� ī�޶� �̵��� ���¿��� �ٸ� ���� ��ư�� ���� ���
  }
  public void Cancle()
  {
    if (UIManager.Instance.IsWorking) return;
    UIManager.Instance.AddUIQueue(cancle());
  }
  private IEnumerator cancle()
  {
    Debug.Log("���� ��ư ���");
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
    if (SelectedTile == null) return;
    //������ �� �ִ� ��Ȳ����, ���������� ����� �� ��ǥ �������� �������� ���� ��� ��ǥ ������ ���ٴ� UI ���
    UIManager.Instance.ResetEventPanels();
    UIManager.Instance.AddUIQueue(movemap());
  }//�̵� ��ư�� �������� �ش� ������, �ش� ȭ��ǥ�� ������ �ٸ� ������ ��ư ��Ȱ��ȭ �� �ٸ� ȭ��ǥ ��Ȱ��ȭ
  private IEnumerator movemap()
  {
    yield return StartCoroutine(movetoplayer());
   // Ÿ�ٵ� ���������� �÷��̾�� ī�޶� �̵�

    //�̵� �����̴� �̵� ��ư�� ��Ȱ��ȭ
    SetMoveButton(2);
    Vector2 _playerrectpos = PlayerRect.anchoredPosition;                 //���� ��ġ(Rect)
    Vector2 _targettilepos = SelectedTile.Rect.anchoredPosition;            //���� ��ġ(Rect)

    Vector2 _startcoor = GameManager.Instance.MyGameData.Coordinate;//���� ��ġ(Ÿ��)
    Vector2 _endcoor = SelectedTile.Coordinate;//���� ��ġ(Ÿ��)

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
        //ĳ���� ���� ��ġ ���� 1ĭ ��,��,���,��,�ٴ� ���� �ľ��ؼ� EventManager�� ������
        IsOpen = false;
        //���߿� ����Ŵϱ� �̵� ��ư Ȱ��ȭ�� ����

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
        //ĳ���� �̵���Ŵ
        //�̵�+����+����

    Debug.Log("�̵� �ڷ�ƾ�� ���� ����~");
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
