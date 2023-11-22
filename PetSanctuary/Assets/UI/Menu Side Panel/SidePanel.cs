using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SidePanel : MonoBehaviour
{
    public void ToStartMenu()
    {
        SceneManager.LoadScene("Start Menu");
    }
}
