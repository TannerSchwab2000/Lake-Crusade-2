using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Resource {

	//config params

	//state

	//cached component reference

	void Start () {
		base.Start();
	}
	
	void Update () {
		base.Update();
	}

	void OnMouseOver()
	{
		if(Input.GetMouseButtonDown(1))
		{
			for(var i=0;i<gameStatus.selection.Count;i++)
			{
				if(gameStatus.selection[i].name == "Peasant" && !gameStatus.selection[i].GetComponent<Unit>().doingJob)
				{
					gameStatus.selection[i].GetComponent<Unit>().StartJob("Chop", gameObject);	
					gameStatus.selection[i].GetComponent<Unit>().SetTargetPos((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition));
				}
			}		
		}
	}
}
