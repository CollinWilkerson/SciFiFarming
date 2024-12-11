using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class LoginRegister : MonoBehaviour
{
    [HideInInspector]
    public string playFabId;

    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;

    public TextMeshProUGUI displayText;

    public UnityEvent onLoggedIn;

    public static LoginRegister instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    public void OnRegister()
    {
        RegisterPlayFabUserRequest registerRequest = new RegisterPlayFabUserRequest
        {
            Username = usernameInput.text,
            DisplayName = usernameInput.text,
            Password = passwordInput.text,
            RequireBothUsernameAndEmail = false
        };

        PlayFabClientAPI.RegisterPlayFabUser(registerRequest,
            result =>
            {
                displayText.gameObject.SetActive(true);
                SetDisplayText(result.PlayFabId, Color.green); //logs the success if we succeed
            },
            error =>
            {
                displayText.gameObject.SetActive(true);
                SetDisplayText(error.ErrorMessage, Color.red); //logs the error if we fail
            }
        );
    }

    public void OnLogin()
    {
        LoginWithPlayFabRequest loginRequest = new LoginWithPlayFabRequest
        {
            Username = usernameInput.text,
            Password = passwordInput.text,
        };

        PlayFabClientAPI.LoginWithPlayFab(loginRequest,
            result =>
            {
                displayText.gameObject.SetActive(true);
                Debug.Log("Logged in as: " + result.PlayFabId);

                playFabId = result.PlayFabId;

                if (onLoggedIn != null)
                {
                    onLoggedIn.Invoke();
                }
            },
            error =>
            {
                displayText.gameObject.SetActive(true);
                SetDisplayText(error.ErrorMessage, Color.red);
            }
        );
    }

    private void SetDisplayText(string text, Color color)
    {
        displayText.text = text;
        displayText.color = color;
    }
}
