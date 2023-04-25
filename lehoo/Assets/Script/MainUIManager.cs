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
  private void Start()
  {
    SetQuestDropdown();
  }
  public void SetQuestDropdown()
  {
    QuestDropdown.options.Clear();
    TMP_Dropdown.OptionData _nullthing = new TMP_Dropdown.OptionData();
    _nullthing.text = "null";
    QuestDropdown.options.Add(_nullthing);
    foreach (var _data in GameManager.Instance.MyProgressData.TotalFoundQuest)
    {
      TMP_Dropdown.OptionData _temp = new TMP_Dropdown.OptionData();
      _temp.text = GameManager.Instance.EventHolder.AllQuests[_data].QuestName;
      QuestDropdown.options.Add(_temp);
    }
  }
  public void SetQuestInfo(int _index)
  {
    if (_index == 0)
    {
      SelectQuestID = "";
      QuestIllust.sprite = GameManager.Instance.ImageHolder.DefaultIllust;
      QuestDescription.text = "Heil Gamja";
    }
    else
    {
      SelectQuestID = GameManager.Instance.MyProgressData.TotalFoundQuest[_index - 1];  //해당 index번 퀘스트 ID
      QuestHolder _targetquest = GameManager.Instance.EventHolder.AllQuests[SelectQuestID];//해당 퀘스트 정보

 //     Debug.Log($"Id : {SelectQuestID}  Description : {_targetquest.PreDescription}");
      QuestIllust.sprite = _targetquest.Illust;
      QuestDescription.text = _targetquest.PreDescription;
    }
  }
}
