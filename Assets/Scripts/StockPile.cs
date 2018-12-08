using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockPile : MonoBehaviour {
	//config params
	public int maxResources = 100;

	//state
	public int wood;
	public int stone;
	public int resources;

	//cached component reference
	GameStatus gameStatus;
	[SerializeField] Sprite[] woodSprites;
	[SerializeField] Sprite[] stoneSprites;

	void Start () {
		gameStatus = FindObjectOfType<GameStatus>();
	}
	
	void Update () {
		Animate();
	}

	void Animate()
	{
		if(wood == 0)
		{
			transform.Find("Wood").GetComponent<SpriteRenderer>().sprite = woodSprites[0];
		}
		else if(wood > 0 && wood < 19)
		{
			transform.Find("Wood").GetComponent<SpriteRenderer>().sprite = woodSprites[1];
		}
		else if(wood > 19 && wood < 29)
		{
			transform.Find("Wood").GetComponent<SpriteRenderer>().sprite = woodSprites[2];
		}
		else if(wood > 29)
		{
			transform.Find("Wood").GetComponent<SpriteRenderer>().sprite = woodSprites[3];
		}

		if(stone == 0)
		{
			transform.Find("Stone").GetComponent<SpriteRenderer>().sprite = stoneSprites[0];
		}
		else if(stone > 0 && stone < 19)
		{
			transform.Find("Stone").GetComponent<SpriteRenderer>().sprite = stoneSprites[1];
		}
		else if(stone > 19 && stone < 29)
		{
			transform.Find("Stone").GetComponent<SpriteRenderer>().sprite = stoneSprites[2];
		}
		else if(stone > 29)
		{
			transform.Find("Stone").GetComponent<SpriteRenderer>().sprite = stoneSprites[3];
		}
	}

	void OnMouseOver()
	{
		if(Input.GetMouseButtonDown(1))
		{
			for(var i=0;i<gameStatus.selection.Count;i++)
			{
				if(!gameStatus.selection[i].GetComponent<Unit>().doingJob)
				{
					gameStatus.selection[i].GetComponent<Unit>().StartJob("Drop", gameObject);	
					gameStatus.selection[i].GetComponent<Unit>().SetTargetPos((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition));	
				}
			}		
		}
	}
}
