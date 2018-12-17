using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HttpCommunicator : MonoBehaviour
{ //startcoroutinを使うためには、monobehaiviourを継承している必要があるようだ。
  //その他には　ScriptableObject などがあるようだ。


    void Start () {	
	}
	
	void Update () {
	}

    //private  string url = "localhost:3000";
    //private string url;
    public HttpCommunicator(string url)
    {
        //this.url = url;
    }

    public HttpCommunicator()
    {
    }


    public void ConnectionStart()
    {
        //StartCoroutine(ConnectionWebForm());
        StartCoroutine(ConnectionJSON());
    }

    private IEnumerator ConnectionWebForm()
    {

        string player = "p3050";
        string score = "3050";

        WWWForm form = new WWWForm();
        form.AddField("score[score]", score);
        form.AddField("score[player]", player);

        UnityWebRequest request = UnityWebRequest.Post("http://localhost:3000/scores", form);


        // send request
        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {
            Debug.Log("NETWORK ERROR:" + request.error);
        }
        else
        {
            Debug.Log("Response Code: " + request.responseCode);
        }
            
    }


    private IEnumerator ConnectionJSON()
    {
        string url = "http://localhost:3000/scores";
        string header = "Content-Type";
        string contentType = "application/json";
        string score = "3070";
        string player = "p3070";

        //sorry, ignoreance of json in unity
        string testJson = " {\"score\" : {\"score\": \"" +score+ "\", \"player\": \"" +player+ "\"}} ";

        byte[] payload = new byte[1024];
        payload = System.Text.Encoding.UTF8.GetBytes(testJson);


        UnityWebRequest request = new UnityWebRequest(url);
        request.timeout = 60;
        request.method = UnityWebRequest.kHttpVerbPOST;
        request.SetRequestHeader(header, contentType);
        request.uploadHandler = new UploadHandlerRaw(payload);;


        // send request
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("ERROR:" + request.error);
        }
        else
        {
            Debug.Log("Response Code: " + request.responseCode);
        }

    }

}
