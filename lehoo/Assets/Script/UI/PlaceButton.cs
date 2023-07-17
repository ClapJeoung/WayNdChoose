using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using TMPro;

public class PlaceButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CanvasGroup MyGroup = null;
    public TextMeshProUGUI MyText = null;
  [SerializeField] private CanvasGroup NameCanvasGroup = null;
    [SerializeField] private UI_EventSuggest MyEventSuggest = null;
    [SerializeField] private Button MyButton = null;
  [SerializeField] private RectTransform NameRect = null;
  [SerializeField] private TextMeshProUGUI MyPlaceName = null;
  public PlaceType MyPlaceType = PlaceType.Residence;
  public bool Clicked = false;
  private void Start()
  {
    MyPlaceName.text = GameManager.Instance.GetTextData(MyPlaceType,0);
  }
  public void OnPointerEnter(PointerEventData eventdata)
  {
    NameCanvasGroup.alpha = 1.0f;
  }
  public void OnPointerExit(PointerEventData eventdata)
  {
    NameCanvasGroup.alpha = 0.0f;
  }
  public void Setup()
    {
    if (GameManager.Instance.MyGameData.VisitedPlaces.Contains(MyPlaceType))
    {
      MyButton.interactable = false;
    }
    else MyButton.interactable = true;

        StartCoroutine(UIManager.Instance.ChangeAlpha(MyGroup, 1.0f, false, false));
}
  public void OnClick()
    {
        MyEventSuggest.SelectPlace(MyPlaceType);
    }
}
