using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
	private UserAction action;
	// state变量表示游戏状态，其中0表示游戏进行中，1表示游戏失败，2表示游戏成功
	public int state = 0;
	GUIStyle style1;
	GUIStyle style2;
	GUIStyle buttonStyle;

	void Start()
	{
		action = Director.getInstance().currentSceneController as UserAction;
		style1 = new GUIStyle();
		style1.fontSize = 40;
		style1.alignment = TextAnchor.MiddleCenter;
		style2 = new GUIStyle();
		style2.fontSize = 20;
		style2.alignment = TextAnchor.MiddleCenter;
		buttonStyle = new GUIStyle("button");
		buttonStyle.fontSize = 30;
		buttonStyle.alignment = TextAnchor.MiddleCenter;
	}

	// 用户交互界面
	void OnGUI()
	{
		// 游戏进行中界面
		if(state==0)
		{
			GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 80, 100, 50), "绿色方块表示牧师，红色球表示魔鬼", style2);
		}
		// 游戏失败界面
		else if (state == 1)
		{
			GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 80, 100, 50), "Gameover", style1);
			if (GUI.Button(new Rect(Screen.width / 2 - 60, Screen.height / 2, 120, 50), "Restart", buttonStyle))
			{
				state = 0;
				action.restart();
			}
		}
		// 游戏成功界面
		else if (state == 2)
		{
			GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 80, 100, 50), "Win", style1);
			if (GUI.Button(new Rect(Screen.width / 2 - 60, Screen.height / 2, 120, 50), "Restart", buttonStyle))
			{
				state = 0;
				action.restart();
			}
		}
	}
}