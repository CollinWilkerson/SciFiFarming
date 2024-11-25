using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class GameManager : MonoBehaviour
{
	public int gold; 
	public static GameManager instance;
	[SerializeField] private GameObject playerInventory;
	[SerializeField] private GameObject sellScreen;
    [SerializeField] private LayerMask initInteractables;
    public static LayerMask interactables;

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
