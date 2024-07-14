using UnityEngine;

public class CollisionToggle : MonoBehaviour
{
    public GameObject[] pair1 = new GameObject[2];
    public GameObject[] pair2 = new GameObject[2];
    public GameObject[] pair3 = new GameObject[2];
    public GameObject[] pair4 = new GameObject[2];
    public GameObject[] pair5 = new GameObject[2];

    void Start()
    {
        ToggleCollision(pair1);
        ToggleCollision(pair2);
        ToggleCollision(pair3);
        ToggleCollision(pair4);
        ToggleCollision(pair5);
    }

    void ToggleCollision(GameObject[] pair)
    {
        if (pair.Length == 2)
        {
            // Selecciona un índice aleatorio entre 0 y 1
            int randomIndex = Random.Range(0, 2);

            // Asigna isTrigger basado en el índice aleatorio
            Collider collider1 = pair[randomIndex].GetComponent<Collider>();
            Collider collider2 = pair[1 - randomIndex].GetComponent<Collider>();

            if (collider1 != null)
            {
                collider1.isTrigger = true;
            }

            if (collider2 != null)
            {
                collider2.isTrigger = false;
            }
        }
        else
        {
            Debug.LogError("Each pair should contain exactly 2 GameObjects.");
        }
    }
}
