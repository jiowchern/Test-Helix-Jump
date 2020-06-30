using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PieSpawner : MonoBehaviour
{
    public Transform Ball;
    private Vector3 _StartPosition;
    readonly System.Collections.Generic.List<GameObject> _Instances;
    public Transform Root;
    public GameObject Origin        ;

    internal void Run(int seed, int count, int space)
    {
        Seed = seed;
        PieCount = count;
        Space = space;
        ResetPosition();
    }

    public GameObject End;
    public int Seed;
    public int PieCount;
    public float Space;
    public PieSpawner()
    {
        _Instances = new List<GameObject>();
    }
    void Start()
    {
        _StartPosition = Ball.position;
        
        _Reset();
    }

    private void _Reset()
    {
        foreach(var instance in _Instances)
        {
            GameObject.Destroy(instance);
        }        


        var rand = new System.Random(Seed);
        var originPosition = Origin.transform.position;
        for (var i = 1; i <= PieCount; ++i)
        {
            var pirObj = GameObject.Instantiate(Origin);
            pirObj.transform.position = new Vector3(originPosition.x, originPosition.y - i * Space, originPosition.z);
            pirObj.SetActive(true);
            pirObj.transform.SetParent(Root);
            _Instances.Add(pirObj);

            var obstacles = pirObj.GetComponentsInChildren<Obstacle>();
            var floors = obstacles.Where(o => o.Type == Obstacle.TYPE.FLOOR).OrderBy((o) => rand.NextDouble()).Take(rand.Next(1, 8));
            foreach (var floor in floors)
            {
                floor.gameObject.transform.parent.gameObject.SetActive(false);
            }

            var boards = obstacles.Where(o => o.Type == Obstacle.TYPE.BOARD && o.gameObject.transform.parent.gameObject.activeSelf).OrderBy((o) => rand.NextDouble()).Take(rand.Next(7, 8));
            foreach (var board in boards)
            {
                board.gameObject.SetActive(false);
            }


        }


        var endObj = GameObject.Instantiate(End);
        endObj.transform.position = new Vector3(originPosition.x, originPosition.y - (PieCount + 1) * Space, originPosition.z);
        endObj.SetActive(true);
        endObj.transform.SetParent(Root);
        _Instances.Add(endObj);
    }

    public void ResetPosition()
    {
        Ball.position = _StartPosition;
        _Reset();
    }
}
