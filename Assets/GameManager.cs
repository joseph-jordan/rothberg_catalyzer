using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Net.Http;
using System.Text;

public class GameManager : MonoBehaviour {

public static int PlayerScore1 = 0;
public static int PlayerScore2 = 0;

public GUISkin layout;

GameObject theBall;

// Use this for initialization
void Start () {
theBall = GameObject.FindGameObjectWithTag ("Ball");
}

public static void Score(string wallID) {
if (wallID == "RightWall") {
PlayerScore1++;
} else {
PlayerScore2++;
}
}

void OnGUI() {
GUI.skin = layout;
GUI.Label (new Rect (Screen.width / 2 - 150 - 12, 20, 100, 100), "" + PlayerScore1);
GUI.Label (new Rect (Screen.width / 2 + 150 + 12, 20, 100, 100), "" + PlayerScore2);

if (GUI.Button (new Rect (Screen.width / 2 - 60, 35, 120, 53), "RESTART")) {
            //only log results when someone has not won in this case, and if numHits is not zero
            if (PlayerScore1 != 10 && PlayerScore2 != 10 && PlayerControls.totalHits != 0)
            {
                logGameResult();
            }
            //resetting done here only
            PlayerScore1 = 0;
PlayerScore2 = 0;
            PlayerControls.totalHits = 0;
            PlayerControls.globalMinAngle = float.MaxValue;
            PlayerControls.globalMaxAngle = float.MinValue;
			PlayerControls.updatedAngles = false;
            theBall.SendMessage ("RestartGame", 0.5f, SendMessageOptions.RequireReceiver);
}

if (PlayerScore1 == 10) {
            logGameResult();
            GUI.Label (new Rect (Screen.width / 2 - 150, 200, 2000, 1000), "PLAYER ONE WINS");
theBall.SendMessage ("ResetBall", null, SendMessageOptions.RequireReceiver);
} else if (PlayerScore2 == 10) {
            logGameResult();
            GUI.Label (new Rect (Screen.width / 2 - 150, 200, 2000, 1000), "PLAYER TWO WINS");
theBall.SendMessage ("ResetBall", null, SendMessageOptions.RequireReceiver);
}
}

    void logGameResult()
    {
		if (!PlayerControls.updatedAngles) {
			return;
		}
        //if the login is valid, store stuff, advance
        WWWForm request = new WWWForm();
        string jsonString = "{\"uid\": \"" + MenuControl.userID + "\",\"userScore\": " + PlayerScore1 + ",\"aiScore\": " + PlayerScore2 +
            ",\"maxAngle\": " + PlayerControls.globalMaxAngle +  ",\"minAngle\": " + PlayerControls.globalMinAngle + ",\"totalHits\": " + PlayerControls.totalHits + "}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString);
        var download = UnityWebRequest.Post("https://us-central1-rothberg-catalyzer-2019.cloudfunctions.net/logGameResult", request);
        UploadHandler handler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        handler.contentType = "application/json";
        download.uploadHandler = handler;//(UploadHandler) new UploadHandler(bodyRaw);
        download.SetRequestHeader("Content-Type", "application/json");
        download.SendWebRequest();
        while (!download.isDone)
        {

        }
        if (download.isNetworkError || download.isHttpError)
        {
            print("Error logging results: " + download.error);
            print(download.downloadHandler.text);
        }
    }

}