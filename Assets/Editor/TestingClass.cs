using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class TestingClass:MonoBehaviour {

	//Check the size of list always have 4 value
	[Test]
	public void CheckListSize(){
		GameController controller = ControllerClass (); //controller class method called
		controller.CircleList = new List<GameObject> ();
		controller.AddItems();
		Assert.AreEqual (controller.CircleList.Count,4);
	}
	//Check random number value is between range of 0,1,2,3
	[Test]
	public void CheckRandomNumber(){
		GameController controller = ControllerClass (); //controller class method called
		int[] randomnumber = new int[] {0, 1, 2,3};
		Assert.AreEqual (controller.index,randomnumber[controller.index]);
	}
	//Check if flash is created
	[Test]
	public void CheckFlashExists(){
		GameController controller = ControllerClass (); //controller class method called
		Assert.NotNull (controller.prefabflash);
	}
	//Check flash position is similar to the position of circle which is picked by unique random number
	[Test]
	public void CheckFlashPosition(){
		GameController controller = ControllerClass (); //controller class method called
		Assert.AreEqual (controller.prefabflash.transform.position.x,controller.CircleList[controller.index].transform.position.x);
		Assert.AreEqual (controller.prefabflash.transform.position.y,controller.CircleList[controller.index].transform.position.y);
	}
	//Check delay of flash after 2 flashes by decreasing it with 0.1f
	[Test]
	public void CheckWaitForFlashes(){
		GameController controller = ControllerClass (); //controller class method called
		controller.CountFlashes = 2;
		controller.WaitForFlashes = 3;
		controller.GenerateFlash ();
		Assert.AreEqual (controller.WaitForFlashes,2.9f);
	}
	//Check score is working correctly by adding
	[Test]
	public void CheckScore(){
		GameObject mygameobject = new GameObject();
		GameUi gameui = mygameobject.AddComponent<GameUi>();
		gameui.ScoreText= gameui.GetComponent<UnityEngine.UI.Text>();
		GameUi.Score = 0;
		gameui.IncrementScore();
		Assert.AreEqual (GameUi.Score,1);
	}
	//Method which will create the instance of GameController class and it is returning instance
	public GameController ControllerClass()
	{
		GameObject mygameobject = new GameObject();
		GameController controller = mygameobject.AddComponent<GameController>();
		controller.CircleList = new List<GameObject> ();
		controller.AddItems();
		controller.Flash = new GameObject();
		controller.prefabflash = new GameObject ();
		controller.GameUiClass = GameObject.Find ("GameController").GetComponent<GameUi>();
		controller.GenerateFlash ();
		return controller;
		
	}


}



