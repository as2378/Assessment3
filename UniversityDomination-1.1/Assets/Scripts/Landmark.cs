using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmark : MonoBehaviour {

    public enum ResourceType {Beer, Knowledge
        //===================code by charlie===================
        ,ViceChancellor //used to represent whether this landmark is the vice chancellor
    };

    public void viceChanceIsClicked()
    {
        if (GetComponentInParent<Sector>().GetOwner() != null)
        {
            if (GetComponentInParent<Sector>().GetOwner().IsActive() /*&& GetComponentInParent<Sector>().AdjacentSelectedUnit() != null*/)
            {

                //start minigame
                Debug.Log("aayyyyy");
            }
        }
    }
    //=======================================

    [SerializeField] private ResourceType resourceType;
    [SerializeField] private int amount = 2;

    public ResourceType GetResourceType() {
        return resourceType;
    }

    public void SetResourceType(ResourceType resourceType) {
        this.resourceType = resourceType;
    }

    public int GetAmount() {
        return amount;
    }

    public void SetAmount(int amount) {
        this.amount = amount;
    }
}
