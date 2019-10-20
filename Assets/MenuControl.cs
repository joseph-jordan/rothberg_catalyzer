using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Net.Http;
using System.Text;



public class MenuControl : MonoBehaviour
{
    private string email;
    private string password;
    private static readonly HttpClient client = new HttpClient();
    private const string URL = "https://us-central1-rothberg-catalyzer-2019.cloudfunctions.net/authenticate";
    public static string userID;

    private void advance() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SetEmail(string newEmail) {
        email = newEmail;
    }

    public void SetPassword(string newPassword) {
        password = newPassword;
    }


    public void OnSubmit() {
        //if the login is valid, store stuff, advance
        WWWForm request = new WWWForm();
        string jsonString = "{\"email\": \"" + email + "\",\"password\": \"" + password + "\"}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString);
        var download = UnityWebRequest.Post(URL, request);
        UploadHandler handler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        handler.contentType = "application/json";
        download.uploadHandler = handler;//(UploadHandler) new UploadHandler(bodyRaw);
        download.SetRequestHeader("Content-Type", "application/json");
        download.SendWebRequest();
        while(!download.isDone)
        {

        }
        print("just finished");
        if (download.isNetworkError || download.isHttpError)
        {
            print("Error downloading: " + download.error);
            print(download.downloadHandler.text);
        }
        else
        {
           userID = download.downloadHandler.text;
           print("userID: " + userID);
           advance();
        }
    }
}