using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This script bascially does as the class says
public class AnimalInformationStorage : MonoBehaviour
{
    //This is a small script filled with lists that relate to certain animals, this stores all their data that will 
    //be shown within the animal Encylopedia, might move this to a text file later on
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
