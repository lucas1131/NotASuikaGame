using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Glowsticks : MonoBehaviour {

    SpriteRenderer spriteRenderer;

    [SerializeField, Range(0f, 3f)] float intensity;
    [SerializeField, Range(0f, 1f)] float thickness;
    [SerializeField, Range(0f, 10f)] float detailX;
    [SerializeField, Range(0f, 10f)] float detailY;

    static readonly int intensityID = Shader.PropertyToID("_Intensity");
    static readonly int thicknessID = Shader.PropertyToID("_Thickness");
    static readonly int detailLevelID = Shader.PropertyToID("_DetailLevel");

    void OnEnable() {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        propertyBlock.SetVector(thicknessID, new Vector4(detailX, detailY, 0f, 0f));
        spriteRenderer.SetPropertyBlock(propertyBlock);
    }
}
