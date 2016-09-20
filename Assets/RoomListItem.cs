using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking.Match;

public class RoomListItem : MonoBehaviour {

	[SerializeField]
	private Text roomNameText;

	private string match;

	public void Setup (string _matchName)
	{
		match = _matchName;

		roomNameText.text = match + " (" + match.currentSize + "/" + match.maxSize + ")";
	}

}

