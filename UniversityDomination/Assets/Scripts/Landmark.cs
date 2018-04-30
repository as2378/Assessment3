//===========================================================================
//Executable Link:
//https://as2378.github.io/unlucky/files/Assessment4/UniversityDomination.zip
//===========================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * This class is used to represent the beer, knowledge and PVC landmarks. It is also used to handle the loading of the PVC minigame.
 */
public class Landmark : MonoBehaviour {

    public enum ResourceType {Beer, Knowledge ,ViceChancellor}; //used to represent whether this landmark is the vice chancellor

    public GameObject GameManager;
    public GameObject Map;
    public GameObject GUI;

    public void OnMouseDown()
    {
        if ( GetComponentInParent<Sector>().GetOwner() != null)
        {
            if (GetComponentInParent<Sector>().GetOwner().IsActive())
            {
                //miniGamePlayer = GameManager.GetComponent<Game>().currentPlayer;

                //GameManager.GetComponent<Game>().NextTurnState();
                StartCoroutine(loadMiniGame());
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

    IEnumerator loadMiniGame()          //Deals with loading the mini-game scene
    {
        var loading = SceneManager.LoadSceneAsync("miniGameScene", LoadSceneMode.Additive);
        yield return loading;
        Scene scene = SceneManager.GetSceneByName("miniGameScene");
        SceneManager.SetActiveScene(scene);

        GameManager.SetActive(false);   //Deactivate the main game so no moves can be accidently made during the mini-game
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
