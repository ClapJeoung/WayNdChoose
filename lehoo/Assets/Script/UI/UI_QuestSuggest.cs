using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_QuestSuggest : UI_default
{
  [SerializeField] private float QuestUIMoveTime = 0.6f;
  [SerializeField] private float QuestUIFadeTime = 0.4f;
  private WaitForSeconds Wait=new WaitForSeconds(0.8f);
  [SerializeField] private RectTransform QuestNameRect = null;
  [SerializeField] private CanvasGroup QuestNameGroup = null;
  [SerializeField] private TextMeshProUGUI QuestName = null;
  private Vector2 QuestNameClosePos = new Vector2(0.0f, 650.0f);
  private Vector2 QuestNameOpenPos = new Vector2(0.0f, 350.0f);
  [Space(10)]
  [SerializeField] private RectTransform QuestIllustRect = null;
  [SerializeField] private CanvasGroup QuestIllustGroup = null;
  [SerializeField] private Image QuestIllust = null;
  private Vector2 QuestIllustClosePos = new Vector2(-1150.0f,-50.0f);
  private Vector2 QuestIllustOpenPos = new Vector2(-415.0f, -50.0f);
  [Space(10)]
  [SerializeField] private RectTransform QuestDescriptionRect = null;
  [SerializeField] private CanvasGroup QuestDescriptionGroup = null;
  [SerializeField] private TextMeshProUGUI QuestDescriptionText_size = null;
  [SerializeField] private TextMeshProUGUI QuestDescriptionText = null;
  private Vector2 QuestDescriptionClosePos = new Vector2(1420.0f, 230.0f);
  private Vector2 QuestDescriptionOpenPos = new Vector2(245.0f, 230.0f);
  [Space(10)]
  [SerializeField] private CanvasGroup NextToExpButtonGroup = null;
  [SerializeField] private TextMeshProUGUI NextToExpButtonText = null;
  [Space(10)]
  [SerializeField] private CanvasGroup ExpGroup = null;
  [SerializeField] private TextMeshProUGUI[] ExpNames = new TextMeshProUGUI[4];
  [SerializeField] private PreviewInteractive[] ExpPreviews=new PreviewInteractive[4];
  private Experience[] QuestExps=new Experience[4];
  [Space(10)]
  [SerializeField] private CanvasGroup MapButtonGroup = null;
  [SerializeField] private TextMeshProUGUI MapButtonText = null;
  [Space(10)]
  [SerializeField] private CanvasGroup RewardExpGroup = null;

  [SerializeField] private CanvasGroup[] RewardLongExpNameGroup = new CanvasGroup[2];
  [SerializeField] private TextMeshProUGUI[] RewardLongExpName = new TextMeshProUGUI[2];
  [SerializeField] private Image[] RewardLongExpCap = new Image[2];
  [SerializeField] private Image[] RewardLongExpIllust = new Image[2];
  [SerializeField] private CanvasGroup[] RewardLongExpTurnGroup = new CanvasGroup[2];
  [SerializeField] private TextMeshProUGUI[] RewardLongExpTurn = new TextMeshProUGUI[2];
  [SerializeField] private PreviewInteractive[] RewardLongExpPreview = new PreviewInteractive[2];

  [SerializeField] private CanvasGroup[] RewardShortExpNameGroup = new CanvasGroup[4];
  [SerializeField] private TextMeshProUGUI[] RewardShortExpName = new TextMeshProUGUI[4];
  [SerializeField] private Image[] RewardShortExpCap = new Image[4];
  [SerializeField] private Image[] RewardShortExpIllust = new Image[4];
  [SerializeField] private CanvasGroup[] RewardShortExpTurnGroup = new CanvasGroup[4];
  [SerializeField] private TextMeshProUGUI[] RewardShortExpTurn = new TextMeshProUGUI[4];
  [SerializeField] private PreviewInteractive[] RewardShortExpPreview = new PreviewInteractive[4];
  [SerializeField] private TextMeshProUGUI RewardExpDescription = null;

  public bool IsActivePanel = false;
  public void OpenQuestSuggestUI()
  {
    IsActivePanel = true;
    UIManager.Instance.AddUIQueue(setquestsuggest_beginning());
  }
  private IEnumerator setquestsuggest_beginning()
  {
    NextToExpButtonText.text = GameManager.Instance.GetTextData("next").Name;
    QuestHolder _quest = GameManager.Instance.MyGameData.CurrentQuest;

    QuestExps = _quest.StartingExps;
    for (int i = 0; i < QuestExps.Length; i++)
    {
      ExpNames[i].text =GameManager.Instance.GetTextData((ThemeType)i).Icon+" "+ QuestExps[i].Name;
      ExpPreviews[i].MyEXP = QuestExps[i];
    }

    QuestName.text = _quest.QuestName;
    StartCoroutine(UIManager.Instance.ChangeAlpha(QuestNameGroup, 1.0f, QuestUIFadeTime, false));
    StartCoroutine(UIManager.Instance.moverect(QuestNameRect, QuestNameClosePos, QuestNameOpenPos, QuestUIMoveTime, UIManager.Instance.UIPanelOpenCurve));
    yield return Wait;

    QuestIllust.sprite = _quest.StartIllust;
    StartCoroutine(UIManager.Instance.ChangeAlpha(QuestIllustGroup,1.0f,QuestUIFadeTime,false));
    StartCoroutine(UIManager.Instance.moverect(QuestIllustRect, QuestIllustClosePos, QuestIllustOpenPos, QuestUIMoveTime, UIManager.Instance.UIPanelOpenCurve));
    yield return Wait;

    QuestDescriptionText_size.text = _quest.StartDialogue;
    QuestDescriptionText.text = _quest.StartDialogue;
    StartCoroutine(UIManager.Instance.ChangeAlpha(QuestDescriptionGroup, 1.0f, QuestUIFadeTime, false));
    yield return StartCoroutine(UIManager.Instance.moverect(QuestDescriptionRect, QuestDescriptionClosePos, QuestDescriptionOpenPos, QuestUIMoveTime, UIManager.Instance.UIPanelOpenCurve));

    StartCoroutine(UIManager.Instance.ChangeAlpha(NextToExpButtonGroup,1.0f, 0.4f, false));
  }
  public void OpenExpSelection()
  {
    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(NextToExpButtonGroup, 0.0f, false, false));
    UIManager.Instance.AddUIQueue(setquestsuggest_exp());
  }
  private IEnumerator setquestsuggest_exp()
  {
    QuestHolder _quest = GameManager.Instance.MyGameData.CurrentQuest;

    StartCoroutine(UIManager.Instance.ChangeAlpha(QuestIllust,0.0f,QuestUIFadeTime));
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(QuestDescriptionText, 0.0f, QuestUIFadeTime));

    QuestIllust.sprite = _quest.ExpSelectionIllust;
    StartCoroutine(UIManager.Instance.ChangeAlpha(QuestIllust, 1.0f, QuestUIFadeTime));
    QuestDescriptionText_size.text = _quest.ExpSelectionDescription;
    QuestDescriptionText.text=_quest.ExpSelectionDescription;
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(QuestDescriptionText,1.0f, QuestUIFadeTime));
    yield return Wait;
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(ExpGroup, 1.0f, QuestUIFadeTime, false));
  }
  public void SelectExp(int index)
  {
    SelectedExp=QuestExps[index];
  }
  private Experience SelectedExp = null;
  public void AddRewardExp_Long()
  {
    if (UIManager.Instance.IsWorking) return;

    if (GameManager.Instance.MyGameData.LongTermEXP == null) GameManager.Instance.AddLongExp(SelectedExp);
    else GameManager.Instance.ShiftLongExp(SelectedExp);
    UIManager.Instance.UpdateExpLongTermIcon();
    StartCoroutine (UIManager.Instance.ChangeAlpha(RewardExpGroup, 0.0f, 0.3f, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(ExpGroup, 0.0f, QuestUIFadeTime, false));
    GameManager.Instance.MyGameData.CurrentQuest.CurrentSequence = QuestSequence.Rising;
  }
  public void AddRewardExp_Short(int _expindex)
  {
    if (UIManager.Instance.IsWorking) return;

    if (GameManager.Instance.MyGameData.ShortTermEXP[_expindex] == null) GameManager.Instance.AddShortExp(SelectedExp, _expindex);
    else GameManager.Instance.ShiftShortExp(SelectedExp, _expindex);
    UIManager.Instance.UpdateExpShortTermIcon();
    StartCoroutine(UIManager.Instance.ChangeAlpha(RewardExpGroup, 0.0f, 0.3f, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(ExpGroup, 0.0f, QuestUIFadeTime, false));
    GameManager.Instance.MyGameData.CurrentQuest.CurrentSequence = QuestSequence.Rising;
  }
  public void OpenRewardExpPanel_reward()
  {
    if (UIManager.Instance.IsWorking) return;
    for (int i = 0; i < 2; i++)
    {
      Experience _longexp = GameManager.Instance.MyGameData.LongTermEXP;
      if (_longexp == null)
      {
        RewardLongExpNameGroup[i].alpha = 0.0f;
        RewardLongExpCap[i].enabled = true;
        RewardLongExpTurnGroup[i].alpha = 0.0f;
      }//해당 슬롯이 비었다면 이름, 턴 끄고 닫힌 일러스트
      else
      {
        RewardLongExpNameGroup[i].alpha = 1.0f;
        RewardLongExpName[i].text = _longexp.Name;
        RewardLongExpCap[i].enabled = false;
        RewardLongExpIllust[i].sprite = _longexp.Illust;
        RewardLongExpTurnGroup[i].alpha = 1.0f;
        RewardLongExpTurn[i].text = _longexp.Duration.ToString();
      }//해당 슬롯에 이미 경험이 있다면 이름,일러스트,턴 표기
      RewardLongExpPreview[i].MyEXP = SelectedExp;
    }
    for (int i = 0; i < 4; i++)
    {
      Experience _shortexp = GameManager.Instance.MyGameData.ShortTermEXP[i];
      if (_shortexp == null)
      {
        RewardShortExpNameGroup[i].alpha = 0.0f;
        RewardShortExpCap[i].enabled = true;
        RewardShortExpTurnGroup[i].alpha = 0.0f;
      }//해당 슬롯이 비었다면 이름, 턴 끄고 닫힌 일러스트
      else
      {
        RewardShortExpNameGroup[i].alpha = 1.0f;
        RewardShortExpName[i].text = _shortexp.Name;
        RewardShortExpCap[i].enabled = false;
        RewardShortExpIllust[i].sprite = _shortexp.Illust;
        RewardShortExpTurnGroup[i].alpha = 1.0f;
        RewardShortExpTurn[i].text = _shortexp.Duration.ToString();
      }//해당 슬롯에 이미 경험이 있다면 이름,일러스트,턴 표기
      RewardShortExpPreview[i].MyEXP = SelectedExp;
    }
    RewardExpDescription.text = GameManager.Instance.GetTextData("savetheexp").Name;
    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(RewardExpGroup, 1.0f, UIManager.Instance.SmallPanelFadeTime, false));
  }

  public void OpenStarting()
  {
    UIManager.Instance.AddUIQueue(setquestsuggest_starting());
  }
  private IEnumerator setquestsuggest_starting()
  {
    QuestHolder _quest = GameManager.Instance.MyGameData.CurrentQuest;

    StartCoroutine(UIManager.Instance.ChangeAlpha(QuestIllust, 0.0f, QuestUIFadeTime));
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(QuestDescriptionText, 0.0f, QuestUIFadeTime));

    yield return Wait;

    QuestIllust.sprite = _quest.StartingIllust;
    StartCoroutine(UIManager.Instance.ChangeAlpha(QuestIllust, 1.0f, QuestUIFadeTime));
    QuestDescriptionText_size.text =string.Format(_quest.StartingDescription,SelectedExp.Name);
    QuestDescriptionText.text = string.Format(_quest.StartingDescription, SelectedExp.Name);
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(QuestDescriptionText, 1.0f, QuestUIFadeTime));

    yield return Wait;

    MapButtonText.text = _quest.StartingSelection;
    StartCoroutine(UIManager.Instance.ChangeAlpha(MapButtonGroup, 1.0f, QuestUIFadeTime, false));
  }
  public void CloseQuestUI()
  {
    IsActivePanel = false;
    StartCoroutine(closequestui());
  }
  private IEnumerator closequestui()
  {
    StartCoroutine(UIManager.Instance.ChangeAlpha(MapButtonGroup,0.0f,QuestUIFadeTime,false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(QuestNameGroup,0.0f, QuestUIFadeTime, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(QuestIllustGroup,0.0f, QuestUIFadeTime, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(QuestDescriptionGroup,0.0f, QuestUIFadeTime, false));
    yield return Wait;
    QuestNameRect.anchoredPosition = QuestNameClosePos;
    QuestIllustRect.anchoredPosition = QuestIllustClosePos;
    QuestDescriptionRect.anchoredPosition = QuestDescriptionClosePos;
  }
}
