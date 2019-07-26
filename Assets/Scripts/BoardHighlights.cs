﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardHighlights : MonoBehaviour
{
    public static BoardHighlights Instance { set; get; }

    public GameObject highlightPrefab;
    private List<GameObject> highlights;

    private void Start()
    {
        Instance = this;
        highlights = new List<GameObject>();
    }

    private GameObject GetHighlightObject()
    {
        GameObject go = highlights.Find (g => !g.activeSelf);   //find inactivated object. g => ... is predicate

        //if no available inactivated prefabs, then create new one
        if(go == null)
        {
            go = Instantiate(highlightPrefab);
            highlights.Add(go);
        }

        return go;
    }

    public void HighlightAllowedMoves(bool[,] moves) {
        for (int i=0; i<8; i++){
            for (int j = 0; j < 8; j++){
                if(moves [i, j])
                {   //if some position (i, j) is true, then active highlight prefab in that position
                    GameObject go = GetHighlightObject();
                    go.SetActive(true);
                    go.transform.position = new Vector3(i + 0.5f, 0.01f, j + 0.5f);
                }

            }

        }    
    }
    
    public void Hidehighlights()
    {
        foreach (GameObject go in highlights)
            go.SetActive(false);
    }
}
