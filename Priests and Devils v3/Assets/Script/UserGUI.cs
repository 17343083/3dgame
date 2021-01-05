using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
	private UserAction action;
	public int status = 0;
	GUIStyle style;
	GUIStyle style2;
	GUIStyle buttonStyle;
	public bool show = false;
	private PristsAndDevilsState start;
	private PristsAndDevilsState end;
	public CoastController coastController = new CoastController("both");
	public int leftPriests = 3;
	public int leftDevils = 3;
	public int rightPriests = 0;
	public int rightDevils = 0;
	public bool boat_pos = true;

	private string tips = "";

	void Start()
	{
		action = Director.getInstance().sceneController as UserAction;
		style = new GUIStyle();
		style.fontSize = 15;
		style.alignment = TextAnchor.MiddleLeft;
		style2 = new GUIStyle();
		style2.fontSize = 30;
		style2.alignment = TextAnchor.MiddleCenter;
		end = new PristsAndDevilsState(0, 0, 3, 3, false, null);
	}

	void OnGUI()
	{
		if (status == -1)
		{
			GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2, 100, 50), "Gameover!", style2);
		}
		else if (status == 1)
		{
			GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2, 100, 50), "Win!", style2);
		}
		buttonStyle = new GUIStyle("button");
		buttonStyle.fontSize = 15;
		if (GUI.Button(new Rect(Screen.width / 2 - 50, 20, 100, 50), "Restart", buttonStyle))
		{
			status = 0;
			action.restart();
		}
		GUI.Label(new Rect(Screen.width / 2 + 100, Screen.height / 2 - 150, 100, 50), tips, style);
		if (GUI.Button(new Rect(Screen.width / 2 - 50, 80, 100, 50), "Tips", buttonStyle))
		{
			int[] arr = action.getNum();
			leftPriests = arr[0];
			leftDevils = arr[1];
			rightPriests = arr[2];
			rightDevils = arr[3];
			if (arr[4] == 0) { boat_pos = true; }
			else { boat_pos = false; }
			start = new PristsAndDevilsState(leftPriests, leftDevils, rightPriests, rightDevils, boat_pos, null);
			PristsAndDevilsState temp = PristsAndDevilsState.BFS(start, end);
			int movePriests = leftPriests - temp.leftPriests;
			int moveDevils = leftDevils - temp.leftDevils;

			if (movePriests == 2 || movePriests == -2)
			{
				tips = "移动两位牧师";
			}
			else if (moveDevils == 2 || moveDevils == -2)
			{
				tips = "移动两位魔鬼";
			}
			else if (movePriests == 1 || movePriests == -1)
			{
				if (moveDevils == 0)
					tips = "移动一位牧师";
				else
					tips = "移动一位牧师和一位魔鬼";
			}
			else
			{
				tips = "移动一位魔鬼";
			}
		}
	}
}
