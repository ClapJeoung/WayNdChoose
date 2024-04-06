using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public enum SFXEnum { 
  ButtonEnter_Preview,
IllustSwap,
  Click,
  MapOpen,
  MapClose,
  TileSelect,
  Moving,
  SettlementEnter_Village,
SettlementEnter_Town,
SettlementEnter_City,
SectorClick_Residence,
  SectorClick_Temple,
  SectorClick_Market,
  SectorClick_Library,
  RestingSFX,
HPLoss,
  SanityGen,
  SanityLoss,
GoldGen,
SkillGain,
  ExpGain,
  ExpLoss,
TendencyChange,
PaySFX,
  CheckSFX,
SuccessSFX,
FailSFX,
MadnessSFX,
CultSFX,
Hungry,
Movement,
PaysanimationSFX,
CheckAnimationSFX,
ResourceGain
}
public class AudioManager : MonoBehaviour
{
  private void Start()
  {
    AudioMixer.SetFloat("BGM", PlayerPrefs.GetFloat("BGMVolume",0) < -40 ? -80 : PlayerPrefs.GetFloat("BGMVolume", 0));
    AudioMixer.SetFloat("SFX", PlayerPrefs.GetFloat("SFXVolume", 0) < -40 ? -80 : PlayerPrefs.GetFloat("SFXVolume", 0));
  }
  public AudioMixer AudioMixer = null;
  private List<AudioChanel> sfxaudios = new List<AudioChanel>();
  private List<AudioChanel> SFXAudios
  {
    get
    {
      if (sfxaudios.Count == 0)
      {
        var _sources = SFXAudio.GetComponents<AudioSource>().ToList();
        for (int i = 0; i < _sources.Count; i++)
          sfxaudios.Add(new AudioChanel(_sources[i], ""));
      }
      return sfxaudios;
    }
  }
  [SerializeField] private GameObject SFXAudio = null;
  public AudioSource BGMAudio = null;

