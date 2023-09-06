using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UI_dialogue : UI_default
{
  private float DialogueUIMoveTime = 0.5f;

  [SerializeField] private CanvasGroup NameTextGroup = null;
  [SerializeField] private TextMeshProUGUI NameText = null;
  [SerializeField] private CanvasGroup IllustImageGroup = null;
  [SerializeField] private Image IllustImage = null;
  private float FadeOutTime = 0.8f;
  private float FadeInTime = 1.2f;
  private float FadeWaitTime = 0.2f;
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
  [SerializeField] private UI_RewardExp RewardExpUI = null;
  private EventDataDefulat CurrentEvent
  {
    get { return GameManager.Instance.MyGameData.CurrentEvent; }
  }
  private RectTransform IllustRect { get {return GetPanelRect("illust").Rect; } }
  private Vector2 IllustOpenPos { get { return GetPanelRect("illust").InsidePos; } }
  private Vector2 IllustClosePos { get { return GetPanelRect("illust").OutisdePos; } }
  private RectTransform DescriptionRect { get { return GetPanelRect("Description").Rect; } }
  private Vector2 DescriptionOpenPos { get { return GetPanelRect("Description").InsidePos; } }
  private Vector2 DescriptionClosePos { get { return GetPanelRect("Description").OutisdePos; } }

  private UI_Selection GetUISelection(TendencyTypeEnum _tendencytype,int index)
  {
    switch (_tendencytype)
    {
      case TendencyTypeEnum.None:return Selection_None;
      case TendencyTypeEnum.Body:
        if (index == 0) return Selection_Rational;
        else return Selection_Physical;
      case TendencyTypeEnum.Head:
        if(index==0) return Selection_Mental;
        else return Selection_Material;
      default:return null;
    }
  }
  private UI_Selection GetOppositeSelection(TendencyTypeEnum _tendencytype,int index)
  {
    switch (_tendencytype)
    {
      case TendencyTypeEnum.None: return Selection_None;
      case TendencyTypeEnum.Body:
        if (index == 1) return Selection_Rational;
        else return Selection_Physical;
      case TendencyTypeEnum.Head:
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
    ResetSelectionPos();

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
    CurrentEventPhaseIndex++;

    if (IsBeginning)
    {
      if (CurrentEventPhaseIndex == CurrentEventPhaseMaxIndex)              //������ �ܰ迡 ����
      {
        if (CurrentEventPhaseIndex == 1)     //UI ó�� ���� �ٷ� �������϶�
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
            if (GameManager.Instance.MyGameData.Quest_Wolf_Type == 0) EventIcon.sprite = GameManager.Instance.ImageHolder.QuestIcon_Cult;
            else EventIcon.sprite = GameManager.Instance.ImageHolder.QuestIcon_Wolf;
          }

          SelectionGroup.alpha = 0.0f;
          NextButtonGroup.alpha = 0.0f;
          if (NextButton.gameObject.activeInHierarchy == true) NextButton.gameObject.SetActive(false);
          if (SelectionGroup.gameObject.activeInHierarchy == false) SelectionGroup.gameObject.SetActive(true);
          if (RewardButtonGroup.gameObject.activeInHierarchy == true) RewardButtonGroup.gameObject.SetActive(false);
          StartCoroutine(displayselection());
          //���� ���� ������ ����

          IllustImageGroup.alpha = 1.0f;
          IllustImage.sprite = CurrentEventIllustHolderes[CurrentEventPhaseIndex].CurrentIllust;
          NameText.text = CurrentEvent.Name;
          NameTextGroup.alpha = 1.0f;
          StartCoroutine(UIManager.Instance.moverect(IllustRect, IllustClosePos, IllustOpenPos, DialogueUIMoveTime, UIManager.Instance.UIPanelOpenCurve));
          yield return new WaitForSeconds(0.3f);
          //�Ϸ���Ʈ+�̸� �����صΰ�  �̵�

          DescriptionTextGroup.alpha = 1.0f;
          DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex];
          yield return StartCoroutine(UIManager.Instance.moverect(DescriptionRect, DescriptionClosePos, DescriptionOpenPos, DialogueUIMoveTime, UIManager.Instance.UIPanelOpenCurve));
        }
        else                                 //���� ��ư ������ �������� �����Ҷ�
        {
          NextButton.interactable = false;
          StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 0.0f, FadeOutTime, false));
          StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 0.0f, FadeOutTime, false));
          StartCoroutine(UIManager.Instance.ChangeAlpha(NextButtonGroup,0.0f,FadeOutTime, false));
          yield return new WaitForSeconds(FadeWaitTime);

          NextButton.gameObject.SetActive(false);
          IllustImage.sprite = CurrentEventIllustHolderes[CurrentEventPhaseIndex].CurrentIllust;
          DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex];
          SelectionGroup.gameObject.SetActive(true);

          StartCoroutine(displayselection());
          StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 1.0f, FadeInTime, false));
          StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 1.0f, FadeInTime, false));
          yield return new WaitForSeconds(FadeWaitTime);
        }
      }
      else                                                                 //���� �������� ����
      {
        if (CurrentEventPhaseIndex == 1)     //UI ó�� ���� ���� �������϶�
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
            if (GameManager.Instance.MyGameData.Quest_Wolf_Type == 0) EventIcon.sprite = GameManager.Instance.ImageHolder.QuestIcon_Cult;
            else EventIcon.sprite = GameManager.Instance.ImageHolder.QuestIcon_Wolf;
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
          NameText.text = CurrentEvent.Name;
          NameTextGroup.alpha = 1.0f;
          StartCoroutine(UIManager.Instance.moverect(IllustRect, IllustClosePos, IllustOpenPos, DialogueUIMoveTime, UIManager.Instance.UIPanelOpenCurve));
          yield return new WaitForSeconds(0.3f);
          //�Ϸ���Ʈ+�̸� �����صΰ�  �̵�

          DescriptionTextGroup.alpha = 1.0f;
          DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex];
         yield return  StartCoroutine(UIManager.Instance.moverect(DescriptionRect, DescriptionClosePos, DescriptionOpenPos, DialogueUIMoveTime, UIManager.Instance.UIPanelOpenCurve));
          NextButton.interactable = true;
        }
        else                                 //���� ��ư ������ ���� ���� �����ϱ�
        {
          NextButton.interactable = false;

          StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup,0.0f,FadeOutTime,false));
          StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup,0.0f,FadeOutTime,false));
          yield return new WaitForSeconds(FadeWaitTime);

          IllustImage.sprite = CurrentEventIllustHolderes[CurrentEventPhaseIndex].CurrentIllust;
          DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex];

          StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 1.0f, FadeInTime, false));
          StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 1.0f, FadeInTime, false));
          yield return new WaitForSeconds(FadeWaitTime);

          NextButton.interactable = true;
        }
      }
    }
    else
    {
      if (CurrentEventPhaseIndex == CurrentEventPhaseMaxIndex)             //���� �ܰ迡 ����
      {
        if (CurrentEventPhaseIndex == 1)     //������ ���� �� �ٷ� �����϶�         (������ �ִϸ��̼��� �Ϸ�)
        {
          StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 0.0f, FadeOutTime, false));
          StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 0.0f, FadeOutTime, false));
          yield return new WaitForSeconds(FadeWaitTime);

          IllustImage.sprite = CurrentEventIllustHolderes[CurrentEventPhaseIndex].CurrentIllust;
          DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex];
          SelectionGroup.gameObject.SetActive(false);
          //�����̸� ���� Setactive(true)

          StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 1.0f, FadeInTime, false));
          StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 1.0f, FadeInTime, false));
          //�����̸� ���� ������Ʈ Ȱ��ȭ

          MoveRectForButton(1);
          if (GameManager.Instance.MyGameData.CurrentSettlement != null) UIManager.Instance.SettleButton.Open(0,this);
          else UIManager.Instance.MapButton.Open(0, this);
        }
        else                                 //���� ��ư ������ ���� �����Ҷ�
        {
          StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 0.0f, FadeOutTime, false));
          StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 0.0f, FadeOutTime, false));
          yield return new WaitForSeconds(FadeWaitTime);

          IllustImage.sprite = CurrentEventIllustHolderes[CurrentEventPhaseIndex].CurrentIllust;
          DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex];
          //�����̸� ���� Setactive(true)

          StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 1.0f, FadeInTime, false));
          StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 1.0f, FadeInTime, false));
          //�����̸� ���� ������Ʈ Ȱ��ȭ

          MoveRectForButton(1);
          if (GameManager.Instance.MyGameData.CurrentSettlement != null) UIManager.Instance.SettleButton.Open(0, this);
          else UIManager.Instance.MapButton.Open(0, this);
        }
      }
      else                                                                 //���� �������� ����
      {
        if (CurrentEventPhaseIndex == 1)     //������ ���� �� ���� �������� �Ѿ�� (������ �ִϸ��̼��� �Ϸ�)
        {
          NextButton.interactable = false;

          StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 0.0f, FadeOutTime, false));
          StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 0.0f, FadeOutTime, false));
          yield return new WaitForSeconds(FadeWaitTime);

          IllustImage.sprite = CurrentEventIllustHolderes[CurrentEventPhaseIndex].CurrentIllust;
          DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex];
          SelectionGroup.gameObject.SetActive(false);
          NextButtonGroup.gameObject.SetActive(true);

          StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 1.0f, FadeInTime, false));
          StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 1.0f, FadeInTime, false));
          StartCoroutine(UIManager.Instance.ChangeAlpha(NextButtonGroup,1.0f,FadeInTime,false));
          yield return new WaitForSeconds(FadeWaitTime);
          NextButton.interactable = true;
        }
        else                                 //���� ��ư ������ ���� ���� �����ϱ�
        {
          NextButton.interactable = false;

          StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 0.0f, FadeOutTime, false));
          StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 0.0f, FadeOutTime, false));
          yield return new WaitForSeconds(FadeWaitTime);

          IllustImage.sprite = CurrentEventIllustHolderes[CurrentEventPhaseIndex].CurrentIllust;
          DescriptionText.text = CurrentEventDescriptions[CurrentEventPhaseIndex];

          StartCoroutine(UIManager.Instance.ChangeAlpha(IllustImageGroup, 1.0f, FadeInTime, false));
          StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionTextGroup, 1.0f, FadeInTime, false));
          yield return new WaitForSeconds(FadeWaitTime);

          NextButton.interactable = true;
        }
      }
    }
  }
  private IEnumerator displayselection()
  {
    SelectionGroup.alpha = 0.0f;

    if (NextButton.gameObject.activeInHierarchy == true) NextButton.gameObject.SetActive(false);
    if (SelectionGroup.gameObject.activeInHierarchy == false) SelectionGroup.gameObject.SetActive(true);
    if (RewardButtonGroup.gameObject.activeInHierarchy == true) RewardButtonGroup.gameObject.SetActive(false);

    if (CurrentEvent.Selection_type == SelectionType.Body || CurrentEvent.Selection_type == SelectionType.Head)
      SelectionCenterImgGroup.alpha = 1.0f;
    //�������� ���� �������� ��� ��� ���� �̹��� Ȱ��ȭ

    switch (CurrentEvent.Selection_type)
    {
      case SelectionType.Single:
        Selection_None.Active(CurrentEvent.SelectionDatas[0]);
        break;
      case SelectionType.Body:
        Selection_Rational.Active(CurrentEvent.SelectionDatas[0]);
        Selection_Physical.Active(CurrentEvent.SelectionDatas[1]);
        break;
      case SelectionType.Head:
        Selection_Mental.Active(CurrentEvent.SelectionDatas[0]);
        Selection_Material.Active(CurrentEvent.SelectionDatas[1]);
        break;
    }

    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(SelectionGroup,1.0f,FadeInTime,false));
  }
  private void ResetSelectionPos()
  {
    SelectionGroup.alpha = 0.0f;
    SelectionCenterImgGroup.alpha = 0.0f;

    Selection_None.MyGroup.alpha = 0.0f;
    Selection_None.MyGroup.interactable = false;
    Selection_None.MyGroup.blocksRaycasts = false;
    Selection_None.GetComponent<RectTransform>().anchoredPosition = Selection_None.OriginPos;

    Selection_Rational.MyGroup.alpha = 0.0f;
    Selection_Rational.MyGroup.interactable = false;
    Selection_Rational.MyGroup.blocksRaycasts = false;
    Selection_Rational.GetComponent<RectTransform>().anchoredPosition = Selection_Rational.OriginPos;

    Selection_Physical.MyGroup.alpha = 0.0f;
    Selection_Physical.MyGroup.interactable = false;
    Selection_Physical.MyGroup.blocksRaycasts = false;
    Selection_Physical.GetComponent<RectTransform>().anchoredPosition = Selection_Physical.OriginPos;

    Selection_Mental.MyGroup.alpha = 0.0f;
    Selection_Mental.MyGroup.interactable = false;
    Selection_Mental.MyGroup.blocksRaycasts = false;
    Selection_Mental.GetComponent<RectTransform>().anchoredPosition = Selection_Mental.OriginPos;

    Selection_Material.MyGroup.alpha = 0.0f;
    Selection_Material.MyGroup.interactable = false;
    Selection_Material.MyGroup.blocksRaycasts = false;
    Selection_Material.GetComponent<RectTransform>().anchoredPosition = Selection_Material.OriginPos;
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
      StartCoroutine(UIManager.Instance.ChangeAlpha(SelectionCenterImgGroup, 0.0f, true, false));
    }
    //�ٸ��� ������� �����
    UIManager.Instance.AddUIQueue(selectionanimation(_selection));
    //������ �������� �߽����� �̵���Ű�� ����, ���� �˻� ���� 
  }
  private IEnumerator selectionanimation(UI_Selection _selection)
  {
    CurrentUISelection = _selection;
    if (!_selection.MyTendencyType.Equals(TendencyTypeEnum.None))
    {
      yield return StartCoroutine(_selection.movetocenter());
    }//������ �����Ѵٸ� ����� �̵���Ŵ

    SelectionData _selectiondata = _selection.MySelectionData;
    int _currentvalue = 0, _checkvalue = 0;    //��� üũ���� ���
    int _successpercent = 0;                   //���� Ȯ��(��� Ȥ�� ��� üũ) 
    bool _issuccess = false;  
    int _pluspercent = GameManager.Instance.MyGameData.LibraryEffect ? ConstValues.SectorEffect_Library : 0;
                                               //������ �湮 �� Ȯ�� ���� ��
    switch (_selectiondata.ThisSelectionType)
    {
      case SelectionTargetType.Pay:
        if (_selectiondata.SelectionPayTarget.Equals(StatusType.HP))
        {
          _issuccess = true;
          GameManager.Instance.MyGameData.HP -= GameManager.Instance.MyGameData.PayHPValue_modified;
          UIManager.Instance.UpdateHPText();
        }
        else if (_selectiondata.SelectionPayTarget.Equals(StatusType.Sanity))
        {
          _issuccess = true;//ü��,���ŷ� ������ ��� ���� ���� ��� ���� �ϴ� �������θ� ģ��
          GameManager.Instance.MyGameData.CurrentSanity -= GameManager.Instance.MyGameData.PaySanityValue_modified;
          UIManager.Instance.UpdateSanityText();
        }
        else        //�� ������ ��� �� ���� �� �����ϴ� ���� �־�� ��
        {
          int _paygoldvalue = (int)(GameManager.Instance.MyGameData.PayGoldValue_modified * GameManager.Instance.MyGameData.GetGoldPayModify(true));
          int _goldsuccesspercent = GameManager.Instance.MyGameData.Gold >= _paygoldvalue ? 100 : GameManager.Instance.MyGameData.CheckPercent_money(_paygoldvalue);
          if (_goldsuccesspercent + _pluspercent >= 100)
          {
            if (_goldsuccesspercent < 100 && !_pluspercent.Equals(0)) GameManager.Instance.MyGameData.LibraryEffect = false;
            //��� ȿ���� ������ �޾� ������ ���̶�� ��� ȿ�� ����
            _issuccess = true;
            GameManager.Instance.MyGameData.Gold -= GameManager.Instance.MyGameData.PayGoldValue_modified;
            UIManager.Instance.UpdateGoldText();
            Debug.Log("������ ���� ������ ����~");
          }//100% Ȯ���� ���� ��Ȳ(���� �����ϰų� ���� ����ϰų� �� ��)
          else
          {
            if (Random.Range(0, 100) < _goldsuccesspercent + _pluspercent)
            {
              if (_goldsuccesspercent < 100 && !_pluspercent.Equals(0)) GameManager.Instance.MyGameData.LibraryEffect = false;
              int _elsevalue = GameManager.Instance.MyGameData.PayGoldValue_modified - GameManager.Instance.MyGameData.Gold;
              //��� ȿ���� ������ �޾� ������ ���̶�� ��� ȿ�� ����
              _issuccess = true;
              GameManager.Instance.MyGameData.Gold = 0;
              GameManager.Instance.MyGameData.CurrentSanity -= (int)(_elsevalue * ConstValues.GoldSanityPayAmplifiedValue);
              UIManager.Instance.UpdateGoldText();
            }//���� ������ ������ ���
            else
            {
              _issuccess = false;
              UIManager.Instance.UpdateSanityText();
              UIManager.Instance.UpdateGoldText();
            }//���� ������ ������ ���
          }//���� ������ üũ�� �ؾ� �ϴ� ��Ȳ
        }
        break;
      case SelectionTargetType.Check_Multy: //���(�ܼ�) �������� Ȯ�� �˻�
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
          //��� ȿ���� ������ �޾� ������ ���̶�� ȿ�� ����
        }
        else _issuccess = false;
        break;
      case SelectionTargetType.Check_Single: //���(����) �������� Ȯ�� �˻�
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
          //��� ȿ���� ������ �޾� ������ ���̶�� ȿ�� ����
        }
        else _issuccess = false;
        break;
    }

    if (_issuccess) //�����ϸ� ����
    {
      Debug.Log("������");
      SetSuccess(CurrentEvent.SuccessDatas[_selection.Index]);
      GameManager.Instance.SuccessCurrentEvent(_selection.MyTendencyType, _selection.Index);
    }
    else            //�����ϸ� ����
    {
      Debug.Log("������");
      SetFail(CurrentEvent.FailureDatas[_selection.Index]);
      GameManager.Instance.FailCurrentEvent(_selection.MyTendencyType, _selection.Index);
    }

  }//������ ������ ���� ���θ� ����ϰ� �ִϸ��̼��� �����Ű�� �ڷ�ƾ
  //�� �ڷ�ƾ���� SetSuccess �ƴϸ� SetFail�� �ٷ� �Ѿ

  private SuccessData CurrentSuccessData = null;
  public bool RemainReward = false;
  public void SetSuccess(SuccessData _success)
  {
    CurrentSuccessData = _success;
    RemainReward = true;
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
        _description = GameManager.Instance.ExpDic[CurrentSuccessData.Reward_EXPID].Name;

        break;
      case RewardTarget.Skill:
        _icon = GameManager.Instance.ImageHolder.GetSkillIcon(CurrentSuccessData.Reward_SkillType);
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
    //���� �̺�Ʈ��, ���� ������ �� �ִ� ���¿��� ������ ��� ���� ���̾�α� ����
  }//������ ��� ���� ���� �����ϰ� �ؽ�Ʈ�� ���� �������� ��ü, ����Ʈ �̺�Ʈ�� ��� ���൵ ++

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
  public override void CloseForGameover()
  {
    StopAllCoroutines();

    IsOpen = false;
    CurrentSuccessData = null;
    CurrentFailData = null;
    EndingGroup.alpha = 0.0f;
    EndingGroup.interactable = false;
    EndingGroup.blocksRaycasts = false;

    StartCoroutine(UIManager.Instance.ChangeAlpha(RewardButtonGroup, 0.0f, 0.3f, false));

    StartCoroutine(UIManager.Instance.moverect(DescriptionRect, DescriptionRect.anchoredPosition, DescriptionClosePos, DialogueUIMoveTime, UIManager.Instance.UIPanelCLoseCurve));

    StartCoroutine(UIManager.Instance.moverect(IllustRect, IllustRect.anchoredPosition, IllustClosePos, DialogueUIMoveTime, UIManager.Instance.UIPanelCLoseCurve));
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

    StartCoroutine(UIManager.Instance.ChangeAlpha(RewardButtonGroup, 0.0f, 0.3f, false));

    StartCoroutine(UIManager.Instance.moverect(DescriptionRect, DescriptionOpenPos, DescriptionClosePos, DialogueUIMoveTime, UIManager.Instance.UIPanelCLoseCurve));
    yield return new WaitForSeconds(0.3f);
    DescriptionTextGroup.alpha = 0.0f;

    yield return StartCoroutine(UIManager.Instance.moverect(IllustRect, IllustOpenPos, IllustClosePos, DialogueUIMoveTime, UIManager.Instance.UIPanelCLoseCurve));
    IllustImageGroup.alpha = 0.0f;
    NameText.text = "";
  }

  #region ����?
  public void SetEndingDialogue(FollowEndingData endingdata, SuccessData successdata)
  {
  }

  private IEnumerator openendingbuttons()
  {
    StartCoroutine(UIManager.Instance.ChangeAlpha(EndingGroup, 1.0f, 0.2f, false));
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
      if (CurrentSuccessData.Reward_Target == RewardTarget.Experience)
      {
        if (GameManager.Instance.MyGameData.AvailableExpSlot == false)
        {
          GameManager.Instance.MyGameData.CurrentSanity += ConstValues.GoodExpAsSanity;
          UIManager.Instance.UpdateSanityText();

          StartCoroutine(UIManager.Instance.ChangeAlpha(RewardButtonGroup, 0.0f, 0.6f, false));
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
            GameManager.Instance.MyGameData.GetSkill(CurrentSuccessData.Reward_SkillType).LevelByDefault++;
            break;
        }

        StartCoroutine(UIManager.Instance.ChangeAlpha(RewardButtonGroup, 0.0f, 0.6f, false));
        RemainReward = false;
      }
    }
    else if (CurrentFailData != null)
    {
      if (GameManager.Instance.MyGameData.AvailableExpSlot == false)
      {
        GameManager.Instance.MyGameData.CurrentSanity -= ConstValues.BadExpAsSanity;
        UIManager.Instance.UpdateSanityText();

        StartCoroutine(UIManager.Instance.ChangeAlpha(RewardButtonGroup, 0.0f, 0.6f, false));
        RemainReward = false;
      }
      else
      RewardExpUI.OpenUI_Penalty(GameManager.Instance.ExpDic[CurrentFailData.ExpID]);
    }
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
        Sprite _icon = GameManager.Instance.ImageHolder.UnknownExp;
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

        StartCoroutine(UIManager.Instance.ChangeAlpha(RewardButtonGroup, 1.0f, false, false));

        break;
    }
  }//������ ��� �г�Ƽ �ΰ��ϴ� ����
}
