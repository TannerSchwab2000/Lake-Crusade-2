using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameStatus : MonoBehaviour {
	//config params
	public float cameraSpeed = 0.3f;

	//state
	bool controlPressed;
	public int wood;
	public int stone;
	GameObject currentBuildOptions;
	GameObject currentBuilding;
	public List<GameObject> selection;
	public List<GameObject> selectionIcons;
	Vector3 cameraVelocity;
	Vector3 cameraPos;

	//cached component reference
	Canvas canvas;
	[SerializeField] GameObject selectionIcon;
	[SerializeField] public GameObject resourcesUI;
	[SerializeField] public GameObject stockpiledResourcesUI;
	[SerializeField] public GameObject buildOptionsUI;
	[SerializeField] public GameObject stockpile;
	[SerializeField] public GameObject house;
	[SerializeField] public GameObject peasant;
	[SerializeField] public GameObject slave;
	Camera camera;

	void Start () 
	{
		canvas = FindObjectOfType<Canvas>();
		camera = Camera.main;
	}
	
	void FixedUpdate () 
	{
		LoadUI();
	}

	void Update()
	{
		PlaceBuilding();
		MoveCamera();
	}

	void LoadUI()
	{
		DisplaySelection();
		ClearSelection();
		DisplayResources();
		DisplayBuildOptions();
		DisplayCurrentBuilding();
	}

	void MoveCamera()
	{
		cameraPos += cameraVelocity;
		cameraPos.z = -10;
		camera.transform.position = cameraPos;
		if(Input.anyKey)
		{
			if(Input.GetKeyDown(KeyCode.RightArrow))
			{
				cameraVelocity = new Vector3(cameraSpeed,0,0);
			}
			else if(Input.GetKeyDown(KeyCode.LeftArrow))
			{
				cameraVelocity = new Vector3(-cameraSpeed,0,0);
			}
			else if(Input.GetKeyDown(KeyCode.UpArrow))
			{
				cameraVelocity = new Vector3(0,cameraSpeed,0);
			}
			else if(Input.GetKeyDown(KeyCode.DownArrow))
			{
				cameraVelocity = new Vector3(0,-cameraSpeed,0);
			}	
		}
		else
		{
			cameraVelocity = new Vector2(0,0);
		}
	}

	public void SelectObject(GameObject selectedObject)
	{
		selection.Add(selectedObject);
	}

	public bool SelectionContains(GameObject referencedGameObject)
	{
		for(var i=0;i<selection.Count;i++)
		{
			if(selection[i] == referencedGameObject)
			{
				return true;
			}
		}
		return false;
	}

	void ClearSelection()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			for(var i=0;i<selectionIcons.Count;i++)
			{
				Destroy(selectionIcons[i]);
			}
			selection.Clear();
			selectionIcons.Clear();
		}
	}

	public void ClearSelectionIcons()
	{
		for(var i=0;i<selectionIcons.Count;i++)
		{
			Destroy(selectionIcons[i]);
		}
		selectionIcons.Clear();
	}


	void DisplaySelection()
	{
		for(var i=0;i<selection.Count;i++)
		{
			if(selection[i] != null)
			{
				var currentName = selection[i].name;	
				var currentSprite = selection[i].GetComponent<SpriteRenderer>().sprite;
				if(!SelectionIconsContains(currentName))
				{
					CreateSelectionIcon(currentName, currentSprite, selection[i]);	
				}
				else
				{
					UpdateSelectionIcon(currentName);
				}	
			}
		}
	}

	void DisplayCurrentBuilding()
	{
		if(currentBuilding != null)
		{
			currentBuilding.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}
	}

	void DisplayBuildOptions()
	{
		if(SelectionIconsContains("Peasant"))
		{
			if(!BuildOptionsDisplayed())
			{
				CreateBuildOptionsUI();	
			}
			else
			{
				UpdateBuildOptionsUI();
			}
		}
		else
		{
			ClearBuildOptionsUI();
		}
	}

	void DisplayResources()
	{
		if(!ResourcesDisplayed())
		{
			CreateResourcesUI();
		}
		else
		{
			UpdateResourcesUI();
		}
	}

	public void RemoveSelection(string name)
	{
		for(var i=0;i<selection.Count;i++)
		{
			if(selection[i] != null)
			{
				if(selection[i].name == name)
				{
					selection.RemoveAt(i);
				}
			}
		}
	}

	public int GetSelectionIconIndex(string name)
	{
		for(var i=0;i<selectionIcons.Count;i++)
		{
			if(selectionIcons[i] != null)
			{
				if(selectionIcons[i].transform.GetChild(0).name == name)
				{
					return i;
				}	
			}	
		}
		return 0;
	}

	int GetSelectionIndex(GameObject referencedGameObject)
	{
		for(var i=0;i<selection.Count;i++)
		{
			if(selection[i] == referencedGameObject)
			{
				return i;
			}
		}
		return 0;
	}

	GameObject GetSelectionIcon(string name)
	{
		for(var i=0;i<selectionIcons.Count;i++)
		{
			if(selectionIcons[i] != null)
			{
				if(selectionIcons[i].transform.GetChild(0).name == name)
				{
					return selectionIcons[i];
				}	
			}	
		}
		return null;
	}

	GameObject GetSelection(string name)
	{
		for(var i=0;i<selection.Count;i++)
		{
			if(selection[i] != null)
			{
				if(selection[i].name == name)
				{
					return selection[i];
				}	
			}
		}
		return null;
	}

	public void Build(string name)
	{
		if(currentBuilding == null)
		{
			if(name == "Stockpile")
			{
				currentBuilding = Instantiate(stockpile);
				currentBuilding.name = "StockPile";
			}
			else if(name == "House")
			{
				currentBuilding = Instantiate(house);
				currentBuilding.name = "House";
			}	
		}
		
	}

	public void PlaceBuilding()
	{
		if(Input.GetMouseButtonDown(0) && currentBuilding != null)
		{
			currentBuilding.transform.position += new Vector3(0,0,-1);
			currentBuilding = null;
		}	
	}

	void CreateBuildOptionsUI()
	{
		currentBuildOptions = Instantiate(buildOptionsUI);
		currentBuildOptions.name = "BuildOptions";
		currentBuildOptions.transform.SetParent(canvas.transform, false);
		UpdateBuildOptionsUI();
	}

	void UpdateBuildOptionsUI()
	{
		if(currentBuildOptions != null)
		{
			for(var i=0;i<currentBuildOptions.transform.childCount;i++)
			{
				if(!SufficentResourcesForCurrentBuildOptions(i))
				{
					Destroy(currentBuildOptions.transform.GetChild(i).gameObject);
				}
			}	
		}
		if(buildOptionsUI != null)
		{
			for(var i=0;i<buildOptionsUI.transform.childCount;i++)
			{
				if(SufficentResourcesForBuildOptionsUI(i) && !CurrentBuildOptionsContains(buildOptionsUI.transform.GetChild(i).name))
				{
					var newBuildOption = Instantiate(buildOptionsUI.transform.GetChild(i));
					newBuildOption.transform.SetParent(currentBuildOptions.transform, false);
					newBuildOption.name = buildOptionsUI.transform.GetChild(i).name;
				}
			}	
		}
			
	}

	void ClearBuildOptionsUI()
	{
		Destroy(currentBuildOptions);
	}

	bool CurrentBuildOptionsContains(string name)
	{
		if(currentBuildOptions != null)
		{
			if(currentBuildOptions.transform.Find(name) != null)
			{
				return true;
			}			
		}
		return false;
	}

	bool SufficentResourcesForBuildOptionsUI(int index)
	{
		bool enoughWood = true;
		bool enoughStone = true;
		if(buildOptionsUI.transform.GetChild(index).transform.Find("WoodCost") != null)
		{
			int woodCost = int.Parse(buildOptionsUI.transform.GetChild(index).transform.Find("WoodCost").GetComponent<Text>().text);
			if(woodCost > wood)
			{
				enoughWood = false;
			}	
		}
		if(buildOptionsUI.transform.GetChild(index).transform.Find("StoneCost") != null)
		{
			int stoneCost = int.Parse(buildOptionsUI.transform.GetChild(index).transform.Find("StoneCost").GetComponent<Text>().text);
			if(stoneCost > stone)
			{
				enoughStone = false;
			}	
		}
		if(enoughWood && enoughStone)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	bool SufficentResourcesForCurrentBuildOptions(int index)
	{
		bool enoughWood = true;
		bool enoughStone = true;
		if(currentBuildOptions.transform.GetChild(index).transform.Find("WoodCost") != null)
		{
			int woodCost = int.Parse(currentBuildOptions.transform.GetChild(index).transform.Find("WoodCost").GetComponent<Text>().text);
			if(woodCost > wood)
			{
				enoughWood = false;
			}	
		}
		if(currentBuildOptions.transform.GetChild(index).transform.Find("StoneCost") != null)
		{
			int stoneCost = int.Parse(currentBuildOptions.transform.GetChild(index).transform.Find("StoneCost").GetComponent<Text>().text);
			if(stoneCost > stone)
			{
				enoughStone = false;
			}	
		}
		if(enoughWood && enoughStone)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	void CreateResourcesUI()
	{
		var resources = Instantiate(stockpiledResourcesUI);
		resources.name = "StockpiledResources";
		resources.transform.SetParent(canvas.transform, false);
		UpdateResourcesUI();
	}

	void UpdateResourcesUI()
	{
		canvas.transform.Find("StockpiledResources").transform.Find("Wood").transform.Find("Number").GetComponent<Text>().text = wood.ToString();
		canvas.transform.Find("StockpiledResources").transform.Find("Stone").transform.Find("Number").GetComponent<Text>().text = stone.ToString();
	}

	void UpdateSelectionIcon(string name)
	{
		GetSelectionIcon(name).transform.GetChild(0).Find("Number").GetComponent<Text>().text = "x" + CountSelectionWithName(name).ToString();
		GetSelectionIcon(name).transform.GetComponent<RectTransform>().localPosition = new Vector2(-612 + GetSelectionIconIndex(name)*100, -449);		
	}

	void CreateSelectionIcon(string name, Sprite sprite, GameObject representedObject)
	{
		var currentSelectionIcon = Instantiate(selectionIcon);
		selectionIcons.Add(currentSelectionIcon);
		currentSelectionIcon.transform.GetChild(0).name = name;
		currentSelectionIcon.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
		currentSelectionIcon.transform.SetParent(canvas.transform, false);
		currentSelectionIcon.transform.GetChild(0).Find("Number").GetComponent<Text>().text = "x" + CountSelectionWithName(name).ToString();	
		currentSelectionIcon.transform.GetComponent<RectTransform>().localPosition = new Vector2(-612 + (selectionIcons.Count-1)*100, -449);
		currentSelectionIcon.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(representedObject.GetComponent<SpriteRenderer>().bounds.size.x, representedObject.GetComponent<SpriteRenderer>().bounds.size.y);
		currentSelectionIcon.GetComponent<SelectionIcon>().representedObject = representedObject;
		FixSize(currentSelectionIcon);
	}

	void FixSize(GameObject currentSelectionIcon)
	{
		while(currentSelectionIcon.transform.GetChild(0).GetComponent<RectTransform>().localScale.x > currentSelectionIcon.transform.GetComponent<RectTransform>().localScale.x || currentSelectionIcon.transform.GetChild(0).GetComponent<RectTransform>().localScale.y > currentSelectionIcon.transform.GetComponent<RectTransform>().localScale.y)
		{
			Vector2 size = currentSelectionIcon.transform.GetChild(0).GetComponent<RectTransform>().localScale;
			Vector2 newSize = size;
			newSize.x *= 0.99f;
			newSize.y *= 0.99f;
			currentSelectionIcon.transform.GetChild(0).GetComponent<RectTransform>().localScale = newSize;
		}
	}	

	int CountSelectionWithName(string name)
	{
		int count = 0;
		for(var i=0;i<selection.Count;i++)
		{
			if(selection[i] != null)
			{
				if(selection[i].name == name)
				{
					count++;
				}	
			}
		}
		return count;
	}

	bool SelectionIconsContains(string name)
	{
		for(var i=0;i<selectionIcons.Count;i++)
		{
			if(selectionIcons[i] != null)
			{
				if(selectionIcons[i].transform.GetChild(0).name == name)
				{
					return true;
				}	
			}
		}
		return false;
	}

	bool ResourcesDisplayed()
	{
		if(canvas.transform.Find("StockpiledResources") != null)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	bool BuildOptionsDisplayed()
	{
		if(canvas.transform.Find("BuildOptions") != null)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
