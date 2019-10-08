using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoginClient;
using TMPro;
using PatchManager;
using System.Threading.Tasks;
public class LoginSignup : MonoBehaviour
{
    [SerializeField] TMP_InputField username;
    [SerializeField] TMP_InputField password;
    [SerializeField] TMP_InputField githubuser;
    [SerializeField] TMP_InputField githubpass;

    [SerializeField] TextMeshProUGUI logsTxt;

    [SerializeField] TMP_InputField ip;
    ReleaseChecker releaseChecker;
   
    public void OnIpChange()
    {
        LoginSystem.Ip = ip.text;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void LoginBtn()
    {
        try
        {
            releaseChecker = new ReleaseChecker("Adanio", "JshReaper", "Adanio", githubuser.text, githubpass.text);
            KeyValuePair<bool, string> keyValue = LoginSystem.AttemptLogin(username.text, password.text);
            if (keyValue.Key)
            {
                Task t = CheckForRelease(keyValue);
            }
            else
            {
                logsTxt.SetText(keyValue.Value);
            }
        }
        catch (System.Exception e)
        {
            logsTxt.SetText(e.Message);
        }
    }
    bool download = false;
    async Task CheckForRelease(KeyValuePair<bool, string> keyValue)
    {

        Task<bool> t = releaseChecker.CheckIfNewRelease();

        await t;
        if (t.IsCompleted)
        {
            if (t.Result)
            {
                logsTxt.SetText(keyValue.Value + "\nThere is a new version of the game!");
                download = true;
                Application.Quit();
            }
            else
            {
                logsTxt.SetText(keyValue.Value + "\nYou have the newest version of the game!");
            }
        }

    }
    public void SignupBtn()
    {
        KeyValuePair<bool, string> keyValue = LoginSystem.AttemptSignup(username.text, password.text);
        logsTxt.SetText(keyValue.Value);
    }
    private void OnApplicationQuit()
    {
#if UNITY_STANDALONE_WIN
        if (download)
        {
            System.Diagnostics.Process.Start(releaseChecker.path + "\\PatchManager.exe");
        }
#endif
    }
}
