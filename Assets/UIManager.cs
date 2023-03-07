using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject DeathScreen;

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

}
