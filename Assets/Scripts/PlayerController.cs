using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public PlayerCharacter character;

	void Awake () {
        character = FindObjectOfType<PlayerCharacter>();
    }
	
	void Update () {
        //if (!character.isAlice) return;
        character.Move();

        bool jump = Input.GetKeyDown(KeyCode.Space);
        if (jump)
        {
            character.Jump();
        }

        bool changeColor = Input.GetKeyDown(KeyCode.E);
        if (changeColor)
        {
            character.ChangeColor();
        }
    }
}
