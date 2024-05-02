using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	// Is it worth to move this to config?
	static readonly int referenceFPS = 30;
	static readonly float secondsPerFrame = 1/referenceFPS;

	[SerializeField] float mergeSlackTime = secondsPerFrame * 6f;
	float elapsedTime;
	ISpawner spawner;

	public void Setup(ISpawner spawner){
		this.spawner = spawner;
	}

	void LateUpdate(){
		if(elapsedTime >= mergeSlackTime){
			spawner.ConsumeMerges();
			elapsedTime = 0f;
		}
		elapsedTime += Time.deltaTime;
	}
}
