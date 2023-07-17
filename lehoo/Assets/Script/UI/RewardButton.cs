using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardButton : MonoBehaviour
{
  [SerializeField] private RewardTarget RewardType = RewardTarget.Experience;
  [SerializeField] private Image Icon_A = null;
  [SerializeField] private Image Icon_B = null;
  [SerializeField] private TextMeshProUGUI RewardInfo = null;
  [SerializeField] private UI_Reward MyUIReward = null;

  private int MyValue = -1;
  private string MyID = "";
  private Vector2[] TargetThemePos;
  private SkillType MySkillName = SkillType.Conversation;
  private Experience MyExp = null;

  public void Setup_value(int _value)
  {
    GetComponent<Button>().interactable = true;
     MyValue = _value;
    switch (RewardType)
    {
      case RewardTarget.HP: RewardInfo.text = GameManager.Instance.GetTextData(StatusType.HP,2)+" "+ MyValue.ToString();break;
      case RewardTarget.Sanity: RewardInfo.text = GameManager.Instance.GetTextData(StatusType.Sanity, 2) + " " + MyValue.ToString(); break;
      case RewardTarget.Gold: RewardInfo.text = GameManager.Instance.GetTextData(StatusType.Gold, 2) + " " + MyValue.ToString(); break;
    }
  }
  public void Setup_Expid(string _id)
  {
    GetComponent<Button>().interactable = true;
    RewardType = RewardTarget.Experience;
    MyID = _id;
    Debug.Log(MyID);
    RewardInfo.text =$"{GameManager.Instance.GetTextData("EXP_NAME")} - {GameManager.Instance.ExpDic[MyID].Name}";
    MyExp=GameManager.Instance.ExpDic[MyID];
  }
  public void Setup_skill(SkillType skill)
  {
    RewardType= RewardTarget.Skill;
    MySkillName = skill;
    GetComponent<Button>().interactable = true;
    Icon_A.sprite = GameManager.Instance.ImageHolder.GetSkillIcon(skill);
    Icon_B.sprite = GameManager.Instance.ImageHolder.UnknownTheme;
    TargetThemePos = new Vector2[1];
  }

  public void GetReward()
  {
    if (UIManager.Instance.IsWorking) return;
    switch (RewardType)
    {
      case RewardTarget.Experience:
        MyUIReward.OpenRewardExpPanel_reward();
        break;
      case RewardTarget.HP:
        MyUIReward.AddRewardHP(MyValue);
        CloseUIButton(); break;
      case RewardTarget.Sanity:
        MyUIReward.AddRewardSanity(MyValue);
        CloseUIButton(); break;
      case RewardTarget.Gold:
        MyUIReward.AddRewardGold(MyValue);
        CloseUIButton(); break;
      case RewardTarget.Skill:
        MyUIReward.AddRewardSkill(MySkillName);
        CloseUIButton(); break;
    }
  }
  public void CloseUIButton()
  {
    GetComponent<Button>().interactable = false;
    UIManager.Instance.PreviewManager.ClosePreview();
    gameObject.SetActive(false);
  }
}
