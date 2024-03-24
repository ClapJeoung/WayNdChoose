using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.IO;

public class TwitchConnect : MonoBehaviour
{
  TcpClient Twitch;
  StreamReader Reader;
  StreamWriter Writer;

  const string URL = "irc.chat.twitch.tv";
  const int PORT = 6667;

  public string User = "";
  public string OAuth = "";
  public string Channel = "";

  public void ConnectToTwitch()
  {
    Twitch = new TcpClient(URL, PORT);
    Reader = new StreamReader(Twitch.GetStream());
    Writer=new StreamWriter(Twitch.GetStream());

    Writer.WriteLine("PASS " + OAuth);
    Writer.WriteLine("NICK " + User.ToLower());
  //  Writer.WriteLine("USER " + User+" 8 * "+User);
    Writer.WriteLine("JOIN #" + Channel.ToLower());
    Writer.Flush();
  }
  private void Awake()
  {
    ConnectToTwitch();
  }
  private void Update()
  {
    if (Twitch.Available > 0)
    {
      string message = Reader.ReadLine();

      print("채팅인레후: "+message);
    }
  }
}
