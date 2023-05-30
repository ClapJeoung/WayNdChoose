using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics.Tracing;
using Google.Apis.Json;

public class UI_EventSuggest : UI_default
{

    [SerializeField] private CanvasGroup SettleNameGroup = null;
    [SerializeField] private TextMeshProUGUI SettleName = null;
  [SerializeField] private CanvasGroup EnvirGroup = null;
  [SerializeField] private GameObject[] EnvirObjs=new GameObject[0];
    [SerializeField] private CanvasGroup UnPleasantGroup = null;
  [SerializeField] private TextMeshProUGUI CurrentUnPlesant = null;
  public PlaceButton Place_residence = null;
  public PlaceButton Place_marketplace = null;
  public PlaceButton Place_temple = null;
  public PlaceButton Place_library = null;
  public PlaceButton Place_theater = null;
  public PlaceButton Place_academy = null;
  private PlaceType CurrentPlace = PlaceType.NULL;
    [SerializeField] private CanvasGroup PlacePanelGroup = null;
    [SerializeField] private TextMeshProUGUI PlaceDescription = null;
    [SerializeField] private CanvasGroup StartButtonGroup = null;
    [SerializeField] private Button StartButton = null;
  private void Start()
  {
    Place_residence.MyText.name = GameManager.Instance.GetTextData(Place_residence.MyPlaceType).Name;
    Place_marketplace.MyText.name = GameManager.Instance.GetTextData(Place_marketplace.MyPlaceType).Name;
    Place_temple.MyText.name = GameManager.Instance.GetTextData(Place_temple.MyPlaceType).Name;
    Place_library.MyText.name = GameManager.Instance.GetTextData(Place_library.MyPlaceType).Name;
    Place_theater.MyText.name = GameManager.Instance.GetTextData(Place_theater.MyPlaceType).Name;
    Place_academy.MyText.name = GameManager.Instance.GetTextData(Place_academy.MyPlaceType).Name;
  }
  public void OpenSuggest()
  {

        UIManager.Instance.AddUIQueue(opensuggest());
    }
    private IEnumerator opensuggest()
    {
        WaitForSeconds _wait = new WaitForSeconds(0.1f);

        SettleName.text = GameManager.Instance.GetTextData(GameManager.Instance.MyGameData.CurrentSettlement.OriginName).Name;
        StartCoroutine(UIManager.Instance.ChangeAlpha(SettleNameGroup, 1.0f, false, UIFadeMoveDir.Right, false));
        yield return _wait;
    var _envirs = GameManager.Instance.MyGameData.CurrentSettlement.TileData.EnvironmentType;
    for(int i = 0; i < EnvirObjs.Length; i++)
    {
      if (_envirs.Contains((EnvironmentType)i))
       { if (EnvirObjs[i].activeInHierarchy.Equals(false)) EnvirObjs[i].SetActive(true); }
      else
      { if (EnvirObjs[i].activeInHierarchy.Equals(true)) EnvirObjs[i].SetActive(false); }

    }
    StartCoroutine(UIManager.Instance.ChangeAlpha( EnvirGroup, 1.0f, false, UIFadeMoveDir.Right, false));
    yield return _wait;
    string _currentdiscomfort = string.Format(GameManager.Instance.GetTextData("currentdiscomfort").Name, GameManager.Instance.MyGameData.AllSettleUnpleasant[GameManager.Instance.MyGameData.CurrentSettlement]);
        //이 정착지의 불쾌 지수는 #UNP#
        string _currentvalue = GameManager.Instance.GetTextData("currentsettlesanityloss").Name;
    _currentvalue = string.Format(_currentvalue, GameManager.Instance.MyGameData.SettleSanityLoss);
        //장소 진입 시 #VALUE# #sanity# 감소
        CurrentUnPlesant.text = _currentdiscomfort + "\n\n" + _currentvalue;
    StartCoroutine(UIManager.Instance.ChangeAlpha(MyGroup, 1.0f, true, false));

        StartCoroutine(UIManager.Instance.ChangeAlpha(UnPleasantGroup, 1.0f, false, UIFadeMoveDir.Right, false));
        yield return _wait;

        StartCoroutine(UIManager.Instance.ChangeAlpha(PlacePanelGroup, 1.0f, false, UIFadeMoveDir.Left, false));
        yield return _wait;
    StartCoroutine(UIManager.Instance.ChangeAlpha(StartButtonGroup, 1.0f, false, UIFadeMoveDir.Down, false));
    StartButton.interactable = false;
    switch (GameManager.Instance.MyGameData.CurrentSettlement.Type)
    {
      case SettlementType.Town:
        if (Place_residence.gameObject.activeInHierarchy.Equals(false)) Place_residence.gameObject.SetActive(true);
        if (Place_marketplace.gameObject.activeInHierarchy.Equals(false)) Place_marketplace.gameObject.SetActive(true);
        if (Place_temple.gameObject.activeInHierarchy.Equals(false)) Place_temple.gameObject.SetActive(true);
        if (Place_library.gameObject.activeInHierarchy.Equals(true)) Place_library.gameObject.SetActive(false);
        if (Place_theater.gameObject.activeInHierarchy.Equals(true)) Place_theater.gameObject.SetActive(false);
        if (Place_academy.gameObject.activeInHierarchy.Equals(true)) Place_academy.gameObject.SetActive(false);
        Place_residence.Setup();
        Place_marketplace.Setup();
        Place_temple.Setup();
        break;
      case SettlementType.City:
        if (Place_residence.gameObject.activeInHierarchy.Equals(false)) Place_residence.gameObject.SetActive(true);
        if (Place_marketplace.gameObject.activeInHierarchy.Equals(false)) Place_marketplace.gameObject.SetActive(true);
        if (Place_temple.gameObject.activeInHierarchy.Equals(false)) Place_temple.gameObject.SetActive(true);
        if (Place_library.gameObject.activeInHierarchy.Equals(false)) Place_library.gameObject.SetActive(true);
        if (Place_theater.gameObject.activeInHierarchy.Equals(true)) Place_theater.gameObject.SetActive(false);
        if (Place_academy.gameObject.activeInHierarchy.Equals(true)) Place_academy.gameObject.SetActive(false);
        Place_residence.Setup();
        Place_marketplace.Setup();
        Place_temple.Setup();
        Place_library.Setup();
        break;
      case SettlementType.Castle:
        if (Place_residence.gameObject.activeInHierarchy.Equals(false)) Place_residence.gameObject.SetActive(true);
        if (Place_marketplace.gameObject.activeInHierarchy.Equals(false)) Place_marketplace.gameObject.SetActive(true);
        if (Place_temple.gameObject.activeInHierarchy.Equals(false)) Place_temple.gameObject.SetActive(true);
        if (Place_library.gameObject.activeInHierarchy.Equals(true)) Place_library.gameObject.SetActive(false);
        if (Place_theater.gameObject.activeInHierarchy.Equals(false)) Place_theater.gameObject.SetActive(true);
        if (Place_academy.gameObject.activeInHierarchy.Equals(false)) Place_academy.gameObject.SetActive(true);
        Place_residence.Setup();
        Place_marketplace.Setup();
        Place_temple.Setup();
        Place_theater.Setup();
        Place_academy.Setup();
        break;
    }

        MyGroup.interactable = true;
        MyGroup.blocksRaycasts = true;
    }
    public void CloseSuggestPanel_normal()
    {
        UIManager.Instance.AddUIQueue(closesuggestpanel());
    }
    private IEnumerator closesuggestpanel()
    {
        MyGroup.interactable = false;
        MyGroup.blocksRaycasts = false;

        WaitForSeconds _wait = new WaitForSeconds(0.1f);
        StartCoroutine(UIManager.Instance.ChangeAlpha(SettleNameGroup, 0.0f, false, UIFadeMoveDir.Left, false));
        yield return _wait;
    StartCoroutine(UIManager.Instance.ChangeAlpha(EnvirGroup, 0.0f, false, UIFadeMoveDir.Left, false));
    yield return _wait;
        StartCoroutine(UIManager.Instance.ChangeAlpha(UnPleasantGroup, 0.0f, false, UIFadeMoveDir.Left, false));
        yield return _wait;

        StartCoroutine(UIManager.Instance.ChangeAlpha(StartButtonGroup, 0.0f, false, UIFadeMoveDir.Down, false));
        yield return _wait;


    
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(PlacePanelGroup, 0.0f, false, UIFadeMoveDir.Right, false));

