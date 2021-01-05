using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRecorder : MonoBehaviour
{
	private float score;

	public float getScore()
	{
		return score;
	}

	public void Record(GameObject disk)
	{
		score += (100 - disk.GetComponent<DiskData>().size * (20 - disk.GetComponent<DiskData>().speed));
	}

	public void Reset()
	{
		score = 0;
	}
}
