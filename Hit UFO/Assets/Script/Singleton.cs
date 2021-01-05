using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : MonoBehaviour
{
	private static T instance;
	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				instance = (T)Object.FindObjectOfType(typeof(T));
			}
			return instance;
		}
	}
}