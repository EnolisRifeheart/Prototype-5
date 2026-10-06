using System;
using UnityEngine;

// This class is just an example of how to using the grid from other scripts
public class GameManagerGridExample : MonoBehaviour
{
    [SerializeField] GridManager gridManager;
    [SerializeField] GameObject playerObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gridManager.InstantiateObjectOnGrid(new Vector2Int(0, 0), playerObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
