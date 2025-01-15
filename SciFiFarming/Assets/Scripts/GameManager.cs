using System;
using UnityEngine;
using TMPro;
using Photon.Pun;
using System.Linq;
using System.IO;

public class GameManager : MonoBehaviourPun
{
	public static GameManager instance;
	[SerializeField] private GameObject playerInventory;
	[SerializeField] private GameObject sellScreen;
    public NPCScreenController npcScreen;
    [SerializeField] private LayerMask initInteractables;
    public static LayerMask interactables;
    [SerializeField] private LayerMask initDestructables;
    public static LayerMask destructables;
    public static ToolbarController toolbar;
    [SerializeField] private TextMeshProUGUI initMoneyText;
    public static TextMeshProUGUI moneyText;

    //photon
    [Header("Players")]
    public PlayerController[] players;
    private int playersInGame;
    public string playerPrefabLocation;
    public Transform[] spawnPoints;

    [Header("For Initialization")]
    [SerializeField] private HeaderInfo playerHeaderInfo;
    [SerializeField] private GameObject playerToolTip;
    [SerializeField] private GameObject damageFilter;
    public GameObject SleepScreen;

    [Header("Quests")]
    public TextMeshProUGUI questShortText;
    public static int kills = 0;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
			Destroy(this);
        }
        else
        {
			instance = this;
        }
        interactables = initInteractables;
        destructables = initDestructables;
        moneyText = initMoneyText;
    }

    private void Start()
    {
        //this makes sure that all items are properly initialized before they are needed in game
        if (playerInventory != null)
        {
            playerInventory.SetActive(true);
            playerInventory.SetActive(false);
        }
        if (sellScreen != null)
        {
            sellScreen.SetActive(true);
            sellScreen.SetActive(false);
        }
        if(FindAnyObjectByType<Leaderboard>(FindObjectsInactive.Include) != null)
        {
            GameObject temp = FindAnyObjectByType<Leaderboard>(FindObjectsInactive.Include).gameObject;
            temp.SetActive(true);
            temp.SetActive(false);
        }
        playerInventory.GetComponent<InventoryController>().ClearInventory();
        PersistentData.SetInventoryFromList(playerInventory.GetComponent<InventoryController>());
        toolbar = ToolbarController.instance;
        if (npcScreen != null)
        {
            npcScreen.SetRacks(FindObjectsByType<RackController>(FindObjectsSortMode.None));
        }

        //photon
        players = new PlayerController[PhotonNetwork.PlayerList.Length];

        photonView.RPC("ImInGame", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void ImInGame()
    {
        playersInGame++;

        if (playersInGame == PhotonNetwork.PlayerList.Length)
        {
            SpawnPlayer();
        }
    }

    private void SpawnPlayer()
    {
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefabLocation,
            spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);

        playerObj.GetComponent<PhotonView>().RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);
    }

    public InventoryController GetPlayerInventory()
    {
        return playerInventory.GetComponent<InventoryController>();
    }

    public HeaderInfo GetHeader()
    {
        return playerHeaderInfo;
    }
    public GameObject GetToolTip()
    {
        return playerToolTip;
    }
    public GameObject GetDamageFilter()
    {
        return damageFilter;
    }

    public PlayerController GetPlayer(int playerId)
    {
        return players.FirstOrDefault(x => x.id == playerId);
    }

    public PlayerController GetPlayer(GameObject playerObj)
    {
        return players.FirstOrDefault(x => x.gameObject == playerObj);
    }

    public static T CopyComponent<T>(T original, GameObject destination) where T : Component
	{
		System.Type type = original.GetType();
		Component copy = destination.AddComponent(type);
		System.Reflection.FieldInfo[] fields = type.GetFields();
		foreach (System.Reflection.FieldInfo field in fields)
		{
			field.SetValue(copy, field.GetValue(original));
		}
		return copy as T;
	}

    public static void SaveGame()
    {
        using (StreamWriter sw = File.CreateText(@"ABSaveData.txt"))
        {
            Debug.Log("Game Saved!");
            string tempInv = "";
            foreach(int i in PlayerController.clientPlayer.inventory.WriteInventory())
            {
                tempInv += i + ",";
            }
            sw.WriteLine(tempInv);
            sw.WriteLine(PersistentData.money);
            //sw.WriteLine();
        }
    }
    public static void TryLoad()
    {
        if (!File.Exists(@"ABSaveData.txt"))
        {
            return;
        }
        using (StreamReader sr = File.OpenText(@"ABSaveData.txt"))
        {
            string tempInv = sr.ReadLine();
            string[] parts = tempInv.Split(",");
            int[] inv = new int[parts.Length - 1];
            for(int i = 0; i < parts.Length - 1; i++)
            {
                Debug.Log(parts[i]);
                inv[i] = Int32.Parse(parts[i]);
            }
            PlayerController.clientPlayer.inventory.ReadInventory(inv);

            PersistentData.money = Int32.Parse(sr.ReadLine());
            moneyText.text = PersistentData.money + "D";
        }
    }
}
