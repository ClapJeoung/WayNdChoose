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
    float _targettime = 1.5f;
    while (DescriptionScrollbar.value > 0.001f || _time < 1.5f)
    {
      DescriptionScrollbar.value = Mathf.Lerp(DescriptionScrollbar.value, 0.0f, 0.013f);
      _time += Time.deltaTime;
      yield return null;
    }
    DescriptionScrollbar.value = 0.0f;
  }
  [SerializeField] private TextMeshProUGUI ButtonText = null;
  [SerializeField] private CanvasGroup ButtonGroup = null;
  private float DisableAlpha = 0.2f;
  private void SetNextButtonDisable()
  {
    ButtonGroup.alpha = DisableAlpha;
    ButtonGroup.interactable = false;
  }
  public void SetNextButtonActive()
  {
    ButtonGroup.alpha = 1.0f;
    ButtonGroup.interactable = true;
  }

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

    Illust.Setup(illust, 0.5f);
    Description.text = description;
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

    Illust.Setup(Illusts[CurrentIndex], 0.5f);
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
      UIManager.Instance.ResetGame(QuitLogo,true);
    }
    else
    UIManager.Instance.StartCoroutine(next());
  }
  private float NextTime = 0.6f;
  private IEnumerator next()
  {
    CurrentIndex++;

    SetNextButtonDisable();
    Illust.Next(Illusts[CurrentIndex], NextTime);
    yield return new WaitForSeconds(NextTime);
    
    Description.text +="<br><br>"+ Descriptions[CurrentIndex];
    LayoutRebuilder.ForceRebuildLayoutImmediate(Description.transform.parent.transform as RectTransform);
    yield return StartCoroutine(updatescrollbar());

    ButtonText.text = CurrentIndex<Length-1? GameManager.Instance.GetTextData("NEXT_TEXT"):LastButtonText;
    LayoutRebuilder.ForceRebuildLayoutImmediate(ButtonGroup.transform as RectTransform);
  }
}
