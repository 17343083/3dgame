using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickGUI : MonoBehaviour
{
	UserAction action;
	// 人物控制器
	MyCharacterController characterController;

	public void setController(MyCharacterController characterCtrl)
	{
		characterController = characterCtrl;
	}

	void Start()
	{
		action = Director.getInstance().currentSceneController as UserAction;
	}

	void OnMouseDown()
	{
		if (gameObject.name == "boat")
		{
			action.moveBoat();
		}
		else
		{
			action.moveCharacter(characterController);
		}
	}
}