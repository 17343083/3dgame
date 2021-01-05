using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class board : MonoBehaviour
{
	public Text text;
	private int frame = 30;

	void Start()
	{
		text.gameObject.SetActive(false);
		Button btn = this.gameObject.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	IEnumerator fold()
	{
		float rx = 0;
		float xy = 50;
		for (int i = 0; i < frame; i++)
		{
			rx -= 50f / frame;
			xy -= 50f / frame;
			text.transform.rotation = Quaternion.Euler(rx, 0, 0);
			text.rectTransform.sizeDelta = new Vector2(text.rectTransform.sizeDelta.x, xy);
			if (i == frame - 1)
			{
				text.gameObject.SetActive(false);
			}
			yield return null;
		}
	}

	IEnumerator unfold()
	{
		float rx = -50;
		float xy = 0;
		for (int i = 0; i < frame; i++)
		{
			rx += 50f / frame;
			xy += 50f / frame;
			text.transform.rotation = Quaternion.Euler(rx, 0, 0);
			text.rectTransform.sizeDelta = new Vector2(text.rectTransform.sizeDelta.x, xy);
			if (i == 0)
				text.gameObject.SetActive(true);
			yield return null;
		}
	}
	void TaskOnClick()
	{
		if (text.gameObject.activeSelf)
			StartCoroutine(fold());
		else
			StartCoroutine(unfold());
	}
}