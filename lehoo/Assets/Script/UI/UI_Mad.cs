using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Mad : UI_default
{
  [SerializeField] private float MoveTime = 2.5f;
  [SerializeField] private Vector2 DownPos = new Vector2(0.0f, -920.0f);
  [SerializeField] private CanvasGroup BlockGroup = null;
  [SerializeField] private Image Illust = null;
  [SerializeField] private TextMeshProUGUI Description = null;
  [SerializeField] private Button YesButton = null;
  [SerializeField] private TextMeshProUGUI YesButtonText = null;
  [SerializeField] private Button NoButton = null;
  [SerializeField] private TextMeshProUGUI NoButtonText = null;

  private Experience CurrentExp = null;
  public void OpenUI(Experience madexp)
  {
    IsOpen = true;

    CurrentExp = madexp;
    DefaultGroup.interactable = true;
    BlockGroup.blocksRaycasts = true;

    Illust.sprite = madexp.Illust;
    Description.text = GameManager.Instance.GetTextData(madexp.ID+ "_GENERATEDESCRIPTION") + "<br><br>" + GameManager.Instance.GetTextData("YOULOSEMAXIMUNSANITY");
   
    YesButtonText.text = (GameManager.Instance.GetTextData("ACCEPTMADNESS"));
    NoButtonText.text = GameManager.Instance.GetTextData("REFUSEMADNESS");

    StartCoroutine(changealpha(true));
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("myrect").Rect, GetPanelRect("myrect").OutisdePos, GetPanelRect("myrect").InsidePos, MoveTime / 2.0f, UIManager.Instance.UIPanelOpenCurve));

  }
  public override void CloseForGameover()
  {
    IsOpen = false;
    StartCoroutine(changealpha(false));
    DefaultGroup.interactable = false;
    UIManager.Instance.moverect(GetPanelRect("myrect").Rect, GetPanelRect("myrect").Rect.anchoredPosition, DownPos, MoveTime / 2.0f, UIManager.Instance.UIPanelCLoseCurve);
  }
public override void CloseUI()
  {
    IsOpen = false;
    StartCoroutine(changealpha(false));
    DefaultGroup.interactable = false;
    UIManager.Instance.AddUIQueue(UIManager.Instance.moverect(GetPanelRect("myrect").Rect, GetPanelRect("myrect").InsidePos, DownPos, MoveTime / 2.0f, UIManager.Instance.UIPanelCLoseCurve));
  }
  private IEnumerator changealpha(bool open)
  {
    float _time = 0.0f;
    float _startalpha = open ? 0.0f : 1.0f;
    float _endalpha = open ? 1.0f : 0.0f;
    while(_time< MoveTime)
    {
      BlockGroup.alpha = Mathf.Lerp(_startalpha, _endalpha, _time / MoveTime);
      _time += Time.deltaTime;
      yield return null;
    }
    BlockGroup.alpha = _endalpha;
  }

  public void AccepMadness()
  {
    if (UIManager.Instance.IsWorking) return;
    UIManager.Instance.ExpRewardUI.OpenUI_Madness(CurrentExp);
  }
  public void RefuseMadness()
  {
    if (UIManager.Instance.IsWorking) return;

    GameManager.Instance.MyGameData.HP -= ConstValues.MaddnesRefuseHPCost;
    GameManager.Instance.MyGameData.CurrentSanity = ConstValues.MadnessRefuseSanityRestore;

    UIManager.Instance.UpdateHPText();
    UIManager.Instance.UpdateSanityText();

    CloseUI();
  }
}
