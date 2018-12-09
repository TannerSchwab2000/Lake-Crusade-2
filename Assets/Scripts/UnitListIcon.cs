using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitListIcon : MonoBehaviour {
	//config params

	//state

	//cached component reference
	GameStatus gameStatus;
	public GameObject house;
	public int index;

	void Start () {
		gameStatus = FindObjectOfType<GameStatus>();
	}
	
	void Update () {
		
	}

	public void ButtonClicked()
	{
		if(gameObject.name == "Peasant")
		{
			var newUnit = Instantiate(gameStatus.peasant);
			newUnit.transform.position = house.transform.position;
			newUnit.name = "Peasant";
			Destroy(house.GetComponent<House>().unitsInside[index]);	
		}
		else if(gameObject.name == "Slave")
		{
			var newUnit = Instantiate(gameStatus.slave);
			newUnit.transform.position = house.transform.position;
			newUnit.name = "Slave";
			Destroy(house.GetComponent<House>().unitsInside[index]);	
		}
		Destroy(gameObject);
	} 
}
