using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_dialogue : UI_default
{
  private float DialogueUIMoveTime = 0.9f;
  private WaitForSeconds Wait = new WaitForSeconds(0.9f);
  public float TextFadeTime = 0.7f;
  private WaitForSeconds TextWait = new WaitForSeconds(0.8f);
  private WaitForSeconds ResultWait = new WaitForSeconds(1.0f);
  private float UIFadeTime = 0.6f;
  [Space(10)]

  [SerializeField] private CanvasGroup NameTextGroup = null;
  [SerializeField] private TextMeshProUGUI NameText = null;
  [SerializeField] private CanvasGroup IllustGroup = null;
  [SerializeField] private CanvasGroup IllustImageGroup = null;
  [SerializeField] private Image Illust = null;
  [SerializeField] private RectTransform IllustRect = null;
  private Vector2 IllustClosePos = new Vector2(-1125.0f, 0.0f);
  private Vector2 IllustOpenPos = new Vector2(-385.0f, 0.0f);
  private float IllustFullChargingTime = 3.0f;
  private float IllustChargingWaitTime = 1.0f;
  [Space(10)]
  [SerializeField] private CanvasGroup DescriptionGroup = null;
  [SerializeField] private TextMeshProUGUI DescriptionText_size = null;
  [SerializeField] private TextMeshProUGUI DescriptionText = null;
  [SerializeField] private CanvasGroup DescriptionTextGroup = null;
  [SerializeField] private RectTransform DescriptionRect = null;
  private Vector2 DescriptionClosePos = new Vector2(1150.0f, 280.0f);
  private Vector2 DescriptionOpenPos = new Vector2(45.0f, 280.0f);
  [Space(10)]
  [SerializeField] private CanvasGroup CenterGroup = null;
  [SerializeField] private UI_Selection Selection_None = null;
  [SerializeField] private UI_Selection Selection_Rational = null;
  [SerializeField] private UI_Selection Selection_Physical = null;
  [SerializeField] private UI_Selection Selection_Mental = null;
  [SerializeField] private UI_Selection Selection_Material = null;
  [SerializeField] private ParticleSystem SuccessParticle = null;
  [SerializeField] private ParticleSystem FailParticle = null;
  [Space(10)]
  [SerializeField] private CanvasGroup SettlementButton = null;
  [SerializeField] private UI_Reward MyUIReward = null;
  [SerializeField] private GameObject RewardButton = null;
  [SerializeField] private CanvasGroup KeepmoveButton=null;
  [SerializeField] private TextMeshProUGUI KeepMoveText = null;
  [Space(10)]
  [SerializeField] private CanvasGroup EndingGroup = null;
  
  private void Start()
  {
    if (KeepMoveText.text.Equals("null")) KeepMoveText.text = GameManager.Instance.GetTextData("KEEPGOING");
  }
  private UI_Selection GetUISelection(TendencyType _tendencytype,int index)
  {
    switch (_tendencytype)
    {
      case TendencyType.None:return Selection_None;
      case TendencyType.Body:
        if (index == 0) return Selection_Rational;
        else return Selection_Physical;
      case TendencyType.Head:
        if(index==0) return Selection_Mental;
        else return Selection_Material;
      default:return null;
    }
  }
  private UI_Selection GetOppositeSelection(TendencyType _tendencytype,int index)
  {
    switch (_tendencytype)
    {
      case TendencyType.None: return Selection_None;
      case TendencyType.Body:
        if (index == 1) return Selection_Rational;
        else return Selection_Physical;
      case TendencyType.Head:
        if (index == 1) return Selection_Mental;
        else return Selection_Material;
      default: return null;
    }
  }
  private UI_Selection GetOppositeSelection(UI_Selection _selection)
  {
    if (_selection.Equals(Selection_None)) return null;
    if (_selection.Equals(Selection_Rational)) return Selection_Physical;
    if (_selection.Equals(Selection_Physical)) return Selection_Rational;
    if (_selection.Equals(Selection_Mental)) return Selection_Material;
    if (_selection.Equals(Selection_Material)) return Selection_Mental;
    return null;
  }
  public void SetEventDialogue()
  {

    //초기화
        UIManager.Instance.AddUIQueue(setnewdialogue());
  }//이벤트 시작 열기
  public void SetEndingDialogue(FollowEndingData endingdata,SuccessData successdata)
  {
    UIManager.Instance.AddUIQueue(updatedialogue(endingdata, successdata));
  }
  public void SetEventDialogue(SuccessData _successdata)
  {
    UIManager.Instance.AddUIQueue(updatedialogue(_successdata));
  }//성공 데이터를 바탕으로 열기
  public void SetEventDialogue(FailureData _faildata)
  {
    UIManager.Instance.AddUIQueue(updatedialogue(_faildata));

  }//성공 데이터를 바탕으로 열기
  public void DeleteOtherSelection(int index)
  {
    EventDataDefulat _currentevent = GameManager.Instance.MyGameData.CurrentEvent;
    switch (_currentevent.Selection_type)
    {
      case SelectionType.Single:
        break;
      case SelectionType.Body:
        StartCoroutine(UIManager.Instance.ChangeAlpha(CenterGroup, 0.0f, false, false));
        //가운데 장식품 제거

        if (index.Equals(0))
        {
          Selection_Physical.DeActive();
        }
        else
        {
          Selection_Rational.DeActive();
        }
        break;
      case SelectionType.Head:
        StartCoroutine(UIManager.Instance.ChangeAlpha(CenterGroup, 0.0f, false, false));
        //가운데 장식품 제거

        if (index.Equals(0))
        {
          Selection_Material.DeActive();
        }
        else
        {
          Selection_Mental.DeActive();
        }
        break;
      case SelectionType.Tendency:
      case SelectionType.Experience:
        break;
    }
  }//선택지 선택한 후 다른 선택지들 닫기
  public void SelectSelection(UI_Selection _selection)
  {
    StartCoroutine(UIManager.Instance.ChangeAlpha(CenterGroup, 0.0f, true, false));
    DeleteOtherSelection(_selection.Index);
    //다른거 사라지게 만들고
    UIManager.Instance.AddUIQueue(selectionanimation(_selection));
    //선택한 선택지를 중심으로 이동시키고 성공, 실패 검사 실행 
  }//선택지 버튼 클릭했을 시
  private IEnumerator setnewdialogue()
  {
    EventDataDefulat _currentevent = GameManager.Instance.MyGameData.CurrentEvent;

    UIManager.Instance.UpdateBackground(_currentevent.EnvironmentType);

    string _descriptiontemp = _currentevent.Description;
    if (_descriptiontemp.Contains("#SETTLE#")) _descriptiontemp = _currentevent.Description.Replace("#SETTLE#", GameManager.Instance.MyGameData.CurrentSettlement.Name);
    Sprite _illust = _currentevent.Illust;
    string _name = _currentevent.Name;

    if (Selection_None.gameObject.activeInHierarchy == true) Selection_None.gameObject.SetActive(false);
    if (Selection_Rational.gameObject.activeInHierarchy == true) Selection_Rational.gameObject.SetActive(false);
    if (Selection_Physical.gameObject.activeInHierarchy == true) Selection_Physical.gameObject.SetActive(false);
    if (Selection_Mental.gameObject.activeInHierarchy == true) Selection_Mental.gameObject.SetActive(false);
    if (Selection_Material.gameObject.activeInHierarchy == true) Selection_Material.gameObject.SetActive(false);
    Selection_None.GetComponent<RectTransform>().anchoredPosition = Selection_None.OriginPos;
    Selection_Rational.GetComponent<RectTransform>().anchoredPosition = Selection_Rational.OriginPos;
    Selection_Physical.GetComponent<RectTransform>().anchoredPosition = Selection_Physical.OriginPos;
    Selection_Mental.GetComponent<RectTransform>().anchoredPosition = Selection_Mental.OriginPos;
    Selection_Material.GetComponent<RectTransform>().anchoredPosition = Selection_Material.OriginPos;
    //모든 선택지 위치 초기화 및 숨기기
    IllustRect.anchoredPosition = IllustClosePos;
    DescriptionRect.anchoredPosition = DescriptionClosePos;

    DescriptionGroup.alpha = 1.0f;
    DescriptionTextGroup.alpha = 0.0f;
    IllustImageGroup.alpha = 0.0f;
    IllustGroup.alpha = 1.0f;
    DescriptionText.text = _descriptiontemp;
    DescriptionText_size.text = _descriptiontemp;
     StartCoroutine(UIManager.Instance.moverect(IllustRect, IllustClosePos, IllustOpenPos, DialogueUIMoveTime, UIManager.Instance.UIPanelOpenCurve));
    yield return new WaitForSeconds(0.3f);
    StartCoroutine(UIManager.Instance.moverect(DescriptionRect, DescriptionClosePos, DescriptionOpenPos, DialogueUIMoveTime, UIManager.Instance.UIPanelOpenCurve));
    yield return Wait;
    NameText.text = _name;
    StartCoroutine(UIManager.Instance.ChangeAlpha(NameTextGroup, 1.0f, UIFadeTime,false));
    yield return TextWait;
    //일러스트, 설명 이동시키고 이름만 출현

    Illust.sprite = _illust;
    StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 1.0f, UIFadeTime, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup,1.0f,UIFadeTime,false));
    yield return Wait;
    //일러스트,내용 동시에 표시

    switch (_currentevent.Selection_type)
    {
      case SelectionType.Single:
        Selection_None.gameObject.SetActive(true);
        Selection_None.Active(_currentevent.SelectionDatas[0]);
        break;
      case SelectionType.Body:
        StartCoroutine(UIManager.Instance.ChangeAlpha(CenterGroup, 1.0f, false, false));
                //가운데 있는 장식품

        Selection_Rational.gameObject.SetActive(true);
        Selection_Rational.Active(_currentevent.SelectionDatas[0]);
        yield return new WaitForSeconds(0.3f);
        Selection_Physical.gameObject.SetActive(true);
        Selection_Physical.Active(_currentevent.SelectionDatas[1]);
        break;
      case SelectionType.Head:
        StartCoroutine(UIManager.Instance.ChangeAlpha(CenterGroup, 1.0f, false, false));
                //가운데 있는 장식품

                Selection_Mental.gameObject.SetActive(true);
        Selection_Mental.Active(_currentevent.SelectionDatas[0]);
        yield return new WaitForSeconds(0.3f);
        Selection_Material.gameObject.SetActive(true);
        Selection_Material.Active(_currentevent.SelectionDatas[1]);
        break;
      case SelectionType.Tendency:
      case SelectionType.Experience:
        Selection_None.gameObject.SetActive(true);
        Selection_None.Active(_currentevent.SelectionDatas[0]);
        break;
    }
        //선택지 오브젝트들이 가운데부터 등장
  }
  private IEnumerator updatedialogue(FollowEndingData endingdata,SuccessData successdata)
  {
    SuccessParticle.Play();
    CurrentUISelection.DeActive();

    StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 0.0f, UIFadeTime, false));
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 0.0f, UIFadeTime, false));
    //일러스트,설명 투명화하고 약간 시간 둔 뒤 새 일러스트,설명으로 전환
    yield return TextWait;

    Illust.sprite = successdata.Illust;
    DescriptionText.text = successdata.Description;
    DescriptionText_size.text = successdata.Description;
    yield return new WaitForEndOfFrame();
    StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 1.0f, UIFadeTime, false));
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 1.0f, UIFadeTime, false));
    yield return ResultWait;

    yield return StartCoroutine(openendingbuttons());
  }
  private IEnumerator updatedialogue(SuccessData _success)
  {
    SuccessParticle.Play();
    CurrentUISelection.DeActive();

    StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 0.0f,UIFadeTime,false));
   yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 0.0f,UIFadeTime,false));
    //일러스트,설명 투명화하고 약간 시간 둔 뒤 새 일러스트,설명으로 전환
    yield return TextWait;

    Illust.sprite = _success.Illust;
    DescriptionText_size.text = _success.Description;
    DescriptionText.text = _success.Description;
    yield return new WaitForEndOfFrame();
    StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 1.0f, UIFadeTime, false));
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 1.0f, UIFadeTime, false));
    yield return ResultWait;

    OpenButtons(_success);
  }
  private IEnumerator updatedialogue(FailureData _faiilure)
  {
    yield return Wait;
    FailParticle.Play();
    CurrentUISelection.DeActive();

    StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 0.0f, UIFadeTime, false));
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 0.0f, UIFadeTime, false));
    //일러스트,설명 투명화하고 약간 시간 둔 뒤 새 일러스트,설명으로 전환
    yield return TextWait;

    Illust.sprite = _faiilure.Illust;
    DescriptionText_size.text = _faiilure.Description;
    DescriptionText.text = _faiilure.Description;
    yield return new WaitForEndOfFrame();
    StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 1.0f, UIFadeTime, false));
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 1.0f, UIFadeTime, false));
    yield return ResultWait;


    if (GameManager.Instance.MyGameData.CurrentSettlement == null)
    {
      StartCoroutine(UIManager.Instance.ChangeAlpha(KeepmoveButton, 1.0f, false, false));
    }
    OpenButtons(_faiilure);
  }
  public override void CloseUI()
  {
    UIManager.Instance.AddUIQueue(closeui());
  }//이벤트->정착지로 천천히 나갈때
  private IEnumerator closeui()
  {
    CloseButtons();

    StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionGroup, 0.0f, UIFadeTime, false));
    StartCoroutine(UIManager.Instance.moverect(DescriptionRect, DescriptionOpenPos, DescriptionClosePos, DialogueUIMoveTime, UIManager.Instance.UIPanelCLoseCurve));
    yield return new WaitForSeconds(0.3f);
    DescriptionTextGroup.alpha = 0.0f;

    StartCoroutine(UIManager.Instance.ChangeAlpha(IllustGroup, 0.0f, UIFadeTime, false));
    yield return StartCoroutine(UIManager.Instance.moverect(IllustRect, IllustOpenPos, IllustClosePos, DialogueUIMoveTime, UIManager.Instance.UIPanelCLoseCurve));
    IllustImageGroup.alpha = 0.0f;
    NameText.text = "";

    yield return Wait;
  }
  public void ClosePanel_quick()
  {
    StartCoroutine(closepanelquick());
  }//빠르게 닫기(지도 아래서 닫는 경우)
  private IEnumerator closepanelquick()
  {
    CanvasGroup _none = Selection_None.GetComponent<CanvasGroup>();
    _none.alpha = 0.0f;
    _none.interactable = false;
    _none.blocksRaycasts = false;
    CanvasGroup _ra = Selection_Rational.GetComponent<CanvasGroup>();
    _ra.alpha = 0.0f;
    _ra.interactable = false;
    _ra.blocksRaycasts = false;
    CanvasGroup _ph = Selection_Physical.GetComponent<CanvasGroup>();
    _ph.alpha = 0.0f;
    _ph.interactable = false;
    _ph.blocksRaycasts = false;
    CanvasGroup _me = Selection_Mental.GetComponent<CanvasGroup>();
    _me.alpha = 0.0f;
    _me.interactable = false;
    _me.blocksRaycasts = false;
    CanvasGroup _ma = Selection_Material.GetComponent<CanvasGroup>();
    _ma.alpha = 0.0f;
    _ma.interactable = false;
    _ma.blocksRaycasts = false;
    CloseButtons();
    yield return null;
    StartCoroutine(UIManager.Instance.ChangeAlpha(IllustGroup, 0.0f,0.3f,false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionGroup, 0.0f, 0.3f, false));
    NameText.text = "";
    NameTextGroup.alpha = 0.0f;
    Illust.sprite = GameManager.Instance.ImageHolder.NoneIllust;
    IllustImageGroup.alpha = 0.0f;
    DescriptionText_size.text = "";
    DescriptionText.text = "";
    DescriptionTextGroup.alpha = 0.0f;

    IllustRect.anchoredPosition = IllustClosePos;
    yield return null;
  }
  private void OpenButtons(SuccessData _success)
  {
    UIManager.Instance.AddUIQueue(openbuttons(_success));
  }
  private IEnumerator openbuttons(SuccessData _success)
  {
    if (GameManager.Instance.MyGameData.CurrentSettlement != null)
    {
            SettlementButton.alpha = 0.0f;
      if (SettlementButton.gameObject.activeInHierarchy == false) SettlementButton.gameObject.SetActive(true);
       StartCoroutine(UIManager.Instance.ChangeAlpha(SettlementButton, 1.0f, false, false));
    }
    else
    {
            KeepmoveButton.alpha = 0.0f;
      if (KeepmoveButton.gameObject.activeInHierarchy == false) KeepmoveButton.gameObject.SetActive(true);
      StartCoroutine(UIManager.Instance.ChangeAlpha(KeepmoveButton, 1.0f, false, false));
    }

    if (RewardButton.activeInHierarchy == false) RewardButton.gameObject.SetActive(true);
    StartCoroutine(UIManager.Instance.ChangeAlpha(RewardButton.GetComponent<CanvasGroup>(),1.0f, false, false));
    yield return null;
  }
  private IEnumerator openendingbuttons()
  {
    StartCoroutine(UIManager.Instance.ChangeAlpha(EndingGroup, 1.0f, UIFadeTime, false));
    yield return null;
  }
  public void OpenEnding()
  {
    if (UIManager.Instance.IsWorking) return;
    FollowEndingData _endingdata = ((FollowEventData)GameManager.Instance.MyGameData.CurrentEvent).EndingData;
    UIManager.Instance.OpenEnding(_endingdata);
  }
  public void RefuseEnding()
  {
    if (UIManager.Instance.IsWorking) return;
    UIManager.Instance.AddUIQueue(refuseending());
  }
  private IEnumerator refuseending()
  {
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(EndingGroup,0.0f,UIFadeTime,false));
    
    if (GameManager.Instance.MyGameData.CurrentSettlement != null)
    {
      SettlementButton.alpha = 0.0f;
      if (SettlementButton.gameObject.activeInHierarchy == false) SettlementButton.gameObject.SetActive(true);
      StartCoroutine(UIManager.Instance.ChangeAlpha(SettlementButton, 1.0f, false, false));
    }
    else
    {
      KeepmoveButton.alpha = 0.0f;
      if (KeepmoveButton.gameObject.activeInHierarchy == false) KeepmoveButton.gameObject.SetActive(true);
      StartCoroutine(UIManager.Instance.ChangeAlpha(KeepmoveButton, 1.0f, false, false));
    }

    if (RewardButton.activeInHierarchy == false) RewardButton.gameObject.SetActive(true);
    StartCoroutine(UIManager.Instance.ChangeAlpha(RewardButton.GetComponent<CanvasGroup>(), 1.0f, false, false));
    yield return null;
  }
  private void OpenButtons(FailureData _fail)
  {
    UIManager.Instance.AddUIQueue(openbuttons(_fail));
  }
  private IEnumerator openbuttons(FailureData _fail)
  {
    if (GameManager.Instance.MyGameData.CurrentSettlement != null)
    {
      SettlementButton.alpha = 0.0f;
      if (SettlementButton.gameObject.activeInHierarchy == false) SettlementButton.gameObject.SetActive(true);
      StartCoroutine(UIManager.Instance.ChangeAlpha(SettlementButton, 1.0f, false, false));
    }
    else
    {
      KeepmoveButton.alpha = 0.0f;
      if (KeepmoveButton.gameObject.activeInHierarchy == false) KeepmoveButton.gameObject.SetActive(true);
      StartCoroutine(UIManager.Instance.ChangeAlpha(KeepmoveButton, 1.0f, false, false));
    }
    yield return null;
  }
  private void CloseButtons()
  {
    EndingGroup.alpha = 0.0f;
    EndingGroup.interactable = false;
    EndingGroup.blocksRaycasts = false;
    if(KeepmoveButton.gameObject.activeInHierarchy == true) KeepmoveButton.gameObject.SetActive(false);
    KeepmoveButton.alpha = 0.0f;
    if (SettlementButton.gameObject.activeInHierarchy == true) SettlementButton.gameObject.SetActive(false);
    SettlementButton.alpha = 0.0f;
    if (RewardButton.activeInHierarchy == true) RewardButton.SetActive(false);
    RewardButton.GetComponent<CanvasGroup>().alpha = 0.0f;
  }

  private UI_Selection CurrentUISelection = null;
  private IEnumerator selectionanimation(UI_Selection _selection)
  {
    CurrentUISelection = _selection;
    if (!_selection.MyTendencyType.Equals(TendencyType.None))
    {
     yield return StartCoroutine(_selection.movetocenter());
    }//성향이 존재한다면 가운데로 이동시킴
   
    SelectionData _selectiondata = _selection.MySelectionData;
    int _currentvalue = 0,_checkvalue=0;
    int _successpercent = 0;
    bool _issuccess = false;
        int _pluspercent = GameManager.Instance.MyGameData.PlaceEffects.ContainsKey(PlaceType.Academy) ? ConstValues.PlaceEffect_acardemy : 0;
    //아카데미 장소 효과 있으면 확률 증가
    switch (_selectiondata.ThisSelectionType)
    {
      case SelectionTargetType.None:
        _issuccess = true;
        break;
      case SelectionTargetType.Pay:
        if (_selectiondata.SelectionPayTarget.Equals(StatusType.HP))
        {
          _issuccess = true;
          GameManager.Instance.MyGameData.HP -= GameManager.Instance.MyGameData.PayHPValue_modified;
          UIManager.Instance.UpdateHPText();
        }
        else if (_selectiondata.SelectionPayTarget.Equals(StatusType.Sanity))
        {
          _issuccess = true;//체력,정신력 지불의 경우 남은 값과 상관 없이 일단 성공으로만 친다
          GameManager.Instance.MyGameData.CurrentSanity -= GameManager.Instance.MyGameData.PaySanityValue_modified;
          UIManager.Instance.UpdateSanityText();
        }
        else        //돈 지불일 경우 돈 적을 때 실행하는 뭔가 있어야 함
        {
          int _paygoldvalue = (int)(GameManager.Instance.MyGameData.PayGoldValue_modified * GameManager.Instance.MyGameData.GetGoldPayModify(true));
          int _goldsuccesspercent = GameManager.Instance.MyGameData.Gold >= _paygoldvalue ? 100 : GameManager.Instance.MyGameData.CheckPercent_money(_paygoldvalue);
          if (_goldsuccesspercent + _pluspercent >= 100)
          {
            if (_goldsuccesspercent < 100 && !_pluspercent.Equals(0)) GameManager.Instance.MyGameData.PlaceEffects.Remove(PlaceType.Academy);
            //장소 효과의 도움을 받아 성공한 것이라면 장소 효과 만료
            _issuccess = true;
            GameManager.Instance.MyGameData.Gold -= GameManager.Instance.MyGameData.PayGoldValue_modified;
            UIManager.Instance.UpdateGoldText();
            Debug.Log("정당한 값을 지불한 레후~");
          }//100% 확률이 나온 상황(돈이 부족하거나 돈이 충분하거나 둘 다)
          else
          {
            if (Random.Range(0, 100) < _goldsuccesspercent + _pluspercent)
            {
              if (_goldsuccesspercent < 100 && !_pluspercent.Equals(0)) GameManager.Instance.MyGameData.PlaceEffects.Remove(PlaceType.Academy);
              int _elsevalue = GameManager.Instance.MyGameData.PayGoldValue_modified - GameManager.Instance.MyGameData.Gold;
              //장소 효과의 도움을 받아 성공한 것이라면 장소 효과 만료
              _issuccess = true;
              GameManager.Instance.MyGameData.Gold = 0;
              GameManager.Instance.MyGameData.CurrentSanity -= (int)(_elsevalue * ConstValues.GoldSanityPayAmplifiedValue);
              UIManager.Instance.UpdateGoldText();
            }//돈이 부족해 성공한 경우
            else
            {
              _issuccess = false;
              UIManager.Instance.UpdateSanityText();
              UIManager.Instance.UpdateGoldText();
            }//돈이 부족해 실패한 경우
          }//돈이 부족해 체크를 해야 하는 상황
        }
        break;
      case SelectionTargetType.Check_Multy: //기술(단수) 선택지면 확률 검사
        _currentvalue = GameManager.Instance.MyGameData.GetSkill(_selectiondata.SelectionCheckSkill[0]).Level;
        _checkvalue = GameManager.Instance.MyGameData.CheckSkillSingleValue;
        _successpercent = GameManager.Instance.MyGameData.CheckPercent_themeorskill(_currentvalue, _checkvalue);
        if (Random.Range(0, 100) < _successpercent + _pluspercent)
        {
          _issuccess = true;
          if (_successpercent < 100 && _pluspercent > 0)
          {
            GameManager.Instance.MyGameData.PlaceEffects.Remove(PlaceType.Academy);
            if (UIManager.Instance != null) UIManager.Instance.UpdatePlaceEffect();
          }
          //장소 효과의 도움을 받아 성공한 것이라면 효과 만료
        }
        else _issuccess = false;
        break;
      case SelectionTargetType.Check_Single: //기술(복수) 선택지면 확률 검사
        _currentvalue = GameManager.Instance.MyGameData.GetSkill(_selectiondata.SelectionCheckSkill[0]).Level+
          GameManager.Instance.MyGameData.GetSkill(_selectiondata.SelectionCheckSkill[1]).Level;
        _checkvalue = GameManager.Instance.MyGameData.CheckSkillMultyValue;
        _successpercent = GameManager.Instance.MyGameData.CheckPercent_themeorskill(_currentvalue, _checkvalue);
        if (Random.Range(0, 100) < _successpercent + _pluspercent)
        {
          _issuccess = true;
          if (_successpercent < 100 && _pluspercent > 0)
          {
            GameManager.Instance.MyGameData.PlaceEffects.Remove(PlaceType.Academy);
            if (UIManager.Instance != null) UIManager.Instance.UpdatePlaceEffect();
          }
          //장소 효과의 도움을 받아 성공한 것이라면 효과 만료
        }
        else _issuccess = false;
        break;
      case SelectionTargetType.Tendency:
        GameManager.Instance.MyGameData.MixTendency();
        _issuccess = true;
        break;
      case SelectionTargetType.Exp:
        GameManager.Instance.MyGameData.MixExp();
        _issuccess = true;
        break;
    }

    if (_issuccess) //성공하면 성공
    {
      Debug.Log("성공함");
      SetSuccess(GameManager.Instance.MyGameData.CurrentEvent.SuccessDatas[_selection.Index]);
      GameManager.Instance.SuccessCurrentEvent(_selection.MyTendencyType,_selection.Index);
    }
    else            //실패하면 실패
    {
      Debug.Log("실패함");
      SetFail(GameManager.Instance.MyGameData.CurrentEvent.FailureDatas[_selection.Index]);
      GameManager.Instance.FailCurrentEvent(_selection.MyTendencyType, _selection.Index);
    }
    GameManager.Instance.EventHolder.RemoveEvent(GameManager.Instance.MyGameData.CurrentEvent.ID);

  }//선택한 선택지 성공 여부를 계산하고 애니메이션을 실행시키는 코루틴

  public void SetSuccess(SuccessData _success)
  {
    MyUIReward.SetRewardPanel(_success);

    var _currentevent = GameManager.Instance.MyGameData.CurrentEvent;

    if (_currentevent.GetType().Equals(typeof(FollowEventData)))
    {
      var _currentfollow=(FollowEventData)_currentevent;
      if(_currentfollow.EndingData!=null) SetEndingDialogue(((FollowEventData)_currentevent).EndingData, _success);
    }
    else SetEventDialogue(_success);
    //연계 이벤트고, 엔딩 설정이 돼 있는 상태에서 성공할 경우 엔딩 다이어로그 전개


  }//성공할 경우 보상 탭을 세팅하고 텍스트를 성공 설명으로 교체, 퀘스트 이벤트일 경우 진행도 ++
  public void SetFail(FailureData _fail)
  {
    SetPenalty(_fail);
    SetEventDialogue(_fail);
  }//실패할 경우 패널티를 부과하고 텍스트를 실패 설명으로 교체
  public void DeleteRewardButton()
  {
    RewardButton.SetActive(false);
  }
  public void SetPenalty(FailureData _fail)
  {
    switch (_fail.Panelty_target)
    {
      case PenaltyTarget.None:
        break;
      case PenaltyTarget.Status:
        switch (_fail.Loss_target)
        {
          case StatusType.HP:
            GameManager.Instance.MyGameData.HP -= GameManager.Instance.MyGameData.FailHPValue_modified;
            UIManager.Instance.UpdateHPText();
            break;
          case StatusType.Sanity:
            GameManager.Instance.MyGameData.CurrentSanity -= GameManager.Instance.MyGameData.FailSanityValue_modified;
            UIManager.Instance.UpdateSanityText();
            break;
          case StatusType.Gold:
            GameManager.Instance.MyGameData.Gold -= GameManager.Instance.MyGameData.FailGoldValue_modified;
            UIManager.Instance.UpdateGoldText();
            break;
        }
        break;
      case PenaltyTarget.EXP:
        GameManager.Instance.AddBadExp(GameManager.Instance.ExpDic[_fail.ExpID]);
        break;
    }
  }//실패할 경우 패널티 부과하는 연출
}
