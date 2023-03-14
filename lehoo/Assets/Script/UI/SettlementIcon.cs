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
  private bool Selected = false;
  public void Setup(Settlement _data)
  {
    _data.IsOpen = true;
    GetComponent<Image>().enabled = false;
    transform.localScale = Vector3.one;
    Color _disablecolor = Color.grey;
    _disablecolor.a = 0.2f;
    MyButton=GetComponent<Button>();
      SettlementData = _data;
    MyButton.onClick.AddListener(SendData);
    for(int i = 0; i < transform.childCount; i++)
    {
      MyImages.Add(transform.GetChild(i).GetComponent<RectTransform>());
      if (!SettlementData.IsOpen)
        MyImages[i].GetComponent<Image>().color = _disablecolor;
    }
    if (!SettlementData.IsOpen) MyButton.interactable = false;
  }
  public void OnPointerEnter(PointerEventData _data)
  {
    if (Selected) return;
    if(SettlementData.IsOpen)
    foreach (RectTransform _rect in MyImages)
      _rect.localScale = Vector3.one * UpSize;
  }
  public void OnPointerExit(PointerEventData _data)
  {
    if (Selected) return;
    if (SettlementData.IsOpen)
      foreach (RectTransform _rect in MyImages)
      _rect.localScale = Vector3.one * OriginSIze;
  }
  public void SendData()
  {
    foreach (RectTransform _rect in MyImages)
      _rect.localScale = Vector3.one * UpSize;
    Debug.Log("·¹ÈÄ~");
  }
}
