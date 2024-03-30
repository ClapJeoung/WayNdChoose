using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Onpointer_expuse : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
  [SerializeField] private UI_Selection MySelectionData = null;
  public void OnPointerEnter(PointerEventData eventData)
  {
    if (UIManager.Instance.Mouse.MouseState == MouseStateEnum.DragExp)
    {
      UIManager.Instance.ExpDragPreview.UpdateEffectAlpha(MySelectionData.MySelectionData);
    }
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    if (UIManager.Instance.Mouse.MouseState == MouseStateEnum.DragExp)
    {
      UIManager.Instance.ExpDragPreview.ResetEffectAlpha();
    }
  }
}
