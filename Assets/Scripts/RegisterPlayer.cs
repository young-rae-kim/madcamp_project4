using GameSparks.Api.Requests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPlayer : MonoBehaviour
{
    public InputField displayNameInput, userIDInput, passwordInput;
    public GameObject registerMessage;

    public void RegisterPlayerButton()
    {
        if (displayNameInput.text != "" && userIDInput.text != "" && passwordInput.text != "")
        {
            new GameSparks.Api.Requests.RegistrationRequest()
                .SetDisplayName(displayNameInput.text)
                .SetUserName(userIDInput.text)
                .SetPassword(passwordInput.text)
                .Send((response) =>
                {
                    if (!response.HasErrors)
                    {
                        new LogEventRequest().SetEventKey("REGISTER_PLAYER")
                        .SetEventAttribute("ID", userIDInput.text)
                        .SetEventAttribute("NAME", displayNameInput.text)
                        .Send((res) =>
                        {
                            if (!res.HasErrors)
                            {
                                Debug.Log("Register succeeded.");
                                registerMessage.SetActive(true);
                                GameObject.Find("RegisterMessage").GetComponentInChildren<Text>().text = "Register succeeded.";
                                Invoke("closeMessage", 3);
                                displayNameInput.text = "";
                                userIDInput.text = "";
                                passwordInput.text = "";
                            }
                            else
                            {
                                Debug.Log("Register failed. : " + res.Errors.JSON.ToString());
                                registerMessage.SetActive(true);
                                GameObject.Find("RegisterMessage").GetComponentInChildren<Text>().text = "Register failed. : " + res.Errors.JSON.ToString();
                                Invoke("closeMessage", 3);
                            }
                        });
                    }
                    else
                    {
                        Debug.Log("Register failed. : " + response.Errors.JSON.ToString());
                        registerMessage.SetActive(true);
                        GameObject.Find("RegisterMessage").GetComponentInChildren<Text>().text = "Register failed. : " + response.Errors.JSON.ToString();
                        Invoke("closeMessage", 3);
                    }
                });
        }
        else
        {
            registerMessage.SetActive(true);
            GameObject.Find("RegisterMessage").GetComponentInChildren<Text>().text = "Register failed. Please fill out all fields.";
            Invoke("closeMessage", 3);
        }
    }

    public void closeMessage()
    {
        registerMessage.SetActive(false);
    }

    public void ExitButton()
    {
        Application.Quit();
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
