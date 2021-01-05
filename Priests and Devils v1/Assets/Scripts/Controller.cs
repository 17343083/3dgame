using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : System.Object
{
	private static Director _instance;
	public SceneController currentSceneController { get; set; }

	public static Director getInstance()
	{
		if (_instance == null)
		{
			_instance = new Director();
		}
		return _instance;
	}
}

public interface SceneController
{
	void loadResources();
}

public interface UserAction
{
	void moveBoat();
	void moveCharacter(MyCharacterController characterCtrl);
	void restart();
}

// 移动控制器
public class MoveController : MonoBehaviour
{
	// 移动速度
	readonly float move_speed = 20;
	int moving_state;
	// 移动位置
	Vector3 destination;
	Vector3 tmp;

	void Update()
	{
		if (moving_state == 1)
		{
			transform.position = Vector3.MoveTowards(transform.position, tmp, move_speed * Time.deltaTime);
			if (transform.position == tmp)
			{
				moving_state = 2;
			}
		}
		else if (moving_state == 2)
		{
			transform.position = Vector3.MoveTowards(transform.position, destination, move_speed * Time.deltaTime);
			if (transform.position == destination)
			{
				moving_state = 0;
			}
		}
	}

	public void setDestination(Vector3 _dest)
	{
		destination = _dest;
		tmp = _dest;
		// 船只移动
		if (_dest.y == transform.position.y)
		{
			moving_state = 2;
		}
		// 人物上船
		else if (_dest.y < transform.position.y)
		{
			tmp.y = transform.position.y;
		}
		// 人物上岸
		else
		{
			tmp.x = transform.position.x;
		}
		moving_state = 1;
	}

	public void reset()
	{
		moving_state = 0;
	}
}

// 人物控制器
// 0代表牧师，1代表魔鬼
public class MyCharacterController
{
	readonly GameObject character;
	readonly MoveController moveableScript;
	readonly ClickGUI clickGUI;
	// 人物属性
	readonly int characterType;
	bool _isOnBoat;
	BankController bankController;

