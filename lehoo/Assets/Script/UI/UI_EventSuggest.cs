using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_EventSuggest : UI_default
{

  [SerializeField] private TextMeshProUGUI SettleName = null;
  [SerializeField] private TextMeshProUGUI CurrentUnPlesant = null;
  [SerializeField] private TextMeshProUGUI CurrentSanityLoss = null;
  [SerializeField] private GameObject[] Panel = null;
  [SerializeField] private TextMeshProUGUI[] Panel_Name = null;
  [SerializeField] private Image[] Panel_Illust = null;
  [SerializeField] private TextMeshProUGUI[] Panel_Description = null;
  [SerializeField] private Image[] Panel_Title = null;
  [SerializeField] private CanvasGroup[] Panel_Group = null;

  public void OpenSuggest()
  {
    SettleName.text = GameManager.Instance.MyGameData.CurrentSettlement.Name;
    List<EventDataDefulat> _eventlist = GameManager.Instance.MyGameData.CurrentSuggestingEvents;
    CurrentUnPlesant.text = $"{GameManager.Instance.GetTextData("currentunpleasant").Name}  {GameManager.Instance.MyGameData.AllSettleUnpleasant[GameManager.Instance.MyGameData.CurrentSettlement]}";
    CurrentSanityLoss.text = $"{GameManager.Instance.GetTextData("currentsettlesanityloss").Name} {GameManager.Instance.GetTextData("sanity").Name} " +
      $"{GameManager.Instance.MyGameData.SettleSanityLoss} {GameManager.Instance.GetTextData("decrease").Name}";

    for(int i=0;i<Panel.Length;i++)if(Panel[i].activeInHierarchy)Panel[i].SetActive(false);

    for(int i = 0; i < _eventlist.Count; i++)
    {
      if(Panel[i].activeInHierarchy==false)Panel[i].SetActive(true);
      Panel_Group[i].alpha = 1.0f;
      Panel_Group[i].interactable = true;
      Panel_Group[i].blocksRaycasts = true;
      Panel_Name[i].text =GameManager.Instance.GetTextData(_eventlist[i].PlaceType).Name;
      Panel_Illust[i].sprite = GameManager.Instance.ImageHolder.GetPlaceIllust(_eventlist[i].PlaceType);
      Panel_Description[i].text = GameManager.Instance.GetTextData(_eventlist[i].GetType()).Name;
      //�߿䵵�� ���� Panel_Tile[i].sprite�� ����
    }
    UIManager.Instance.OpenUI(MyRect, MyGroup, MyDir, false);
  }//�������� �����ϸ� �̺�Ʈ�� �޾ƿ� ���� ���� �г��� ������ �� ����
  public void SelectEvent(int _index)
  {
    if (UIManager.Instance.IsWorking) return;

    EventDataDefulat _selectevent = GameManager.Instance.MyGameData.CurrentSuggestingEvents[_index];
    GameManager.Instance.SelectEvent(_selectevent);

    for(int i = 0; i < Panel.Length; i++)
    {
      if (i.Equals(_index)) continue;
      if (Panel[i].activeInHierarchy == false) continue;
      UIManager.Instance.CloseUI(Panel_Group[i],true);
    }
    CloseUI();
  }//�г��� Ŭ���ϸ� ���� �̺�Ʈ ������ �����ϰ�, �ִϸ��̼� ����, ������ �����Ѵ�
  
}
