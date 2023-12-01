using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum MouseStateEnum { Idle,DragMap}
public class MouseScript : MonoBehaviour
{
  public MouseStateEnum MouseState = MouseStateEnum.Idle;
  public GraphicRaycaster RayCaster = null;
  public EventSystem CurrentEventSystem = null;
  private PointerEventData MyPointerEventData = null;
  public Vector2 LastPos= Vector2.zero;
  public Vector2 Offset= Vector2.zero;
  public float MapMoveDegree = 30.0f;
  private void Update()
  {
    if (Input.GetMouseButtonDown(1))
    {
      MyPointerEventData = new PointerEventData(CurrentEventSystem);
      //Set the Pointer Event Position to that of the game object
      MyPointerEventData.position = Input.mousePosition;

      //Create a list of Raycast Results
      List<RaycastResult> results = new List<RaycastResult>();

      //Raycast using the Graphics Raycaster and mouse click position
      RayCaster.Raycast(MyPointerEventData, results);

      if (results.Count > 0)
      {
        foreach (var _obj in results)
          if (_obj.gameObject.CompareTag("TilePanel"))
          {
            MouseState = MouseStateEnum.DragMap;
            LastPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            break;
          }
      }
    }
    if (Input.GetMouseButton(1)&&MouseState==MouseStateEnum.DragMap)
    {
      MyPointerEventData = new PointerEventData(CurrentEventSystem);
      //Set the Pointer Event Position to that of the game object
      MyPointerEventData.position = Input.mousePosition;

      //Create a list of Raycast Results
      List<RaycastResult> results = new List<RaycastResult>();

      //Raycast using the Graphics Raycaster and mouse click position
      RayCaster.Raycast(MyPointerEventData, results);

      if (results.Count == 0)
      {
        MouseState = MouseStateEnum.Idle;
      }
      else
      {
        bool _ismappanel = false;
        foreach (var _obj in results)
        {
          if (_obj.gameObject.CompareTag("TilePanel"))
          {
            _ismappanel = true;
            break;
          }
        }
        if (_ismappanel)
        {
          Vector2 _newpos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
          Offset = _newpos - LastPos;
          LastPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
          UIManager.Instance.MapUI.MoveHolderRect_mouse(Offset * MapMoveDegree);
        }
        else
        {
          MouseState = MouseStateEnum.Idle;
        }
      }
    }
    if (Input.GetMouseButtonUp(1)) MouseState = MouseStateEnum.Idle;
  }
}
