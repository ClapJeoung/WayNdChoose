using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_QuestWolf : UI_default
{
  [Space(10)]
  [SerializeField] private UI_Settlement SettlementUI = null;
  public bool Skip = false;
  [SerializeField] private float UIMoveInTime = 0.7f;
  [SerializeField] private float UIMoveOutTime = 0.4f;//Rect 움직이는거
  [SerializeField] private float FadeInTime = 1.0f;
  [SerializeField] private float FadeOutTime = 0.4f;  //이미지,텍스트 투명도
  private QuestHolder_Cult QuestHolder = null;

  #region 프롤로그
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

    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("illust_start").Rect, GetPanelRect("illust_start").OutisdePos, GetPanelRect("illust_start").InsidePos, UIMoveInTime, UIManager.Instance.UIPanelOpenCurve));
    yield return new WaitForSeconds(0.1f);
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("description_start").Rect, GetPanelRect("description_start").OutisdePos, GetPanelRect("description_start").InsidePos, UIMoveInTime, UIManager.Instance.UIPanelOpenCurve));
    yield return new WaitForSeconds(0.5f);
    StartCoroutine(UIManager.Instance.ChangeAlpha(Prologue_IllustGroup, 1.0f, FadeInTime, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(Prologue_DescriptionGroup, 1.0f, FadeInTime, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(Prologue_ButtonHolderGroup, 1.0f, FadeInTime, false));
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
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("illust_start").Rect, GetPanelRect("illust_start").InsidePos, GetPanelRect("illust_start").OutisdePos, UIMoveOutTime, UIManager.Instance.UIPanelCLoseCurve));
    yield return new WaitForSeconds(0.1f);
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("description_start").Rect, GetPanelRect("description_start").InsidePos, GetPanelRect("description_start").OutisdePos, UIMoveOutTime, UIManager.Instance.UIPanelCLoseCurve));
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
        UIManager.Instance.QuestSidePanel_Cult.UpdateUI();
        GameManager.Instance.MyGameData.Quest_Cult_Sabbat_TokenedSectors.Add(SectorType.Residence, 0);
        GameManager.Instance.MyGameData.Quest_Cult_Sabbat_TokenedSectors.Add(SectorType.Temple, 0);
        GameManager.Instance.MyGameData.Quest_Cult_Sabbat_TokenedSectors.Add(SectorType.Marketplace, 0);
        GameManager.Instance.MyGameData.Quest_Cult_Sabbat_TokenedSectors.Add(SectorType.Library, 0);
        break;
    }

    StartCoroutine(UIManager.Instance.ChangeAlpha(Prologue_IllustGroup, 0.0f, FadeOutTime, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(Prologue_DescriptionGroup, 0.0f, FadeOutTime, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(Prologue_ButtonHolderGroup, 0.0f, FadeOutTime, false));
    yield return new WaitForSeconds(FadeOutTime);

    Prologue_IllustImage.sprite = _illust;
    Prologue_Description.text = _description;
    Prologue_ButtonText_A.text= _buttontext_a;
    Prologue_Button_A.onClick.RemoveAllListeners();
    LayoutRebuilder.ForceRebuildLayoutImmediate(Prologue_ButtonHolderGroup.GetComponent<RectTransform>());
    LayoutRebuilder.ForceRebuildLayoutImmediate(Prologue_ButtonHolderGroup.transform.parent.GetComponent<RectTransform>());

    if (CurrentPrologueIndex == 8)                  //프롤로그 종료할 때 - A 비활성화
    {
      Prologue_Button_A.gameObject.SetActive(false);
    }
    else
    {
      if (_buttontext_b == null)                //선택지 없는 상황 - A 세팅하고 B 비활성화
      {
        Prologue_Button_A.onClick.AddListener(Next);

        if (Prologue_Button_B.gameObject.activeInHierarchy == true) Prologue_Button_B.gameObject.SetActive(false);
      }
      else                                      //선택지 있는 상황 - A 세팅하고 B 활성화,세팅
      {
        Prologue_Button_A.onClick.AddListener(() => SelectTendency(0));

        if (Prologue_Button_B.gameObject.activeInHierarchy == false) Prologue_Button_B.gameObject.SetActive(true);
        Prologue_ButtonText_B.text = _buttontext_b;
      }
    }

    StartCoroutine(UIManager.Instance.ChangeAlpha(Prologue_IllustGroup, 1.0f, FadeInTime, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(Prologue_DescriptionGroup, 1.0f, FadeInTime, false));
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(Prologue_ButtonHolderGroup, 1.0f, FadeInTime, false));

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
        break;//(정신적+대화)선택 , (육체적+무력)선택

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
        break;//(감정적+자연)선택 , (물질적+지성)선택
    }

    UIManager.Instance.AddUIQueue(next());
  }
  #endregion

  #region 탐문
  [Space(5)]
  [SerializeField] private Image Searching_IllustImage = null;
  [SerializeField] private TextMeshProUGUI Searching_Description = null;
  [SerializeField] private CanvasGroup Searching_RewardButton_Group = null;
  [SerializeField] private Button Searching_RewardButton = null;
  [SerializeField] private TextMeshProUGUI Searching_ButtonText = null;
  [SerializeField] private Button Searching_NextButton = null;
  [SerializeField] private TextMeshProUGUI Searching_NextButtonText = null;
  public void OpenUI_Searching()
  {
    if (DefaultRect.anchoredPosition != Vector2.zero) DefaultRect.anchoredPosition = Vector2.zero;
    IsOpen = true;

    UIManager.Instance.AddUIQueue(openui_searching());
  }
  private IEnumerator openui_searching()
  {
    Sprite _illust = null;
    string _description = "";

    switch (GameManager.Instance.MyGameData.Quest_Cult_Progress)
    {
      case 0:
        _illust = QuestHolder.Searching_0_Illust;
        _description = QuestHolder.Searching_0_Description;
        break;
      case 1:
        _illust = QuestHolder.Searching_1_Illust;
        _description = QuestHolder.Searching_1_Description;
        break;
    }

    Searching_IllustImage.sprite = _illust;
    Searching_Description.text = _description;
    Searching_ButtonText.text = $"{GameManager.Instance.GetTextData(StatusType.Sanity, 2)} {WNCText.GetSanityColor(ConstValues.Quest_Wolf_Searching_Sanityrewardvalue)}";
  
    Searching_RewardButton_Group.alpha = 1.0f;
    Searching_RewardButton_Group.interactable = true;
    Searching_RewardButton_Group.blocksRaycasts = true;
    Searching_RewardButton.interactable = true;

    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("illust_searching").Rect, GetPanelRect("illust_searching").OutisdePos, GetPanelRect("illust_searching").InsidePos, UIMoveInTime, UIManager.Instance.UIPanelOpenCurve));
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("description_searching").Rect, GetPanelRect("description_searching").OutisdePos, GetPanelRect("description_searching").InsidePos, UIMoveInTime, UIManager.Instance.UIPanelOpenCurve));

    if (GameManager.Instance.MyGameData.Quest_Cult_Progress == 1)
    {
      StartCoroutine(UIManager.Instance.moverect(GetPanelRect("nextbutton_searching").Rect, GetPanelRect("nextbutton_searching").OutisdePos, GetPanelRect("nextbutton_searching").InsidePos, UIMoveInTime, UIManager.Instance.UIPanelOpenCurve));
      if (Searching_NextButton.interactable == false) Searching_NextButton.interactable = true;
      Searching_NextButtonText.text = GameManager.Instance.GetTextData("Quest0_Sidepanel_Searching_Finish");
    }
    else
    {
      UIManager.Instance.SettleButton.Open(1, this);
    }
    UIManager.Instance.QuestSidePanel_Cult.UpdateUI();

    GameManager.Instance.MyGameData.Quest_Cult_Progress++;

    yield return null;
  }
  public void SearchingSanityReward()
  {
    if (UIManager.Instance.IsWorking) return;
    GameManager.Instance.MyGameData.CurrentSanity += ConstValues.Quest_Wolf_Searching_Sanityrewardvalue;
    StartCoroutine(UIManager.Instance.ChangeAlpha(Searching_RewardButton_Group, 0.0f, 0.4f, false));
  }
  public void CloseUI_Searching()
  {
    IsOpen = false;

    UIManager.Instance.AddUIQueue(closeui_searching());
  }
  private IEnumerator closeui_searching()
  {
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("illust_searching").Rect, GetPanelRect("illust_searching").InsidePos, GetPanelRect("illust_searching").OutisdePos, UIMoveOutTime, UIManager.Instance.UIPanelCLoseCurve));
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("description_searching").Rect, GetPanelRect("description_searching").InsidePos, GetPanelRect("description_searching").OutisdePos, UIMoveOutTime, UIManager.Instance.UIPanelCLoseCurve));
    yield return null;
  }
  #endregion

  #region 선택
  [Space(5)]
  [SerializeField] private TextMeshProUGUI Wanted_Description = null;
  [SerializeField] private CanvasGroup Wanted_Sabbat_Group = null;
  [SerializeField] private TextMeshProUGUI Wanted_Sabbat_Description = null;
  [SerializeField] private CanvasGroup Wanted_Ritual_Group = null;
  [SerializeField] private TextMeshProUGUI Wanted_Ritual_Description = null;
  [Space(10)]
  [SerializeField] private Image WantedResult_Icon = null;
  [SerializeField] private Image WantedResult_Illust = null;
  [SerializeField] private TextMeshProUGUI WantedResult_Description = null;
  [SerializeField] private TextMeshProUGUI WantedResult_SettlebuttonText = null;

  public void NextToChoose()
  {
    if (UIManager.Instance.IsWorking) return;
    UIManager.Instance.AddUIQueue(nexttowanted());
  }
  private IEnumerator nexttowanted()
  {
    if (Wanted_Sabbat_Group.alpha == 0.0f)
    {
      Wanted_Sabbat_Group.alpha = 1.0f;
      Wanted_Sabbat_Group.interactable = true;
      Wanted_Sabbat_Group.blocksRaycasts = true;
      Wanted_Ritual_Group.alpha = 1.0f;
      Wanted_Ritual_Group.interactable = true;
      Wanted_Ritual_Group.blocksRaycasts = true;
    }
    Wanted_Description.text = QuestHolder.Wanted_Description;
    Wanted_Sabbat_Description.text = QuestHolder.Wanted_Description_Sabbat;
    Wanted_Ritual_Description.text = QuestHolder.Wanted_Description_Ritual;
    WaitForSeconds _wait = new WaitForSeconds(0.3f);

    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("nextbutton_searching").Rect, GetPanelRect("nextbutton_searching").InsidePos, GetPanelRect("nextbutton_searching").OutisdePos, UIMoveOutTime, UIManager.Instance.UIPanelCLoseCurve));
    yield return _wait;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("illust_searching").Rect, GetPanelRect("illust_searching").InsidePos, GetPanelRect("illust_searching").OutisdePos, UIMoveOutTime, UIManager.Instance.UIPanelCLoseCurve));
    yield return _wait;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("description_searching").Rect, GetPanelRect("description_searching").InsidePos, GetPanelRect("description_searching").OutisdePos, UIMoveOutTime, UIManager.Instance.UIPanelCLoseCurve));
    yield return new WaitForSeconds(1.0f);
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("wanted_description").Rect, GetPanelRect("wanted_description").OutisdePos, GetPanelRect("wanted_description").InsidePos, UIMoveInTime, UIManager.Instance.UIPanelOpenCurve));
    yield return _wait;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("wanted_cult").Rect, GetPanelRect("wanted_cult").OutisdePos, GetPanelRect("wanted_cult").InsidePos, UIMoveInTime, UIManager.Instance.UIPanelOpenCurve));
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("wanted_wolf").Rect, GetPanelRect("wanted_wolf").OutisdePos, GetPanelRect("wanted_wolf").InsidePos, UIMoveInTime, UIManager.Instance.UIPanelOpenCurve));
  }
  public void SelectCultLine()
  {
    if (UIManager.Instance.IsWorking) return;
    UIManager.Instance.AddUIQueue(selectcultline());
  }
  private IEnumerator selectcultline()
  {
    GameManager.Instance.MyGameData.Quest_Cult_Phase = 1;
    GameManager.Instance.MyGameData.Quest_Cult_Type = 0;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("wanted_description").Rect, GetPanelRect("wanted_description").OutisdePos, GetPanelRect("wanted_description").InsidePos, UIMoveOutTime, UIManager.Instance.UIPanelOpenCurve));
    yield return new WaitForSeconds(0.3f);
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("wanted_wolf").Rect, GetPanelRect("wanted_wolf").InsidePos, GetPanelRect("wanted_wolf").OutisdePos, UIMoveOutTime, UIManager.Instance.UIPanelCLoseCurve));
    yield return new WaitForSeconds(0.3f);
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("wanted_cult").Rect, GetPanelRect("wanted_cult").InsidePos, GetPanelRect("wanted_cult").OutisdePos, UIMoveOutTime, UIManager.Instance.UIPanelCLoseCurve));
    yield return new WaitForSeconds(1.0f);

    WantedResult_Icon.sprite = GameManager.Instance.ImageHolder.QuestIcon_Cult;
    WantedResult_Illust.sprite = QuestHolder.Wanted_Sabbat_Illust;
    WantedResult_Description.text = QuestHolder.Wanted_Sabbat_Description;
    WantedResult_SettlebuttonText.text = GameManager.Instance.GetTextData("GOTOSETTLEMENT");
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("wantedresult_illust").Rect, GetPanelRect("wantedresult_illust").OutisdePos, GetPanelRect("wantedresult_illust").InsidePos, UIMoveInTime, UIManager.Instance.UIPanelOpenCurve));
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("wantedresult_description").Rect, GetPanelRect("wantedresult_description").OutisdePos, GetPanelRect("wantedresult_description").InsidePos, UIMoveInTime, UIManager.Instance.UIPanelOpenCurve));
  
    UIManager.Instance.QuestSidePanel_Cult.UpdateUI();
  }
  public void SelectWolfLine()
  {
    if (UIManager.Instance.IsWorking) return;
    UIManager.Instance.AddUIQueue(selectwolfline());
  }
  private IEnumerator selectwolfline()
  {
    GameManager.Instance.MyGameData.Quest_Cult_Phase = 1;
    GameManager.Instance.MyGameData.Quest_Cult_Type = 1;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("wanted_description").Rect, GetPanelRect("wanted_description").OutisdePos, GetPanelRect("wanted_description").InsidePos, UIMoveOutTime, UIManager.Instance.UIPanelOpenCurve));
    yield return new WaitForSeconds(0.3f);
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("wanted_cult").Rect, GetPanelRect("wanted_cult").InsidePos, GetPanelRect("wanted_cult").OutisdePos, UIMoveOutTime, UIManager.Instance.UIPanelCLoseCurve));
    yield return new WaitForSeconds(0.3f);
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("wanted_wolf").Rect, GetPanelRect("wanted_wolf").InsidePos, GetPanelRect("wanted_wolf").OutisdePos, UIMoveOutTime, UIManager.Instance.UIPanelCLoseCurve));
    yield return new WaitForSeconds(1.0f);

    WantedResult_Icon.sprite = GameManager.Instance.ImageHolder.QuestIcon_Wolf;
    WantedResult_Illust.sprite = QuestHolder.Wanted_Ritual_Illust;
    WantedResult_Description.text = QuestHolder.Wanted_Ritual_Description;
    WantedResult_SettlebuttonText.text = GameManager.Instance.GetTextData("GOTOSETTLEMENT");
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("wantedresult_illust").Rect, GetPanelRect("wantedresult_illust").OutisdePos, GetPanelRect("wantedresult_illust").InsidePos, UIMoveInTime, UIManager.Instance.UIPanelOpenCurve));
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("wantedresult_description").Rect, GetPanelRect("wantedresult_description").OutisdePos, GetPanelRect("wantedresult_description").InsidePos, UIMoveInTime, UIManager.Instance.UIPanelOpenCurve));

    UIManager.Instance.QuestSidePanel_Cult.UpdateUI();
  }
  public void WantedResult_GoSettlement()
  {
    if (UIManager.Instance.IsWorking) return;
    IsOpen = false;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("wantedresult_illust").Rect, GetPanelRect("wantedresult_illust").InsidePos, GetPanelRect("wantedresult_illust").OutisdePos, UIMoveOutTime, UIManager.Instance.UIPanelOpenCurve));
    UIManager.Instance.AddUIQueue(UIManager.Instance.moverect(GetPanelRect("wantedresult_description").Rect, GetPanelRect("wantedresult_description").InsidePos, GetPanelRect("wantedresult_description").OutisdePos, UIMoveOutTime, UIManager.Instance.UIPanelOpenCurve));
    SettlementUI.OpenUI();
  }
  #endregion
  public void CloseUI_Auto()
  {
    switch (GameManager.Instance.MyGameData.Quest_Cult_Phase)
    {
      case 0:
        if (CurrentPrologueIndex == 8) CloseUI_Prologue();
        else
        {
          CloseUI_Searching();
        }
        break;
    }
  }
}
