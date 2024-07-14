using UnityEngine;
using System.Collections;

public class Dispenser : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Transform spawnPoint;
    public float spawnInterval = 1f;
    public float launchSpeed = 5f;
    public float timeDestroy;

    [SerializeField] private LineaDeSalida lineaDeSalida;

    private bool isSpawning = false;

    private void Start()
    {
        lineaDeSalida = FindObjectOfType<LineaDeSalida>();
    }

    private void Update()
    {
        if (lineaDeSalida != null && lineaDeSalida.objDestroyed && !isSpawning)
        {
            StartCoroutine(SpawnPrefab());
        }
    }

    private IEnumerator SpawnPrefab()
    {
        isSpawning = true;

        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
            StartCoroutine(MoveForward(spawnedObject));
            Destroy(spawnedObject, timeDestroy);
        }

        // Optionally, you can set isSpawning to false if you want to stop spawning under certain conditions
    }

    private IEnumerator MoveForward(GameObject obj)
    {
        float timePassed = 0f;
        while (timePassed < 3f)
        {
            obj.transform.Translate(Vector3.forward * launchSpeed * Time.deltaTime);
            timePassed += Time.deltaTime;
            yield return null;
        }
    }
}
