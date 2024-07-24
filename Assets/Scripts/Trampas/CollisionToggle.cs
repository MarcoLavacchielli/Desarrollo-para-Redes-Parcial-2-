using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionToggle : MonoBehaviour
{
    public GameObject[] pair1 = new GameObject[2];
    public GameObject[] pair2 = new GameObject[2];
    public GameObject[] pair3 = new GameObject[2];
    public GameObject[] pair4 = new GameObject[2];
    public GameObject[] pair5 = new GameObject[2];

    private void Start()
    {
        ToggleCollision(pair1);
        ToggleCollision(pair2);
        ToggleCollision(pair3);
        ToggleCollision(pair4);
        ToggleCollision(pair5);
    }

    private void ToggleCollision(GameObject[] pair)
    {
        if (pair.Length == 2)
        {
            int randomIndex = Random.Range(0, 2);
            GameObject selectedObject = pair[randomIndex];
            Collider collider = selectedObject.GetComponent<Collider>();
            if (collider != null)
            {
                collider.isTrigger = true;
            }
        }
    }
}
