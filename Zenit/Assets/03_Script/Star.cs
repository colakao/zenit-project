using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Star", menuName = "Star")]
public class Star : ScriptableObject
{
    public int planets;
    public Color color;
    public new string name;
    public Sprite artwork;

    //public void StarInfo(int newPlanets, Color newColor, string newName)
    //{
    //    planets = newPlanets;
    //    color = newColor;
    //    name = newName;
    //}
}
