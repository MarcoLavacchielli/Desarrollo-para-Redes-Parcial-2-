using UnityEngine;
using System.Collections;

public class Dispenser : MonoBehaviour
{
    public GameObject prefabToSpawn; 
    public Transform spawnPoint;     
    public float spawnInterval = 1f; 
    public float launchSpeed = 5f;
    public float timeDestroy;

    private bool isRotating = false;
    private int playersInScene = 0;
    private bool startedShoot = false;

    /*private void Start()
    {
        // Inicia la corutina de spawn
        StartCoroutine(SpawnPrefab());
    }*/

    private void Update()
    {
        CountPlayers();

        if (isRotating && startedShoot==false)
        {
            StartCoroutine(SpawnPrefab());
            startedShoot = true;
        }
    }

    private IEnumerator SpawnPrefab()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
            StartCoroutine(MoveForward(spawnedObject));
            Destroy(spawnedObject, timeDestroy); 
        }
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

    private void CountPlayers()
    {
        var players = FindObjectsOfType<PlayerHostMovement>();
        playersInScene = players.Length;

        if (playersInScene >= 2)
        {
            StartRotating();
        }
        else
        {
            StopRotating();
        }
    }

    public void OnPlayerCountChanged()
    {
        CountPlayers();
    }

    private void StartRotating()
    {
        isRotating = true;
    }

    private void StopRotating()
    {
        isRotating = false;
    }
}
