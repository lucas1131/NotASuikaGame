using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	int frameCounter;
	ISpawner spawner;

	public void Setup(ISpawner spawner){
		this.spawner = spawner;
	}

	void LateUpdate(){
		if(frameCounter >= 3){
			spawner.ConsumeMerges();
		}
		frameCounter++;
	}
}
