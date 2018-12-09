using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionIcon : MonoBehaviour {

	//config params

	//state

	//cached component reference
	GameStatus gameStatus;
	public GameObject representedObject;

	void Start () {
		gameStatus = FindObjectOfType<GameStatus>();
	}
	
	void Update () {
		
	}

	public void ButtonClicked()
	{
		Destroy(gameStatus.selectionIcons[gameStatus.GetSelectionIconIndex(transform.GetChild(0).name)]);
		gameStatus.selectionIcons.RemoveAt(gameStatus.GetSelectionIconIndex(gameStatus.selectionIcons[gameStatus.GetSelectionIconIndex(transform.GetChild(0).name)].name));
	}


}
