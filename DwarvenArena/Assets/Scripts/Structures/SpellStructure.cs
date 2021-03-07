using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellStructure : Structure
{
    public GameObject holder = null;
    public GameObject spellReference = null;

    private bool spellCanBeTaken = true;

    public GameObject book;

    protected override void Start()
    {
        base.Start();

    }

    public override void Use(GameObject hero)
    {
        if (spellCanBeTaken)
        {
            PlayerSlots playerSlots = hero.GetComponent<PlayerSlots>();

            if (playerSlots)
            {
                playerSlots.ChangeSpell(spellReference);
                spellCanBeTaken = false;
                book.SetActive(false);
            }
        }
    }

    public void ReturnSpell()
    {
        if (holder)
            spellReference.transform.parent = holder.transform;

        //probably different way to set correct values
        spellReference.transform.localPosition = Vector3.zero;
        spellReference.transform.localRotation = Quaternion.identity;
        spellReference.transform.localScale = Vector3.one;
        book.SetActive(true);

        spellCanBeTaken = true;
    }
}
