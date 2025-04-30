using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemiesController: MonoBehaviour
{
    [HideInInspector]
    public List<Enemy> Enemies;

    private void Start()
    {
        Enemies = GetComponentsInChildren<Enemy>().ToList();
    }
}