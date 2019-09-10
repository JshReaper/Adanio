using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Text;

public class FileShare : MonoBehaviour
{
    [SerializeField] int id;
    [SerializeField] GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitJson());
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
    IEnumerator InitJson()
    {
        string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\My Games\\Adanio";
        string filename = "data.json";
        List<PlayerData> data = new List<PlayerData>();

        string file = Encoding.Default.GetString(System.IO.File.ReadAllBytes(path +"\\"+ filename));

       // data.AddRange(JsonConvert.DeserializeObject<List<PlayerData>>(file));

        PlayerData pd = new PlayerData(id, player.transform.position, player.transform.localScale);
        data.Add(pd);
        string j = JsonConvert.SerializeObject(data, Formatting.Indented);
        byte[] ba = Encoding.ASCII.GetBytes(j);
        if (System.IO.File.Exists(path + "\\" + filename))
        {
            using (System.IO.FileStream fs = System.IO.File.Open(path + "\\" + filename, System.IO.FileMode.Open))
            {
                for (int i = 0; i < ba.Length; i++)
                {
                    fs.WriteByte(ba[i]);
                }
                Debug.Log("done writing");
            }
        }
        else
        {
            System.IO.Directory.CreateDirectory(path);
            using (System.IO.FileStream fs = System.IO.File.Open(path + "\\" + filename, System.IO.FileMode.Create))
            {
                for (int i = 0; i < ba.Length; i++)
                {
                    fs.WriteByte(ba[i]);
                }
                Debug.Log("done writing");
            }
        }
        yield return null;
    }
}
