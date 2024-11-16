using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RackBehavior : MonoBehaviour
{
    [Header("Physical")]
    private bool isUp;
    [SerializeField] private float riseSpeed = 0.3f;
    private int rackLevels = 5;
    private int plantsPerRack = 4;
    [SerializeField] private GameObject top;
    [SerializeField] private GameObject rack;
    [SerializeField] private GameObject tankConnection;

    [Header("Nutrient Tank")]
    private float nutrientQuality;
    private float tankLevel;
    [SerializeField] private float tankMax = 20;
    [SerializeField] private float fillRate = 5;
    public float tempQuality; //this willl be replaced by the quality of nutrient the player has
    private int[] crops;

    private bool actionBuffer;

    private void Awake()
    {
        isUp = false;
        crops = new int[rackLevels * plantsPerRack];
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
                PlantSeeds();
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

    private void PlantSeeds()//this will eventually take a seed but i just have ints for now
    {
        for(int i = 0; i < crops.Length; i++)
        {
            if (crops[i] == 0) // if there is no crop
            {
                crops[i] = 1;
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
            if (crops[i] < 4 && crops[i] != 0 && tankLevel >= 1)
            {
               crops[i]++;
               tankLevel -= 1;
            }
        }
    }

    /// <summary>
    /// remove all fully grown crops from the array
    /// </summary>
    private void Harvest()
    {
        for (int i = 0; i < crops.Length; i++)
        {
            if(crops[i] == 4) // if the crop is fully grown remove it from the rack and place into the players inventory
            {
                crops[i] = 0;
            }
        }
    }
}
