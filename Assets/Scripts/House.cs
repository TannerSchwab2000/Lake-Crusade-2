using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class House : MonoBehaviour {
	//config params

	//state
	List<GameObject> unitsInside;

	//cached component reference
	GameStatus gameStatus;
	Canvas canvas;
	[SerializeField] GameObject UI;
	[SerializeField] GameObject unitIcon;
	GameObject currentUI;

	void Start () {
		gameStatus = FindObjectOfType<GameStatus>();
		canvas = FindObjectOfType<Canvas>();
	}
	
	void Update () {
		LoadUI();
	}

	void LoadUI()
	{
		if(gameStatus.SelectionContains(gameObject) && gameStatus.selection.Count == 1)
		{
			if(currentUI == null)
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

	void UpdateUI()
	{
		UpdateUnitListUI();
	}

	void UpdateUnitListUI()
	{
		ClearUnitListUI();
		if(unitsInside != null)
		{
			for(var i=0;i<unitsInside.Count;i++)
			{
				CreateUnitIcon(unitsInside[i].name, unitsInside[i].GetComponent<SpriteRenderer>().sprite);
			}	
		}	
	}

	void CreateUI()
	{
		if(currentUI == null)
		{
			currentUI = Instantiate(UI);
			currentUI.transform.SetParent(canvas.transform, false);
		}
	}

	void CreateUnitIcon(string name, Sprite sprite)
	{
		var newUnitIcon = Instantiate(unitIcon);
		newUnitIcon.transform.SetParent(currentUI.transform.Find("UnitList").transform, false);
		newUnitIcon.name = name;
		newUnitIcon.transform.Find("Image").GetComponent<Image>().sprite = sprite;
	}

	void ClearUI()
	{
		if(currentUI != null)
		{
			Destroy(currentUI);
		}
	}

	void ClearUnitListUI()
	{
		if(currentUI != null)
		{
			if(currentUI.transform.Find("UnitList").childCount > 0)
			{
				for(var i=0;i<currentUI.transform.Find("UnitList").childCount;i++)
				{
					Destroy(currentUI.transform.Find("UnitList").GetChild(i).gameObject);
				}
			}
		}
	}

	void OnMouseDown()
	{
		if(!gameStatus.SelectionContains(gameObject))
		{
			gameStatus.SelectObject(gameObject);
		}
	}
}
