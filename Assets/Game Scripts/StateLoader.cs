using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.IO;

public class StateLoader {

	public static void saveGame () {
		BinaryFormatter bF = new BinaryFormatter ();
		FileStream outFile = new FileStream (Application.persistentDataPath + "/gameSave.dat", FileMode.Create, FileAccess.Write); 
		bF.Serialize (outFile, GameState.prepareGameState ());
		outFile.Close ();
	}

	public static void loadGame () {
		if (File.Exists(Application.persistentDataPath + "/gameSave.dat")) {
			BinaryFormatter bF = new BinaryFormatter();
			FileStream inFile = File.Open(Application.persistentDataPath + "/gameSave.dat", FileMode.Open);
			GameState gS = (GameState) bF.Deserialize(inFile);
			inFile.Close();

			GameState.applyGameState(gS);
		}
	}
}