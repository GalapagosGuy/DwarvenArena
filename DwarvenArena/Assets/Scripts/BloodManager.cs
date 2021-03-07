using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodManager : MonoBehaviour
{
    public static BloodManager Instance = null;

    private void Awake()
    {
        if (BloodManager.Instance == null)
            BloodManager.Instance = this;
        else
            Destroy(this.gameObject);
    }

    [SerializeField] private float bloodYposition;
    [SerializeField] private float bloodChance;
    [SerializeField] private float xzAxisRandomizer;

    public GameObject bloodOnGround;

    public Sprite[] bloodSprites;
    public Color bloodColor;

    public void CreateBloodSplash(Vector3 position)
    {
        if (Random.Range(0, 100) < bloodChance)
        {
            Sprite choosenBlood = bloodSprites[Random.Range(0, bloodSprites.Length)];

            position.x += Random.Range(-xzAxisRandomizer, xzAxisRandomizer);
            position.y = bloodYposition;
            position.z += Random.Range(-xzAxisRandomizer, xzAxisRandomizer);

            GameObject bloodOnGroundGO = Instantiate(bloodOnGround, position, bloodOnGround.transform.rotation);
            bloodOnGroundGO.GetComponentInChildren<Image>().sprite = choosenBlood;
            bloodOnGroundGO.GetComponentInChildren<Image>().color = bloodColor;

            bloodOnGroundGO.GetComponent<BloodSplash>().SetTimeToDestroy(Random.Range(2.0f, 4.0f));
        }
    }
}
