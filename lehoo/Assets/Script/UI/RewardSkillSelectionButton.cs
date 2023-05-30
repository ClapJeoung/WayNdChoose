using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RewardSkillSelectionButton : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
  [SerializeField] private UI_Reward MyUIReward = null;

  [SerializeField] private ThemeType MyTheme = ThemeType.Conversation;
  [SerializeField] private RectTransform MyIconRect = null;
  [SerializeField] private Image MyIconImage = null;

  [SerializeField] private TMPro.TextMeshProUGUI SkillNameText = null;

  public void OnPointerEnter(PointerEventData data)
  {
    SkillNameText.text = GameManager.Instance.GetTextData(GameManager.Instance.MyGameData.GetSkillByTheme(MyUIReward.CurrentSuccesData.Reward_Theme, MyTheme)).Name;
  }
  public void OnPointerExit(PointerEventData data)
  {
    SkillNameText.text = "";
  }

  public void GetReward()
  {
    if (UIManager.Instance.IsWorking) return;
    SkillName _skill = GameManager.Instance.MyGameData.GetSkillByTheme(MyUIReward.CurrentSuccesData.Reward_Theme, MyTheme);
    MyUIReward.AddRewardSkill(_skill);
  }
}
