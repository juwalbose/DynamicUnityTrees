using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class LSystemTree : MonoBehaviour {

	List<Branch> branches;
	bool isDebug=false;
	Mesh lTree;
	List<int> faces;
	List<Vector3> vertices;

	public string[] vars;
	Dictionary<string,string> rules;
	string lString;
	public int iterations=3;
	public int angle=25;
	public float startLength=2;
	public float startWidth=1;

	List<LevelState> levelStates;
	public Vector3 treeOrigin;
	public float widthDecreaseFactor;
	public float lengthDecreaseFactor;
	public bool useRandomAngle;
	

	// Use this for initialization
	void Start () {
		lString=vars[0];
		rules=new Dictionary<string, string>();
		rules[vars[0]]="F+[+FF-F-F]-[-F+F+FF]";//"FF+[+F-F-F]-[-F+F+F]";
		for(int i=0;i<iterations;i++){
			lString=IterateLString(lString);
		}
		
		Debug.Log(lString);

		branches=new List<Branch>();
		faces=new List<int>();
		vertices=new List<Vector3>();
		lTree=GetComponent<MeshFilter>().mesh;
		lTree.name="l system tree";
		CreateTree();
	}
	string IterateLString(string currentLString){
		string newLString="";
		char[] chars=currentLString.ToCharArray();
		for(int i=0;i<chars.Length;i++){
			if(rules.ContainsKey(chars[i].ToString())){
				newLString+=rules[chars[i].ToString()];
			}else{
				newLString+=chars[i].ToString();
			}

		}
		return newLString;
	}
	private void CreateTree(){
		levelStates=new List<LevelState>();
		char[] chars=lString.ToCharArray();
		float currentRotation=0;
		float currentLength=startLength;
		float currentWidth=startWidth;
		Vector3 currentPosition=treeOrigin;
		int levelIndex=0;
		LevelState levelState=new LevelState();
		levelState.position=currentPosition;
		levelState.levelIndex=levelIndex;
		levelState.width=currentWidth;
		levelState.length=currentLength;
		levelState.rotation=currentRotation;
		levelState.logicBranches=new List<LevelState.BranchState>();
		levelStates.Add(levelState);
		Vector3 tipPosition=new Vector3();
		Queue<LevelState> savedStates=new Queue<LevelState>();
		for(int i=0;i<chars.Length;i++){
			switch(chars[i]){
				case 'F':
					if(levelState.levelIndex!=levelIndex){
						foreach(LevelState ls in levelStates){
							if(ls.levelIndex==levelIndex){
								levelState=ls;
								break;
							}
						}	
					}
					tipPosition.x=levelState.position.x+(currentLength*Mathf.Sin(Mathf.Deg2Rad*currentRotation));
					tipPosition.y=levelState.position.y+(currentLength*Mathf.Cos(Mathf.Deg2Rad*currentRotation));
					levelIndex++;
					LevelState.BranchState branchState=new LevelState.BranchState();
					branchState.rotation=currentRotation;
					branchState.length=currentLength;
					levelState.logicBranches.Add(branchState);

					currentWidth*=widthDecreaseFactor;

					currentPosition=tipPosition;
					levelState=new LevelState();
					levelState.position=currentPosition;
					levelState.levelIndex=levelIndex;
					levelState.width=currentWidth;
					levelState.length=currentLength;
					levelState.rotation=currentRotation;
					levelState.logicBranches=new List<LevelState.BranchState>();
					levelStates.Add(levelState);

					currentLength*=lengthDecreaseFactor;
					
				break;
				case '+':
					if(useRandomAngle){
						currentRotation+=Random.Range(angle-5,angle+5);
					}else{
						currentRotation+=angle;
					}
					
				break;
				case '-':
					if(useRandomAngle){
						currentRotation-=Random.Range(angle-5,angle+5);
					}else{
						currentRotation-=angle;
					}
				break;
				case '[':
					savedStates.Enqueue(levelState);
				break;
				case ']':
					levelState=savedStates.Dequeue();
					currentPosition=levelState.position;
					currentRotation=levelState.rotation;
					currentLength=levelState.length;
					currentWidth=levelState.width;
					levelIndex=levelState.levelIndex;
				break;
			}
		}
		
		for(int i=0;i<levelStates.Count;i++){
			levelState=levelStates[i];
			int baseVertexPointer=vertices.Count;
			for(int j=0;j<levelState.logicBranches.Count;j++){
				LevelState.BranchState bs=levelState.logicBranches[j];
				Branch branch=new Branch(Vector3.zero,bs.length,levelState.width,bs.rotation, levelState.position,widthDecreaseFactor);
				branches.Add(branch);
				if(i==0&&j==0){
					vertices.AddRange(branch.vertices);
					faces.Add(baseVertexPointer);
					faces.Add(baseVertexPointer+1);
					faces.Add(baseVertexPointer+3);
					faces.Add(baseVertexPointer+3);
					faces.Add(baseVertexPointer+1);
					faces.Add(baseVertexPointer+2);
					baseVertexPointer=vertices.Count;
				}else{
					int vertexPointer=vertices.Count;
					vertices.Add(branch.vertices[1]);
					vertices.Add(branch.vertices[2]);
					Vector2Int vertexPointerXX=GetClosestVextexIndices(branch.vertices[0],branch.vertices[3],vertices,i);
					faces.Add(vertexPointerXX.x);
					faces.Add(vertexPointer);
					faces.Add(vertexPointerXX.y);
					faces.Add(vertexPointerXX.y);
					faces.Add(vertexPointer);
					faces.Add(vertexPointer+1);
				}
				
			}
		}
		 
		lTree.vertices=vertices.ToArray();
		lTree.triangles = faces.ToArray();
		lTree.RecalculateNormals();
		Debug.Log("Tree has "+vertices.Count.ToString()+" vertices");
		
	}

    private Vector2Int GetClosestVextexIndices(Vector3 pos1, Vector3 pos2, List<Vector3> vertexList, int index)
    {
        Vector2Int pointer=new Vector2Int();
		if(index==0){
			pointer.x=0;
			pointer.y=3;
		}else{
			float distance1=10;
			float distance2=10;
			float distance;
			Vector3 pos3;
			for(int i=0;i<vertexList.Count;i++){
				pos3=vertexList[i];
				distance=Vector3.Distance(pos3,pos1);
				if(distance<distance1){
					distance1=distance;
					pointer.x=i;
				}
				distance=Vector3.Distance(pos3,pos2);
				if(distance<distance2){
					distance2=distance;
					pointer.y=i;
				}
			}
		}
		return pointer;
    }

    private void OnDrawGizmos () {
		if(!isDebug)return;
		if (branches == null) {
			return;
		}
		Gizmos.color = Color.green;
		foreach(Branch br in branches){
			for (int i = 0; i < br.vertices.Count; i++) {
				Gizmos.DrawSphere(br.vertices[i], 0.05f);
			}
		}
		
	}
	
}
