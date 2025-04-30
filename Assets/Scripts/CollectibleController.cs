using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CollectibleController : MonoBehaviour
{
    [HideInInspector]
    public List<Collectible> Collectibles = new List<Collectible>();
    public TextMeshProUGUI textMeshProUGUI;

    public bool allDead = false;

    private readonly Predicate<Collectible> pre = delegate (Collectible a) { return a.dead; };

    // Start is called before the first frame update
    void Start()
    {
        Collectibles = GetComponentsInChildren<Collectible>().ToList();
    }

    // Update is called once per frame
    void Update()
    {

        int score = 0;
        foreach (Collectible a in Collectibles)
        {
            score += a.dead ? 1 : 0;
        }

        textMeshProUGUI.text = "Score: " + score.ToString();

        if (Collectibles.TrueForAll(pre))
        {
            allDead = true;
        }
    }
}
