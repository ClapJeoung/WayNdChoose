using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlaceButton : MonoBehaviour
{
    [SerializeField] private CanvasGroup MyGroup = null;
    [SerializeField] private TextMeshProUGUI MyText = null;
    [SerializeField] private UI_EventSuggest MyEventSuggest = null;
    [SerializeField] private Button MyButton = null;
    public PlaceType MyPlaceType = PlaceType.Residence;

    public void Setup(PlaceType _targetplace)
    {
        MyPlaceType = _targetplace;
        MyText.text = GameManager.Instance.GetTextData(MyPlaceType).Name;
        if (GameManager.Instance.MyGameData.VisitedPlaces.Contains(MyPlaceType))
        {
            MyButton.interactable = false;
        }
        StartCoroutine(UIManager.Instance.ChangeAlpha(MyGroup, 1.0f, false, UIFadeMoveDir.Left));
    }
    public void Close()
    {
    StartCoroutine(close());
    }
  private IEnumerator close()
  {
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(MyGroup, 0.0f, false));
    gameObject.SetActive(false);
  }
  public void OnClick()
    {
        MyEventSuggest.SelectPlace(MyPlaceType);
    }
}
