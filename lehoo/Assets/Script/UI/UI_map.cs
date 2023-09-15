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
    switch (settlement.SettlementType)
    {
      case SettlementType.City:return CityIcon;
      case SettlementType.Town:
        foreach (GameObject town in TownIcons)
          if (town.name.Contains(_originname)) return town;
        return null;
      case SettlementType.Village:
        foreach(GameObject village in VillageIcons)
          if(village.name.Contains(_originname)) return village;
        return null;
    }
    Debug.Log("���� �̻��� ����~");
    return null;
  }
  public float AppearTime = 1.2f;
  [SerializeField] private AnimationCurve ZoomInCurve = null;
  [SerializeField] private RectTransform HolderRect = null;
  [SerializeField] private RectTransform ScaleRect = null;
  private Vector3 IdleScale = Vector3.one;
  [SerializeField] private Vector3 ZoomInScale = Vector3.one* 1.5f;
  [SerializeField] private float ZoomInTime = 1.2f;

  [SerializeField] private CanvasGroup MoveInfoGroup = null;
  [SerializeField] private TextMeshProUGUI ProgressGuideText = null;
  [SerializeField] private Image TilePreview_Bottom = null;
  [SerializeField] private Image TilePreview_Top = null;
  [SerializeField] private Image TilePreview_Landmark = null;
  [SerializeField] private GameObject SettlementInfoHolder = null;
  [SerializeField] private TextMeshProUGUI SettlementNameText = null;
  [SerializeField] private TextMeshProUGUI DiscomfortText = null;
  [SerializeField] private CanvasGroup MovecostButtonGroup = null;
  [SerializeField] private CanvasGroup SanitybuttonGroup = null;
  [SerializeField] private CanvasGroup GoldbuttonGroup = null;
  [SerializeField] private PreviewInteractive GoldButtonPreview = null;
  private float MoveButtonDisableAlpha = 0.2f;
  private StatusType selectedcosttype = StatusType.HP;
  public StatusType SelectedCostType
  {
    get { return selectedcosttype; }
    set
    {
      selectedcosttype = value;
      switch (selectedcosttype)
      {
        case StatusType.Sanity:
          SanitybuttonGroup.alpha = 1.0f;
          GoldbuttonGroup.alpha = MoveButtonDisableAlpha;

          MoveCostText.text = GameManager.Instance.MyGameData.MovePoint > 0 ?
      string.Format(GameManager.Instance.GetTextData("MOVECOST_ENOUGH"), "<#FFBF00>-1</color>", GameManager.Instance.GetTextData(StatusType.Sanity, 2), WNCText.GetGoldColor("-" + SanityCost)) :
      string.Format(GameManager.Instance.GetTextData("MOVECOST_NOTENOUGH"), GameManager.Instance.GetTextData(StatusType.Sanity, 2), WNCText.GetGoldColor("-" + SanityCost));

          if(MoveButton.gameObject.activeInHierarchy==false)MoveButton.gameObject.SetActive(true);
          break;
        case StatusType.Gold:
          SanitybuttonGroup.alpha = MoveButtonDisableAlpha;
          GoldbuttonGroup.alpha = 1.0f;
      
          MoveCostText.text = GameManager.Instance.MyGameData.MovePoint > 0 ?
      string.Format(GameManager.Instance.GetTextData("MOVECOST_ENOUGH"), "<#FFBF00>-1</color>", GameManager.Instance.GetTextData(StatusType.Gold, 2), WNCText.GetGoldColor("-" + GoldCost)) :
      string.Format(GameManager.Instance.GetTextData("MOVECOST_NOTENOUGH"), GameManager.Instance.GetTextData(StatusType.Gold, 2), WNCText.GetGoldColor("-" + GoldCost));

          if (MoveButton.gameObject.activeInHierarchy == false) MoveButton.gameObject.SetActive(true);
          break;
        default:                                    //���� �ܰ�� ���ư��°�
          ProgressGuideText.text = GameManager.Instance.GetTextData("CHOOSETILE_MAP");

          TilePreview_Bottom.sprite = GameManager.Instance.ImageHolder.UnknownTile;
          TilePreview_Bottom.transform.rotation =Quaternion.Euler(Vector3.zero);
          TilePreview_Top.sprite = GameManager.Instance.ImageHolder.Transparent;
          TilePreview_Top.sprite = GameManager.Instance.ImageHolder.Transparent;

          if (SettlementInfoHolder.activeInHierarchy == true)
          {
            SettlementInfoHolder.SetActive(false);
            SettlementNameText.text = "";
            DiscomfortText.text = "";
          }

          MovecostButtonGroup.alpha = 0.0f;
          MovecostButtonGroup.interactable = false;
          MovecostButtonGroup.blocksRaycasts = false;
          SanitybuttonGroup.alpha = MoveButtonDisableAlpha;
          GoldbuttonGroup.alpha = MoveButtonDisableAlpha;

          MoveCostText.text = "";

          MoveButton.gameObject.SetActive(false);

          SelectedTile = null;
          break;
      }
    }
  }
  [SerializeField] private TextMeshProUGUI MoveCostText = null;
  [SerializeField] private TextMeshProUGUI MoveButtonText = null;
  [SerializeField] private Button MoveButton = null;

  public Transform SelectTileHolder = null;

  public Color NormalColor = Color.white;
  public Color DisableColor = Color.grey;
  private List<Settlement> ActiveSettles = new List<Settlement>();
  private List<TileData> ActiveTileData = new List<TileData>();

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
        if (_tile.ButtonScript.TopImage != null) _tile.ButtonScript.TopImage.color= NormalColor;
      }//���� Ȱ��ȭ Ÿ�� ��Ͽ� ���� Ÿ���̶�� ����Ʈ�� �߰��ϰ� Ȱ��ȭ

      if (_tile.TileSettle != null)
      {
        _currentsettles.Add(_tile.TileSettle);
        if (!ActiveSettles.Contains(_tile.TileSettle))
        {
          ActiveSettles.Add(_tile.TileSettle);
      //    StartCoroutine(UIManager.Instance.ChangeAlpha(_tile.TileSettle.HolderObject.GetComponent<CanvasGroup>(), 1.0f, 0.8f,false));
        }
      }//���� Ȱ��ȭ ������ ��Ͽ� ���� Ÿ���̶�� ����Ʈ�� �߰��ϰ� Ȱ��ȭ
    }

    List<TileData> _removetiles = new List<TileData>();       //ActiveSettles�� �־����� �� _current�� ��� ����� �͵�
    List<Settlement> _removesettles = new List<Settlement>();

    foreach (TileData _tile in ActiveTileData)
    {
      if (!_currents.Contains(_tile))
      {
        _removetiles.Add(_tile);
         _tile.ButtonScript.Button.interactable = false;
        if (_tile.ButtonScript.TopImage != null) _tile.ButtonScript.TopImage.color = DisableColor;
      }//ȭ�鿡�� ����� Ÿ�ϵ��� ��Ȱ��ȭ ���·� ��ȯ
    }

    foreach (Settlement _settle in ActiveSettles)
    {
      if (!_currentsettles.Contains(_settle))
      {
        _removesettles.Add(_settle);
     //   StartCoroutine(UIManager.Instance.ChangeAlpha(GetSettleIcon(_settle).GetComponent<CanvasGroup>(), 0.0f, 0.3f, false));
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
    ResetEnableTiles();

    MoveInfoGroup.interactable = true;
    if (MoveButtonText.text == "")
    {
      MoveButtonText.text = GameManager.Instance.GetTextData("MOVE");
      LayoutRebuilder.ForceRebuildLayoutImmediate(MoveButton.transform as RectTransform);
    }
    SelectedCostType = StatusType.HP;

    if (DefaultGroup.alpha == 1.0f) DefaultGroup.alpha = 0.0f;
    if (DefaultGroup.interactable == true) DefaultGroup.interactable = false;
    if (DefaultGroup.blocksRaycasts == true) DefaultGroup.blocksRaycasts = false;

    if (GetPanelRect("myrect").Rect.anchoredPosition == GetPanelRect("myrect").OutisdePos)
      GetPanelRect("myrect").Rect.anchoredPosition = GetPanelRect("myrect").InsidePos;

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
  public void SelectTile(TileData selectedtiledata,Vector2 position)
  {
    //������ ��ǥ�� ȣ����� �ʰ� �̹� �Ÿ�

    if (SelectedTile != null) SelectedTile.ButtonScript.CancleTile();
    if(SelectedCostType!=StatusType.HP)SelectedCostType= StatusType.HP;
    TileData _currenttile = GameManager.Instance.MyGameData.MyMapData.Tile(GameManager.Instance.MyGameData.Coordinate);
    int _length = GameManager.Instance.MyGameData.MyMapData.GetLength(_currenttile, selectedtiledata);

    SelectedTile = selectedtiledata;

    ProgressGuideText.text = GameManager.Instance.GetTextData("CHOOSECOSTTYPE_MAP");

    TilePreview_Bottom.sprite = MapCreater.MyTiles.GetTile(SelectedTile.BottomEnvirSprite);
    TilePreview_Bottom.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -60.0f * SelectedTile.Rotation));
    TilePreview_Top.sprite = MapCreater.MyTiles.GetTile(SelectedTile.TopEnvirSprite);
    TilePreview_Landmark.sprite = MapCreater.MyTiles.GetTile(SelectedTile.landmarkSprite);

    if (SelectedTile.TileSettle != null)
    {
      SettlementInfoHolder.SetActive(true);
      SettlementNameText.text=SelectedTile.TileSettle.Name;
      DiscomfortText.text = SelectedTile.TileSettle.Discomfort.ToString();
    }
    else
    {
      if (SettlementInfoHolder.activeInHierarchy == true) SettlementInfoHolder.SetActive(false);
    }
    MovecostButtonGroup.alpha = 1.0f;
    MovecostButtonGroup.interactable = true;
    MovecostButtonGroup.blocksRaycasts = true;
    SanityCost = GameManager.Instance.MyGameData.GetMoveSanityCost(_length);
    GoldCost = GameManager.Instance.MyGameData.GetMoveGoldCost(_length);
    SanitybuttonGroup.interactable = true;
    if (GameManager.Instance.MyGameData.Gold >= GoldCost)
    {
      GoldbuttonGroup.interactable = true;
      GoldButtonPreview.PanelType = PreviewPanelType.MoveCostGold;
    }
    else
    {
      GoldbuttonGroup.interactable = false;
      GoldButtonPreview.PanelType = PreviewPanelType.MoveCostGoldNogold;
    }

  }

  public void SelectCostType_Sanity()
  {
    SelectedCostType = StatusType.Sanity;
  }
  public void SelectCostType_Gold()
  {
    SelectedCostType = StatusType.Gold;
  }

  public void MoveMap()
  {
    MoveInfoGroup.interactable = false;
    DefaultGroup.blocksRaycasts = true;
    UIManager.Instance.ResetEventPanels();
    UIManager.Instance.AddUIQueue(movemap());
  }//�̵� ��ư�� �������� �ش� ������, �ش� ȭ��ǥ�� ������ �ٸ� ������ ��ư ��Ȱ��ȭ �� �ٸ� ȭ��ǥ ��Ȱ��ȭ
  private IEnumerator movemap()
  {
    GameManager.Instance.MyGameData.Turn++;

    Vector2 _playerrectpos = PlayerRect.anchoredPosition;                 //���� ��ġ(Rect)
    Vector2 _targettilepos = SelectedTile.Rect.anchoredPosition;            //���� ��ġ(Rect)

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
    if (GameManager.Instance.MyGameData.MovePoint > 0) GameManager.Instance.MyGameData.MovePoint--;

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

    GameManager.Instance.MyGameData.Coordinate = _endcoor;

    switch (SelectedTile.Landmark)
    {
      case LandmarkType.Outer:
        GameManager.Instance.MyGameData.CurrentSettlement = null;
        yield return new WaitForSeconds(1.0f);
        EventManager.Instance.SetOutsideEvent(GameManager.Instance.MyGameData.MyMapData.GetTileData(SelectedTile.Coordinate));
        //ĳ���� ���� ��ġ ���� 1ĭ ��,��,���,��,�ٴ� ���� �ľ��ؼ� EventManager�� ������
        IsOpen = false;
        break;

      case LandmarkType.Village:
      case LandmarkType.Town:
      case LandmarkType.City:
        GameManager.Instance.EnterSettlement(SelectedTile.TileSettle);
        break;
    }

    StartCoroutine(zoominview());
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DefaultGroup, 0.0f, ZoomInTime + 0.6f, false));
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
  }
  public override void CloseUI()
  {
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
    DefaultGroup.blocksRaycasts = false;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("myrect").Rect, GetPanelRect("myrect").Rect.anchoredPosition, GetPanelRect("myrect").OutisdePos, 0.3f, UIManager.Instance.UIPanelCLoseCurve));
  }

}
