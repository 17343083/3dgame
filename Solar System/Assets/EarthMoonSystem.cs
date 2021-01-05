using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthMoonSystem : MonoBehaviour
{
	public Transform Earth;//地球
	public Transform Moon;//月亮

	void Start()
	{
		Earth.localPosition = Vector3.zero;
		Moon.localPosition = new Vector3(1, 0, 0);
	}

	void Update()
	{
		Vector3 a = Vector3.down;
		
		Earth.Rotate(Vector3.down * 20 * Time.deltaTime);

		Moon.RotateAround(Earth.position, a, 10 * Time.deltaTime);
		Moon.Rotate(Vector3.down * 10 * Time.deltaTime);
	}
}