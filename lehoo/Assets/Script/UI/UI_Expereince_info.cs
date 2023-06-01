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
  [SerializeField] private TextMeshProUGUI ExpEffect = null;
  [SerializeField] private TextMeshProUGUI ExpTurn = null;
  [SerializeField] private CanvasGroup BackButton = null; 
  private Vector2 ClosePos =new Vector2(1500.0f,0.0f);
  private Vector2 OpenPos =new Vector2(309.0f,0.0f);
  public void OpenLongExpUI(int _index)
  {
    BackButton.interactable = true;
    BackButton.blocksRaycasts = true;
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
            ExpEffect.text = _description+"\n\n"+_effect;
            ExpTurn.text = _exp.Duration.ToString();

      UIManager.Instance.AddUIQueue(UIManager.Instance.OpenUI(MyRect, ClosePos,OpenPos, UIManager.Instance.LargePanelMoveTime,true));
    }//���ʴ� �ƹ� �����̳� Ŭ���ϸ� ����
    else
        {
            if (CurrentExp == _exp) CloseUI();    //���� ���� Ŭ���ϸ� �ݱ�
            else
            {
                ExpName.text = _name;
                ExpIllust.sprite = _illust;
                ExpEffect.text = _description + "\n\n" + _effect;
                ExpTurn.text = _exp.Duration.ToString();

            }//�ٸ� ���� Ŭ���ϸ� �ش� ���� ������ ��ü
        }
  }
    public void OpenShortExpUI(int _index)
    {
    BackButton.interactable = true;
    BackButton.blocksRaycasts = true;
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
      ExpEffect.text = _description + "\n\n" + _effect;
      ExpTurn.text = _exp.Duration.ToString();

      UIManager.Instance.AddUIQueue(UIManager.Instance.OpenUI(MyRect, ClosePos,OpenPos, UIManager.Instance.LargePanelMoveTime,true));
    }//���ʴ� �ƹ� �����̳� Ŭ���ϸ� ����
    else
        {
            if (CurrentExp == _exp) CloseUI();    //���� ���� Ŭ���ϸ� �ݱ�
            else
            {
                ExpName.text = _name;
                ExpIllust.sprite = _illust;
        ExpEffect.text = _description + "\n\n" + _effect;
        ExpTurn.text = _exp.Duration.ToString();

            }//�ٸ� ���� Ŭ���ϸ� �ش� ���� ������ ��ü
        }
    }
    public override void CloseUI()
  {
    BackButton.interactable = false;
    BackButton.blocksRaycasts = false;

    TouchBlock.enabled = false;
    StartCoroutine( UIManager.Instance.CloseUI(MyRect, OpenPos,ClosePos, UIManager.Instance.LargePanelMoveTime,true));
    IsOpen = false;
  }
}
