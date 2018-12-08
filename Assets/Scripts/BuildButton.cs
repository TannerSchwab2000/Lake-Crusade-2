using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class BuildButton : MonoBehaviour {

	//config params

	//state
	public List<GameObject> stockpiles;
	Scene scene;

	//cached component reference
	GameStatus gameStatus;

	void Start () {
		gameStatus = FindObjectOfType<GameStatus>();
		scene = SceneManager.GetActiveScene();
	}
	
	void Update () {
		
	}

	public void ButtonClicked()
	{
		UseResources();
		gameStatus.Build(gameObject.name);
		Destroy(gameObject);
	}

	void UseResources()
	{
		if(transform.Find("WoodCost") != null)
		{
			gameStatus.wood -= int.Parse(transform.Find("WoodCost").GetComponent<Text>().text);
			RemoveResourcesFromStockpile("wood", int.Parse(transform.Find("WoodCost").GetComponent<Text>().text));
		}
		if(transform.Find("StoneCost") != null)
		{
			gameStatus.stone -= int.Parse(transform.Find("StoneCost").GetComponent<Text>().text);	
			RemoveResourcesFromStockpile("stone", int.Parse(transform.Find("StoneCost").GetComponent<Text>().text));
		}
	}

	void RemoveResourcesFromStockpile(string type, int number)
	{
		UpdateStockpileList();
		bool resourcesRemoved = false;
		for(var i=0;i<stockpiles.Count;i++)
		{
			if(resourcesRemoved == false)
			{
				if(type == "wood")
				{
					if(stockpiles[i].GetComponent<StockPile>().wood >= number)
					{
						stockpiles[i].GetComponent<StockPile>().wood -= number;
						resourcesRemoved = true;
					}
				}	
				else if(type == "stone")
				{
					if(stockpiles[i].GetComponent<StockPile>().stone >= number)
					{
						stockpiles[i].GetComponent<StockPile>().stone -= number;
						resourcesRemoved = true;
					}
				}
			}
			else
			{
				return;
			}
		}
	}

	void UpdateStockpileList()
	{
		var gameObjects = scene.GetRootGameObjects();
		stockpiles.Clear();
		for(var i=0;i<gameObjects.Length;i++)
		{
			if(gameObjects[i].name == "StockPile")
			{
				stockpiles.Add(gameObjects[i]);
			}
		}	
	}
}
