using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class HeaderInfo : MonoBehaviourPun
{
    public Image bar;
    private float maxValue = 1f;


    public void Initialize()
    {
        bar.fillAmount = 1f;
    }

    [PunRPC]
    public void UpdateHealthBar(float value)
    {
        //percentage of health as the fill amount
        bar.fillAmount = (float)value / maxValue;
    }
}
