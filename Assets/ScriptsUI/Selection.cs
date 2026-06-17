using UnityEngine;

public class AnimalSelectionManager : MonoBehaviour
{
    public static string SelectedAnimal;

    public GameObject menuCanvas;
    public GameObject placementHint;

    public void SelectAnimal(string animalName)
    {
        SelectedAnimal = animalName;

        Debug.Log("Gewählt: " + animalName);

        menuCanvas.SetActive(false);
        placementHint.SetActive(true);
    }
}