using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChedkPoint : MonoBehaviour {

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerCharacter>())
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        }
    }
}
