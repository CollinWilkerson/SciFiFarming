using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class GameManager : MonoBehaviourPun
{
	public int gold; 
	public static GameManager instance;
	[SerializeField] private GameObject playerInventory;
	[SerializeField] private GameObject sellScreen;
    [SerializeField] private NPCScreenController npcScreen;
    [SerializeField] private LayerMask initInteractables;
    public static LayerMask interactables;
    [SerializeField] private LayerMask initDestructables;
    public static LayerMask destructables;
    public static ToolbarController toolbar;

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
            spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);

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
}
