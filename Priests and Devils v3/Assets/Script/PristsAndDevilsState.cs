using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PristsAndDevilsState
{
	public int leftPriests;
	public int rightPriests;
	public int leftDevils;
	public int rightDevils;
	public bool boat_pos;
	public PristsAndDevilsState parent_state;
	public PristsAndDevilsState() {}
	
	public PristsAndDevilsState(int leftPriests, int leftDevils, int rightPriests,
	  int rightDevils, bool boat_pos, PristsAndDevilsState parent_state)
	{
		this.leftPriests = leftPriests;
		this.rightPriests = rightPriests;
		this.leftDevils = leftDevils;
		this.rightDevils = rightDevils;
		this.boat_pos = boat_pos;
		this.parent_state = parent_state;
	}
	public PristsAndDevilsState(PristsAndDevilsState temp)
	{
		this.leftPriests = temp.leftPriests;
		this.rightPriests = temp.rightPriests;
		this.leftDevils = temp.leftDevils;
		this.rightDevils = temp.rightDevils;
		this.boat_pos = temp.boat_pos;
		this.parent_state = temp.parent_state;
	}
	public static bool operator ==(PristsAndDevilsState lhs, PristsAndDevilsState rhs)
	{
		return (lhs.leftPriests == rhs.leftPriests && lhs.rightPriests == rhs.rightPriests &&
		lhs.leftDevils == rhs.leftDevils && lhs.rightDevils == rhs.rightDevils &&
		lhs.boat_pos == rhs.boat_pos);
	}
	public static bool operator !=(PristsAndDevilsState lhs, PristsAndDevilsState rhs)
	{
		return !(lhs == rhs);
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (obj.GetType().Equals(this.GetType()) == false)
		{
			return false;
		}
		PristsAndDevilsState temp = (PristsAndDevilsState)obj;
		return this.leftPriests.Equals(temp.leftPriests) && this.rightPriests.Equals(temp.rightPriests) && this.rightDevils.Equals(temp.rightDevils)
			&& this.leftDevils.Equals(temp.leftDevils) && this.boat_pos.Equals(temp.boat_pos);
	}

	public override int GetHashCode()
	{
		return this.leftPriests.GetHashCode() + this.rightPriests.GetHashCode() + this.leftDevils.GetHashCode()
		   + this.rightDevils.GetHashCode() + this.boat_pos.GetHashCode();
	}
	public bool isValid()
	{
		return ((leftPriests == 0 || leftPriests >= leftDevils) && (rightPriests == 0 || rightPriests >= rightDevils));
	}

	public static PristsAndDevilsState BFS(PristsAndDevilsState start, PristsAndDevilsState end)
	{
		Queue<PristsAndDevilsState> found = new Queue<PristsAndDevilsState>();
		PristsAndDevilsState temp = new PristsAndDevilsState(start.leftPriests, start.leftDevils, start.rightPriests, start.rightDevils, start.boat_pos, null);
		found.Enqueue(temp);
		while (found.Count > 0)
		{
			temp = found.Peek();
			if (temp == end)
			{
				while (temp.parent_state != start)
				{
					temp = temp.parent_state;
				}
				return temp;
			}
			found.Dequeue();

			if (temp.boat_pos)
			{
				if (temp.leftPriests > 0)
				{
					PristsAndDevilsState next = new PristsAndDevilsState(temp);
					next.parent_state = new PristsAndDevilsState(temp);
					next.boat_pos = false;
					next.leftPriests--;
					next.rightPriests++;
					if (next.isValid() && !found.Contains(next))
					{
						found.Enqueue(next);
					}
				}
				if (temp.leftDevils > 0)
				{
					PristsAndDevilsState next = new PristsAndDevilsState(temp);
					next.parent_state = new PristsAndDevilsState(temp);
					next.boat_pos = false;
					next.leftDevils--;
					next.rightDevils++;
					if (next.isValid() && !found.Contains(next))
					{
						found.Enqueue(next);
					}
				}
				if (temp.leftDevils > 0 && temp.leftPriests > 0)
				{
					PristsAndDevilsState next = new PristsAndDevilsState(temp);
					next.parent_state = new PristsAndDevilsState(temp);
					next.boat_pos = false;
					next.leftDevils--;
					next.rightDevils++;
					next.leftPriests--;
					next.rightPriests++;
					if (next.isValid() && !found.Contains(next))
					{
						found.Enqueue(next);
					}
				}
				if (temp.leftPriests > 1)
				{
					PristsAndDevilsState next = new PristsAndDevilsState(temp);
					next.parent_state = new PristsAndDevilsState(temp);
					next.boat_pos = false;
					next.leftPriests -= 2;
					next.rightPriests += 2;
					if (next.isValid() && !found.Contains(next))
					{
						found.Enqueue(next);
					}
				}
				if (temp.leftDevils > 1)
				{
					PristsAndDevilsState next = new PristsAndDevilsState(temp);
					next.parent_state = new PristsAndDevilsState(temp);
					next.boat_pos = false;
					next.leftDevils -= 2;
					next.rightDevils += 2;
					if (next.isValid() && !found.Contains(next))
					{
						found.Enqueue(next);
					}
				}
			}
			else
			{
				if (temp.rightPriests > 0)
				{
					PristsAndDevilsState next = new PristsAndDevilsState(temp);
					next.parent_state = new PristsAndDevilsState(temp);
					next.boat_pos = true;
					next.rightPriests--;
					next.leftPriests++;
					if (next.isValid() && !found.Contains(next))
					{
						found.Enqueue(next);
					}
				}
				if (temp.rightDevils > 0)
				{
					PristsAndDevilsState next = new PristsAndDevilsState(temp);
					next.parent_state = new PristsAndDevilsState(temp);
					next.boat_pos = true;
					next.rightDevils--;
					next.leftDevils++;
					if (next.isValid() && !found.Contains(next))
					{
						found.Enqueue(next);
					}
				}
				if (temp.rightDevils > 0 && temp.rightPriests > 0)
				{
					PristsAndDevilsState next = new PristsAndDevilsState(temp);
					next.parent_state = new PristsAndDevilsState(temp);
					next.boat_pos = true;
					next.rightDevils--;
					next.leftDevils++;
					next.rightPriests--;
					next.leftPriests++;
					if (next.isValid() && !found.Contains(next))
					{
						found.Enqueue(next);
					}
				}
				if (temp.rightDevils > 1)
				{
					PristsAndDevilsState next = new PristsAndDevilsState(temp);
					next.parent_state = new PristsAndDevilsState(temp);
					next.boat_pos = true;
					next.rightDevils -= 2;
					next.leftDevils += 2;
					if (next.isValid() && !found.Contains(next))
					{
						found.Enqueue(next);
					}
				}
				if (temp.rightPriests > 1)
				{
					PristsAndDevilsState next = new PristsAndDevilsState(temp);
					next.parent_state = new PristsAndDevilsState(temp);
					next.boat_pos = true;
					next.rightPriests -= 2;
					next.leftPriests += 2;
					if (next.isValid() && !found.Contains(next))
					{
						found.Enqueue(next);
					}
				}
			}
		}
		return null;
	}
}