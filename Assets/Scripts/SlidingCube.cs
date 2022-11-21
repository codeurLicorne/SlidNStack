using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlidingCube : MonoBehaviour
{
    [SerializeField] public float slideSpeed = 1;
   
    public static SlidingCube CurrentCube { get; private set; }
    public static SlidingCube LastCube { get; private set; }
    public SlideDirection SlideDirection { get;  set; }

    private void OnEnable()
    {
        if(LastCube == null)
            LastCube = GameObject.Find("BaseCube").GetComponent<SlidingCube>();
        
        
        CurrentCube = this;
        GetComponent<Renderer>().material.color = GetRandomColour();


        transform.localScale = new Vector3(LastCube.transform.localScale.x, transform.localScale.y, LastCube.transform.localScale.z);
    }

    private Color GetRandomColour()
    {
        return new Color(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f));
    }

    internal void StopSlide()
    {
        if (gameObject.name == "BaseCube")
        {
            return;
        }

        slideSpeed = 0;
        float hangOver = GetHangover();

        float max = SlideDirection == SlideDirection.Z ? LastCube.transform.localScale.z : LastCube.transform.localScale.x;
        if (Mathf.Abs(hangOver) >= max)
        {
            LastCube = null;
            CurrentCube = null;
            SceneManager.LoadScene(0);
        }

        float direction = hangOver > 0 ? 1f : -1f;

        if (SlideDirection == SlideDirection.Z)
        {
            SplitCubeOnZ(hangOver, direction);
        }
        else
        {
            SplitCubeOnX(hangOver, direction);
        }

        LastCube = this;
    }

    private float GetHangover()
    {
        if(SlideDirection==SlideDirection.Z)
        {
            return transform.position.z - LastCube.transform.position.z;
        }
        else
        {
            return transform.position.x - LastCube.transform.position.x;
        }
        
    }

    private void SplitCubeOnX(float hangOver, float direction)
    {
        float newXSize = LastCube.transform.localScale.x - Mathf.Abs(hangOver);
        float cubeSliceSize = transform.localScale.x - newXSize;

        float newXPosition = LastCube.transform.position.x + (hangOver / 2);
        transform.localScale = new Vector3(newXSize, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z );

        float cubeEdge = transform.position.x + (newXSize / 2f * direction);
        float cubeSliceXPosition = cubeEdge + cubeSliceSize / 2f * direction;

        SpawnFallingCube(cubeSliceXPosition, cubeSliceSize);
    }

    private void SplitCubeOnZ(float hangOver, float direction)
    {
        float newZSize = LastCube.transform.localScale.z - Mathf.Abs(hangOver);
        float cubeSliceSize = transform.localScale.z - newZSize;

        float newZPosition = LastCube.transform.position.z + (hangOver / 2);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZSize);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);

        float cubeEdge = transform.position.z + (newZSize / 2f * direction);
        float cubeSliceZPosition = cubeEdge + cubeSliceSize / 2f * direction;
        
        SpawnFallingCube(cubeSliceZPosition, cubeSliceSize);
    }

    private void SpawnFallingCube(float cubeSliceZPosition, float cubeSliceSize)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        if(SlideDirection == SlideDirection.Z)
        {
            cube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, cubeSliceSize);
            cube.transform.position = new Vector3(transform.position.x, transform.position.y, cubeSliceZPosition);
        }
        else
        {
            cube.transform.localScale = new Vector3(cubeSliceSize, transform.localScale.y, transform.localScale.z );
            cube.transform.position = new Vector3(cubeSliceZPosition, transform.position.y, transform.position.z );
        }
       

        cube.AddComponent<Rigidbody>();
        cube.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;
        Destroy(cube.gameObject, 1.5f);
       
    }

    void Update()
    {
        if(SlideDirection == SlideDirection.Z)
        {
            transform.position += transform.forward * Time.deltaTime * slideSpeed;
        }
        else
        {
            transform.position += transform.right * Time.deltaTime * slideSpeed;
        }
    }
}
