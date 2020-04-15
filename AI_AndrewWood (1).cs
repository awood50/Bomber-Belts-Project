/*
 * Andrew Wood
 * Assignment 3 Bomber Belts
 * 12-1-19
 */

using UnityEngine;
using System;
using System.Linq;

public class AI_AndrewWood : MonoBehaviour
{
    //it has been a while since I used C# so I tried my best with the syntax, but did have to search how to use the indexof feature and a few other things so I hope I used them correctly
    //also I don't really like my AI because it doesn't win a lot. But I don't think the AI had to win to get a good grade, it just had to be functional right?
    public CharacterScript mainScript;

    public float[] bombSpeeds;
    public float[] buttonCooldowns;
    public float[] buttonDistance = new float[8];//array to hold the distances between player and button
    public float[] bombDistances;//array to hold the distances of the bombs
    public float[] buttonLocations;//array to hold the locations of the buttons
    public float[] urgency = new float[8];//array to hold the urgency/priority of the belt that needs attention

    public int[] beltDirections;

    public float playerSpeed;
    public float playerLocation;//float to hold the location of the player
    public float priority;//score of the belt

    public int next;

    //This method is used to initilize the game
    void Start()
        {
        mainScript = GetComponent<CharacterScript>();

        if (mainScript == null)//If there is not a script to run
            {
            print("No CharacterScript found on " + gameObject.name);//Error messages are printed
            this.enabled = false;
            }

        buttonLocations = mainScript.getButtonLocations();//Uses the main script to get the locations of the button

        playerSpeed = mainScript.getPlayerSpeed();//calls the getPlayerSpeed method to get the speed of the player to increase or decrease
        }

    // Update is called once per frame
    void Update()
        {

        buttonCooldowns = mainScript.getButtonCooldowns();//Checks on the status of the button cooldowns
        beltDirections = mainScript.getBeltDirections();//Figures out which direction the belt is moving to
        //AI Code Added after this

        playerLocation = mainScript.getCharacterLocation();//provided method used to get the location of the character
        bombDistances = mainScript.getBombDistances();//provided method used to get the distances of the bombs
        bombSpeeds = mainScript.getBombSpeeds();//provided method used to get the speed of the bombs


        next = Array.IndexOf(urgency, urgency.Max());//This statement finds the position of the belt that needs to be pressed before any other
        //This next set of if statements is to start the movement of the player. The numbers help represent the direction and urgency
        if (buttonLocations[next] + .1 >= playerLocation)//If the position of the most urgent belt is above the player then it moves up
        {

            if (buttonDistance[next] <= .9) //if the distance of the most urgent belt is above the player rather than below, then the player will push the button and keep moving
                mainScript.push();//method called from mainscript to push the button and flip the belt direction
                mainScript.moveUp();//moves on to the next buttons until instructed to move down

        }
        else if (buttonLocations[next] - .1 <= playerLocation)//if the distance of the most urgent belt is below the player than it will push the button and move down
        {

            if (buttonDistance[next] <= .9)
                mainScript.push();//pushes the button
                mainScript.moveDown();//will keep moving down unless otherwise instructed

        }


        StepsToButton();  //This gets the actual steps in takes to get to the button from the player


        SetPriority(); //This method creates the priority for whichever button needs to be pressed more urgently

        

        }

    void StepsToButton()//This method is used to get the number of steps to the button in order to prioritize button needed to be pushed
    {
        for (int i = 0; i < 8; i++)//for loop used for the number of buttons existing
            {

            buttonDistance[i] = Math.Abs(buttonLocations[i] - playerLocation);  //this statement uses the absolute value of the math class to subtract the location of the player from the location of the button at the given index in order to get the distance between a player and a button 

            }
    }

    //this void method ranks the urgency of a belt that needs to be pushed, so that the AI will know which one is more important. I found that it was the most simple way for an AI to proritize is to have an urgency rating after failing many times. I came up with this on my own, although I am assuming it is a popular idea
    void SetPriority()
    {
        priority = 0;//this will be used to determine the priority of whatever button needs to be pressed



        for (int i = 0; i < 8; i++)
            {   //the number 8 is used in my for loops since that is the number of belts/buttons
            //noticed from samples that belt directions set to -1 means that it is coming at your player
                if (bombSpeeds[i] >= 4 && beltDirections[i] == -1)//so if the bomb speed is a 4 being the minimum speed and it is coming at you, then the priority is HIGH
                {
                priority += 12;//priority is increased due to the fact that it is coming at you
                }
            if (beltDirections[i] == 0)//this means if the belt is not moving towards you
            {
                priority += 1;
            }
            if (beltDirections[i] == -1)//if the belt is moving at you
                {
                priority += 5;//priority is high
                }
            
            if (buttonCooldowns[i] >= .8)//if the button cooldowns are lower the priority is lower
                {
                priority -= 1;
                
                }
            urgency[i] = priority;//the value of the urgency index will be set to the calculated priority after each frame of the game
            priority = 0;//priority is always set back to 0 after each updated game frame

        }
    }
}
