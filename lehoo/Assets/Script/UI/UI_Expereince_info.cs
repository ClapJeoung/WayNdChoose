using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_Expereince_info : UI_default
{

  private Experience CurrentExp = null;
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

    IsOpen = true;

    string _name = _exp.Name;
    Sprite _illust = _exp.Illust;
    string _description = _exp.Description;
    string _effect = _exp.EffectString;

    ExpName.text = _name;
    ExpIllust.sprite = _illust;
    ExpDescription.text = _description;
    ExpEffect.text = _effect;
    ExpTurn.text = _exp.Duration.ToString();

    if(CurrentExp==null) UIManager.Instance.OpenUI(MyRect, MyGroup, MyDir, false);
    else
    {
      if (CurrentExp == _exp) CloseUI();
      else
      {

      }
    }
  }
  public void OpenShortExpUI(int _index)
  {
    Experience _exp = GameManager.Instance.MyGameData.ShortTermEXP[_index];
    //���߿� �μ��� �޾ƾ���
    if (UIManager.Instance.IsWorking || _exp == null) return;
    if (IsOpen && CurrentExp.Equals(_exp)) { CloseUI(); IsOpen = false; return; }

    IsOpen = true;

    string _name = _exp.Name;
    Sprite _illust = _exp.Illust;
    string _description = _exp.Description;
    string _effect = _exp.EffectString;

    ExpName.text = _name;
    ExpIllust.sprite = _illust;
    ExpDescription.text = _description;
    ExpEffect.text = _effect;
    ExpTurn.text = _exp.Duration.ToString();

    if (CurrentExp==null) UIManager.Instance.OpenUI(MyRect, MyGroup, MyDir, false);//���� â ó�� ����
    else
    {
      if (CurrentExp == _exp) CloseUI();
      else
      {

      }
    }//�̹� ���� ���¿��� �ٸ� ���� Ŭ��������
  }
public override void CloseUI()
  {
    base.CloseUI();
  }
}
