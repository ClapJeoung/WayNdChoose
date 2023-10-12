using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Ending : UI_default
{
  [SerializeField] private Image Illust = null;
  [SerializeField] private CanvasGroup IllustGroup = null;
  [SerializeField] private TextMeshProUGUI Description = null;
  [SerializeField] private CanvasGroup DescriptionGroup = null;
  [SerializeField] private TextMeshProUGUI ButtonText = null;

  private List<Sprite> Illusts=new List<Sprite>();
  private List<string> Descriptions=new List<string>();
  private string LastButtonText = "";
  private int CurrentIndex = 0;
  private string QuitLogo = "";
  private int Length { get {  return Illusts.Count; } }

  private bool IsDead = false;
  public void OpenUI_Dead(Sprite illust,string description)
  {
    UIManager.Instance.PreviewManager.ClosePreview();

    Illust.sprite= illust;
    Description.text= description;
    ButtonText.text = GameManager.Instance.GetTextData("QUITTOMAIN");
    LayoutRebuilder.ForceRebuildLayoutImmediate(Description.transform.parent.transform as RectTransform);
    LayoutRebuilder.ForceRebuildLayoutImmediate(ButtonText.transform.parent.transform as RectTransform);
    IsDead = true;
    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(DefaultGroup, 1.0f, 2.0f));
  }

public void OpenUI_Ending(Tuple<List<Sprite>, List<string>, string, string> data)
  {
    UIManager.Instance.PreviewManager.ClosePreview();

    Illusts = data.Item1; Descriptions = data.Item2; LastButtonText=data.Item3; QuitLogo = data.Item4;

    Illust.sprite = Illusts[CurrentIndex];
    Description.text = Descriptions[CurrentIndex];
    ButtonText.text = GameManager.Instance.GetTextData("NEXT_TEXT");

    LayoutRebuilder.ForceRebuildLayoutImmediate(Description.transform.parent.transform as RectTransform);
    LayoutRebuilder.ForceRebuildLayoutImmediate(ButtonText.transform.parent.transform as RectTransform);

    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(DefaultGroup, 1.0f, 2.0f));
  }
  public void Next()
  {
    if (UIManager.Instance.IsWorking) return;

    if (CurrentIndex == Length - 1||IsDead)
    {
      UIManager.Instance.ResetGame(QuitLogo);
    }
    else
    UIManager.Instance.StartCoroutine(next());
  }
  private IEnumerator next()
  {
    CurrentIndex++;

    StartCoroutine(UIManager.Instance.ChangeAlpha(IllustGroup, 0.0f, 0.6f));
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionGroup, 0.0f, 0.6f));
    Illust.sprite = Illusts[CurrentIndex];
    Description.text = Descriptions[CurrentIndex];
    ButtonText.text = CurrentIndex<Length-1? GameManager.Instance.GetTextData("NEXT_TEXT"):LastButtonText;
    LayoutRebuilder.ForceRebuildLayoutImmediate(ButtonText.transform.parent.transform as RectTransform);
    StartCoroutine(UIManager.Instance.ChangeAlpha(IllustGroup, 1.0f, 0.6f));
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionGroup, 1.0f, 0.6f));
  }
}
