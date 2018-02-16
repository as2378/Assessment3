using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class viceScript : MonoBehaviour {

    public GameObject speechBubble; //the speech bubble showing how many clothing items were guessed correctly, set in editor

    int guesses; //integer representing the number of guesses the user has submitted so far

    int correctTorso; //integer representing the correct torso clothing
    int correctLegs; //integer representing the correct legs clothing

    public List<Sprite> torsos; //list of possible torso clothes, set in editor
    public List<Sprite> legs; //list of possible leg clothes, set in editor

    public GameObject torso; //panel gameobject representing torso, set in editor
    public GameObject leg; //panel gameobject representing legs, set in editor

    int torsoChoice; //integer representing which torso sprite the user has selected
    int legChoice; //integer representing which leg sprite the user has selected

    void Start()
    {
        guesses = 0;

        System.Random rnd = new System.Random();

        correctTorso = rnd.Next(0, torsos.Count);
        correctLegs = rnd.Next(0, legs.Count);

        torsoChoice = 0;
        legChoice = 0;

        torso.GetComponent<Image>().sprite = torsos[torsoChoice];
        leg.GetComponent<Image>().sprite = legs[legChoice];

        speechBubble.SetActive(false);
    }

    public void torsoLeftPressed()
    {
        torsoChoice = (torsoChoice + 1) % torsos.Count;
        torso.GetComponent<Image>().sprite = torsos[System.Math.Abs(torsoChoice)];
    }

    public void torsoRightPressed()
    {
        torsoChoice = (torsoChoice - 1) % torsos.Count;
        torso.GetComponent<Image>().sprite = torsos[System.Math.Abs(torsoChoice)];
    }

    public void legsRightPressed()
    {
        legChoice = (legChoice + 1) % legs.Count;
        leg.GetComponent<Image>().sprite = legs[System.Math.Abs(legChoice)];
    }

    public void legsLeftPressed()
    {
        legChoice = (legChoice - 1) % legs.Count;
        leg.GetComponent<Image>().sprite = legs[System.Math.Abs(legChoice)];
    }

    public void submitGuess()
    {
        speechBubble.SetActive(true);

        guesses++;

        int correctGuesses = 0;

        if (System.Math.Abs(legChoice) == correctLegs)
        {
            correctGuesses++;
        }

        if (System.Math.Abs(torsoChoice) == correctTorso)
        {
            correctGuesses++;
        }

        if (correctGuesses == 2)
        {
            speechBubble.GetComponentInChildren<Text>().text = "I love it! You win!";
        }
        else if (guesses == 3)
        {
            speechBubble.GetComponentInChildren<Text>().text = "I've given you enough chances! Get out!";
        }
        else
        {
            speechBubble.GetComponentInChildren<Text>().text = "You correctly guessed " + correctGuesses.ToString() +
                " item(s) of clothing out of 2. You have " + (3 - guesses).ToString() + " guesses left.";
        }

        //Debug.Log(correctGuesses.ToString());
    }
}
