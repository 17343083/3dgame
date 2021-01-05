using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour
{
	public Transform Sun;//太阳
	public Transform Mercury;//水星
	public Transform Venus;//金星
	public Transform EarthMoon;//地月系统
	public Transform Mars;//火星
	public Transform Jupiter;//木星
	public Transform Saturn;//土星
	public Transform Uranus;//天王星
	public Transform Neptune;//海王星

	void Start()
	{
		Sun.localPosition = Vector3.zero;
		Mercury.localPosition = new Vector3(6, 0, 0);
		Venus.localPosition = new Vector3(8, 0, 0);
		EarthMoon.localPosition = new Vector3(10, 0, 0);
		Mars.localPosition = new Vector3(12, 0, 0);
		Jupiter.localPosition = new Vector3(15, 0, 0);
		Saturn.localPosition = new Vector3(20, 0, 0);
		Uranus.localPosition = new Vector3(24, 0, 0);
		Neptune.localPosition = new Vector3(28, 0, 0);
	}

	void Update()
	{
		Vector3 a1 = new Vector3(0, -10, 5);
		Vector3 a2 = new Vector3(0, -10, 1);
		Vector3 a3 = Vector3.down;
		Vector3 a4 = new Vector3(0, -10, -5);
		Vector3 a5 = new Vector3(0, -10, -4);
		Vector3 a6 = new Vector3(0, -10, -3);
		Vector3 a7 = new Vector3(0, -10, -2);
		Vector3 a8 = new Vector3(0, -10, -1);

		Sun.Rotate(Vector3.down * 10 * Time.deltaTime);

		Mercury.RotateAround(Sun.position, a1, 50 * Time.deltaTime);
		Mercury.Rotate(Vector3.down * 10 * Time.deltaTime);

		Venus.RotateAround(Sun.position, a2, 30 * Time.deltaTime);
		Venus.Rotate(Vector3.up * 5 * Time.deltaTime);

		EarthMoon.RotateAround(Sun.position, a3, 20 * Time.deltaTime);

		Mars.RotateAround(Sun.position, a4, 10 * Time.deltaTime);
		Mars.Rotate(Vector3.down * 20 * Time.deltaTime);

		Jupiter.RotateAround(Sun.position, a5, 8 * Time.deltaTime);
		Jupiter.Rotate(Vector3.down * 30 * Time.deltaTime);

		Saturn.RotateAround(Sun.position, a6, 5 * Time.deltaTime);
		Saturn.Rotate(Vector3.down * 30 * Time.deltaTime);

		Uranus.RotateAround(Sun.position, a7, 3 * Time.deltaTime);
		Uranus.Rotate(Vector3.forward * 30 * Time.deltaTime);

		Neptune.RotateAround(Sun.position, a8, 1 * Time.deltaTime);
		Neptune.Rotate(Vector3.down * 30 * Time.deltaTime);
	}
}