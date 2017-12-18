using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;
using System.Text.RegularExpressions;
public class BlendshapesDataReciever : MonoBehaviour {

	// Use this for initialization

	Thread receiveThread;
	UdpClient client;
	public int port;

	//info

	public string lastReceivedUDPPacket = "";
	public string allReceivedUDPPackets = "";

	void Start () {
		init();
		//ExtractResults();
	}
public string[] ExtractResults(string socket_string_input)
{
	 string str= "[[100 100]\n[200 10]\n[10 30]\n[20 20]]";
	 string[] the_coords= new string[68*2]; //landmarks x,y
	 String regex = @"[\[\]']+";
	 Regex regex_newline = new Regex("(\r\n|\r|\n)");   // remove the '+'

	string output1= Regex.Replace(socket_string_input,regex,"");

    string output2= regex_newline.Replace(output1, ",");
    string result = Regex.Replace(output2, @"(?<=\d)\p{Zs}(?=\d)", ",");
	//Debug.Log("sadasdfg "+result);
      //Debug.Log(test);
	 // Debug.Log(test2);

	return Regex.Split(result,",");
	
	 //making the list :D 

	//put the exteract method here dude 
	
	
}
	void OnGUI(){
		Rect  rectObj=new Rect (40,10,200,400);
		
		GUIStyle  style  = new GUIStyle ();
		
		style .alignment  = TextAnchor.UpperLeft;
		
		GUI .Box (rectObj,"# UDPReceive\n127.0.0.1 "+port +" #\n"
		          
		          //+ "shell> nc -u 127.0.0.1 : "+port +" \n"
		          
		          + "\nLast Packet: \n"+ lastReceivedUDPPacket
		          
		          //+ "\n\nAll Messages: \n"+allReceivedUDPPackets
		          
		          ,style );

	}

	private void init(){
		print ("UPDSend.init()");

		port = 5065;

		print ("Sending to 127.0.0.1 : " + port);

		receiveThread = new Thread (new ThreadStart(ReceiveData));
		receiveThread.IsBackground = true;
		receiveThread.Start ();

	}

	private void ReceiveData(){
		client = new UdpClient (port);
		while (true) {
			try{
				IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
				byte[] data = client.Receive(ref anyIP);

				string text = Encoding.UTF8.GetString(data);
				string[] results= ExtractResults(text);
				 Debug.Log(results[0]);
				//elimenate first "[]" and then make the sub method 
				Debug.Log(text);
				lastReceivedUDPPacket=text;
				allReceivedUDPPackets=allReceivedUDPPackets+text;
				//xPos = float.Parse(text);
			//	xPos *= 0.021818f;
			}catch(Exception e){
				print (e.ToString());
			}
		}
	}

	public string getLatestUDPPacket(){
		allReceivedUDPPackets = "";
		return lastReceivedUDPPacket;
	}
	
	// Update is called once per frame
	void Update () {
	//	hero.transform.position = new Vector3(xPos-6.0f,-3,0);
	}

	void OnApplicationQuit(){
		if (receiveThread != null) {
			receiveThread.Abort();
			Debug.Log(receiveThread.IsAlive); //must be false
		}
	}
}