        SettleName.text = "";
        CurrentUnPlesant.text = "";
        CurrentPlace = PlaceType.NULL;
        PlaceDescription.text = "";

  }
    public void CloseSuggestPanel_quick()
    {
    if (GameManager.Instance.MyGameData.CurrentSettlement == null) return;
        MyGroup.interactable = false;
        MyGroup.blocksRaycasts = false;

        SettleNameGroup.alpha = 0.0f;
        SettleName.text = "";
    EnvirGroup.alpha = 0.0f;
        UnPleasantGroup.alpha = 0.0f;
        CurrentUnPlesant.text = "";
        if (Place_residence.gameObject.activeInHierarchy.Equals(true)) Place_residence.gameObject.SetActive(false);
        if (Place_marketplace.gameObject.activeInHierarchy.Equals(true)) Place_marketplace.gameObject.SetActive(false);
    if (Place_temple.gameObject.activeInHierarchy.Equals(true)) Place_temple.gameObject.SetActive(false);
    if (Place_theater.gameObject.activeInHierarchy.Equals(true)) Place_theater.gameObject.SetActive(false);
    if (Place_theater.gameObject.activeInHierarchy.Equals(true)) Place_theater.gameObject.SetActive(false);
    CurrentPlace = PlaceType.NULL;
        PlacePanelGroup.alpha = 0.0f;
        PlaceDescription.text = "";
    }//빠른종료(그냥 초기화만
  public void SelectPlace(PlaceType _targetplace)
  {
  //  Debug.Log(_targetplace);
    if (CurrentPlace.Equals(_targetplace)) return;
  //  Debug.Log("정보 전개");
    CurrentPlace = _targetplace;
    string _description = GameManager.Instance.GetTextData(_targetplace).Description;
    string _effect = "";
    switch (_targetplace)
    {
      case PlaceType.Residence:
        _effect =string.Format( GameManager.Instance.GetPlaceEffectTextData(_targetplace).Name,"3", ((int)(ConstValues.PlaceEffect_residence*100.0f)).ToString());
        break;
      case PlaceType.Marketplace:
        _effect=string.Format(GameManager.Instance.GetPlaceEffectTextData(_targetplace).Name,ConstValues.PlaceEffect_marketplace.ToString());
        break;
      case PlaceType.Temple:
        _effect = GameManager.Instance.GetPlaceEffectTextData(_targetplace).Name;
        break;
      case PlaceType.Library:
        ThemeType _theme = GameManager.Instance.MyGameData.CurrentSettlement.LibraryType;
        string _themeicon = GameManager.Instance.GetTextData(_theme).Icon;
        _effect=string.Format(GameManager.Instance.GetPlaceEffectTextData(_targetplace).Name,"3", _themeicon,GameManager.Instance.GetTextData(_theme).Name);
        break;
      case PlaceType.Theater:
        _effect = GameManager.Instance.GetPlaceEffectTextData(_targetplace).Name;
        break;
      case PlaceType.Academy:
        _effect=string.Format(GameManager.Instance.GetPlaceEffectTextData(_targetplace).Name,"3",ConstValues.PlaceEffect_acardemy.ToString());
        break;
    }


    PlaceDescription.text = _description+"\n\n"+_effect;
    StartButton.interactable = true;
  }
  public void StartEvent()
    {
    Debug.Log("이벤트 시작인레후~~");
        CloseSuggestPanel_normal();
        EventManager.Instance.SetSettleEvent(CurrentPlace);
    }//시작 버튼 눌렀을 때 호출
}
