using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Text;

public class UI_Main_Illusts : MonoBehaviour
{
  private int CurrentIndex = 0;
  #region 엔딩 일러스트
  [SerializeField] private Button EndingPanelButton = null;
  [SerializeField] private TextMeshProUGUI EndingPanelButton_text = null;
  [SerializeField] private CanvasGroup EndingGroup = null;
  [SerializeField] private PreviewInteractive[] EndingPreviews = null;
  [SerializeField] private GameObject EndingIllustObj = null;
  [SerializeField] private ImageSwapScript EndingIllust = null;
  [SerializeField] private GameObject EndingArrowHolder = null;
  [SerializeField] private GameObject EndingIllust_Previewbutton = null;
  [SerializeField] private GameObject EndingIllust_Nextbutton = null;
  private EndingData SelectedEnding = null;
  public void SelectEnding(int index)
  {
    if (SelectedEnding == GameManager.Instance.ImageHolder.EndingList[index]) return;
    SelectedEnding= GameManager.Instance.ImageHolder.EndingList[index];
    CurrentIndex = 0;
    if (!EndingIllustObj.activeSelf) EndingIllustObj.SetActive(true);
    if (!EndingArrowHolder.activeSelf) EndingArrowHolder.SetActive(true);
    EndingIllust.Next(SelectedEnding.Illusts[CurrentIndex]);
    if(EndingIllust_Previewbutton.activeSelf) EndingIllust_Previewbutton.SetActive(false);
    if(SelectedEnding.Illusts.Length>1&&!EndingIllust_Nextbutton.activeSelf) EndingIllust_Nextbutton.SetActive(true);
  }
  public void EndingIllust_Preview()
  {
    CurrentIndex--;
    EndingIllust.Next(SelectedEnding.Illusts[CurrentIndex]);
    if(CurrentIndex==0) EndingIllust_Previewbutton.SetActive(false);
    if (SelectedEnding.Illusts.Length > 1 && !EndingIllust_Nextbutton.activeSelf) EndingIllust_Nextbutton.SetActive(true);
  }
  public void EndingIllust_Next()
  {
    CurrentIndex++;
    EndingIllust.Next(SelectedEnding.Illusts[CurrentIndex]);
    if (!EndingIllust_Previewbutton.activeSelf) EndingIllust_Previewbutton.SetActive(true);
    if (SelectedEnding.Illusts.Length - 1 == CurrentIndex) EndingIllust_Nextbutton.SetActive(false);
  }
  #endregion
  #region 이벤트 일러스트
  [Space(20)]
  [SerializeField] private Button EventPanelButton = null;
  [SerializeField] private TextMeshProUGUI EventPanelButton_text = null;
  [SerializeField] private CanvasGroup EventGroup = null;
  [SerializeField] private GameObject[] EventButton_Name = null;
  [SerializeField] private TextMeshProUGUI SelectEventName = null;
  [SerializeField] private GameObject EventProgressHolder = null;
  [SerializeField] private Button[] EventButton_Progress = null;
  [SerializeField] private CanvasGroup[] EventButton_Progress_Group = null;
  [SerializeField] private GameObject SeasonHolder = null;
  [SerializeField] private Button[] SeasonButtons = null;
  [SerializeField] private GameObject EventIllust_Arrowholder = null;
  [SerializeField] private Button EventIllust_Previewbutton = null;
  [SerializeField] private Button EventIllust_Nextbutton = null;
  [SerializeField] private ImageSwapScript EventIllust = null;
  private EventData SelectedEvent = null;
  private EventIllustHolder[] SelectedIllusts = null;
  public void SelectEvent(string id)
  {
    if (SelectedEvent == GameManager.Instance.EventHolder.GetEvent(id)) return;

    for (int i = 0; i < EventButton_Progress.Length; i++)
      EventButton_Progress[i].interactable = i!=0;

    EventIllust.Next(GameManager.Instance.ImageHolder.Transparent);
    SelectedEvent = GameManager.Instance.EventHolder.GetEvent(id);
    SelectEventName.text = SelectedEvent.Name;
    bool _active = false;
    switch (SelectedEvent.Selection_type)
    {
      case SelectionTypeEnum.Single:
        SpriteState _state = GameManager.Instance.ImageHolder.GetSelectionButtonBackground(TendencyTypeEnum.None, false);
        EventButton_Progress[1].GetComponent<Button>().spriteState = _state;
        EventButton_Progress[1].GetComponent<Image>().sprite = _state.selectedSprite;
        _active = GameManager.Instance.ProgressData.EventList[id].LSuccess > 1;
        EventButton_Progress[1].GetComponent<Button>().interactable = GameManager.Instance.ProgressData.EventList[id].LSuccess>1;
        EventButton_Progress_Group[1].alpha = _active?1.0f:0.4f;
        EventButton_Progress_Group[1].interactable = _active;
        EventButton_Progress[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
          SelectedEvent.SelectionDatas[0].Name+ GameManager.Instance.GetTextData("EventProgress_NormalSuccess");

        if (SelectedEvent.SelectionDatas[0].FailData != null)
        {
          EventButton_Progress[2].GetComponent<Button>().spriteState = _state;
          EventButton_Progress[2].GetComponent<Image>().sprite = _state.selectedSprite;
          _active = GameManager.Instance.ProgressData.EventList[id].LSuccess > 0;
          EventButton_Progress[2].GetComponent<Button>().interactable = _active;
          EventButton_Progress_Group[2].alpha = _active ? 1.0f:0.4f;
          EventButton_Progress_Group[2].interactable = _active;
          EventButton_Progress[2].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
    SelectedEvent.SelectionDatas[0].Name + GameManager.Instance.GetTextData("EventProgress_NormalFail");
          if (!EventButton_Progress[2].gameObject.activeSelf) EventButton_Progress[2].gameObject.SetActive(true);
        }
        else
        {
          if (EventButton_Progress[2].gameObject.activeSelf) EventButton_Progress[2].gameObject.SetActive(false);
        }

        if (EventButton_Progress[3].gameObject.activeSelf) EventButton_Progress[3].gameObject.SetActive(false);
        if (EventButton_Progress[4].gameObject.activeSelf) EventButton_Progress[4].gameObject.SetActive(false);
        break;
      case SelectionTypeEnum.Body:
        SpriteState _state_body_l = GameManager.Instance.ImageHolder.GetSelectionButtonBackground(TendencyTypeEnum.Body, true);
        EventButton_Progress[1].GetComponent<Button>().spriteState = _state_body_l;
        EventButton_Progress[1].GetComponent<Image>().sprite = _state_body_l.selectedSprite;
        _active = GameManager.Instance.ProgressData.EventList[id].LSuccess > 1;
        EventButton_Progress[1].GetComponent<Button>().interactable = _active;
        EventButton_Progress_Group[1].alpha = _active ? 1.0f : 0.4f;
        EventButton_Progress_Group[1].interactable = _active;
        EventButton_Progress[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
          SelectedEvent.SelectionDatas[0].Name+ GameManager.Instance.GetTextData("EventProgress_NormalSuccess");

        if (SelectedEvent.SelectionDatas[0].FailData != null)
        {
          EventButton_Progress[2].GetComponent<Button>().spriteState = _state_body_l;
          EventButton_Progress[2].GetComponent<Image>().sprite = _state_body_l.selectedSprite;
          _active = GameManager.Instance.ProgressData.EventList[id].LSuccess > 0;
          EventButton_Progress[2].GetComponent<Button>().interactable = _active;
          EventButton_Progress_Group[2].alpha = _active ? 1.0f : 0.4f;
          EventButton_Progress_Group[2].interactable = _active;
          EventButton_Progress[2].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
            SelectedEvent.SelectionDatas[0].Name + GameManager.Instance.GetTextData("EventProgress_NormalFail");
          if (!EventButton_Progress[2].gameObject.activeSelf) EventButton_Progress[2].gameObject.SetActive(true);
        }
        else
        {
          if (EventButton_Progress[2].gameObject.activeSelf) EventButton_Progress[2].gameObject.SetActive(false);
        }

        SpriteState _state_body_r = GameManager.Instance.ImageHolder.GetSelectionButtonBackground(TendencyTypeEnum.Body, false);
        EventButton_Progress[3].GetComponent<Button>().spriteState = _state_body_r;
        EventButton_Progress[3].GetComponent<Image>().sprite = _state_body_r.selectedSprite;
        _active = GameManager.Instance.ProgressData.EventList[id].RSuccess > 1;
        EventButton_Progress[3].GetComponent<Button>().interactable = _active;
        EventButton_Progress_Group[3].alpha = _active ? 1.0f : 0.4f;
        EventButton_Progress_Group[3].interactable = _active;
        EventButton_Progress[3].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
          SelectedEvent.SelectionDatas[1].Name + GameManager.Instance.GetTextData("EventProgress_NormalSuccess");
        if (!EventButton_Progress[3].gameObject.activeSelf) EventButton_Progress[3].gameObject.SetActive(true);

        if (SelectedEvent.SelectionDatas[1].FailData != null)
        {
          EventButton_Progress[4].GetComponent<Button>().spriteState = _state_body_r;
          EventButton_Progress[4].GetComponent<Image>().sprite = _state_body_r.selectedSprite;
          _active = GameManager.Instance.ProgressData.EventList[id].RSuccess > 0;
          EventButton_Progress[4].GetComponent<Button>().interactable = _active;
          EventButton_Progress_Group[4].alpha = _active ? 1.0f : 0.4f;
          EventButton_Progress_Group[4].interactable = _active;
          EventButton_Progress[4].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
            SelectedEvent.SelectionDatas[1].Name + GameManager.Instance.GetTextData("EventProgress_NormalFail");
          if (!EventButton_Progress[4].gameObject.activeSelf) EventButton_Progress[4].gameObject.SetActive(true);
        }
        else
        {
          if (EventButton_Progress[4].gameObject.activeSelf) EventButton_Progress[4].gameObject.SetActive(false);
        }
        break;
      case SelectionTypeEnum.Head:
        SpriteState _state_head_l = GameManager.Instance.ImageHolder.GetSelectionButtonBackground(TendencyTypeEnum.Head, true);
        EventButton_Progress[1].GetComponent<Button>().spriteState = _state_head_l;
        EventButton_Progress[1].GetComponent<Image>().sprite = _state_head_l.selectedSprite;
        _active = GameManager.Instance.ProgressData.EventList[id].LSuccess > 1;
        EventButton_Progress[1].GetComponent<Button>().interactable = _active;
        EventButton_Progress_Group[1].alpha = _active ? 1.0f : 0.4f;
        EventButton_Progress_Group[1].interactable = _active;
        EventButton_Progress[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
          SelectedEvent.SelectionDatas[0].Name + GameManager.Instance.GetTextData("EventProgress_NormalSuccess");

        if (SelectedEvent.SelectionDatas[0].FailData != null)
        {
          EventButton_Progress[2].GetComponent<Button>().spriteState = _state_head_l;
          EventButton_Progress[2].GetComponent<Image>().sprite = _state_head_l.selectedSprite;
          _active = GameManager.Instance.ProgressData.EventList[id].LSuccess > 0;
          EventButton_Progress[2].GetComponent<Button>().interactable = _active;
          EventButton_Progress_Group[2].alpha = _active ? 1.0f : 0.4f;
          EventButton_Progress_Group[2].interactable = _active;
          EventButton_Progress[2].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
            SelectedEvent.SelectionDatas[0].Name + GameManager.Instance.GetTextData("EventProgress_NormalFail");
          if (!EventButton_Progress[2].gameObject.activeSelf) EventButton_Progress[2].gameObject.SetActive(true);
        }
        else
        {
          if (EventButton_Progress[2].gameObject.activeSelf) EventButton_Progress[2].gameObject.SetActive(false);
        }

        SpriteState _state_head_r = GameManager.Instance.ImageHolder.GetSelectionButtonBackground(TendencyTypeEnum.Head, false);
        EventButton_Progress[3].GetComponent<Button>().spriteState = _state_head_r;
        EventButton_Progress[3].GetComponent<Image>().sprite = _state_head_r.selectedSprite;
        _active = GameManager.Instance.ProgressData.EventList[id].RSuccess > 1;
        EventButton_Progress[3].GetComponent<Button>().interactable = _active;
        EventButton_Progress_Group[3].alpha = _active ? 1.0f : 0.4f;
        EventButton_Progress[3].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
          SelectedEvent.SelectionDatas[1].Name + GameManager.Instance.GetTextData("EventProgress_NormalSuccess");
        if (!EventButton_Progress[3].gameObject.activeSelf) EventButton_Progress[3].gameObject.SetActive(true);

        if (SelectedEvent.SelectionDatas[1].FailData != null)
        {
          EventButton_Progress[4].GetComponent<Button>().spriteState = _state_head_r;
          EventButton_Progress[4].GetComponent<Image>().sprite = _state_head_r.selectedSprite;
          _active = GameManager.Instance.ProgressData.EventList[id].RSuccess > 0;
          EventButton_Progress[4].GetComponent<Button>().interactable = _active;
          EventButton_Progress_Group[4].alpha = _active ? 1.0f : 0.4f;
          EventButton_Progress_Group[4].interactable = _active;
          EventButton_Progress[4].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
            SelectedEvent.SelectionDatas[1].Name + GameManager.Instance.GetTextData("EventProgress_NormalFail");
          if (!EventButton_Progress[4].gameObject.activeSelf) EventButton_Progress[4].gameObject.SetActive(true);
        }
        else
        {
          if (EventButton_Progress[4].gameObject.activeSelf) EventButton_Progress[4].gameObject.SetActive(false);
        }
        break;
    }
    SelectEventIllust(0);
    if (!EventProgressHolder.activeSelf) EventProgressHolder.SetActive(true);
  }
  public void SelectEventIllust(int index)
  {
    for (int i = 0; i < EventButton_Progress.Length; i++)
      EventButton_Progress[i].interactable = index != i;
    CurrentIndex = 0;
    switch(index)
    {
      case 0:
        setup(SelectedEvent.BeginningIllusts);
        break;
      case 1:
        switch (SelectedEvent.Selection_type)
        {
          case SelectionTypeEnum.Single:
            setup(SelectedEvent.SelectionDatas[0].SuccessData.Illusts);
            break;
          case SelectionTypeEnum.Body:
            setup(SelectedEvent.SelectionDatas[0].SuccessData.Illusts);
            break;
          case SelectionTypeEnum.Head:
            setup(SelectedEvent.SelectionDatas[0].SuccessData.Illusts);
            break;
        }
        break;
      case 2: 
        switch (SelectedEvent.Selection_type)
        {
          case SelectionTypeEnum.Single:
            setup(SelectedEvent.SelectionDatas[0].FailData.Illusts);
            break;
          case SelectionTypeEnum.Body:
            setup(SelectedEvent.SelectionDatas[0].FailData.Illusts);
            break;
          case SelectionTypeEnum.Head:
            setup(SelectedEvent.SelectionDatas[0].FailData.Illusts);
            break;
        }
        break;
      case 3:
        switch (SelectedEvent.Selection_type)
        {
          case SelectionTypeEnum.Single:
            //이거 없음
            break;
          case SelectionTypeEnum.Body:
            setup(SelectedEvent.SelectionDatas[1].SuccessData.Illusts);
            break;
          case SelectionTypeEnum.Head:
            setup(SelectedEvent.SelectionDatas[1].SuccessData.Illusts);
            break;
        }
        break;
      case 4:
        switch (SelectedEvent.Selection_type)
        {
          case SelectionTypeEnum.Single:
            //이거 없음
            break;
          case SelectionTypeEnum.Body:
            setup(SelectedEvent.SelectionDatas[1].FailData.Illusts);
            break;
          case SelectionTypeEnum.Head:
            setup(SelectedEvent.SelectionDatas[1].FailData.Illusts);
            break;
        }
        break;
    }
    void setup(EventIllustHolder[] illustholders)
    {
      SelectedIllusts = illustholders;
      if (illustholders.Length > 1)
      {
        if (!EventIllust_Arrowholder.activeSelf) EventIllust_Arrowholder.SetActive(true);
        EventIllust_Previewbutton.interactable = false;
        EventIllust_Nextbutton.interactable = true;
      }
      else
      {
        if (EventIllust_Arrowholder.activeSelf) EventIllust_Arrowholder.SetActive(false);
      }
      SetupSeasonIllust(illustholders[0]);
    }
    for(int i = 0; i < EventButton_Progress.Length; i++)
    {
      if (EventButton_Progress[i].gameObject.activeInHierarchy)
        EventButton_Progress[i].interactable = index != i;
    }
  }
  private void SetupSeasonIllust(EventIllustHolder illustholder)
  {
    if (illustholder.IsSeason)
    {
      if (!SeasonHolder.activeSelf) SeasonHolder.SetActive(true);
      SelectSeason(0);
    }
    else
    {
      if (SeasonHolder.activeSelf) SeasonHolder.SetActive(false);
      EventIllust.Next(illustholder.CurrentIllust);
    }

  }
  public void SelectSeason(int index)
  {
    EventIllust.Next(SelectedIllusts[CurrentIndex].GetSeasonIllust(index));
    for(int i=0;i<SeasonButtons.Length;i++) SeasonButtons[i].interactable = index != i;
  }
  public void EventIllust_Preview()
  {
    CurrentIndex--;
    SetupSeasonIllust(SelectedIllusts[CurrentIndex]);
    if (CurrentIndex == 0) EventIllust_Previewbutton.interactable = false;
    EventIllust_Nextbutton.interactable = true;
  }
  public void EventIllust_Next()
  {
    CurrentIndex++;
    SetupSeasonIllust(SelectedIllusts[CurrentIndex]);
    EventIllust_Previewbutton.interactable = true;
    if (SelectedIllusts.Length - 1 == CurrentIndex) EventIllust_Nextbutton.interactable = false;
  }
  #endregion
  #region 기록
  [Space(20)]
  [SerializeField] private Button FinishInfoButton = null;
  [SerializeField] private TextMeshProUGUI FinishInfoButton_text = null;
  [SerializeField] private CanvasGroup FinishInfoGroup = null;
  [SerializeField] private Color MadColor = new Color();
  [SerializeField] private List<GameObject> FinishObjs = null;
  #endregion
  public void Setup()
  {
    EndingPanelButton.interactable = false;
    EndingPanelButton_text.text = GameManager.Instance.GetTextData("EndingList");
    for (int i = 0; i < GameManager.Instance.ImageHolder.EndingList.Count; i++)
    {
      EndingData _ending = GameManager.Instance.ImageHolder.EndingList[i];
      EndingPreviews[i].EndingID = _ending.ID;

      if (GameManager.Instance.ProgressData.EndingLists.Contains(_ending.ID))
      {
        EndingPreviews[i].transform.GetChild(0).GetComponent<Image>().sprite = _ending.PreviewIcon;
      }
      else
      {
        EndingPreviews[i].transform.GetChild(0).GetComponent<Image>().sprite = GameManager.Instance.ImageHolder.UnknownExpRewardIcon;
        EndingPreviews[i].transform.GetComponent<Button>().interactable = false;
      }
    }

    EventPanelButton.interactable = true;
    EventPanelButton_text.text = GameManager.Instance.GetTextData("EventList");
    EventButton_Progress[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GameManager.Instance.GetTextData("EventProgress_Beginning");
    for(int i = 0; i < EventButton_Name.Length; i++)
    {
      if (GameManager.Instance.ProgressData.EventList.ContainsKey(GameManager.Instance.EventHolder.AllEvent[i].ID))
      {
        int _index = i;
        EventButton_Name[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GameManager.Instance.EventHolder.AllEvent[i].Name;
        EventButton_Name[i].GetComponent<Button>().onClick.AddListener(() => 
        { SelectEvent(GameManager.Instance.EventHolder.AllEvent[_index].ID); });
      }
      else
      {
        EventButton_Name[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "???";
        EventButton_Name[i].GetComponent<Button>().interactable = false;
      }
    }

    FinishInfoButton.interactable = GameManager.Instance.ProgressData.FinishDatas.Count>0;
    FinishInfoButton_text.text = GameManager.Instance.GetTextData("FinishInfoList");
    int _finishcount = GameManager.Instance.ProgressData.FinishDatas.Count;
    StringBuilder _finishstr= new StringBuilder();
    string[] _finishstrs = GameManager.Instance.GetTextData("FinishInfo").Split('@');
    for (int i = 0; i < FinishObjs.Count; i++)
    {
      if (i < _finishcount)
      {
        FinishData _data = GameManager.Instance.ProgressData.FinishDatas.Dequeue();
        Image _icon = FinishObjs[i].transform.GetChild(0).GetChild(0).GetComponent<Image>();
        switch (_data.Type)
        {
          case 0:
            _icon.sprite = GameManager.Instance.ImageHolder.HPBroken;
            break;
          case 1:
            _icon.sprite = GameManager.Instance.ImageHolder.GetSkillIcon(SkillTypeEnum.Conversation, false);
            _icon.color = MadColor;
            break;
          case 2:
            _icon.sprite = GameManager.Instance.ImageHolder.GetSkillIcon(SkillTypeEnum.Force, false);
            _icon.color = MadColor;
            break;
          case 3:
            _icon.sprite = GameManager.Instance.ImageHolder.GetSkillIcon(SkillTypeEnum.Wild, false);
            _icon.color = MadColor;
            break;
          case 4:
            _icon.sprite = GameManager.Instance.ImageHolder.GetSkillIcon(SkillTypeEnum.Intelligence, false);
            _icon.color = MadColor;
            break;
          case 5:
            _icon.sprite = GameManager.Instance.ImageHolder.MadnessActive;
            break;
          case 6:
            _icon.sprite = GameManager.Instance.ImageHolder.EndingList[0].PreviewIcon; break;
          case 7:
            _icon.sprite = GameManager.Instance.ImageHolder.EndingList[1].PreviewIcon; break;
          case 8:
            _icon.sprite = GameManager.Instance.ImageHolder.EndingList[2].PreviewIcon; break;
          case 9:
            _icon.sprite = GameManager.Instance.ImageHolder.EndingList[3].PreviewIcon; break;
          case 10:
            _icon.sprite = GameManager.Instance.ImageHolder.EndingList[4].PreviewIcon; break;
          case 11:
            _icon.sprite = GameManager.Instance.ImageHolder.EndingList[5].PreviewIcon; break;
          case 12:
            _icon.sprite = GameManager.Instance.ImageHolder.EndingList[6].PreviewIcon; break;
        }

        _finishstr.Append(string.Format(GameManager.Instance.GetTextData("FinishInfo_year"), _data.Year));
        _finishstr.Append(_finishstrs[_data.Type]);
        FinishObjs[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text=_finishstr.ToString();

        if (_data.Head_Left == -1)
        {
          FinishObjs[i].transform.GetChild(2).gameObject.SetActive(false);
        }
        else
        {
          FinishObjs[i].transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = _data.Body_Left.ToString();
          FinishObjs[i].transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = _data.Body_Right.ToString();
          FinishObjs[i].transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = _data.Head_Left.ToString();
          FinishObjs[i].transform.GetChild(2).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = _data.Head_Right.ToString();
        }
        _finishstr.Length = 0;
        GameManager.Instance.ProgressData.AddFinishData(_data);
      }
      else
      {
        FinishObjs[i].SetActive(false);
      }
    }
  }
  public void Select_Ending()
  {
    EndingPanelButton.interactable = false;
    EndingGroup.alpha = 1.0f;
    EndingGroup.interactable = true;
    EndingGroup.blocksRaycasts = true;

    EventPanelButton.interactable = true;
    EventGroup.alpha = 0.0f;
    EventGroup.interactable = false;
    EventGroup.blocksRaycasts = false;
    EventIllust.StopAllCoroutines();
    EndingIllust.SetTransparent();
    EndingIllustObj.SetActive(false);
    EndingArrowHolder.SetActive(false);
    SelectedEnding = null;

    FinishInfoButton.interactable = GameManager.Instance.ProgressData.FinishDatas.Count > 0;
    FinishInfoGroup.alpha = 0.0f;
    FinishInfoGroup.interactable = false;
    FinishInfoGroup.blocksRaycasts = false;
  }
  public void Select_Event()
  {
    EndingPanelButton.interactable = true;
    EndingGroup.alpha = 0.0f;
    EndingGroup.interactable = false;
    EndingGroup.blocksRaycasts = false;

    EventPanelButton.interactable = false;
    EventGroup.alpha = 1.0f;
    EventGroup.interactable = true;
    EventGroup.blocksRaycasts = true;
    if (EventProgressHolder.activeSelf) EventProgressHolder.SetActive(false);
    EventIllust.SetTransparent();
    EndingIllust.StopAllCoroutines();
    SelectedEvent = null;

    FinishInfoButton.interactable = GameManager.Instance.ProgressData.FinishDatas.Count > 0;
    FinishInfoGroup.alpha = 0.0f;
    FinishInfoGroup.interactable = false;
    FinishInfoGroup.blocksRaycasts = false;
  }
  public void Select_Log()
  {
    if (GameManager.Instance.ProgressData.FinishDatas.Count == 0) return;
    EndingPanelButton.interactable = true;
    EndingGroup.alpha = 0.0f;
    EndingGroup.interactable = false;
    EndingGroup.blocksRaycasts = false;

    EventPanelButton.interactable = true;
    EventGroup.alpha = 0.0f;
    EventGroup.interactable = false;
    EventGroup.blocksRaycasts = false;
    EventIllust.StopAllCoroutines();
    EndingIllust.SetTransparent();
    EndingIllustObj.SetActive(false);
    EndingArrowHolder.SetActive(false);
    SelectedEnding = null;

    FinishInfoButton.interactable = false;
    FinishInfoGroup.alpha = 1.0f;
    FinishInfoGroup.interactable = true;
    FinishInfoGroup.blocksRaycasts = true;
  }
}
