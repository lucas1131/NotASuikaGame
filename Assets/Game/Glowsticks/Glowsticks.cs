using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Glowsticks : MonoBehaviour {

    SpriteRenderer spriteRenderer;

    [SerializeField, Range(0f, 3f)] float intensity;
    [SerializeField, Range(0f, 1f)] float thickness;

    static readonly int intensityID = Shader.PropertyToID("_Intensity");
    static readonly int thicknessID = Shader.PropertyToID("_Thickness");

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnValidate(){
        if(spriteRenderer == null){
            return;
        }

        UpdateMaterial();
    }

    void UpdateMaterial() {
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat(intensityID, intensity);
        // Just scale thickness a bit to make it easier to edit in editor
        propertyBlock.SetFloat(thicknessID, thickness/10f);
        spriteRenderer.SetPropertyBlock(propertyBlock);
    }
}
