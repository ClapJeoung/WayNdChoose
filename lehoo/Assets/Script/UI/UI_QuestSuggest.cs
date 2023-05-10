using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_QuestSuggest : UI_default
{
  [SerializeField] private CanvasGroup QuestNameGroup = null;
  [SerializeField] private TextMeshProUGUI QuestName = null;
  [SerializeField] private CanvasGroup QuestIllustGroup = null;
  [SerializeField] private Image QuestIllust = null;
  [SerializeField] private CanvasGroup QuestDescriptionGroup = null;
  [SerializeField] private TextMeshProUGUI QuestDescription = null;
  [SerializeField] private CanvasGroup MapButtonGroup = null;
  [SerializeField] private RectMask2D IllustRectMask = null;
  public void OpenQuestSuggestUI()
  {
    if (QuestNameGroup.alpha != 0.0f) CloseQuestSuggest();
    //시작 시 모든 요소 투명

    StartCoroutine(UIManager.Instance.OpenUI(MyGroup, true));
        UIManager.Instance.AddUIQueue(setquestsuggest());
    }
    public void CloseQuestSuggest()
  {
    if (MyGroup.alpha.Equals(1.0f)) return;
    QuestName.text = "";

    QuestNameGroup.alpha = 0.0f;
    QuestIllustGroup.alpha = 0.0f;
    QuestDescriptionGroup.alpha = 0.0f;
    MyGroup.alpha = 0.0f;
    MyGroup.interactable = false;
    MyGroup.blocksRaycasts = false;
    MapButtonGroup.alpha = 0.0f;
    MapButtonGroup.interactable = false;
    MapButtonGroup.blocksRaycasts = false;
  }
    public void CloseQuestUI()
    {
        StartCoroutine(UIManager.Instance.CloseUI(MyRect, MyGroup, MyDir));
    }
  private IEnumerator setquestsuggest()
  {
    QuestHolder _quest = GameManager.Instance.MyGameData.CurrentQuest;
    QuestName.text = _quest.QuestName;
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(QuestNameGroup, 1.0f, true));
    yield return new WaitForSeconds(UIManager.Instance.FadeWaitTime);

    QuestIllust.sprite = _quest.Illust;
    IllustRectMask.softness = new Vector2Int(UIManager.Instance.IllustSoftness_start, IllustRectMask.softness.y);
    StartCoroutine(UIManager.Instance.OpenSoftness(IllustRectMask));
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(QuestIllustGroup, 1.0f, true));
    yield return new WaitForSeconds(UIManager.Instance.FadeWaitTime);

    Color _color = Color.white;
    _color.a = 0.0f;
    QuestDescription.color = _color;
    QuestDescription.text = _quest.StartDialogue;
    _quest.CurrentSequence = QuestSequence.Rising;
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(QuestDescriptionGroup, 1.0f, true));
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(QuestDescription, 1.0f));

    yield return new WaitForSeconds(UIManager.Instance.FadeWaitTime);
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(MapButtonGroup, 1.0f, true));
  }

}
