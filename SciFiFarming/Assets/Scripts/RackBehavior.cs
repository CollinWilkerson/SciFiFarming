using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RackBehavior : MonoBehaviourPun
{
    [Header("Physical")]
    private bool isUp;
    [SerializeField] private float riseSpeed = 0.3f;
    private int rackLevels = 5;
    private int plantsPerRack = 4;
    [SerializeField] private GameObject top;
    [SerializeField] private GameObject rack;
    [SerializeField] private GameObject tankConnection;
    [SerializeField] private Transform[] rackPositions;

    [Header("Nutrient Tank")]
    private float nutrientQuality;
    private float tankLevel;
    [SerializeField] private float tankMax = 20;
    [SerializeField] private float fillRate = 5;
    public float tempQuality; //this willl be replaced by the quality of nutrient the player has
    public PlantBehavior tempSeed; //this will be replaced by the index of the plant type the player's seed is
    //this tuple effectivley represents every crop as 3 integers, so they are easy to move around and identify
    private (int type, int value, int stage) [] crops;
    private GameObject[] cropObjects;

    private bool actionBuffer;

    private void Awake()
    {
        isUp = false;
        crops = new (int type, int value, int stage)[rackLevels * plantsPerRack];
        cropObjects = new GameObject[rackLevels * plantsPerRack];
        //set each crop to -1 so they are identifiable as empty
        for (int i = 0; i < crops.Length; i++)
        {
            crops[i].type = -1;
        }
    }

    private void Start()
    {
        tempSeed = PlantLibrary.library[0];
    }

    //this currently contains temp inputs that will be replaced by player context
    private void Update()
    {
        if (!isUp)
        {
            //moves rack up
            rack.transform.position = new Vector3(rack.transform.position.x, (Mathf.Lerp(rack.transform.position.y, rackLevels * -1.1f, riseSpeed)), rack.transform.position.z);
            //rack changes position on interaction with top
            if (Input.GetKeyDown(KeyCode.E))
            {
                //this prevents the rack from going up and down in one frame
                actionBuffer = true;
                isUp = true;
            }
        }
        if (isUp)
        {
            //moves rack down
            rack.transform.position = new Vector3(rack.transform.position.x, (Mathf.Lerp(rack.transform.position.y, 0, riseSpeed)), rack.transform.position.z);
            //rack changes position on interaction with top
            if (Input.GetKeyDown(KeyCode.E) && !actionBuffer)
            {
                isUp = false;
            }
            if (Input.GetKey(KeyCode.R)) // player will interact wiht the tank connection for this
            {
                FillTank(tempQuality);
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                PlantSeeds(tempSeed.plantIndex);
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                CycleAdvance();
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                Harvest();
            }
        }

        //reset buffer on frame end
        actionBuffer = false;
    }

    /// <summary>
    /// Fills a Hydroponic nutrient solution tank with the solution quality supplied
    /// </summary>
    /// <param name="quality"></param>
    private void FillTank(float quality)
    {
        if(tankLevel >= tankMax)
        {
            tankLevel = tankMax;
            return;
        }
        //(c1V1 + c2V2)/(V1 + V2) = c3
        nutrientQuality = (tankLevel * nutrientQuality + fillRate * quality * Time.deltaTime) / (tankLevel + fillRate * Time.deltaTime);
        tankLevel += fillRate * Time.deltaTime;
    }

    [PunRPC]
    private void PlantSeeds(int seed)//seed should be a growth stage 0 crop
    {
        Debug.Log("plant");
        for (int i = 0; i < crops.Length; i++)
        {
            if (crops[i].type == -1) // if there is no crop
            {
                crops[i].type = seed;
                crops[i].value = PlantLibrary.library[seed].value;

                //Mesh instantiation, replace with photon
                Vector3 spawnPos = new Vector3(rackPositions[i % rackPositions.Length].position.x, 
                    rackPositions[i%rackPositions.Length].position.y +  i/rackPositions.Length * 1.1f,
                    rackPositions[i%rackPositions.Length].position.z);
                GameObject spawnObj = Resources.Load(PlantLibrary.library[seed].stageModels[0]) as GameObject;
                cropObjects[i] = Instantiate(spawnObj, spawnPos, Quaternion.identity);
            }
        }
    }

    /// <summary>
    /// The behavior of the stand after the day ends
    /// </summary>
    private void CycleAdvance()
    {
        for (int i = 0; i < crops.Length; i++)
        {
            //don't grow past max crop growth, growth takes 1 tank level
            if (crops[i].type != -1 && crops[i].stage < PlantLibrary.library[crops[i].type].harvestStage && tankLevel >= 1)
            {
                crops[i].stage++;
                crops[i].value = (int)(crops[i].value * nutrientQuality);
                tankLevel -= 1;

                //updating the mesh
                Vector3 spawnPos = cropObjects[i].transform.position;
                GameObject spawnObj = Resources.Load(PlantLibrary.library[crops[i].type].stageModels[crops[i].stage]) as GameObject;
                Destroy(cropObjects[i]);
                cropObjects[i] = Instantiate(spawnObj, spawnPos, Quaternion.identity);
            }
        }
    }

    /// <summary>
    /// remove all fully grown crops from the array
    /// </summary>
    private void Harvest() // this could be moved to the plant behavior for a more interactive harvest mechanic
    {
        for (int i = 0; i < crops.Length; i++)
        {
            if(crops[i].type != -1 && crops[i].stage == PlantLibrary.library[crops[i].type].harvestStage) // if the crop is fully grown remove it from the rack and place into the players inventory
            {
                //add to inventory
                Debug.Log("Harvest");
                GameManager.instance.gold += crops[i].value;
                //remove crop
                crops[i].type = -1;
                crops[i].value = 0;
                crops[i].stage = 0;
                Destroy(cropObjects[i]);
            }
        }
    }


}
