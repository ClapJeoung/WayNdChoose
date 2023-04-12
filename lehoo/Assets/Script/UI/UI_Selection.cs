using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Selection : MonoBehaviour
{
  [SerializeField] private CanvasGroup MyGroup = null;
  [SerializeField] private RectTransform MyRect = null;
  [SerializeField] private Button MyButton = null;
  [SerializeField] private TendencyType MyTendencyType = TendencyType.None;
  private string CurrentDescription = null;
  //���� �� �������� ������ ����

  public void DeActive()
  {
    MyGroup.alpha = 0.0f;
    MyGroup.interactable = false;
  }
  public void Active(string _description)
  {

  }
}
