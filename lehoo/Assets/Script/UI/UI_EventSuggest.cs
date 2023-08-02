using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics.Tracing;
using Google.Apis.Json;

public class UI_EventSuggest : UI_default
{
  private float UIOpenMoveTime = 0.6f;
  private float UIOpenFadeTime = 0.4f;
  private WaitForSeconds LittleWait = new WaitForSeconds(0.2f);
  private float UICloseMoveTime = 0.5f;
  private float UICloseFadeTime = 0.3f;

  [SerializeField] private CanvasGroup SettleNameGroup = null;
  [SerializeField] private RectTransform SettleNameRect = null;
  [SerializeField] private TextMeshProUGUI SettleName = null;
  private Vector2 SettleNameClosePos = new Vector2(0.0f, 600.0f);
  private Vector2 SettleNameOpenPos = new Vector2(0.0f, 330.0f);

  [SerializeField] private HorizontalLayoutGroup EnvirLayout = null;
  [SerializeField] private CanvasGroup EnvirGroup = null;
  [SerializeField] private GameObject[] EnvirObjs = new GameObject[0];
  [SerializeField] private RectTransform[] EnvirImageRects=new RectTransform[0];
  [SerializeField] private CanvasGroup[] EnvirImageGroups=new CanvasGroup[0];

  [SerializeField] private CanvasGroup DiscomfortGroup = null;
  [SerializeField] private RectTransform DiscomfortRect = null;
  [SerializeField] private TextMeshProUGUI DiscomfortCount = null;
  private Vector2 DiscomfortClosePos = new Vector2(0.0f, 330.0f);
  private Vector2 discomfortOpenPos = new Vector2(350.0f, 330.0f);
  [SerializeField] private CanvasGroup MapButtonGroup = null;

  [SerializeField] private CanvasGroup ReRollButtonGroup = null;
  [SerializeField] private CanvasGroup PlaceGroup = null;
  [SerializeField] PlaceButton[] PlaceButtons = null;
  [SerializeField] RectTransform[] PlaceImageRects = null;
  [SerializeField] CanvasGroup[] PlaceButtonGroups = null;
  private PlaceType CurrentPlace = PlaceType.NULL;
  [SerializeField] private RectTransform PlaceDescription = null;
  [SerializeField] private CanvasGroup PlaceDescriptionGroup = null;
  [SerializeField] private TextMeshProUGUI PlaceDescription_size = null;
  [SerializeField] private TextMeshProUGUI PlaceDescription_text = null;

