using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, SceneController, UserAction
{
	// 用户界面
	UserGUI userGUI;
	// 两个河岸的控制器
	public BankController fromBank;
	public BankController toBank;
	// 船的控制器
	public BoatController boat;
	// 人物控制器
	private MyCharacterController[] characters;

	void Awake()
	{
		Director director = Director.getInstance();
		director.currentSceneController = this;
		userGUI = gameObject.AddComponent<UserGUI>() as UserGUI;
		// 新建六个人物
		characters = new MyCharacterController[6];
		loadResources();
	}

	public void loadResources()
	{
		// 加载水
		GameObject water = Instantiate(Resources.Load("Prefabs/Water", typeof(GameObject)), new Vector3(0, 0.5F, 0), Quaternion.identity, null) as GameObject;
		water.name = "water";
		// 加载河岸
		fromBank = new BankController("right");
		toBank = new BankController("left");
		// 加载船只
		boat = new BoatController();

		// 加载人物
		for (int i = 0; i < 3; i++)
		{
			MyCharacterController priest = new MyCharacterController("priest");
			priest.setName("priest" + i);
			priest.setPosition(fromBank.getEmptyPosition());
			priest.getOnBank(fromBank);
			fromBank.getOnBank(priest);
			characters[i] = priest;
		}
		for (int i = 0; i < 3; i++)
		{
			MyCharacterController devil = new MyCharacterController("devil");
			devil.setName("devil" + i);
			devil.setPosition(fromBank.getEmptyPosition());
			devil.getOnBank(fromBank);
			fromBank.getOnBank(devil);
			characters[i + 3] = devil;
		}
	}

	//移动船只
	public void moveBoat()
	{
		// 如果游戏结束，船只不能移动
		if (userGUI.state != 0) return;
		if (boat.isEmpty())
			return;
		boat.Move();
		userGUI.state = check();
	}

	//移动人物
	public void moveCharacter(MyCharacterController characterCtrl)
	{
		// 如果游戏结束，人物不能移动
		if (userGUI.state != 0) return;
		if (characterCtrl.isOnBoat())
		{
			BankController whichBank;
			if (boat.get_to_or_from() == -1)
				whichBank = toBank;
			else
				whichBank = fromBank;

			boat.GetOffBoat(characterCtrl.getName());
			characterCtrl.moveToPosition(whichBank.getEmptyPosition());
			characterCtrl.getOnBank(whichBank);
			whichBank.getOnBank(characterCtrl);

		}
		else
		{
			BankController whichBank = characterCtrl.getBankController();

			if (boat.getEmptyIndex() == -1)
				return;

			if (whichBank.get_to_or_from() != boat.get_to_or_from())
				return;

			whichBank.getOffBank(characterCtrl.getName());
			characterCtrl.moveToPosition(boat.getEmptyPosition());
			characterCtrl.getOnBoat(boat);
			boat.GetOnBoat(characterCtrl);
		}
		userGUI.state = check();
	}

	// 判断游戏是否结束
	// 0表示游戏进行中，1表示游戏失败，2表示游戏成功
	int check()
	{
		int from_priest = 0;
		int from_devil = 0;
		int to_priest = 0;
		int to_devil = 0;

		int[] fromCount = fromBank.getCharacterNum();
		from_priest += fromCount[0];
		from_devil += fromCount[1];

		int[] toCount = toBank.getCharacterNum();
		to_priest += toCount[0];
		to_devil += toCount[1];

		if (to_priest + to_devil == 6)
			return 2;

		int[] boatCount = boat.getCharacterNum();

		if (boat.get_to_or_from() == -1)
		{
			to_priest += boatCount[0];
			to_devil += boatCount[1];
		}
		else
		{
			from_priest += boatCount[0];
			from_devil += boatCount[1];
		}

		if (from_priest < from_devil && from_priest > 0)
		{
			return 1;
		}
		if (to_priest < to_devil && to_priest > 0)
		{
			return 1;
		}
		return 0;
	}

	// 重新开始
	public void restart()
	{
		boat.reset();
		fromBank.reset();
		toBank.reset();
		for (int i = 0; i < characters.Length; i++)
		{
			characters[i].reset();
		}
	}
}