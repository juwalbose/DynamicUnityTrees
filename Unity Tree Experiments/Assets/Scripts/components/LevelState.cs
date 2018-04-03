using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelState  {

	public struct BranchState{
		public float rotation;
		public float length;
		
	}
	public int levelIndex;
	public float width;
	public Vector3 position;
	public float rotation;
	public float length;
	public List<BranchState> logicBranches;
}
