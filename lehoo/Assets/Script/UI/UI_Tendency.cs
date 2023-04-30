using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Tendency : UI_default
{
  [SerializeField] private Image TendencyIllust = null;
  [SerializeField] private TextMeshProUGUI TendencyName = null;
  [SerializeField] private TextMeshProUGUI TendencyLevel = null;
  [SerializeField] private TextMeshProUGUI TendencyEffect = null;
  [SerializeField] private TextMeshProUGUI TendencyDescription = null;
  [Space(10)]
  [SerializeField] private Image Rational = null;
  [SerializeField] private Image Physical = null;
  [SerializeField] private Image Mental = null;
  [SerializeField] private Image Material = null;
  [Space(10)]
  [SerializeField] private Vector2 OriginIconSize = new Vector2(90.0f,90.0f);
  [SerializeField] private Vector2 SelectIconSize = new Vector2(120.0f,120.0f);
  private float UnselectAlpha = 0.4f;
  private float SelectAlpha = 1.0f;
  private TendencyType CurrentTendency=TendencyType.None;
  public  void OpenUI(int _index)
  {
    Debug.Log("특성 창을 열은 레후~");
    TendencyType _targettendency = (TendencyType)_index;
    if (UIManager.Instance.IsWorking) return;
    if (IsOpen&&CurrentTendency==_targettendency) { CloseUI(); IsOpen = false; return; }
    IsOpen = true;

    TextData _textdata = GameManager.Instance.GetTextData(_targettendency);
    Sprite _illust = GameManager.Instance.ImageHolder.GetTendencyIllust(_targettendency);

    TendencyIllust.sprite = _illust;
    TendencyName.text = _textdata.Name;
    TendencyDescription.text = _textdata.Description;
    TendencyEffect.text=GameManager.Instance.MyGameData.GetTendencyEffectString(_targettendency);
    TendencyLevel.text = GameManager.Instance.MyGameData.GetTendencyLevel(_targettendency).ToString();

    Color _unselectcolor = Color.grey;
    _unselectcolor.a = UnselectAlpha;
    Color _selectcolor = Color.white;
    _selectcolor.a = SelectAlpha;

    Rational.color = _unselectcolor;
    Rational.rectTransform.sizeDelta = OriginIconSize;
    Physical.color = _unselectcolor;
    Physical.rectTransform.sizeDelta = OriginIconSize;
    Mental.color = _unselectcolor;
    Mental.rectTransform.sizeDelta = OriginIconSize;
    Material.color = _unselectcolor;
    Material.rectTransform.sizeDelta = OriginIconSize;
    switch (_targettendency)
    {
      case TendencyType.Rational:
        Rational.color = _selectcolor;
        Rational.rectTransform.sizeDelta = SelectIconSize;
        break;
      case TendencyType.Physical:
        Physical.color = _selectcolor;
        Physical.rectTransform.sizeDelta = SelectIconSize;
        break;
      case TendencyType.Mental:
        Mental.color = _selectcolor;
        Mental.rectTransform.sizeDelta = SelectIconSize;
        break;
      case TendencyType.Material:
        Material.color = _selectcolor;
        Material.rectTransform.sizeDelta = SelectIconSize;
        break;
    }

    if(CurrentTendency==TendencyType.None) UIManager.Instance.AddUIQueue(UIManager.Instance.OpenUI(MyRect, MyGroup, MyDir, true));

    CurrentTendency = _targettendency;
  }
  public override void CloseUI()
  {
    base.CloseUI();
    CurrentTendency = TendencyType.None;
  }
}
