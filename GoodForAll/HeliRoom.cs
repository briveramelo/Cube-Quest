using UnityEngine;
using System.Collections;

public class HeliRoom : MonoBehaviour {

	private string heliBlockString;
	private MiniCopterBlock[] heliBlocks;
	private Vector3[] heliPositions;
	private int i;
	private int j;
	private AudioSource[] songs;
	private float[] songLengths;
	private int numSongs;
	// Use this for initialization
	void Awake () {
		heliBlockString = "Prefabs/Enemies/MiniCopterBlock"; 
		heliBlocks = GameObject.FindObjectsOfType<MiniCopterBlock> ();
		i = 0;
		heliPositions = new Vector3[heliBlocks.Length];
		foreach (MiniCopterBlock mini in heliBlocks) {
			heliPositions[i] = mini.transform.position;
			i++;
		}
		songs = GameObject.Find ("MainCamera").GetComponents<AudioSource> ();
		numSongs = songs.Length;
		songLengths = new float[numSongs];
		i = 0;
		foreach (AudioSource song in songs) {
			songLengths[i] = song.clip.length;
			i++;
		}
		i = 0;
		StartCoroutine (Songs (songLengths[i]));
	}

	public IEnumerator Songs(float waitTime){
		songs [i].Play ();
		yield return new WaitForSeconds (waitTime);
		songs [i].Stop ();
		i++;
		if (i > numSongs-1) {
			i=0;
		}
		StartCoroutine (Songs (songLengths [i]));
	}

	public IEnumerator SpawnCopters(){
		j = 0;
		foreach (Vector3 pos in heliPositions){
			if (!heliBlocks[j]){
				Instantiate ( Resources.Load (heliBlockString),pos,Quaternion.identity);
			}
			j++;
		}
		yield return null;
	}
}
