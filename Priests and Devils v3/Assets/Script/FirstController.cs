using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, SceneController, UserAction
{
	Vector3 water_pos = new Vector3(0, 0.5f, 0);

	public CoastController leftCoast;
	public CoastController rightCoast;
	public BoatController Boat;
	public PristsAndDevilsState PristsAndDevilsState;

	private MyCharacterController[] characters = null;
	private UserGUI userGUI = null;
	public bool flag_stop = false;

	public float speed = 2.0f;

	public CCActionManager actionManager;
	void Awake()
	{
		Director director = Director.getInstance();
		director.sceneController = this;
		userGUI = gameObject.AddComponent<UserGUI>() as UserGUI;
		characters = new MyCharacterController[6];
		load();
		flag_stop = false;
	}
	public void load()
	{
		GameObject Water = Instantiate(Resources.Load("Prefabs/Water", typeof(GameObject)), water_pos, Quaternion.identity, null) as GameObject;
		Water.name = "Water";
		leftCoast = new CoastController("left");
		rightCoast = new CoastController("right");
		Boat = new BoatController();
		for (int i = 0; i < 3; i++)
		{
			MyCharacterController character = new MyCharacterController("Priest");
			character.setPosition(leftCoast.getEmptyPosition());
			character.Oncoast(leftCoast);
			leftCoast.getOnCoast(character);
			characters[i] = character;
			character.setName("Priest" + i);
		}
		for (int i = 0; i < 3; i++)
		{
			MyCharacterController character = new MyCharacterController("Devil");
			character.setPosition(leftCoast.getEmptyPosition());
			character.Oncoast(leftCoast);
			leftCoast.getOnCoast(character);
			characters[i + 3] = character;
			character.setName("Devil" + i);
		}
	}
	int check_game_over()
	{
		int left_priest = 0, left_devil = 0, right_priest = 0, right_devil = 0;
		int[] fromCount = leftCoast.getCharacterNum();
		int[] toCount = rightCoast.getCharacterNum();
		left_priest += fromCount[0];
		left_devil += fromCount[1];
		right_priest += toCount[0];
		right_devil += toCount[1];
		if (right_priest + right_devil == 6)
			return 1;
		int[] boatCount = Boat.getCharacterNum();
		if (!Boat.get_is_left())
		{
			right_priest += boatCount[0];
			right_devil += boatCount[1];
		}
		else
		{
			left_priest += boatCount[0];
			left_devil += boatCount[1];
		}
		if ((left_priest < left_devil && left_priest > 0) || (right_priest < right_devil && right_priest > 0))
		{
			return -1;
		}
		return 0;
	}
	public void restart()
	{
		Boat.reset();
		leftCoast.reset();
		rightCoast.reset();
		for (int i = 0; i < characters.Length; i++)
		{
			characters[i].reset();
		}
	}
	public bool stop()
	{
		if (check_game_over() != 0)
			return true;
		return false;
	}
	public void moveBoat()
	{
		if (Boat.isEmpty()) return;
		actionManager.moveBoatAction(Boat.getBoat(), Boat.BoatMoveToPosition(), speed);
		userGUI.status = check_game_over();
	}

	public void characterIsClicked(MyCharacterController character)
	{
		if (userGUI.status != 0) return;
		if (character.getis_onboat())
		{
			CoastController coast;
			if (!Boat.get_is_left())
			{
				coast = rightCoast;
			}
			else
			{
				coast = leftCoast;
			}
			Boat.GetOffBoat(character.getName());
			Vector3 end_pos = coast.getEmptyPosition();
			Vector3 middle_pos = new Vector3(character.getGameObject().transform.position.x, end_pos.y, end_pos.z);
			actionManager.moveCharacterAction(character.getGameObject(), middle_pos, end_pos, speed);
			character.Oncoast(coast);
			coast.getOnCoast(character);
		}
		else
		{
			CoastController coast = character.getcoastController();
			if (Boat.getEmptyIndex() == -1)
			{
				return;
			}
			if (coast.get_is_right() == Boat.get_is_left())
				return;

			coast.getOffCoast(character.getName());
			Vector3 end_pos = Boat.getEmptyPos();
			Vector3 middle_pos = new Vector3(end_pos.x, character.getGameObject().transform.position.y, end_pos.z);
			actionManager.moveCharacterAction(character.getGameObject(), middle_pos, end_pos, character.speed);
			character.Onboat(Boat);
			Boat.GetOnBoat(character);
		}
		userGUI.status = check_game_over();
	}

	public int[] getNum()
	{
		int[] arr = new int[5];
		arr[0] = leftCoast.Priests;
		arr[1] = leftCoast.Devils;
		arr[2] = rightCoast.Priests;
		arr[3] = rightCoast.Devils;
		arr[4] = Boat.is_left ? 0 : 1;
		return arr;
	}

}
