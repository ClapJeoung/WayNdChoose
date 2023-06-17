using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapArrowButton : MonoBehaviour
{
  public SettlementIcon MySettlementIcon = null;
  public CanvasGroup MyGroup = null;
  public bool Selected = false;
  [SerializeField] private UI_map MyUIMap = null;
  private float ActiveAlpha = 1.0f;
  private float DeActiveAlpha = 0.6f;

  public void Setup(Vector2 anchordposition,Vector2 size,Vector2 rotation,SettlementIcon settleicon)
  {
  //  MyRect.anchoredPosition = anchordposition;
 //   MyRect.sizeDelta = size;
  //  MyRect.rotation=Quaternion.Euler(rotation);

    MySettlementIcon = settleicon;
    MyGroup.alpha = ActiveAlpha;
    MyGroup.blocksRaycasts = true;
    MyGroup.interactable = true;
    Selected = false;
    DeActive();
  }
  public void StartMoving()
  {
    MyGroup.blocksRaycasts = false;
    MyGroup.interactable = false;
    if (Selected.Equals(true))
    {
      MyGroup.alpha = 0.0f;
    }
  }
  public void Clicked()
  {
    if (Selected.Equals(true))
    {
      DeActive();
    }
    else
    {
      Active();
    }
  }
  public void Active()
  {
    if (UIManager.Instance.IsWorking) return;
    Debug.Log($"{gameObject.name} 방향 버튼 클릭해 활성화");
    Selected = true;
    MyUIMap.UpdatePanel(MySettlementIcon.SettlementData,this);

    MySettlementIcon.ActiveButton();
    MyGroup.alpha = ActiveAlpha;
  }
  public void DeActive()
  {
    if (UIManager.Instance.IsWorking) return;
    MyUIMap.UpdatePanel(null,null);
    deactivecolor();
    Debug.Log($"{gameObject.name} 방향 버튼 비활성화");
  }
  public void deactivecolor()
  {
    Selected = false;
    MyGroup.alpha = DeActiveAlpha;
    MySettlementIcon.DeActiveButton();
  }
  public void CloseArrow()
  {
    Selected = false;
    MyGroup.alpha = 0.0f;
    
    MySettlementIcon.DeActiveButton();
  }
}
