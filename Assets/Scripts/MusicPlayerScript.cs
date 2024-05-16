using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerScript : MonoBehaviour {

	static MusicPlayerScript myMusicPlayer = null;
	// Use this for initialization
	void Awake () {

		if(myMusicPlayer == null)
		{
			myMusicPlayer = this;
			//this refers to this current copy of the class
			GameObject.DontDestroyOnLoad(gameObject);
			/* GameObject (with a capital G) is the class which contains the 
			DontDestroyOnLoad method. This method is used so that the object
			we need (in this case, the MusicPlayer object), does not get 
			destroyed when we switch from one scene to another. Therefore,
			the background music, will keep on playing during the game.
			gameObject (with a small g) is a keyword used to refer to the 
			object which contains the current script. In our case, MusicPlayer
			contains the MusicPlayerScript (current script), therefore, 
			gameObject is referring to the MusicPlayer object.
			*/
		}
		else
		{
			Destroy(gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
