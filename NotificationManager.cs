using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NotificationManager : MonoBehaviour {

	public static bool leaveNotificationON;
	public GameObject [] NotificationPanel;
	public Text [] NotificationEditor;
	private static string txttodisplay = "";
	private static int PanelToDisplay = 0;
	private static bool _notify;
	
	WaitForSeconds _delay = new WaitForSeconds (1f);
	
	void Start () {
	}

		public static void CHAT (string _message)
		{
			PanelToDisplay = 0;
			AddText (_message);
		}

		public static void Notification (string _message)
		{
			PanelToDisplay = 1;
			leaveNotificationON = true;
			AddText (_message);
		}

	//private static int threshhold = 200;
		private static void  AddText (string _text)
	{
		txttodisplay = _text;
		int tmp = _text.Length;
		float _rate = 0.091f; //time taken to read a single character
		float _finalValue = (float) tmp *_rate;
		NotificationTime = new WaitForSeconds (_finalValue);	
		_notify = true;
	}
	private bool endnotification;
	public static WaitForSeconds NotificationTime;
	IEnumerator Notify (GameObject _obj)
	{
		endnotification = true;
		OnNotificationPanel (_obj);
		yield return null;
		if (!leaveNotificationON) {
				yield return NotificationTime;
			if (endnotification) {
				StartCoroutine (OffNoticationPanel (_obj));
			}
		}
	}

	void OnNotificationPanel (GameObject _obj)
	{
		_obj.SetActive (true);
		AnimationAndEffectsManager.PlayChannel (_obj.GetComponent<Animator>(), 0);
		//NotificationPanel [PanelToDisplay].GetComponent<Button> ().interactable = true;
	}

	IEnumerator OffNoticationPanel (GameObject _obj)
	{
			if (_obj.GetComponent<Animator> () != null) {
				AnimationAndEffectsManager.PlayChannel (_obj.GetComponent<Animator> (), 1);
				yield return _delay;
			} else {
			_obj.SetActive (false);
		}
		}
	
	/// <summary>
	/// This bool sets the display off
	/// </summary>
	public static bool enddisplay;
	public void EndDisplay ()
	{
		StartCoroutine (OffNoticationPanel(NotificationPanel[PanelToDisplay]));
		endnotification = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (_notify) {
			_notify = false;
			StartCoroutine (Notify(NotificationPanel[PanelToDisplay]));
		}
		if (enddisplay) {
			enddisplay = false;
			EndDisplay ();
		}

			NotificationEditor [PanelToDisplay].text = txttodisplay;
	}
}
