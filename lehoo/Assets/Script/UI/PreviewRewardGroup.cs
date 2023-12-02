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
  public GameObject ExpEffectObj = null;
  public GameObject ExpEffect_Conv = null;
  public GameObject ExpEffect_Forc = null;
  public GameObject ExpEffect_Wild = null;
  public GameObject ExpEffect_Intel = null;
  public GameObject ExpEffect_HP = null;
  public GameObject ExpEffect_Sanity = null;
  public GameObject ExpEffect_Gold = null;
  private void SetExpEffectIcon(List<EffectType> effects)
  {
    if (effects.Contains(EffectType.Conversation))
    {
      if (!ExpEffect_Conv.activeInHierarchy) ExpEffect_Conv.SetActive(true);
    }
    else
    {
      if (ExpEffect_Conv.activeInHierarchy) ExpEffect_Conv.SetActive(false);
    }
    if (effects.Contains(EffectType.Force))
    {
      if (!ExpEffect_Forc.activeInHierarchy) ExpEffect_Forc.SetActive(true);
    }
    else
    {
      if (ExpEffect_Forc.activeInHierarchy) ExpEffect_Forc.SetActive(false);
    }
    if (effects.Contains(EffectType.Wild))
    {
      if (!ExpEffect_Wild.activeInHierarchy) ExpEffect_Wild.SetActive(true);
    }
    else
    {
      if (ExpEffect_Wild.activeInHierarchy) ExpEffect_Wild.SetActive(false);
    }
    if (effects.Contains(EffectType.Intelligence))
    {
      if (!ExpEffect_Intel.activeInHierarchy) ExpEffect_Intel.SetActive(true);
    }
    else
    {
      if (ExpEffect_Intel.activeInHierarchy) ExpEffect_Intel.SetActive(false);
    }
    if (effects.Contains(EffectType.HPLoss))
    {
      if (!ExpEffect_HP.activeInHierarchy) ExpEffect_HP.SetActive(true);
    }
    else
    {
      if (ExpEffect_HP.activeInHierarchy) ExpEffect_HP.SetActive(false);
    }
    if (effects.Contains(EffectType.SanityLoss))
    {
      if (!ExpEffect_Sanity.activeInHierarchy) ExpEffect_Sanity.SetActive(true);
    }
    else
    {
      if (ExpEffect_Sanity.activeInHierarchy) ExpEffect_Sanity.SetActive(false);
    }
    if (effects.Contains(EffectType.GoldGen))
    {
      if (!ExpEffect_Gold.activeInHierarchy) ExpEffect_Gold.SetActive(true);
    }
    else
    {
      if (ExpEffect_Gold.activeInHierarchy) ExpEffect_Gold.SetActive(false);
    }
  }
  public GameObject PenaltyObj = null;
  public TextMeshProUGUI PenaltyText = null;
  public Image PenaltyIcon = null;

  public bool Setup(SelectionData selection)
  {
    bool _reward = false, _penalty = false;
    bool _hidereward=false, _hidepenalty = false;
    switch(selection.Tendencytype)
    {
      case TendencyTypeEnum.Body:
        _hidepenalty = selection.Index == 0 ? GameManager.Instance.MyGameData.Tendency_Body.Level < 0 : GameManager.Instance.MyGameData.Tendency_Body.Level > 0;
        _hidereward = selection.Index == 0 ? GameManager.Instance.MyGameData.Tendency_Body.Level < -1 : GameManager.Instance.MyGameData.Tendency_Body.Level > 1;
        break;
      case TendencyTypeEnum.Head:
        _hidepenalty = selection.Index == 0 ? GameManager.Instance.MyGameData.Tendency_Head.Level < 0 : GameManager.Instance.MyGameData.Tendency_Head.Level > 0;
        _hidereward = selection.Index == 0 ? GameManager.Instance.MyGameData.Tendency_Head.Level < -1 : GameManager.Instance.MyGameData.Tendency_Head.Level > 1;
        break;
    }

    if (RewardText.text == "") RewardText.text = GameManager.Instance.GetTextData("Success")+" :";
    if (PenaltyText.text == "") PenaltyText.text = GameManager.Instance.GetTextData("Fail") + " :";

    if (!_hidereward)
    {
      SuccessData _success = selection.SuccessData;
      if (_success.Reward_Type == RewardTypeEnum.None)
      {
        if (RewardObj.activeInHierarchy == true) RewardObj.SetActive(false);
      }
      else
      {
        if (RewardObj.activeInHierarchy == false) RewardObj.SetActive(true);

        Sprite _rewardicon = null;
        switch (_success.Reward_Type)
        {
          case RewardTypeEnum.Status:
            if (!RewardIcon.gameObject.activeInHierarchy) RewardIcon.gameObject.SetActive(true);
            if (ExpEffectObj.activeInHierarchy) ExpEffectObj.SetActive(false);
            switch (_success.Reward_StatusType)
            {
              case StatusTypeEnum.HP: _rewardicon = GameManager.Instance.ImageHolder.HPIcon; break;
              case StatusTypeEnum.Sanity: _rewardicon = GameManager.Instance.ImageHolder.SanityIcon; break;
              case StatusTypeEnum.Gold: _rewardicon = GameManager.Instance.ImageHolder.GoldIcon; break;
            }
            RewardIcon.sprite = _rewardicon;
            break;
          case RewardTypeEnum.Experience:
            if (RewardIcon.gameObject.activeInHierarchy) RewardIcon.gameObject.SetActive(false);
            if (!ExpEffectObj.activeInHierarchy) ExpEffectObj.SetActive(true);
            SetExpEffectIcon(GameManager.Instance.ExpDic[_success.Reward_EXPID].Effects);
            break;
          case RewardTypeEnum.Skill:
            if (!RewardIcon.gameObject.activeInHierarchy) RewardIcon.gameObject.SetActive(true);
            if (ExpEffectObj.activeInHierarchy) ExpEffectObj.SetActive(false);
            _rewardicon = GameManager.Instance.ImageHolder.GetSkillIcon(_success.Reward_SkillType, false);
            RewardIcon.sprite = _rewardicon;
            break;
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(RewardIcon.transform.parent.transform as RectTransform);
        _reward = true;
      }
    }
    else
    {
      if (RewardObj.activeInHierarchy == false) RewardObj.SetActive(true);
      if (!RewardIcon.gameObject.activeInHierarchy) RewardIcon.gameObject.SetActive(true);
      if (ExpEffectObj.activeInHierarchy) ExpEffectObj.SetActive(false);
      RewardIcon.sprite = GameManager.Instance.ImageHolder.UnknownExpRewardIcon;
      _reward = true;
    }

    if (!_hidepenalty)
    {
      FailData _faildata = selection.FailData;
      if (_faildata == null || selection.ThisSelectionType == SelectionTargetType.Pay)
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
          PenaltyIcon.sprite = _penaltyicon;

          _penalty = true;
        }
      }
    }
    else
    {
      if (selection.ThisSelectionType == SelectionTargetType.Check_Single || selection.ThisSelectionType == SelectionTargetType.Check_Multy)
      {
        if (PenaltyObj.activeInHierarchy == false) PenaltyObj.SetActive(true);
        PenaltyIcon.sprite = GameManager.Instance.ImageHolder.UnknownExpRewardIcon;
      }
      else
      {
        if (PenaltyObj.activeInHierarchy == true) PenaltyObj.SetActive(false);
      }
    }

    LayoutRebuilder.ForceRebuildLayoutImmediate(RewardObj.transform.parent.transform as RectTransform);

    return _reward  || _penalty ;

  }
}
