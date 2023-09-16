using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_FollowEnding : MonoBehaviour
{
  [SerializeField] private CanvasGroup IllustGroup = null;
  [SerializeField] private RectTransform IllustRect = null;
  [SerializeField] private Image IllustImage = null;
  private Vector2 IllustClosePos = new Vector2(0.0f, 800.0f);
  private Vector2 IllustOpenPos = new Vector2(0.0f,200.0f);
  [Space(10)]
  [SerializeField] private CanvasGroup DescriptionGroup = null;
  [SerializeField] private RectTransform DescriptionRect = null;
  [SerializeField] private TextMeshProUGUI DescriptionText_size = null;
  [SerializeField] private TextMeshProUGUI DescriptionText = null;
  private Vector2 DescriptionClosePos = new Vector2(0.0f, 700.0f);
  private Vector2 DescriptionOpenPos = new Vector2(0.0f, -117.0f);
  [Space(10)]
  [SerializeField] private CanvasGroup ButtonGroup = null;
  FollowEndingData MyEndingData = null;

  private WaitForSeconds Wait = new WaitForSeconds(1.0f);
  private float UIMoveTime = 1.2f;
  public void OpenEnding(FollowEndingData endingdata)
  {
    MyEndingData= endingdata;
    UIManager.Instance.AddUIQueue(openending());
  }
  private IEnumerator openending()
  {
    IllustGroup.alpha = 1.0f;
    IllustImage.sprite = MyEndingData.Illust;

    DescriptionGroup.alpha = 1.0f;
    DescriptionText_size.text = MyEndingData.Description;
    DescriptionText.text = MyEndingData.Description;

    yield return StartCoroutine(UIManager.Instance.moverect(IllustRect, IllustClosePos, IllustOpenPos, UIMoveTime, UIManager.Instance.UIPanelOpenCurve));
    yield return StartCoroutine(UIManager.Instance.moverect(DescriptionRect, DescriptionClosePos, DescriptionOpenPos, UIMoveTime, UIManager.Instance.UIPanelOpenCurve));
    yield return Wait;
    StartCoroutine(UIManager.Instance.ChangeAlpha(ButtonGroup, 1.0f, 0.8f));

    yield return null;
  }
  public void CloseUI()
  {
    if (UIManager.Instance.IsWorking) return;
    UIManager.Instance.AddUIQueue(closeui());
  }
  private IEnumerator closeui()
  {
    StartCoroutine(UIManager.Instance.ChangeAlpha(IllustGroup, 0.0f, 0.4f));
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionGroup,0.0f,0.4f));
    IllustImage.sprite = GameManager.Instance.ImageHolder.Transparent;
    IllustRect.anchoredPosition = IllustClosePos;
    DescriptionRect.anchoredPosition = DescriptionClosePos;
  }
}


