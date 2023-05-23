using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalInformationStorage : MonoBehaviour
{
    public string[] AnimalName;
    public Texture[] AnimalImage;
    public Texture[] AnimalLivingArea;
    public string[] AnimalDesc;
    [Tooltip("0 = Herbivore, 1 = Carnivore, 2 = Omnivore")]
    public int[] Diet;
    [Tooltip("In Metres")]
    public float[] Size;
    [Tooltip("In Kg")]
    public float[] Weight;
    [Tooltip("1 = Not Aggressive, 10 = Extremely Aggressive")]
    public int[] Temperment;
}
