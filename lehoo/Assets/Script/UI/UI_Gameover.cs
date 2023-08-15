using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Gameover : UI_default
{
  [SerializeField] private Image Illust = null;
  [SerializeField] private AnimationCurve IllustOpenCurve = null;
  [SerializeField] private float IllustOpenTime = 4.0f;
  [SerializeField] private TextMeshProUGUI Description = null;
  [SerializeField] private TextMeshProUGUI ButtonText = null;
  [SerializeField] private float CloseTime = 2.5f;
  public void OpenUI()
  {
    Illust.sprite = GameManager.Instance.ImageHolder.GetGameoverIllust();

    UIManager.Instance.StartCoroutine(openui());
  }
  private IEnumerator openui()
  {
    yield return StartCoroutine(UIManager.Instance.moverect(GetPanelRect("illust").Rect, GetPanelRect("illust").OutisdePos, GetPanelRect("illust").InsidePos, IllustOpenTime, IllustOpenCurve));
  }
}
