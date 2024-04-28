using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Menu : UI_default
{
  [SerializeField] private TextMeshProUGUI ReturnText = null;
  [SerializeField] private TextMeshProUGUI BGMText = null;
  [SerializeField] private Slider BGMSlider = null;
  [SerializeField] private Slider SFXSlider = null;
  [SerializeField] private TextMeshProUGUI QuitText = null;
  [SerializeField] private TextMeshProUGUI DataSaveInfo = null;
  private bool IsWorking = false;
  private Dictionary<string, string> BGMNames = new Dictionary<string, string>()
  {
    {"Ale and Anecdotes", "Darren Curtis" },
    {"Minstrels Song", "Keys of Moon" },
    {"One Bard Band", "Alexander Nakarada" },
    {"The Virgin", "JuliushH" },
    {"Kings Feast", "RandomMind" },
    {"The Bards Tale", "RandomMind" },
    {"Exploration", "RandomMind" },
    {"The Old Tower Inn", "RandomMind" }
  };
  public void SetBGMName()
  {
    string _name = UIManager.Instance.AudioManager.BGMAudio.clip.name;
    BGMText.text = $"BGM<br>{_name}-{BGMNames[_name]}";
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      if (!GameManager.Instance.IsPlaying) return;

      Click();
    }
  }
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
    UIManager.Instance.AudioManager.AudioMixer.SetFloat("BGM",BGMSlider.value<-40?-80:BGMSlider.value);
    PlayerPrefs.SetFloat("BGMVolume", BGMSlider.value);
  }
  public void SetSFMMixer()
  {
    UIManager.Instance.AudioManager.AudioMixer.SetFloat("SFX", SFXSlider.value < -40 ? -80 : SFXSlider.value);
    PlayerPrefs.SetFloat("SFXVolume", SFXSlider.value);
  }
  private IEnumerator openui()
  {
    SetBGMName();
    IsWorking = true;
    ReturnText.text = GameManager.Instance.GetTextData("Return");
    LayoutRebuilder.ForceRebuildLayoutImmediate(ReturnText.transform.parent.transform as RectTransform);

    float _bgmvalue = 0;
    UIManager.Instance.AudioManager.AudioMixer.GetFloat("BGM", out _bgmvalue);
    if (_bgmvalue < -40) _bgmvalue = -40;
    BGMSlider.value = _bgmvalue;

    float _sfxvalue = 0;
    UIManager.Instance.AudioManager.AudioMixer.GetFloat("SFX", out _sfxvalue);
    if (_sfxvalue < -40) _sfxvalue = -40;
    SFXSlider.value = _sfxvalue;

    QuitText.text = GameManager.Instance.GetTextData("QUITGAME");
    LayoutRebuilder.ForceRebuildLayoutImmediate(QuitText.transform.parent.transform as RectTransform);
    DataSaveInfo.text = GameManager.Instance.GetTextData("AutosaveInfo");

    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DefaultGroup, 1.0f, 0.2f));
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
  public void QuitGame()
  {
    if (IsWorking) return;

    StartCoroutine(UIManager.Instance.CloseGameAsDead());
  }
}
