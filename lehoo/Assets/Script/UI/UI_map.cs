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
  [HideInInspector] public GameObject CityIcon = null;
  [HideInInspector] public List<GameObject> TownIcons = new List<GameObject>();
  [HideInInspector] public List<GameObject> VillageIcons = new List<GameObject>();
  public GameObject GetSettleIcon(Settlement settlement)
  {
    string _originname = settlement.OriginName;
    switch (settlement.Type)
    {
      case SettlementType.City:return CityIcon;
      case SettlementType.Town:
        foreach (GameObject city in TownIcons)
          if (city.name.Contains(_originname)) return city;
        return null;
      case SettlementType.Village:
        foreach(GameObject town in VillageIcons)
          if(town.name.Contains(_originname)) return town;
        return null;
    }
    Debug.Log("���� �̻��� ����~");
    return null;
  }
  public float AppearTime = 0.5f;
  [SerializeField] private AnimationCurve ZoomInCurve = null;
  [SerializeField] private RectTransform HolderRect = null;
  [SerializeField] private RectTransform ScaleRect = null;
  private Vector3 IdleScale = Vector3.one;
  [SerializeField] private Vector3 ZoomInScale = Vector3.one* 1.5f;
  [SerializeField] private float ZoomInTime = 1.2f;

  [SerializeField] private TextMeshProUGUI ClickInfoText = null;

  [SerializeField] private GameObject MoveCostInfoHolder = null;
  [SerializeField] private Image MovePointImage = null;
  [SerializeField] private TextMeshProUGUI MovePointText = null;
  [SerializeField] private GameObject MoveCostTypeHolder = null;

  [SerializeField] private RectTransform SettleInfoRect = null;
  [SerializeField] private CanvasGroup SettleInfoGroup = null;
  [SerializeField] private TextMeshProUGUI SettleNameText = null;
  [SerializeField] private TextMeshProUGUI DiscomfortCountText = null;
  public Transform SelectTileHolder = null;

  public Color NormalColor = Color.white;
  public Color DisableColor = Color.grey;
  private List<Settlement> ActiveSettles = new List<Settlement>();
  private List<TileData> ActiveTileData = new List<TileData>();
  public int SelectProgress = 0;//0: Ÿ�� ����(��ǥ ����)  1: ���� ��� ����  2:Ÿ�� ����(Ȯ��,���)

  /// <summary>
  /// ���� 2ĭ Ÿ�� ������Ʈ
  /// </summary>
  private void ResetEnableTiles()
  {
    List<TileData> _currents = GameManager.Instance.MyGameData.MyMapData.GetAroundTile(GameManager.Instance.MyGameData.Coordinate, 2);
    List<Settlement> _currentsettles = new List<Settlement>();
    foreach (TileData _tile in _currents) //���� 2ĭ Ÿ�� ���� ��������
    {
      if (!ActiveTileData.Contains(_tile))
      {
        ActiveTileData.Add(_tile);
        _tile.ButtonScript.Button.interactable = true;
        if (_tile.ButtonScript.TopEnvirImage != null) _tile.ButtonScript.TopEnvirImage.color= NormalColor;
      }//���� Ȱ��ȭ Ÿ�� ��Ͽ� ���� Ÿ���̶�� ����Ʈ�� �߰��ϰ� Ȱ��ȭ

      if (_tile.TileSettle != null)
      {
        _currentsettles.Add(_tile.TileSettle);
        if (!ActiveSettles.Contains(_tile.TileSettle))
        {
          ActiveSettles.Add(_tile.TileSettle);
          StartCoroutine(UIManager.Instance.ChangeAlpha(GetSettleIcon(_tile.TileSettle).GetComponent<CanvasGroup>(), 1.0f, 0.8f,false));
        }
      }//���� Ȱ��ȭ ������ ��Ͽ� ���� Ÿ���̶�� ����Ʈ�� �߰��ϰ� Ȱ��ȭ
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
      }//ȭ�鿡�� ����� Ÿ�ϵ��� ��Ȱ��ȭ ���·� ��ȯ
    }

    foreach (Settlement _settle in ActiveSettles)
    {
      if (!_currentsettles.Contains(_settle))
      {
        _removesettles.Add(_settle);
        StartCoroutine(UIManager.Instance.ChangeAlpha(GetSettleIcon(_settle).GetComponent<CanvasGroup>(), 0.0f, 0.3f, false));
      }//ȭ�鿡�� ����� ���������� ����ȭ
    }

    foreach(TileData _tile in _removetiles)ActiveTileData.Remove(_tile);
    foreach(Settlement _settle in _removesettles)ActiveSettles.Remove(_settle);
  }
  
  public void OpenUI()
  {
    IsOpen = true;
    UIManager.Instance.AddUIQueue(openui());
  }
  
  private IEnumerator openui()
  {
    if(MoveCostTypeHolder.activeInHierarchy==true) MoveCostTypeHolder.SetActive(false);
    MoveCostInfoHolder.SetActive(false);
    MovePointImage.sprite=GameManager.Instance.MyGameData.MovePoint>0?GameManager.Instance.ImageHolder.MovePointIcon_Enable:GameManager.Instance.ImageHolder.MovePointIcon_Lack;
    MovePointText.text = GameManager.Instance.MyGameData.MovePoint > 0 ? GameManager.Instance.GetTextData("ENOUGHMOVEPOINT") : GameManager.Instance.GetTextData("NOTENOUGHMOVEPOINT");
    HolderRect.anchoredPosition = Vector2.zero;
    if (DefaultGroup.alpha == 1.0f) DefaultGroup.alpha = 0.0f;
    if (DefaultGroup.interactable == true) DefaultGroup.interactable = false;
    if (DefaultGroup.blocksRaycasts == true) DefaultGroup.blocksRaycasts = false;

    ClickInfoText.text = GameManager.Instance.GetTextData("CHOOSETILE_MAP");

    ScaleRect.localScale = IdleScale;
    ScaleRect.anchoredPosition = Vector2.zero;

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
  public int SanityCost = 0, GoldCost = 0;
  public void SelectTile(TileData tiledata,Vector2 position)
  {
    TileData _currenttile = GameManager.Instance.MyGameData.MyMapData.Tile(GameManager.Instance.MyGameData.Coordinate);
    int _length = GameManager.Instance.MyGameData.MyMapData.GetLength(_currenttile, SelectedTile);
    switch (SelectProgress)
    {
      case 0: //0�ܰ�� Ÿ���� �ϳ��� �������� ���� �����̹Ƿ� ���� ��� �������� �Ѿ��
        SelectedTile = tiledata;
        ClickInfoText.text = GameManager.Instance.GetTextData("CHOOSECOSTTYPE_MAP");

        SanityCost = GameManager.Instance.MyGameData.GetMoveSanityCost(_length);
        GoldCost = GameManager.Instance.MyGameData.GetMoveGoldCost(_length);

        if (SelectedTile.TileSettle != null)
        {
          SettleInfoRect.position = position;
          SettleInfoRect.anchoredPosition3D = new Vector3(SettleInfoRect.anchoredPosition.x, SettleInfoRect.anchoredPosition.y, 0.0f);
          SettleInfoGroup.alpha = 1.0f;

          SettleNameText.text = SelectedTile.TileSettle.Name;
          DiscomfortCountText.text = "<#5F04B4>" + SelectedTile.TileSettle.Discomfort.ToString() + "</color>";
        }
        else
        {
          SettleInfoGroup.alpha = 0.0f;
        }

        SelectProgress=1;  //0->1
        break;
      case 1:
        if (SelectedTile == tiledata)
        {
          SelectedTile.ButtonScript.CancleTile();
          SelectedTile = null;
          ClickInfoText.text = GameManager.Instance.GetTextData("CHOSSETILEAGAIN_MAP");
          if (MoveCostTypeHolder.activeInHierarchy == true) MoveCostTypeHolder.SetActive(false);
          if (SettleInfoGroup.alpha == 1.0f) SettleInfoGroup.alpha = 0.0f;
          SelectProgress = 0;
          return;
        }//������ Ÿ�� ���������� ���� �ܰ�� ȸ��(Ÿ�� ����)  0<-1
        else //�ٸ� Ÿ�� ���������� ������ ���� (1�ܰ�)
        {
          SelectedTile = tiledata;

          SanityCost = GameManager.Instance.MyGameData.GetMoveSanityCost(_length);
          GoldCost = GameManager.Instance.MyGameData.GetMoveGoldCost(_length);

          if (SelectedTile.TileSettle != null)
          {
            SettleInfoRect.position = position;
            SettleInfoRect.anchoredPosition3D = new Vector3(SettleInfoRect.anchoredPosition.x, SettleInfoRect.anchoredPosition.y, 0.0f);
            SettleInfoGroup.alpha = 1.0f;

            SettleNameText.text = SelectedTile.TileSettle.Name;
            DiscomfortCountText.text = "<#5F04B4>" + SelectedTile.TileSettle.Discomfort.ToString() + "</color>";
          }
          else
          {
            SettleInfoGroup.alpha = 0.0f;
          }
        }
        break;
      case 2:
        if (SelectedTile == tiledata)
        {
          MoveMap();
        }//2�ܰ迡�� ��ǥ Ÿ�� Ŭ���ϸ� �̵�
        else//2�ܰ�(����ǥ��,Ÿ��Ŭ���ϼ�)���� �ٸ� Ÿ�� Ŭ���ϸ� 1�ܰ�� ȸ��(���� ��ư ǥ��) 1<-2
        {
          SelectedTile = tiledata;

          SanityCost = GameManager.Instance.MyGameData.GetMoveSanityCost(_length);
          GoldCost = GameManager.Instance.MyGameData.GetMoveGoldCost(_length);

          if (SelectedTile.TileSettle != null)
          {
            SettleInfoRect.position = position;
            SettleInfoRect.anchoredPosition3D = new Vector3(SettleInfoRect.anchoredPosition.x, SettleInfoRect.anchoredPosition.y, 0.0f);
            SettleInfoGroup.alpha = 1.0f;

            SettleNameText.text = SelectedTile.TileSettle.Name;
            DiscomfortCountText.text = "<#5F04B4>" + SelectedTile.TileSettle.Discomfort.ToString() + "</color>";
          }
          else
          {
            SettleInfoGroup.alpha = 0.0f;
          }

          MoveCostInfoHolder.SetActive(false);
          MoveCostTypeHolder.SetActive(true);
          SelectProgress = 1;
        }
        break;
    }
  }
  public void CancleTile()
  {
    SelectedTile = null;
    SettleInfoGroup.alpha = 0.0f;
  }
  public void SelectCostType_Sanity()
  {
    SelectedCostType = StatusType.Sanity;

    string _movecost = GameManager.Instance.MyGameData.MovePoint > 0 ?
      string.Format(GameManager.Instance.GetTextData("MOVECOST_ENOUGH"), "<#FFBF00>-1</color>", GameManager.Instance.GetTextData(StatusType.Sanity, 2), WNCText.GetSanityColor("-" + SanityCost)) :
      string.Format(GameManager.Instance.GetTextData("MOVECOST_NOTENOUGH"), GameManager.Instance.GetTextData(StatusType.Sanity, 2), WNCText.GetSanityColor("-" + SanityCost));
  
    if (MoveCostInfoHolder.activeInHierarchy == false) MoveCostInfoHolder.SetActive(true);
    MoveCostTypeHolder.SetActive(false);

    SelectProgress=2;
  }
  public void SelectCostType_Gold()
  {
    if (GameManager.Instance.MyGameData.Gold <= GoldCost) return;
    SelectedCostType = StatusType.Gold;

    string _movecost = GameManager.Instance.MyGameData.MovePoint > 0 ?
      string.Format(GameManager.Instance.GetTextData("MOVECOST_ENOUGH"), "<#FFBF00>-1</color>", GameManager.Instance.GetTextData(StatusType.Gold, 2), WNCText.GetGoldColor("-" + GoldCost)) :
      string.Format(GameManager.Instance.GetTextData("MOVECOST_NOTENOUGH"), GameManager.Instance.GetTextData(StatusType.Gold, 2), WNCText.GetGoldColor("-" + GoldCost));

    if (MoveCostInfoHolder.activeInHierarchy == false) MoveCostInfoHolder.SetActive(true);
    MoveCostTypeHolder.SetActive(false);

    SelectProgress = 2;
  }
  public StatusType SelectedCostType = StatusType.Sanity;
  public void MoveMap()
  {
    SettleInfoGroup.alpha = 0.0f;
    DefaultGroup.blocksRaycasts = true;
    UIManager.Instance.ResetEventPanels();
    UIManager.Instance.AddUIQueue(movemap());
  }//�̵� ��ư�� �������� �ش� ������, �ش� ȭ��ǥ�� ������ �ٸ� ������ ��ư ��Ȱ��ȭ �� �ٸ� ȭ��ǥ ��Ȱ��ȭ
  private IEnumerator movemap()
  {

    Vector2 _playerrectpos = PlayerRect.anchoredPosition;                 //���� ��ġ(Rect)
    Vector2 _targettilepos = SelectedTile.Rect.anchoredPosition;            //���� ��ġ(Rect)

    Vector2 _startcoor = GameManager.Instance.MyGameData.Coordinate;//���� ��ġ(Ÿ��)
    Vector2 _endcoor = SelectedTile.Coordinate;//���� ��ġ(Ÿ��)

    GameManager.Instance.MyGameData.ClearBeforeEvents();

    switch (SelectedCostType)
    {
      case StatusType.Sanity:
        if (GameManager.Instance.MyGameData.MaxSanity <= ConstValues.MadnessDefaultSanityLose)
        {
          GameManager.Instance.MyGameData.CurrentSanity -= (GameManager.Instance.MyGameData.CurrentSanity - 1);
        }
        else
        {
          GameManager.Instance.MyGameData.CurrentSanity -= SanityCost;
        }
        break;
      case StatusType.Gold:
        GameManager.Instance.MyGameData.Gold -= GoldCost;
        break;
    }
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
        //ĳ���� ���� ��ġ ���� 1ĭ ��,��,���,��,�ٴ� ���� �ľ��ؼ� EventManager�� ������
        IsOpen = false;
        break;

      case LandscapeType.Settlement:
        GameManager.Instance.EnterSettlement(SelectedTile.TileSettle);
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
    Debug.Log("�̵� �ڷ�ƾ�� ���� ����~");
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
  }//������,�߿� �̵� �� ���� ���� �ϴ� �ڷ�ƾ
  public void SetPlayerPos(Vector2 coordinate)
  {
    Debug.Log("���� �÷��̾� ��ġ ����");
    TileData _targettile = GameManager.Instance.MyGameData.MyMapData.Tile(coordinate);
    PlayerRect.anchoredPosition = _targettile.Rect.anchoredPosition;
    ScaleRect.localScale =IdleScale;
    HolderRect.anchoredPosition = PlayerRect.anchoredPosition * -1.0f;
    GameManager.Instance.MyGameData.Coordinate = coordinate;
    ResetEnableTiles();
  }
  public override void CloseUI()
  {
    SelectProgress = 0;
    UIManager.Instance.AddUIQueue(closeui());
    IsOpen = false;
  }
  private IEnumerator closeui()
  {
    DefaultGroup.blocksRaycasts = false;
    yield return StartCoroutine(UIManager.Instance.moverect(GetPanelRect("myrect").Rect, GetPanelRect("myrect").InsidePos, GetPanelRect("myrect").OutisdePos, 0.3f, UIManager.Instance.UIPanelCLoseCurve));
    DefaultGroup.interactable = false;
  }
  public override void CloseForGameover()
  {
    SelectProgress = 0;
    DefaultGroup.blocksRaycasts = false;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("myrect").Rect, GetPanelRect("myrect").Rect.anchoredPosition, GetPanelRect("myrect").OutisdePos, 0.3f, UIManager.Instance.UIPanelCLoseCurve));
  }

}
