using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Species", menuName = "Unstable Ecosystems/Species", order = 0)]
public class Species : ScriptableObject {
    public SpeciesType SpeciesType;
    public Color color;

    public float reproducingThreshold;
    public float deathThreshold;
    public float speed;
    public float maxSize;
    public float eatSpeed;
    public float babySize;
    public float lookRange;
    
}

public enum SpeciesType{
    PLANT,
    HERBIVORE,
    CARNIVORE,
}
