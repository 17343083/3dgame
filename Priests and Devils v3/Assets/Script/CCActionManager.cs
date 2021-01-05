using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCActionManager : SSActionManager
{
	public CCMoveToAction ccmoveBoat;
	public CCSequenceAction ccmoveCharacter;
	public FirstController sceneController;

	protected new void Start()
	{
		sceneController = (FirstController)Director.getInstance().sceneController;
		sceneController.actionManager = this;
	}
	public void moveBoatAction(GameObject boat, Vector3 target, float speed)
	{
		ccmoveBoat = CCMoveToAction.GetSSAction(target, speed);
		this.RunAction(boat, ccmoveBoat, this);
	}

	public void moveCharacterAction(GameObject character, Vector3 middle_pos, Vector3 end_pos, float speed)
	{
		SSAction action1 = CCMoveToAction.GetSSAction(middle_pos, speed);
		SSAction action2 = CCMoveToAction.GetSSAction(end_pos, speed);
		ccmoveCharacter = CCSequenceAction.GetSSAction(1, 0, new List<SSAction> { action1, action2 });
		this.RunAction(character, ccmoveCharacter, this);
	}
}