using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private SlidingCube cubePrefab;
    [SerializeField] private SlideDirection slideDirection;

    public void SpawnCube()
    {
        var cube = Instantiate(cubePrefab);

        if (SlidingCube.LastCube != null && SlidingCube.LastCube.gameObject != GameObject.Find("BaseCube"))
        {

           float x = slideDirection == SlideDirection.X ? transform.position.x : SlidingCube.LastCube.transform.position.x;
           float z = slideDirection == SlideDirection.Z ? transform.position.z : SlidingCube.LastCube.transform.position.z;

           cube.transform.position = new Vector3(x, SlidingCube.LastCube.transform.position.y + cubePrefab.transform.localScale.y, z);
        }
        else
        {
            cube.transform.position = transform.position;
        }

        cube.SlideDirection = slideDirection;
       
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, cubePrefab.transform.localScale);
    }
}
