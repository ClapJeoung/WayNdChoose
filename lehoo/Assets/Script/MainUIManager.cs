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

  private string SelectQuestID = "";
  private void Start()
  {
    SetQuestDropdown();
  }
  public void SetQuestDropdown()
  {
    QuestDropdown.options.Clear();
    foreach(var _data in GameManager.Instance.MyProgressData.TotalFoundQuest)
    {
      TMP_Dropdown.OptionData _temp = new TMP_Dropdown.OptionData();
      _temp.text = GameManager.Instance.EventHolder.AllQuests[_data].QuestName;
      QuestDropdown.options.Add(_temp);
    }
  }
  public void SetQuestInfo(int _index)
  {
    SelectQuestID = GameManager.Instance.MyProgressData.TotalFoundQuest[_index];  //�ش� index�� ����Ʈ ID
    QuestHolder _targetquest = GameManager.Instance.EventHolder.AllQuests[SelectQuestID];//�ش� ����Ʈ ����

    Debug.Log($"Id : {SelectQuestID}  spriteID : {_targetquest.StartIllustID}   Description : {_targetquest.PreDescription}");
    QuestIllust.sprite = GameManager.Instance.ImageHolder.GetEventIllust(_targetquest.StartIllustID);
    QuestDescription.text = _targetquest.PreDescription;
  }
}
