using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour {
	//config params
	public float stoppingDistance = 1;
	public float walkSpeed = 1;
	public float inventorySize = 10;
	public float harvestInterval = 1;

	//state
	Vector2 targetPos;
	public int wood = 0;
	public int stone = 0;
	public string job;
	public bool doingJob = false;
	float lastHarvestTime;

	//cached component reference
	GameStatus gameStatus;
	Rigidbody2D rb;
	Animator animator;
	GameObject resourcesUI;
	Canvas canvas;
	GameObject jobSite;

	void Start () 
	{
		gameStatus = FindObjectOfType<GameStatus>();
		rb = gameObject.GetComponent<Rigidbody2D>();
		animator = gameObject.GetComponent<Animator>();
		canvas = FindObjectOfType<Canvas>();
	}
	
	void Update () 
	{
		Move();
		LoadUI();
		DoJob();
		Animate();
	}

	void OnMouseDown()
	{
		if(!gameStatus.SelectionContains(gameObject))
		{
			gameStatus.SelectObject(gameObject);
		}
	}

	public void StartJob(string jobType, GameObject site)
	{
		if(!doingJob)
		{
			job = jobType;
			jobSite = site;	
		}
	}

	void DoJob()
	{
		if(job != null && jobSite != null)
		{
			Vector2 raycastDirection = targetPos - (Vector2)transform.position;
			if(!doingJob)
			{
				if(raycastDirection.magnitude < stoppingDistance)
				{
					doingJob = true;	
				}
	     	}
	     	else
	     	{
	     		if(job == "Chop")
	     		{
	     			if(jobSite != null)
	     			{
	     				if(jobSite.GetComponent<Tree>().resourcesAvailible > 0)
	     				{
		     				if(!InventoryFull())
			     			{
			     				if(Time.realtimeSinceStartup - lastHarvestTime > harvestInterval)
			     				{
									lastHarvestTime = Time.realtimeSinceStartup;
					     			wood++;
					     			jobSite.GetComponent<Tree>().resourcesAvailible--;	
			     				}
			     			}	
			     			else
	     					{	
	     						doingJob = false;
	     						job = null;
	     						jobSite = null;
	     					}
	     				}	
	     			}
	     			else
	     			{
	     				doingJob = false;
	     				job = null;
	     				jobSite = null;
	     			}	
	     		}
	     		else if(job == "Mine")
	     		{
	     			if(jobSite != null)
	     			{
	     				if(jobSite.GetComponent<Rock>().resourcesAvailible > 0)
	     				{
		     				if(!InventoryFull())
			     			{
			     				if(Time.realtimeSinceStartup - lastHarvestTime > harvestInterval)
			     				{
									lastHarvestTime = Time.realtimeSinceStartup;
					     			stone++;
					     			jobSite.GetComponent<Rock>().resourcesAvailible--;	
			     				}
			     			}
			     			else
			     			{
			     				doingJob = false;
			     				job = null;
			     				jobSite = null;
			     			}		
	     				}
	     			}
	     			else
			     	{
			     		doingJob = false;
			     		job = null;
			     		jobSite = null;
			     	}	
	     		}
	     		else if(job == "Drop" && jobSite.GetComponent<StockPile>().resources < jobSite.GetComponent<StockPile>().maxResources - inventorySize)
	     		{
	     			if(!InventoryEmpty())
	     			{
	     				jobSite.GetComponent<StockPile>().wood += wood;	
	     				jobSite.GetComponent<StockPile>().stone += stone;	
	     				jobSite.GetComponent<StockPile>().resources += wood+stone;	
	     				gameStatus.wood += wood;
	     				gameStatus.stone += stone;
			     		ClearInventory();
	     			}
	     			else
	     			{
	     				doingJob = false;
	     				job = null;
	     				jobSite = null;
	     			}
	     		}
	     		else if(job == "EnterBuilding"/* && jobSite.GetComponent<House>().unitsInside.Count < jobSite.GetComponent<House>().maxUnits*/)	
	     		{
	     			if(doingJob)
	     			{
	     				doingJob = false;
	     				var houseUnit = Instantiate(gameStatus.peasant);
	     				houseUnit.name = gameObject.name;
	     				houseUnit.gameObject.SetActive(false);
	     				jobSite.GetComponent<House>().unitsInside.Add(houseUnit.gameObject);
	     				ClearUI();
	     				Destroy(gameObject);
	     			}
	     		}	
	     	}
		}
		else
		{
			doingJob = false;
		}
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

	void Animate()
	{
		animator.SetBool("DoingJob", doingJob);
		if(rb.velocity.magnitude > 0)
	    {
	        animator.SetBool("Walking", true);	
	    }
	    else
	    {
	        animator.SetBool("Walking", false);	
	    }
	}

	void Move()
	{
		if(jobSite == null)
		{
			SetTargetPos((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition));
		}
			Vector2 raycastDirection = targetPos - (Vector2)transform.position;

	        if (raycastDirection.magnitude < stoppingDistance)
	        {
	            rb.velocity = new Vector2(0,0);
	        }
	        else
	        {
	        	rb.AddForce(raycastDirection.normalized * walkSpeed);
	        }     
	}

	public void SetTargetPos(Vector2 pos)
	{
		if(Input.GetMouseButtonDown(1) && gameStatus.SelectionContains(gameObject))		
		{
			targetPos = pos;
		}	
	}

	void CreateUI()
	{
		resourcesUI = Instantiate(gameStatus.resourcesUI);
		resourcesUI.transform.Find("Wood").transform.Find("Number").GetComponent<Text>().text = wood.ToString();
		resourcesUI.transform.Find("Stone").transform.Find("Number").GetComponent<Text>().text = stone.ToString();
		resourcesUI.transform.SetParent(canvas.transform, false);
	}

	void UpdateUI()
	{
		resourcesUI.transform.Find("Wood").transform.Find("Number").GetComponent<Text>().text = wood.ToString();
		resourcesUI.transform.Find("Stone").transform.Find("Number").GetComponent<Text>().text = stone.ToString();
	}

	void ClearUI()
	{
		if(resourcesUI != null)
		{
			Destroy(resourcesUI);
		}
	}

	bool InventoryFull()
	{
		if(wood+stone < inventorySize)
		{
			return false;
		}
		else
		{
			return true;
		}
	}

	bool InventoryEmpty()
	{
		if(wood+stone > 0)
		{
			return false;
		}
		else
		{
			return true;
		}
	}

	void ClearInventory()
	{
		wood = 0;
		stone = 0;
	}

}
