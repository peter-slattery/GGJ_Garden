using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Animal : MonoBehaviour
{
    public enum AI_Type { Static, Homebody, Wanderer }
    
    public float Speed = 5.0f;
    public float WanderMinUpdateTime = 3.0f;
    public float WanderMaxUpdateTime = 7.0f;
    public float WanderMinDistance = 3.0f;
    public float WanderMaxDistance = 10.0f;
    // Note for Wander variables: the shorter the min/max distance and time, the more crazy the animal will wander

    public TileTypeController.TileType AttractedTile; // TODO: eventually make this a List
    public AI_Type m_AIType;

    Vector3 InitialPosition;
    Vector3 DestinationPostion;
    bool canMoveAgain = true;

    void Awake()
    {
        InitialPosition = transform.position;
        DestinationPostion = InitialPosition;
    }

    void Update()
    {
        // Get Destination
        if (canMoveAgain && m_AIType == AI_Type.Homebody)
        {
            StartCoroutine("ChooseNewDestination");
        }
        else if (canMoveAgain && m_AIType == AI_Type.Wanderer)
        {
            StartCoroutine("GoToAttracted");
        }

        // Move to Destination
        if ((transform.position - DestinationPostion) != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, DestinationPostion, Speed * Time.deltaTime);
        }
    }

    IEnumerator GoToAttracted()
    {
        canMoveAgain = false;

        if (AttractedTile != null)
        {
            // to turn grid to world position do xy to xz of vector3
            List<Tile> attractedTiles = GridController.getCurInstance().QueryForTileType(AttractedTile);

            // Pick a random number between min and max of the attractedTiles
            int randomIndex = Random.Range(0, (attractedTiles.Count-1));

            // Go to that tile
			Vector3 tilePos = GridController.GridToWorld(attractedTiles[randomIndex].addr);
            DestinationPostion = tilePos;

            //Debug.Log(tempPos);

            //for (var i=0; i< attractedTiles.Count; i++)
            //{
            //    GridController.GridToWorld(attractedTiles[i].addr);
            //    Debug.Log(GridController.GridToWorld(attractedTiles[i].addr));
            //}
        }

        float waitTime = Random.Range(WanderMinUpdateTime, WanderMaxUpdateTime);
        yield return new WaitForSeconds(waitTime);
        canMoveAgain = true;

    }    

    IEnumerator ChooseNewDestination()
    {
        canMoveAgain = false;

        ChooseWanderDestination();
        float waitTime = Random.Range(WanderMinUpdateTime, WanderMaxUpdateTime);

        yield return new WaitForSeconds(waitTime);

        canMoveAgain = true;
    }

    void ChooseWanderDestination()
    {
        float x = Random.Range(WanderMinDistance, WanderMaxDistance);
        float z = Random.Range(WanderMinDistance, WanderMaxDistance);
        Vector3 movePosition = new Vector3(x, 0, z);
        DestinationPostion = InitialPosition + movePosition;
    }
}
