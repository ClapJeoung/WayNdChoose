using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileObjScript : MonoBehaviour
{
  public RectTransform Rect = null;
  public UI_map MapUI = null;
  public TileData TileData = null;
  public Image BottomImage = null;
  public Image TopImage = null;
  public Image LandmarkImage = null;
  public Onpointer_tileoutline OnPointer = null;
  public PreviewInteractive Preview = null;
  public CanvasGroup DiscomfortOutline = null;
  public CanvasGroup FogGroup = null;
  private float FogAlpha_reveal = 0.0f, FogAlpha_visible = 0.4f;
  private float FogAlphaChangeTime = 0.4f;
  private float FogScaleChangeTime = 0.6f;
  public void Clicked()
  {
    if (UIManager.Instance.IsWorking||MapUI.IsDraggingMap) return;

    MapUI.SelectTile(TileData);
  }
  public void SetReveal()
  {
    StopAllCoroutines();
    StartCoroutine(setreveal());
  }
  private IEnumerator setreveal()
  {
    float _time = 0.0f;
    float _startalpha = FogGroup.alpha, _endalpha = FogAlpha_reveal;
    Vector3 _startscale = FogGroup.transform.localScale, _endscale = Vector3.zero;
    while(_time < FogScaleChangeTime)
    {
      if(_time<=FogAlphaChangeTime) FogGroup.alpha = Mathf.Lerp(_startalpha, _endalpha, _time / FogAlphaChangeTime);
      FogGroup.transform.localScale = Vector3.Lerp(_startscale, _endscale,_time / FogScaleChangeTime);

      _time += Time.deltaTime; yield return null;
    }

    FogGroup.alpha = _endalpha;
    FogGroup.transform.localScale = _endscale;
  }
  public void SetVisible()
  {
    StopAllCoroutines();
    StartCoroutine(setvisible());
  }
  private IEnumerator setvisible()
  {
    float _time = 0.0f;
    float _startalpha = FogGroup.alpha, _endalpha = FogAlpha_visible;
    Vector3 _startscale = FogGroup.transform.localScale, _endscale = Vector3.one;
    while (_time < FogScaleChangeTime)
    {
      FogGroup.alpha = Mathf.Lerp(_startalpha, _endalpha, (_time / FogScaleChangeTime));
      FogGroup.transform.localScale = Vector3.Lerp(_startscale, _endscale, _time / FogScaleChangeTime);

      _time += Time.deltaTime; yield return null;
    }

    FogGroup.alpha = _endalpha;
    FogGroup.transform.localScale = _endscale;
  }
}

