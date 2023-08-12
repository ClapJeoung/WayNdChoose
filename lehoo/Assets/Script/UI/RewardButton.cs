using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardButton : MonoBehaviour
{
  [SerializeField] private RewardTarget RewardType = RewardTarget.Experience;
  [SerializeField] private TextMeshProUGUI RewardInfo = null;
  [SerializeField] private UI_Reward MyUIReward = null;

  private SkillType MySkillName = SkillType.Conversation;
  private Experience MyExp = null;

  public void Setup(StatusType statustype,int value,UI_Reward rewardui)
  {
    string _text = "";
    switch (statustype)
    {
      case StatusType.HP:RewardType = RewardTarget.HP;
        _text = $"{GameManager.Instance.GetTextData(StatusType.HP, 2)} +{WNCText.GetHPColor(value)}";
        break;
      case StatusType.Sanity:RewardType = RewardTarget.Sanity;
        _text = $"{GameManager.Instance.GetTextData(StatusType.Sanity, 2)} +{WNCText.GetHPColor(value)}";
        break;
      case StatusType.Gold:RewardType = RewardTarget.Gold;
        _text = $"{GameManager.Instance.GetTextData(StatusType.Gold, 2)} +{WNCText.GetHPColor(value)}";
        break;
    }
    
    RewardInfo.text = _text;
    MyUIReward = rewardui;
  }
  public void Setup(SkillType skilltype, UI_Reward rewardui)
  {
    MySkillName = skilltype;

    RewardInfo.text = $"{GameManager.Instance.GetTextData(skilltype,1)} {GameManager.Instance.GetTextData("SKILL_NAME")} +1";

    MyUIReward = rewardui;
  }
  public void Setup(Experience exp, UI_Reward rewardui)
  {
    MyExp= exp;

    RewardInfo.text = $"{GameManager.Instance.GetTextData("EXP_NAME")} : {exp.Name}";
    MyUIReward = rewardui;
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
        MyUIReward.AddRewardHP();
        CloseUIButton(); break;
      case RewardTarget.Sanity:
        MyUIReward.AddRewardSanity();
        CloseUIButton(); break;
      case RewardTarget.Gold:
        MyUIReward.AddRewardGold();
        CloseUIButton(); break;
      case RewardTarget.Skill:
        MyUIReward.AddRewardSkill(MySkillName);
        CloseUIButton(); break;
    }
  }
  public void CloseUIButton()
  {
    UIManager.Instance.PreviewManager.ClosePreview();
    Destroy(gameObject);
  }
}
