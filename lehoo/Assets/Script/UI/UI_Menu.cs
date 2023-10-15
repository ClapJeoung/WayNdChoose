using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Menu : UI_default
{
  [SerializeField] private TextMeshProUGUI ReturnText = null;
  [SerializeField] private Slider BGMSlider = null;
  [SerializeField] private Slider SFXSlider = null;
  [SerializeField] private TextMeshProUGUI QuitText = null;
  private bool IsWorking = false;
  public void Click()
  {
    if (IsWorking) return;

    if (IsOpen)
    {
      StartCoroutine(closeui());
    }
    else
    {
      StartCoroutine(openui());
    }
  }
  public void SetBGMMixer()
  {
    GameManager.Instance.AudioManager.AudioMixer.SetFloat("BGM",BGMSlider.value<-40?-80:BGMSlider.value);
  }
  public void SetSFMMixer()
  {
    GameManager.Instance.AudioManager.AudioMixer.SetFloat("SFX", SFXSlider.value < -40 ? -80 : SFXSlider.value);
  }
  private IEnumerator openui()
  {
    IsWorking = true;
    ReturnText.text = GameManager.Instance.GetTextData("Return");
    LayoutRebuilder.ForceRebuildLayoutImmediate(ReturnText.transform.parent.transform as RectTransform);

    float _bgmvalue = 0;
    GameManager.Instance.AudioManager.AudioMixer.GetFloat("BGM", out _bgmvalue);
    if (_bgmvalue < -40) _bgmvalue = -40;
    BGMSlider.value = _bgmvalue;

    float _sfxvalue = 0;
    GameManager.Instance.AudioManager.AudioMixer.GetFloat("SFX", out _sfxvalue);
    if (_sfxvalue < -40) _sfxvalue = -40;
    SFXSlider.value = _sfxvalue;

    QuitText.text = GameManager.Instance.GetTextData("QUITGAME");
    LayoutRebuilder.ForceRebuildLayoutImmediate(QuitText.transform.parent.transform as RectTransform);

    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DefaultGroup, 1.0f, 0.4f));
    IsOpen = true;
    IsWorking = false;
  }
  public void CloseUI()
  {
    if (IsWorking) return;
    StartCoroutine(closeui());
  }
private IEnumerator closeui()
  {
    IsWorking = true;
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DefaultGroup, 0.0f, 0.4f));
    IsWorking = false;
    IsOpen = false;
  }
}
