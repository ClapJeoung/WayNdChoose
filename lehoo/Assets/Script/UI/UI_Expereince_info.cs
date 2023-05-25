using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_Expereince_info : UI_default
{

  private Experience CurrentExp = null;
  [SerializeField] private Image TouchBlock = null;
  [SerializeField] private TextMeshProUGUI ExpName = null;
  [SerializeField] private Image ExpIllust = null;
  [SerializeField] private TextMeshProUGUI ExpDescription = null;
  [SerializeField] private TextMeshProUGUI ExpEffect = null;
  [SerializeField] private TextMeshProUGUI ExpTurn = null;
  public void OpenLongExpUI(int _index)
  {
    Experience _exp = GameManager.Instance.MyGameData.LongTermEXP[_index];
    //나중에 인수도 받아야함
    if (UIManager.Instance.IsWorking||_exp==null) return;
    if (IsOpen&&CurrentExp.Equals(_exp)) { CloseUI(); IsOpen = false; return; }

    TouchBlock.enabled = true;
    IsOpen = true;

        string _name = _exp.Name;
        Sprite _illust = _exp.Illust;
        string _description = _exp.Description;
        string _effect = _exp.EffectString;
        if (CurrentExp == null)
        {

            ExpName.text = _name;
            ExpIllust.sprite = _illust;
            ExpDescription.text = _description;
            ExpEffect.text = _effect;
            ExpTurn.text = _exp.Duration.ToString();

            UIManager.Instance.AddUIQueue(UIManager.Instance.OpenUI(MyRect, MyGroup, MyDir, true));
        }//최초는 아무 경험이나 클릭하면 열기
        else
        {
            if (CurrentExp == _exp) CloseUI();    //같은 경험 클릭하면 닫기
            else
            {
                ExpName.text = _name;
                ExpIllust.sprite = _illust;
                ExpDescription.text = _description;
                ExpEffect.text = _effect;
                ExpTurn.text = _exp.Duration.ToString();

            }//다른 경험 클릭하면 해당 경험 정보로 대체
        }
  }
    public void OpenShortExpUI(int _index)
    {
        Experience _exp = GameManager.Instance.MyGameData.ShortTermEXP[_index];
        //나중에 인수도 받아야함
        if (UIManager.Instance.IsWorking || _exp == null) return;
        if (IsOpen && CurrentExp.Equals(_exp)) { CloseUI(); IsOpen = false; return; }

    TouchBlock.enabled = true;
    IsOpen = true;

        string _name = _exp.Name;
        Sprite _illust = _exp.Illust;
        string _description = _exp.Description;
        string _effect = _exp.EffectString;
        if (CurrentExp == null)
        {
            ExpName.text = _name;
            ExpIllust.sprite = _illust;
            ExpDescription.text = _description;
            ExpEffect.text = _effect;
            ExpTurn.text = _exp.Duration.ToString();

      UIManager.Instance.AddUIQueue(UIManager.Instance.OpenUI(MyRect, MyDir, UIManager.Instance.LargePanelMoveTime));
    }//최초는 아무 경험이나 클릭하면 열기
    else
        {
            if (CurrentExp == _exp) CloseUI();    //같은 경험 클릭하면 닫기
            else
            {
                ExpName.text = _name;
                ExpIllust.sprite = _illust;
                ExpDescription.text = _description;
                ExpEffect.text = _effect;
                ExpTurn.text = _exp.Duration.ToString();

            }//다른 경험 클릭하면 해당 경험 정보로 대체
        }
    }
    public override void CloseUI()
  {
    TouchBlock.enabled = false;
    UIManager.Instance.AddUIQueue(UIManager.Instance.CloseUI(MyRect, MyDir, UIManager.Instance.LargePanelMoveTime));
    IsOpen = false;
  }
}
