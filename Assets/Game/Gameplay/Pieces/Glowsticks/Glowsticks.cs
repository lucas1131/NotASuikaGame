using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Glowsticks : MonoBehaviour {

    SpriteRenderer spriteRenderer;

    [SerializeField, Range(0f, 3f)] float intensity;
    [SerializeField, Range(0f, 1f)] float thickness;
    [SerializeField, Range(0f, 10f)] float animationScaleX;
    [SerializeField, Range(0f, 10f)] float animationScaleY;
    [SerializeField, Range(0f, 10f)] float animationIntensity;
    [SerializeField, Range(0f, 100f)] float animationTimescale;

    static readonly int intensityID = Shader.PropertyToID("_Intensity");
    static readonly int thicknessID = Shader.PropertyToID("_Thickness");
    static readonly int animationID = Shader.PropertyToID("_AnimationSettings");

    void OnEnable() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateMaterial();
    }

    void OnValidate(){

        // This is editor only so dont really care about performance here
        // Actually this entire script is mostly for editor editing the object
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(spriteRenderer == null){
            return;
        }

        UpdateMaterial();
    }

    void UpdateMaterial() {
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat(intensityID, intensity);
        // Just scale down thickness a bit to make it easier to edit in editor
        propertyBlock.SetFloat(thicknessID, thickness/10f);
        propertyBlock.SetVector(animationID, new Vector4(animationScaleX, animationScaleY, animationIntensity, animationTimescale));
        spriteRenderer.SetPropertyBlock(propertyBlock);
    }
}
