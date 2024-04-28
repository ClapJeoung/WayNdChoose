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
  [SerializeField] private CanvasGroup NextButtonGroup = null;
  [SerializeField] private CanvasGroup QuitButtonGroup = null;
  [SerializeField] private TextMeshProUGUI QuitButtonText = null;

  private Sprite[] Illusts=new Sprite[0];
  private List<string> Descriptions=new List<string>();
  private string LastButtonText = "";
  private int CurrentIndex = 0;

  public bool IsDead = false;
  private bool lehu = false;
  private EndingData CurrentEndingData = null;
  public void OpenUI_Dead(Sprite illust,string description)
  {
    GameManager.Instance.DeleteSaveData();
    NextButtonGroup.alpha = 0.0f;
    NextButtonGroup.interactable = false;
    UIManager.Instance.PreviewManager.ClosePreview();
    Illust.Next(illust, 0.5f);
    Description.text = description;
    QuitButtonText.text = GameManager.Instance.GetTextData("QUITTOMAIN");
    LayoutRebuilder.ForceRebuildLayoutImmediate(Description.transform.parent.transform as RectTransform);
    LayoutRebuilder.ForceRebuildLayoutImmediate(QuitButtonGroup.transform as RectTransform);
    QuitButtonGroup.alpha = 1.0f;
    QuitButtonGroup.interactable = true;
    StartCoroutine(UIManager.Instance.ChangeAlpha(DefaultGroup, 1.0f, 2.0f));
  }

  public void OpenUI_Ending(EndingData endingdata)
  {
    GameManager.Instance.DeleteSaveData();
    UIManager.Instance.PreviewManager.ClosePreview();
    CurrentEndingData = endingdata;

    Illusts = endingdata.Illusts; 
    Descriptions = endingdata.Descriptions; 
    LastButtonText= endingdata.LastWord;
    QuitButtonText.text = endingdata.LastWord;
    QuitButtonGroup.alpha = 0.0f;
    QuitButtonGroup.interactable = false;

    Illust.Next(Illusts[CurrentIndex], 0.5f);
    Description.text = Descriptions[CurrentIndex];

    LayoutRebuilder.ForceRebuildLayoutImmediate(Description.transform.parent.transform as RectTransform);
    LayoutRebuilder.ForceRebuildLayoutImmediate(QuitButtonGroup.transform.parent.transform as RectTransform);

    NextButtonGroup.alpha = 1.0f;
    NextButtonGroup.interactable=true;

    StartCoroutine(UIManager.Instance.ChangeAlpha(DefaultGroup, 1.0f, 2.0f));
  }
  public void QuitGame()
  {
    if (lehu) return;
    lehu = false;
    if (IsDead)
      StartCoroutine(UIManager.Instance.CloseGameAsDead());
    else
      StartCoroutine(UIManager.Instance.CurtainUI.OpenEndingDescription(CurrentEndingData));
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

    if (CurrentIndex == Illusts.Length - 1)
    {
      StartCoroutine(UIManager.Instance.ChangeAlpha(NextButtonGroup, 0.0f, 0.5f));
    }
    else
    {
      NextButtonGroup.interactable = false;
    }
    Illust.Next(Illusts[CurrentIndex], NextTime);
    
    Description.text +="<br><br>"+ Descriptions[CurrentIndex];
    LayoutRebuilder.ForceRebuildLayoutImmediate(Description.transform as RectTransform);
    LayoutRebuilder.ForceRebuildLayoutImmediate(Description.transform.parent.transform as RectTransform);
    yield return StartCoroutine(UIManager.Instance.updatescrollbar(DescriptionScrollbar));

    if (CurrentIndex == Illusts.Length - 1)
    {
      StartCoroutine(UIManager.Instance.ChangeAlpha(QuitButtonGroup, 1.0f, 0.5f));
    }
    else
    {
      NextButtonGroup.interactable = true;
    }
  }
}
