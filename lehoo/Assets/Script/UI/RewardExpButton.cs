using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardExpButton : MonoBehaviour
{
  [SerializeField] private UI_Reward MyUIReward = null;

  [SerializeField] private RectTransform Long_0 = null;
  [SerializeField] private RectTransform Long_1 = null;
  [SerializeField] private RectTransform Short_0 = null;
  [SerializeField] private RectTransform Short_1 = null;
  [SerializeField] private RectTransform Short_2 = null;
  [SerializeField] private RectTransform Short_3= null;
  public void AddExpLongTerm(int _index)
  {
    if (UIManager.Instance.IsWorking) return;
    MyUIReward.AddRewardExp_Long(_index);
  }
  public void AddExpShortTerm(int _index)
  {
    if (UIManager.Instance.IsWorking) return;
    MyUIReward.AddRewardExp_Short(_index);
  }
}
