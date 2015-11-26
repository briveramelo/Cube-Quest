using UnityEngine;
using System.Collections;
using System.Linq;

public class Coin : MonoBehaviour {

	public GatherAllKnowledge knowAll;
	public string coinSoundString;
	public string[] playerNames1;
	public string[] playerNames2;
	public HUD hud;

	void Awake(){
		if (name == "Coin") {
			coinSoundString = "Prefabs/Effects/CoinSound1";
		}
		else if (name == "Strawberry"){
			coinSoundString = "Prefabs/Effects/CoinSound2";
		}
		playerNames1 = new string[]{
			"Hero1",
			"CC1.1",
			"CC1.2",
			"CC1.3",
			"CC1.4"
		};
		playerNames2 = new string[]{
			"Hero2",
			"CC2.1",
			"CC2.2",
			"CC2.3",
			"CC2.4"
		};
		knowAll = GameObject.Find ("theOmniscient").GetComponent<GatherAllKnowledge> ();
	}

	void OnTriggerEnter2D(Collider2D col){
		if (playerNames1.Contains (col.name) || playerNames2.Contains (col.name)) {

			if (playerNames1.Contains (col.name)) {
				hud = GameObject.Find ("HUD1").GetComponent<HUD> ();
			}
			else if (playerNames2.Contains (col.name)) {
				hud = GameObject.Find ("HUD2").GetComponent<HUD> ();
			}
			
			knowAll.numCoins++;
			Instantiate (Resources.Load (coinSoundString), transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
}
