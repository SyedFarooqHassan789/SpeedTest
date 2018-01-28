using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	const int Size=4;
	public List<GameObject> CircleList;
	public GameObject Flash,prefabflash;
	public int index,CountFlashes;
	public float WaitForFlashes,MissingTime;
	public GameUi GameUiClass;
	public bool isflashdestoyed;
	public bool isGameStarted;
	int UniqueChecker;
	IEnumerator coroutine;
	public Ray ray;
	public RaycastHit hit;

	//Start method is used to initialize all the variables
	//It is unity default method which will be called at start for one time in game lifecycle
	void Start()
	{
		isGameStarted = false;
		isflashdestoyed = false;
		MissingTime = 5f;
		WaitForFlashes = 3f;
		CountFlashes = 0;
		index =Random.Range(0,Size);
		GameUiClass = GameObject.Find ("GameController").GetComponent<GameUi> (); //Getting instance of class gameui to used later
		prefabflash = new GameObject (); 
		AddItems (); //calling add items which add the items is list
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0) && isGameStarted) {  //Check an mouse clicking event
			ray = Camera.main.ScreenPointToRay( Input.mousePosition );
			if( Physics.Raycast( ray, out hit, 100 )){ 	//create a ray of raycast from camera with length 100 from camera
				if (prefabflash.gameObject == null) { 	//if the prefab(flash) not instantiated and circle is clicked show game over
					ShowGameOver (); //Call Gameover Method 
				}
				else if (hit.collider.transform.position.x == prefabflash.transform.position.x && hit.collider.transform.position.y == prefabflash.transform.position.y) { //if the the prefab(value) position is equal to collider of circle where ray hit
					DestroyFlash (prefabflash); //Call destroy function to destroy flash
					GameUiClass.IncrementScore (); //Call increment score
					GameUiClass.ShowScore (); //Call show score from other class
					isflashdestoyed = true; 
				}else {
					ShowGameOver ();  //Call Gameover Method
				}
			}
		}
	}
	//This function add the items in the list at the very begining in unity start default method
	public void AddItems(){
		GameObject circles = GameObject.Find("Circles");   //Find gameobject circle children from unity editor
		for (int i = 0; i < Size; i++) {
			CircleList.Add (circles.gameObject.transform.GetChild(i).gameObject); //add it into list
		}
	}
	//This method is used to generate flash
	//It will be called first time from the script gameui when game start image is clicked
	public void GenerateFlash(){
		if (!GameUiClass.isGameOver) {
			//Generating a unique random number so that flashes does not repeat itself
			while (index == UniqueChecker) {
				index = Random.Range (0, Size); //creating a random number
			}
			UniqueChecker = index;
			Vector3 positionofcircle = CircleList [index].gameObject.transform.position; //taking position of random circle where flash will be genearated
			Vector3 flashposition = new Vector3 (positionofcircle.x, positionofcircle.y, -1.86f); //Storing position of random circle position to pass it to Flash
			prefabflash = (GameObject)Instantiate (Flash, flashposition, CircleList [index].gameObject.transform.rotation); //Creating flash at random psotion 
			CountFlashes = CountFlashes + 1; //Counting flashes
			StartCoroutine (UpdateFlash (prefabflash)); //starting a coroutine to include intervals and delays
		} else {
			StopCoroutine (UpdateFlash (prefabflash));
		}
	}
	//Update Flash Timing 
	IEnumerator UpdateFlash(GameObject myflash) {
		if(CountFlashes > 2){
			WaitForFlashes = WaitForFlashes - 0.1f;  //After 2 flash decrease time b/w flashes by 0.1f as by requirement
			if (WaitForFlashes <= 1f) { 
				GameUiClass.GameOver (); //Showing game over from gameui class
			}
		}
		yield return new WaitForSeconds(WaitForFlashes); //Adding delays
		if (isflashdestoyed){ 
			GenerateFlash (); 			//Again call Generate Flash function
			isflashdestoyed = false;
		}else{
			yield return new WaitForSeconds (MissingTime); //Check if user has missed to click circle as time is 5s from requirement
			DestroyFlash (myflash); //If missed destroy flash and show game over
			GameUiClass.GameOver ();
		}

	}
	//Method to destroy flash
	public void DestroyFlash(GameObject myflash){
		Destroy (myflash);
	}
	//Method to show gameover from gameui class
	public void ShowGameOver(){
		DestroyFlash (prefabflash);
		GameUiClass.GameOver ();
	}
}
