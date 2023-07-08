using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    
    [Header("Speedometer")]
    [SerializeField] GameObject speedometerPointer;
    [SerializeField] Car2 carScript;

    [SerializeField] float minSpeedRotation;
    [SerializeField] float maxSpeedRotation;
    float speed = 0f;


    [Header("Current")]
    [SerializeField] Text currentPlacement;
    [SerializeField] Text lapText;

    public int position;
    public int totalLaps;
    public int laps;


    [Header("Arrival")]
    [SerializeField] Text finalPlacement;
    [SerializeField] GameObject finalPlacementPanel;
    [SerializeField] Image cheaterPanel;
    [SerializeField] Text cheaterWriting;

    Color cheaterPanelDefaultColor;
    Color cheaterWritingDefaultColor;
    float alphaTimer = 0f;
    float dissolveDuration = 5f;

    public float cheaterTimer = 0f;
    float cheaterTime = 7f;


    [Header("Menu")]
    [SerializeField] GameObject menuCanvas;





    private void Start()
    {
        finalPlacementPanel.SetActive(false);
        cheaterPanel.gameObject.SetActive(false);
        cheaterPanelDefaultColor = cheaterPanel.color;
        cheaterWritingDefaultColor = cheaterWriting.color;
    }




    private void Update()
    {
        //SPEEDOMETER
        speed = carScript.GetSpeed();

        speedometerPointer.transform.localEulerAngles = new Vector3(0, 180, minSpeedRotation + (maxSpeedRotation - minSpeedRotation) / 180 * speed);

        //CHEATER AND POSITION MANAGEMENT
        cheaterTimer += Time.deltaTime;

        currentPlacement.text = position.ToString();
        lapText.text = "lap " + (laps + 1) + "/" + totalLaps;

        //MENU
        if (Input.GetButtonDown("Pause"))
        {
            Time.timeScale = 0;
            menuCanvas.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }





    #region ArrivalFunctions

    public void ShowFinalPlacement(int position)
    {
        finalPlacement.text = position + " PLACE!!";
        finalPlacementPanel.SetActive(true);
    }


    public void ShowCheaterPanel()
    {
        if (cheaterTimer >= cheaterTime)
        {
            StartCoroutine(CheaterPanelCoroutine());
            cheaterTimer = 0f;
        }
    }




    IEnumerator CheaterPanelCoroutine()
    {
        cheaterPanel.gameObject.SetActive(true);
        alphaTimer = 0f;

        while (cheaterPanel.color.a > 0f)
        {
            //DISSOLVE EFFECT
            Color tmpColor = cheaterPanel.color;
            tmpColor.a = Mathf.Lerp(cheaterPanelDefaultColor.a, 0, alphaTimer / dissolveDuration);
            cheaterPanel.color = tmpColor;

            tmpColor = cheaterWriting.color;
            tmpColor.a = Mathf.Lerp(cheaterWritingDefaultColor.a, 0, alphaTimer / dissolveDuration);
            cheaterWriting.color = tmpColor;

            alphaTimer += Time.deltaTime;

            yield return null;
        }

        cheaterPanel.gameObject.SetActive(false);
        cheaterPanel.color = cheaterPanelDefaultColor;
        cheaterWriting.color = cheaterWritingDefaultColor;

        yield break;
    }


    #endregion

}
