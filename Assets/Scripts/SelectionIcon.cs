using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionIcon : MonoBehaviour {

	//config params

	//state

	//cached component reference
	GameStatus gameStatus;

	void Start () {
		gameStatus = FindObjectOfType<GameStatus>();
	}
	
	void Update () {
		
	}

	public void ButtonClicked()
	{
		gameStatus.RemoveSelection(gameObject.transform.GetChild(0).name);
		Destroy(gameObject);
	}


}
