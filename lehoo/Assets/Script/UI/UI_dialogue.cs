using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_dialogue : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI NameText = null;
  [SerializeField] private Image Illust = null;
  [SerializeField] private TextMeshProUGUI DialogueText = null;
  [SerializeField] private CanvasGroup MapButton = null;

  public void SetStartDialoue()
  {
    ResetPanel();
    StartCoroutine(setdialogue());
  }
  private IEnumerator setdialogue()
  {
    UIManager.Instance.ChangeAlpha(NameText, 1.0f);
    yield return new WaitForSeconds(0.2f);
    UIManager.Instance.ChangeAlpha(Illust, 1.0f);
    yield return new WaitForSeconds(0.2f);
    UIManager.Instance.ChangeAlpha(DialogueText, 1.0f);
    yield return new WaitForSeconds(0.2f);
    UIManager.Instance.ChangeAlpha(MapButton, 1.0f);
  }
  public void ResetPanel()
  {
    Color _col = Color.white;
    _col.a = 0.0f;
    NameText.faceColor = _col;
    Illust.color = _col;
    DialogueText.faceColor = _col;
    MapButton.alpha = 0.0f;
    MapButton.interactable = false;
  }
}
