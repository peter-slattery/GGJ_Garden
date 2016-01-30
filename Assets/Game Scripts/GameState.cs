using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameState {

	public SpaceGrid 			gameGrid 	= null;

	// NOTE: This function collects all state to be saved
	public static GameState prepareGameState () {
		GameState gS = new GameState ();

		gS.gameGrid = GridController.curInstance;

		return gS;
	}

	// NOTE: This function directs the loaded game state to the appropriate locations
	public static void applyGameState (GameState gS) {
		// TODO: check difference in date times and apply the necessary number of simulation updates (maybe in worker thread?)
		GridController.setCurInstance(gS.gameGrid);
	}
}
