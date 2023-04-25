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

  [SerializeField] private RectTransform HPRect = null;
  [SerializeField] private RectTransform SanityRect = null;
  [SerializeField] private RectTransform GoldRect = null;
  [SerializeField] private RectTransform TraitRect = null;
  [SerializeField] private RectTransform ConversationRect = null;
  [SerializeField] private RectTransform ForceRect = null;
  [SerializeField] private RectTransform NatureRect = null;
  [SerializeField] private RectTransform IntelligenceRect = null;
  [SerializeField] private RectTransform ExpRect = null;
  private int MyValue = -1;
  private string MyID = "";
  private Vector2[] TargetThemePos;
  private ThemeType MyThemeType = ThemeType.Conversation;
  private SkillName MySkillName = SkillName.Speech;
  private Experience MyExp = null;
  private Trait MyTrait = null;

  public void Setup_value(int _value)
  {
    GetComponent<Button>().interactable = true;
     MyValue = _value;
    RewardInfo.text = MyValue.ToString();
  }
  public void Setup_Traitid(string _id)
  {
    GetComponent<Button>().interactable = true;
    RewardType = RewardTarget.Trait;
    MyID= _id;
    RewardInfo.text = GameManager.Instance.GetTextData(MyID).Name;
    MyTrait=GameManager.Instance.TraitsDic[MyID];
  }
  public void Setup_Expid(string _id)
  {
    GetComponent<Button>().interactable = true;
    RewardType = RewardTarget.Experience;
    MyID = _id;
    RewardInfo.text = GameManager.Instance.GetTextData(MyID).Name;
    MyExp=GameManager.Instance.ExpDic[MyID];
  }
  public void Setup_theme(ThemeType _theme)
  {
    RewardType= RewardTarget.Theme;
    MyThemeType = _theme;
    GetComponent<Button>().interactable = true;
    Icon_A.sprite = GameManager.Instance.ImageHolder.GetThemeIcon(_theme);
    Icon_B.sprite = GameManager.Instance.ImageHolder.UnknownTheme;
    TargetThemePos = new Vector2[1];
    switch (_theme)
    {
      case ThemeType.Conversation:TargetThemePos[0]=ConversationRect.anchoredPosition; break;
      case ThemeType.Force: TargetThemePos[0] = ForceRect.anchoredPosition; break;
      case ThemeType.Nature: TargetThemePos[0] = NatureRect.anchoredPosition; break;
      case ThemeType.Intelligence: TargetThemePos[0] = IntelligenceRect.anchoredPosition; break;
    }
  }
  public void Setup_skill(SkillName _skill)
  {
    RewardType = RewardTarget.Skill;
    MySkillName = _skill;
    GetComponent<Button>().interactable = true;
    Sprite[] _icons = new Sprite[2];
    GameManager.Instance.ImageHolder.GetSkillIcons(_skill, ref _icons);
    Icon_A.sprite = _icons[0];
    Icon_B.sprite=_icons[1];
    TargetThemePos = new Vector2[2];
    switch (_skill)
    {
      case SkillName.Speech: TargetThemePos[0] = ConversationRect.position;TargetThemePos[1] = ConversationRect.position; break;
      case SkillName.Threat: TargetThemePos[0] = ConversationRect.position; TargetThemePos[1] = ForceRect.position; break;
      case SkillName.Deception: TargetThemePos[0] = ConversationRect.position; TargetThemePos[1] = NatureRect.position; break;
      case SkillName.Logic: TargetThemePos[0] = ConversationRect.position; TargetThemePos[1] = IntelligenceRect.position; break;
      case SkillName.Martialarts: TargetThemePos[0] = ForceRect.position; TargetThemePos[1] = ForceRect.position; break;
      case SkillName.Bow: TargetThemePos[0] = NatureRect.position; TargetThemePos[1] = ForceRect.position; break;
      case SkillName.Somatology: TargetThemePos[0] = ForceRect.position; TargetThemePos[1] = IntelligenceRect.position; break;
      case SkillName.Survivable: TargetThemePos[0] = NatureRect.position; TargetThemePos[1] = NatureRect.position; break;
      case SkillName.Biology: TargetThemePos[0] = NatureRect.position; TargetThemePos[1] = IntelligenceRect.position; break;
      case SkillName.Knowledge:  TargetThemePos[0] = IntelligenceRect.position; TargetThemePos[1] = IntelligenceRect.position; break;
    }
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
        MyUIReward.AddRewardHP(MyValue, new Sprite[] { Icon_A.sprite }, new Vector2[] { Icon_A.rectTransform.position }, new Vector2[] { HPRect.position });
        CloseUIButton(); break;
      case RewardTarget.Sanity:
        MyUIReward.AddRewardSanity(MyValue,new Sprite[] { Icon_A.sprite }, new Vector2[] { Icon_A.rectTransform.position }, new Vector2[] { SanityRect.position });
        CloseUIButton(); break;
      case RewardTarget.Gold:
        MyUIReward.AddRewardGold(MyValue,new Sprite[] { Icon_A.sprite }, new Vector2[] { Icon_A.rectTransform.position }, new Vector2[] { GoldRect.position });
        CloseUIButton(); break;
      case RewardTarget.Theme:
        MyUIReward.OpenRewardSkillPanel(MyThemeType);
        CloseUIButton(); break;
      case RewardTarget.Skill:
        MyUIReward.AddRewardSkill(MySkillName, new Sprite[] { Icon_A.sprite, Icon_B.sprite }, new Vector2[] { Icon_A.rectTransform.position, Icon_B.rectTransform.position },
   TargetThemePos);
        CloseUIButton(); break;
      case RewardTarget.Trait:
        MyUIReward.AddRewardTrait(MyTrait, new Sprite[] { Icon_A.sprite }, new Vector2[] { Icon_A.rectTransform.position },new Vector2[]{TraitRect.position});
        CloseUIButton();  break;
    }
  }
  public void CloseUIButton()
  {
    GetComponent<Button>().interactable = false;
    UIManager.Instance.PreviewManager.ClosePreview();
    gameObject.SetActive(false);
  }
}