  [SerializeField] private CanvasGroup StartButtonGroup = null;
  [SerializeField] private Button StartButton = null;
  [SerializeField] private TextMeshProUGUI StartButtonText = null;
  [SerializeField] private ParticleSystem DiscomfortParticle = null;
  private void Start()
  {
    for (int i = 0; i < PlaceButtons.Length; i++)
      PlaceButtons[i].MyText.name = GameManager.Instance.GetTextData(PlaceButtons[i].MyPlaceType,0);
    StartButtonText.text = GameManager.Instance.GetTextData("TOTHEWORLD");
  }
  public void OpenSuggest()
  {
    UIManager.Instance.AddUIQueue(opensuggest());
  }
  private IEnumerator opensuggest()
  {
    Settlement _currentsettle = GameManager.Instance.MyGameData.CurrentSettlement;
    if (_currentsettle.TileInfoData.EnvirList.Count.Equals(0)) UIManager.Instance.UpdateBackground(EnvironmentType.NULL);
    else UIManager.Instance.UpdateBackground(_currentsettle.TileInfoData.EnvirList[Random.Range(0, _currentsettle.TileInfoData.EnvirList.Count)]);

    SettleNameGroup.alpha = 1.0f;
    SettleName.text = GameManager.Instance.GetTextData(_currentsettle.OriginName);
    yield return StartCoroutine(UIManager.Instance.moverect(SettleNameRect, SettleNameClosePos, SettleNameOpenPos, UIOpenMoveTime, UIManager.Instance.UIPanelOpenCurve));
    yield return LittleWait;
    //�̸� �����ϰ� ��Ÿ��

    yield return StartCoroutine(openenvir());
       yield return LittleWait;
 //ȯ�� ���� ���� ��Ÿ��

    DiscomfortGroup.alpha = 0.0f;
    DiscomfortCount.text = _currentsettle.Discomfort.ToString();
    StartCoroutine(UIManager.Instance.moverect(DiscomfortRect, DiscomfortClosePos, discomfortOpenPos, UIOpenMoveTime, UIManager.Instance.UIPanelOpenCurve));
    StartCoroutine(UIManager.Instance.ChangeAlpha(DiscomfortGroup, 1.0f, UIOpenFadeTime, false));
    yield return LittleWait;
    //���� ���� �����ϰ� ��Ÿ��

    string _currentdiscomfort = string.Format(GameManager.Instance.GetTextData("CURRENTDISCOMFORT"), GameManager.Instance.MyGameData.CurrentSettlement.Discomfort);
    string _currentvalue = string.Format(GameManager.Instance.GetTextData("PLACESANITYCOST"), GameManager.Instance.MyGameData.SettleSanityLoss);
    //��� ���� �� #VALUE# #sanity# ����
    //�� �������� ���� ������ #UNP#

    yield return StartCoroutine(setplacebuttons());
    StartCoroutine(UIManager.Instance.ChangeAlpha(ReRollButtonGroup, 1.0f, UIOpenFadeTime, false));

    StartCoroutine(UIManager.Instance.ChangeAlpha(MapButtonGroup, 1.0f, UIOpenFadeTime, false));
    yield return LittleWait;
    //���� ��ư�� ���̵�� ��Ÿ��

    //��� �г� ��ư�� ��Ÿ���� ����

    StartButton.interactable = false;

    //��� ������ ��� ���� ��Ÿ��

    MyGroup.interactable = true;
    MyGroup.blocksRaycasts = true;
  }
  private IEnumerator setplacebuttons()
  {
    List<PlaceType> _enableplaces = GameManager.Instance.MyGameData.CurrentSettlement.EnablePlaces;
    PlaceType _place;
    for(int i = 0; i < 6; i++)
    {
      _place= (PlaceType)i;
      if (_enableplaces.Contains(_place))
      {
        if (PlaceButtons[i].gameObject.activeInHierarchy.Equals(false))
        {
          PlaceButtonGroups[i].alpha = 0.0f;
          PlaceButtons[i].gameObject.SetActive(true);
        }
      }
      else
      {
        if(PlaceButtons[i].gameObject.activeInHierarchy.Equals(true))PlaceButtons[i].gameObject.SetActive(false);
      }
    }//ó���� SetActive�θ� �ٷ��ְ�

    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(PlaceGroup,1.0f, UIOpenFadeTime, false));

