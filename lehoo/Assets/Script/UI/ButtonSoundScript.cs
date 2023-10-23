using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSoundScript :MonoBehaviour, IPointerEnterHandler,IPointerClickHandler
{
  public AudioClip EnterSound = null;
  public AudioClip ClickSound = null;
  public void OnPointerEnter(PointerEventData eventData)
  {
    if (EnterSound == null) return;

    GameManager.Instance.AudioManager.PlaySFX(EnterSound, 2);
  }
  public void OnPointerClick(PointerEventData eventData)
  {
    if(ClickSound == null) return;
    GameManager.Instance.AudioManager.PlaySFX(ClickSound, 2);
  }
}
