using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolbarController : MonoBehaviour
{
    [SerializeField] private Transform handSpawnPos;
    private GameObject handObject;

    public static ToolbarController instance;
    private InventorySlotController[] toolbar;
    public InventorySlotController activeTool;
    private WeaponData activeWeapon;
    private PlantData activePlant;
    private int activeIndex;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        toolbar = GetComponentsInChildren<InventorySlotController>();
        activeTool = toolbar[0];
        activeIndex = 0;
    }

    void Update()
    {
        //switching items
        if(Input.mouseScrollDelta.y != 0)
        {
            if(handObject != null)
            {
                Destroy(handObject);
            }
            activeTool.SetColor(Color.white);
            activeIndex = (activeIndex - (int) Input.mouseScrollDelta.y) % toolbar.Length;
            activeIndex = activeIndex < 0 ? 5 : activeIndex; //wraps scroll
            activeTool = toolbar[activeIndex];
            activeTool.SetColor(Color.red);

            if (!activeTool.isFilled)
            {
                return;
            }

            GameObject spawnObj;
            switch (activeTool.type)
            {
                case ItemType.plant:
                    activePlant = PlantLibrary.library[activeTool.GetLibraryIndex()];
                    spawnObj = Resources.Load(activePlant.stageModels[activePlant.harvestStage]) as GameObject;
                    handObject = Instantiate(spawnObj, handSpawnPos.position, PlayerController.clientPlayer.transform.rotation, PlayerController.clientPlayer.transform);
                    break;
                case ItemType.weapon:
                    activeWeapon = WeaponLibrary.library[activeTool.GetLibraryIndex()];
                    spawnObj = Resources.Load(WeaponLibrary.library[activeWeapon.weaponIndex].model) as GameObject;
                    handObject = Instantiate(spawnObj, handSpawnPos.position, PlayerController.clientPlayer.transform.rotation, PlayerController.clientPlayer.transform);
                    break;
            }
        }

        //in game action
        if (Input.GetMouseButtonDown(0))
        {
            if (!activeTool.isFilled)
            {
                Debug.Log("nothing in hand");
                return;
            }

            switch (activeTool.type)
            {
                case ItemType.plant:
                    break;
                case ItemType.weapon:
                    RaycastHit hit;

                    if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, activeWeapon.range, GameManager.destructables))
                    {
                        if (hit.collider.gameObject.CompareTag("Enemy"))
                        {
                            hit.collider.gameObject.GetComponent<BugEnemy>().TakeDamage(activeWeapon.damage);
                        }
                    }
                    break;
            }
        }
    }
}
