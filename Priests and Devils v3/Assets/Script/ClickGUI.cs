using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickGUI : MonoBehaviour
{
	UserAction action;
	MyCharacterController character;
	void Start()
	{
		action = Director.getInstance().sceneController as UserAction;
	}
	void OnMouseDown()
	{
		if (action.stop())
			return;
		if (gameObject.name == "Boat")
		{
			action.moveBoat();
		}
		else
		{
			action.characterIsClicked(character);
		}
	}
	public void setController(MyCharacterController characterCtrl)
	{
		character = characterCtrl;
	}
}
