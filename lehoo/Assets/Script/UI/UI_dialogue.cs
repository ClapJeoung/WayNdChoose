using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_dialogue : UI_default
{
  public float OpenTime = 1.5f;
  public float CloseTime = 1.0f;
  private float MoveForButtonTime = 0.5f;

  [SerializeField] private TextMeshProUGUI NameText = null;
  [SerializeField] private ImageSwapScript Illust = null;
  [SerializeField] private Image IllustEffect_Image = null;
  [SerializeField] private CanvasGroup IllustEffect_Group = null;
  [SerializeField] private Color IdleColor = Color.white;
  [SerializeField] private Color RationalColor = Color.white;
  [SerializeField] private Color PhysicalColor = Color.white;
  [SerializeField] private Color MentalColor = Color.white;
  [SerializeField] private Color MaterialColor = Color.white;
  [SerializeField] private Color FailColor = Color.red;
  // [SerializeField] private float FadeOutTime = 0.7f;
  // [SerializeField] private float FadeInTime = 0.9f;
  //[SerializeField] private float FadeWaitTime = 0.3f;
  public float FadeTime = 0.8f;
  public float DisableAlpha = 0.2f;
  [SerializeField] private float ButtonFadeinTime = 0.4f;
 // [SerializeField] private Image EventIcon = null;
 // private GameObject EventIconHolder { get { return EventIcon.transform.parent.gameObject; } }
  [Space(10)]
  [SerializeField] private TextMeshProUGUI DescriptionText = null;
  public Scrollbar DescriptionScrollBar = null;
  public AnimationCurve ScrollbarCurve = new AnimationCurve();
  private IEnumerator updatescrollbar()
  {
    yield return new WaitForSeconds(0.05f);

    float _time = 0.0f;
    float _targettime = 1.0f;
    while (DescriptionScrollBar.value>0.001f)
    {
      DescriptionScrollBar.value = Mathf.Lerp(DescriptionScrollBar.value, 0.0f, 0.013f);
      _time += Time.deltaTime;
      yield return null;

    }
    DescriptionScrollBar.value = 0.0f;
  }
  // [SerializeField] private CanvasGroup DescriptionTextGroup = null;
  [Space(10)]
  [SerializeField] private CanvasGroup NextButtonGroup = null;
  private void SetNextButtonDisable()
  {
    NextButtonGroup.alpha = DisableAlpha;
    NextButtonGroup.interactable = false;
  }
  public void SetNextButtonActive()
  {
    NextButtonGroup.alpha = 1.0f;
    NextButtonGroup.interactable = true;
  }

  //  [SerializeField] private UnityEngine.UI.Button NextButton = null;
  [SerializeField] private TextMeshProUGUI NextButtonText = null;
  [Space(10)]
  [SerializeField] private CanvasGroup SelectionGroup = null;
  [SerializeField] private UI_Selection Selection_A = null;
  [SerializeField] private UI_Selection Selection_B = null;
//  [SerializeField] private ParticleSystem SuccessParticle = null;
//  [SerializeField] private ParticleSystem FailParticle = null;
  [Space(10)]
  [SerializeField] private CanvasGroup RewardButtonGroup = null;
  [SerializeField] private Image RewardIcon = null;
 // [SerializeField] private TextMeshProUGUI RewardName = null;
  [SerializeField] private TextMeshProUGUI RewardDescription = null;
 // [SerializeField] private TextMeshProUGUI Reward_clicktoget = null;
  [SerializeField] private Onpointer_highlight Reward_Highlight = null;
  [Space(10)]
  [SerializeField] private CanvasGroup EndingButtonGroup = null;
  [SerializeField] private TextMeshProUGUI EndingButtonText = null;
  [Space(10)]
  [SerializeField] private UI_Settlement SettlementUI = null;
  [SerializeField] private UI_map MapUI = null;
  [SerializeField] private UI_RewardExp RewardExpUI = null;
  private Vector2 LeftPos = new Vector2(-1350.0f, 0.0f);
  private Vector2 CenterPos = new Vector2(0.0f, 0.0f);
  private Vector2 RightPos = new Vector2(1670.0f, 0.0f);
  private EventData CurrentEvent
  {
    get { return GameManager.Instance.MyGameData.CurrentEvent; }
  }

  private UI_Selection GetOppositeSelection(UI_Selection _selection)
  {
    if (_selection == Selection_A) return Selection_B;
    return Selection_A;
  }
  public void OpenUI(bool dir)
  {
    IsOpen = true;
    UIManager.Instance.UpdateBackground(CurrentEvent.EnvironmentType);
    if (NextButtonText.text == "next") NextButtonText.text = GameManager.Instance.GetTextData("NEXT_TEXT");
  
    Reward_Highlight.Interactive = false;
    EndingButtonGroup.alpha = 0.0f;
    EndingButtonGroup.interactable = false;
    EndingButtonGroup.blocksRaycasts = false;

    CurrentEventIllustHolderes = CurrentEvent.BeginningIllusts;
    CurrentEventDescriptions = CurrentEvent.BeginningDescriptions;
    IsBeginning = true;
    CurrentEventPhaseIndex = 0;
    CurrentEventPhaseMaxIndex = CurrentEventIllustHolderes.Count;

    DescriptionText.text = "";

    UIManager.Instance.AddUIQueue(openui());
  }
  public void OpenUI(bool issuccess,bool isleft)
  {
    IsOpen = true;
    UIManager.Instance.UpdateBackground(CurrentEvent.EnvironmentType);
    if (NextButtonText.text == "next") NextButtonText.text = GameManager.Instance.GetTextData("NEXT_TEXT");

    Reward_Highlight.Interactive = false;
    EndingButtonGroup.alpha = 0.0f;
    EndingButtonGroup.interactable = false;
    EndingButtonGroup.blocksRaycasts = false;

    DescriptionText.text = "";
    for(int i=0;i<CurrentEvent.BeginningDescriptions.Count;i++)
    {
      if (i > 0) DescriptionText.text += "<br><br>";
      DescriptionText.text += CurrentEvent.BeginningDescriptions[i];
    }

    IsBeginning = false;
    CurrentEventPhaseIndex = 0;
    if (issuccess)
    {
      if (isleft)
      {
        CurrentSuccessData = CurrentEvent.SelectionDatas[0].SuccessData;
      }
      else
      {
        CurrentSuccessData = CurrentEvent.SelectionDatas[1].SuccessData;
      }
      CurrentEventIllustHolderes = CurrentSuccessData.Illusts;
      CurrentEventDescriptions = CurrentSuccessData.Descriptions;
    }
    else
    {
      if (isleft)
      {
        CurrentFailData = CurrentEvent.SelectionDatas[0].FailData;
      }
      else
      {
        CurrentFailData = CurrentEvent.SelectionDatas[1].FailData;
      }
      CurrentEventIllustHolderes = CurrentFailData.Illusts;
      CurrentEventDescriptions = CurrentFailData.Descriptions;
    }
    CurrentEventPhaseMaxIndex = CurrentEventIllustHolderes.Count;

    UIManager.Instance.AddUIQueue(openui());
  }
  [Space(15)]
  public int CurrentEventPhaseMaxIndex = 0;
  public int CurrentEventPhaseIndex = 0;
  private List<EventIllustHolder> CurrentEventIllustHolderes = null;
  private List<string> CurrentEventDescriptions = null;
  private bool IsBeginning = false;
  public void NextDescription()
  {
    if (UIManager.Instance.IsWorking) return;

    UIManager.Instance.AddUIQueue(displaynextindex(true));
  }
  private IEnumerator openui()
  {
    DefaultGroup.interactable = false;
    StartCoroutine(displaynextindex(true));

    yield return  StartCoroutine(UIManager.Instance.moverect(DefaultRect, LeftPos, CenterPos, OpenTime, UIManager.Instance.UIPanelOpenCurve));

    DefaultGroup.interactable = true;
    yield return null;
  }
  private IEnumerator displaynextindex(bool dir)
  {
    if (IsBeginning)
    {
      if (CurrentEventPhaseIndex == CurrentEventPhaseMaxIndex-1)              //선택지 단계에 도달
      {
        if (CurrentEventPhaseIndex == 0)     //UI 처음 열고 바로 선택지일때
        {
          if (SelectionGroup.gameObject.activeInHierarchy == false) SelectionGroup.gameObject.SetActive(true);
          SelectionGroup.alpha = 0.0f;
          StartCoroutine(setupselections());
          //열기 전에 선택지 세팅

          if (NextButtonGroup.gameObject.activeInHierarchy == true) NextButtonGroup.gameObject.SetActive(false);
          if (RewardButtonGroup.gameObject.activeInHierarchy == true) RewardButtonGroup.gameObject.SetActive(false);
          if (EndingButtonGroup.gameObject.activeInHierarchy == true) EndingButtonGroup.gameObject.SetActive(false);

          Illust.Setup(CurrentEventIllustHolderes[CurrentEventPhaseIndex].CurrentIllust,0.5f);
          NameText.text = CurrentEvent.Name;
          DescriptionText.text += CurrentEventDescriptions[CurrentEventPhaseIndex];
          LayoutRebuilder.ForceRebuildLayoutImmediate(DescriptionText.transform.parent.transform as RectTransform);

        }
        else                                 //다음 버튼 눌러서 선택지에 도달할때
        {
          Illust.Next(CurrentEventIllustHolderes[CurrentEventPhaseIndex].CurrentIllust,FadeTime);
          NextButtonGroup.gameObject.SetActive(false);

          yield return new WaitForSeconds(FadeTime);

          DescriptionText.text += "<br><br>" + CurrentEventDescriptions[CurrentEventPhaseIndex];
          LayoutRebuilder.ForceRebuildLayoutImmediate(DescriptionText.transform.parent.transform as RectTransform);
          yield return StartCoroutine(updatescrollbar());
          SelectionGroup.gameObject.SetActive(true);

          yield return StartCoroutine(setupselections());
        }
      }
      else                                                                 //다음 내용으로 진행
      {
        if (CurrentEventPhaseIndex == 0)     //UI 처음 열고 설명 페이즈일때
        {
          SetNextButtonDisable();
          if (NextButtonGroup.gameObject.activeInHierarchy == false) NextButtonGroup.gameObject.SetActive(true);
          if (SelectionGroup.gameObject.activeInHierarchy == true) SelectionGroup.gameObject.SetActive(false);
          if (RewardButtonGroup.gameObject.activeInHierarchy == true) RewardButtonGroup.gameObject.SetActive(false);
          if (EndingButtonGroup.gameObject.activeInHierarchy == true) EndingButtonGroup.gameObject.SetActive(false);

          Illust.Setup(CurrentEventIllustHolderes[CurrentEventPhaseIndex].CurrentIllust, 0.5f) ;
          NameText.text = CurrentEvent.Name;
          DescriptionText.text += CurrentEventDescriptions[CurrentEventPhaseIndex];
          LayoutRebuilder.ForceRebuildLayoutImmediate(DescriptionText.transform.parent.transform as RectTransform);

          SetNextButtonActive();
        }
        else                                 //다음 버튼 눌러서 다음 내용 전개하기
        {
          SetNextButtonDisable();

          Illust.Next(CurrentEventIllustHolderes[CurrentEventPhaseIndex].CurrentIllust, FadeTime);
          yield return new WaitForSeconds(FadeTime);

          DescriptionText.text += "<br><br>" + CurrentEventDescriptions[CurrentEventPhaseIndex];
          LayoutRebuilder.ForceRebuildLayoutImmediate(DescriptionText.transform.parent.transform as RectTransform);
          yield return StartCoroutine(updatescrollbar());
          SetNextButtonActive();
        }
      }
    }
    else
    {
      if (CurrentEventPhaseIndex == CurrentEventPhaseMaxIndex-1)             //보상 단계에 도달
      {
        if (CurrentEventPhaseIndex == 0)     //선택지 선택 후 바로 보상일때         (선택지 애니메이션은 완료)
        {
          if(SelectionGroup.gameObject.activeInHierarchy==true) SelectionGroup.gameObject.SetActive(false);
          if (RewardButtonGroup.gameObject.activeInHierarchy == true) RewardButtonGroup.gameObject.SetActive(false);
          if (EndingButtonGroup.gameObject.activeInHierarchy == true) EndingButtonGroup.gameObject.SetActive(false);

          SetNextButtonDisable();

          Illust.Next(CurrentEventIllustHolderes[CurrentEventPhaseIndex].CurrentIllust, FadeTime);
          yield return new WaitForSeconds(FadeTime);

          DescriptionText.text +="<br><br>"+ CurrentEventDescriptions[CurrentEventPhaseIndex];
          LayoutRebuilder.ForceRebuildLayoutImmediate(DescriptionText.transform.parent.transform as RectTransform);
          yield return StartCoroutine(updatescrollbar());
          SetNextButtonActive();

          if (CurrentSuccessData != null)
          {
            SetRewardButton();
            
            if (CurrentSuccessData.Reward_Type != RewardTypeEnum.None)
            {
              RewardButtonGroup.alpha = 0.0f;
              if (RewardButtonGroup.gameObject.activeInHierarchy == false) RewardButtonGroup.gameObject.SetActive(true);
              StartCoroutine(UIManager.Instance.ChangeAlpha(RewardButtonGroup, 1.0f, ButtonFadeinTime));
            }

            if (CurrentEvent.EndingID != "")
            {
              if (EndingButtonGroup.gameObject.activeInHierarchy == false) EndingButtonGroup.gameObject.SetActive(true);
              LayoutRebuilder.ForceRebuildLayoutImmediate(EndingButtonGroup.transform as RectTransform);
              StartCoroutine(UIManager.Instance.ChangeAlpha(EndingButtonGroup,1.0f,ButtonFadeinTime));
            }
          }
          else if (CurrentFailData != null)
          {
            SetPenalty();
            
            switch (CurrentFailData.Penelty_target)
            {
              case PenaltyTarget.None:
                break;
              case PenaltyTarget.EXP:
                break;
              case PenaltyTarget.Status:
                switch (CurrentFailData.StatusType)
                {
                  case StatusTypeEnum.HP:
                    DescriptionText.text += WNCText.SetSize(30,$"<br><br>{GameManager.Instance.GetTextData(StatusTypeEnum.HP,2)} {-1*PenaltyValue}");
                    break;
                  case StatusTypeEnum.Sanity:
                    DescriptionText.text +=   WNCText.SetSize(30, $"<br><br>{GameManager.Instance.GetTextData(StatusTypeEnum.Sanity, 2)} {-1*PenaltyValue}");
                    break;
                  case StatusTypeEnum.Gold:
                    DescriptionText.text += WNCText.SetSize(30, $"<br><br>{GameManager.Instance.GetTextData(StatusTypeEnum.Gold, 2)} {-1*-PenaltyValue}");
                    break;
                }
                break;
            }
          }
        }
        else                                 //다음 버튼 눌러서 보상에 도달할때
        {
          NextButtonGroup.gameObject.SetActive(false);

          Illust.Next(CurrentEventIllustHolderes[CurrentEventPhaseIndex].CurrentIllust, FadeTime);
          yield return new WaitForSeconds(FadeTime);

          DescriptionText.text += "<br><br>" + CurrentEventDescriptions[CurrentEventPhaseIndex];

          if (CurrentSuccessData != null)
          {
            SetRewardButton();

            if (CurrentSuccessData.Reward_Type != RewardTypeEnum.None)
            {
              RewardButtonGroup.alpha = 0.0f;
              if (RewardButtonGroup.gameObject.activeInHierarchy == false) RewardButtonGroup.gameObject.SetActive(true);
              StartCoroutine(UIManager.Instance.ChangeAlpha(RewardButtonGroup, 1.0f, ButtonFadeinTime));
            }

            if (CurrentEvent.EndingID != "")
            {
              if (EndingButtonGroup.gameObject.activeInHierarchy == false) EndingButtonGroup.gameObject.SetActive(true);
              LayoutRebuilder.ForceRebuildLayoutImmediate(EndingButtonGroup.transform as RectTransform);
              StartCoroutine(UIManager.Instance.ChangeAlpha(EndingButtonGroup, 1.0f, ButtonFadeinTime));
            }
          }
          else if (CurrentFailData != null)
          {
            switch (CurrentFailData.Penelty_target)
            {
              case PenaltyTarget.None:
                break;
              case PenaltyTarget.EXP:
                break;
              case PenaltyTarget.Status:
                switch (CurrentFailData.StatusType)
                {
                  case StatusTypeEnum.HP:
                    DescriptionText.text += "<br>" +
                     WNCText.SetSize(30, $"<br><br>{GameManager.Instance.GetTextData(StatusTypeEnum.HP, 2)} {-1*PenaltyValue}");
                    break;
                  case StatusTypeEnum.Sanity:
                    DescriptionText.text += "<br>" + CurrentEventDescriptions[CurrentEventPhaseIndex] +
                     WNCText.SetSize(30, $"<br><br>{GameManager.Instance.GetTextData(StatusTypeEnum.HP, 2)} {-1*PenaltyValue}");
                    break;
                  case StatusTypeEnum.Gold:
                    DescriptionText.text += "<br>" + CurrentEventDescriptions[CurrentEventPhaseIndex] +
                     WNCText.SetSize(30, $"<br><br>{GameManager.Instance.GetTextData(StatusTypeEnum.HP, 2)} {-1*PenaltyValue}");
                    break;
                }
                break;
            }
          }
          LayoutRebuilder.ForceRebuildLayoutImmediate(DescriptionText.transform.parent.transform as RectTransform);

          yield return StartCoroutine(updatescrollbar());
        }

        OpenReturnButton();
      }
      else                                                                 //다음 내용으로 진행
      {
        if (CurrentEventPhaseIndex == 0)     //선택지 선택 후 새로 설명으로 넘어갈때 
        {
          
          if (SelectionGroup.gameObject.activeInHierarchy == true) SelectionGroup.gameObject.SetActive(false);
          if (RewardButtonGroup.gameObject.activeInHierarchy == true) RewardButtonGroup.gameObject.SetActive(false);
          if (EndingButtonGroup.gameObject.activeInHierarchy == true) EndingButtonGroup.gameObject.SetActive(false);

          if (NextButtonGroup.gameObject.activeInHierarchy == false) NextButtonGroup.gameObject.SetActive(true);
          SetNextButtonDisable();
          Illust.Next(CurrentEventIllustHolderes[CurrentEventPhaseIndex].CurrentIllust, FadeTime);

          yield return new WaitForSeconds(FadeTime);

          DescriptionText.text +="<br><br>"+ CurrentEventDescriptions[CurrentEventPhaseIndex];
          LayoutRebuilder.ForceRebuildLayoutImmediate(DescriptionText.transform.parent.transform as RectTransform);
          yield return StartCoroutine(updatescrollbar());
          SetNextButtonActive();
        }
        else                                 //다음 버튼 눌러서 다음 내용 전개하기
        {
          SetNextButtonDisable();
          Illust.Next(CurrentEventIllustHolderes[CurrentEventPhaseIndex].CurrentIllust, FadeTime);

          yield return new WaitForSeconds(FadeTime);

          DescriptionText.text += "<br><br>" + CurrentEventDescriptions[CurrentEventPhaseIndex];
          LayoutRebuilder.ForceRebuildLayoutImmediate(DescriptionText.transform.parent.transform as RectTransform);
          yield return StartCoroutine(updatescrollbar());
          SetNextButtonActive();
        }
      }
    }

    CurrentEventPhaseIndex++;
  }
  private IEnumerator setupselections()
  {
    SelectionGroup.alpha = 0.0f;

    NextButtonGroup.alpha = DisableAlpha;
    NextButtonGroup.interactable = false;
    if (SelectionGroup.gameObject.activeInHierarchy == false) SelectionGroup.gameObject.SetActive(true);
    if (RewardButtonGroup.gameObject.activeInHierarchy == true) RewardButtonGroup.gameObject.SetActive(false);


    switch (CurrentEvent.Selection_type)
    {
      case SelectionTypeEnum.Single:
        Selection_A.Setup(CurrentEvent.SelectionDatas[0]);
        if (Selection_B.gameObject.activeInHierarchy==true) Selection_B.gameObject.SetActive(false);
        break;
      case SelectionTypeEnum.Body:
        Selection_A.Setup(CurrentEvent.SelectionDatas[0]);
        if (Selection_B.gameObject.activeInHierarchy==false) Selection_B.gameObject.SetActive(true);
        Selection_B.Setup(CurrentEvent.SelectionDatas[1]);
        break;
      case SelectionTypeEnum.Head:
        Selection_A.Setup(CurrentEvent.SelectionDatas[0]);
        if (Selection_B.gameObject.activeInHierarchy == false) Selection_B.gameObject.SetActive(true);
        Selection_B.Setup(CurrentEvent.SelectionDatas[1]);
        break;
    }

    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(SelectionGroup,1.0f,FadeTime));
  }
  private UI_Selection CurrentUISelection = null;
  /// <summary>
  /// 선택지 클릭했을때 선택지 스크립트에서 호출
  /// </summary>
  /// <param name="_selection"></param>
  public void SelectSelection(UI_Selection _selection)
  {
    if (_selection.MyTendencyType != TendencyTypeEnum.None)
    {
      GetOppositeSelection(_selection).DeActive();
    }
    //다른거 사라지게 만들고
    UIManager.Instance.AddUIQueue(selectionanimation(_selection));
    //성공, 실패 검사 실행 
  }
  private IEnumerator selectionanimation(UI_Selection _selection)
  {
    CurrentUISelection = _selection;

    SelectionData _selectiondata = _selection.MySelectionData;
    int _payvalue = 0;
    int _currentvalue = 0, _checkvalue = 0;    //기술 체크에만 사용
    int _percentvalue = UnityEngine.Random.Range(1,101);
    int _requirevalue = 0;                   //성공 확률(골드 혹은 기술 체크) 
    bool _issuccess = false;  
                                               
    switch (_selectiondata.ThisSelectionType)
    {
      case SelectionTargetType.None:
        _issuccess = true;
        break;
      case SelectionTargetType.Pay:
        if (_selectiondata.SelectionPayTarget.Equals(StatusTypeEnum.HP))
        {
          _payvalue = GameManager.Instance.MyGameData.PayHPValue;
       //   yield return StartCoroutine(UIManager.Instance.SetIconEffect(true, StatusTypeEnum.HP, _selection.PayIcon.transform as RectTransform));
          yield return StartCoroutine(payanimation(_selection.PayIcon, StatusTypeEnum.HP, _payvalue, 0, _selection.PayInfo));

          _issuccess = true;
          GameManager.Instance.MyGameData.HP -= GameManager.Instance.MyGameData.PayHPValue;
        }
        else if (_selectiondata.SelectionPayTarget.Equals(StatusTypeEnum.Sanity))
        {
          _payvalue = GameManager.Instance.MyGameData.PaySanityValue;
       //   yield return StartCoroutine(UIManager.Instance.SetIconEffect(true, StatusTypeEnum.Sanity, _selection.PayIcon.transform as RectTransform));
          yield return StartCoroutine(payanimation(_selection.PayIcon, StatusTypeEnum.Sanity, _payvalue, 0, _selection.PayInfo));

          _issuccess = true;//체력,정신력 지불의 경우 남은 값과 상관 없이 일단 성공으로만 친다
          GameManager.Instance.MyGameData.Sanity -= GameManager.Instance.MyGameData.PaySanityValue;
        }
        else        //돈 지불일 경우 돈 적을 때 실행하는 뭔가 있어야 함
        {
          _payvalue = GameManager.Instance.MyGameData.PayGoldValue;
          int _goldsuccesspercent = GameManager.Instance.MyGameData.Gold >= _payvalue ? 100 : GameManager.Instance.MyGameData.CheckPercent_money(_payvalue);

          if (GameManager.Instance.MyGameData.Gold >= _payvalue)
          {
            //    yield return StartCoroutine(UIManager.Instance.SetIconEffect(true, StatusTypeEnum.Gold, _selection.PayIcon.transform as RectTransform));
            _issuccess = true;
            yield return StartCoroutine(payanimation(_selection.PayIcon, StatusTypeEnum.Sanity, _payvalue, 0, _selection.PayInfo));
          }
          else
          {
            if (_percentvalue > _goldsuccesspercent)
            {
              int _elsevalue = GameManager.Instance.MyGameData.PayGoldValue - GameManager.Instance.MyGameData.Gold;

           //   StartCoroutine(UIManager.Instance.SetIconEffect(true, StatusTypeEnum.Gold, _selection.PayIcon.transform as RectTransform));
            //  StartCoroutine(UIManager.Instance.SetIconEffect(true, StatusTypeEnum.Sanity, _selection.PayIcon.transform as RectTransform));
              yield return StartCoroutine(payanimation(_selection.PayIcon, StatusTypeEnum.Sanity, _payvalue, 0, _selection.PayInfo));

              _issuccess = true;
              GameManager.Instance.MyGameData.Gold = 0;
              GameManager.Instance.MyGameData.Sanity -= (int)(_elsevalue * ConstValues.GoldSanityPayAmplifiedValue);
              Debug.Log("정당한 값을 지불한 레후~");
            }//돈이 부족해 성공한 경우
            else
            {
              yield return StartCoroutine(payanimation(_selection.PayIcon, StatusTypeEnum.Sanity, _payvalue,_payvalue - GameManager.Instance.MyGameData.Gold, _selection.PayInfo));

              _issuccess = false;
            }//돈이 부족해 실패한 경우
          }
        }
        break;
      case SelectionTargetType.Check_Single: //기술(단수) 선택지면 확률 검사
        _currentvalue = GameManager.Instance.MyGameData.GetSkill(_selectiondata.SelectionCheckSkill[0]).Level;
        _checkvalue = GameManager.Instance.MyGameData.CheckSkillSingleValue;
        _requirevalue = GameManager.Instance.MyGameData.CheckPercent_themeorskill(_currentvalue, _checkvalue);
        if (_percentvalue >= _requirevalue)
        {
          _issuccess = true;
        }
        else
        {
          _issuccess = false;
        }

        yield return StartCoroutine(checkanimation(_selection.SkillIcon_A,Mathf.Clamp(_percentvalue / (float)_requirevalue, 0.0f, 1.0f)));
        break;
      case SelectionTargetType.Check_Multy: //기술(복수) 선택지면 확률 검사
        _currentvalue = GameManager.Instance.MyGameData.GetSkill(_selectiondata.SelectionCheckSkill[0]).Level +
          GameManager.Instance.MyGameData.GetSkill(_selectiondata.SelectionCheckSkill[1]).Level;
        _checkvalue = GameManager.Instance.MyGameData.CheckSkillMultyValue;
        _requirevalue = GameManager.Instance.MyGameData.CheckPercent_themeorskill(_currentvalue, _checkvalue);
        if (_percentvalue >= _requirevalue)
        {
          _issuccess = true;
        }
        else
        {
          _issuccess = false;
        }
        yield return StartCoroutine(checkanimation(_selection.SkillIcon_A, _selection.SkillIcon_B,Mathf.Clamp(_percentvalue / (float)_requirevalue, 0.0f, 1.0f)));
        break;
    }

    if (_issuccess) //성공하면 성공
    {
      Debug.Log("성공함");
      SetSuccess(CurrentEvent.SelectionDatas[_selection.Index].SuccessData);
      GameManager.Instance.SuccessCurrentEvent(_selection.MyTendencyType, _selection.Index);
    }
    else            //실패하면 실패
    {
      Debug.Log("실패함");
      SetFail(CurrentEvent.SelectionDatas[_selection.Index].FailData);
      GameManager.Instance.FailCurrentEvent(_selection.MyTendencyType, _selection.Index);
    }
    _selection.DeActive();

    GameManager.Instance.SaveData();

    yield return null;
  }//선택한 선택지 성공 여부를 계산하고 애니메이션을 실행시키는 코루틴
  //이 코루틴에서 SetSuccess 아니면 SetFail로 바로 넘어감
  [SerializeField] private float SelectionEffectTime_check = 4.0f;
  [SerializeField] private float SelectionEffectTime_pay = 1.5f;
  [SerializeField] private AnimationCurve SelectionCheckCurve = null;
  private IEnumerator payanimation(Image image, StatusTypeEnum status,int payvalue, int targetvalue, TextMeshProUGUI tmp)
  {
    float _time = 0.0f;
    string _str = "";
    while(_time< SelectionEffectTime_pay * (1.0f-targetvalue/(float)payvalue))
    {
      image.fillAmount = 1.0f - _time / SelectionEffectTime_pay;
      switch (status)
      {
        case StatusTypeEnum.HP:_str = WNCText.GetHPColor((int)Mathf.Lerp(payvalue, targetvalue, _time / SelectionEffectTime_pay));break;
        case StatusTypeEnum.Sanity: _str = WNCText.GetSanityColor((int)Mathf.Lerp(payvalue, targetvalue, _time / SelectionEffectTime_pay)); break;
        case StatusTypeEnum.Gold: _str = WNCText.GetGoldColor((int)Mathf.Lerp(payvalue, targetvalue, _time / SelectionEffectTime_pay)); break;
      }
      tmp.text = _str;
      _time += Time.deltaTime;
      yield return null;
    }
    switch (status)
    {
      case StatusTypeEnum.HP: _str = WNCText.GetHPColor(targetvalue); break;
      case StatusTypeEnum.Sanity: _str = WNCText.GetSanityColor(targetvalue); break;
      case StatusTypeEnum.Gold: _str = WNCText.GetGoldColor(targetvalue); break;
    }
    tmp.text= _str;

    yield return new WaitForSeconds(0.3f);
  }
  private IEnumerator checkanimation(Image image,float successvalue)
  {
    Debug.Log(successvalue);
    float _time = 0.0f;
    while(_time< SelectionEffectTime_check * successvalue)
    {
      image.fillAmount=Mathf.Lerp(1.0f,0.0f, SelectionCheckCurve.Evaluate(_time/ SelectionEffectTime_check));
      _time += Time.deltaTime;yield return null;
    }
    // image.fillAmount = SelectionCheckCurve.Evaluate(_time / SelectionEffectTime);

    yield return new WaitForSeconds(0.3f);
  }
  private IEnumerator checkanimation(Image image_L,Image image_R,float successvalue)
  {
    Debug.Log(successvalue);
    float _time = 0.0f;
    while (_time < SelectionEffectTime_check * successvalue)
    {
      if (_time < SelectionEffectTime_check / 2)
        image_L.fillAmount = Mathf.Lerp(1.0f, 0.0f, SelectionCheckCurve.Evaluate(_time / (SelectionEffectTime_check / 2)));
      else
        image_R.fillAmount = Mathf.Lerp(1.0f, 0.0f, SelectionCheckCurve.Evaluate((_time-SelectionEffectTime_check/2) / (SelectionEffectTime_check / 2)));

      _time += Time.deltaTime;yield return null;
    }

    yield return new WaitForSeconds(0.3f);
    //  image_L.fillAmount = successvalue > 0.5f ? 0.0f : 1.0f - successvalue * 2.0f;
    //  image_R.fillAmount = successvalue > 0.5f ? 1.0f - (successvalue - 0.5f) * 2.0f : 1.0f;
  }
  private SuccessData CurrentSuccessData = null;
  public bool RemainReward = false;
  [SerializeField] private float ResultEffectTime = 2.0f;
  public void SetSuccess(SuccessData _success)
  {
    CurrentSuccessData = _success;

    IsBeginning = false;
      CurrentEventPhaseMaxIndex = CurrentSuccessData.Descriptions.Count;
      CurrentEventPhaseIndex = 0;
      CurrentEventIllustHolderes = CurrentSuccessData.Illusts;
      CurrentEventDescriptions = CurrentSuccessData.Descriptions;

    Color _color = new Color();
    switch (_success.Tendencytype)
    {
      case TendencyTypeEnum.None:_color = IdleColor;break;
      case TendencyTypeEnum.Body:_color = _success.Index == 0 ? RationalColor : PhysicalColor;break;
      case TendencyTypeEnum.Head:_color = _success.Index == 0 ? MentalColor : MaterialColor;break;
    }
    IllustEffect_Image.color = _color;
    StartCoroutine (UIManager.Instance.ChangeAlpha(IllustEffect_Group, 0.0f, ResultEffectTime));

      UIManager.Instance.AddUIQueue(displaynextindex(true));

    if (CurrentEvent.EndingID != "")
    {
      EndingButtonText.text = GameManager.Instance.ImageHolder.GetEndingIllust(CurrentEvent.EndingID).Name + "<br>" +
        GameManager.Instance.GetTextData("Ending_Description");
    }
    //연계 이벤트고, 엔딩 설정이 돼 있는 상태에서 성공할 경우 엔딩 다이어로그 전개
  }//성공할 경우 보상 탭을 세팅하고 텍스트를 성공 설명으로 교체, 퀘스트 이벤트일 경우 진행도 ++
  private void SetRewardButton()
  {
    RewardButtonGroup.alpha = 0.0f;
    if (RewardButtonGroup.gameObject.activeInHierarchy == false) RewardButtonGroup.gameObject.SetActive(true);
    if (NextButtonGroup.gameObject.activeInHierarchy == true) NextButtonGroup.gameObject.SetActive(false);

    Reward_Highlight.RemoveAllCall();
    RemainReward = CurrentSuccessData.Reward_Type == RewardTypeEnum.None ? false : true;
    Sprite _icon = null;
    string _description = "";
    switch (CurrentSuccessData.Reward_Type)
    {
      case RewardTypeEnum.Status:
        switch (CurrentSuccessData.Reward_StatusType)
        {
          case StatusTypeEnum.HP:
            _icon = GameManager.Instance.ImageHolder.HPIcon;
            _description = $"+{WNCText.GetHPColor(GameManager.Instance.MyGameData.RewardHPValue)}";
            Reward_Highlight.SetInfo(HighlightEffectEnum.HP, GameManager.Instance.MyGameData.RewardHPValue);
            break;
          case StatusTypeEnum.Sanity:
            _icon = GameManager.Instance.ImageHolder.SanityIcon;
            _description = $"+{WNCText.GetSanityColor(GameManager.Instance.MyGameData.RewardSanityValue)}";
            Reward_Highlight.SetInfo(HighlightEffectEnum.Sanity, GameManager.Instance.MyGameData.RewardSanityValue);
            break;
          case StatusTypeEnum.Gold:
            _icon = GameManager.Instance.ImageHolder.GoldIcon;
            _description = $"+{WNCText.GetGoldColor(GameManager.Instance.MyGameData.RewardGoldValue)}";
            Reward_Highlight.SetInfo(HighlightEffectEnum.Gold, GameManager.Instance.MyGameData.RewardGoldValue);
            break;
        }
        break;
      case RewardTypeEnum.Experience:
        _icon = GameManager.Instance.ImageHolder.UnknownExpRewardIcon;
        //   _name = GameManager.Instance.GetTextData("EXP_NAME");
        _description = GameManager.Instance.ExpDic[CurrentSuccessData.Reward_EXPID].Name;
        Reward_Highlight.SetInfo(HighlightEffectEnum.Exp);
        break;
      case RewardTypeEnum.Skill:
        _icon = GameManager.Instance.ImageHolder.GetSkillIcon(CurrentSuccessData.Reward_SkillType, false);
        _description = $"{GameManager.Instance.GetTextData(CurrentSuccessData.Reward_SkillType, 0)} +1";
        Reward_Highlight.SetInfo(new List<SkillTypeEnum> { CurrentSuccessData.Reward_SkillType });
        break;
    }
    RewardIcon.sprite = _icon;
    RewardDescription.text = _description;

    StartCoroutine(UIManager.Instance.ChangeAlpha(RewardButtonGroup, 1.0f, 0.3f));
  }
  private FailData CurrentFailData = null;
  public UnityEngine.UI.Button CurrentReturnButton = null;
  public void SetFail(FailData _fail)
  {
    CurrentFailData = _fail;

    IsBeginning = false;
    CurrentEventPhaseMaxIndex = CurrentFailData.Descriptions.Count;
    CurrentEventPhaseIndex = 0;
    CurrentEventIllustHolderes = CurrentFailData.Illusts;
    CurrentEventDescriptions = CurrentFailData.Descriptions;

    IllustEffect_Image.color = FailColor;
    StartCoroutine((UIManager.Instance.ChangeAlpha(IllustEffect_Group, 0.0f, ResultEffectTime)));

    UIManager.Instance.AddUIQueue(displaynextindex(true));
  }//실패할 경우 패널티를 부과하고 텍스트를 실패 설명으로 교체

  public void OpenReturnButton()
  {
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
  /// <summary>
  /// true:왼쪽으로 false:오른쪽으로
  /// </summary>
  /// <param name="dir"></param>
  public void CloseUI(bool dir)
  {
    IsOpen = false;
    UIManager.Instance.AddUIQueue(closeui(dir));
  }
  private IEnumerator closeui(bool dir)
  {
    CurrentSuccessData = null;
    CurrentFailData = null;

    UIManager.Instance.OffBackground();
    StartCoroutine(UIManager.Instance.ChangeAlpha(RewardButtonGroup, 0.0f, 0.3f));

    if (dir == true)
    {
      yield return StartCoroutine(UIManager.Instance.moverect(DefaultRect, DefaultRect.anchoredPosition, LeftPos, CloseTime, UIManager.Instance.UIPanelCLoseCurve));
    }
    else
    {
      yield return StartCoroutine(UIManager.Instance.moverect(DefaultRect, DefaultRect.anchoredPosition, RightPos, CloseTime, UIManager.Instance.UIPanelCLoseCurve));
    }

  }
  public void GetEnding()
  {
    GameManager.Instance.SubEnding(GameManager.Instance.ImageHolder.GetEndingIllust(GameManager.Instance.MyGameData.CurrentEvent.EndingID));
  }
  public void GetReward()
  {
    if (UIManager.Instance.IsWorking) return;
    UIManager.Instance.AddUIQueue(getreward());
  }
  private IEnumerator getreward()
  {
    Reward_Highlight.Interactive = false;

    if (CurrentSuccessData != null)
    {
      if (CurrentSuccessData.Reward_Type == RewardTypeEnum.Experience)
      {
        RewardExpUI.OpenUI_RewardExp(GameManager.Instance.ExpDic[CurrentSuccessData.Reward_EXPID]);
        yield break;
      }
      else
      {
        switch (CurrentSuccessData.Reward_Type)
        {
          case RewardTypeEnum.Status:
            switch (CurrentSuccessData.Reward_StatusType)
            {
              case StatusTypeEnum.HP:
             //   yield return StartCoroutine(UIManager.Instance.SetIconEffect(false, StatusTypeEnum.HP, RewardIcon.transform as RectTransform));
                GameManager.Instance.MyGameData.HP += GameManager.Instance.MyGameData.RewardHPValue;
                break;
              case StatusTypeEnum.Sanity:
              //  yield return StartCoroutine(UIManager.Instance.SetIconEffect(false, StatusTypeEnum.Sanity, RewardIcon.transform as RectTransform));
                GameManager.Instance.MyGameData.Sanity += GameManager.Instance.MyGameData.RewardSanityValue;
                break;
              case StatusTypeEnum.Gold:
              //  yield return StartCoroutine(UIManager.Instance.SetIconEffect(false, StatusTypeEnum.Gold, RewardIcon.transform as RectTransform));
                GameManager.Instance.MyGameData.Gold += GameManager.Instance.MyGameData.RewardGoldValue;
                break;
            }
            break;
          case RewardTypeEnum.Skill:
            yield return StartCoroutine(UIManager.Instance.SetIconEffect(false,CurrentSuccessData.Reward_SkillType,RewardIcon.transform as RectTransform));
            GameManager.Instance.MyGameData.GetSkill(CurrentSuccessData.Reward_SkillType).LevelByDefault++;
            break;
        }

        StartCoroutine(UIManager.Instance.ChangeAlpha(RewardButtonGroup, 0.0f, 0.6f));
        RemainReward = false;
      }
    }
    else if (CurrentFailData != null)
    {
      RewardExpUI.OpenUI_Penalty(GameManager.Instance.ExpDic[CurrentFailData.ExpID]);
    }
  }
  private int PenaltyValue = 0;
  public void ExpAcquired()
  {
    RemainReward = false;
    StartCoroutine(UIManager.Instance.ChangeAlpha(RewardButtonGroup, 0.0f, 0.6f));
  }
  public void SetPenalty()
  {
    switch (CurrentFailData.Penelty_target)
    {
      case PenaltyTarget.None:
        break;
      case PenaltyTarget.Status:
        switch (CurrentFailData.StatusType)
        {
          case StatusTypeEnum.HP:
            GameManager.Instance.MyGameData.HP -= GameManager.Instance.MyGameData.FailHPValue;
            PenaltyValue = GameManager.Instance.MyGameData.FailHPValue;
            break;
          case StatusTypeEnum.Sanity:
            GameManager.Instance.MyGameData.Sanity -= GameManager.Instance.MyGameData.FailSanityValue;
            PenaltyValue = GameManager.Instance.MyGameData.FailSanityValue;
            break;
          case StatusTypeEnum.Gold:
            if (GameManager.Instance.MyGameData.Gold >= GameManager.Instance.MyGameData.FailGoldValue)
            {
              GameManager.Instance.MyGameData.Gold -= GameManager.Instance.MyGameData.FailGoldValue;
              PenaltyValue = GameManager.Instance.MyGameData.FailGoldValue;
            }
            else
            {
              PenaltyValue = GameManager.Instance.MyGameData.Gold;
              GameManager.Instance.MyGameData.Gold = 0;
            }
            break;
        }
        break;
    }
  }//실패할 경우 패널티 부과하는 연출
}
