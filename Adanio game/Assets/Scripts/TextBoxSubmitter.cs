using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxSubmitter : MonoBehaviour
{
    [SerializeField] RectTransform textUiItem;
    [SerializeField] RectTransform chatLogParrent;
    [SerializeField] TMPro.TMP_InputField inputField;

    private void Start()
    {
        inputField.onSubmit.AddListener(val => PostMessage());
    }

    public void PostMessage()
    {
        if (inputField.text != "") {

            string[] input = inputField.text.ToString().Split(' ');

            switch (input[0])
            {
                case "/help":
                    Instantiate(textUiItem, chatLogParrent.transform).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = 
                        "*Commands*:" + "\n" +
                        "/pm \"playerName\" \"message\"" + "\n" +
                        "\"message\"" + "\n" +
                        "/group \"PlayerName1:PlayerName2\" \"message\"";

                    inputField.text = "";
                    break;
                case "/pm":
                    Instantiate(textUiItem, chatLogParrent.transform).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = input[1] + " => "  + inputField.text.Remove(0,3 + input[1].Length + 1);
                    FindObjectOfType<ChatClient>().Send("MSG",input[1], inputField.text.Remove(0, 3 + input[1].Length + 1));
                    inputField.text = "";

                    break;
                case "/group":
                    TMPro.TextMeshProUGUI go = Instantiate(textUiItem, chatLogParrent.transform).GetComponentInChildren<TMPro.TextMeshProUGUI>();
                    go.text = inputField.text.Remove(0, 6 + input[1].Length + 1);
                    go.color = Color.green;
                    FindObjectOfType<ChatClient>().Send("MSGROUP", input[1], inputField.text.Remove(0, 6 + input[1].Length + 1));
                    inputField.text = "";
                    break;
                default:
                    Instantiate(textUiItem, chatLogParrent.transform).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = inputField.text;
                    FindObjectOfType<ChatClient>().Send("MSGALL", "null", inputField.text);
                    inputField.text = "";
                    break;
            }
            //buffer item
            TextTerminationTimer t = Instantiate(textUiItem, chatLogParrent.transform).GetComponent<TextTerminationTimer>();
            t.lifeTime = 0;
            t.readTime = 0;


        }
    }

    public void IncomingMessage(string output)
    {
        Instantiate(textUiItem, chatLogParrent.transform).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = output;

        TextTerminationTimer t = Instantiate(textUiItem, chatLogParrent.transform).GetComponent<TextTerminationTimer>();
        t.lifeTime = 0;
        t.readTime = 0;
    }

}
