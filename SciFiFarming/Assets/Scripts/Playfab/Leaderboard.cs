using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class Leaderboard : MonoBehaviour
{
    public GameObject leaderboardCanvas;
    public GameObject[] leaderboardEntries;
    [SerializeField] GameObject leaderboardObject;

    public static Leaderboard instance;
    private void Awake()
    {
        if (PersistentData.guestUser)
        {
            Destroy(leaderboardObject);
            Destroy(this);
        }
        if (instance == null)
        {
            instance = this;
        }
    }

    public void OnLoggedIn()
    {
        leaderboardCanvas.SetActive(true);
        DisplayLeaderboard();
    }

    public void DisplayLeaderboard()
    {
        GetLeaderboardRequest getLeaderboardRequest = new GetLeaderboardRequest
        {
            StatisticName = "FastestTime",
            MaxResultsCount = 10
        };

        PlayFabClientAPI.GetLeaderboard(getLeaderboardRequest,
            result => UpdateLeaderboardUI(result.Leaderboard),
            error => Debug.Log(error.ErrorMessage)
        );
    }

    public void UpdateLeaderboardUI(List<PlayerLeaderboardEntry> leaderboard)
    {
        for (int x = 0; x < leaderboardEntries.Length; x++)
        {
            leaderboardEntries[x].SetActive(x < leaderboard.Count);
            if (x >= leaderboard.Count)
            {
                continue;
            }
            //adjusts the player name and rank
            leaderboardEntries[x].transform.Find("Name").GetComponent<TextMeshProUGUI>().text =
                (leaderboard[x].Position + 1) + ". " + leaderboard[x].DisplayName;
            //adjusts the players time
            leaderboardEntries[x].transform.Find("Score").GetComponent<TextMeshProUGUI>().text =
                ((float)leaderboard[x].StatValue).ToString();
        }
    }

    public void SetLeaderboardEntry(int newScore)
    {
        // NOTE: the original version of this game used server-side automation inJavascript to update
        // the leaderboard. Microsoft removed that feature for new PlayFabapplications, so we'll demonstrate
        // updating a leaderboard using two different methods below. The 'legacymethod' calls the custom server-side
        // javascript (cloud script). The alternative uses the latestPlayFabClientAPI to update the player's best score
        bool useLegacyMethod = false;
        if (useLegacyMethod)
        {
            ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
            {
                FunctionName = "UpdateHighscore",
                FunctionParameter = new { score = newScore }
            };
            //converts the request generated into JSON file that can be executed server side
            PlayFabClientAPI.ExecuteCloudScript(request,
            result =>
            {
                Debug.Log(result);
                DisplayLeaderboard();
                Debug.Log(result.ToJson());
            },
            error =>
            {
                Debug.Log(error.ErrorMessage);
                Debug.Log("ERROR");
            }
            );
        }
        else
        {
            // NOTE: by default, clients can't update player statistics
            // So for the code below to succeed:
            // 1. Log into PlayFab (from your web browser)
            // 2. Select your Title.
            // 3. Select Settings from the left-menu.
            // 4. Select the API Features tab.
            // 5. Find and activate Allow client to post player statistics.
            // (source:https://learn.microsoft.com/en-us/gaming/playfab/features/data/playerdata/using-player - statistics)

            bool hasFastest = false;
            //this should prevent overwriting times with worse time 
            PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(),
                result =>
                {
                    foreach (var eachStat in result.Statistics)
                    {
                        if (eachStat.StatisticName == "FastestTime")
                        {
                            hasFastest = true;
                            Debug.Log("old: " + eachStat.Value + "\nNew: " + newScore);
                            Debug.Log("Mark 2: " + hasFastest);
                            if (eachStat.Value < newScore) //this one is fine, it's the other that sucks
                            {
                                Debug.Log("update");
                                UpdatePlayerStatisics(newScore);
                            }
                        }
                    }
                    if (!hasFastest)
                    {
                        Debug.Log("initialize");
                        UpdatePlayerStatisics(newScore);
                    }
                },
                error => { Debug.Log(error.ErrorMessage); }
            );
        }
    }

    private void UpdatePlayerStatisics(int newScore)
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
            {
                // request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required.
                // probably for adjusting players that move on the statistics
                Statistics = new List<StatisticUpdate>
                    {
                        new StatisticUpdate { StatisticName = "FastestTime", Value = newScore },
                    }
            },
            result => { Debug.Log("User statistics updated"); },
            error => { Debug.LogError(error.GenerateErrorReport()); }
        );
        DisplayLeaderboard();
    }
}