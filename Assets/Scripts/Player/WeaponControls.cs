using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControls : MonoBehaviour
{
    //The type of ammo and ammo capacity
    public bool needAmmo;
    public string ammoType;
    public int ammoAmount;

    //Its attack stats
    public int AtkDmg;
    public int AtkSpd;

    //The type of weapon and its name
    public string WeaponName;
    public int WeaponType;

    //The swing point for the sword and its attack range
    public Transform SwingPoint;
    public float attackRange;
    public LayerMask Animals;
    void Start()
    {
        //If the weapon doesnt need ammo just set it to infinity so it can still run
        if(!needAmmo)
        {
            ammoAmount = 9999999;
        }
    }
    public void ActivateWeapons()
    {
        //Once this runs itll check which weapon type it is and the run what type it is
        switch(WeaponType)
        {
            case 1:
                ActivateSword();
                break;
            case 2:
                if (ammoAmount > 0)
                {
                    ActivateBow();
                }
                break;
        }
    }
    //If its a sword itll place a sphere around the Swingpoint and then detect all the animals it hit in the sphere
    public void ActivateSword()
    {
        Collider[] HitAnimals = Physics.OverlapSphere(SwingPoint.position, attackRange, Animals);
        foreach(Collider animal in HitAnimals)
        {
            animal.GetComponentInParent<Generalist_AI>().TakeDmg(AtkDmg);
        }
    }
    //Currently not implemented
    public void ActivateBow()
    {
        
    }
}
