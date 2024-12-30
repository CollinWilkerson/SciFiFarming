using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ToolbarController : MonoBehaviourPun
{
    [SerializeField] private Transform handSpawnPos;
    private GameObject handObject;

    public static ToolbarController instance;
    private InventorySlotController[] toolbar;
    public InventorySlotController activeTool;
    private WeaponData activeWeapon;
    private PlantData activePlant;
    private int activeIndex;

    //weaponStuff
    private float lastAttackTime;

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
            //this removes the gameobject
            ClearHand();
            //change the aperance of the toolbar
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
                    
                    //handObject = PhotonNetwork.Instantiate(activePlant.stageModels[activePlant.harvestStage], handSpawnPos.position, PlayerController.clientPlayer.transform.rotation);
                    //handObject.transform.parent = PlayerController.clientPlayer.transform;
                    break;
                case ItemType.weapon:
                    activeWeapon = WeaponLibrary.library[activeTool.GetLibraryIndex()];
                    
                    spawnObj = Resources.Load(activeWeapon.model) as GameObject;
                    handObject = Instantiate(spawnObj, handSpawnPos.position, PlayerController.clientPlayer.transform.rotation, PlayerController.clientPlayer.transform);
                    
                    //this is worth a go but there is no way to parent the object on instantiation so it might suck
                    // could maybe parent client side and transform view the prefab.
                    //handObject = PhotonNetwork.Instantiate(activeWeapon.model, handSpawnPos.position, PlayerController.clientPlayer.transform.rotation);
                    //handObject.transform.parent = PlayerController.clientPlayer.transform;
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
                    Attack();
                    break;
            }
        }
    }

    private void Attack()
    {
        RaycastHit hit;

        if (Time.time - lastAttackTime < activeWeapon.rateOfFire)
        {
            return;
        }
        lastAttackTime = Time.time;
        Debug.Log("Firing: " + activeWeapon.type);
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, activeWeapon.range, GameManager.destructables))
        {
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Enemy hit");
                //hit.collider.gameObject.GetComponent<BugEnemy>().TakeDamage(activeWeapon.damage);
                hit.collider.gameObject.GetComponent<BugEnemy>().photonView.RPC("TakeDamage", RpcTarget.All, activeWeapon.damage);
            }
        }
    }

    public void SetHandSpawnPos(Transform t)
    {
        handSpawnPos = t;
    }

    public void ClearHand()
    {
        if (handObject != null)
        {
            Destroy(handObject);
            //PhotonNetwork.Destroy(handObject);
        }
    }
}
