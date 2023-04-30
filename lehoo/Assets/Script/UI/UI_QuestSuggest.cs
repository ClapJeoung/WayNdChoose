using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_QuestSuggest : UI_default
{
  [SerializeField] private TextMeshProUGUI QuestName = null;
  [SerializeField] private Image QuestIllust = null;
  [SerializeField] private TextMeshProUGUI QuestDescription = null;
  [SerializeField] private CanvasGroup MapButtonGroup = null;
  public void OpenQuestSuggestUI()
  {
    QuestHolder _quest = GameManager.Instance.MyGameData.CurrentQuest;
    MyGroup.alpha = 1.0f;
    MyGroup.interactable = true;
    MyGroup.blocksRaycasts = true;
    Color _color = Color.white;
    _color.a = 0.0f;
    QuestName.color = _color;
    QuestIllust.color = _color;
    QuestDescription.color = _color;
    QuestName.text = _quest.QuestName;
    QuestIllust.sprite = _quest.Illust;
    QuestDescription.text = _quest.PreDescription;
    _quest.CurrentSequence = QuestSequence.Rising;
        UIManager.Instance.AddUIQueue(setquestsuggest());
    }
    public void CloseQuestSuggest()
  {
    if (MyGroup.alpha.Equals(1.0f)) return;
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
    Color _color = Color.white;
    _color.a = 0.0f;
    QuestName.color = _color;
    yield return StartCoroutine( UIManager.Instance.ChangeAlpha(QuestName, 1.0f));
    yield return new WaitForSeconds(UIManager.Instance.FadeWaitTime);
     QuestIllust.color = _color;
    yield return StartCoroutine( UIManager.Instance.ChangeAlpha(QuestIllust, 1.0f));
    yield return new WaitForSeconds(UIManager.Instance.FadeWaitTime);
    QuestDescription.color = _color;
     yield return StartCoroutine(UIManager.Instance.ChangeAlpha(QuestDescription, 1.0f));
    yield return new WaitForSeconds(UIManager.Instance.FadeWaitTime);
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(MapButtonGroup, 1.0f, true));
  }

}
