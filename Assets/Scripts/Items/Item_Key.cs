using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Key : MonoBehaviour
{
    public GameObject lockDoor;
    public GameObject unlockDoor;

    public void OpenDoor() 
    {
        lockDoor.SetActive(false);
        unlockDoor.SetActive(true);
    }

}
