using Steamworks;
using Stove.PCSDK.NET;
using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class StovePCSDKManager : MonoBehaviour
{
    // Setting value filled through 'LoadConfig'
  [SerializeField] private string Env;
  [SerializeField] private string AppKey;
  [SerializeField] private string AppSecret;
  [SerializeField] private string GameId;
  [SerializeField] private StovePCLogLevel LogLevel;
  [SerializeField] private string LogPath;

  private StovePCCallback callback;
    private Coroutine runcallbackCoroutine;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);

    StovePCResult sdkResult = StovePCResult.NoError;

    StovePCConfig config = new StovePCConfig
    {
      Env = this.Env,
      AppKey = this.AppKey,
      AppSecret = this.AppSecret,
      GameId = this.GameId,
      LogLevel = this.LogLevel,
      LogPath = this.LogPath
    };

    this.callback = new StovePCCallback
    {
      OnError = new StovePCErrorDelegate(this.OnError),
      OnInitializationComplete = new StovePCInitializationCompleteDelegate(this.OnInitializationComplete),
      OnToken = new StovePCTokenDelegate(this.OnToken),
      OnUser = new StovePCUserDelegate(this.OnUser)
    };

    sdkResult = StovePC.Initialize(config, callback);
    WriteLog("Initialize", sdkResult);

    sdkResult = StovePC.GetUser();
    WriteLog("UserInfo", sdkResult);

    sdkResult = StovePC.GetToken();
    WriteLog("TokenInfo", sdkResult);
  }
#if UNITY_EDITOR
  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.PageUp))
      ButtonGetUser_Click();
  }
