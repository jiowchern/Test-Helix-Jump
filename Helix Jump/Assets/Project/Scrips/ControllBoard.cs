using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllBoard : MonoBehaviour
{
    public PieSpawner Spawner;
    public UnityEngine.UI.InputField Seed;
    public UnityEngine.UI.InputField Count;
    public UnityEngine.UI.InputField Space;
    private readonly System.Random _Rand;

    public ControllBoard()
    {
        _Rand = new System.Random();
    }
    public void Run()
    {
        Spawner.Run(int.Parse(Seed.text), int.Parse(Count.text), int.Parse(Space.text));
    }
    public void Rand()
    {
        Seed.text = _Rand.Next(int.MaxValue).ToString();
    }
}
