using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HttpCommunicator : MonoBehaviour
{
    /*
    //local
    private const string host = "localhost"; //remove const after implementing setter
    private const int port = 3000; //remove const after implementing setter
    private const string protocol = "http://";
    */


    // you can see score online
    // https://overlays-webapp.herokuapp.com/scores
    //
    // Server will be waken up if being accessed. 
    // If thee are no accesses, server fall in sleep again after 30 minutes.
    //


    private const string host = "https://overlays-webapp.herokuapp.com"; //remove const after implementing setter
    private const int port = 443; //remove const after implementing setter
    //private const string protocol = "https://";
    //private const string protocol = "https";

    private const string path = "/scores";
    private const string header = "Content-Type";
    private const string contentType = "application/json";

    //private  const int score = 3098; //remove const and default value after implementing constructor or method
    //private  const string player = "p3098"; //remove const and default value after implementing constructor or method

    private int score;
    private string player;


    void Start () {	
	}
	
	void Update () {
	}

    public HttpCommunicator(string player, int score)
    {
        //this.player = player;
        //this.score = score;
    }

    /*
    public HttpCommunicator(string url)
    {
        //this.url = url;
    }

    public HttpCommunicator()
    {
    }
    */

    public void SendScoreToServer(string player, int score)
    {      
        this.player = player;
        this.score = score;

        ConnectionStart();

    }


    public void ConnectionStart()
    {
        StartCoroutine(ConnectionJSON());
    }


    private IEnumerator ConnectionJSON()
    {
        /*
        string host = "localhost";
        int port = 3000;

        string protocol = "http://";
        string path = "/scores";
        string url = protocol + host + ":" + port + path;

        string header = "Content-Type";
        string contentType = "application/json";

        string score = "3090";
        string player = "p3090";
        */

        //string url00 = protocol + host + ":" + port + path;

        //string url = protocol + "://" + host + ":" + port + path;

        string url = host + ":" + port + path;


        //sorry, ignoreance of json in unity
        string scoreDataJson = " {\"score\" : {\"score\": \"" + score + "\", \"player\": \"" + player + "\"}} ";
        //Debug.Log(testJson);

        //byte[] payload = new byte[1024];
        byte[] payload = new byte[scoreDataJson.Length];
        payload = System.Text.Encoding.UTF8.GetBytes(scoreDataJson);


        UnityWebRequest request = new UnityWebRequest(url);
        request.timeout = 60;
        request.method = UnityWebRequest.kHttpVerbPOST;
        request.SetRequestHeader(header, contentType);
        request.uploadHandler = new UploadHandlerRaw(payload); ;


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


    //old version, not being updated more 
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


}
