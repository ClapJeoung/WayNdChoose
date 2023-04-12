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
  [SerializeField] private UI_Selection Selection_None = null;
  [SerializeField] private UI_Selection Selection_Rational = null;
  [SerializeField] private UI_Selection Selection_Force = null;
  [SerializeField] private UI_Selection Selection_Mental = null;
  [SerializeField] private UI_Selection Selection_Material = null;

  public void SetEventDialogue(EventDataDefulat _event)
  {
    ResetPanel();
    //√ ±‚»≠
    NameText.text = _event.Name;
    Illust.sprite = _event.Illust;
    DialogueText.text = _event.Description;
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
