using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardSkillSelectionButton : MonoBehaviour
{
  [SerializeField] private UI_Reward MyUIReward = null;

  [SerializeField] private ThemeType MyTheme = ThemeType.Conversation;
  [SerializeField] private RectTransform MyIconRect = null;
  [SerializeField] private Image MyIconImage = null;
  [SerializeField] private RectTransform ConversationRect = null;
  [SerializeField] private RectTransform ForceRect = null;
  [SerializeField] private RectTransform WildRect = null;
  [SerializeField] private RectTransform IntelligenceRect = null;

  public void GetReward()
  {
    if (UIManager.Instance.IsWorking) return;
    Sprite[] _icons = new Sprite[] { MyIconImage.sprite };
    Vector2[] _startpos = new Vector2[] { MyIconRect.position };
    Vector2[] _endpos = new Vector2[1];
    switch (MyUIReward.CurrentSuccesData.Reward_Theme)
    {
      case ThemeType.Conversation:
        _endpos[0] = ConversationRect.position;
        break;
      case ThemeType.Force:
        _endpos[0] = ForceRect.position;
        break;
      case ThemeType.Wild:
        _endpos[0] = WildRect.position;
        break;
      case ThemeType.Intelligence:
        _endpos[0] = IntelligenceRect.position;
        break;
    }
    SkillName _skill = GameManager.Instance.MyGameData.GetSkillByTheme(MyUIReward.CurrentSuccesData.Reward_Theme, MyTheme);
    MyUIReward.AddRewardSkill(_skill, _icons, _startpos, _endpos);
  }
}
