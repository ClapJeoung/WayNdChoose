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
  [SerializeField] private CanvasGroup IllustImageGroup = null;
  [SerializeField] private Image IllustImage = null;
  private float IllustFullChargingTime = 3.0f;
  private float IllustChargingWaitTime = 1.0f;
  [Space(10)]
  [SerializeField] private TextMeshProUGUI DescriptionText = null;
  [SerializeField] private CanvasGroup DescriptionTextGroup = null;
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
  [SerializeField] private CanvasGroup RewardButtonGroup = null;
  [SerializeField] private Image RewardIcon = null;
  [SerializeField] private TextMeshProUGUI RewardName = null;
  [SerializeField] private TextMeshProUGUI RewardDescription = null;
  [SerializeField] private TextMeshProUGUI Reward_clicktoget = null;
  [Space(10)]
  [SerializeField] private CanvasGroup EndingGroup = null;
  [Space(10)]
  [SerializeField] private UI_Settlement SettlementUI = null;
  [SerializeField] private UI_map MapUI = null;
  private EventDataDefulat CurrentEvent = null;
  private RectTransform IllustRect { get {return GetPanelRect("illust").Rect; } }
  private Vector2 IllustOpenPos { get { return GetPanelRect("illust").InsidePos; } }
  private Vector2 IllustClosePos { get { return GetPanelRect("illust").OutisdePos; } }
  private RectTransform DescriptionRect { get { return GetPanelRect("Description").Rect; } }
  private Vector2 DescriptionOpenPos { get { return GetPanelRect("Description").InsidePos; } }
  private Vector2 DescriptionClosePos { get { return GetPanelRect("Description").OutisdePos; } }

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
  public void OpenUI()
  {
    UIManager.Instance.AddUIQueue(setnewdialogue());
  }
  private IEnumerator setnewdialogue()
  {
    if (Reward_clicktoget.text == "") Reward_clicktoget.text = GameManager.Instance.GetTextData("GETREWARD");
    if(DefaultRect.anchoredPosition!=Vector2.zero)DefaultRect.anchoredPosition = Vector2.zero;
    CurrentEvent = GameManager.Instance.MyGameData.CurrentEvent;
    UIManager.Instance.UpdateBackground(CurrentEvent.EnvironmentType);

    string _descriptiontemp = CurrentEvent.Description;
    if (_descriptiontemp.Contains("#SETTLE#")) _descriptiontemp = CurrentEvent.Description.Replace("#SETTLE#", GameManager.Instance.MyGameData.CurrentSettlement.Name);
    Sprite _illust = CurrentEvent.Illust;
    string _name = CurrentEvent.Name;

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

    IllustImageGroup.alpha = 0.0f;
    IllustImage.sprite = _illust;
    NameText.text = _name;
    NameTextGroup.alpha = 0.0f;
    StartCoroutine(UIManager.Instance.moverect(IllustRect, IllustClosePos, IllustOpenPos, DialogueUIMoveTime, UIManager.Instance.UIPanelOpenCurve));
    yield return new WaitForSeconds(0.3f);
    //일러스트+이름 세팅해두고 투명화 -> 이동

    DescriptionTextGroup.alpha = 0.0f;
    DescriptionText.text = _descriptiontemp;
    StartCoroutine(UIManager.Instance.moverect(DescriptionRect, DescriptionClosePos, DescriptionOpenPos, DialogueUIMoveTime, UIManager.Instance.UIPanelOpenCurve));
    yield return Wait;
    //설명 텍스트 세팅해두고 투명화 -> 이동

    StartCoroutine(UIManager.Instance.ChangeAlpha(NameTextGroup, 1.0f, UIFadeTime, false));
    yield return TextWait;
    //이름 투명화 -> 드러내기

    StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 1.0f, UIFadeTime, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 1.0f, UIFadeTime, false));
    yield return Wait;
    //일러스트+설명 투명화 -> 드러내기

    if(CurrentEvent.Selection_type==SelectionType.Body|| CurrentEvent.Selection_type==SelectionType.Head)
      StartCoroutine(UIManager.Instance.ChangeAlpha(CenterGroup, 1.0f, false, false));
    //양자택일 형태 선택지일 경우 가운데 구분 이미지 활성화

    switch (CurrentEvent.Selection_type)
    {
      case SelectionType.Single:
      case SelectionType.Tendency:
      case SelectionType.Experience:
        Selection_None.gameObject.SetActive(true);
        Selection_None.Active(CurrentEvent.SelectionDatas[0]);
        break;
      case SelectionType.Body:

        Selection_Rational.gameObject.SetActive(true);
        Selection_Rational.Active(CurrentEvent.SelectionDatas[0]);
        yield return new WaitForSeconds(0.3f);
        Selection_Physical.gameObject.SetActive(true);
        Selection_Physical.Active(CurrentEvent.SelectionDatas[1]);
        break;
      case SelectionType.Head:

        Selection_Mental.gameObject.SetActive(true);
        Selection_Mental.Active(CurrentEvent.SelectionDatas[0]);
        yield return new WaitForSeconds(0.3f);
        Selection_Material.gameObject.SetActive(true);
        Selection_Material.Active(CurrentEvent.SelectionDatas[1]);
        break;
    }
    //선택지 오브젝트들 세팅 + 이동
  }
  private UI_Selection CurrentUISelection = null;
  /// <summary>
  /// 선택지 클릭했을때 선택지 스크립트에서 호출
  /// </summary>
  /// <param name="_selection"></param>
  public void SelectSelection(UI_Selection _selection)
  {
    if (_selection.MyTendencyType != TendencyType.None)
    {
      GetOppositeSelection(_selection).DeActive();
      StartCoroutine(UIManager.Instance.ChangeAlpha(CenterGroup, 0.0f, true, false));
    }
    //다른거 사라지게 만들고
    UIManager.Instance.AddUIQueue(selectionanimation(_selection));
    //선택한 선택지를 중심으로 이동시키고 성공, 실패 검사 실행 
  }
  private IEnumerator selectionanimation(UI_Selection _selection)
  {
    CurrentUISelection = _selection;
    if (!_selection.MyTendencyType.Equals(TendencyType.None))
    {
      yield return StartCoroutine(_selection.movetocenter());
    }//성향이 존재한다면 가운데로 이동시킴

    SelectionData _selectiondata = _selection.MySelectionData;
    int _currentvalue = 0, _checkvalue = 0;    //기술 체크에만 사용
    int _successpercent = 0;                   //성공 확률(골드 혹은 기술 체크) 
    bool _issuccess = false;  
    int _pluspercent = GameManager.Instance.MyGameData.PlaceEffects.ContainsKey(PlaceType.Academy) ? ConstValues.PlaceEffect_acardemy : 0;
                                               //학술원 방문 시 확률 증가 값
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
        _currentvalue = GameManager.Instance.MyGameData.GetSkill(_selectiondata.SelectionCheckSkill[0]).Level +
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
      GameManager.Instance.SuccessCurrentEvent(_selection.MyTendencyType, _selection.Index);
    }
    else            //실패하면 실패
    {
      Debug.Log("실패함");
      SetFail(GameManager.Instance.MyGameData.CurrentEvent.FailureDatas[_selection.Index]);
      GameManager.Instance.FailCurrentEvent(_selection.MyTendencyType, _selection.Index);
    }
    GameManager.Instance.EventHolder.RemoveEvent(GameManager.Instance.MyGameData.CurrentEvent.ID);

  }//선택한 선택지 성공 여부를 계산하고 애니메이션을 실행시키는 코루틴
  private SuccessData CurrentSuccessData = null;
  public void SetSuccess(SuccessData _success)
  {
    CurrentSuccessData = _success;
    Sprite _icon = null;
    string _name = "";
    string _description = "";
    switch (_success.Reward_Target)
    {
      case RewardTarget.HP:
        _icon = GameManager.Instance.ImageHolder.HPIcon;
        _name=GameManager.Instance.GetTextData(StatusType.HP,0);
        _description = $"+{WNCText.GetHPColor(GameManager.Instance.MyGameData.RewardHPValue_modified)}";
        break;
      case RewardTarget.Sanity:
        _icon = GameManager.Instance.ImageHolder.SanityIcon;
        _name = GameManager.Instance.GetTextData(StatusType.Sanity, 0);
        _description = $"+{WNCText.GetSanityColor(GameManager.Instance.MyGameData.RewardSanityValue_modified)}";
        break;
      case RewardTarget.Gold:
        _icon = GameManager.Instance.ImageHolder.GoldIcon;
        _name = GameManager.Instance.GetTextData(StatusType.Gold, 0);
        _description = $"+{WNCText.GetGoldColor(GameManager.Instance.MyGameData.RewardGoldValue_modified)}";
        break;
      case RewardTarget.Experience:
        _icon = GameManager.Instance.ImageHolder.UnknownExp;
        _name = GameManager.Instance.GetTextData("EXP_NAME");
        _description = GameManager.Instance.ExpDic[CurrentSuccessData.Reward_ID].Name;
        break;
      case RewardTarget.Skill:
        _icon = GameManager.Instance.ImageHolder.GetSkillIcon(CurrentSuccessData.Reward_Skill);
        _name = $"{GameManager.Instance.GetTextData(CurrentSuccessData.Reward_Skill,0)} +1";
        break;
    }
    RewardIcon.sprite = _icon;
    RewardName.text = _name;
    RewardDescription.text = _description;

    var _currentevent = GameManager.Instance.MyGameData.CurrentEvent;

    if (_currentevent.GetType().Equals(typeof(FollowEventData)))
    {
      var _currentfollow = (FollowEventData)_currentevent;
      if (_currentfollow.EndingData != null) SetEndingDialogue(((FollowEventData)_currentevent).EndingData, _success);
    }
    else SetEventDialogue(_success);
    //연계 이벤트고, 엔딩 설정이 돼 있는 상태에서 성공할 경우 엔딩 다이어로그 전개


  }//성공할 경우 보상 탭을 세팅하고 텍스트를 성공 설명으로 교체, 퀘스트 이벤트일 경우 진행도 ++
  public void SetFail(FailureData _fail)
  {
    SetPenalty(_fail);
    SetEventDialogue(_fail);
  }//실패할 경우 패널티를 부과하고 텍스트를 실패 설명으로 교체

  public void SetEventDialogue(SuccessData _successdata)=> UIManager.Instance.AddUIQueue(updatedialogue(_successdata));
  public void SetEventDialogue(FailureData _faildata) => UIManager.Instance.AddUIQueue(updatedialogue(_faildata));

  private IEnumerator updatedialogue(SuccessData _success)
  {
    SuccessParticle.Play();
    CurrentUISelection.DeActive();

    StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 0.0f,UIFadeTime,false));
   yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 0.0f,UIFadeTime,false));
    //일러스트,설명 투명화하고 약간 시간 둔 뒤 새 일러스트,설명으로 전환
    yield return TextWait;

    IllustImage.sprite = _success.Illust;
    DescriptionText.text = _success.Description;
    StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 1.0f, UIFadeTime, false));
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 1.0f, UIFadeTime, false));
    yield return ResultWait;


    StartCoroutine(UIManager.Instance.ChangeAlpha(RewardButtonGroup, 1.0f, false, false));

    MoveRectForButton(1);
    if (GameManager.Instance.MyGameData.CurrentSettlement != null)
    {
      UIManager.Instance.SettleButton.Open(0, this);
    }//정착지에서 이벤트를 끝낸 경우 정착지로 돌아가는 버튼 활성화
    else
    {
      UIManager.Instance.MapButton.Open(0, this);
    }//야외에서 이벤트를 끝낸 경우 야외로 돌아가는 버튼 활성화
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

    IllustImage.sprite = _faiilure.Illust;
    DescriptionText.text = _faiilure.Description;
    yield return new WaitForEndOfFrame();
    StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 1.0f, UIFadeTime, false));
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 1.0f, UIFadeTime, false));
    yield return ResultWait;

    MoveRectForButton(1);
    if (GameManager.Instance.MyGameData.CurrentSettlement != null)
    {
      UIManager.Instance.SettleButton.Open(0, this);
    }//정착지에서 이벤트를 끝낸 경우 정착지로 돌아가는 버튼 활성화
    else
    {
      UIManager.Instance.MapButton.Open(0, this);
    }//야외에서 이벤트를 끝낸 경우 야외로 돌아가는 버튼 활성화
  }
  public override void CloseUI() => UIManager.Instance.AddUIQueue(closeui());

  private IEnumerator closeui()
  {
    EndingGroup.alpha = 0.0f;
    EndingGroup.interactable = false;
    EndingGroup.blocksRaycasts = false;

    StartCoroutine(UIManager.Instance.ChangeAlpha(RewardButtonGroup, 0.0f, 0.3f, false));

    StartCoroutine(UIManager.Instance.moverect(DescriptionRect, DescriptionOpenPos, DescriptionClosePos, DialogueUIMoveTime, UIManager.Instance.UIPanelCLoseCurve));
    yield return new WaitForSeconds(0.3f);
    DescriptionTextGroup.alpha = 0.0f;

    yield return StartCoroutine(UIManager.Instance.moverect(IllustRect, IllustOpenPos, IllustClosePos, DialogueUIMoveTime, UIManager.Instance.UIPanelCLoseCurve));
    IllustImageGroup.alpha = 0.0f;
    NameText.text = "";

    yield return Wait;
  }
  public void SetEndingDialogue(FollowEndingData endingdata, SuccessData successdata)
  {
    UIManager.Instance.AddUIQueue(updatedialogue(endingdata, successdata));
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
  private IEnumerator updatedialogue(FollowEndingData endingdata, SuccessData successdata)
  {
    SuccessParticle.Play();
    CurrentUISelection.DeActive();

    StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 0.0f, UIFadeTime, false));
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 0.0f, UIFadeTime, false));
    //일러스트,설명 투명화하고 약간 시간 둔 뒤 새 일러스트,설명으로 전환
    yield return TextWait;

    IllustImage.sprite = successdata.Illust;
    DescriptionText.text = successdata.Description;

    StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 1.0f, UIFadeTime, false));
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 1.0f, UIFadeTime, false));
    yield return ResultWait;

    yield return StartCoroutine(openendingbuttons());
  }
  public void RefuseEnding()
  {
    if (UIManager.Instance.IsWorking) return;
    UIManager.Instance.AddUIQueue(refuseending());
  }
  private IEnumerator refuseending()
  {
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(EndingGroup,0.0f,UIFadeTime,false));

    yield return null;
  }

  public void GetReward()
  {
    if(CurrentSuccessData.Reward_Target==RewardTarget.Experience)
    {

    }

    else
    {
      switch (CurrentSuccessData.Reward_Target)
      {
        case RewardTarget.HP:
          GameManager.Instance.MyGameData.HP += GameManager.Instance.MyGameData.RewardHPValue_modified;
          UIManager.Instance.UpdateHPText();
          break;
        case RewardTarget.Sanity:
          GameManager.Instance.MyGameData.CurrentSanity += GameManager.Instance.MyGameData.RewardSanityValue_modified;
          UIManager.Instance.UpdateSanityText();
          break;
        case RewardTarget.Gold:
          GameManager.Instance.MyGameData.Gold += GameManager.Instance.MyGameData.RewardGoldValue_modified;
          UIManager.Instance.UpdateGoldText();
          break;
        case RewardTarget.Skill:
          GameManager.Instance.MyGameData.GetSkill(skill).LevelByDefault++;
          break;
      }
    }
  }

  public void DeleteRewardButton()
  {
    RewardButtonGroup.alpha = 0.0f;
    RewardButtonGroup.interactable = false;
    RewardButtonGroup.blocksRaycasts = false;
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
