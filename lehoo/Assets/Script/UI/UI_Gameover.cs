using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Gameover : UI_default
{
  [SerializeField] private Image Illust = null;
  [SerializeField] private AnimationCurve IllustOpenCurve = null;
  [SerializeField] private float IllustOpenTime = 4.0f;
  [SerializeField] private TextMeshProUGUI Description = null;
  [SerializeField] private CanvasGroup ButtonGroup = null;
  [SerializeField] private TextMeshProUGUI ButtonText = null;
  [SerializeField] private float CloseTime = 2.5f;
  public void OpenUI(GameOverTypeEnum gameovertype)
  {
    string _name = "GameOver";
    switch (gameovertype)
    {
      case GameOverTypeEnum.HP:
        _name += "_HP";
        break;
      case GameOverTypeEnum.Sanity:
        _name += "_Sanity";
        break;
    }

    EnvironmentType _envir = EnvironmentType.NULL;
    if (GameManager.Instance.MyGameData.CurrentSettlement != null)
    {
      _name += "_Settlement";
      _envir = GameManager.Instance.MyGameData.CurrentSettlement.TileInfoData.EnvirList[Random.Range(0, GameManager.Instance.MyGameData.CurrentSettlement.TileInfoData.EnvirList.Count)];
    }
    else
    {
      _name += "Outer";
      if (GameManager.Instance.MyGameData.CurrentEvent != null && GameManager.Instance.MyGameData.CurrentEvent.EnvironmentType != EnvironmentType.NULL)
      {
        _envir = GameManager.Instance.MyGameData.CurrentEvent.EnvironmentType;
      }
      else
      {
        _envir = GameManager.Instance.MyGameData.MyMapData.GetTileData(GameManager.Instance.MyGameData.Coordinate).EnvirList[Random.Range(0, GameManager.Instance.MyGameData.MyMapData.GetTileData(GameManager.Instance.MyGameData.Coordinate).EnvirList.Count)];
      }
    }

    _name += EnvirText(_envir);
    Illust.sprite = GameManager.Instance.ImageHolder.GetGameoverIllust(_name);
    Description.text = GameManager.Instance.GetTextData(_name);

    ButtonGroup.alpha = 0.0f;
    ButtonGroup.interactable = false;
    ButtonText.text = GameManager.Instance.GetTextData("QUITTOMAIN");

    DefaultGroup.interactable = true;
    DefaultGroup.blocksRaycasts = true;
    StartCoroutine(changealpha(true));

    UIManager.Instance.StartCoroutine(openui());
  }
  public string EnvirText(EnvironmentType type)
  {
    switch (type)
    {
      case EnvironmentType.Beach: return "_Beach";
      case EnvironmentType.Land: return "_Land";
      case EnvironmentType.River: return "_River";
      case EnvironmentType.Sea: return "_Sea";
      case EnvironmentType.RiverBeach: return "_RiverBeach";
      case EnvironmentType.Forest: return "_Forest";
      case EnvironmentType.Mountain: return "_Mountain";
      case EnvironmentType.Highland: return "_Highland";
    }
    return "·¹ÈÄ";
  }

  private IEnumerator openui()
  {

    yield return StartCoroutine(UIManager.Instance.moverect(GetPanelRect("illust").Rect, GetPanelRect("illust").OutisdePos, GetPanelRect("illust").InsidePos, IllustOpenTime, IllustOpenCurve));

    yield return new WaitForSeconds(3.0f);
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("description").Rect, GetPanelRect("description").OutisdePos, GetPanelRect("description").InsidePos,1.5f,UIManager.Instance.UIPanelOpenCurve));
    yield return new WaitForSeconds(2.0f);
    StartCoroutine(UIManager.Instance.ChangeAlpha(ButtonGroup, 1.0f, 1.0f));
  }

  public void GoToMain()
  {
    if (UIManager.Instance.IsWorking) return;
    StartCoroutine(changealpha(false));
    DefaultGroup.interactable = false;
    DefaultGroup.blocksRaycasts = false;
    UIManager.Instance.AddUIQueue(closeui());
  }
  private IEnumerator closeui()
  {
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("illust").Rect, GetPanelRect("illust").InsidePos, GetPanelRect("illust").OutisdePos, CloseTime, UIManager.Instance.UIPanelCLoseCurve));
   yield return StartCoroutine(UIManager.Instance.moverect(GetPanelRect("description").Rect, GetPanelRect("description").InsidePos, GetPanelRect("description").OutisdePos, CloseTime, UIManager.Instance.UIPanelCLoseCurve));

    UIManager.Instance.MainUI.SetupMain();
  }
  private IEnumerator changealpha(bool isopen)
  {
    float _time = 0.0f;
    float _targettime = 1.0f;
    float _start = isopen ? 0.0f : 1.0f;
    float _end=isopen ? 1.0f : 0.0f;
    while(_time < _targettime)
    {
      DefaultGroup.alpha = Mathf.Lerp(_start, _end, _time / _targettime);
      _time += Time.deltaTime;
      yield return null;
    }
    DefaultGroup.alpha = _end;
  }
}
