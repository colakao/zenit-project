using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGenerator : MonoBehaviour
{
    public int planetCount;
    private int _currentPlanets;

    List<int> planetNumber = new List<int>();

    public List<Star> starList = new List<Star>(); //List of scriptableobject (star)

    List<Star> levels = new List<Star>();

    public static bool mapGenerated;


    //void Awake() //Throws a random value based on difficulty
    //{
    //    switch (Difficulty.gameMode)
    //    {
    //        case Difficulty.GameMode.EASY:
    //            planetCount = Mathf.RoundToInt(Random.Range(3, 5));
    //            break;
    //        case Difficulty.GameMode.MEDIUM:
    //            planetCount = Mathf.RoundToInt(Random.Range(4, 9));
    //            break;
    //        case Difficulty.GameMode.HARD:
    //            planetCount = Mathf.RoundToInt(Random.Range(9, 13));
    //            break;
    //    }

    //    _currentPlanets = 0;

    //    for (int i = 0; i < starList.Count; i++)
    //    {
    //        planetNumber.Add(starList[i].planets);
    //    }

    //}

    private void Awake()
    {
        if (!mapGenerated)
        {
            Setup();
            mapGenerated = true;
        }
    }

    public void Setup()
    {
        switch (Difficulty.gameMode)
        {
            case Difficulty.GameMode.EASY:
                planetCount = Mathf.RoundToInt(Random.Range(3, 5));
                break;
            case Difficulty.GameMode.MEDIUM:
                planetCount = Mathf.RoundToInt(Random.Range(4, 9));
                break;
            case Difficulty.GameMode.HARD:
                planetCount = Mathf.RoundToInt(Random.Range(9, 13));
                break;
        }

        _currentPlanets = 0;

        for (int i = 0; i < starList.Count; i++)
        {
            planetNumber.Add(starList[i].planets);
        }
    }

    private void Update()
    {
        while (_currentPlanets < planetCount) //checks amount of planets with each iteration compared to the max number of planets given by our random value
        {
            Star s = starList[Random.Range(0, planetNumber.Count)]; //picks random star from list
            int p = s.planets; //sets p value to the number of planets of said star

            int tempSum = p + _currentPlanets; //does a temporary sum of the current amount of planets and the star's amount of planets

            if (tempSum <= planetCount) //if it doesn't surpass the max amount of stars it adds the star to the list
            {
                _currentPlanets += p;
                levels.Add(s);
            }

            tempSum = 0; //reset 
            break;
        }
    }

    public void DebugP()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            Debug.Log(levels[i].name + " has " + levels[i].planets + " planets");
        }
        Debug.Log("Current planet amount: " + _currentPlanets);
        Debug.Log("Randomized number of max planets: " + planetCount);
    }
}
