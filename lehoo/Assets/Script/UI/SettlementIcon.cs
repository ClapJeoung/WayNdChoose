using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SettlementIcon : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
  private float UpSize = 1.2f;
  private float OriginSIze = 1.0f;
  private Settlement SettlementData = null;
  List<RectTransform> MyImages=new List<RectTransform>();
  private Button MyButton = null;
  private bool selected;
  public bool Selected
  {
    get { return selected; }
    set { selected = value;if (selected == true) SizeUp(); else SizeDown(); }
  }
    private UI_map MapUI = null;
  private Vector2 position;
  public Vector2 Position { get { Vector2 _pos = Vector2.zero;foreach (var _rect in MyImages) _pos += _rect.anchoredPosition; return _pos / MyImages.Count; } }
  public void ActiveButton()
  {
    SettlementData.IsOpen = true;
    Color _availablecolor = Color.white;
    _availablecolor.a = 1.0f;
    foreach(var _image in MyImages)_image.GetComponent<Image>().color = _availablecolor;
    MyButton.interactable = true;
    SizeDown();
  }
  public void DeActiveButton()
  {
    Color _disablecolor = Color.grey;
    _disablecolor.a = 0.2f;
    foreach (var _image in MyImages) _image.GetComponent<Image>().color = _disablecolor;
    MyButton.interactable = false;
    SizeUp();
  }
  public void Setup(Settlement _data)
  {
    GetComponent<Image>().enabled = false;
    transform.localScale = Vector3.one;
    Color _disablecolor = Color.grey;
    _disablecolor.a = 0.2f;
    MyButton=GetComponent<Button>();
      SettlementData = _data;
    UIManager.Instance.UpdateMap_AddSettle(SettlementData.Name, this);
    MyButton.onClick.AddListener(SendData);
    for(int i = 0; i < transform.childCount; i++)
    {
      MyImages.Add(transform.GetChild(i).GetComponent<RectTransform>());
      if (!SettlementData.IsOpen)
        MyImages[i].GetComponent<Image>().color = _disablecolor;
    }
    if (!SettlementData.IsOpen) MyButton.interactable = false;
  }
  private void SizeUp()
  {
    foreach (RectTransform _rect in MyImages)
      _rect.localScale = Vector3.one * UpSize;
  }
  private void SizeDown()
  {
    foreach (RectTransform _rect in MyImages)
      _rect.localScale = Vector3.one * OriginSIze;
  }
  public void OnPointerEnter(PointerEventData _data)
  {
    if (Selected||UIManager.Instance.IsWorking) return;
    if (SettlementData.IsOpen) SizeUp();
  }
  public void OnPointerExit(PointerEventData _data)
  {
    if (Selected || UIManager.Instance.IsWorking) return;
    if (SettlementData.IsOpen) SizeDown();
  }
  public void SendData()
  {
    foreach (RectTransform _rect in MyImages)
      _rect.localScale = Vector3.one * UpSize;
    UIManager.Instance.UpdateMap_SettlePanel(SettlementData);
  }//눌렀을때 메소드
}
