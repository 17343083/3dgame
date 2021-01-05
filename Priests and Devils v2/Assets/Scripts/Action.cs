using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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