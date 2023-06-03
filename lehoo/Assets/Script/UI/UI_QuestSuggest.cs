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
  private Vector2 QuestNameClosePos = new Vector2(500.0f, 0.0f);
  private Vector2 QuestNameOpenPos = new Vector2(350.0f, 0.0f);
  [Space(10)]
  [SerializeField] private RectTransform QuestIllustRect = null;
  [SerializeField] private CanvasGroup QuestIllustGroup = null;
  [SerializeField] private Image QuestIllust = null;
  private Vector2 QuestIllustClosePos = new Vector2(-1150.0f,-50.0f);
  private Vector2 QuestIllustOpenPos = new Vector2(415.0f, -50.0f);
  [Space(10)]
  [SerializeField] private RectTransform QuestDescriptionRect = null;
  [SerializeField] private CanvasGroup QuestDescriptionGroup = null;
  [SerializeField] private TextMeshProUGUI QuestDescriptionText_size = null;
  [SerializeField] private TextMeshProUGUI QuestDescriptionText = null;
  private Vector2 QuestDescriptionClosePos = new Vector2(950.0f, 240.0f);
  private Vector2 QuestDescriptionOpenPos = new Vector2(245.0f, 240.0f);
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
  [SerializeField] private RectMask2D IllustRectMask = null;

  public bool IsActivePanel = false;
  public void OpenQuestSuggestUI()
  {
    IsActivePanel = true;
    UIManager.Instance.AddUIQueue(setquestsuggest_beginning());
  }
  private IEnumerator setquestsuggest_beginning()
  {
    QuestHolder _quest = GameManager.Instance.MyGameData.CurrentQuest;

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
    QuestExps = _quest.StartingExps;
    for (int i = 0; i < QuestExps.Length; i++)
      ExpPreviews[i].MyEXP = QuestExps[i];
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(NextToExpButtonGroup,0.0f,false,false));
    yield return Wait;
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(ExpGroup, 1.0f, QuestUIFadeTime, false));
  }
  public void SelectExp(int index)
  {
    Experience _exp=QuestExps[index];
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
    QuestDescriptionText_size.text = _quest.StartingDescription;
    QuestDescriptionText.text = _quest.StartingDescription;
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(QuestDescriptionText, 1.0f, QuestUIFadeTime));

    yield return Wait;

    MapButtonText.text = _quest.StartingDescription;
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
