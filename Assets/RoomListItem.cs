using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking.Match;

public class RoomListItem : MonoBehaviour {

//	public delegate void JoinRoomDelegate (MatchDesc _match);
// vid20 12:33 - https://www.youtube.com/watch?v=Oa-jR-3KAEM&list=PLPV2KyIb3jR5PhGqsO7G4PsbEC_Al-kPZ&index=19


	[SerializeField]
	private Text roomNameText;

	private string match;

	public void Setup (string _match)
	{
		match = _match;

//		roomNameText.text = match + " (" + match.currentSize + "/" + match.maxSize + ")";
	}

	public void JoinRoom ()
	{
		
	}

}

