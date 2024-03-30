using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSoundScript :MonoBehaviour, IPointerEnterHandler,IPointerClickHandler
{
  [SerializeField] private AudioClip EnterSound = null;
  [SerializeField] private AudioClip ClickSound = null;
  public void OnPointerEnter(PointerEventData eventData)
  {
    if (EnterSound == null) return;

    UIManager.Instance.AudioManager.PlaySFX(EnterSound, "button");
  }
  public void OnPointerClick(PointerEventData eventData)
  {
    if(ClickSound == null) return;
    UIManager.Instance.AudioManager.PlaySFX(ClickSound, "button");
  }
}
