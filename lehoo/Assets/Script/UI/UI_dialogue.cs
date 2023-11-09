using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class UI_dialogue : UI_default
{
  public float OpenTime = 1.5f;
  public float CloseTime = 1.0f;
  public RectTransform DialogueRect = null;

  public Vector2 EventDialogueSize = new Vector2(475.0f, 480.0f);
  public Vector2 SettlementDialogueSize = new Vector2(475.0f, 780.0f);
  public Vector2 LeftPos = new Vector2(-1350.0f, 0.0f);
  public Vector2 CenterPos = Vector2.zero;
  public Vector2 RightPos = new Vector2(1250.0f, 0.0f);
  public Image SettlementBackground = null;
  [Header("�̺�Ʈ")]
  #region �̺�Ʈ
  public GameObject EventObjectHolder = null;
  [Space(10)]
  [SerializeField] private RectTransform IllustRect = null;
  [SerializeField] private ImageSwapScript Illust = null;
  public float FadeTime = 0.8f;
  [SerializeField] private Image IllustEffect_Image = null;
  [SerializeField] private CanvasGroup IllustEffect_Group = null;
  [SerializeField] private Color SuccessColor = Color.white;
  [SerializeField] private Color FailColor = Color.red;

  [SerializeField] private TextMeshProUGUI NameText = null;
  [SerializeField] private float ButtonFadeinTime = 0.4f;
  [SerializeField] private TextMeshProUGUI DescriptionText = null;
  public Scrollbar DescriptionScrollBar = null;
  public AnimationCurve ScrollbarCurve = new AnimationCurve();
  private IEnumerator updatescrollbar()
  {
    yield return new WaitForSeconds(0.05f);

    float _time = 0.0f;
    while (DescriptionScrollBar.value > 0.001f && _time < ConstValues.ScrollTime)
    {
      DescriptionScrollBar.value = Mathf.Lerp(DescriptionScrollBar.value, 0.0f, ConstValues.ScrollSpeed);
      _time += Time.deltaTime;
      yield return null;

    }
    DescriptionScrollBar.value = 0.0f;
  }
  [SerializeField] private CanvasGroup NextButtonGroup = null;
  private void SetNextButtonDisable()
  {
    NextButtonGroup.interactable = false;
  }
  public void SetNextButtonActive()
  {
    NextButtonGroup.interactable = true;
  }
  [SerializeField] private CanvasGroup RewardButtonGroup = null;
  [SerializeField] private Image RewardIcon = null;
  [SerializeField] private TextMeshProUGUI RewardDescription = null;
  [SerializeField] private UI_RewardExp RewardExpUI = null;
  [SerializeField] private RectTransform MapbuttonPos_Event = null;
  [SerializeField] private CanvasGroup EndingButtonGroup = null;
  [SerializeField] private TextMeshProUGUI EndingButtonText = null;
  [SerializeField] private CanvasGroup EndingRefuseGroup = null;
  [SerializeField] private TextMeshProUGUI EndingRefuseText = null;
  [SerializeField] private CanvasGroup SelectionGroup = null;
  [SerializeField] private UI_Selection Selection_A = null;
  [SerializeField] private UI_Selection Selection_B = null;

  private EventData CurrentEvent
  {
    get { return GameManager.Instance.MyGameData.CurrentEvent; }
  }

  private UI_Selection GetOppositeSelection(UI_Selection _selection)
  {
    if (_selection == Selection_A) return Selection_B;
    return Selection_A;
  }
  public IEnumerator OpenEventUI(bool dir)
  {
    if (MadenssEffect.enabled) MadenssEffect.enabled = false;
    IsOpen = true;

    UIManager.Instance.SettleButton.DeActive();
    UIManager.Instance.MapButton.DeActive();
    if (EventObjectHolder.activeInHierarchy == false) EventObjectHolder.SetActive(true);
    if (SettlementObjectHolder.activeInHierarchy == true) SettlementObjectHolder.SetActive(false);
    if(SettlementBackground.enabled==true) SettlementBackground.enabled = false;
    DialogueRect.sizeDelta = EventDialogueSize;

    if(CurrentEvent.AppearSpace==EventAppearType.Outer) UIManager.Instance.UpdateBackground(CurrentEvent.EnvironmentType);
  
    EndingButtonGroup.alpha = 0.0f;
    EndingButtonGroup.interactable = false;
    EndingButtonGroup.blocksRaycasts = false;
    EndingRefuseGroup.alpha = 0.0f;
    EndingRefuseGroup.interactable = false;
    EndingRefuseGroup.blocksRaycasts = false;
    NextButtonGroup.alpha = 0.0f;
    NextButtonGroup.interactable = false;
    RewardButtonGroup.alpha = 0.0f;
    RewardButtonGroup.interactable = false;
    SelectionGroup.alpha = 0.0f;
    SelectionGroup.interactable = false;

    CurrentEventIllustHolderes = CurrentEvent.BeginningIllusts;
    CurrentEventDescriptions = CurrentEvent.BeginningDescriptions;
    IsBeginning = true;
    CurrentEventPhaseIndex = 0;
    CurrentEventPhaseMaxIndex = CurrentEventIllustHolderes.Count;

    NameText.text = CurrentEvent.Name;
    DescriptionText.text = "";

    DefaultGroup.interactable = false;
    StartCoroutine(displaynextindex(true));

    Vector2 _startpos = dir ? LeftPos : RightPos;
    yield return StartCoroutine(UIManager.Instance.moverect(DefaultRect, _startpos, CenterPos, OpenTime, UIManager.Instance.UIPanelOpenCurve));

    DefaultGroup.interactable = true;
    yield return null;
  }
  public IEnumerator OpenEventUI(bool issuccess,bool isleft,bool dir)
  {
    if (MadenssEffect.enabled) MadenssEffect.enabled = false;
    IsOpen = true;
    if (EventObjectHolder.activeInHierarchy == false) EventObjectHolder.SetActive(true);
    if (SettlementObjectHolder.activeInHierarchy == true) SettlementObjectHolder.SetActive(false);
    if (SettlementBackground.enabled == true) SettlementBackground.enabled = false;
    UIManager.Instance.SettleButton.DeActive();
    DialogueRect.sizeDelta = EventDialogueSize;

    if (CurrentEvent.AppearSpace == EventAppearType.Outer) UIManager.Instance.UpdateBackground(CurrentEvent.EnvironmentType);

    EndingButtonGroup.alpha = 0.0f;
    EndingButtonGroup.interactable = false;
    EndingButtonGroup.blocksRaycasts = false;
    EndingRefuseGroup.alpha = 0.0f;
    EndingRefuseGroup.interactable = false;
    EndingRefuseGroup.blocksRaycasts = false;
    NextButtonGroup.alpha = 0.0f;
    NextButtonGroup.interactable = false;
    RewardButtonGroup.alpha = 0.0f;
    RewardButtonGroup.interactable = false;
    SelectionGroup.alpha = 0.0f;
    SelectionGroup.interactable = false;

    NameText.text = CurrentEvent.Name;
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

    DefaultGroup.interactable = false;
    Illust.Setup(CurrentEventIllustHolderes[0].CurrentIllust);
    StartCoroutine(displaynextindex(true));

    Vector2 _startpos = dir ? LeftPos : RightPos;
    yield return StartCoroutine(UIManager.Instance.moverect(DefaultRect, _startpos, CenterPos, OpenTime, UIManager.Instance.UIPanelOpenCurve));

    DefaultGroup.interactable = true;
    yield return null;
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
  private IEnumerator displaynextindex(bool dir)
  {
    SetNextButtonDisable();

    int _phasetype = 0;
    if (IsBeginning)
    {
      if (CurrentEventPhaseIndex == CurrentEventPhaseMaxIndex-1)              //������ �ܰ迡 ����
      {
        if (CurrentEventPhaseIndex == 0)     //UI ó�� ���� �ٷ� �������϶�
        {
          _phasetype = 3;
        }
        else                                 //���� ��ư ������ �������� �����Ҷ�
        {
          _phasetype = 2;
        }
      }
      else                                                                 //���� �������� ����
      {
        if (CurrentEventPhaseIndex == 0)     //UI ó�� ���� ���� �������϶�
        {
          _phasetype = 0;
        }
        else                                 //���� ��ư ������ ���� ���� �����ϱ�
        {
          _phasetype = 1;
        }
      }

      if(CurrentEventPhaseIndex==0) Illust.Setup(CurrentEventIllustHolderes[CurrentEventPhaseIndex].CurrentIllust, 0.1f);
      else Illust.Next(CurrentEventIllustHolderes[CurrentEventPhaseIndex].CurrentIllust, FadeTime);
      yield return new WaitForSeconds(FadeTime);
      DescriptionText.text += "<br><br>" + CurrentEventDescriptions[CurrentEventPhaseIndex];
      LayoutRebuilder.ForceRebuildLayoutImmediate(DescriptionText.transform.parent.transform as RectTransform);

      switch (_phasetype)
      {
        case 0:
          if (NextButtonGroup.alpha == 0.0f) StartCoroutine(UIManager.Instance.ChangeAlpha(NextButtonGroup, 1.0f, FadeTime));

          yield return StartCoroutine(updatescrollbar());

          SetNextButtonActive();
          break;
        case 1:
          if (NextButtonGroup.alpha == 0.0f) StartCoroutine(UIManager.Instance.ChangeAlpha(NextButtonGroup, 1.0f, FadeTime));

          yield return StartCoroutine(updatescrollbar());

          SetNextButtonActive();
          break;
        case 2:
          if (NextButtonGroup.alpha == 1.0f) StartCoroutine(UIManager.Instance.ChangeAlpha(NextButtonGroup, 0.0f, 0.5f));

          StartCoroutine(setupselections());
          yield return StartCoroutine(updatescrollbar());

          break;
        case 3:
          StartCoroutine(setupselections());
          yield return StartCoroutine(updatescrollbar());

          break;
      }

    }
    else
    {
      if (CurrentEventPhaseIndex == CurrentEventPhaseMaxIndex-1)             //���� �ܰ迡 ����
      {
        if (CurrentEventPhaseIndex == 0)     //������ ���� �� �ٷ� �����϶�         (������ �ִϸ��̼��� �Ϸ�)
        {
          _phasetype = 3;
        }
        else                                 //���� ��ư ������ ���� �����Ҷ�
        {
          _phasetype = 2;
        }
      }
      else                                                                 //���� �������� ����
      {
        if (CurrentEventPhaseIndex == 0)     //������ ���� �� ���� �������� �Ѿ�� 
        {
          _phasetype = 0;
        }
        else                                 //���� ��ư ������ ���� ���� �����ϱ�
        {
          _phasetype = 1;
        }
      }

      if (SelectionGroup.alpha == 1.0f) StartCoroutine(UIManager.Instance.ChangeAlpha(SelectionGroup, 0.0f, 0.5f));

      Illust.Next(CurrentEventIllustHolderes[CurrentEventPhaseIndex].CurrentIllust, FadeTime);
      yield return new WaitForSeconds(FadeTime);

      if (CurrentEventDescriptions[CurrentEventPhaseIndex].Contains("#Penalty#"))
      {
        DescriptionText.text += "<br><br>" + CurrentEventDescriptions[CurrentEventPhaseIndex].Replace("#Penalty#","");
        SetPenalty();
        if (CurrentFailData != null)
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
                  DescriptionText.text += WNCText.SetSize(30, $"<br><br>{GameManager.Instance.GetTextData(StatusTypeEnum.HP, 2)} {-1 * PenaltyValue}");
                  break;
                case StatusTypeEnum.Sanity:
                  DescriptionText.text += WNCText.SetSize(30, $"<br><br>{GameManager.Instance.GetTextData(StatusTypeEnum.Sanity, 2)} {-1 * PenaltyValue}");
                  break;
                case StatusTypeEnum.Gold:
                  DescriptionText.text += WNCText.SetSize(30, $"<br><br>{GameManager.Instance.GetTextData(StatusTypeEnum.Gold, 2)} {-1 * PenaltyValue}");
                  break;
              }
              break;
          }
        }
      }
      else
      {
        DescriptionText.text += "<br><br>" + CurrentEventDescriptions[CurrentEventPhaseIndex];
      }
      LayoutRebuilder.ForceRebuildLayoutImmediate(DescriptionText.transform.parent.transform as RectTransform);


      switch (_phasetype)
      {
        case 0:
          if (NextButtonGroup.alpha == 0.0f) StartCoroutine(UIManager.Instance.ChangeAlpha(NextButtonGroup, 1.0f, FadeTime));

          yield return StartCoroutine(updatescrollbar());
          SetNextButtonActive();
          break;
        case 1:

          yield return StartCoroutine(updatescrollbar());
          SetNextButtonActive();
          break;
        case 2:
          if (NextButtonGroup.alpha == 1.0f) StartCoroutine(UIManager.Instance.ChangeAlpha(NextButtonGroup,0.0f, 0.5f));

          if (CurrentSuccessData != null && CurrentEvent.EndingID != "")
          {
            CurrentEndingData = GameManager.Instance.ImageHolder.GetEndingData(CurrentEvent.EndingID);
            EndingButtonGroup.alpha = 0.0f;
            EndingButtonText.text = string.Format(GameManager.Instance.GetTextData("Ending"), CurrentEndingData.Name);
            EndingRefuseText.text = CurrentEndingData.Refuse_Name;
            LayoutRebuilder.ForceRebuildLayoutImmediate(EndingButtonGroup.transform as RectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(EndingRefuseGroup.transform as RectTransform);

            StartCoroutine(UIManager.Instance.ChangeAlpha(EndingButtonGroup, 1.0f, FadeTime));
            StartCoroutine(UIManager.Instance.ChangeAlpha(EndingRefuseGroup, 1.0f, FadeTime));
            yield return StartCoroutine(updatescrollbar());
          }
          else
          {
            yield return StartCoroutine(updatescrollbar());

            if (CurrentSuccessData != null) SetRewardButton();
            OpenReturnButton();
          }
          break;
        case 3:
          if (CurrentSuccessData != null && CurrentEvent.EndingID != "")
          {
            CurrentEndingData = GameManager.Instance.ImageHolder.GetEndingData(CurrentEvent.EndingID);
            EndingButtonGroup.alpha = 0.0f;
            EndingButtonText.text = string.Format(GameManager.Instance.GetTextData("Ending"), CurrentEndingData.Name);
            EndingRefuseText.text = CurrentEndingData.Refuse_Name;
            LayoutRebuilder.ForceRebuildLayoutImmediate(EndingButtonGroup.transform as RectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(EndingRefuseGroup.transform as RectTransform);

            StartCoroutine(UIManager.Instance.ChangeAlpha(EndingButtonGroup, 1.0f, FadeTime));
            StartCoroutine(UIManager.Instance.ChangeAlpha(EndingRefuseGroup, 1.0f, FadeTime));
            yield return StartCoroutine(updatescrollbar());
          }
          else
          {
            yield return StartCoroutine(updatescrollbar());

            if (CurrentSuccessData != null) SetRewardButton();
            OpenReturnButton();
          }
          break;
      }
    }
    CurrentEventPhaseIndex++;
  }
  public void RefuseEnding()
  {
    if (UIManager.Instance.IsWorking) return;
    UIManager.Instance.AddUIQueue(refuseending());
  }
  private IEnumerator refuseending()
  {
    StartCoroutine(UIManager.Instance.ChangeAlpha(EndingButtonGroup, 0.0f, 0.5f));
    StartCoroutine(UIManager.Instance.ChangeAlpha(EndingRefuseGroup, 0.0f, 0.5f));

    Illust.Next(CurrentEndingData.RefuseIllust,FadeTime);
    DescriptionText.text += "<br><br>" + CurrentEndingData.Refuse_Description;
    LayoutRebuilder.ForceRebuildLayoutImmediate(DescriptionText.transform.parent.transform as RectTransform);
    yield return StartCoroutine(updatescrollbar());

    SetRewardButton();
    OpenReturnButton();
  }
  private IEnumerator setupselections()
  {
    SelectionGroup.alpha = 0.0f;

    NextButtonGroup.interactable = false;

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
    int _percentvalue = UnityEngine.Random.Range(1,101);
    int _requirevalue = 0;                   //���� Ȯ��(��� Ȥ�� ��� üũ) 
    bool _issuccess = false;  
                                               
    switch (_selectiondata.ThisSelectionType)
    {
      case SelectionTargetType.None:
        _issuccess = true;
        UIManager.Instance.AudioManager.PlaySFX(25);
        break;
      case SelectionTargetType.Pay:
        UIManager.Instance.AudioManager.PlaySFX(2);
        if (_selectiondata.SelectionPayTarget.Equals(StatusTypeEnum.HP))
        {
          _payvalue = GameManager.Instance.MyGameData.PayHPValue;
       //   yield return StartCoroutine(UIManager.Instance.SetIconEffect(true, StatusTypeEnum.HP, _selection.PayIcon.transform as RectTransform));
          yield return StartCoroutine(payanimation(_selection.PayIcon, _payvalue, 0, _selection.PayInfo));

          _issuccess = true;
          UIManager.Instance.AudioManager.PlaySFX(25);
          GameManager.Instance.MyGameData.HP -= GameManager.Instance.MyGameData.PayHPValue;
        }
        else if (_selectiondata.SelectionPayTarget.Equals(StatusTypeEnum.Sanity))
        {
          _payvalue = GameManager.Instance.MyGameData.PaySanityValue;
       //   yield return StartCoroutine(UIManager.Instance.SetIconEffect(true, StatusTypeEnum.Sanity, _selection.PayIcon.transform as RectTransform));
          yield return StartCoroutine(payanimation(_selection.PayIcon, _payvalue, 0, _selection.PayInfo));

          _issuccess = true;//ü��,���ŷ� ������ ��� ���� ���� ��� ���� �ϴ� �������θ� ģ��
          UIManager.Instance.AudioManager.PlaySFX(25);
          GameManager.Instance.MyGameData.Sanity -= GameManager.Instance.MyGameData.PaySanityValue;
        }
        else        //�� ������ ��� �� ���� �� �����ϴ� ���� �־�� ��
        {
          _payvalue = GameManager.Instance.MyGameData.PayGoldValue;
          int _goldsuccesspercent = GameManager.Instance.MyGameData.Gold >= _payvalue ? 100 : GameManager.Instance.MyGameData.RequireValue_Money(_payvalue);

          if (GameManager.Instance.MyGameData.Gold >= _payvalue)
          {
            //    yield return StartCoroutine(UIManager.Instance.SetIconEffect(true, StatusTypeEnum.Gold, _selection.PayIcon.transform as RectTransform));
            yield return StartCoroutine(payanimation(_selection.PayIcon, _payvalue, 0, _selection.PayInfo));

            GameManager.Instance.MyGameData.Gold -= _payvalue;
            UIManager.Instance.AudioManager.PlaySFX(25);
            _issuccess = true;
          }
          else
          {
            if (_percentvalue >= _goldsuccesspercent)
            {
              int _elsevalue = GameManager.Instance.MyGameData.PayGoldValue - GameManager.Instance.MyGameData.Gold;

           //   StartCoroutine(UIManager.Instance.SetIconEffect(true, StatusTypeEnum.Gold, _selection.PayIcon.transform as RectTransform));
            //  StartCoroutine(UIManager.Instance.SetIconEffect(true, StatusTypeEnum.Sanity, _selection.PayIcon.transform as RectTransform));
              yield return StartCoroutine(payanimation(_selection.PayIcon,_payvalue, 0, _selection.PayInfo));

              _issuccess = true;
              UIManager.Instance.AudioManager.PlaySFX(25);
              GameManager.Instance.MyGameData.Gold = 0;
              GameManager.Instance.MyGameData.Sanity -= (int)(_elsevalue * ConstValues.GoldSanityPayAmplifiedValue);
              Debug.Log("������ ���� ������ ����~");
            }//���� ������ ������ ���
            else
            {
              yield return StartCoroutine(payanimation(_selection.PayIcon,  _payvalue,_payvalue - GameManager.Instance.MyGameData.Gold, _selection.PayInfo));

              _issuccess = false;
              UIManager.Instance.AudioManager.PlaySFX(26);
            }//���� ������ ������ ���
          }
        }

        break;
      case SelectionTargetType.Check_Single: //���(�ܼ�) �������� Ȯ�� �˻�
        UIManager.Instance.AudioManager.PlaySFX(2);

        _currentvalue = GameManager.Instance.MyGameData.GetSkill(_selectiondata.SelectionCheckSkill[0]).Level;
        _checkvalue = GameManager.Instance.MyGameData.CheckSkillSingleValue;
        _requirevalue = GameManager.Instance.MyGameData.RequireValue_SkillCheck(_currentvalue, _checkvalue);
        if (_percentvalue >= _requirevalue)
        {
          _issuccess = true;
        }
        else
        {
          _issuccess = false;
        }

        yield return StartCoroutine(checkanimation(_selection.SkillIcon_A,Mathf.Clamp(_percentvalue / (float)_requirevalue, 0.0f, 1.0f)));
        yield return new WaitForSeconds(0.5f);
        break;
      case SelectionTargetType.Check_Multy: //���(����) �������� Ȯ�� �˻�
        UIManager.Instance.AudioManager.PlaySFX(2);
        
        _currentvalue = GameManager.Instance.MyGameData.GetSkill(_selectiondata.SelectionCheckSkill[0]).Level +
          GameManager.Instance.MyGameData.GetSkill(_selectiondata.SelectionCheckSkill[1]).Level;
        _checkvalue = GameManager.Instance.MyGameData.CheckSkillMultyValue;
        _requirevalue = GameManager.Instance.MyGameData.RequireValue_SkillCheck(_currentvalue, _checkvalue);
        if (_percentvalue >= _requirevalue)
        {
          _issuccess = true;
        }
        else
        {
          _issuccess = false;
        }
        yield return StartCoroutine(checkanimation(_selection.SkillIcon_A, _selection.SkillIcon_B,Mathf.Clamp(_percentvalue / (float)_requirevalue, 0.0f, 1.0f)));
        yield return new WaitForSeconds(0.5f);
        break;
    }

    if (_issuccess) //�����ϸ� ����
    {
      Debug.Log("������");
      SetSuccess(CurrentEvent.SelectionDatas[_selection.Index].SuccessData);
      GameManager.Instance.SuccessCurrentEvent(_selection.MyTendencyType, _selection.IsLeft);
    }
    else            //�����ϸ� ����
    {
      Debug.Log("������");
      SetFail(CurrentEvent.SelectionDatas[_selection.Index].FailData);
      GameManager.Instance.FailCurrentEvent(_selection.MyTendencyType, _selection.IsLeft);
    }
    _selection.DeActive();

    GameManager.Instance.SaveData();

    yield return null;
  }//������ ������ ���� ���θ� ����ϰ� �ִϸ��̼��� �����Ű�� �ڷ�ƾ
  //�� �ڷ�ƾ���� SetSuccess �ƴϸ� SetFail�� �ٷ� �Ѿ
  [SerializeField] private float SelectionEffectTime_check = 3.5f;
  [SerializeField] private float SelectionEffectTime_pay = 1.0f;
  [SerializeField] private AnimationCurve SelectionCheckCurve = null;
  private IEnumerator payanimation(Image image, int payvalue, int targetvalue, TextMeshProUGUI tmp)
  {
    float _time = 0.0f;
    string _str = "";
    float _stoptime = SelectionEffectTime_pay * (1.0f - targetvalue / (float)payvalue);

    while (_time< _stoptime)
    {
      image.fillAmount = 1.0f - _time / SelectionEffectTime_pay;
      _str = ((int)Mathf.Lerp(payvalue, targetvalue, _time / SelectionEffectTime_pay)).ToString();
      tmp.text = _str;
      _time += Time.deltaTime;
      yield return null;
    }
    _str = targetvalue.ToString();
    tmp.text= _str;
    yield return new WaitForSeconds(0.5f);
  }
  private IEnumerator checkanimation(Image image,float successvalue)
  {
  //  Debug.Log(successvalue);
    float _time = 0.0f;
    while(_time< SelectionEffectTime_check * successvalue)
    {
      image.fillAmount=Mathf.Lerp(1.0f,0.0f, SelectionCheckCurve.Evaluate(_time/ SelectionEffectTime_check));
      _time += Time.deltaTime;yield return null;
    }
    if (successvalue == 1.0f) { UIManager.Instance.AudioManager.PlaySFX(25); }
    else { UIManager.Instance.AudioManager.PlaySFX(26); }

    yield return new WaitForSeconds(0.5f);
  }
  private IEnumerator checkanimation(Image image_L,Image image_R,float successvalue)
  {
    float _time = 0.0f;
    float _firstvalue = Mathf.Clamp(successvalue * 2.0f, 0.0f, 1.0f), _secondvalue = (successvalue - 0.5f) * 2.0f;

    while (_time < (SelectionEffectTime_check/2)* _firstvalue)
    {
      image_L.fillAmount = Mathf.Lerp(1.0f, 0.0f, SelectionCheckCurve.Evaluate(_time / (SelectionEffectTime_check / 2)));
      _time += Time.deltaTime; yield return null;
    }
    if (_firstvalue == 1.0f) { UIManager.Instance.AudioManager.PlaySFX(25); _time = 0.0f; }
    else { UIManager.Instance.AudioManager.PlaySFX(26); yield return new WaitForSeconds(0.5f); yield break; }


    while (_time < (SelectionEffectTime_check / 2) *_secondvalue)
    {
      image_R.fillAmount = Mathf.Lerp(1.0f, 0.0f, SelectionCheckCurve.Evaluate(_time / (SelectionEffectTime_check / 2)));
      _time += Time.deltaTime;yield return null;
    }
    if (_secondvalue == 1.0f) { UIManager.Instance.AudioManager.PlaySFX(25);  }
    else { UIManager.Instance.AudioManager.PlaySFX(26); }

    yield return new WaitForSeconds(0.5f);
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

    IllustEffect_Image.color = SuccessColor;
    StartCoroutine (UIManager.Instance.ChangeAlpha(IllustEffect_Group, 0.0f, ResultEffectTime));

      UIManager.Instance.AddUIQueue(displaynextindex(true));

    //���� �̺�Ʈ��, ���� ������ �� �ִ� ���¿��� ������ ��� ���� ���̾�α� ����
  }//������ ��� ���� ���� �����ϰ� �ؽ�Ʈ�� ���� �������� ��ü, ����Ʈ �̺�Ʈ�� ��� ���൵ ++
  public EndingDatas CurrentEndingData = null;
  private void SetRewardButton()
  {
    if (CurrentSuccessData.Reward_Type == RewardTypeEnum.None) return;

    RewardButtonGroup.alpha = 0.0f;

  //  Reward_Highlight.RemoveAllCall();
   // Reward_Highlight.Interactive = true;
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
        //    Reward_Highlight.SetInfo(HighlightEffectEnum.HP, GameManager.Instance.MyGameData.RewardHPValue);
            break;
          case StatusTypeEnum.Sanity:
            _icon = GameManager.Instance.ImageHolder.SanityIcon;
            _description = $"+{WNCText.GetSanityColor(GameManager.Instance.MyGameData.RewardSanityValue)}";
      //      Reward_Highlight.SetInfo(HighlightEffectEnum.Sanity, GameManager.Instance.MyGameData.RewardSanityValue);
            break;
          case StatusTypeEnum.Gold:
            _icon = GameManager.Instance.ImageHolder.GoldIcon;
            _description = $"+{WNCText.GetGoldColor(GameManager.Instance.MyGameData.RewardGoldValue)}";
        //    Reward_Highlight.SetInfo(HighlightEffectEnum.Gold, GameManager.Instance.MyGameData.RewardGoldValue);
            break;
        }
        break;
      case RewardTypeEnum.Experience:
        _icon = GameManager.Instance.ImageHolder.UnknownExpRewardIcon;
        //   _name = GameManager.Instance.GetTextData("EXP_NAME");
        _description = GameManager.Instance.ExpDic[CurrentSuccessData.Reward_EXPID].Name;
      //  Reward_Highlight.SetInfo(HighlightEffectEnum.Exp);
        break;
      case RewardTypeEnum.Skill:
        _icon = GameManager.Instance.ImageHolder.GetSkillIcon(CurrentSuccessData.Reward_SkillType, false);
        _description = $"{GameManager.Instance.GetTextData(CurrentSuccessData.Reward_SkillType, 0)} +1";
     //   Reward_Highlight.SetInfo(new List<SkillTypeEnum> { CurrentSuccessData.Reward_SkillType });
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

    StartCoroutine(shakeillust());

    IllustEffect_Image.color = FailColor;
    StartCoroutine((UIManager.Instance.ChangeAlpha(IllustEffect_Group, 0.0f, ResultEffectTime)));

    UIManager.Instance.AddUIQueue(displaynextindex(true));
  }//������ ��� �г�Ƽ�� �ΰ��ϰ� �ؽ�Ʈ�� ���� �������� ��ü
  [SerializeField] private float IllustShakeDegree = 10;
  [SerializeField] private float IllustShakeTime = 0.7f;
  [SerializeField] private float IllustShakePeriod = 0.1f;
  [SerializeField] private float IllustRotateDegree = 2.5f;
  private IEnumerator shakeillust()
  {
    Vector2 _originpos = IllustRect.anchoredPosition;
    float _time = 0.0f;
    while (_time < IllustShakeTime)
    {
      IllustRect.anchoredPosition = _originpos + new Vector2(Random.Range(-IllustShakeDegree, IllustShakeDegree), Random.Range(-IllustShakeDegree, IllustShakeDegree));
      IllustRect.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, Random.Range(-IllustRotateDegree, IllustRotateDegree)));

      yield return new WaitForSeconds(IllustShakePeriod);
      _time += IllustShakePeriod;
      yield return null;
    }
    IllustRect.anchoredPosition = _originpos;
    IllustRect.rotation = Quaternion.Euler(Vector3.zero);
  }
  private void Update()
  {
    if(Input.GetKeyDown(KeyCode.KeypadPlus)) StartCoroutine(shakeillust());

  }
  public void OpenReturnButton()
  {
    if (GameManager.Instance.MyGameData.CurrentSettlement != null)
    {
      UIManager.Instance.SettleButton.SetCurrentUI(this, MapbuttonPos_Event,0.0f);
    }//���������� �̺�Ʈ�� ���� ��� �������� ���ư��� ��ư Ȱ��ȭ
    else
    {
      UIManager.Instance.MapButton.SetCurrentUI(this, MapbuttonPos_Event,0.0f);
    }//�߿ܿ��� �̺�Ʈ�� ���� ��� �߿ܷ� ���ư��� ��ư Ȱ��ȭ
  }
  /// <summary>
  /// true:�������� false:����������
  /// </summary>
  /// <param name="dir"></param>
  public void CloseUI(bool dir)
  {
    UIManager.Instance.AddUIQueue(closeui_all(dir));
  }
  private IEnumerator closeui_all(bool dir)
  {
    IsOpen = false;

    CurrentSuccessData = null;
    CurrentFailData = null;
    
    UIManager.Instance.SidePanelCultUI.SetSabbatEffect(false);


    if (RewardButtonGroup.alpha==1.0f) StartCoroutine(UIManager.Instance.ChangeAlpha(RewardButtonGroup, 0.0f, 0.3f));

    if (dir == true)
    {
      yield return StartCoroutine(UIManager.Instance.moverect(DefaultRect, DefaultRect.anchoredPosition, LeftPos, CloseTime, UIManager.Instance.UIPanelOpenCurve));
    }
    else
    {
      yield return StartCoroutine(UIManager.Instance.moverect(DefaultRect, DefaultRect.anchoredPosition, RightPos, CloseTime, UIManager.Instance.UIPanelOpenCurve));
    }

  }
  public void GetEnding()
  {
    GameManager.Instance.SubEnding(GameManager.Instance.ImageHolder.GetEndingData(GameManager.Instance.MyGameData.CurrentEvent.EndingID));
  }
  public void GetReward()
  {
    if (UIManager.Instance.IsWorking) return;
    UIManager.Instance.AddUIQueue(getreward());
  }
  private IEnumerator getreward()
  {
 //   Reward_Highlight.Interactive = false;

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
            UIManager.Instance.AudioManager.PlaySFX(19);
            break;
        }

        StartCoroutine(UIManager.Instance.ChangeAlpha(RewardButtonGroup, 0.0f, 0.6f));
        RemainReward = false;
      }
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
        return;
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
  }//������ ��� �г�Ƽ �ΰ��ϴ� ����
  #endregion
  [Space(20)]
  [Header("������")]
  #region ������
  public GameObject SettlementObjectHolder = null;
  public Image MadenssEffect = null;
  [SerializeField] private RectTransform MapbuttonPos_Settlemtn = null;
  [SerializeField] private UnityEngine.UI.Image SettlementIcon = null;
  [SerializeField] private TextMeshProUGUI SettlementNameText = null;
  [SerializeField] private RectTransform DiscomfortIcon = null;
  [SerializeField] private TextMeshProUGUI DiscomfortText = null;
  [SerializeField] private TextMeshProUGUI RestCostValueText = null;
  [SerializeField] private List<PlaceIconScript> SectorIcons = new List<PlaceIconScript>();
  private PlaceIconScript GetSectorIconScript(SectorTypeEnum sectortype)
  {
    for (int i = 0; i < SectorIcons.Count; i++)
    {
      if (SectorIcons[i].MyType == sectortype) return SectorIcons[i];
    }
    return null;
  }
  [SerializeField] private UnityEngine.UI.Image SelectSectorIcon = null;
  [SerializeField] private TextMeshProUGUI SectorName = null;
  [SerializeField] private TextMeshProUGUI SectorEffect = null;
  [SerializeField] private TextMeshProUGUI RestResult = null;
  [SerializeField] private GameObject RestButtonHolder = null;
  [SerializeField] private TextMeshProUGUI CostText = null;
  [SerializeField] private Onpointer_highlight CostHighlight_Sanity = null;
  [SerializeField] private UnityEngine.UI.Button Cost_Sanity = null;
  [SerializeField] private Onpointer_highlight CostHighlight_Gold = null;
  [SerializeField] private UnityEngine.UI.Button Cost_Gold = null;
  private Settlement CurrentSettlement = null;
  private SectorTypeEnum SelectedSector = SectorTypeEnum.NULL;
  private bool IsMad = false;
  public IEnumerator openui_settlement(bool dir)
  {
    if (GameManager.Instance.MyGameData.CurrentEvent != null)
    {
      GameManager.Instance.MyGameData.Turn++;
      GameManager.Instance.MyGameData.CurrentEvent = null;
      GameManager.Instance.SaveData();
    }

    if (PlayerPrefs.GetInt("Tutorial_Settlement") == 0) UIManager.Instance.TutorialUI.OpenTutorial_Settlement();

    if (GameManager.Instance.MyGameData.Madness_Force && (GameManager.Instance.MyGameData.TotalRestCount % ConstValues.MadnessEffect_Force == ConstValues.MadnessEffect_Force-1))
    {
      Debug.Log("���� ���� �ߵ�");
      UIManager.Instance.HighlightManager.HighlightAnimation(HighlightEffectEnum.Madness, SkillTypeEnum.Force);
      UIManager.Instance.AudioManager.PlaySFX(27, 5);
      if (!MadenssEffect.enabled) MadenssEffect.enabled = true;
      IsMad = true;
    }
    else
    {
      UIManager.Instance.AudioManager.PlaySFX(14, 3);
      if (MadenssEffect.enabled) MadenssEffect.enabled = false;
      IsMad = false;
    }
    UIManager.Instance.OffBackground();


    DefaultGroup.interactable = false;

    IsOpen = true;

    if (EventObjectHolder.activeInHierarchy == true) EventObjectHolder.SetActive(false);
    if (SettlementObjectHolder.activeInHierarchy == false) SettlementObjectHolder.SetActive(true);
    if (SettlementBackground.enabled == false) SettlementBackground.enabled = true;
    DialogueRect.sizeDelta = SettlementDialogueSize;

    IsSelectSector = false;
    QuestSectorInfo = false;
    SelectedSector = SectorTypeEnum.NULL;
    CurrentSettlement = GameManager.Instance.MyGameData.CurrentSettlement;
    Illust.Setup(GameManager.Instance.ImageHolder.GetSettlementIllust(CurrentSettlement.SettlementType, GameManager.Instance.MyGameData.Turn));


SettlementNameText.text = CurrentSettlement.Name;
    DiscomfortIcon.sizeDelta = Vector2.one * Mathf.Lerp(ConstValues.DiscomfortIconSize_min, ConstValues.DiscomfortIconsize_max,
      Mathf.Clamp(CurrentSettlement.Discomfort * 0.1f, 0.0f, 1.0f));
    DiscomfortText.fontSize = Mathf.Lerp(ConstValues.DiscomfortFontSize_min, ConstValues.DiscomfortFontSize_max,
          Mathf.Clamp(CurrentSettlement.Discomfort * 0.1f, 0.0f, 1.0f));
    DiscomfortText.text = CurrentSettlement.Discomfort.ToString();
    RestCostValueText.text = string.Format(GameManager.Instance.GetTextData("RestCostValue"),
     (int)(GameManager.Instance.MyGameData.GetDiscomfortValue(CurrentSettlement.Discomfort) * 100));
    if (RestButtonHolder.gameObject.activeInHierarchy == true) RestButtonHolder.gameObject.SetActive(false);

    Sprite _settlementicon = null;
    switch (CurrentSettlement.SettlementType)
    {
      case SettlementType.Village: _settlementicon = GameManager.Instance.ImageHolder.VillageIcon_white; break;
      case SettlementType.Town:  _settlementicon = GameManager.Instance.ImageHolder.TownIcon_white; break;
      case SettlementType.City:_settlementicon = GameManager.Instance.ImageHolder.CityIcon_white; break;
    }
    for (int i = 0; i < SectorIcons.Count; i++)
    {
      if (CurrentSettlement.Sectors.Contains(SectorIcons[i].MyType))
      {
        if (SectorIcons[i].gameObject.activeInHierarchy == false) SectorIcons[i].gameObject.SetActive(true);
        SectorIcons[i].OpenIcon();
      }
      else
      {
        if (SectorIcons[i].gameObject.activeInHierarchy == true) SectorIcons[i].gameObject.SetActive(false);
      }
    }

    SettlementIcon.sprite = _settlementicon;
    SelectSectorIcon.sprite = GameManager.Instance.ImageHolder.Transparent;
    SectorName.text =IsMad?"": GameManager.Instance.GetTextData("SELECTPLACE");
    SectorEffect.text = "";

    RestResult.text = "";
    CostText.text = "";

    Vector2 _startpos_panel = dir?LeftPos:RightPos, _endpos_panel = CenterPos;
    UIManager.Instance.MapButton.SetCurrentUI(this, MapbuttonPos_Settlemtn, 1.0f);

    yield return StartCoroutine(UIManager.Instance.moverect(DefaultRect, _startpos_panel, _endpos_panel, OpenTime, UIManager.Instance.UIPanelOpenCurve));


    DefaultGroup.interactable = true;
  }
  /// <summary>
  /// 0:�Ϲ� 1:��ȸ 2:���Ƽ��
  /// </summary>
  [HideInInspector] public bool QuestSectorInfo = false;
  [HideInInspector] public int GoldCost = 0;
  [HideInInspector] public int SanityCost = 0;
  [HideInInspector] public int DiscomfortValue = 0;
  [HideInInspector] public int MovePointValue = 0;
  [HideInInspector] public bool IsSelectSector = false;

  public void OnPointerSector(SectorTypeEnum sector)
  {
    if (IsSelectSector == true) return;

    QuestSectorInfo = GameManager.Instance.MyGameData.Cult_SabbatSector == sector;

    SelectSectorIcon.sprite =IsMad?GameManager.Instance.ImageHolder.MadnessActive: GameManager.Instance.ImageHolder.GetSectorIcon(sector);
    SectorName.text = GameManager.Instance.GetTextData(sector, 0);
    string _effect = GameManager.Instance.GetTextData(sector, 3);
    int _discomfort_default = (GameManager.Instance.MyGameData.FirstRest && GameManager.Instance.MyGameData.Tendency_Head.Level ==2) == true ?
      ConstValues.Tendency_Head_p2 : ConstValues.Rest_Discomfort;
    switch (sector)
    {
      case SectorTypeEnum.Residence:
        _effect = IsMad ? GameManager.Instance.GetTextData("Madness_Force_Description") : string.Format(_effect,
          WNCText.PositiveColor(ConstValues.SectorEffect_residence_discomfort.ToString()));

        GoldCost = GameManager.Instance.MyGameData.RestCost_Gold;
        SanityCost = GameManager.Instance.MyGameData.RestCost_Sanity;
        DiscomfortValue=IsMad? _discomfort_default: _discomfort_default > 1 ? _discomfort_default - ConstValues.SectorEffect_residence_discomfort : 0;
        MovePointValue = ConstValues.Rest_MovePoint;
        break;
      case SectorTypeEnum.Temple:
        _effect = IsMad ? GameManager.Instance.GetTextData("Madness_Force_Description") : string.Format(_effect, ConstValues.SectorEffect_temple);

        GoldCost = GameManager.Instance.MyGameData.RestCost_Gold;
        SanityCost = GameManager.Instance.MyGameData.RestCost_Sanity;
        DiscomfortValue = _discomfort_default;
        MovePointValue = ConstValues.Rest_MovePoint;
        break;
      case SectorTypeEnum.Marketplace:
        _effect = IsMad ? GameManager.Instance.GetTextData("Madness_Force_Description") : string.Format(_effect, ConstValues.SectorEffect_marketSector);

        GoldCost =IsMad? GameManager.Instance.MyGameData.RestCost_Gold: Mathf.FloorToInt(GameManager.Instance.MyGameData.RestCost_Gold * (1.0f - ConstValues.SectorEffect_marketSector / 100.0f));
        SanityCost = IsMad ? GameManager.Instance.MyGameData.RestCost_Sanity : Mathf.FloorToInt(GameManager.Instance.MyGameData.RestCost_Sanity * (1.0f - ConstValues.SectorEffect_marketSector / 100.0f));
        DiscomfortValue = _discomfort_default;
        MovePointValue = ConstValues.Rest_MovePoint;
        break;
      case SectorTypeEnum.Library:
        _effect = IsMad ? GameManager.Instance.GetTextData("Madness_Force_Description") : string.Format(_effect, ConstValues.SectorEffect_Library);

        GoldCost = GameManager.Instance.MyGameData.RestCost_Gold;
        SanityCost = GameManager.Instance.MyGameData.RestCost_Sanity;
        DiscomfortValue = _discomfort_default;
        MovePointValue = ConstValues.Rest_MovePoint;
        break;
    }

    string _sabbatdescription = "";
    switch (QuestSectorInfo)
    {
      case false:
        SectorEffect.text = _effect;
        break;
      case true:
        _sabbatdescription = "<br>" + string.Format(GameManager.Instance.GetTextData("Cult_Progress_Sabbat_Effect"),
        ConstValues.Quest_Cult_Progress_Sabbat);
        SectorEffect.text = _effect + _sabbatdescription;
        break;
    }
    RestResult.text = string.Format(GameManager.Instance.GetTextData("RestResult"),
      GameManager.Instance.MyGameData.Tendency_Head.Level == +1 && GameManager.Instance.MyGameData.FirstRest ? "(<sprite=95>)" : ""
      ,DiscomfortValue, MovePointValue);

  }
  public void OutPointerSector()
  {
    if (IsSelectSector == true) return;

    SelectSectorIcon.sprite = GameManager.Instance.ImageHolder.Transparent;
    SectorName.text = "";
    SectorEffect.text = "";
    RestResult.text = "";
  }
  public void SelectPlace(int index)  //Sectortype�� 0�� NULL��
  {
    if (SelectedSector == (SectorTypeEnum)index) return;

    if (SelectedSector != SectorTypeEnum.NULL) GetSectorIconScript(SelectedSector).SetIdleColor();
    SelectedSector = (SectorTypeEnum)index;
    IsSelectSector = true;

    switch (SelectedSector)
    {
      case SectorTypeEnum.Residence:
        UIManager.Instance.AudioManager.PlaySFX(10,1);
        break;
      case SectorTypeEnum.Temple:
        UIManager.Instance.AudioManager.PlaySFX(11,1);
        break;
      case SectorTypeEnum.Marketplace:
        UIManager.Instance.AudioManager.PlaySFX(12,1);
        break;
      case SectorTypeEnum.Library:
        UIManager.Instance.AudioManager.PlaySFX(13,1);
        break;
    }

    Illust.Setup(GameManager.Instance.ImageHolder.GetSectorIllust(CurrentSettlement.SettlementType, SelectedSector, GameManager.Instance.MyGameData.Turn));

    QuestSectorInfo = GameManager.Instance.MyGameData.Cult_SabbatSector==SelectedSector;

    SelectSectorIcon.sprite = IsMad ? GameManager.Instance.ImageHolder.MadnessActive: GameManager.Instance.ImageHolder.GetSectorIcon(SelectedSector);
    SectorName.text = GameManager.Instance.GetTextData(SelectedSector, 0);
    string _effect = GameManager.Instance.GetTextData(SelectedSector, 3);
    int _discomfort_default = (GameManager.Instance.MyGameData.FirstRest && GameManager.Instance.MyGameData.Tendency_Head.Level ==2) == true ?
      ConstValues.Tendency_Head_p2 : ConstValues.Rest_Discomfort;
    switch (SelectedSector)
    {
      case SectorTypeEnum.Residence:
        _effect = IsMad ? GameManager.Instance.GetTextData("Madness_Force_Description") : string.Format(_effect,
          WNCText.PositiveColor(ConstValues.SectorEffect_residence_discomfort.ToString()));

        GoldCost = GameManager.Instance.MyGameData.RestCost_Gold;
        SanityCost = GameManager.Instance.MyGameData.RestCost_Sanity;
        DiscomfortValue = IsMad ? _discomfort_default : _discomfort_default > 1 ? _discomfort_default - ConstValues.SectorEffect_residence_discomfort : 0;
        MovePointValue = ConstValues.Rest_MovePoint;
        break;
      case SectorTypeEnum.Temple:
        _effect = IsMad ? GameManager.Instance.GetTextData("Madness_Force_Description") : string.Format(_effect, ConstValues.SectorEffect_temple);

        GoldCost = GameManager.Instance.MyGameData.RestCost_Gold;
        SanityCost = GameManager.Instance.MyGameData.RestCost_Sanity;
        DiscomfortValue = _discomfort_default;
        MovePointValue = ConstValues.Rest_MovePoint;
        break;
      case SectorTypeEnum.Marketplace:
        _effect = IsMad ? GameManager.Instance.GetTextData("Madness_Force_Description") : string.Format(_effect, ConstValues.SectorEffect_marketSector);

        GoldCost = IsMad ? GameManager.Instance.MyGameData.RestCost_Gold : Mathf.FloorToInt(GameManager.Instance.MyGameData.RestCost_Gold * (1.0f - ConstValues.SectorEffect_marketSector / 100.0f));
        SanityCost = IsMad ? GameManager.Instance.MyGameData.RestCost_Sanity : Mathf.FloorToInt(GameManager.Instance.MyGameData.RestCost_Sanity * (1.0f - ConstValues.SectorEffect_marketSector / 100.0f));
        DiscomfortValue = _discomfort_default;
        MovePointValue = ConstValues.Rest_MovePoint;
        break;
      case SectorTypeEnum.Library:
        _effect = IsMad ? GameManager.Instance.GetTextData("Madness_Force_Description") : string.Format(_effect, ConstValues.SectorEffect_Library);

        GoldCost = GameManager.Instance.MyGameData.RestCost_Gold;
        SanityCost = GameManager.Instance.MyGameData.RestCost_Sanity;
        DiscomfortValue = _discomfort_default;
        MovePointValue = ConstValues.Rest_MovePoint;
        break;
    }

    string _sabbatdescription = "";
    switch (QuestSectorInfo)
    {
      case false:
        SectorEffect.text = _effect;
        UIManager.Instance.SidePanelCultUI.SetSabbatEffect(false);
        break;
      case true:
        DiscomfortValue += ConstValues.Quest_Cult_SabbatDiscomfort;
        _sabbatdescription = "<br>" + string.Format(GameManager.Instance.GetTextData("Cult_Progress_Sabbat_Effect"),
        ConstValues.Quest_Cult_Progress_Sabbat);
        SectorEffect.text = _effect + _sabbatdescription;
        UIManager.Instance.SidePanelCultUI.SetSabbatEffect(true);
        break;
    }
    RestResult.text = string.Format(GameManager.Instance.GetTextData("RestResult"), 
      GameManager.Instance.MyGameData.Tendency_Head.Level==1&&GameManager.Instance.MyGameData.FirstRest ? "(<sprite=95>)":"",
      DiscomfortValue, MovePointValue);

    CostText.text = "";
    if (RestButtonHolder.gameObject.activeInHierarchy == false)
    {
      RestButtonHolder.gameObject.SetActive(true);
    }

    CostHighlight_Sanity.Interactive = true;
    CostHighlight_Sanity.SetInfo(HighlightEffectEnum.Sanity, -1 * SanityCost);

    bool _goldable = GameManager.Instance.MyGameData.Gold >= GoldCost;
    Cost_Gold.interactable = _goldable;
    Cost_Gold.GetComponent<CanvasGroup>().alpha=_goldable?1.0f:0.3f;
    CostHighlight_Gold.Interactive = _goldable;
    if (_goldable) CostHighlight_Gold.SetInfo(HighlightEffectEnum.Gold, -1 * GoldCost);
  }
  public void OnPointerRestType(StatusTypeEnum type)
  {
    if (UIManager.Instance.IsWorking) return;

    switch (type)
    {
      case StatusTypeEnum.Sanity:

        CostText.text = string.Format(GameManager.Instance.GetTextData("Restbutton_Sanity"), SanityCost);
        break;
      case StatusTypeEnum.Gold:

        CostText.text = string.Format(GameManager.Instance.GetTextData("Restbutton_Gold"), GoldCost);
        break;

    }
  }
  public void OnExitRestType(StatusTypeEnum type)
  {
    return;
  }
  public void StartRest_Sanity()
  {
    if (UIManager.Instance.IsWorking) return;

    CostHighlight_Sanity.Interactive = false;
    CostHighlight_Gold.Interactive = false;

    UIManager.Instance.AddUIQueue(restinsector(StatusTypeEnum.Sanity));
  }
  public void StartRest_Gold()
  {
    if (UIManager.Instance.IsWorking) return;

    CostHighlight_Sanity.Interactive = false;
    CostHighlight_Gold.Interactive = false;

    UIManager.Instance.AddUIQueue(restinsector(StatusTypeEnum.Gold));
  }

  private IEnumerator restinsector(StatusTypeEnum statustype)
  {
    IsOpen = false;

    GameManager.Instance.MyGameData.FirstRest = false;
    if (GameManager.Instance.MyGameData.Madness_Force)
    {
      GameManager.Instance.MyGameData.TotalRestCount++;
    }

    int _discomfortvalue = DiscomfortValue;
    switch (GameManager.Instance.MyGameData.QuestType)
    {
      case QuestType.Cult:
        switch (QuestSectorInfo)
        {
          case false:
            break;
          case true:
            UIManager.Instance.CultUI.AddProgress(3);
            break;
        }
        break;
    }

    switch (statustype)
    {
      case StatusTypeEnum.Sanity:
        GameManager.Instance.MyGameData.Sanity -= SanityCost;
        CurrentSettlement.Discomfort += _discomfortvalue;
        DiscomfortText.text = CurrentSettlement.Discomfort.ToString();
        if (DiscomfortValue > 0)
        {
          yield return StartCoroutine(discomfortscale());
        }
        break;
      case StatusTypeEnum.Gold:

        GameManager.Instance.MyGameData.Gold -= GoldCost;
        CurrentSettlement.Discomfort += _discomfortvalue;
        DiscomfortText.text = CurrentSettlement.Discomfort.ToString();
        if (DiscomfortValue > 0)
        {
          yield return StartCoroutine(discomfortscale());
        }
        break;
    }
    GameManager.Instance.MyGameData.MovePoint += MovePointValue;
    yield return StartCoroutine(UIManager.Instance.SetIconEffect_movepoint_gain(SettlementIcon.rectTransform, MovePointValue));
    GameManager.Instance.MyGameData.ApplySectorEffect(IsMad? SectorTypeEnum.NULL: SelectedSector);

    UIManager.Instance.AudioManager.PlaySFX(2);

    yield return new WaitForSeconds(0.8f);

    UIManager.Instance.AudioManager.PlaySFX(14);

    yield return StartCoroutine(closeui_all(true));

    EventManager.Instance.SetSettlementEvent(SelectedSector);

  }
  public float DiscomfortScaleEffectTime = 0.3f;
  public AnimationCurve DiscomfortAnimationCurve = new AnimationCurve();
  private IEnumerator discomfortscale()
  {
    float _time = 0.0f;
    float _startsize = DiscomfortIcon.sizeDelta.x,
      _endsize = Mathf.Lerp(ConstValues.DiscomfortIconSize_min, ConstValues.DiscomfortIconsize_max,
  Mathf.Clamp(CurrentSettlement.Discomfort * 0.1f, 0.0f, 1.0f));
    float _fontstartsize = DiscomfortText.fontSize;
    float _fontendsize = Mathf.Lerp(ConstValues.DiscomfortFontSize_min, ConstValues.DiscomfortFontSize_max,
          Mathf.Clamp(CurrentSettlement.Discomfort * 0.1f, 0.0f, 1.0f));

    while (_time < DiscomfortScaleEffectTime)
    {
      DiscomfortIcon.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.5f, DiscomfortAnimationCurve.Evaluate(_time / DiscomfortScaleEffectTime));
      DiscomfortIcon.sizeDelta = Vector2.one * Mathf.Lerp(_startsize, _endsize, _time / DiscomfortScaleEffectTime);
      DiscomfortText.fontSize = Mathf.Lerp(_fontstartsize, _fontendsize, _time / DiscomfortScaleEffectTime);
      _time += Time.deltaTime;
      yield return null;
    }
    DiscomfortIcon.localScale = Vector3.one;
    DiscomfortText.fontSize = _fontendsize;
  }
  #endregion

  [SerializeField] private UI_map MapUI = null;

}
