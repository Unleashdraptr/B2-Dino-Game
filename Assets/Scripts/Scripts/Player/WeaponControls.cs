using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControls : MonoBehaviour
{
    public bool needAmmo;
    public string ammoType;
    public int ammoAmount;

    public int AtkDmg;
    public int AtkSpd;

    public string WeaponName;
    public int WeaponType;

    public Transform SwingPoint;
    public float attackRange;
    public LayerMask Animals;
    // Start is called before the first frame update
    void Start()
    {
        if(!needAmmo)
        {
            ammoAmount = 9999999;
        }
    }
    public void ActivateWeapons()
    {
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
    public void ActivateSword()
    {
        Collider[] HitAnimals = Physics.OverlapSphere(SwingPoint.position, attackRange, Animals);
        foreach(Collider animal in HitAnimals)
        {
            animal.GetComponentInParent<Generalist_AI>().TakeDmg(AtkDmg);
        }
    }
    public void ActivateBow()
    {
        Debug.Log("sHARP THING");
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(SwingPoint.position, attackRange);
    }
}