    for (int i = 0; i < 6; i++)
    {
      _place = (PlaceType)i;
      if (_enableplaces.Contains(_place))
      {
        StartCoroutine(UIManager.Instance.ChangeAlpha(PlaceButtonGroups[i], 1.0f, UIOpenFadeTime, false));
      }
      yield return LittleWait;
    }//��ġ ���������� ���� ����



  }//��� �����ϴ°�
  private IEnumerator openenvir()
  {
    if (EnvirLayout.enabled.Equals(false)) EnvirLayout.enabled = true;
    if(EnvirGroup.alpha.Equals(1.0f)) EnvirGroup.alpha = 0.0f;

    List<int> _activeenvir=new List<int>();
    var _envirs = GameManager.Instance.MyGameData.CurrentSettlement.TileInfoData.EnvirList;
    for (int i = 0; i < EnvirObjs.Length; i++)
    {
      if (_envirs.Contains((EnvironmentType)i))
      {
        if (EnvirObjs[i].activeInHierarchy.Equals(false))
        {
          EnvirImageGroups[i].alpha = 0.0f;
          EnvirObjs[i].SetActive(true);
          _activeenvir.Add(i);
        }
      }
      else
      { if (EnvirObjs[i].activeInHierarchy.Equals(true)) EnvirObjs[i].SetActive(false); }
    }//ȯ�� ������Ʈ SetActive
    yield return UIManager.Instance.ChangeAlpha(EnvirGroup, 1.0f, UIOpenFadeTime, false);
  }
  private IEnumerator closeenvir()
  {
    yield return UIManager.Instance.ChangeAlpha(EnvirGroup, 0.0f, UICloseFadeTime, false);
  }
  public void RerollPlaces()
  {
    GameManager.Instance.MyGameData.CurrentSettlement.SetAvailablePlaces();
    CurrentPlace = PlaceType.NULL;
    StartCoroutine(rerollplaces());
  }
  private IEnumerator rerollplaces()
  {
    StartButton.interactable = false;
    StartCoroutine(UIManager.Instance.ChangeAlpha(PlaceDescriptionGroup,0.0f,UICloseFadeTime,false));
    yield return StartCoroutine(destroyplacebuttons());
    DiscomfortParticle.Play();
    yield return new WaitForSeconds(1.0f);
    GameManager.Instance.MyGameData.Turn++;
  }
  private IEnumerator destroyplacebuttons()
  {
    List<int> _activeindex=new List<int>();
    for (int i = 0; i < PlaceButtons.Length; i++)
    {
      if (PlaceButtons[i].gameObject.activeInHierarchy)_activeindex.Add(i);
    }
    for (int i = 0; i < _activeindex.Count; i++)
    {
      StartCoroutine(UIManager.Instance.ChangeAlpha(PlaceButtonGroups[i],0.0f,UICloseFadeTime,false));
      if (i != 0) yield return LittleWait;
    }
    StartCoroutine(UIManager.Instance.ChangeAlpha(PlaceGroup,0.0f,UICloseFadeTime,false));
  }
  public void CloseSuggestPanel_normal()
  {
    UIManager.Instance.AddUIQueue(closesuggestpanel());
  }//��� ������ ������->�̺�Ʈ�� �Ѿ �� ���������� �г� �����ϴ� ����
  private IEnumerator closesuggestpanel()
  {
    MyGroup.interactable = false;
    MyGroup.blocksRaycasts = false;

   if(DiscomfortParticle!=null) DiscomfortParticle.Play();
    yield return LittleWait;

    yield return StartCoroutine(closeenvir());
    yield return StartCoroutine(UIManager.Instance.moverect(DiscomfortRect, DiscomfortClosePos, SettleNameOpenPos, UICloseMoveTime, UIManager.Instance.UIPanelOpenCurve));
    DiscomfortGroup.alpha = 0.0f;
    EnvirGroup.alpha = 0.0f;

    StartCoroutine(UIManager.Instance.moverect(SettleNameRect,SettleNameOpenPos,SettleNameClosePos, UICloseMoveTime, UIManager.Instance.UIPanelCLoseCurve));
    yield return LittleWait;

    yield return StartCoroutine(destroyplacebuttons());

    StartCoroutine(UIManager.Instance.ChangeAlpha(PlaceDescriptionGroup, 0.0f, UICloseFadeTime, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(StartButtonGroup, 0.0f, UICloseFadeTime, false));
    yield return LittleWait;

    StartCoroutine(UIManager.Instance.ChangeAlpha(MapButtonGroup, 0.0f, UICloseFadeTime, false));
    yield return LittleWait;
    StartCoroutine(UIManager.Instance.ChangeAlpha(ReRollButtonGroup, 0.0f, UICloseFadeTime, false));

    yield return LittleWait;
    yield return LittleWait;

    for (int i = 0; i < PlaceButtons.Length; i++)
      if (GameManager.Instance.MyGameData.CurrentSettlement.EnablePlaces.Contains(PlaceButtons[i].MyPlaceType) && CurrentPlace .Equals(PlaceButtons[i].MyPlaceType))
        yield return StartCoroutine(UIManager.Instance.ChangeAlpha(PlaceButtonGroups[i], 0.0f, UICloseFadeTime, false));
    CurrentPlace = PlaceType.NULL;
  }
  public void CloseSuggestPanel_quick()
    {
    for (int i = 0; i < PlaceButtons.Length; i++)
      if (PlaceButtons[i].gameObject.activeInHierarchy.Equals(true)) PlaceButtons[i].gameObject.SetActive(false);
     StartCoroutine(UIManager.Instance.ChangeAlpha(EnvirGroup, 0.0f, UICloseFadeTime, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(SettleNameGroup, 0.0f, UICloseFadeTime, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(DiscomfortGroup, 0.0f, UICloseFadeTime, false));
   if(PlaceDescriptionGroup.alpha.Equals(1.0f)) StartCoroutine(UIManager.Instance.ChangeAlpha(PlaceDescriptionGroup, 0.0f, UICloseFadeTime, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(StartButtonGroup, 0.0f, UICloseFadeTime, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(MapButtonGroup, 0.0f, UICloseFadeTime, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(ReRollButtonGroup, 0.0f, UICloseFadeTime, false));

    CurrentPlace = PlaceType.NULL;
    }//��������(�׳� �ʱ�ȭ��
  public void SelectPlace(PlaceType _targetplace)
  {
  //  Debug.Log(_targetplace);
    if (CurrentPlace.Equals(_targetplace)) return;
  //  Debug.Log("���� ����");
    CurrentPlace = _targetplace;
    string _description = GameManager.Instance.GetTextData(_targetplace,1);
    string _effect = "";
    switch (_targetplace)
    {
      case PlaceType.Residence:
        _effect =string.Format( GameManager.Instance.GetTextData(_targetplace,0),"3", ((int)(ConstValues.PlaceEffect_residence*100.0f)).ToString());
        break;
      case PlaceType.Marketplace:
        _effect=string.Format(GameManager.Instance.GetTextData(_targetplace,0),ConstValues.PlaceEffect_marketplace.ToString());
        break;
      case PlaceType.Temple:
        _effect = GameManager.Instance.GetTextData(_targetplace,0);
        break;
      case PlaceType.Library:
        SkillType _skill = GameManager.Instance.MyGameData.CurrentSettlement.LibraryType;
        string _themeicon = GameManager.Instance.GetTextData(_skill,2);
        _effect=string.Format(GameManager.Instance.GetTextData(_targetplace,0),"3", _themeicon,GameManager.Instance.GetTextData(_skill,0));
        break;
      case PlaceType.Theater:
        _effect = GameManager.Instance.GetTextData(_targetplace,0);
        break;
      case PlaceType.Academy:
        _effect=string.Format(GameManager.Instance.GetTextData(_targetplace,0),"3",ConstValues.PlaceEffect_acardemy.ToString());
        break;
    }

    PlaceDescription_size.text = _description+"\n\n"+_effect;
    PlaceDescription_text.text = _description + "\n\n" + _effect;
    if (PlaceDescriptionGroup.alpha.Equals(0.0f))
    {
      StartCoroutine(UIManager.Instance.ChangeAlpha(PlaceDescriptionGroup, 1.0f, 0.3f, false));
      StartCoroutine(UIManager.Instance.ChangeAlpha(StartButtonGroup,1.0f,0.3f,false));
    }
    StartButton.interactable = true;
  }
  public void StartEvent()
    {
    Debug.Log("�̺�Ʈ �����η���~~");
        CloseSuggestPanel_normal();
        EventManager.Instance.SetSettleEvent(CurrentPlace);
    }//���� ��ư ������ �� ȣ��
}
