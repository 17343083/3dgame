using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
	private IUserAction action;
	private float width, height;
	private string countDownTitle;

	void Start()
	{
		countDownTitle = "Start";
		action = SSDirector.getInstance().currentScenceController as IUserAction;
	}

	float castw(float scale)
	{
		return (Screen.width - width) / scale;
	}

	float casth(float scale)
	{
		return (Screen.height - height) / scale;
	}

	void OnGUI()
	{
		width = Screen.width / 12;
		height = Screen.height / 12;

		GUI.Button(new Rect(10, 60, 80, 30), "Round " + ((RoundController)SSDirector.getInstance().currentScenceController).getRound());
		GUI.Button(new Rect(10, 90, 80, 30), "Time: " + ((RoundController)SSDirector.getInstance().currentScenceController).count.ToString());
		GUI.Button(new Rect(10, 120, 80, 30), "Score:" + ((RoundController)SSDirector.getInstance().currentScenceController).scoreRecorder.getScore().ToString());

		if (GUI.Button(new Rect(10, 10, 80, 50), countDownTitle))
		{
			if (countDownTitle == "Start")
			{
				countDownTitle = "Restart";
				SSDirector.getInstance().currentScenceController.Resume();
			}
			else
			{
				SSDirector.getInstance().currentScenceController.Restart();
			}
		}

		if (SSDirector.getInstance().currentScenceController.state == State.WIN)
		{
			if (GUI.Button(new Rect(castw(2f), casth(6f), width, height), "Win!"))
			{
				SSDirector.getInstance().currentScenceController.Restart();
			}
		}
		else if (SSDirector.getInstance().currentScenceController.state == State.LOSE)
		{
			if (GUI.Button(new Rect(castw(2f), casth(6f), width, height), "Lose!"))
			{
				SSDirector.getInstance().currentScenceController.Restart();
			}
		}
	}
	void Update()
	{
		action.shoot();
	}
}