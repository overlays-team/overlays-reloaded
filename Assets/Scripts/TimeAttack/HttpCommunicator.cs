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
    // If thee are no accesses, server fall in sleep after 30 minutes.
    //


    private const string host = "https://overlays-webapp.herokuapp.com"; 
    private const int port = 443; 
    private const string path = "/scores";
    private const string header = "Content-Type";
    private const string contentType = "application/json";

    private int score;
    private string player;


    void Start () {	
	}
	
	void Update () {
	}


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
        string url = host + ":" + port + path;

        //sorry, ignoreance of json in unity
        string scoreDataJson = " {\"score\" : {\"score\": \"" + score + "\", \"player\": \"" + player + "\"}} ";
        //Debug.Log(testJson);

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

}
