using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardExpButton : MonoBehaviour
{
  [SerializeField] private UI_Reward MyUIReward = null;
  public void AddExpLongTerm()
  {
    if (UIManager.Instance.IsWorking) return;
    MyUIReward.AddRewardExp_Long();
  }
  public void AddExpShortTerm(int _index)
  {
    if (UIManager.Instance.IsWorking) return;
    MyUIReward.AddRewardExp_Short(_index);
  }
}
