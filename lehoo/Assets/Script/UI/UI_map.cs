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
    Debug.Log("뭔가 이상한 레후~");
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
  public int SelectProgress = 0;//0: 타일 선택(목표 선택)  1: 지불 방식 선택  2:타일 선택(확정,출발)

  /// <summary>
  /// 주위 2칸 타일 업데이트
  /// </summary>
  private void ResetEnableTiles()
  {
    List<TileData> _currents = GameManager.Instance.MyGameData.MyMapData.GetAroundTile(GameManager.Instance.MyGameData.Coordinate, 2);
    List<Settlement> _currentsettles = new List<Settlement>();
    foreach (TileData _tile in _currents) //주위 2칸 타일 전부 가져오기
    {
      if (!ActiveTileData.Contains(_tile))
      {
        ActiveTileData.Add(_tile);
        _tile.ButtonScript.Button.interactable = true;
        if (_tile.ButtonScript.TopEnvirImage != null) _tile.ButtonScript.TopEnvirImage.color= NormalColor;
      }//기존 활성화 타일 목록에 없던 타일이라면 리스트에 추가하고 활성화

      if (_tile.TileSettle != null)
      {
        _currentsettles.Add(_tile.TileSettle);
        if (!ActiveSettles.Contains(_tile.TileSettle))
        {
          ActiveSettles.Add(_tile.TileSettle);
          StartCoroutine(UIManager.Instance.ChangeAlpha(GetSettleIcon(_tile.TileSettle).GetComponent<CanvasGroup>(), 1.0f, 0.8f,false));
        }
      }//기존 활성화 정착지 목록에 없던 타일이라면 리스트에 추가하고 활성화
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
      }//화면에서 사라진 타일들은 비활성화 상태로 전환
    }

    foreach (Settlement _settle in ActiveSettles)
    {
      if (!_currentsettles.Contains(_settle))
      {
        _removesettles.Add(_settle);
        StartCoroutine(UIManager.Instance.ChangeAlpha(GetSettleIcon(_settle).GetComponent<CanvasGroup>(), 0.0f, 0.3f, false));
      }//화면에서 사라진 정착지들은 투명화
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
      case 0: //0단계는 타일을 하나도 선택하지 않은 상태이므로 지불 방식 선택으로 넘어가기
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
        }//동일한 타일 선택했으면 이전 단계로 회귀(타일 선택)  0<-1
        else //다른 타일 선택했으면 정보만 변경 (1단계)
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
        }//2단계에서 목표 타일 클릭하면 이동
        else//2단계(정보표시,타일클릭하셈)에서 다른 타일 클릭하면 1단계로 회귀(지불 버튼 표시) 1<-2
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
  }//이동 버튼을 눌렀으니 해당 정착지, 해당 화살표를 제외한 다른 정착지 버튼 비활성화 및 다른 화살표 비활성화
  private IEnumerator movemap()
  {

    Vector2 _playerrectpos = PlayerRect.anchoredPosition;                 //현재 위치(Rect)
    Vector2 _targettilepos = SelectedTile.Rect.anchoredPosition;            //종점 위치(Rect)

    Vector2 _startcoor = GameManager.Instance.MyGameData.Coordinate;//현재 위치(타일)
    Vector2 _endcoor = SelectedTile.Coordinate;//종점 위치(타일)

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
        //캐릭터 멈춘 위치 주위 1칸 강,숲,언덕,산,바다 유무 파악해서 EventManager에 던져줌
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
    Debug.Log("이동 코루틴이 끝난 레후~");
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
  }//정착지,야외 이동 후 지도 줌인 하는 코루틴
  public void SetPlayerPos(Vector2 coordinate)
  {
    Debug.Log("시작 플레이어 위치 설정");
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
