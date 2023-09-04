using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleRandomWalkData")]
public class SimpleRandomWalkData : ScriptableObject
{
    public int interations = 10;
    public int walkLength = 10;
    public int stepOffset = 2;
    public int wallLayer = 1;
    public bool startRandomlyEachInteraction = true;
}
