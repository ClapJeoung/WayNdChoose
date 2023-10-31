using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Ending : UI_default
{
  [SerializeField] private ImageSwapScript Illust = null;
  [SerializeField] private TextMeshProUGUI Description = null;
  [SerializeField] private Scrollbar DescriptionScrollbar = null;
  public AnimationCurve ScrollbarCurve = new AnimationCurve();
  private IEnumerator updatescrollbar()
  {
    yield return new WaitForSeconds(0.05f);

    float _time = 0.0f;
    while (DescriptionScrollbar.value > 0.001f && _time < ConstValues.ScrollTime)
    {
      DescriptionScrollbar.value = Mathf.Lerp(DescriptionScrollbar.value, 0.0f, ConstValues.ScrollSpeed);
      _time += Time.deltaTime;
      yield return null;
    }
    DescriptionScrollbar.value = 0.0f;
  }
  [SerializeField] private CanvasGroup NextButtonGroup = null;
  [SerializeField] private CanvasGroup QuitButtonGroup = null;
  [SerializeField] private TextMeshProUGUI QuitButtonText = null;

  private List<Sprite> Illusts=new List<Sprite>();
  private List<string> Descriptions=new List<string>();
  private string LastButtonText = "";
  private int CurrentIndex = 0;
  private int Length { get {  return Illusts.Count; } }

  private bool IsDead = false;
  public void OpenUI_Dead(Sprite illust,string description)
  {
    UIManager.Instance.PreviewManager.ClosePreview();

    Illust.Setup(illust, 0.5f);
    Description.text = description;
    QuitButtonText.text = GameManager.Instance.GetTextData("QUITTOMAIN");
    LayoutRebuilder.ForceRebuildLayoutImmediate(Description.transform.parent.transform as RectTransform);
    LayoutRebuilder.ForceRebuildLayoutImmediate(QuitButtonGroup.transform as RectTransform);
    IsDead = true;
    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(DefaultGroup, 1.0f, 2.0f));
  }

  public void OpenUI_Ending(EndingDatas endingdata)
  {
    UIManager.Instance.PreviewManager.ClosePreview();

    Illusts = endingdata.Illusts; 
    Descriptions = endingdata.Descriptions; 
    LastButtonText= endingdata.LastWord;
    QuitButtonText.text = endingdata.LastWord;

    Illust.Setup(Illusts[CurrentIndex], 0.5f);
    Description.text = Descriptions[CurrentIndex];

    LayoutRebuilder.ForceRebuildLayoutImmediate(Description.transform.parent.transform as RectTransform);
    LayoutRebuilder.ForceRebuildLayoutImmediate(QuitButtonGroup.transform.parent.transform as RectTransform);

    NextButtonGroup.alpha = 1.0f;
    NextButtonGroup.interactable=true;

    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(DefaultGroup, 1.0f, 2.0f));
  }
  public void QuitGame()
  {
    UIManager.Instance.ResetGame(IsDead?"":GameManager.Instance.GetTextData("ThanksForPlaying"), true);
  }
  public void Next()
  {
    if (UIManager.Instance.IsWorking) return;
    UIManager.Instance.StartCoroutine(next());
  }
  private float NextTime = 0.6f;
  private IEnumerator next()
  {
    CurrentIndex++;

    if (CurrentIndex == Illusts.Count - 1)
    {
      StartCoroutine(UIManager.Instance.ChangeAlpha(NextButtonGroup, 0.0f, 0.5f));
    }
    else
    {
      NextButtonGroup.interactable = false;
    }
    Illust.Next(Illusts[CurrentIndex], NextTime);
    yield return new WaitForSeconds(NextTime);
    
    Description.text +="<br><br>"+ Descriptions[CurrentIndex];
    LayoutRebuilder.ForceRebuildLayoutImmediate(Description.transform as RectTransform);
    LayoutRebuilder.ForceRebuildLayoutImmediate(Description.transform.parent.transform as RectTransform);
    yield return StartCoroutine(updatescrollbar());

    if (CurrentIndex == Illusts.Count - 1)
    {
      StartCoroutine(UIManager.Instance.ChangeAlpha(QuitButtonGroup, 1.0f, 0.5f));
    }
    else
    {
      NextButtonGroup.interactable = true;
    }
  }
}
