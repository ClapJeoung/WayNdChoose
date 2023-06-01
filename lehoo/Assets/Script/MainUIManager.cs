using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainUIManager : MonoBehaviour
{
    private static MainUIManager instance;
  public static MainUIManager Instance { get { return instance; } }
  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
    }
    else Destroy(gameObject);
  }

  [SerializeField] private TMP_Dropdown QuestDropdown = null;
  [SerializeField] private Image QuestIllust = null;
  [SerializeField] private TextMeshProUGUI QuestDescription = null;

  [HideInInspector]public string SelectQuestID = "";
}
