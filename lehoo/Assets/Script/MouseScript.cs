using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum MouseStateEnum { Idle,DragMap,DragExp}
public class MouseScript : MonoBehaviour
{
  public MouseStateEnum MouseState = MouseStateEnum.Idle;
  public GraphicRaycaster RayCaster = null;
  public EventSystem CurrentEventSystem = null;
  private PointerEventData MyPointerEventData = null;
  public Vector2 LastPos= Vector2.zero;
  public Vector2 Offset= Vector2.zero;
  public float MapMoveDegree = 30.0f;
  public Experience DragExpTarget = null;
  private TileData SelectingTile = null;
  Vector2 ClickPosition= Vector2.zero;
  private void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      if (GameManager.Instance.IsPlaying)
      {
        if(GameManager.Instance.MyGameData.CurrentEventSequence == EventSequence.Progress)
        {
          GameObject _expicon = CheckObjTag("ExpIcon");
          if (_expicon != null && _expicon.GetComponent<Button>().interactable)
          {
            if (_expicon.name[_expicon.name.Length - 1] == '0')
            {
              DragExpTarget = GameManager.Instance.MyGameData.LongExp;
            }
            else if (_expicon.name[_expicon.name.Length - 1] == '1')
            {
              DragExpTarget = GameManager.Instance.MyGameData.ShortExp_A;
            }
            else
            {
              DragExpTarget = GameManager.Instance.MyGameData.ShortExp_B;
            }

            if (DragExpTarget != null && DragExpTarget.Duration > 1)
            {
              UIManager.Instance.ExpDragPreview.gameObject.SetActive(true);
              UIManager.Instance.ExpDragPreview.Setup(DragExpTarget);
              MouseState = MouseStateEnum.DragExp;
            }
          }
        }
        
        if (CheckObjTag("TilePanel") != null)
        {
          MouseState = MouseStateEnum.DragMap;
          LastPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

          if (CheckObjTag("Tile") != null)
          {
            string _coordinate = CheckObjTag("Tile").name;
            TileData _tile = GameManager.Instance.MyGameData.MyMapData.TileDatas[
              int.Parse(_coordinate.Split(',')[0]), int.Parse(_coordinate.Split(',')[1])];
            SelectingTile = !_tile.Interactable || _tile.Fogstate != 2 ? null : _tile;

            ClickPosition = Input.mousePosition;
          }
        }
      }

    }
    if (Input.GetMouseButtonUp(0))
    {
      if (GameManager.Instance.IsPlaying )
      {
        if (MouseState == MouseStateEnum.DragExp)
        {
          GameObject _object = CheckObjTag("Selection");
          if (_object != null)
          {
            _object.GetComponent<UI_Selection>().AddExp(DragExpTarget);
          }

          MouseState = MouseStateEnum.Idle;
          DragExpTarget = null;
          UIManager.Instance.ExpDragPreview.SetDown();
        }
        
        if (SelectingTile!=null&& CheckObjTag("Tile") != null)
        {
          string _coordinate = CheckObjTag("Tile").name;
          TileData _tile = GameManager.Instance.MyGameData.MyMapData.TileDatas[
            int.Parse(_coordinate.Split(',')[0]), int.Parse(_coordinate.Split(',')[1])];
          if (_tile == SelectingTile) SelectingTile.ButtonScript.Clicked();

          SelectingTile = null;
        }
      }
      if (MouseState == MouseStateEnum.DragMap) MouseState = MouseStateEnum.Idle;
    }
    if (Input.GetMouseButtonDown(1))
    {
      if (MouseState == MouseStateEnum.DragExp)
      {
        MouseState = MouseStateEnum.Idle;
        UIManager.Instance.MapUI.IsDraggingMap = false;
      }
      if (GameManager.Instance.IsPlaying && GameManager.Instance.MyGameData.CurrentEventSequence == EventSequence.Progress&& CheckObjTag("ExpIcon"))
      {
        GameObject _expicon = CheckObjTag("ExpIcon");
        if (_expicon.GetComponent<Button>().interactable)
        {
          Experience _selectedexp = null;
          if (_expicon.name[_expicon.name.Length - 1] == '0')
          {
            _selectedexp = GameManager.Instance.MyGameData.LongExp;
          }
          else if (_expicon.name[_expicon.name.Length - 1] == '1')
          {
            _selectedexp = GameManager.Instance.MyGameData.ShortExp_A;
          }
          else
          {
            _selectedexp = GameManager.Instance.MyGameData.ShortExp_B;
          }

          if (_selectedexp != null) UIManager.Instance.DialogueUI.SubExp(_selectedexp);
        }
      }
    }
    if (Input.GetMouseButton(0)&&MouseState==MouseStateEnum.DragMap)
    {
      MyPointerEventData = new PointerEventData(CurrentEventSystem);
      //Set the Pointer Event Position to that of the game object
      MyPointerEventData.position = Input.mousePosition;

     // Debug.Log($"°Å¸® : {(ClickPosition - (Vector2)Input.mousePosition).magnitude}");
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
  }
  private GameObject CheckObjTag(string tag)
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
        if (_obj.gameObject.CompareTag(tag))
        {
          return _obj.gameObject;
        }
      return null;
    }
    else return null;
  }
}
