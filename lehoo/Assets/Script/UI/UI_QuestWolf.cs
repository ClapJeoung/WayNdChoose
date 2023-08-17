using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_QuestWolf : UI_default
{
  [SerializeField] private Canvas MyCanvas = null;
  [Space(10)]
  [SerializeField] private float UIMoveInTime = 0.7f;
  [SerializeField] private float UIMoveOutTime = 0.4f;//Rect 움직이는거
  [SerializeField] private float FadeInTime = 1.0f;
  [SerializeField] private float FadeOutTime = 0.4f;  //이미지,텍스트 투명도

  [SerializeField] private Image IllustImage = null;
  [SerializeField] private CanvasGroup IllustGroup = null;
  [SerializeField] private TextMeshProUGUI Description = null;
  [SerializeField] private CanvasGroup DescriptionGroup = null;
  [SerializeField] private CanvasGroup ButtonHolderGroup = null;
  [SerializeField] private Button Button_A = null;
  [SerializeField] private TextMeshProUGUI ButtonText_A = null;
  [SerializeField] private Button Button_B = null;
  [SerializeField] private TextMeshProUGUI ButtonText_B = null;
  public bool Skip = false;
  public bool PanelActive = false;
  private QuestHolder_Wolf QuestHolder = null;
  public int CurrentIndex = 0;
  public void OpenUI(QuestHolder_Wolf wolf)
  {
    if(DefaultRect.anchoredPosition!=Vector2.zero)DefaultRect.anchoredPosition = Vector2.zero;
    CurrentIndex = 0;
    QuestHolder = wolf;
    PanelActive = true;
    UIManager.Instance.AddUIQueue(openui());
  }
  private IEnumerator openui()
  {
    IllustImage.sprite = QuestHolder.Prologue_0_Illust;
    IllustGroup.alpha = 0.0f;
    Description.text = QuestHolder.Prologue_0_Description;
    DescriptionGroup.alpha = 0.0f;
    ButtonHolderGroup.alpha = 0.0f;
    Button_A.gameObject.SetActive(true);
    ButtonText_A.text = QuestHolder.Prologue_0_Selection;
    Button_B.gameObject.SetActive(false);
    Canvas.ForceUpdateCanvases();
    LayoutRebuilder.ForceRebuildLayoutImmediate(ButtonHolderGroup.GetComponent<RectTransform>());
    LayoutRebuilder.ForceRebuildLayoutImmediate(GetPanelRect("description").Rect); 
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("illust").Rect, GetPanelRect("illust").OutisdePos, GetPanelRect("illust").InsidePos, UIMoveInTime, UIManager.Instance.UIPanelOpenCurve));
    yield return new WaitForSeconds(0.1f);
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("description").Rect, GetPanelRect("description").OutisdePos, GetPanelRect("description").InsidePos, UIMoveInTime, UIManager.Instance.UIPanelOpenCurve));
    yield return new WaitForSeconds(0.5f);
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(IllustGroup, 1.0f, FadeInTime, false));
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionGroup, 1.0f, FadeInTime, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(ButtonHolderGroup, 1.0f, FadeInTime, false));
    Button_A.onClick.RemoveAllListeners();
    Button_A.onClick.AddListener(Next);
    ButtonHolderGroup.interactable = true;
    ButtonHolderGroup.blocksRaycasts = true;
    DefaultGroup.interactable = true;
    DefaultGroup.blocksRaycasts = true;
    yield return null;
  }
  public override void CloseUI() => UIManager.Instance.AddUIQueue(closeui());
  private IEnumerator closeui()
  {
    DefaultGroup.interactable = false;
    CurrentIndex = 0;
    PanelActive = false;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("illust").Rect, GetPanelRect("illust").InsidePos, GetPanelRect("illust").OutisdePos, UIMoveOutTime, UIManager.Instance.UIPanelCLoseCurve));
    yield return new WaitForSeconds(0.1f);
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("description").Rect, GetPanelRect("description").InsidePos, GetPanelRect("description").OutisdePos, UIMoveOutTime, UIManager.Instance.UIPanelCLoseCurve));
    yield return new WaitForSeconds(2.0f);
    ButtonText_A.gameObject.SetActive(true);
  }

  public void SelectTendency(int index)
  {
    ButtonHolderGroup.interactable = false;
    switch (CurrentIndex)
    {
      case 1:
        if (index == 0)
        {
          GameManager.Instance.MyGameData.Skill_Conversation.LevelByDefault += 1;
          GameManager.Instance.MyGameData.Tendency_Body.Level = -1;
          UIManager.Instance.AddUIQueue(setaftertendencypanel(QuestHolder.Prologue_Tendency_0_Selection_0_Illust, QuestHolder.Prologue_Tendency_0_Selection_0_Description, QuestHolder.Prologue_Tendency_0_Selection_0_Selection));
        }
        else
        {
          GameManager.Instance.MyGameData.Skill_Force.LevelByDefault += 1;
          GameManager.Instance.MyGameData.Tendency_Body.Level = +1;
          UIManager.Instance.AddUIQueue(setaftertendencypanel(QuestHolder.Prologue_Tendency_0_Selection_1_Illust, QuestHolder.Prologue_Tendency_0_Selection_1_Description, QuestHolder.Prologue_Tendency_0_Selection_1_Selection));
        }
        break;//(정신적+대화)선택 , (육체적+무력)선택

      case 2:
        if (index == 0)
        {
          GameManager.Instance.MyGameData.Skill_Wild.LevelByDefault += 1;
          GameManager.Instance.MyGameData.Tendency_Head.Level = -1;
          UIManager.Instance.AddUIQueue(setaftertendencypanel(QuestHolder.Prologue_Tendency_1_Selection_0_Illust, QuestHolder.Prologue_Tendency_1_Selection_0_Description, QuestHolder.Prologue_Tendency_1_Selection_0_Selection));
        }
        else
        {
          GameManager.Instance.MyGameData.Skill_Intelligence.LevelByDefault += 1;
          GameManager.Instance.MyGameData.Tendency_Head.Level = +1;
          UIManager.Instance.AddUIQueue(setaftertendencypanel(QuestHolder.Prologue_Tendency_1_Selection_1_Illust, QuestHolder.Prologue_Tendency_1_Selection_1_Description, QuestHolder.Prologue_Tendency_1_Selection_1_Selection));
        }
        break;//(감정적+자연)선택 , (물질적+지성)선택
    }
  }
  private IEnumerator setaftertendencypanel(Sprite illust,string description,string selection)
  {
    StartCoroutine(UIManager.Instance.ChangeAlpha(IllustGroup, 0.0f, FadeOutTime, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionGroup, 0.0f, FadeOutTime, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(ButtonHolderGroup, 0.0f, FadeOutTime, false));
    yield return new WaitForSeconds(FadeOutTime);
    IllustImage.sprite = illust;
    Description.text = description;
    ButtonText_A.text = selection;
    Button_A.onClick.RemoveAllListeners();
    Button_A.onClick.AddListener(() => Next());
    Button_B.gameObject.SetActive(false);
    Canvas.ForceUpdateCanvases();
    LayoutRebuilder.ForceRebuildLayoutImmediate(ButtonHolderGroup.GetComponent<RectTransform>());
    LayoutRebuilder.ForceRebuildLayoutImmediate(GetPanelRect("description").Rect);
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(IllustGroup, 1.0f, FadeInTime, false));
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionGroup, 1.0f, FadeInTime, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(ButtonHolderGroup, 1.0f, FadeInTime, false));

    ButtonHolderGroup.interactable = true;

    yield return new WaitForSeconds(0.4f);
  }
  public void Next()
  {
    if (Skip)
    {
      UIManager.Instance.AddUIQueue(setprologue_3());
      return;
    }
    CurrentIndex++;
    ButtonHolderGroup.interactable = false;
    switch (CurrentIndex)
    {
      case 1:
        UIManager.Instance.AddUIQueue(setprologue_1());
        break;
      case 2:
        UIManager.Instance.AddUIQueue(setprologue_2());
        break;
      case 3:
        UIManager.Instance.AddUIQueue(setprologue_3());
        break;
    }
  }
  private IEnumerator setprologue_1()
  {
    StartCoroutine(UIManager.Instance.ChangeAlpha(IllustGroup, 0.0f, FadeOutTime, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionGroup, 0.0f, FadeOutTime, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(ButtonHolderGroup, 0.0f, FadeOutTime, false));
    yield return new WaitForSeconds(FadeOutTime);
    Button_B.gameObject.SetActive(true);
    IllustImage.sprite = QuestHolder.Prologue_Tendency_0_Illust;
    Description.text = QuestHolder.Prologue_Tendency_0_Description;
    ButtonText_A.text = QuestHolder.Prologue_Tendency_0_Selection_0;
    ButtonText_B.text = QuestHolder.Prologue_Tendency_0_Selection_1;
    Button_A.onClick.RemoveAllListeners();
    Button_A.onClick.AddListener(()=>SelectTendency(0));
    Button_B.onClick.AddListener(()=>SelectTendency(1));
    Canvas.ForceUpdateCanvases();
    LayoutRebuilder.ForceRebuildLayoutImmediate(ButtonHolderGroup.GetComponent<RectTransform>());
    LayoutRebuilder.ForceRebuildLayoutImmediate(GetPanelRect("description").Rect);
    yield return new WaitForEndOfFrame();
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(IllustGroup, 1.0f, FadeInTime, false));
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionGroup, 1.0f, FadeInTime, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(ButtonHolderGroup, 1.0f, FadeInTime, false));

    ButtonHolderGroup.interactable = true;

    yield return new WaitForSeconds(0.4f);
  }//정신적,육체적 선택지 세팅
  private IEnumerator setprologue_2()
  {
    StartCoroutine(UIManager.Instance.ChangeAlpha(IllustGroup, 0.0f, FadeOutTime, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionGroup, 0.0f, FadeOutTime, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(ButtonHolderGroup, 0.0f, FadeOutTime, false));
    yield return new WaitForSeconds(FadeOutTime);
    Button_B.gameObject.SetActive(true);
    IllustImage.sprite = QuestHolder.Prologue_Tendency_1_Illust;
    Description.text = QuestHolder.Prologue_Tendency_1_Description;
    ButtonText_A.text = QuestHolder.Prologue_Tendency_1_Selection_0;
    ButtonText_B.text = QuestHolder.Prologue_Tendency_1_Selection_1;
    Button_A.onClick.RemoveAllListeners();
    Button_A.onClick.AddListener(() => SelectTendency(0));
    Button_B.onClick.AddListener(() => SelectTendency(1));
    Canvas.ForceUpdateCanvases();
    LayoutRebuilder.ForceRebuildLayoutImmediate(ButtonHolderGroup.GetComponent<RectTransform>());
    LayoutRebuilder.ForceRebuildLayoutImmediate(GetPanelRect("description").Rect);
    yield return new WaitForEndOfFrame();
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(IllustGroup, 1.0f, FadeInTime, false));
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionGroup, 1.0f, FadeInTime, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(ButtonHolderGroup, 1.0f, FadeInTime, false));

    ButtonHolderGroup.interactable = true;

    yield return new WaitForSeconds(0.4f);
  }//감정적,물질적 선택지 세팅
  private IEnumerator setprologue_3()
  {
    StartCoroutine(UIManager.Instance.ChangeAlpha(IllustGroup, 0.0f, FadeOutTime, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionGroup, 0.0f, FadeOutTime, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(ButtonHolderGroup, 0.0f, FadeOutTime, false));
    yield return new WaitForSeconds(FadeOutTime);
    Button_A.gameObject.SetActive(false);
    Button_B.gameObject.SetActive(false);

    IllustImage.sprite = QuestHolder.Prologue_Tendency_Last_Illust;
    Description.text = QuestHolder.Prologue_Last_Description;

    Canvas.ForceUpdateCanvases();
    LayoutRebuilder.ForceRebuildLayoutImmediate(ButtonHolderGroup.GetComponent<RectTransform>());
    LayoutRebuilder.ForceRebuildLayoutImmediate(GetPanelRect("description").Rect);

    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(IllustGroup, 1.0f, FadeInTime, false));
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionGroup, 1.0f, FadeInTime, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(ButtonHolderGroup, 1.0f, FadeInTime, false));

    MoveRectForButton(0);
    UIManager.Instance.MapButton.Open(1, this);

  }//지도 여는 상황 세팅

}
