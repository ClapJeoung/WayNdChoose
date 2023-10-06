using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UI_dialogue : UI_default
{
  private float DialogueUIMoveTime = 0.8f;

  [SerializeField] private TextMeshProUGUI NameText = null;
  [SerializeField] private CanvasGroup IllustImageGroup = null;
  [SerializeField] private Image IllustImage = null;
  private float FadeOutTime = 0.7f;
  private float FadeInTime = 0.9f;
  private float FadeWaitTime = 0.3f;
  private float ButtonFadeinTime = 0.4f;
  [SerializeField] private Image EventIcon = null;
  private GameObject EventIconHolder { get { return EventIcon.transform.parent.gameObject; } }
  [Space(10)]
  [SerializeField] private TextMeshProUGUI DescriptionText = null;
  [SerializeField] private CanvasGroup DescriptionTextGroup = null;
  [Space(10)]
  [SerializeField] private CanvasGroup NextButtonGroup = null;
  [SerializeField] private Button NextButton = null;
  [SerializeField] private TextMeshProUGUI NextButtonText = null;
  [Space(10)]
  [SerializeField] private CanvasGroup SelectionGroup = null;
  [SerializeField] private CanvasGroup SelectionCenterImgGroup = null;
  [SerializeField] private UI_Selection Selection_A = null;
  [SerializeField] private UI_Selection Selection_B = null;
//  [SerializeField] private ParticleSystem SuccessParticle = null;
//  [SerializeField] private ParticleSystem FailParticle = null;
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
  [SerializeField] private UI_RewardExp RewardExpUI = null;
  private EventDataDefulat CurrentEvent
  {
    get { return GameManager.Instance.MyGameData.CurrentEvent; }
  }
  private RectTransform IllustRect { get {return GetPanelRect("illust").Rect; } }
  private Vector2 IllustOpenPos { get { return GetPanelRect("illust").InsidePos; } }
  private Vector2 IllustClosePos { get { return GetPanelRect("illust").OutisdePos; } }
  private RectTransform DescriptionRect { get { return GetPanelRect("description").Rect; } }
  private Vector2 DescriptionOpenPos { get { return GetPanelRect("description").InsidePos; } }
  private Vector2 DescriptionClosePos { get { return GetPanelRect("description").OutisdePos; } }
  private RectTransform NameRect { get { return GetPanelRect("name").Rect; } }
  private Vector2 NameOpenpos { get { return GetPanelRect("name").InsidePos; } }
  private Vector2 NameClosePos { get { return GetPanelRect("name").OutisdePos; } }

  private UI_Selection GetOppositeSelection(UI_Selection _selection)
  {
    if (_selection == Selection_A) return Selection_B;
    return Selection_A;
  }
  public void OpenUI()
  {
    IsOpen = true;
    if (NextButtonText.text == "next") NextButtonText.text = GameManager.Instance.GetTextData("NEXT_TEXT");
    if (Reward_clicktoget.text == "getreward") Reward_clicktoget.text = GameManager.Instance.GetTextData("GETREWARD");

    if (DefaultRect.anchoredPosition != Vector2.zero) DefaultRect.anchoredPosition = Vector2.zero;
    UIManager.Instance.UpdateBackground(CurrentEvent.EnvironmentType);

    CurrentEventPhaseIndex = 0;
    CurrentEventIllustHolderes = CurrentEvent.BeginningIllusts;
    CurrentEventDescriptions = CurrentEvent.BeginningDescriptions;
    CurrentEventPhaseMaxIndex=CurrentEventIllustHolderes.Count;
    IsBeginning = true;

    UIManager.Instance.AddUIQueue(displaynextindex());
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

    UIManager.Instance.AddUIQueue(displaynextindex());
  }
  private IEnumerator displaynextindex()
  {
    if (IsBeginning)
    {
      if (CurrentEventPhaseIndex == CurrentEventPhaseMaxIndex-1)              //선택지 단계에 도달
      {
        if (CurrentEventPhaseIndex == 0)     //UI 처음 열고 바로 선택지일때
        {
          if (CurrentEvent.GetType() == typeof(EventData))
          {
            if (EventIconHolder.activeInHierarchy == true) EventIconHolder.SetActive(false);
          }
          else if (CurrentEvent.GetType() == typeof(FollowEventData))
          {
            if (EventIconHolder.activeInHierarchy == true) EventIconHolder.SetActive(false);
          }
          else if (CurrentEvent.GetType() == typeof(QuestEventData_Wolf))
          {
            if (EventIconHolder.activeInHierarchy == false) EventIconHolder.SetActive(true);
             EventIcon.sprite = GameManager.Instance.ImageHolder.QuestIcon_Cult;
          }

          SelectionGroup.alpha = 0.0f;
          NextButtonGroup.alpha = 0.0f;
          if (NextButton.gameObject.activeInHierarchy == true) NextButton.gameObject.SetActive(false);
          if (SelectionGroup.gameObject.activeInHierarchy == false) SelectionGroup.gameObject.SetActive(true);
          if (RewardButtonGroup.gameObject.activeInHierarchy == true) RewardButtonGroup.gameObject.SetActive(false);
          StartCoroutine(setupselections());
          //열기 전에 선택지 세팅

          IllustImageGroup.alpha = 1.0f;
          IllustImage.sprite = CurrentEventIllustHolderes[CurrentEventPhaseIndex].CurrentIllust;
          NameText.text = CurrentEvent.Name;
          DescriptionTextGroup.alpha = 1.0f;
          DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex];
          LayoutRebuilder.ForceRebuildLayoutImmediate(DescriptionText.transform as RectTransform);
          LayoutRebuilder.ForceRebuildLayoutImmediate(NameText.transform.parent.transform as RectTransform);

          StartCoroutine(UIManager.Instance.moverect(NameRect, NameOpenpos, NameClosePos, DialogueUIMoveTime, true));
          yield return new WaitForSeconds(FadeWaitTime);
          StartCoroutine(UIManager.Instance.moverect(IllustRect, IllustClosePos, IllustOpenPos, DialogueUIMoveTime, true));
          yield return new WaitForSeconds(FadeWaitTime);
          //일러스트+이름 세팅해두고  이동

          yield return StartCoroutine(UIManager.Instance.moverect(DescriptionRect, DescriptionClosePos, DescriptionOpenPos, DialogueUIMoveTime, true));
        }
        else                                 //다음 버튼 눌러서 선택지에 도달할때
        {
          NextButton.interactable = false;
          StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 0.0f, FadeOutTime));
          StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 0.0f, FadeOutTime));
          yield return StartCoroutine(UIManager.Instance.ChangeAlpha(NextButtonGroup,0.0f,FadeOutTime));
          yield return new WaitForSeconds(FadeWaitTime);
          NextButton.gameObject.SetActive(false);
          IllustImage.sprite = CurrentEventIllustHolderes[CurrentEventPhaseIndex].CurrentIllust;
          DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex];
          LayoutRebuilder.ForceRebuildLayoutImmediate(DescriptionText.transform as RectTransform);
          SelectionGroup.gameObject.SetActive(true);

          StartCoroutine(setupselections());
          StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 1.0f, FadeInTime));
          yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 1.0f, FadeInTime));
        }
      }
      else                                                                 //다음 내용으로 진행
      {
        if (CurrentEventPhaseIndex == 0)     //UI 처음 열고 설명 페이즈일때
        {
          if (CurrentEvent.GetType() == typeof(EventData))
          {
            if (EventIconHolder.activeInHierarchy == true) EventIconHolder.SetActive(false);
          }
          else if (CurrentEvent.GetType() == typeof(FollowEventData))
          {
            if (EventIconHolder.activeInHierarchy == true) EventIconHolder.SetActive(false);
          }
          else if (CurrentEvent.GetType() == typeof(QuestEventData_Wolf))
          {
            if (EventIconHolder.activeInHierarchy == false) EventIconHolder.SetActive(true);
            EventIcon.sprite = GameManager.Instance.ImageHolder.QuestIcon_Cult;
          }

          NextButtonGroup.alpha = 1.0f;
          NextButtonGroup.interactable = true;
          NextButtonGroup.blocksRaycasts = true;
          NextButton.interactable = false;
          if (NextButton.gameObject.activeInHierarchy == false) NextButton.gameObject.SetActive(true);
          if (SelectionGroup.gameObject.activeInHierarchy == true) SelectionGroup.gameObject.SetActive(false);
          if (RewardButtonGroup.gameObject.activeInHierarchy == true) RewardButtonGroup.gameObject.SetActive(false);

          IllustImageGroup.alpha = 1.0f;
          IllustImage.sprite = CurrentEventIllustHolderes[CurrentEventPhaseIndex].CurrentIllust;
          LayoutRebuilder.ForceRebuildLayoutImmediate(NameText.transform as RectTransform);
          NameText.text = CurrentEvent.Name;
          LayoutRebuilder.ForceRebuildLayoutImmediate(NameText.transform.parent.transform as RectTransform);
          StartCoroutine(UIManager.Instance.moverect(NameRect, NameOpenpos, NameClosePos, DialogueUIMoveTime, true));
          yield return new WaitForSeconds(FadeWaitTime);
          StartCoroutine(UIManager.Instance.moverect(IllustRect, IllustClosePos, IllustOpenPos, DialogueUIMoveTime, true));
          yield return new WaitForSeconds(FadeWaitTime);
          //일러스트+이름 세팅해두고  이동

          DescriptionTextGroup.alpha = 1.0f;
          DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex];
          LayoutRebuilder.ForceRebuildLayoutImmediate(DescriptionText.transform as RectTransform);
          yield return  StartCoroutine(UIManager.Instance.moverect(DescriptionRect, DescriptionClosePos, DescriptionOpenPos, DialogueUIMoveTime, true));
          NextButton.interactable = true;
        }
        else                                 //다음 버튼 눌러서 다음 내용 전개하기
        {
          NextButton.interactable = false;

          StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup,0.0f,FadeOutTime));
          yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup,0.0f,FadeOutTime));
          yield return new WaitForSeconds(FadeWaitTime);

          IllustImage.sprite = CurrentEventIllustHolderes[CurrentEventPhaseIndex].CurrentIllust;
          DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex];
          LayoutRebuilder.ForceRebuildLayoutImmediate(DescriptionText.transform as RectTransform);

          StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 1.0f, FadeInTime));
          yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 1.0f, FadeInTime));
          yield return new WaitForSeconds(FadeWaitTime);

          NextButton.interactable = true;
        }
      }
    }
    else
    {
      if (CurrentEventPhaseIndex == CurrentEventPhaseMaxIndex-1)             //보상 단계에 도달
      {
        if (CurrentEventPhaseIndex == 0)     //선택지 선택 후 바로 보상일때         (선택지 애니메이션은 완료)
        {
          StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 0.0f, FadeOutTime));
          yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 0.0f, FadeOutTime));
          yield return new WaitForSeconds(FadeWaitTime);

          IllustImage.sprite = CurrentEventIllustHolderes[CurrentEventPhaseIndex].CurrentIllust;
          LayoutRebuilder.ForceRebuildLayoutImmediate(DescriptionText.transform as RectTransform);
          SelectionGroup.gameObject.SetActive(false);
          if (CurrentSuccessData != null)
          {
            RewardButtonGroup.alpha = 0.0f;
            if (RewardButtonGroup.gameObject.activeInHierarchy == false) RewardButtonGroup.gameObject.SetActive(true);
            StartCoroutine(UIManager.Instance.ChangeAlpha(RewardButtonGroup, 1.0f, ButtonFadeinTime));
            DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex];
          }
          else if (CurrentFailData != null)
          {
            switch (CurrentFailData.Panelty_target)
            {
              case PenaltyTarget.None:
                DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex];
                break;
              case PenaltyTarget.EXP:
                DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex];
                break;
              case PenaltyTarget.Status:
                switch (CurrentFailData.Loss_target)
                {
                  case StatusTypeEnum.HP:
                    DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex]+
                      $"<br><br>{GameManager.Instance.GetTextData(StatusTypeEnum.HP,2)} {WNCText.GetHPColor(-PenaltyValue)}";
                    break;
                  case StatusTypeEnum.Sanity:
                    DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex] +
        $"<br><br>{GameManager.Instance.GetTextData(StatusTypeEnum.Sanity, 2)} {WNCText.GetHPColor(-PenaltyValue)}";
                    break;
                  case StatusTypeEnum.Gold:
                    DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex] +
        $"<br><br>{GameManager.Instance.GetTextData(StatusTypeEnum.Gold, 2)} {WNCText.GetHPColor(-PenaltyValue)}";
                    break;
                }
                break;
            }
          }
          StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 1.0f, FadeInTime));
          yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 1.0f, FadeInTime));
        }
        else                                 //다음 버튼 눌러서 보상에 도달할때
        {
          StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 0.0f, FadeOutTime));
          StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 0.0f, FadeOutTime));
          yield return StartCoroutine(UIManager.Instance.ChangeAlpha(NextButtonGroup,0.0f,FadeOutTime));
          yield return new WaitForSeconds(FadeWaitTime);

          if (CurrentSuccessData != null)
          {
            RewardButtonGroup.alpha = 0.0f;
            if (RewardButtonGroup.gameObject.activeInHierarchy == false) RewardButtonGroup.gameObject.SetActive(true);
            StartCoroutine(UIManager.Instance.ChangeAlpha(RewardButtonGroup, 1.0f, ButtonFadeinTime));
            DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex];
          }
          else if (CurrentFailData != null)
          {
            switch (CurrentFailData.Panelty_target)
            {
              case PenaltyTarget.None:
                DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex];
                break;
              case PenaltyTarget.EXP:
                DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex];
                break;
              case PenaltyTarget.Status:
                switch (CurrentFailData.Loss_target)
                {
                  case StatusTypeEnum.HP:
                    DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex] +
                      $"<br><br>{GameManager.Instance.GetTextData(StatusTypeEnum.HP, 2)} {WNCText.GetHPColor(-PenaltyValue)}";
                    break;
                  case StatusTypeEnum.Sanity:
                    DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex] +
        $"<br><br>{GameManager.Instance.GetTextData(StatusTypeEnum.Sanity, 2)} {WNCText.GetHPColor(-PenaltyValue)}";
                    break;
                  case StatusTypeEnum.Gold:
                    DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex] +
        $"<br><br>{GameManager.Instance.GetTextData(StatusTypeEnum.Gold, 2)} {WNCText.GetHPColor(-PenaltyValue)}";
                    break;
                }
                break;
            }
          }
          IllustImage.sprite = CurrentEventIllustHolderes[CurrentEventPhaseIndex].CurrentIllust;
          LayoutRebuilder.ForceRebuildLayoutImmediate(DescriptionText.transform as RectTransform);

          StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 1.0f, FadeInTime));
          StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 1.0f, FadeInTime));
        }

        OpenReturnButton();
      }
      else                                                                 //다음 내용으로 진행
      {
        if (CurrentEventPhaseIndex == 0)     //선택지 선택 후 새로 설명으로 넘어갈때 (선택지 애니메이션은 완료)
        {
          NextButton.interactable = false;

          StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 0.0f, FadeOutTime));
          yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 0.0f, FadeOutTime));
          yield return new WaitForSeconds(FadeWaitTime);

          IllustImage.sprite = CurrentEventIllustHolderes[CurrentEventPhaseIndex].CurrentIllust;
          DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex];
          LayoutRebuilder.ForceRebuildLayoutImmediate(DescriptionText.transform as RectTransform);
          SelectionGroup.gameObject.SetActive(false);
          NextButtonGroup.gameObject.SetActive(true);

          StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 1.0f, FadeInTime));
          StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 1.0f, FadeInTime));
          yield return StartCoroutine(UIManager.Instance.ChangeAlpha(NextButtonGroup,1.0f, ButtonFadeinTime));
          yield return new WaitForSeconds(FadeWaitTime);
          NextButton.interactable = true;
        }
        else                                 //다음 버튼 눌러서 다음 내용 전개하기
        {
          NextButton.interactable = false;

          StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 0.0f, FadeOutTime));
          yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 0.0f, FadeOutTime));
          yield return new WaitForSeconds(FadeWaitTime);

          IllustImage.sprite = CurrentEventIllustHolderes[CurrentEventPhaseIndex].CurrentIllust;
          DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex];
          LayoutRebuilder.ForceRebuildLayoutImmediate(DescriptionText.transform as RectTransform);

          StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 1.0f, FadeInTime));
          yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 1.0f, FadeInTime));
          yield return new WaitForSeconds(FadeWaitTime);

          NextButton.interactable = true;
        }
      }
    }

    CurrentEventPhaseIndex++;
  }
  private IEnumerator setupselections()
  {
    SelectionGroup.alpha = 0.0f;

    if (NextButton.gameObject.activeInHierarchy == true) NextButton.gameObject.SetActive(false);
    if (SelectionGroup.gameObject.activeInHierarchy == false) SelectionGroup.gameObject.SetActive(true);
    if (RewardButtonGroup.gameObject.activeInHierarchy == true) RewardButtonGroup.gameObject.SetActive(false);


    if (CurrentEvent.Selection_type == SelectionTypeEnum.Body || CurrentEvent.Selection_type == SelectionTypeEnum.Head)
      SelectionCenterImgGroup.alpha = 1.0f;
    //양자택일 형태 선택지일 경우 가운데 구분 이미지 활성화

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

    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(SelectionGroup,1.0f,FadeInTime));
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
      StartCoroutine(UIManager.Instance.ChangeAlpha(SelectionCenterImgGroup, 0.0f, 0.4f));
    }
    //다른거 사라지게 만들고
    UIManager.Instance.AddUIQueue(selectionanimation(_selection));
    //선택한 선택지를 중심으로 이동시키고 성공, 실패 검사 실행 
  }
  private IEnumerator selectionanimation(UI_Selection _selection)
  {
    CurrentUISelection = _selection;
    if (!_selection.MyTendencyType.Equals(TendencyTypeEnum.None))
    {
       yield return StartCoroutine(_selection.movetocenter());
    }//성향이 존재한다면 가운데로 이동시킴

    SelectionData _selectiondata = _selection.MySelectionData;
    int _currentvalue = 0, _checkvalue = 0;    //기술 체크에만 사용
    int _successpercent = 0;                   //성공 확률(골드 혹은 기술 체크) 
    bool _issuccess = false;  
    int _pluspercent = GameManager.Instance.MyGameData.LibraryEffect ? ConstValues.SectorEffect_Library : 0;
                                               //도서관 방문 시 확률 증가 값
    switch (_selectiondata.ThisSelectionType)
    {
      case SelectionTargetType.None:
        _issuccess = true;
        break;
      case SelectionTargetType.Pay:
        if (_selectiondata.SelectionPayTarget.Equals(StatusTypeEnum.HP))
        {
          _issuccess = true;
          GameManager.Instance.MyGameData.HP -= GameManager.Instance.MyGameData.PayHPValue_modified;
        }
        else if (_selectiondata.SelectionPayTarget.Equals(StatusTypeEnum.Sanity))
        {
          _issuccess = true;//체력,정신력 지불의 경우 남은 값과 상관 없이 일단 성공으로만 친다
          GameManager.Instance.MyGameData.Sanity -= GameManager.Instance.MyGameData.PaySanityValue_modified;
        }
        else        //돈 지불일 경우 돈 적을 때 실행하는 뭔가 있어야 함
        {
          int _paygoldvalue = (int)(GameManager.Instance.MyGameData.PayGoldValue_modified * GameManager.Instance.MyGameData.GetGoldPayModify(true));
          int _goldsuccesspercent = GameManager.Instance.MyGameData.Gold >= _paygoldvalue ? 100 : GameManager.Instance.MyGameData.CheckPercent_money(_paygoldvalue);
          if (_goldsuccesspercent + _pluspercent >= 100)
          {
            if (_goldsuccesspercent < 100 && !_pluspercent.Equals(0)) GameManager.Instance.MyGameData.LibraryEffect = false;
            //장소 효과의 도움을 받아 성공한 것이라면 장소 효과 만료
            _issuccess = true;
            GameManager.Instance.MyGameData.Gold -= GameManager.Instance.MyGameData.PayGoldValue_modified;
            Debug.Log("정당한 값을 지불한 레후~");
          }//100% 확률이 나온 상황(돈이 부족하거나 돈이 충분하거나 둘 다)
          else
          {
            if (Random.Range(0, 100) < _goldsuccesspercent + _pluspercent)
            {
              if (_goldsuccesspercent < 100 && !_pluspercent.Equals(0)) GameManager.Instance.MyGameData.LibraryEffect = false;
              int _elsevalue = GameManager.Instance.MyGameData.PayGoldValue_modified - GameManager.Instance.MyGameData.Gold;
              //장소 효과의 도움을 받아 성공한 것이라면 장소 효과 만료
              _issuccess = true;
              GameManager.Instance.MyGameData.Gold = 0;
              GameManager.Instance.MyGameData.Sanity -= (int)(_elsevalue * ConstValues.GoldSanityPayAmplifiedValue);
            }//돈이 부족해 성공한 경우
            else
            {
              _issuccess = false;
            }//돈이 부족해 실패한 경우
          }//돈이 부족해 체크를 해야 하는 상황
        }
        break;
      case SelectionTargetType.Check_Single: //기술(단수) 선택지면 확률 검사
        _currentvalue = GameManager.Instance.MyGameData.GetSkill(_selectiondata.SelectionCheckSkill[0]).Level;
        _checkvalue = GameManager.Instance.MyGameData.CheckSkillSingleValue;
        _successpercent = GameManager.Instance.MyGameData.CheckPercent_themeorskill(_currentvalue, _checkvalue);
        if (Random.Range(0, 100) < _successpercent + _pluspercent)
        {
          _issuccess = true;
          if (_successpercent < 100 && _pluspercent > 0)
          {
            GameManager.Instance.MyGameData.LibraryEffect = false;
          }
          //장소 효과의 도움을 받아 성공한 것이라면 효과 만료
        }
        else _issuccess = false;
        break;
      case SelectionTargetType.Check_Multy: //기술(복수) 선택지면 확률 검사
        _currentvalue = GameManager.Instance.MyGameData.GetSkill(_selectiondata.SelectionCheckSkill[0]).Level +
          GameManager.Instance.MyGameData.GetSkill(_selectiondata.SelectionCheckSkill[1]).Level;
        _checkvalue = GameManager.Instance.MyGameData.CheckSkillMultyValue;
        _successpercent = GameManager.Instance.MyGameData.CheckPercent_themeorskill(_currentvalue, _checkvalue);
        if (Random.Range(0, 100) < _successpercent + _pluspercent)
        {
          _issuccess = true;
          if (_successpercent < 100 && _pluspercent > 0)
          {
            GameManager.Instance.MyGameData.LibraryEffect = false;
          }
          //장소 효과의 도움을 받아 성공한 것이라면 효과 만료
        }
        else _issuccess = false;
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
      SetFail(CurrentEvent.SelectionDatas[_selection.Index].FailureData);
      GameManager.Instance.FailCurrentEvent(_selection.MyTendencyType, _selection.Index);
    }

    yield return null;
  }//선택한 선택지 성공 여부를 계산하고 애니메이션을 실행시키는 코루틴
  //이 코루틴에서 SetSuccess 아니면 SetFail로 바로 넘어감

  private SuccessData CurrentSuccessData = null;
  public bool RemainReward = false;
  public void SetSuccess(SuccessData _success)
  {
    CurrentSuccessData = _success;
    RemainReward = true;
    Sprite _icon = null;
    string _name = "";
    string _description = "";
    switch (_success.Reward_Type)
    {
      case RewardTypeEnum.Status:
        switch (_success.Reward_StatusType)
        {
          case StatusTypeEnum.HP:
            _icon = GameManager.Instance.ImageHolder.HPIcon;
            _name = GameManager.Instance.GetTextData(StatusTypeEnum.HP, 0);
            _description = $"+{WNCText.GetHPColor(GameManager.Instance.MyGameData.RewardHPValue_modified)}";
            break;
          case StatusTypeEnum.Sanity:
            _icon = GameManager.Instance.ImageHolder.SanityIcon;
            _name = GameManager.Instance.GetTextData(StatusTypeEnum.Sanity, 0);
            _description = $"+{WNCText.GetSanityColor(GameManager.Instance.MyGameData.RewardSanityValue_modified)}";
            break;
          case StatusTypeEnum.Gold:
            _icon = GameManager.Instance.ImageHolder.GoldIcon;
            _name = GameManager.Instance.GetTextData(StatusTypeEnum.Gold, 0);
            _description = $"+{WNCText.GetGoldColor(GameManager.Instance.MyGameData.RewardGoldValue_modified)}";
            break;
        }
        break;
      case RewardTypeEnum.Experience:
        _icon = GameManager.Instance.ImageHolder.UnknownExpRewardIcon;
        _name = GameManager.Instance.GetTextData("EXP_NAME");
        _description = GameManager.Instance.ExpDic[CurrentSuccessData.Reward_EXPID].Name;

        break;
      case RewardTypeEnum.Skill:
        _icon = GameManager.Instance.ImageHolder.GetSkillIcon(CurrentSuccessData.Reward_SkillType,false);
        _name = $"{GameManager.Instance.GetTextData(CurrentSuccessData.Reward_SkillType,0)} +1";
        break;
    }
    RewardIcon.sprite = _icon;
    RewardName.text = _name;
    RewardDescription.text = _description;

    if (CurrentEvent.GetType().Equals(typeof(FollowEventData)))
    {
      var _currentfollow = (FollowEventData)CurrentEvent;
      if (_currentfollow.EndingData != null) SetEndingDialogue(((FollowEventData)CurrentEvent).EndingData, _success);
    }
    else
    {
      IsBeginning = false;
      CurrentEventPhaseMaxIndex = CurrentSuccessData.Descriptions.Count;
      CurrentEventPhaseIndex = 0;
      CurrentEventIllustHolderes = CurrentSuccessData.Illusts;
      CurrentEventDescriptions = CurrentSuccessData.Descriptions;

      UIManager.Instance.AddUIQueue(displaynextindex());
    }
    //연계 이벤트고, 엔딩 설정이 돼 있는 상태에서 성공할 경우 엔딩 다이어로그 전개
  }//성공할 경우 보상 탭을 세팅하고 텍스트를 성공 설명으로 교체, 퀘스트 이벤트일 경우 진행도 ++

  private FailureData CurrentFailData = null;
  public Button CurrentReturnButton = null;
  public void SetFail(FailureData _fail)
  {
    CurrentFailData = _fail;
    SetPenalty(_fail);

    IsBeginning = false;
    CurrentEventPhaseMaxIndex = CurrentFailData.Descriptions.Count;
    CurrentEventPhaseIndex = 0;
    CurrentEventIllustHolderes = CurrentFailData.Illusts;
    CurrentEventDescriptions = CurrentFailData.Descriptions;

    UIManager.Instance.AddUIQueue(displaynextindex());
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
  public override void CloseForGameover()
  {
    StopAllCoroutines();

    IsOpen = false;
    CurrentSuccessData = null;
    CurrentFailData = null;
    EndingGroup.alpha = 0.0f;
    EndingGroup.interactable = false;
    EndingGroup.blocksRaycasts = false;

    StartCoroutine(UIManager.Instance.ChangeAlpha(RewardButtonGroup, 0.0f, 0.3f));

    StartCoroutine(UIManager.Instance.moverect(NameRect, NameRect.anchoredPosition, NameClosePos, DialogueUIMoveTime, false));

    StartCoroutine(UIManager.Instance.moverect(DescriptionRect, DescriptionRect.anchoredPosition, DescriptionClosePos, DialogueUIMoveTime, false));

    StartCoroutine(UIManager.Instance.moverect(IllustRect, IllustRect.anchoredPosition, IllustClosePos, DialogueUIMoveTime, false));
  }
  public override void CloseUI()
  {
    IsOpen = false;
    UIManager.Instance.AddUIQueue(closeui());
  }
  private IEnumerator closeui()
  {
    CurrentSuccessData = null;
    CurrentFailData = null;
    EndingGroup.alpha = 0.0f;
    EndingGroup.interactable = false;
    EndingGroup.blocksRaycasts = false;

    StartCoroutine(UIManager.Instance.ChangeAlpha(RewardButtonGroup, 0.0f, 0.3f));

    StartCoroutine(UIManager.Instance.moverect(NameRect, NameRect.anchoredPosition, NameClosePos, DialogueUIMoveTime, false));
    StartCoroutine(UIManager.Instance.moverect(DescriptionRect, DescriptionOpenPos, DescriptionClosePos, DialogueUIMoveTime, false));
    yield return StartCoroutine(UIManager.Instance.moverect(IllustRect, IllustOpenPos, IllustClosePos, DialogueUIMoveTime, false));
  }

  #region 엔딩?
  public void SetEndingDialogue(FollowEndingData endingdata, SuccessData successdata)
  {
  }

  private IEnumerator openendingbuttons()
  {
    StartCoroutine(UIManager.Instance.ChangeAlpha(EndingGroup, 1.0f, 0.2f));
    yield return null;
  }
  public void OpenEnding()
  {
    if (UIManager.Instance.IsWorking) return;
    FollowEndingData _endingdata = ((FollowEventData)CurrentEvent).EndingData;
    UIManager.Instance.OpenEnding(_endingdata);
  }
  public void RefuseEnding()
  {
    if (UIManager.Instance.IsWorking) return;
    UIManager.Instance.AddUIQueue(refuseending());
  }
  private IEnumerator refuseending()
  {
    yield return null;
  }
  #endregion

  public void GetReward()
  {
    if (UIManager.Instance.IsWorking) return;

    if (CurrentSuccessData != null)
    {
      if (CurrentSuccessData.Reward_Type == RewardTypeEnum.Experience)
      {
        if (GameManager.Instance.MyGameData.AvailableExpSlot == false)
        {
          GameManager.Instance.MyGameData.Sanity += ConstValues.GoodExpAsSanity;

          StartCoroutine(UIManager.Instance.ChangeAlpha(RewardButtonGroup, 0.0f, 0.6f));
          RemainReward = false;
        }
        else
        {
          RewardExpUI.OpenUI_RewardExp(GameManager.Instance.ExpDic[CurrentSuccessData.Reward_EXPID]);
        }
        return;
      }
      else
      {
        switch (CurrentSuccessData.Reward_Type)
        {
          case RewardTypeEnum.Status:
            switch (CurrentSuccessData.Reward_StatusType)
            {
              case StatusTypeEnum.HP:
                GameManager.Instance.MyGameData.HP += GameManager.Instance.MyGameData.RewardHPValue_modified;
                break;
              case StatusTypeEnum.Sanity:
                GameManager.Instance.MyGameData.Sanity += GameManager.Instance.MyGameData.RewardSanityValue_modified;
                break;
              case StatusTypeEnum.Gold:
                GameManager.Instance.MyGameData.Gold += GameManager.Instance.MyGameData.RewardGoldValue_modified;
                break;
            }
            break;
          case RewardTypeEnum.Skill:
            GameManager.Instance.MyGameData.GetSkill(CurrentSuccessData.Reward_SkillType).LevelByDefault++;
            break;
        }

        StartCoroutine(UIManager.Instance.ChangeAlpha(RewardButtonGroup, 0.0f, 0.6f));
        RemainReward = false;
      }
    }
    else if (CurrentFailData != null)
    {
      if (GameManager.Instance.MyGameData.AvailableExpSlot == false)
      {
        GameManager.Instance.MyGameData.Sanity -= ConstValues.BadExpAsSanity;

        StartCoroutine(UIManager.Instance.ChangeAlpha(RewardButtonGroup, 0.0f, 0.6f));
        RemainReward = false;
      }
      else
      RewardExpUI.OpenUI_Penalty(GameManager.Instance.ExpDic[CurrentFailData.ExpID]);
    }
  }
  private int PenaltyValue = 0;
  public void ExpAcquired()
  {
    RemainReward = false;
    StartCoroutine(UIManager.Instance.ChangeAlpha(RewardButtonGroup, 0.0f, 0.6f));
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
          case StatusTypeEnum.HP:
            GameManager.Instance.MyGameData.HP -= GameManager.Instance.MyGameData.FailHPValue_modified;
            PenaltyValue = GameManager.Instance.MyGameData.FailHPValue_modified;
            break;
          case StatusTypeEnum.Sanity:
            GameManager.Instance.MyGameData.Sanity -= GameManager.Instance.MyGameData.FailSanityValue_modified;
            PenaltyValue = GameManager.Instance.MyGameData.FailSanityValue_modified;
            break;
          case StatusTypeEnum.Gold:
            if (GameManager.Instance.MyGameData.Gold >= GameManager.Instance.MyGameData.FailGoldValue_modified)
            {
              GameManager.Instance.MyGameData.Gold -= GameManager.Instance.MyGameData.FailGoldValue_modified;
              PenaltyValue = GameManager.Instance.MyGameData.FailGoldValue_modified;
            }
            else
            {
              PenaltyValue = GameManager.Instance.MyGameData.Gold;
              GameManager.Instance.MyGameData.Gold = 0;
            }
            break;
        }
        break;
      case PenaltyTarget.EXP:
        Sprite _icon = GameManager.Instance.ImageHolder.UnknownExpRewardIcon;
        string _name = "";
        string _description = "";

        if (GameManager.Instance.MyGameData.AvailableExpSlot == false)
        {
          _name = "<s>" + GameManager.Instance.GetTextData("EXP_NAME") + "</s>";
          _description = "<s>" + GameManager.Instance.ExpDic[CurrentSuccessData.Reward_EXPID].Name + "</s><br>" + string.Format(GameManager.Instance.GetTextData("NOEMPTYSLOT"), WNCText.GetSanityColor(ConstValues.BadExpAsSanity));
        }
        else
        {
          _name = GameManager.Instance.GetTextData("EXP_NAME");
          _description = GameManager.Instance.ExpDic[CurrentSuccessData.Reward_EXPID].Name;
        }

        StartCoroutine(UIManager.Instance.ChangeAlpha(RewardButtonGroup, 1.0f, ButtonFadeinTime));

        break;
    }
  }//실패할 경우 패널티 부과하는 연출
}
