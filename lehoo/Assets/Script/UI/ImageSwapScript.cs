using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwapScript : MonoBehaviour
{
  [SerializeField] private Image Image_A = null;
  [SerializeField] private CanvasGroup Group_A = null;
  [SerializeField] private Image Image_B = null;
  [SerializeField] private CanvasGroup Group_B = null;
  [SerializeField] private bool Index = true;
  [SerializeField] private float ChangeTime = 0.8f;
  private Image NextImage { get { return Index==true?Image_B : Image_A; } }
  private CanvasGroup CurrentGroup { get { return Index==true?Group_A : Group_B; } }
  private CanvasGroup NextGroup { get { return Index==true?Group_B : Group_A; } }
  [SerializeField] private bool Sound = true;
  public void SetTransparent()
  {
    Image_A.sprite = GameManager.Instance.ImageHolder.Transparent;
    Group_A.alpha = 0.0f;
    Image_B.sprite = GameManager.Instance.ImageHolder.Transparent;
    Group_B.alpha = 0.0f;
  }
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
