using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoginClient;
using TMPro;

public class LoginSignup : MonoBehaviour
{
    [SerializeField] TMP_InputField username;
    [SerializeField] TMP_InputField password;

    [SerializeField] TextMeshProUGUI logsTxt;

    [SerializeField] string ip;

    private void Awake()
    {
        LoginSystem.Ip = ip;
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
        KeyValuePair<bool, string> keyValue = LoginSystem.AttemptLogin(username.text, password.text);
        logsTxt.SetText(keyValue.Value);
    }
    public void SignupBtn()
    {
        KeyValuePair<bool, string> keyValue = LoginSystem.AttemptSignup(username.text, password.text);
        logsTxt.SetText(keyValue.Value);
    }
}
