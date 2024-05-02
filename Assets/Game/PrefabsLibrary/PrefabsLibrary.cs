using UnityEngine; 

[CreateAssetMenu(menuName = "Suika/PrefabsLibrary")]
public class PrefabsLibrary : ScriptableObject
{
    [Tooltip("This list defines which prefab appears in which order in game logic")]
    public PieceGraphics[] piecesPrefabList;
    // TODO: this should become addressables path to load so we can distribute whatever assets are necessary remotely
    // instead of using this scriptable object to reference assets
    // this would also make them a weak reference and avoid loading everything at startup
    // static readonly string addressablePrefabPath = "";
}
