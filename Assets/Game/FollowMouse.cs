using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour {

    [SerializeField] GameObject obj;
    [SerializeField, Range(0.1f, 10f)] float maxSpeed = 3f;
    [SerializeField, Range(0f, 1f)] float smoothTime = 0.2f;

    Vector2 velocity;

    void Update(){
        float mouseX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        Vector2 target = new Vector2(mouseX, obj.transform.position.y);
        obj.transform.position = Vector2.SmoothDamp(obj.transform.position, target, ref velocity, smoothTime, maxSpeed);
    }
}
