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
    //정착지 주위 환경 설명 요소
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
    [SerializeField] private Image IllustImage = null;
    [SerializeField] private TextMeshProUGUI PlaceDescription = null;
    [SerializeField] private TextMeshProUGUI PlaceEffect = null;
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
        StartCoroutine(UIManager.Instance.ChangeAlpha(SettleNameGroup, 1.0f, false, UIFadeMoveDir.Right));
        yield return _wait;
    //환경 미리보기 설정 해야됨
    string _currentuncomfortable = string.Format(GameManager.Instance.GetTextData("currentuncomfortable").Name, GameManager.Instance.MyGameData.AllSettleUnpleasant[GameManager.Instance.MyGameData.CurrentSettlement]);
        //이 정착지의 불쾌 지수는 #UNP#
        string _currentvalue = GameManager.Instance.GetTextData("currentsettlesanityloss").Name;
    _currentvalue = string.Format(_currentvalue, GameManager.Instance.MyGameData.SettleSanityLoss);
        //장소 진입 시 #VALUE# #sanity# 감소
        CurrentUnPlesant.text = _currentuncomfortable + "\n" + _currentvalue;
    StartCoroutine(UIManager.Instance.ChangeAlpha(MyGroup, 1.0f, true));

        StartCoroutine(UIManager.Instance.ChangeAlpha(UnPleasantGroup, 1.0f, false, UIFadeMoveDir.Right));
        yield return _wait;

        StartCoroutine(UIManager.Instance.ChangeAlpha(PlacePanelGroup, 1.0f, false, UIFadeMoveDir.Left));
        yield return _wait;

    switch (GameManager.Instance.MyGameData.CurrentSettlement.Type)
    {
      case SettlementType.Town:
        Place_residence.Setup();
        Place_marketplace.Setup();
        Place_temple.Setup();
        break;
      case SettlementType.City:
        Place_residence.Setup();
        Place_marketplace.Setup();
        Place_temple.Setup();
        Place_library.Setup();
        break;
      case SettlementType.Castle:
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
        StartCoroutine(UIManager.Instance.ChangeAlpha(SettleNameGroup, 0.0f, false, UIFadeMoveDir.Left));
        yield return _wait;
        //환경 탭 제거
        StartCoroutine(UIManager.Instance.ChangeAlpha(UnPleasantGroup, 0.0f, false, UIFadeMoveDir.Left));
        yield return _wait;

        StartCoroutine(UIManager.Instance.ChangeAlpha(StartButtonGroup, 0.0f, false, UIFadeMoveDir.Down));
        yield return _wait;


    
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(PlacePanelGroup, 0.0f, false, UIFadeMoveDir.Right));

        SettleName.text = "";
        CurrentUnPlesant.text = "";
        CurrentPlace = PlaceType.NULL;
        IllustImage.sprite = GameManager.Instance.ImageHolder.NoneIllust;
        PlaceDescription.text = "";
        PlaceEffect.text = "";

  }
    public void CloseSuggestPanel_quick()
    {
        MyGroup.interactable = false;
        MyGroup.blocksRaycasts = false;

        SettleNameGroup.alpha = 0.0f;
        SettleName.text = "";
        //환경 초기화
        UnPleasantGroup.alpha = 0.0f;
        CurrentUnPlesant.text = "";
    switch (GameManager.Instance.MyGameData.CurrentSettlement.Type)
    {
      case SettlementType.Town:
        Place_residence.Close();
        Place_marketplace.Close();
        Place_temple.Close();
        break;
      case SettlementType.City:
        Place_residence.Close();
        Place_marketplace.Close();
        Place_temple.Close();
        Place_library.Close();
        break;
      case SettlementType.Castle:
        Place_residence.Close();
        Place_marketplace.Close();
        Place_temple.Close();
        Place_theater.Close();
        Place_academy.Close();
        break;
    }
    CurrentPlace = PlaceType.NULL;
        PlacePanelGroup.alpha = 0.0f;
        IllustImage.sprite = GameManager.Instance.ImageHolder.NoneIllust;
        PlaceDescription.text = "";
        PlaceEffect.text = "";
    }//빠른종료(그냥 초기화만
  public void SelectPlace(PlaceType _targetplace)
  {
    if (CurrentPlace.Equals(_targetplace)) return;

    CurrentPlace = _targetplace;
    string _description = GameManager.Instance.GetTextData(_targetplace).Description;
    string _effect = "";
    switch (_targetplace)
    {
      case PlaceType.Residence:
        _effect =string.Format( GameManager.Instance.GetTextData("residenceeffect").Name, ConstValues.PlaceEffect_residence.ToString());
        break;
      case PlaceType.Marketplace:
        _effect=string.Format(GameManager.Instance.GetTextData("marketeffect").Name,ConstValues.PlaceEffect_marketplace.ToString());
        break;
      case PlaceType.Temple:
        _effect = GameManager.Instance.GetTextData("templeeffect").Name;
        break;
      case PlaceType.Library:
        ThemeType _theme = GameManager.Instance.MyGameData.CurrentSettlement.LibraryType;
        string _themeicon = GameManager.Instance.GetTextData(_theme).Icon;
        _effect=string.Format(GameManager.Instance.GetTextData("libraryeffect").Name, _themeicon,GameManager.Instance.GetTextData(_theme).Name);
        break;
      case PlaceType.Theater:
        _effect = GameManager.Instance.GetTextData("theatereffect").Name;
        break;
      case PlaceType.Academy:
        _effect=string.Format(GameManager.Instance.GetTextData("academyeffect").Name,ConstValues.PlaceEffect_acardemy.ToString());
        break;
    }


    PlaceDescription.text = _description;
    PlaceEffect.text = _effect;
    StartButton.interactable = true;
  }
  public void StartEvent()
    {
        CloseSuggestPanel_normal();
        EventManager.Instance.SetSettleEvent(CurrentPlace);
    }//시작 버튼 눌렀을 때 호출
}
