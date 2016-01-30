using UnityEngine;
using System.Collections;

[System.Serializable]
public class SpaceGrid {

	public Node rootNode = null;
	public int propertyIndex = 0;
	
	public Tile getTile (CanAddr cAddr) {
		if (rootNode == null) { rootNode = new Node(); }
		return rootNode.getTile(cAddr);
	}
	
	public void updateState () {
		if (rootNode == null) { rootNode = new Node(); }
		rootNode.updateState();
	}
	
	public void updateProperties () {
		if (rootNode == null) { rootNode = new Node(); }
		propertyIndex = (propertyIndex + 1) % 2;
		rootNode.updateProperties();
	}
	
	public void setState (CanAddr cAddr, byte state) {
		if (rootNode == null) { rootNode = new Node(); }
		rootNode.setState(cAddr, state);
	}
}
