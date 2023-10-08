using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;

public class UI_QuestWolf : UI_default
{
  [Space(10)]
  [SerializeField] private UI_Settlement SettlementUI = null;
  public bool Skip = false;
  [SerializeField] private float UIMoveInTime = 0.7f;
  [SerializeField] private float UIMoveOutTime = 0.4f;//Rect �����̴°�
  [SerializeField] private float FadeInTime = 1.0f;
  [SerializeField] private float FadeOutTime = 0.4f;  //�̹���,�ؽ�Ʈ ����
  private QuestHolder_Cult QuestHolder = null;

  #region ���ѷα�
  [Space(5)]
  [SerializeField] private Image Prologue_IllustImage = null;
  [SerializeField] private CanvasGroup Prologue_IllustGroup = null;
  [SerializeField] private TextMeshProUGUI Prologue_Description = null;
  [SerializeField] private CanvasGroup Prologue_DescriptionGroup = null;
  [SerializeField] private CanvasGroup Prologue_ButtonHolderGroup = null;
  [SerializeField] private Button Prologue_Button_A = null;
  [SerializeField] private TextMeshProUGUI Prologue_ButtonText_A = null;
  [SerializeField] private Button Prologue_Button_B = null;
  [SerializeField] private TextMeshProUGUI Prologue_ButtonText_B = null;
  public int CurrentPrologueIndex = 0;
  public void OpenUI_Prologue(QuestHolder_Cult wolf)
  {
    if(DefaultRect.anchoredPosition!=Vector2.zero)DefaultRect.anchoredPosition = Vector2.zero;
    CurrentPrologueIndex = 0;
    QuestHolder = wolf;
    IsOpen = true;
    UIManager.Instance.QuestSidePanel_Cult.UpdateUI();
    UIManager.Instance.AddUIQueue(openui_prologue());
  }
  private IEnumerator openui_prologue()
  {
    Prologue_IllustImage.sprite = QuestHolder.Prologue_0_Illust;
    Prologue_IllustGroup.alpha = 0.0f;

    Prologue_Description.text = QuestHolder.Prologue_0_Description;
    Prologue_DescriptionGroup.alpha = 0.0f;

    Prologue_ButtonHolderGroup.alpha = 0.0f;
    Prologue_Button_A.gameObject.SetActive(true);
    Prologue_Button_A.onClick.RemoveAllListeners();
    Prologue_Button_A.onClick.AddListener(Next);
    Prologue_ButtonText_A.text = GameManager.Instance.GetTextData("NEXT_TEXT");
    Prologue_Button_B.gameObject.SetActive(false);

    LayoutRebuilder.ForceRebuildLayoutImmediate(Prologue_ButtonHolderGroup.GetComponent<RectTransform>());
    LayoutRebuilder.ForceRebuildLayoutImmediate(Prologue_ButtonHolderGroup.transform.parent.GetComponent<RectTransform>());

    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("illust_start").Rect, GetPanelRect("illust_start").OutisdePos, GetPanelRect("illust_start").InsidePos, UIMoveInTime, true));
    yield return new WaitForSeconds(0.1f);
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("description_start").Rect, GetPanelRect("description_start").OutisdePos, GetPanelRect("description_start").InsidePos, UIMoveInTime, true));
    yield return new WaitForSeconds(0.5f);
    StartCoroutine(UIManager.Instance.ChangeAlpha(Prologue_IllustGroup, 1.0f, FadeInTime));
    StartCoroutine(UIManager.Instance.ChangeAlpha(Prologue_DescriptionGroup, 1.0f, FadeInTime));
    StartCoroutine(UIManager.Instance.ChangeAlpha(Prologue_ButtonHolderGroup, 1.0f, FadeInTime));
    Prologue_ButtonHolderGroup.interactable = true;
    Prologue_ButtonHolderGroup.blocksRaycasts = true;
    DefaultGroup.interactable = true;
    DefaultGroup.blocksRaycasts = true;
    yield return null;
  }
  public  void CloseUI_Prologue() => UIManager.Instance.AddUIQueue(closeui_prologue());
  private IEnumerator closeui_prologue()
  {
    CurrentPrologueIndex = 0;
    IsOpen = false;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("illust_start").Rect, GetPanelRect("illust_start").InsidePos, GetPanelRect("illust_start").OutisdePos, UIMoveOutTime, false));
    yield return new WaitForSeconds(0.1f);
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("description_start").Rect, GetPanelRect("description_start").InsidePos, GetPanelRect("description_start").OutisdePos, UIMoveOutTime, false));
    yield return new WaitForSeconds(0.5f);
    Prologue_ButtonText_A.gameObject.SetActive(true);
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
    Prologue_ButtonHolderGroup.interactable = false;

    UIManager.Instance.AddUIQueue(next());
  }
  private IEnumerator next()
  {
    CurrentPrologueIndex++;

    Sprite _illust = null;
    string _description = null;
    string _buttontext_a = null;
    string _buttontext_b = null;

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
          _buttontext_a = GameManager.Instance.GetTextData("NEXT_TEXT");
        }
        else
        {
          _illust = QuestHolder.Prologue_2_1_Illust;
          _description = QuestHolder.Prologue_2_1_Description;
          _buttontext_a = GameManager.Instance.GetTextData("NEXT_TEXT");
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
          _buttontext_a = GameManager.Instance.GetTextData("NEXT_TEXT");
        }
        else
        {
          _illust = QuestHolder.Prologue_4_1_Illust;
          _description = QuestHolder.Prologue_4_1_Description;
          _buttontext_a = GameManager.Instance.GetTextData("NEXT_TEXT");
        }
        break;
      case 5:
        _illust = QuestHolder.Prologue_5_Illust;
        _description = QuestHolder.Prologue_5_Description;
        _buttontext_a = GameManager.Instance.GetTextData("NEXT_TEXT");
        break;
      case 6:
        _illust = QuestHolder.Prologue_6_Illust;
        _description = QuestHolder.Prologue_6_Description;
        _buttontext_a = GameManager.Instance.GetTextData("NEXT_TEXT");
        break;
      case 7:
        _illust = QuestHolder.Prologue_7_Illust;
        _description = QuestHolder.Prologue_7_Description;
        _buttontext_a = GameManager.Instance.GetTextData("NEXT_TEXT");
        break;
      case 8:
        _illust = QuestHolder.Prologue_8_Illust;
        _description = QuestHolder.Prologue_8_Description;
        _buttontext_a = GameManager.Instance.GetTextData("NEXT_TEXT");
        break;
    }

    StartCoroutine(UIManager.Instance.ChangeAlpha(Prologue_IllustGroup, 0.0f, FadeOutTime));
    StartCoroutine(UIManager.Instance.ChangeAlpha(Prologue_DescriptionGroup, 0.0f, FadeOutTime));
    StartCoroutine(UIManager.Instance.ChangeAlpha(Prologue_ButtonHolderGroup, 0.0f, FadeOutTime));
    yield return new WaitForSeconds(FadeOutTime);

    Prologue_IllustImage.sprite = _illust;
    Prologue_Description.text = _description;
    Prologue_ButtonText_A.text= _buttontext_a;
    Prologue_Button_A.onClick.RemoveAllListeners();
    LayoutRebuilder.ForceRebuildLayoutImmediate(Prologue_ButtonHolderGroup.GetComponent<RectTransform>());
    LayoutRebuilder.ForceRebuildLayoutImmediate(Prologue_ButtonHolderGroup.transform.parent.GetComponent<RectTransform>());

    if (CurrentPrologueIndex == 8)                  //���ѷα� ������ �� - A ��Ȱ��ȭ
    {
      Prologue_Button_A.gameObject.SetActive(false);
    }
    else
    {
      if (_buttontext_b == null)                //������ ���� ��Ȳ - A �����ϰ� B ��Ȱ��ȭ
      {
        Prologue_Button_A.onClick.AddListener(Next);

        if (Prologue_Button_B.gameObject.activeInHierarchy == true) Prologue_Button_B.gameObject.SetActive(false);
      }
      else                                      //������ �ִ� ��Ȳ - A �����ϰ� B Ȱ��ȭ,����
      {
        Prologue_Button_A.onClick.AddListener(() => SelectTendency(0));

        if (Prologue_Button_B.gameObject.activeInHierarchy == false) Prologue_Button_B.gameObject.SetActive(true);
        Prologue_ButtonText_B.text = _buttontext_b;
      }
    }

    StartCoroutine(UIManager.Instance.ChangeAlpha(Prologue_IllustGroup, 1.0f, FadeInTime));
    StartCoroutine(UIManager.Instance.ChangeAlpha(Prologue_DescriptionGroup, 1.0f, FadeInTime));
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(Prologue_ButtonHolderGroup, 1.0f, FadeInTime));

    Prologue_ButtonHolderGroup.interactable = true;

    if (CurrentPrologueIndex == 8)
    {
      MoveRectForButton(0);
      UIManager.Instance.MapButton.Open(1, this);
    }
  }
  public void SelectTendency(int index)
  {
    if (UIManager.Instance.IsWorking) return;

    Prologue_ButtonHolderGroup.interactable = false;
    switch (CurrentPrologueIndex)
    {
      case 1:
        if (index == 0)
        {
          GameManager.Instance.MyGameData.Skill_Conversation.LevelByDefault += 1;
          GameManager.Instance.MyGameData.Tendency_Body.Level = -1;
        }
        else
        {
          GameManager.Instance.MyGameData.Skill_Force.LevelByDefault += 1;
          GameManager.Instance.MyGameData.Tendency_Body.Level = +1;
        }
        break;//(������+��ȭ)���� , (��ü��+����)����

      case 3:
        if (index == 0)
        {
          GameManager.Instance.MyGameData.Skill_Wild.LevelByDefault += 1;
          GameManager.Instance.MyGameData.Tendency_Head.Level = -1;
        }
        else
        {
          GameManager.Instance.MyGameData.Skill_Intelligence.LevelByDefault += 1;
          GameManager.Instance.MyGameData.Tendency_Head.Level = +1;
        }
        break;//(������+�ڿ�)���� , (������+����)����
    }

    UIManager.Instance.AddUIQueue(next());
  }
  #endregion

  #region ���൵ �̺�Ʈ
  [SerializeField] private CanvasGroup ProgressBackgroundGroup = null;
  [SerializeField] private RectTransform ProgresseventHolder = null;
  [SerializeField] private Vector2 ProgressHolder_TopPos= Vector2.zero;
  [SerializeField] private Vector2 ProgressHolder_DownPos = Vector2.zero;
  [SerializeField] private Image ProgressEventIllust = null;
  [SerializeField] private TextMeshProUGUI ProgressEventDescription = null;
  [SerializeField] private TextMeshProUGUI ProgressQuitButtonText = null;
  private bool IsProgressWorking=false;
  public void OpenProgressEvent(bool issabbat)
  {
    Sprite _illust = null;
    string _description = "";
    string _buttontext = "";
    switch (GameManager.Instance.MyGameData.Quest_Cult_Phase)
    {
      case 0:
        GameManager.Instance.MyGameData.Quest_Cult_Progress += ConstValues.Quest_Cult_Progress_Settlement;
        if (GameManager.Instance.MyGameData.Quest_Cult_Phase > 0)
        {
          var _tuple = QuestHolder.GetPhaseUpgradeData;
          _illust = _tuple.Item1;
          _description=_tuple.Item2;
          _buttontext = GameManager.Instance.GetTextData("NEXT_TEXT");
        }
        else
        {
          var _tuple = QuestHolder.GetSettlementData;
          _illust = _tuple.Item1;
          _description=_tuple.Item2;
          _buttontext=_tuple.Item3;
        }
        break;
      case 1:
        GameManager.Instance.MyGameData.Quest_Cult_Progress += issabbat ? ConstValues.Quest_Cult_Progress_Sector_Sabbat : ConstValues.Quest_Cult_Progress_Outer_Ritual;
        if (GameManager.Instance.MyGameData.Quest_Cult_Phase > 1)
        {
          var _tuple = QuestHolder.GetPhaseUpgradeData;
          _illust = _tuple.Item1;
          _description = _tuple.Item2;
          _buttontext = GameManager.Instance.GetTextData("NEXT_TEXT");
        }
        else
        {
          var _tuple = issabbat ? QuestHolder.GetSabbatData : QuestHolder.GetRitualData;
          _illust = _tuple.Item1;
          _description = _tuple.Item2;
          _buttontext = _tuple.Item3;
        }
        break;
      case 2:
        GameManager.Instance.MyGameData.Quest_Cult_Progress += issabbat ? ConstValues.Quest_Cult_Progress_Sector_Sabbat : ConstValues.Quest_Cult_Progress_Outer_Ritual;
        if (GameManager.Instance.MyGameData.Quest_Cult_Phase > 2)
        {
          var _tuple = QuestHolder.GetPhaseUpgradeData;
          _illust = _tuple.Item1;
          _description = _tuple.Item2;
          _buttontext = GameManager.Instance.GetTextData("NEXT_TEXT");
        }
        else
        {
          var _tuple = issabbat ? QuestHolder.GetSabbatData : QuestHolder.GetRitualData;
          _illust = _tuple.Item1;
          _description = _tuple.Item2;
          _buttontext = _tuple.Item3;
        }
        break;
    }

    ProgressEventIllust.sprite = _illust;
    ProgressEventDescription.text = _description;
    ProgressQuitButtonText.text = _buttontext;

    LayoutRebuilder.ForceRebuildLayoutImmediate(ProgresseventHolder);
    StartCoroutine(openprogress());
  }
  private IEnumerator openprogress()
  {
    IsProgressWorking = true;
    ProgressBackgroundGroup.blocksRaycasts = true;
    ProgressBackgroundGroup.interactable = true;
     yield return StartCoroutine(UIManager.Instance.moverect(ProgresseventHolder, ProgressHolder_TopPos, Vector2.zero, 1.0f, UIManager.Instance.UIPanelOpenCurve));
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
    ProgressBackgroundGroup.blocksRaycasts = false;
    ProgressBackgroundGroup.interactable = false;
    yield return StartCoroutine(UIManager.Instance.moverect(ProgresseventHolder,Vector2.zero,ProgressHolder_DownPos, 1.0f, UIManager.Instance.UIPanelCLoseCurve));
    IsProgressWorking = false;
  }

  #endregion
  public void CloseUI_Auto()
  {
    if (CurrentPrologueIndex == 8)
    {
      CloseUI_Prologue();
      CurrentPrologueIndex = -1;
    }
  }

}
