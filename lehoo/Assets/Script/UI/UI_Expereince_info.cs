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
    //���߿� �μ��� �޾ƾ���
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
        }//���ʴ� �ƹ� �����̳� Ŭ���ϸ� ����
        else
        {
            if (CurrentExp == _exp) CloseUI();    //���� ���� Ŭ���ϸ� �ݱ�
            else
            {
                ExpName.text = _name;
                ExpIllust.sprite = _illust;
                ExpDescription.text = _description;
                ExpEffect.text = _effect;
                ExpTurn.text = _exp.Duration.ToString();

            }//�ٸ� ���� Ŭ���ϸ� �ش� ���� ������ ��ü
        }
  }
    public void OpenShortExpUI(int _index)
    {
        Experience _exp = GameManager.Instance.MyGameData.ShortTermEXP[_index];
        //���߿� �μ��� �޾ƾ���
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
    }//���ʴ� �ƹ� �����̳� Ŭ���ϸ� ����
    else
        {
            if (CurrentExp == _exp) CloseUI();    //���� ���� Ŭ���ϸ� �ݱ�
            else
            {
                ExpName.text = _name;
                ExpIllust.sprite = _illust;
                ExpDescription.text = _description;
                ExpEffect.text = _effect;
                ExpTurn.text = _exp.Duration.ToString();

            }//�ٸ� ���� Ŭ���ϸ� �ش� ���� ������ ��ü
        }
    }
    public override void CloseUI()
  {
    TouchBlock.enabled = false;
    UIManager.Instance.AddUIQueue(UIManager.Instance.CloseUI(MyRect, MyDir, UIManager.Instance.LargePanelMoveTime));
    IsOpen = false;
  }
}
