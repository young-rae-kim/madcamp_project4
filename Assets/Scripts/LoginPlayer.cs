using GameSparks.Api.Requests;
using GameSparks.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginPlayer : MonoBehaviour
{
    public static LoginPlayer Instance { set; get; }

    public InputField userIDInput, passwordInput;
    public GameObject loginMessage;

    public void LoginPlayerButton()
    {
        if (userIDInput.text != "" && passwordInput.text != "")
        {
            new GameSparks.Api.Requests.AuthenticationRequest()
                .SetUserName(userIDInput.text)
                .SetPassword(passwordInput.text)
                .Send((response) =>
                {
                    if (!response.HasErrors)
                    {
                        Debug.Log("Login succeeded.");
                        new LogEventRequest().SetEventKey("LOAD_PLAYER")
                        .Send((res) =>
                        {
                            GSData data = res.ScriptData.GetGSData("player_data");
                            GameSparksManager.instance.userID = data.GetString("playerID");
                            GameSparksManager.instance.userName = data.GetString("playerName");
                            GameSparksManager.instance.userEXP = int.Parse(data.GetInt("playerEXP").ToString());
                            GameSparksManager.instance.userGold = int.Parse(data.GetInt("playerGold").ToString());

                            Debug.Log("Player ID: " + data.GetString("playerID"));
                            Debug.Log("Player Name: " + data.GetString("playerName"));
                            Debug.Log("Player EXP: " + data.GetInt("playerEXP"));
                            Debug.Log("Player Gold: " + data.GetInt("playerGold"));
                            SceneManager.LoadScene("Menu");
                        });
                    }
                    else
                    {
                        Debug.Log("Login failed. : " + response.Errors.JSON.ToString());
                        loginMessage.SetActive(true);
                        GameObject.Find("LoginMessage").GetComponentInChildren<Text>().text = "Login failed. : " + response.Errors.JSON.ToString();
                        Invoke("closeMessage", 3);
                    }
                });
        }
        else
        {
            loginMessage.SetActive(true);
            GameObject.Find("LoginMessage").GetComponentInChildren<Text>().text = "Login failed. Please fill out all fields.";
            Invoke("closeMessage", 3);
        }
    }

    public void closeMessage()
    {
        loginMessage.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
