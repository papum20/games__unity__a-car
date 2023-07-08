using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapCounter : MonoBehaviour
{

    [SerializeField] HudManager hudManager;



    bool started;

    bool firstCheck;
    bool secondCheck;
    bool thirdCheck;

    public int lapsToDo = 1;
    int laps;

    int position = 1;



    private void Start()
    {
        started = false;
        laps = 0;
        firstCheck = false;
        secondCheck = false;
        thirdCheck = false;


        int tmpLaps = PlayerPrefs.GetInt("laps");
        if (tmpLaps > 0)
            lapsToDo = tmpLaps;
        else
            lapsToDo = 1;

        hudManager.totalLaps = lapsToDo;
    }



    private void Update()
    {
        hudManager.position = position;
        hudManager.laps = laps;
    }





    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "firstCheck")
        {
            firstCheck = true;
            secondCheck = false;
            thirdCheck = false;
        }
        else if(firstCheck == true && other.tag == "secondCheck")
        {
            secondCheck = true;
            thirdCheck = false;
        }
        else if(secondCheck == true && other.tag == "thirdCheck")
        {
            thirdCheck = true;
        }
        else if(other.tag == "start")
        {
            if (!started)
            {
                started = true;
                hudManager.cheaterTimer = 0f;
            }
            else if (firstCheck && secondCheck && thirdCheck)
            {
                laps++;
                firstCheck = false;
                secondCheck = false;
                thirdCheck = false;

                //ARRIVAL
                if (laps == lapsToDo && gameObject.tag == "Player")
                {
                    hudManager.ShowFinalPlacement(position);
                }
                hudManager.cheaterTimer = 0f;
            }
            //CHEATER
            else if (started && gameObject.tag == "Player")
                hudManager.ShowCheaterPanel();
        }
    }





}
