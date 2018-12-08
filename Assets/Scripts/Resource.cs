using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resource : MonoBehaviour {
	//config params
	public int resourcesAvailible = 10;

	//state

	//cached component reference
	protected GameStatus gameStatus;
	GameObject resourcesUI;
	[SerializeField] GameObject UI;
	Canvas canvas;

	protected void Start () {
		gameStatus = FindObjectOfType<GameStatus>();
		canvas = FindObjectOfType<Canvas>();
	}
	
	void FixedUpdate()
	{
		if(resourcesAvailible < 1)
		{
			Destroy(gameObject);
		}
	}

	protected void Update () {
	    LoadUI();
	}

	void LoadUI()
	{
		if(gameStatus.SelectionContains(gameObject) && gameStatus.selection.Count == 1)
		{
			if(resourcesUI == null)
			{
				CreateUI();
			}
			else
			{
				UpdateUI();
			}
		}
		else
		{
			ClearUI();
		}
	}

	void OnMouseDown()
	{
		gameStatus = FindObjectOfType<GameStatus>();
		if(!gameStatus.SelectionContains(gameObject))
		{
			gameStatus.SelectObject(gameObject);
		}
	}

	void CreateUI()
	{
		resourcesUI = Instantiate(UI);
		resourcesUI.transform.SetParent(canvas.transform, false);
		UpdateUI();
	}

	void UpdateUI()
	{
		if(resourcesUI.transform.Find("Stone") != null)
		{
			resourcesUI.transform.Find("Stone").transform.Find("Number").GetComponent<Text>().text = "x" + resourcesAvailible;
		}
		if(resourcesUI.transform.Find("Wood") != null)
		{
			resourcesUI.transform.Find("Wood").transform.Find("Number").GetComponent<Text>().text = "x" + resourcesAvailible;
		}
	}

	void ClearUI()
	{
		if(resourcesUI != null)
		{
			Destroy(resourcesUI);
		}
	}
}
