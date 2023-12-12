using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour
{
    public async void OnClickLog()
    {
        await CloudSaveWrapper.Save<int>("WoodResource", 2);
    }
}
