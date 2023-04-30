using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_trait : UI_default
{
  private Trait CurrentTrait = null;
  [SerializeField] private TextMeshProUGUI TraitName = null;
  [SerializeField] private Image TraitIllust = null;
  [SerializeField] private TextMeshProUGUI TraitDescription = null;
  [SerializeField] private TextMeshProUGUI TraitEffect = null;
  public void OpenUI(Trait _trait)
  {
    if (UIManager.Instance.IsWorking) return;
    if (IsOpen&&CurrentTrait.Equals(_trait)) { CloseUI(); IsOpen = false; return; }
    IsOpen = true;

    Sprite _illust = _trait.Illust;
    string _name = _trait.Name;
    string _description = _trait.Description;
    string _effect = _trait.EffectString;

    TraitName.text = _name;
    TraitIllust.sprite=_illust;
    TraitDescription.text = _description;
    TraitEffect.text = _effect;

    if(CurrentTrait==null)UIManager.Instance.AddUIQueue(UIManager.Instance.OpenUI(MyRect, MyGroup, MyDir, true));
    else
    {
      if(CurrentTrait==_trait)CloseUI();
      else
      {

      }
    }

    CurrentTrait = _trait;
  }
  public override void CloseUI()
  {
    base.CloseUI();
    CurrentTrait = null;
  }
}
