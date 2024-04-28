using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnPointer_SelectionForTendency : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
  [SerializeField] private CanvasGroup MyGroup = null;
  [SerializeField] private UI_Selection Myselection = null;
  [SerializeField] private UI_Tendency TendencyUI = null;
  public void OnPointerEnter(PointerEventData eventData)
  {
    if (!MyGroup.interactable || Myselection.MyTendencyType == TendencyTypeEnum.None) return;
    int effecttype = 0; //0:없음 1:회전 2:구르기
    bool _dir = Myselection.IsLeft;
    Tendency _checktendency = Myselection.MyTendencyType == TendencyTypeEnum.Body ?
      GameManager.Instance.MyGameData.Tendency_Body : GameManager.Instance.MyGameData.Tendency_Head;
    if (_dir)
    {
      switch (_checktendency.Level)
      {
        case -2:
          effecttype = 1;
          break;
        case -1:
          if (_checktendency.Progress == -(GameManager.Instance.Status.TendencyProgress_1to2 - 1)) effecttype = 2;
          else effecttype = 1;
          break;
        case 1:
          if (_checktendency.Progress == -(GameManager.Instance.Status.TendencyRegress - 1)) effecttype = 2;
          break;
        case 2:
          if (_checktendency.Progress == -(GameManager.Instance.Status.TendencyRegress - 1)) effecttype = 2;
          break;
      }
    }
    else
    {
      switch (_checktendency.Level)
      {
        case -2:
          if (_checktendency.Progress == (GameManager.Instance.Status.TendencyRegress - 1)) effecttype = 2;
          break;
        case -1:
          if (_checktendency.Progress == (GameManager.Instance.Status.TendencyRegress - 1)) effecttype = 2;
          break;
        case 1:
          if (_checktendency.Progress == (GameManager.Instance.Status.TendencyProgress_1to2 - 1)) effecttype = 2;
          else effecttype = 1;
          break;
        case 2:
          effecttype = 1;
          break;
      }
    }

    if (effecttype==1) TendencyUI.StartSpinning(_checktendency.Type);
    else if(effecttype==2) TendencyUI.StartRolling(_checktendency.Type, _dir==true?-1:1);
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    if(Myselection.MyTendencyType == TendencyTypeEnum.None) return;

    TendencyUI.StopWhatever();
  }
}
