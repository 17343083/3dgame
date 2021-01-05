using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : System.Object
{
	private static Director D_instance;
	public SceneController sceneController { get; set; }

	public static Director getInstance()
	{
		if (D_instance == null)
			D_instance = new Director();
		return D_instance;
	}
}

public interface SceneController
{
	void load();
}

public interface UserAction
{
	void moveBoat();
	void characterIsClicked(MyCharacterController c_controller);
	void restart();
	bool stop();
	int[] getNum();
}
public class MyCharacterController
{
	readonly GameObject character;
	readonly ClickGUI clickGUI;
	readonly bool is_priest;
	public float speed = 200;
	bool is_onboat;
	CoastController coastController;
	public MyCharacterController(string c_str)
	{
		if (c_str == "Priest")
		{
			is_priest = true;
			character = Object.Instantiate(Resources.Load("Prefabs/Priest", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
		}
		else if (c_str == "Devil")
		{
			is_priest = false;
			character = Object.Instantiate(Resources.Load("Prefabs/Devil", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
		}
		clickGUI = character.AddComponent(typeof(ClickGUI)) as ClickGUI;
		clickGUI.setController(this);
	}
	public void Onboat(BoatController boatController)
	{
		coastController = null;
		character.transform.parent = boatController.getBoat().transform;
		is_onboat = true;
	}
	public void Oncoast(CoastController temp)
	{
		coastController = temp;
		character.transform.parent = null;
		is_onboat = false;
	}
	public void reset()
	{
		coastController = (Director.getInstance().sceneController as FirstController).leftCoast;
		Oncoast(coastController);
		setPosition(coastController.getEmptyPosition());
		coastController.getOnCoast(this);
	}
	public void setName(string name)
	{
		character.name = name;
	}
	public GameObject getGameObject()
	{
		return character;
	}
	public string getName()
	{
		return character.name;
	}
	public void setPosition(Vector3 position)
	{
		character.transform.position = position;
	}
	public Vector3 getPosition()
	{
		return character.transform.position;
	}
	public bool getType()
	{
		return is_priest;
	}
	public bool getis_onboat()
	{
		return is_onboat;
	}
	public CoastController getcoastController()
	{
		return coastController;
	}
}

public class BoatController
{
	readonly GameObject Boat;
	public float speed = 200;
	readonly Vector3 right_pos = new Vector3(4, 1, 0);
	readonly Vector3 left_pos = new Vector3(-4, 1, 0);
	readonly Vector3[] start_pos;
	readonly Vector3[] end_pos;
	public bool is_left;
	MyCharacterController[] passenger = new MyCharacterController[2];
	public BoatController()
	{
		is_left = true;
		end_pos = new Vector3[] { new Vector3(3F, 2F, 0), new Vector3(4.5F, 2F, 0) };
		start_pos = new Vector3[] { new Vector3(-4.5F, 2F, 0), new Vector3(-3F, 2F, 0) };
		Boat = Object.Instantiate(Resources.Load("Prefabs/Boat", typeof(GameObject)), left_pos, Quaternion.identity, null) as GameObject;
		Boat.name = "Boat";
		Boat.AddComponent(typeof(ClickGUI));
	}
	public bool isEmpty()
	{
		for (int i = 0; i < passenger.Length; i++)
		{
			if (passenger[i] != null)
				return false;
		}
		return true;
	}
	public int getEmptyIndex()
	{
		for (int i = 0; i < passenger.Length; i++)
		{
			if (passenger[i] == null) return i;
		}
		return -1;
	}
	public Vector3 getEmptyPos()
	{
		int index = getEmptyIndex();
		if (is_left)
			return start_pos[index];
		else
			return end_pos[index];
	}
	public Vector3 BoatMoveToPosition()
	{
		is_left = !is_left;
		if (is_left == true)
		{
			return left_pos;
		}
		else
		{
			return right_pos;
		}
	}
	public void GetOnBoat(MyCharacterController charactercontroller)
	{
		int index = getEmptyIndex();
		if (index != -1)
			passenger[index] = charactercontroller;
	}
	public MyCharacterController GetOffBoat(string name)
	{
		for (int i = 0; i < passenger.Length; i++)
		{
			if (passenger[i] != null && passenger[i].getName() == name)
			{
				MyCharacterController mycharacter = passenger[i];
				passenger[i] = null;
				return mycharacter;
			}
		}
		return null;
	}
	public void reset()
	{
		is_left = true;
		Boat.transform.position = left_pos;
		passenger = new MyCharacterController[2];
	}
	public bool get_is_left()
	{
		return is_left;
	}
	public GameObject getBoat()
	{
		return Boat;
	}
	public int[] getCharacterNum()
	{
		int[] count = { 0, 0 };
		for (int i = 0; i < passenger.Length; i++)
		{
			if (passenger[i] != null)
			{
				if (passenger[i].getType() == true)
				{
					count[0]++;
				}
				else
				{
					count[1]++;
				}
			}
		}
		return count;
	}
}

public class CoastController
{
	readonly GameObject coast;
	readonly Vector3 right_pos = new Vector3(10, 1, 0);
	readonly Vector3 left_pos = new Vector3(-10, 1, 0);
	readonly Vector3[] positions;
	readonly bool is_right;

	public int Priests = 0;
	public int Devils = 0;

	MyCharacterController[] passenger;

	public CoastController(string pos)
	{
		positions = new Vector3[] {new Vector3(6.5F,2.6F,0), new Vector3(7.7F,2.6F,0), new Vector3(8.9F,2.6F,0),
			new Vector3(10.1F,2.6F,0), new Vector3(11.3F,2.6F,0), new Vector3(12.5F,2.6F,0)};
		passenger = new MyCharacterController[6];
		if (pos == "right")
		{
			coast = Object.Instantiate(Resources.Load("Prefabs/coast", typeof(GameObject)), right_pos, Quaternion.identity, null) as GameObject;
			coast.name = "right";
			is_right = true;
		}
		else if (pos == "left")
		{
			coast = Object.Instantiate(Resources.Load("Prefabs/coast", typeof(GameObject)), left_pos, Quaternion.identity, null) as GameObject;
			coast.name = "left";
			is_right = false;
		}
	}
	public int getEmptyIndex()
	{
		for (int i = 0; i < passenger.Length; i++)
		{
			if (passenger[i] == null)
			{
				return i;
			}
		}
		return -1;
	}
	public Vector3 getEmptyPosition()
	{
		Vector3 pos = positions[getEmptyIndex()];
		if (is_right == false)
			pos.x *= -1;
		return pos;
	}
	public void getOnCoast(MyCharacterController mycharacter)
	{
		passenger[getEmptyIndex()] = mycharacter;
		if (mycharacter.getType())
		{
			Priests++;
		}
		else
		{
			Devils++;
		}
	}
	public MyCharacterController getOffCoast(string name)
	{
		for (int i = 0; i < passenger.Length; i++)
		{
			if (passenger[i] != null && passenger[i].getName() == name)
			{
				if (passenger[i].getType() == true)
				{
					Priests--;
				}
				else
				{
					Devils--;
				}
				MyCharacterController mycharacter = passenger[i];
				passenger[i] = null;
				return mycharacter;
			}
		}
		return null;
	}
	public void reset()
	{
		passenger = new MyCharacterController[6];
		Priests = 0;
		Devils = 0;
	}
	public bool get_is_right()
	{
		return is_right;
	}
	public int[] getCharacterNum()
	{
		int[] count = { 0, 0 };
		for (int i = 0; i < passenger.Length; i++)
		{
			if (passenger[i] != null)
			{
				if (passenger[i].getType() == true)
				{
					count[0]++;
				}
				else
				{
					count[1]++;
				}
			}
		}
		return count;
	}
}