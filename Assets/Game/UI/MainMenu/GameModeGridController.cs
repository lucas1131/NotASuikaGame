using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameModeGridController : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private GameObject gameModeButtonPrefab;
    [SerializeField] private GameConfigLibrary gameConfigLibrary;
    private List<Button> gameModeButtons;

    private void Start()
    {
        gameModeButtons = new List<Button>();
        for (int i = 0; i < gameConfigLibrary.Configs.Length; i++)
        {
            GameObject newButtonObj = Instantiate(gameModeButtonPrefab, transform);
            
            Button newButton = newButtonObj.GetComponent<Button>();
            int configIndex = i;
            newButton.onClick.AddListener(() => LoadGame(configIndex));
            
            newButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = gameConfigLibrary.Configs[configIndex].DisplayName;
            
            gameModeButtons.Add(newButton);
        }
    }

    private void OnDestroy()
    {
        foreach (Button button in gameModeButtons)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    private void LoadGame(int configIndex)
    {
        gameConfigLibrary.selectedConfigIndex = configIndex;
        SceneManager.LoadScene(sceneName);
    }
}
