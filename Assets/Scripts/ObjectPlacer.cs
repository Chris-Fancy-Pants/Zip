using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer: MonoBehaviour {

    public Transform leftEdge;
    public Transform rightEdge;

    public Transform objectHolder;

    public GameObject[] objectsToPlace;

    public float minPlacementDistance = 0.5f;
    public float maxPlacementDistance = 4f;

    public float minScaleModifier = 0.7f;
    public float maxScaleModifier = 1.5f;
  
	// Use this for initialization
	public void PlaceObjects() {

        float treePos = leftEdge.position.x;



        while(treePos < rightEdge.position.x)
        {

            treePos += Random.Range(minPlacementDistance, maxPlacementDistance);

            if (treePos < rightEdge.position.x)
            {
                int randomTree = Random.Range(0, objectsToPlace.Length);
                GameObject obj = Instantiate(objectsToPlace[randomTree], new Vector2(treePos, transform.position.y), Quaternion.identity) as GameObject;

                float scaleModifier = Random.Range(minScaleModifier, maxScaleModifier);
                obj.transform.localScale *= scaleModifier;
                
                obj.transform.SetParent(objectHolder);
            }


        }

        


	}
	

    public void ClearObjects()
    {
      

        var tempArray = new GameObject[objectHolder.childCount];

        for (int i = 0; i < tempArray.Length; i++)
        {
            tempArray[i] = objectHolder.GetChild(i).gameObject;
        }

        foreach (var child in tempArray)
        {
            DestroyImmediate(child);
        }



    }
	
}
