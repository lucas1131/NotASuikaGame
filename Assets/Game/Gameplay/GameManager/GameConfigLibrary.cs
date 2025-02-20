using UnityEngine;

[CreateAssetMenu(menuName = "Suika/GameConfigLibrary")]
public class GameConfigLibrary : ScriptableObject
{
    [SerializeField] private GameConfig[] configs;
    public GameConfig[] Configs => configs;
    public int selectedConfigIndex = 0;

    public GameConfig GetConfig(int index) => Configs[index];
}
