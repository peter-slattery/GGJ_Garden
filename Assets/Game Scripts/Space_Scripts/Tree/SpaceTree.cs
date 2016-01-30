using UnityEngine;
using System.Collections;

public class SpaceTree : MonoBehaviour {

	private static Node rootNode = null;

	public static int propertyIndex = 0;

	public static Tile getTile (CanAddr cAddr) {
		if (rootNode == null) { rootNode = new Node(); }
		return rootNode.getTile(cAddr);
	}

	public static void updateState () {
		if (rootNode == null) { rootNode = new Node(); }
		rootNode.updateState();
	}

	public static void updateProperties () {
		if (rootNode == null) { rootNode = new Node(); }
		propertyIndex = (propertyIndex + 1) % 2;
		rootNode.updateProperties();
	}

	public static void setState (CanAddr cAddr, byte state) {
		if (rootNode == null) { rootNode = new Node(); }
		rootNode.setState(cAddr, state);
	}
}
