using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SettlementIcon : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
  private float UpSize = 1.2f;
  private float OriginSIze = 1.0f;
  public Settlement SettlementData = null;
  List<RectTransform> MyImages=new List<RectTransform>();
  Image QuestIcon = null;
  private Button MyButton = null;
  private bool selected;
  public bool Selected
  {
    get { return selected; }
    set { selected = value;if (selected == true) SizeUp(); else SizeDown(); }
  }
    private UI_map MapUI = null;
  public void ActiveButton()
  {
    Color _availablecolor = Color.white;
    _availablecolor.a = 1.0f;
    foreach(var _image in MyImages)_image.GetComponent<Image>().color = _availablecolor;
    MyButton.interactable = true;
    SizeDown();
  }//활성화 시키기
  public void DeActiveButton()
  {
    Color _disablecolor = Color.grey;
    _disablecolor.a = 0.2f;
    foreach (var _image in MyImages) _image.GetComponent<Image>().color = _disablecolor;
    MyButton.interactable = false;
    SizeDown();
  }//비활성화 시키기
  public void Setup(Settlement _data,Image _questicon)
  {
    GetComponent<Image>().enabled = false;
    QuestIcon=_questicon;
    transform.localScale = Vector3.one;
    Color _disablecolor = Color.grey;
    _disablecolor.a = 0.2f;
    MyButton=GetComponent<Button>();
      SettlementData = _data;
    UIManager.Instance.UpdateMap_AddSettle(SettlementData.OriginName, this);
    MyButton.onClick.AddListener(SendData);
    for(int i = 0; i < transform.childCount; i++)
    {
      MyImages.Add(transform.GetChild(i).GetComponent<RectTransform>());
    }
  }
  /// <summary>
  /// 0: 없음(기) 1:승 2:전 3:결
  /// </summary>
  /// <param name="_index"></param>
  public void SetQuestIcon(int _index)
  {
    if (_index.Equals(0))
    {
      if(QuestIcon.enabled.Equals(true))QuestIcon.enabled = false;return;
    }
    Sprite _sprite = null;
    switch (_index)
    {
      case 1:_sprite = GameManager.Instance.ImageHolder.Quest_risig;break;
      case 2:_sprite = GameManager.Instance.ImageHolder.Quest_climax;break;
      case 3:_sprite = GameManager.Instance.ImageHolder.Quest_fall;break;
    }
    if(QuestIcon.enabled.Equals(false))QuestIcon.enabled = true;
    QuestIcon.sprite = _sprite;
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
  }
  public void OnPointerExit(PointerEventData _data)
  {
    if (Selected || UIManager.Instance.IsWorking) return;
  }
  public void SendData()
  {
    foreach (RectTransform _rect in MyImages)
      _rect.localScale = Vector3.one * UpSize;
    UIManager.Instance.UpdateMap_SettlePanel(SettlementData);
  }//눌렀을때 메소드
}