  [SerializeField] private List<AudioClip> BackgroundMusics = new List<AudioClip>();
  [SerializeField] private List<AudioClip> SFXs = new List<AudioClip>();
  [SerializeField] private List<AudioClip> WalkingSFXs= new List<AudioClip>();
  [SerializeField] private List<AudioClip> ResourceUseSFXs= new List<AudioClip>();
  private List<AudioClip> PlayedList = new List<AudioClip>();
  [SerializeField] private UI_Menu Menu = null;
  public void PlayBGM()
  {
    StartCoroutine(bgm());
  }
  private IEnumerator bgm()
  {
    AudioClip _nextclip = BackgroundMusics[Random.Range(0, BackgroundMusics.Count)];
    BGMAudio.clip = _nextclip;
    PlayedList.Add(_nextclip);
    BGMAudio.Play();
    yield return StartCoroutine(setvolume(BGMAudio, 0.0f, 1.0f));

    while (true)
    {
      Menu.SetBGMName();
      yield return new WaitUntil(() => { return !BGMAudio.isPlaying||Input.GetKeyDown(KeyCode.N); });

      _nextclip = BackgroundMusics[Random.Range(0, BackgroundMusics.Count)];
      while (PlayedList.Contains(_nextclip))
      {
        _nextclip = BackgroundMusics[Random.Range(0, BackgroundMusics.Count)];
        yield return null;
      }
      PlayedList.Add(_nextclip);
      BGMAudio.clip = _nextclip;
      BGMAudio.Play();
      if(PlayedList.Count== BackgroundMusics.Count)
      {
        PlayedList.Clear();
        PlayedList.Add(_nextclip);
      }
      yield return null;
    }
  }
  public void SetoffBGM() => StartCoroutine(setvolume(BGMAudio, 1.0f, 0.0f));
  private IEnumerator setvolume(AudioSource source,float startvolume, float targetvolume)
  {
    source.volume = startvolume;
    float _time = 0.0f, _targettime = 2.0f;

    while (_time < _targettime)
    {
      BGMAudio.volume = Mathf.Lerp(startvolume, targetvolume, _time / _targettime);
      _time += Time.deltaTime;
      yield return null;
    }
    source.volume = targetvolume;
  }
  private List<string> BlockedChanels=new List<string>();
  public void BlockChanel(string id)
  {
    if(BlockedChanels.Contains(id)) return;
    BlockedChanels.Add(id);
  }
  public void ReleaseChanel(string id)
  {
    if (!BlockedChanels.Contains(id)) return;
    BlockedChanels.Remove(id);
  }
  public void PlaySFX(int index)
  {
    foreach(var audio in SFXAudios)
    {
      if (audio.Audio.isPlaying) continue;
      audio.Audio.clip = SFXs[index];
      audio.Audio.Play();
      audio.Channel = "";
      break;
    }
  }
  public void PlaySFX(int index,string chanel)
  {
    if(BlockedChanels.Contains(chanel)) return;
    foreach (var audio in SFXAudios)
    {
      if (audio.Channel == chanel&& audio.Audio.isPlaying)
      {
        audio.Audio.Stop();
        audio.Audio.clip = SFXs[index];
        audio.Audio.Play();
        return;
      }
    }
    foreach (var audio in SFXAudios)
    {
      if (audio.Audio.isPlaying) continue;
      audio.Audio.clip = SFXs[index];
      audio.Audio.Play();
      audio.Channel = chanel;
      break;
    }
  }
  public void StopSFX(int index)
  {
    AudioClip _clip = SFXs[index];
    foreach (var audio in SFXAudios)
    {
      if (audio.Audio.isPlaying && audio.Audio.clip == _clip)
      {
        audio.Audio.Stop();
        return;
      }
    }
  }
  public void PlaySFX(AudioClip clip)
  {
    foreach (var audio in SFXAudios)
    {
      if (audio.Audio.isPlaying) continue;
      audio.Audio.clip = clip;
      audio.Audio.Play();
      audio.Channel = "";
      break;
    }
  }
  /// <summary>
  /// 채널은 1번부터 쓰기
  /// </summary>
  /// <param name="index"></param>
  /// <param name="chanel"></param>
  public void PlaySFX(AudioClip clip, string chanel)
  {
    if (BlockedChanels.Contains(chanel)) return;
    foreach (var audio in SFXAudios)
    {
      if (audio.Channel == chanel && audio.Audio.isPlaying)
      {
        audio.Audio.Stop();
        audio.Audio.clip = clip;
        audio.Audio.Play();
        return;
      }
    }
    foreach (var audio in SFXAudios)
    {
      if (audio.Audio.isPlaying) continue;
      audio.Audio.clip = clip;
      audio.Audio.Play();
      audio.Channel = chanel;
      break;
    }
  }
  public void StopSFX(AudioClip clip)
  {
    foreach (var audio in SFXAudios)
    {
      if (audio.Audio.isPlaying && audio.Audio.clip == clip)
      {
        audio.Audio.Stop();
        return;
      }
    }
  }
  public void StopSFX(string chanel)
  {
    foreach (var audio in SFXAudios)
    {
      if (audio.Audio.isPlaying && audio.Channel == chanel)
      {
        audio.Audio.Stop();
        return;
      }
    }
  }
  private AudioChanel CurrentWalkingChanel;
  private bool Walking = true;
  private IEnumerator WalkingCoroiutine = null;
  public void PlayWalking()
  {
    if (WalkingCoroiutine == null)
    {
      WalkingCoroiutine = playwalking();
      StartCoroutine(WalkingCoroiutine);
    }
  }
  private IEnumerator playwalking()
  {
    yield return new WaitForSeconds(0.5f);
    Walking = true;
    foreach (var audio in SFXAudios)
    {
      if (audio.Audio.isPlaying) continue;

      CurrentWalkingChanel=audio;
      break;
    }
    if (CurrentWalkingChanel == null) yield break;
    AudioClip _clip= WalkingSFXs[Random.Range(0, WalkingSFXs.Count)];
    CurrentWalkingChanel.Channel = "walking";
    CurrentWalkingChanel.Audio.clip= _clip;
    CurrentWalkingChanel.Audio.Play();
    while (true)
    {
      if (!Walking) break;
      yield return new WaitUntil(() => { return !CurrentWalkingChanel.Audio.isPlaying; });
      _clip = WalkingSFXs[Random.Range(0, WalkingSFXs.Count)];
      CurrentWalkingChanel.Audio.clip = _clip;
      CurrentWalkingChanel.Audio.Play();
      yield return new WaitForSeconds(0.25f);
      yield return null;
    }
  }
  public void StopWalking()
  {
    if (CurrentWalkingChanel != null)
    {
      StopCoroutine(WalkingCoroiutine);
      Walking = false;
      CurrentWalkingChanel.Audio.Stop();
      CurrentWalkingChanel.Channel = "";
      CurrentWalkingChanel = null;
    }
  }
  int LastResourceIndex = -1;
  public void PlayResourceUseSFX()
  {
    int _index = Random.Range(0, ResourceUseSFXs.Count);
    while (_index == LastResourceIndex) _index = Random.Range(0, ResourceUseSFXs.Count);
    PlaySFX(ResourceUseSFXs[_index]);
    LastResourceIndex = _index;
  }
}
public class AudioChanel
{
  public AudioSource Audio = null;
  public string Channel = "";
  public AudioChanel(AudioSource audio,string chanel)
  {
    Audio = audio;
    Channel = chanel;
  }
}
