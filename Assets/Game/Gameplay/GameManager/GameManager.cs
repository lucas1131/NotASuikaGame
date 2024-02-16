using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	// worth to have this in config?
	static readonly int referenceFPS = 30;
	static readonly float spf = 1/referenceFPS;

	[SerializeField] float mergeSlackTime = spf * 3f;
	float elapsedTime;
	ISpawner spawner;

	public void Setup(ISpawner spawner){
		this.spawner = spawner;
	}

	void LateUpdate(){
		if(mergeSlackTime >= mergeSlackTime){
			spawner.ConsumeMerges();
			elapsedTime = 0f;
		}
		elapsedTime += Time.deltaTime;
	}
}
