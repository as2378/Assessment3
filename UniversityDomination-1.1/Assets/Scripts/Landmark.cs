using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//=======code by charlie=========
using UnityEngine.SceneManagement;
//===============================


public class Landmark : MonoBehaviour {

    public enum ResourceType {Beer, Knowledge
        //===================code by charlie===================
        ,ViceChancellor //used to represent whether this landmark is the vice chancellor
    };

    public GameObject scene1Thing1;
    public GameObject scene1Thing2;
    public GameObject scene1Thing3;

    public void viceChanceIsClicked()
    {
        if (GetComponentInParent<Sector>().GetOwner() != null)
        {
            if (GetComponentInParent<Sector>().GetOwner().IsActive() /*&& GetComponentInParent<Sector>().AdjacentSelectedUnit() != null*/)
            {
                StartCoroutine(loadThings());
            }
        }
    }

    IEnumerator loadThings()
    {
        var loading = SceneManager.LoadSceneAsync("miniGameScene", LoadSceneMode.Additive);
        yield return loading;
        Scene scene = SceneManager.GetSceneByName("miniGameScene");
        SceneManager.SetActiveScene(scene);

        scene1Thing1.SetActive(false);
        scene1Thing2.SetActive(false);
        scene1Thing3.SetActive(false);
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
