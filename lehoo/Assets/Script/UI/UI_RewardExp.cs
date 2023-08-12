using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_RewardExp : MonoBehaviour
{
  [SerializeField] private CanvasGroup MyGroup = null;

  [SerializeField] private TextMeshProUGUI RewardLongExpName = null;
  [SerializeField] private Image RewardLongExpCap = null;
  [SerializeField] private Image RewardLongExpIllust = null;
  [SerializeField] private CanvasGroup RewardLongExpTurnGroup = null;
  [SerializeField] private TextMeshProUGUI RewardLongExpTurn = null;
  [SerializeField] private PreviewInteractive RewardLongExpPreview = null;

  [SerializeField] private CanvasGroup[] RewardShortExpNameGroup = new CanvasGroup[2];
  [SerializeField] private TextMeshProUGUI[] RewardShortExpName = new TextMeshProUGUI[2];
  [SerializeField] private Image[] RewardShortExpCap = new Image[2];
  [SerializeField] private Image[] RewardShortExpIllust = new Image[2];
  [SerializeField] private CanvasGroup[] RewardShortExpTurnGroup = new CanvasGroup[2];
  [SerializeField] private TextMeshProUGUI[] RewardShortExpTurn = new TextMeshProUGUI[2];
  [SerializeField] private PreviewInteractive[] RewardShortExpPreview = new PreviewInteractive[2];
  [SerializeField] private GameObject RewardExpQuitButton = null;
  [SerializeField] private TextMeshProUGUI RewardExpDescription = null;
  private Experience CurrentExp = null;
  public void Setup(Experience rewardexp)
  {
    CurrentExp = rewardexp;


  }
}