#endif
  private void OnDestroy()
    {
        if (runcallbackCoroutine != null)
        {
            StopCoroutine(runcallbackCoroutine);
            runcallbackCoroutine = null;
        }

        StovePC.Uninitialize();
    Debug.Log("스토브가 종료되는레후?");
    }

    #region Event Handlers
    public void ButtonLoadConfig_Click()
    {
        string configFilePath = Application.streamingAssetsPath + "/Text/StovePCConfig.Unity.txt";

        if (File.Exists(configFilePath))
        {
            string configText = File.ReadAllText(configFilePath);
            StovePCConfig config = JsonUtility.FromJson<StovePCConfig>(configText);

            this.Env = config.Env;
            this.AppKey = config.AppKey;
            this.AppSecret = config.AppSecret;
            this.GameId = config.GameId;
            this.LogLevel = config.LogLevel;
            this.LogPath = config.LogPath;

            WriteLog(configText);
        }
        else
        {
            string msg = String.Format("File not found : {0}", configFilePath);
            WriteLog(msg);
        }
    }

    public void ToggleRunCallback_ValueChanged(bool isOn)
    {
        if (isOn)
        {
            float intervalSeconds = 1f;
            runcallbackCoroutine = StartCoroutine(RunCallback(intervalSeconds));

            WriteLog("RunCallback Start");
        }
        else
        {
            if (runcallbackCoroutine != null)
            {
                StopCoroutine(runcallbackCoroutine);
                runcallbackCoroutine = null;

                WriteLog("RunCallback Stop");
            }
        }
    }
    #endregion


    #region Coroutine
    private IEnumerator RunCallback(float intervalSeconds)
    {
        WaitForSeconds wfs = new WaitForSeconds(intervalSeconds);
        while (true)
        {
            StovePC.RunCallback();
            yield return wfs;
        }
    }
    #endregion


    #region Methods
    private void WriteLog(string functionName, StovePCResult result)
    {
        if (String.IsNullOrEmpty(functionName))
            functionName = "Unknown";

        string msg = String.Format("{0} Success", functionName);
        if (result != StovePCResult.NoError)
        {
            msg = String.Format("{0} Fail : {1}", functionName, result.ToString());
        }

        Debug.Log(msg + Environment.NewLine);

      //  AppendUILog(msg);
    }
    private void WriteLog(string log)
    {
        Debug.Log(log + Environment.NewLine);

        AppendUILog(log);
    }

    private void AppendUILog(string log)
    {
        GameObject content = GameObject.Find("ContentLog");
        GameObject textLog = content.transform.GetChild(0).gameObject;
        GameObject copyLog = Instantiate<GameObject>(textLog, content.transform);
        var copyTextComponent = copyLog.GetComponent<Text>();
        copyTextComponent.text = log;
    }

    private void ToggleRunCallback(bool isOn)
    {
        GameObject toggleRunCallback = GameObject.Find("ToggleRunCallback");
        Toggle toggleComponent = toggleRunCallback.GetComponent<Toggle>();
        toggleComponent.isOn = isOn;
    }
    #endregion


    #region SDK Callback Methods
    private void OnError(StovePCError error)
    {
    Debug.Log("뭔가 실패한 레뺫!");

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("OnError");
        sb.AppendFormat(" - error.FunctionType : {0}" + Environment.NewLine, error.FunctionType.ToString());
        sb.AppendFormat(" - error.Result : {0}" + Environment.NewLine, error.Result.ToString());
        sb.AppendFormat(" - error.Message : {0}" + Environment.NewLine, error.Message);
        sb.AppendFormat(" - error.ExternalError : {0}", error.ExternalError.ToString());

        WriteLog(sb.ToString());

    Application.Quit();
    }
    #endregion


    #region SDK Initialization
    public void ButtonInitialize_Click()
    {
    StovePCResult sdkResult = StovePCResult.NoError;

    StovePCConfig config = new StovePCConfig
    {
      Env = this.Env,
      AppKey = this.AppKey,
      AppSecret = this.AppSecret,
      GameId = this.GameId,
      LogLevel = this.LogLevel,
      LogPath = this.LogPath
    };

    this.callback = new StovePCCallback
    {
      OnError = new StovePCErrorDelegate(this.OnError),
      OnInitializationComplete = new StovePCInitializationCompleteDelegate(this.OnInitializationComplete),
      OnToken = new StovePCTokenDelegate(this.OnToken),
      OnUser = new StovePCUserDelegate(this.OnUser)
    };

    sdkResult = StovePC.Initialize(config, callback);
    WriteLog("Initialize", sdkResult);
    }

    private void OnInitializationComplete()
    {
    Debug.Log("스토브 로그인에 성공한 레후~");
    }
    #endregion


    #region SDK Termination
    public void ButtonUninitialize_Click()
    {
        ToggleRunCallback(false);

        StovePCResult sdkResult = StovePCResult.NoError;

        // Todo: Write your code here.

        WriteLog("Uninitialize", sdkResult);
    }
    #endregion


    #region Acquiring User Information
    public void ButtonGetUser_Click()
    {
        StovePCResult sdkResult = StovePCResult.NoError;

    // Todo: Write your code here.
    sdkResult = StovePC.GetUser();

    WriteLog("GetUser", sdkResult);
    }

    private void OnUser(StovePCUser user)
    {
    Debug.Log("유저 정보를 불러와보는 레후~");
        StringBuilder sb = new StringBuilder();

    sb.AppendLine("OnUser");
    sb.AppendFormat(" - user.MemberNo : {0}" + Environment.NewLine, user.MemberNo.ToString());
    sb.AppendFormat(" - user.Nickname : {0}" + Environment.NewLine, user.Nickname);
    sb.AppendFormat(" - user.GameUserId : {0}", user.GameUserId);

    WriteLog(sb.ToString());
    }
    #endregion


    #region Acquiring Token Information
    public void ButtonGetToken_Click()
    {
        StovePCResult sdkResult = StovePCResult.NoError;

        // Todo: Write your code here.

        WriteLog("GetToken", sdkResult);
    }

    private void OnToken(StovePCToken token)
    {
        StringBuilder sb = new StringBuilder();

    // Todo: Write your code here.
    sb.AppendLine("OnToken");
    sb.AppendFormat(" - token.AccessToken : {0}", token.AccessToken);

    WriteLog(sb.ToString());
    }
    #endregion
}
