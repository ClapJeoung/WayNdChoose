using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileObjScript : MonoBehaviour
{
  public RectTransform Rect = null;
  public Button Button = null;
  public UI_map MapUI = null;
  public TileData TileData = null;
  public Image BottomImage = null;
  public Image TopImage = null;
  public Image LandmarkImage = null;
  public Onpointer_tileoutline OnPointer = null;
  public PreviewInteractive Preview = null;
  public CanvasGroup DiscomfortOutline = null;
  public CanvasGroup FogGroup = null;
  public void Clicked()
  {
    if (UIManager.Instance.IsWorking) return;

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
    float _startalpha = FogGroup.alpha, _endalpha = ConstValues.FogAlpha_reveal;
    Vector3 _startscale = FogGroup.transform.localScale, _endscale = Vector3.zero;
    while(_time < ConstValues.FogScaleChangeTime)
    {
      if(_time<=ConstValues.FogAlphaChangeTime) FogGroup.alpha = Mathf.Lerp(_startalpha, _endalpha, _time / ConstValues.FogAlphaChangeTime);
      FogGroup.transform.localScale = Vector3.Lerp(_startscale, _endscale,_time / ConstValues.FogScaleChangeTime);

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
    float _startalpha = FogGroup.alpha, _endalpha = ConstValues.FogAlpha_visible;
    Vector3 _startscale = FogGroup.transform.localScale, _endscale = Vector3.one;
    while (_time < ConstValues.FogScaleChangeTime)
    {
      if (_time > ConstValues.FogAlphaChangeTime)
        FogGroup.alpha = Mathf.Lerp(_startalpha, _endalpha, (_time- ConstValues.FogScaleChangeTime) /  ConstValues.FogAlphaChangeTime);
      FogGroup.transform.localScale = Vector3.Lerp(_startscale, _endscale, _time / ConstValues.FogScaleChangeTime);

      _time += Time.deltaTime; yield return null;
    }

    FogGroup.alpha = _endalpha;
    FogGroup.transform.localScale = _endscale;
  }
}

