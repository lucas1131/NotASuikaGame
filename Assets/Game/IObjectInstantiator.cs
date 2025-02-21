using UnityEngine;

public interface IObjectInstantiator {
	T Instantiate<T>(T prefab) where T : Object;
	T Instantiate<T>(T prefab, Transform parent) where T : Object;
	T Instantiate<T>(T prefab, Vector3 position, Quaternion rotation) where T : Object;
}
