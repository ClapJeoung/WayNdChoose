using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
  public AudioMixer AudioMixer = null;
  private List<AudioSource> sfxaudiosources = new List<AudioSource>();
  private List<AudioSource> SFXAudioSources
  {
    get
    {
      if (sfxaudiosources.Count == 0)
      {
        sfxaudiosources = SFXAudio.GetComponents<AudioSource>().ToList();
      }
      return sfxaudiosources;
    }
  }
  [SerializeField] private GameObject SFXAudio = null;
  [SerializeField] private AudioSource BGMAudio = null;
}
