using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    [Header("Car")]

    [SerializeField] MeshRenderer carTexture;

    [SerializeField] Material[] carMaterials;
    int carMaterialsIndex;


    [Header("Laps")]

    [SerializeField] Text lapsText;
    int laps;







    private void Start()
    {
        carMaterialsIndex = 0;
        laps = 1;

        PlayerPrefs.SetInt("carMaterial", 0);
    }






    #region BUTTONS

    public void PlayButton()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }


    public void ExitButton()
    {
        Application.Quit();
    }

    #endregion


    #region CAR

    public void ChangeCarLeftButton()
    {
        if (carMaterialsIndex - 1 > 0)
            carMaterialsIndex -= 1;
        else
            carMaterialsIndex = carMaterials.Length - 1;

        Material[] tmp = carTexture.materials;
        tmp[0] = carMaterials[carMaterialsIndex];
        carTexture.materials = tmp;

        PlayerPrefs.SetInt("carMaterial", carMaterialsIndex);
    }


    public void ChangeCarRightButton()
    {
        if (carMaterialsIndex + 1 < carMaterials.Length)
            carMaterialsIndex += 1;
        else
            carMaterialsIndex = 0;

        Material[] tmp = carTexture.materials;
        tmp[0] = carMaterials[carMaterialsIndex];
        carTexture.materials = tmp;

        PlayerPrefs.SetInt("carMaterial", carMaterialsIndex);
    }


    #endregion



    #region LAPS

    public void LapsLeftButton()
    {
        if (laps > 0)
        {
            laps--;
            lapsText.text = "Laps: " + laps;
            PlayerPrefs.SetInt("laps", laps);
        }
    }


    public void LapsRightButton()
    {
        laps++;
        lapsText.text = "Laps: " + laps;
        PlayerPrefs.SetInt("laps", laps);
    }


    #endregion

}
