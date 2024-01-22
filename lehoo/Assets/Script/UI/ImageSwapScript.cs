using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwapScript : MonoBehaviour
{
  public Image Image_A = null;
  public CanvasGroup Group_A = null;
  public Image Image_B = null;
  public CanvasGroup Group_B = null;
  public bool Index = true;
  private float ChangeTime = 0.8f;
  private Image CurrentImage { get { return Index == true? Image_A : Image_B; } }
  private Image NextImage { get { return Index==true?Image_B : Image_A; } }
  private CanvasGroup CurrentGroup { get { return Index==true?Group_A : Group_B; } }
  private CanvasGroup NextGroup { get { return Index==true?Group_B : Group_A; } }
  public bool Sound = true;
  public void Next(Sprite illust)
  {
    StopAllCoroutines();
    NextImage.sprite = illust;
    StartCoroutine(changealpha( CurrentGroup, 0.0f, ChangeTime));
    StartCoroutine(changealpha(NextGroup,1.0f,ChangeTime));
    NextGroup.transform.SetSiblingIndex(0);
    Index = !Index;

    UIManager.Instance.AudioManager.PlaySFX(1);
  }
  public void Next(Sprite illust,float time)
  {
    StopAllCoroutines();
    NextImage.sprite = illust;
    StartCoroutine(changealpha(CurrentGroup, 0.0f, time));
    StartCoroutine(changealpha(NextGroup, 1.0f, time));
    NextGroup.transform.SetSiblingIndex(0);
    Index = !Index;

   if(Sound) UIManager.Instance.AudioManager.PlaySFX(1);
  }
  private IEnumerator changealpha(CanvasGroup group,float targetalpha, float targettime)
  {
    float _start = 1.0f - targetalpha, _end = targetalpha;
    group.alpha = _start;
    float _time = 0.0f;
    while (_time < targettime)
    {
      group.alpha = Mathf.Lerp(_start, _end, _time / targettime);
      _time += Time.deltaTime;yield return null;
    }
    group.alpha = _end;
  }
}
