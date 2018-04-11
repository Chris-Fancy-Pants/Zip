using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeStrip : MonoBehaviour {

    public Transform leftEdge;
    public Transform rightEdge;

    public Transform treeHolder;

    public GameObject[] trees;

    public float minTreeDistance = 0.5f;
    public float maxTreeDistance = 4f;
  
	// Use this for initialization
	public void PlaceTrees () {

        float treePos = leftEdge.position.x;



        while(treePos < rightEdge.position.x)
        {

            treePos += Random.Range(minTreeDistance, maxTreeDistance);

            if (treePos < rightEdge.position.x)
            {
                int randomTree = Random.Range(0, trees.Length);
                GameObject obj = Instantiate(trees[randomTree], new Vector2(treePos, transform.position.y), Quaternion.identity) as GameObject;
                obj.transform.SetParent(treeHolder);
            }


        }

        


	}
	

    public void ClearTrees()
    {
      

        var tempArray = new GameObject[treeHolder.childCount];

        for (int i = 0; i < tempArray.Length; i++)
        {
            tempArray[i] = treeHolder.GetChild(i).gameObject;
        }

        foreach (var child in tempArray)
        {
            DestroyImmediate(child);
        }



    }
	
}
