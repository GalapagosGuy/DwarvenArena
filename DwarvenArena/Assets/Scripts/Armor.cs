using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tierOne;
    [SerializeField]
    private GameObject[] tierTwo;
    [SerializeField]
    private GameObject[] tierThree;

    private int currentArmor = 0;
    // Start is called before the first frame update
    void Start()
    {
        ToggleArmor(false, tierOne);
        ToggleArmor(false, tierTwo);
        ToggleArmor(false, tierThree);
    }

    private void ToggleArmor(bool toggle, GameObject[] armorTier)
    {
        foreach (GameObject armor in armorTier)
        {
            armor.SetActive(toggle);
        }
    }
    
    public void ActivateTierOne()
    {
        if(currentArmor == 0)
        {
            ToggleArmor(true, tierOne);
            PlayerStats.Instance.SetMaxHp(50);
            currentArmor++;
        }
        
    }

    public void ActivateTierTwo()
    {
        if (currentArmor == 1)
        {
            ToggleArmor(true, tierTwo);
            PlayerStats.Instance.SetMaxHp(125);
            currentArmor++;
        }
            
    }

    public void ActivateTierThree()
    {
        if (currentArmor == 2)
        {
            ToggleArmor(true, tierThree);
            PlayerStats.Instance.SetMaxHp(200);
        }
    }

}
