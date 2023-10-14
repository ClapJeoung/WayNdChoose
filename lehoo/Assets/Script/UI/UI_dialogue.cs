using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_dialogue : UI_default
{
  public float DialogueUIMoveTime = 0.8f;

  [SerializeField] private TextMeshProUGUI NameText = null;
  [SerializeField] private CanvasGroup IllustImageGroup = null;
  [SerializeField] private Image IllustImage = null;
  [SerializeField] private Image IllustEffect_Image = null;
  [SerializeField] private CanvasGroup IllustEffect_Group = null;
  [SerializeField] private Color IdleColor = Color.white;
  [SerializeField] private Color RationalColor= Color.white;
  [SerializeField] private Color PhysicalColor = Color.white;
  [SerializeField] private Color MentalColor = Color.white;
  [SerializeField] private Color MaterialColor = Color.white;
  [SerializeField] private Color FailColor = Color.red;
  private float FadeOutTime = 0.7f;
  private float FadeInTime = 0.9f;
  private float FadeWaitTime = 0.3f;
  private float ButtonFadeinTime = 0.4f;
 // [SerializeField] private Image EventIcon = null;
 // private GameObject EventIconHolder { get { return EventIcon.transform.parent.gameObject; } }
  [Space(10)]
  [SerializeField] private TextMeshProUGUI DescriptionText = null;
  [SerializeField] private CanvasGroup DescriptionTextGroup = null;
  [Space(10)]
  [SerializeField] private CanvasGroup NextButtonGroup = null;
  [SerializeField] private UnityEngine.UI.Button NextButton = null;
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
  [SerializeField] private TextMeshProUGUI RewardName = null;
  [SerializeField] private TextMeshProUGUI RewardDescription = null;
  [SerializeField] private TextMeshProUGUI Reward_clicktoget = null;
  [Space(10)]
  [SerializeField] private CanvasGroup EndingButtonGroup = null;
  [SerializeField] private TextMeshProUGUI EndingButtonText = null;
  [Space(10)]
  [SerializeField] private UI_Settlement SettlementUI = null;
  [SerializeField] private UI_map MapUI = null;
  [SerializeField] private UI_RewardExp RewardExpUI = null;
  private EventData CurrentEvent
  {
    get { return GameManager.Instance.MyGameData.CurrentEvent; }
  }
  private RectTransform IllustRect { get {return GetPanelRect("illust").Rect; } }
  private Vector2 IllustOpenPos { get { return GetPanelRect("illust").InsidePos; } }
  private Vector2 IllustClosePos { get { return GetPanelRect("illust").OutisdePos; } }
  private RectTransform DescriptionRect { get { return GetPanelRect("description").Rect; } }
  private Vector2 DescriptionOpenPos { get { return GetPanelRect("description").InsidePos; } }
  private Vector2 DescriptionClosePos { get { return GetPanelRect("description").OutisdePos; } }
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
    EndingButtonGroup.alpha = 0.0f;
    EndingButtonGroup.interactable = false;
    EndingButtonGroup.blocksRaycasts = false;

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
      if (CurrentEventPhaseIndex == CurrentEventPhaseMaxIndex-1)              //������ �ܰ迡 ����
      {
        if (CurrentEventPhaseIndex == 0)     //UI ó�� ���� �ٷ� �������϶�
        {

          SelectionGroup.alpha = 0.0f;
          NextButtonGroup.alpha = 0.0f;
          if (NextButton.gameObject.activeInHierarchy == true) NextButton.gameObject.SetActive(false);
          if (SelectionGroup.gameObject.activeInHierarchy == false) SelectionGroup.gameObject.SetActive(true);
          if (RewardButtonGroup.gameObject.activeInHierarchy == true) RewardButtonGroup.gameObject.SetActive(false);
          if (EndingButtonGroup.gameObject.activeInHierarchy == true) EndingButtonGroup.gameObject.SetActive(false);
          StartCoroutine(setupselections());
          //���� ���� ������ ����

          IllustImageGroup.alpha = 1.0f;
          IllustImage.sprite = CurrentEventIllustHolderes[CurrentEventPhaseIndex].CurrentIllust;
          NameText.text = CurrentEvent.Name;
          DescriptionTextGroup.alpha = 1.0f;
          DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex];
          LayoutRebuilder.ForceRebuildLayoutImmediate(DescriptionText.transform as RectTransform);
          LayoutRebuilder.ForceRebuildLayoutImmediate(NameText.transform.parent.transform as RectTransform);
          LayoutRebuilder.ForceRebuildLayoutImmediate(DescriptionText.transform.parent.transform.parent.transform as RectTransform);

          StartCoroutine(UIManager.Instance.moverect(IllustRect, IllustClosePos, IllustOpenPos, DialogueUIMoveTime, true));
          yield return new WaitForSeconds(FadeWaitTime);
          //�Ϸ���Ʈ+�̸� �����صΰ�  �̵�

          yield return StartCoroutine(UIManager.Instance.moverect(DescriptionRect, DescriptionClosePos, DescriptionOpenPos, DialogueUIMoveTime, true));
        }
        else                                 //���� ��ư ������ �������� �����Ҷ�
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

          LayoutRebuilder.ForceRebuildLayoutImmediate(DescriptionText.transform.parent.transform.parent.transform as RectTransform);

          StartCoroutine(setupselections());
          StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 1.0f, FadeInTime));
          yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 1.0f, FadeInTime));
        }
      }
      else                                                                 //���� �������� ����
      {
        if (CurrentEventPhaseIndex == 0)     //UI ó�� ���� ���� �������϶�
        {
          NextButtonGroup.alpha = 1.0f;
          NextButtonGroup.interactable = true;
          NextButtonGroup.blocksRaycasts = true;
          NextButton.interactable = false;
          if (NextButton.gameObject.activeInHierarchy == false) NextButton.gameObject.SetActive(true);
          if (SelectionGroup.gameObject.activeInHierarchy == true) SelectionGroup.gameObject.SetActive(false);
          if (RewardButtonGroup.gameObject.activeInHierarchy == true) RewardButtonGroup.gameObject.SetActive(false);
          if (EndingButtonGroup.gameObject.activeInHierarchy == true) EndingButtonGroup.gameObject.SetActive(false);

          IllustImageGroup.alpha = 1.0f;
          IllustImage.sprite = CurrentEventIllustHolderes[CurrentEventPhaseIndex].CurrentIllust;
          LayoutRebuilder.ForceRebuildLayoutImmediate(NameText.transform as RectTransform);
          NameText.text = CurrentEvent.Name;
          LayoutRebuilder.ForceRebuildLayoutImmediate(NameText.transform.parent.transform as RectTransform);
          StartCoroutine(UIManager.Instance.moverect(IllustRect, IllustClosePos, IllustOpenPos, DialogueUIMoveTime, true));
          yield return new WaitForSeconds(FadeWaitTime);
          //�Ϸ���Ʈ+�̸� �����صΰ�  �̵�

          DescriptionTextGroup.alpha = 1.0f;
          DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex];
          LayoutRebuilder.ForceRebuildLayoutImmediate(DescriptionText.transform as RectTransform);
          yield return  StartCoroutine(UIManager.Instance.moverect(DescriptionRect, DescriptionClosePos, DescriptionOpenPos, DialogueUIMoveTime, true));
          NextButton.interactable = true;
        }
        else                                 //���� ��ư ������ ���� ���� �����ϱ�
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
      if (CurrentEventPhaseIndex == CurrentEventPhaseMaxIndex-1)             //���� �ܰ迡 ����
      {
        if (CurrentEventPhaseIndex == 0)     //������ ���� �� �ٷ� �����϶�         (������ �ִϸ��̼��� �Ϸ�)
        {
          StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 0.0f, FadeOutTime));
          yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 0.0f, FadeOutTime));
          yield return new WaitForSeconds(FadeWaitTime);

          IllustImage.sprite = CurrentEventIllustHolderes[CurrentEventPhaseIndex].CurrentIllust;
          LayoutRebuilder.ForceRebuildLayoutImmediate(DescriptionText.transform as RectTransform);
          SelectionGroup.gameObject.SetActive(false);
          if (CurrentSuccessData != null)
          {
            if (CurrentSuccessData.Reward_Type != RewardTypeEnum.None)
            {
              RewardButtonGroup.alpha = 0.0f;
              if (RewardButtonGroup.gameObject.activeInHierarchy == false) RewardButtonGroup.gameObject.SetActive(true);
              StartCoroutine(UIManager.Instance.ChangeAlpha(RewardButtonGroup, 1.0f, ButtonFadeinTime));
            }

            DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex];

            if (CurrentEvent.EndingID != "")
            {
              if (EndingButtonGroup.gameObject.activeInHierarchy == false) EndingButtonGroup.gameObject.SetActive(true);
              LayoutRebuilder.ForceRebuildLayoutImmediate(EndingButtonGroup.transform as RectTransform);
              StartCoroutine(UIManager.Instance.ChangeAlpha(EndingButtonGroup,1.0f,ButtonFadeinTime));
            }
          }
          else if (CurrentFailData != null)
          {
            switch (CurrentFailData.Penelty_target)
            {
              case PenaltyTarget.None:
                DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex];
                break;
              case PenaltyTarget.EXP:
                DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex];
                break;
              case PenaltyTarget.Status:
                switch (CurrentFailData.StatusType)
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
        else                                 //���� ��ư ������ ���� �����Ҷ�
        {
          StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 0.0f, FadeOutTime));
          StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 0.0f, FadeOutTime));
          yield return StartCoroutine(UIManager.Instance.ChangeAlpha(NextButtonGroup,0.0f,FadeOutTime));
          yield return new WaitForSeconds(FadeWaitTime);

          if (CurrentSuccessData != null)
          {
            if (CurrentSuccessData.Reward_Type != RewardTypeEnum.None)
            {
              RewardButtonGroup.alpha = 0.0f;
              if (RewardButtonGroup.gameObject.activeInHierarchy == false) RewardButtonGroup.gameObject.SetActive(true);
              StartCoroutine(UIManager.Instance.ChangeAlpha(RewardButtonGroup, 1.0f, ButtonFadeinTime));
            }

            DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex];

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
                DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex];
                break;
              case PenaltyTarget.EXP:
                DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex];
                break;
              case PenaltyTarget.Status:
                switch (CurrentFailData.StatusType)
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
      else                                                                 //���� �������� ����
      {
        if (CurrentEventPhaseIndex == 0)     //������ ���� �� ���� �������� �Ѿ�� (������ �ִϸ��̼��� �Ϸ�)
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
        else                                 //���� ��ư ������ ���� ���� �����ϱ�
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
  /// ������ Ŭ�������� ������ ��ũ��Ʈ���� ȣ��
  /// </summary>
  /// <param name="_selection"></param>
  public void SelectSelection(UI_Selection _selection)
  {
    if (_selection.MyTendencyType != TendencyTypeEnum.None)
    {
      GetOppositeSelection(_selection).DeActive();
    }
    //�ٸ��� ������� �����
    UIManager.Instance.AddUIQueue(selectionanimation(_selection));
    //����, ���� �˻� ���� 
  }
  private IEnumerator selectionanimation(UI_Selection _selection)
  {
    CurrentUISelection = _selection;

    SelectionData _selectiondata = _selection.MySelectionData;
    int _payvalue = 0;
    int _currentvalue = 0, _checkvalue = 0;    //��� üũ���� ���
    int _percentvalue = UnityEngine.Random.Range(0,100);
    int _successpercent = 0;                   //���� Ȯ��(��� Ȥ�� ��� üũ) 
    bool _issuccess = false;  
                                               
    switch (_selectiondata.ThisSelectionType)
    {
      case SelectionTargetType.None:
        _issuccess = true;
        break;
      case SelectionTargetType.Pay:
        if (_selectiondata.SelectionPayTarget.Equals(StatusTypeEnum.HP))
        {
          _payvalue = GameManager.Instance.MyGameData.PayHPValue_modified;
          yield return StartCoroutine(payanimation(_selection.PayIcon, StatusTypeEnum.HP, _payvalue, 0, _selection.PayInfo));

          _issuccess = true;
          GameManager.Instance.MyGameData.HP -= GameManager.Instance.MyGameData.PayHPValue_modified;
        }
        else if (_selectiondata.SelectionPayTarget.Equals(StatusTypeEnum.Sanity))
        {
          _payvalue = GameManager.Instance.MyGameData.PaySanityValue_modified;
          yield return StartCoroutine(payanimation(_selection.PayIcon, StatusTypeEnum.Sanity, _payvalue, 0, _selection.PayInfo));

          _issuccess = true;//ü��,���ŷ� ������ ��� ���� ���� ��� ���� �ϴ� �������θ� ģ��
          GameManager.Instance.MyGameData.Sanity -= GameManager.Instance.MyGameData.PaySanityValue_modified;
        }
        else        //�� ������ ��� �� ���� �� �����ϴ� ���� �־�� ��
        {
          _payvalue = GameManager.Instance.MyGameData.PayGoldValue_modified;
          int _goldsuccesspercent = GameManager.Instance.MyGameData.Gold >= _payvalue ? 100 : GameManager.Instance.MyGameData.CheckPercent_money(_payvalue);

          if (GameManager.Instance.MyGameData.Gold >= _payvalue)
          {
            yield return StartCoroutine(payanimation(_selection.PayIcon, StatusTypeEnum.Sanity, _payvalue, 0, _selection.PayInfo));
          }
          else
          {
            if (Random.Range(0, 100) > _goldsuccesspercent)
            {
              int _elsevalue = GameManager.Instance.MyGameData.PayGoldValue_modified - GameManager.Instance.MyGameData.Gold;

              yield return StartCoroutine(payanimation(_selection.PayIcon, StatusTypeEnum.Sanity, _payvalue, 0, _selection.PayInfo));

              _issuccess = true;
              GameManager.Instance.MyGameData.Gold = 0;
              GameManager.Instance.MyGameData.Sanity -= (int)(_elsevalue * ConstValues.GoldSanityPayAmplifiedValue);
              Debug.Log("������ ���� ������ ����~");
            }//���� ������ ������ ���
            else
            {
              yield return StartCoroutine(payanimation(_selection.PayIcon, StatusTypeEnum.Sanity, _payvalue,_payvalue - GameManager.Instance.MyGameData.Gold, _selection.PayInfo));

              _issuccess = false;
            }//���� ������ ������ ���
          }
        }
        break;
      case SelectionTargetType.Check_Single: //���(�ܼ�) �������� Ȯ�� �˻�
        _currentvalue = GameManager.Instance.MyGameData.GetSkill(_selectiondata.SelectionCheckSkill[0]).Level;
        _checkvalue = GameManager.Instance.MyGameData.CheckSkillSingleValue;
        _successpercent = GameManager.Instance.MyGameData.CheckPercent_themeorskill(_currentvalue, _checkvalue);
        if (_percentvalue >= _successpercent)
        {
          _issuccess = true;
        }
        else
        {
          _issuccess = false;
        }

        yield return StartCoroutine(checkanimation(_selection.SkillIcon_A, _percentvalue / (float)_successpercent));
        break;
      case SelectionTargetType.Check_Multy: //���(����) �������� Ȯ�� �˻�
        _currentvalue = GameManager.Instance.MyGameData.GetSkill(_selectiondata.SelectionCheckSkill[0]).Level +
          GameManager.Instance.MyGameData.GetSkill(_selectiondata.SelectionCheckSkill[1]).Level;
        _checkvalue = GameManager.Instance.MyGameData.CheckSkillMultyValue;
        _successpercent = GameManager.Instance.MyGameData.CheckPercent_themeorskill(_currentvalue, _checkvalue);
        if (_percentvalue >= _successpercent)
        {
          _issuccess = true;
        }
        else
        {
          _issuccess = false;
        }
        yield return StartCoroutine(checkanimation(_selection.SkillIcon_A, _selection.SkillIcon_B, _percentvalue / (float)_successpercent));
        break;
    }

    if (_issuccess) //�����ϸ� ����
    {
      Debug.Log("������");
      SetSuccess(CurrentEvent.SelectionDatas[_selection.Index].SuccessData);
      GameManager.Instance.SuccessCurrentEvent(_selection.MyTendencyType, _selection.Index);
    }
    else            //�����ϸ� ����
    {
      Debug.Log("������");
      SetFail(CurrentEvent.SelectionDatas[_selection.Index].FailureData);
      GameManager.Instance.FailCurrentEvent(_selection.MyTendencyType, _selection.Index);
    }

    yield return null;
  }//������ ������ ���� ���θ� ����ϰ� �ִϸ��̼��� �����Ű�� �ڷ�ƾ
  //�� �ڷ�ƾ���� SetSuccess �ƴϸ� SetFail�� �ٷ� �Ѿ
  [SerializeField] private float SelectionEffectTime = 2.0f;
  [SerializeField] private AnimationCurve SelectionCheckCurve = null;
  private IEnumerator payanimation(Image image, StatusTypeEnum status,int payvalue, int targetvalue, TextMeshProUGUI tmp)
  {
    float _time = 0.0f;
    string _str = "";
    while(_time<SelectionEffectTime*(1.0f-targetvalue/(float)payvalue))
    {
      image.fillAmount = 1.0f - _time / SelectionEffectTime;
      switch (status)
      {
        case StatusTypeEnum.HP:_str = WNCText.GetHPColor((int)Mathf.Lerp(payvalue, targetvalue, _time / SelectionEffectTime));break;
        case StatusTypeEnum.Sanity: _str = WNCText.GetSanityColor((int)Mathf.Lerp(payvalue, targetvalue, _time / SelectionEffectTime)); break;
        case StatusTypeEnum.Gold: _str = WNCText.GetGoldColor((int)Mathf.Lerp(payvalue, targetvalue, _time / SelectionEffectTime)); break;
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
  }
  private IEnumerator checkanimation(Image image,float successvalue)
  {
    Debug.Log(successvalue);
    float _time = 0.0f;
    while(_time<SelectionEffectTime* successvalue)
    {
      image.fillAmount=Mathf.Lerp(1.0f,0.0f, SelectionCheckCurve.Evaluate(_time/SelectionEffectTime));
      _time += Time.deltaTime;yield return null;
    }
   // image.fillAmount = SelectionCheckCurve.Evaluate(_time / SelectionEffectTime);

  }
  private IEnumerator checkanimation(Image image_L,Image image_R,float successvalue)
  {
    Debug.Log(successvalue);
    float _time = 0.0f;
    while (_time < SelectionEffectTime * successvalue)
    {
      if (_time < SelectionEffectTime / 2)
        image_L.fillAmount = Mathf.Lerp(1.0f, 0.0f, SelectionCheckCurve.Evaluate(_time / (SelectionEffectTime / 2)));
      else
        image_R.fillAmount = Mathf.Lerp(1.0f, 0.0f, SelectionCheckCurve.Evaluate(_time / (SelectionEffectTime / 2)));
      _time += Time.deltaTime;yield return null;
    }
  //  image_L.fillAmount = successvalue > 0.5f ? 0.0f : 1.0f - successvalue * 2.0f;
  //  image_R.fillAmount = successvalue > 0.5f ? 1.0f - (successvalue - 0.5f) * 2.0f : 1.0f;
  }
  private SuccessData CurrentSuccessData = null;
  public bool RemainReward = false;
  [SerializeField] private float ResultEffectTime = 2.0f;
  public void SetSuccess(SuccessData _success)
  {
    CurrentSuccessData = _success;
    RemainReward = _success.Reward_Type == RewardTypeEnum.None ? false : true;
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

      UIManager.Instance.AddUIQueue(displaynextindex());

    if (CurrentEvent.EndingID != "")
    {
      EndingButtonText.text = GameManager.Instance.ImageHolder.GetEndingIllust(CurrentEvent.EndingID).Name + "<br>" +
        GameManager.Instance.GetTextData("Ending_Description");
    }
    //���� �̺�Ʈ��, ���� ������ �� �ִ� ���¿��� ������ ��� ���� ���̾�α� ����
  }//������ ��� ���� ���� �����ϰ� �ؽ�Ʈ�� ���� �������� ��ü, ����Ʈ �̺�Ʈ�� ��� ���൵ ++

  private FailData CurrentFailData = null;
  public UnityEngine.UI.Button CurrentReturnButton = null;
  public void SetFail(FailData _fail)
  {
    CurrentFailData = _fail;
    SetPenalty(_fail);

    IsBeginning = false;
    CurrentEventPhaseMaxIndex = CurrentFailData.Descriptions.Count;
    CurrentEventPhaseIndex = 0;
    CurrentEventIllustHolderes = CurrentFailData.Illusts;
    CurrentEventDescriptions = CurrentFailData.Descriptions;

    IllustEffect_Image.color = FailColor;
    StartCoroutine((UIManager.Instance.ChangeAlpha(IllustEffect_Group, 0.0f, ResultEffectTime)));

    UIManager.Instance.AddUIQueue(displaynextindex());
  }//������ ��� �г�Ƽ�� �ΰ��ϰ� �ؽ�Ʈ�� ���� �������� ��ü

  public void OpenReturnButton()
  {
    MoveRectForButton(1);
    if (GameManager.Instance.MyGameData.CurrentSettlement != null)
    {
      UIManager.Instance.SettleButton.Open(0, this);
    }//���������� �̺�Ʈ�� ���� ��� �������� ���ư��� ��ư Ȱ��ȭ
    else
    {
      UIManager.Instance.MapButton.Open(0, this);
    }//�߿ܿ��� �̺�Ʈ�� ���� ��� �߿ܷ� ���ư��� ��ư Ȱ��ȭ
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

    UIManager.Instance.OffBackground();
    StartCoroutine(UIManager.Instance.ChangeAlpha(RewardButtonGroup, 0.0f, 0.3f));

    StartCoroutine(UIManager.Instance.moverect(DescriptionRect, DescriptionOpenPos, DescriptionClosePos, DialogueUIMoveTime, false));
    yield return StartCoroutine(UIManager.Instance.moverect(IllustRect, IllustOpenPos, IllustClosePos, DialogueUIMoveTime, false));
  }
  public void GetEnding()
  {
    GameManager.Instance.SubEnding(GameManager.Instance.ImageHolder.GetEndingIllust(GameManager.Instance.MyGameData.CurrentEvent.EndingID));
  }
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
  public void SetPenalty(FailData _fail)
  {
    switch (_fail.Penelty_target)
    {
      case PenaltyTarget.None:
        break;
      case PenaltyTarget.Status:
        switch (_fail.StatusType)
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
  }//������ ��� �г�Ƽ �ΰ��ϴ� ����
}
