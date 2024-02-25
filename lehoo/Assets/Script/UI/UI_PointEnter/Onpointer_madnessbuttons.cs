using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Onpointer_madnessbuttons : MonoBehaviour,IPointerEnterHandler
{
  public UI_Mad MadnessUI = null;
  public int Index = 0;
    public void OnPointerEnter(PointerEventData data)
  {
    if (!MadnessUI.IsOpen) return;
    switch (Index)
    {
      case 0:
        if (GameManager.Instance.MyGameData.Madness_Conversation == true) return;
        break;
      case 1:
        if (GameManager.Instance.MyGameData.Madness_Force == true) return;
        break;
      case 2:
        if (GameManager.Instance.MyGameData.Madness_Wild == true) return;
        break;
      case 3:
        if (GameManager.Instance.MyGameData.Madness_Intelligence == true) return;
        break;
    }

    MadnessUI.OnEnterMadness(Index);
  }
}
