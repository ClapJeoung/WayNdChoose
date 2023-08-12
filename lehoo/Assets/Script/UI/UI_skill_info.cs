using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_skill_info : UI_default//��ũ��Ʈ �̸��� Skill�ε� Theme ǥ���ϴ� �����
{
  [SerializeField] private Image TouchBlock = null;
  [SerializeField] private TextMeshProUGUI SkillName;
  [SerializeField] private Image SkillIllust = null;
  [SerializeField] private TextMeshProUGUI SkillDescription = null;
  [SerializeField] private TextMeshProUGUI SkillLevel = null;
  [SerializeField] private Image SkillIcon = null;
  [SerializeField] private CanvasGroup BackButton = null;

  private int CurrentThemeIndex = -1;
  private Vector2 ClosePos =new Vector2(1608.0f, -470.0f);
  private Vector2 OpenPos =new Vector2( 342.0f,-470.0f);
  public void OpenUI(int _index)
  {
    if (UIManager.Instance.IsWorking) return;
    if (IsOpen && CurrentThemeIndex.Equals(_index)) { CloseUI(); return; }
    DefaultGroup.alpha = 1.0f;
    DefaultGroup.interactable = true;
    DefaultGroup.blocksRaycasts = true;
    BackButton.interactable = true;
    BackButton.blocksRaycasts = true;

    IsOpen = true;
    TouchBlock.enabled = true;
    SkillType _skilltype = (SkillType)_index;
    Sprite _icon = GameManager.Instance.ImageHolder.GetSkillIcon(_skilltype);
    Sprite _illust = GameManager.Instance.ImageHolder.GetSkillIllust(_skilltype);
    string _name = GameManager.Instance.GetTextData(_skilltype, 0), _desscription = GameManager.Instance.GetTextData(_skilltype, 3);

    SkillName.text = _name;
    SkillDescription.text = _desscription;
    SkillIcon.sprite = _icon;
    SkillIllust.sprite= _illust;
    SkillLevel.text = GameManager.Instance.MyGameData.GetSkill(_skilltype).Level.ToString();

    if (CurrentThemeIndex.Equals(-1))
    {
      UIManager.Instance.AddUIQueue(UIManager.Instance.OpenUI(DefaultRect,ClosePos,OpenPos,UIManager.Instance.LargePanelMoveTime,true));
    }//���� �ִ� ���¿��� ó������ ���������� UI ���� ����Ʈ
    else
    {
    }//���� ���¿��� �ٸ� �׸� ������ Ŭ���ѰŸ� ���빰�� �ٲٱ�
    CurrentThemeIndex = _index;
  }
  public override void CloseUI()
  {
    UIManager.Instance.CurrentTopUI = null;
    IsOpen = false;
    BackButton.interactable = false;
    BackButton.blocksRaycasts = false;
    TouchBlock.enabled = false;
    StartCoroutine(UIManager.Instance.ChangeAlpha(DefaultGroup, 0.0f, 0.1f, false));
    StartCoroutine(UIManager.Instance.CloseUI(DefaultRect,OpenPos,ClosePos,UIManager.Instance.LargePanelMoveTime, false));
    UIManager.Instance.CurrentTopUI = null;
    CurrentThemeIndex = -1;
  }
}
