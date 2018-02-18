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

    public GameObject GameManager;
    public GameObject Map;
    public GameObject GUI;
    //public Player miniGamePlayer;

    public void OnMouseDown()
    {
        if ( GetComponentInParent<Sector>().GetOwner() != null)
        {
            if (GetComponentInParent<Sector>().GetOwner().IsActive())
            {
                //miniGamePlayer = GameManager.GetComponent<Game>().currentPlayer;

                //GameManager.GetComponent<Game>().NextTurnState();
                StartCoroutine(loadThings());
            }
            else
            {
                GetComponentInParent<Sector>().OnMouseUpAsButtonAccessible();
            }
        }
        else
        {
            GetComponentInParent<Sector>().OnMouseUpAsButtonAccessible();
        }
    }

    IEnumerator loadThings()
    {
        var loading = SceneManager.LoadSceneAsync("miniGameScene", LoadSceneMode.Additive);
        yield return loading;
        Scene scene = SceneManager.GetSceneByName("miniGameScene");
        SceneManager.SetActiveScene(scene);

        GameManager.SetActive(false);
        Map.SetActive(false);
        GUI.SetActive(false);
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