	public MyCharacterController(string which_character)
	{
		if (which_character == "priest")
		{
			character = Object.Instantiate(Resources.Load("Prefabs/Priest", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
			characterType = 0;
		}
		else
		{
			character = Object.Instantiate(Resources.Load("Prefabs/Devil", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
			characterType = 1;
		}
		moveableScript = character.AddComponent(typeof(MoveController)) as MoveController;

		clickGUI = character.AddComponent(typeof(ClickGUI)) as ClickGUI;
		clickGUI.setController(this);
	}

	public void setName(string name)
	{
		character.name = name;
	}

	public void setPosition(Vector3 pos)
	{
		character.transform.position = pos;
	}

	public void moveToPosition(Vector3 destination)
	{
		moveableScript.setDestination(destination);
	}

	public int getType()
	{
		return characterType;
	}

	public string getName()
	{
		return character.name;
	}

	public void getOnBoat(BoatController boatCtrl)
	{
		bankController = null;
		character.transform.parent = boatCtrl.getGameobj().transform;
		_isOnBoat = true;
	}

	public void getOnBank(BankController bankCtrl)
	{
		bankController = bankCtrl;
		character.transform.parent = null;
		_isOnBoat = false;
	}

	public bool isOnBoat()
	{
		return _isOnBoat;
	}

	public BankController getBankController()
	{
		return bankController;
	}

	public void reset()
	{
		moveableScript.reset();
		bankController = (Director.getInstance().currentSceneController as FirstController).fromBank;
		getOnBank(bankController);
		setPosition(bankController.getEmptyPosition());
		bankController.getOnBank(this);
	}
}

// 河岸控制器
public class BankController
{
	readonly GameObject bank;
	readonly Vector3 from_pos = new Vector3(9, 1, 0);
	readonly Vector3 to_pos = new Vector3(-9, 1, 0);
	readonly Vector3[] positions;
	readonly int to_or_from;

	MyCharacterController[] passengerPlaner;

	public BankController(string _to_or_from)
	{
		positions = new Vector3[] {new Vector3(6.5F,2.25F,0), new Vector3(7.5F,2.25F,0), new Vector3(8.5F,2.25F,0),
			new Vector3(9.5F,2.25F,0), new Vector3(10.5F,2.25F,0), new Vector3(11.5F,2.25F,0)};

		passengerPlaner = new MyCharacterController[6];

		if (_to_or_from == "right")
		{
			bank = Object.Instantiate(Resources.Load("Prefabs/Stone", typeof(GameObject)), from_pos, Quaternion.identity, null) as GameObject;
			bank.name = "right";
			to_or_from = 1;
		}
		else
		{
			bank = Object.Instantiate(Resources.Load("Prefabs/Stone", typeof(GameObject)), to_pos, Quaternion.identity, null) as GameObject;
			bank.name = "left";
			to_or_from = -1;
		}
	}

	public int getEmptyIndex()
	{
		for (int i = 0; i < passengerPlaner.Length; i++)
		{
			if (passengerPlaner[i] == null)
			{
				return i;
			}
		}
		return -1;
	}

	public Vector3 getEmptyPosition()
	{
		Vector3 pos = positions[getEmptyIndex()];
		pos.x *= to_or_from;
		return pos;
	}

	public void getOnBank(MyCharacterController characterCtrl)
	{
		int index = getEmptyIndex();
		passengerPlaner[index] = characterCtrl;
	}

	public MyCharacterController getOffBank(string passenger_name)
	{
		for (int i = 0; i < passengerPlaner.Length; i++)
		{
			if (passengerPlaner[i] != null && passengerPlaner[i].getName() == passenger_name)
			{
				MyCharacterController charactorCtrl = passengerPlaner[i];
				passengerPlaner[i] = null;
				return charactorCtrl;
			}
		}
		return null;
	}

	public int get_to_or_from()
	{
		return to_or_from;
	}

	public int[] getCharacterNum()
	{
		int[] count = { 0, 0 };
		for (int i = 0; i < passengerPlaner.Length; i++)
		{
			if (passengerPlaner[i] == null)
				continue;
			if (passengerPlaner[i].getType() == 0)
			{
				count[0]++;
			}
			else
			{
				count[1]++;
			}
		}
		return count;
	}

	public void reset()
	{
		passengerPlaner = new MyCharacterController[6];
	}
}

// 船只控制器
public class BoatController
{
	readonly GameObject boat;
	readonly MoveController moveableScript;
	readonly Vector3 fromPosition = new Vector3(5, 1, 0);
	readonly Vector3 toPosition = new Vector3(-5, 1, 0);
	readonly Vector3[] from_positions;
	readonly Vector3[] to_positions;

	int to_or_from;
	MyCharacterController[] passenger = new MyCharacterController[2];

	public BoatController()
	{
		to_or_from = 1;

		from_positions = new Vector3[] { new Vector3(4.5F, 1.5F, 0), new Vector3(5.5F, 1.5F, 0) };
		to_positions = new Vector3[] { new Vector3(-5.5F, 1.5F, 0), new Vector3(-4.5F, 1.5F, 0) };

		boat = Object.Instantiate(Resources.Load("Prefabs/Boat", typeof(GameObject)), fromPosition, Quaternion.identity, null) as GameObject;
		boat.name = "boat";

		moveableScript = boat.AddComponent(typeof(MoveController)) as MoveController;
		boat.AddComponent(typeof(ClickGUI));
	}

	public void Move()
	{
		if (to_or_from == -1)
		{
			moveableScript.setDestination(fromPosition);
			to_or_from = 1;
		}
		else
		{
			moveableScript.setDestination(toPosition);
			to_or_from = -1;
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

	public bool isEmpty()
	{
		for (int i = 0; i < passenger.Length; i++)
		{
			if (passenger[i] != null)
			{
				return false;
			}
		}
		return true;
	}

	public Vector3 getEmptyPosition()
	{
		Vector3 pos;
		int emptyIndex = getEmptyIndex();
		if (to_or_from == -1)
		{
			pos = to_positions[emptyIndex];
		}
		else
		{
			pos = from_positions[emptyIndex];
		}
		return pos;
	}

	public void GetOnBoat(MyCharacterController characterCtrl)
	{
		int index = getEmptyIndex();
		passenger[index] = characterCtrl;
	}

	public MyCharacterController GetOffBoat(string passenger_name)
	{
		for (int i = 0; i < passenger.Length; i++)
		{
			if (passenger[i] != null && passenger[i].getName() == passenger_name)
			{
				MyCharacterController charactorCtrl = passenger[i];
				passenger[i] = null;
				return charactorCtrl;
			}
		}
		return null;
	}

	public GameObject getGameobj()
	{
		return boat;
	}

	public int get_to_or_from()
	{
		return to_or_from;
	}

	public int[] getCharacterNum()
	{
		int[] count = { 0, 0 };
		for (int i = 0; i < passenger.Length; i++)
		{
			if (passenger[i] == null)
				continue;
			if (passenger[i].getType() == 0)
			{
				count[0]++;
			}
			else
			{
				count[1]++;
			}
		}
		return count;
	}

	public void reset()
	{
		moveableScript.reset();
		if (to_or_from == -1)
		{
			Move();
		}
		passenger = new MyCharacterController[2];
	}
}