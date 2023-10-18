using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PreviewRewardGroup : MonoBehaviour
{
  public GameObject RewardObj = null;
  public TextMeshProUGUI RewardText = null;
  public Image RewardIcon = null;
  public GameObject PenaltyObj = null;
  public TextMeshProUGUI PenaltyText = null;
  public Image PenaltyIcon = null;

  public bool Setup(SelectionData selection)
  {
    bool _reward = false, _penalty = false;

    SuccessData _success = selection.SuccessData;
    if (_success.Reward_Type == RewardTypeEnum.None)
    {
      if(RewardObj.activeInHierarchy==true)RewardObj.SetActive(false);
    }
    else
    {
      if (RewardObj.activeInHierarchy == false) RewardObj.SetActive(true);
      if (RewardText.text=="")RewardText.text = GameManager.Instance.GetTextData("SuccessReward");

      Sprite _rewardicon = null;
      switch (_success.Reward_Type)
      {
        case RewardTypeEnum.Status:
          switch (_success.Reward_StatusType)
          {
            case StatusTypeEnum.HP: _rewardicon = GameManager.Instance.ImageHolder.HPIcon; break;
            case StatusTypeEnum.Sanity: _rewardicon = GameManager.Instance.ImageHolder.SanityIcon; break;
            case StatusTypeEnum.Gold: _rewardicon = GameManager.Instance.ImageHolder.GoldIcon; break;
          }
          break;
        case RewardTypeEnum.Experience: _rewardicon = GameManager.Instance.ImageHolder.UnknownExpRewardIcon; break;
        case RewardTypeEnum.Skill:
          _rewardicon = GameManager.Instance.ImageHolder.GetSkillIcon(_success.Reward_SkillType, false);
          break;
      }
      RewardIcon.sprite = _rewardicon;

      _reward = true;
    }

    FailData _faildata = selection.FailureData;
    if (_faildata == null||selection.ThisSelectionType==SelectionTargetType.Pay)
    {
      if (PenaltyObj.activeInHierarchy == true) PenaltyObj.SetActive(false);
    }
    else
    {
      if (_faildata.Penelty_target == PenaltyTarget.None)
      {
        if (PenaltyObj.activeInHierarchy == true) PenaltyObj.SetActive(false);
      }
      else
      {

        if (PenaltyObj.activeInHierarchy == false) PenaltyObj.SetActive(true);
        Sprite _penaltyicon = null;
        switch (_faildata.Penelty_target)
        {
          case PenaltyTarget.Status:
            switch (_faildata.StatusType)
            {
              case StatusTypeEnum.HP: _penaltyicon = GameManager.Instance.ImageHolder.HPDecreaseIcon; break;
              case StatusTypeEnum.Sanity: _penaltyicon = GameManager.Instance.ImageHolder.SanityDecreaseIcon; break;
              case StatusTypeEnum.Gold: _penaltyicon = GameManager.Instance.ImageHolder.GoldDecreaseIcon; break;
            }
            break;
        }
        if(PenaltyText.text=="") PenaltyText.text = GameManager.Instance.GetTextData("FailPenalty");
        PenaltyIcon.sprite = _penaltyicon;

        _penalty = true;
      }
    }

    return _reward  || _penalty ;

  }
}
