using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UI_QuestWolf : UI_default
{
  [Space(10)]
  public bool Skip = false;
  [SerializeField] private float UIMoveInTime = 0.7f;
  [SerializeField] private float UIMoveOutTime = 0.4f;//Rect 움직이는거
  [SerializeField] private float FadeInTime = 1.0f;
  [SerializeField] private float FadeOutTime = 0.4f;  //이미지,텍스트 투명도
  private QuestHolder_Cult questholder = null;
  private QuestHolder_Cult QuestHolder
  {
    get 
    { 
      if (questholder == null) questholder = (QuestHolder_Cult)GameManager.Instance.MyGameData.CurrentQuestData;
      return questholder;
    }
  }

  #region 프롤로그
  [Space(5)]
  [SerializeField] private CanvasGroup PrologueGroup = null;
  [SerializeField] private ImageSwapScript Illust = null;
  [SerializeField] private TextMeshProUGUI Prologue_Description = null;
  [SerializeField] private Scrollbar PrologueScrollbar = null;
  public AnimationCurve ScrollbarCurve = new AnimationCurve();
  [SerializeField] private float ScrollSpeed = 0.4f;
  private IEnumerator updatescrollbar()
  {
    yield return new WaitForSeconds(0.05f);

    float _time = 0.0f;
    while (PrologueScrollbar.value > 0.001f && _time < ConstValues.ScrollTime)
    {
      PrologueScrollbar.value = Mathf.Lerp(PrologueScrollbar.value, 0.0f, ConstValues.ScrollSpeed);
      _time += Time.deltaTime;
      yield return null;

    }
    PrologueScrollbar.value = 0.0f;
  }
  [SerializeField] private CanvasGroup Prologue_TendencyGroup = null;
  [SerializeField] private Button Prologue_Button_Left = null;
  [SerializeField] private TextMeshProUGUI Prologue_ButtonText_Left = null;
  [SerializeField] private Button Prologue_Button_Right = null;
  [SerializeField] private TextMeshProUGUI Prologue_ButtonText_Right = null;
  [SerializeField] private CanvasGroup Prologue_Nextbutton_Group = null;
  [SerializeField] private float PrologueFadetime = 0.7f;
  private void ActiveSetNextButton()
  {
    if (Prologue_TendencyGroup.gameObject.activeInHierarchy == true) Prologue_TendencyGroup.gameObject.SetActive(false);
    if (Prologue_Nextbutton_Group.gameObject.activeInHierarchy == false) Prologue_Nextbutton_Group.gameObject.SetActive(true);

    Prologue_Nextbutton_Group.interactable = true;
  }
  public void DeActiveSetNextButton()
  {
    if (Prologue_TendencyGroup.gameObject.activeInHierarchy == true) Prologue_TendencyGroup.gameObject.SetActive(false);
    if (Prologue_Nextbutton_Group.gameObject.activeInHierarchy == false) Prologue_Nextbutton_Group.gameObject.SetActive(true);

    Prologue_Nextbutton_Group.interactable = false;
  }
  public void ActiveTendencybutton()
  {
    Prologue_TendencyGroup.alpha = 0.0f;
    Prologue_TendencyGroup.interactable = false;

    if (Prologue_TendencyGroup.gameObject.activeInHierarchy == false) Prologue_TendencyGroup.gameObject.SetActive(true);
    if (Prologue_Nextbutton_Group.gameObject.activeInHierarchy == true) Prologue_Nextbutton_Group.gameObject.SetActive(false);

    StartCoroutine(UIManager.Instance.ChangeAlpha(Prologue_TendencyGroup, 1.0f, 0.5f));
  }
  public int CurrentPrologueIndex = 0;
  public void OpenUI_Prologue(QuestHolder_Cult wolf)
  {
    if(DefaultRect.anchoredPosition!=Vector2.zero)DefaultRect.anchoredPosition = Vector2.zero;
    CurrentPrologueIndex = 0;
    IsOpen = true;
    UIManager.Instance.SidePanelCultUI.UpdateUI();
    UIManager.Instance.AddUIQueue(openui_prologue());
  }
  private IEnumerator openui_prologue()
  {
    DeActiveSetNextButton();
     
    Illust.Setup(QuestHolder.Prologue_0_Illust, 0.1f);

    Prologue_Description.text = QuestHolder.Prologue_0_Description;
    LayoutRebuilder.ForceRebuildLayoutImmediate(Prologue_Description.transform.transform.transform as RectTransform);
    StartCoroutine(updatescrollbar());

    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(PrologueGroup, 1.0f, 2.0f));

    ActiveSetNextButton();

    yield return null;
  }
  public  void CloseUI_Prologue() => UIManager.Instance.AddUIQueue(closeui_prologue());
  private IEnumerator closeui_prologue()
  {
    CurrentPrologueIndex = 0;
    IsOpen = false;
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(PrologueGroup, 0.0f, FadeOutTime));
  }
  public void Next()
  {
    if (UIManager.Instance.IsWorking) return;
    if (Skip)
    {
      CurrentPrologueIndex = 7;
      GameManager.Instance.MyGameData.Tendency_Head.Level = 1;
      GameManager.Instance.MyGameData.Tendency_Body.Level = -1;
      UIManager.Instance.AddUIQueue(next());
      return;
    }
    DeActiveSetNextButton();

    UIManager.Instance.AddUIQueue(next());
  }
  private IEnumerator next()
  {
    CurrentPrologueIndex++;

    Sprite _illust = null;
    string _description = null;
    string _buttontext_a = "";
    string _buttontext_b = "";


    switch (CurrentPrologueIndex)
    {
      case 1:
        _illust = QuestHolder.Prologue_1_Illust;
        _description = QuestHolder.Prologue_1_Description;
        _buttontext_a = QuestHolder.Prologue_1_Selection_0;
        _buttontext_b=QuestHolder.Prologue_1_Selection_1;
        break;
      case 2:
        if (GameManager.Instance.MyGameData.Tendency_Body.Level == -1)
        {
          _illust = QuestHolder.Prologue_2_0_Illust;
          _description = QuestHolder.Prologue_2_0_Description;
        }
        else
        {
          _illust = QuestHolder.Prologue_2_1_Illust;
          _description = QuestHolder.Prologue_2_1_Description;
        }
        break;
      case 3:
        _illust = QuestHolder.Prologue_3_Illust;
        _description = QuestHolder.Prologue_3_Description;
        _buttontext_a = QuestHolder.Prologue_3_Selection_0;
        _buttontext_b = QuestHolder.Prologue_3_Selection_1;
        break;
      case 4:
        if (GameManager.Instance.MyGameData.Tendency_Head.Level == -1)
        {
          _illust = QuestHolder.Prologue_4_0_Illust;
          _description = QuestHolder.Prologue_4_0_Description;
        }
        else
        {
          _illust = QuestHolder.Prologue_4_1_Illust;
          _description = QuestHolder.Prologue_4_1_Description;
        }
        break;
      case 5:
        _illust = QuestHolder.Prologue_5_Illust;
        _description = QuestHolder.Prologue_5_Description;
        break;
      case 6:
        _illust = QuestHolder.Prologue_6_Illust;
        _description = QuestHolder.Prologue_6_Description;
        break;
      case 7:
        _illust = QuestHolder.Prologue_7_Illust;
        _description = QuestHolder.Prologue_7_Description;
        break;
      case 8:
        _illust = QuestHolder.Prologue_8_Illust;
        _description = QuestHolder.Prologue_8_Description;
        break;
    }
    Illust.Next(_illust,  PrologueFadetime);

    Prologue_Description.text += "<br><br>" + _description;
    LayoutRebuilder.ForceRebuildLayoutImmediate(Prologue_Description.transform as RectTransform);
    LayoutRebuilder.ForceRebuildLayoutImmediate(Prologue_Description.transform.parent.transform as RectTransform);
    yield return StartCoroutine(updatescrollbar());

    if (_buttontext_a == "")
    {
      ActiveSetNextButton();
    }
    else
    {
      Prologue_TendencyGroup.alpha = 0.0f;
      Prologue_TendencyGroup.interactable = false;
      if (Prologue_TendencyGroup.gameObject.activeInHierarchy == false) Prologue_TendencyGroup.gameObject.SetActive(true);

      Prologue_ButtonText_Left.text = _buttontext_a;
      Prologue_Button_Left.transform.GetComponent<Image>().sprite =
        CurrentPrologueIndex == 1 ? GameManager.Instance.ImageHolder.SelectionButtonImage_BodyM_Idle : GameManager.Instance.ImageHolder.SelectionButtonImage_HeadM_Idle;
      Prologue_Button_Left.spriteState = GameManager.Instance.ImageHolder.GetSelectionButtonBackground(CurrentPrologueIndex == 1 ? TendencyTypeEnum.Body : TendencyTypeEnum.Head, true);
      LayoutRebuilder.ForceRebuildLayoutImmediate(Prologue_ButtonText_Left.transform as RectTransform);
      LayoutRebuilder.ForceRebuildLayoutImmediate(Prologue_Button_Left.transform as RectTransform);
      Prologue_ButtonText_Right.text = _buttontext_b;
      Prologue_Button_Right.transform.GetComponent<Image>().sprite =
    CurrentPrologueIndex == 1 ? GameManager.Instance.ImageHolder.SelectionButtonImage_BodyP_Idle : GameManager.Instance.ImageHolder.SelectionButtonImage_HeadP_Idle;
      Prologue_Button_Right.spriteState = GameManager.Instance.ImageHolder.GetSelectionButtonBackground(CurrentPrologueIndex == 1 ? TendencyTypeEnum.Body : TendencyTypeEnum.Head, false);
      LayoutRebuilder.ForceRebuildLayoutImmediate(Prologue_ButtonText_Right.transform as RectTransform);
      LayoutRebuilder.ForceRebuildLayoutImmediate(Prologue_Button_Right.transform as RectTransform);
      ActiveTendencybutton();
    }


    if (CurrentPrologueIndex == 8)                  //프롤로그 종료할 때 - A 비활성화
    {
      DeActiveSetNextButton();
      MoveRectForButton(0);
      UIManager.Instance.MapButton.Open(1, this);
    }

  }
  public void SelectTendency(int index)
  {
    if (UIManager.Instance.IsWorking) return;

    Prologue_TendencyGroup.interactable = false;
    switch (CurrentPrologueIndex)
    {
      case 1:
        if (index == 0)
        {
          GameManager.Instance.MyGameData.Tendency_Body.Level = -1;
        }
        else
        {
          GameManager.Instance.MyGameData.Tendency_Body.Level = +1;
        }
        break;//(정신적+대화)선택 , (육체적+무력)선택

      case 3:
        if (index == 0)
        {
          GameManager.Instance.MyGameData.Tendency_Head.Level = -1;
        }
        else
        {
          GameManager.Instance.MyGameData.Tendency_Head.Level = +1;
        }
        break;//(감정적+자연)선택 , (물질적+지성)선택
    }

    StartCoroutine(UIManager.Instance.ChangeAlpha(Prologue_TendencyGroup, 0.0f, 0.5f));

    UIManager.Instance.AddUIQueue(next());
  }
  #endregion

  #region 진행도 이벤트
  [SerializeField] private CanvasGroup ProgressEventGroup = null;
  [SerializeField] private GameObject ProgressBackgroundButton = null;
  [SerializeField] private Image ProgressEventIllust = null;
  [SerializeField] private TextMeshProUGUI ProgressEventDescription = null;
  private bool IsProgressWorking=false;
  /// <summary>
  /// 0:성공 1:실패 2:정착지 3:집회 4:의식
  /// </summary>
  /// <param name="progresstype"></param>
  public void AddProgress(int progresstype)
  {
    if (DefaultRect.anchoredPosition != Vector2.zero) DefaultRect.anchoredPosition = Vector2.zero;

    int _eventtype = 0;
    //0:없음 1:없음 2:정착지 3:집회 4:의식

    switch (progresstype)
    {
      case 0:
        GameManager.Instance.MyGameData.Quest_Cult_Progress += GameManager.Instance.MyGameData.Quest_Cult_Phase < 2 ?
              ConstValues.Qeust_Cult_EventProgress_Clear : ConstValues.Qeust_Cult_EventProgress_Clear;
        break;
      case 1:
        GameManager.Instance.MyGameData.Quest_Cult_Progress += GameManager.Instance.MyGameData.Quest_Cult_Phase < 2 ?
ConstValues.Quest_Cult_EventProgress_Fail : ConstValues.Quest_Cult_EventProgress_Fail;
        break;
      case 2:
        GameManager.Instance.MyGameData.Quest_Cult_Phase++;

        switch (GameManager.Instance.MyGameData.CurrentSettlement.SettlementType)
        {
          case SettlementType.Village:
            GameManager.Instance.MyGameData.Quest_Cult_Progress += ConstValues.Quest_Cult_Progress_Village;
            break;
          case SettlementType.Town:
            GameManager.Instance.MyGameData.Quest_Cult_Progress += ConstValues.Quest_Cult_Progress_Town;
            break;
          case SettlementType.City:
            GameManager.Instance.MyGameData.SetSabbat();
            GameManager.Instance.MyGameData.Quest_Cult_Progress += ConstValues.Quest_Cult_Progress_City;
            break;
        }
        _eventtype = 2;
        break;
      case 3:
        GameManager.Instance.MyGameData.SetRitual();
        GameManager.Instance.MyGameData.Quest_Cult_Progress += ConstValues.Quest_Cult_Progress_Sabbat;
        _eventtype = 3;
        break;
      case 4:
        GameManager.Instance.MyGameData.SetSabbat();
        GameManager.Instance.MyGameData.Quest_Cult_Progress += ConstValues.Quest_Cult_Progress_Ritual;
        _eventtype = 4;
        break;
      default:
        Debug.Log("아니이게머임???");
        return;
    }

    if (_eventtype == 0) return;

    Sprite _illust = null;
    string _description = "";
    switch (_eventtype)
    {
      case 2:
        var _tuple_0 = QuestHolder.GetSettlementData(GameManager.Instance.MyGameData.CurrentSettlement.SettlementType);
        _illust = _tuple_0.Item1;
        _description = _tuple_0.Item2;
        break;
      case 3:
        var _tuple_1 = QuestHolder.GetSabbatData;
        _illust = _tuple_1.Item1;
        _description = _tuple_1.Item2;
        break;
      case 4:
        var _tuple_2 = QuestHolder.GetRitualData;
        _illust = _tuple_2.Item1;
        _description = _tuple_2.Item2;
        break;
    }
    ProgressEventIllust.sprite = _illust;
    ProgressEventDescription.text = _description;

    StartCoroutine(openprogress());
  }
  private IEnumerator openprogress()
  {
    IsProgressWorking = true;
    ProgressBackgroundButton.SetActive(true);
    UIManager.Instance.AudioManager.PlaySFX(28);
    UIManager.Instance.SidePanelCultUI.UpdateUI();
     yield return StartCoroutine(UIManager.Instance.ChangeAlpha(ProgressEventGroup, 1.0f,UIMoveInTime));
    IsProgressWorking = false;
  }
  public void CloseProgress()
  {
    if (IsProgressWorking) return;
    StartCoroutine(closeprogress());
  }
  private IEnumerator closeprogress()
  {
    IsProgressWorking = true;
    ProgressBackgroundButton.SetActive(false);
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(ProgressEventGroup, 0.0f, UIMoveOutTime));
    IsProgressWorking = false;
  }

  #endregion
  public void CloseUI_Auto()
  {
    if (CurrentPrologueIndex >0)
    {
       CloseUI_Prologue();
      CurrentPrologueIndex = -1;
    }
  }

}
