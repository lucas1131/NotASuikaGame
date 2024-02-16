using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	int frameCounter;
	IPieceMergerManager merger;

	public void Setup(IPieceMergerManager merger){
		this.merger = merger;
	}

	void LateUpdate(){
		if(frameCounter >= 3){
			merger?.Merge();
		}
	}
}
