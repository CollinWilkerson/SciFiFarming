using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public int gold; 
	public static GameManager instance;
	[SerializeField] private GameObject playerInventory;
	[SerializeField] private GameObject sellScreen;

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
		
		//this makes sure that all items are properly initialized before they are needed in game
		//check with slease about this
		FindFirstObjectByType<PlantLibrary>().Awake();
		playerInventory.SetActive(true);
		playerInventory.SetActive(false);
		sellScreen.SetActive(true);
		sellScreen.SetActive(false);
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
