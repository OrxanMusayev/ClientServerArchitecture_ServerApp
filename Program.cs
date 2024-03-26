using System.Net;
using System.Net.Sockets;
using System.Text;

// info about our localhost -- includes the ip adress
IPHostEntry ipentry = await Dns.GetHostEntryAsync(Dns.GetHostName());

//we will extract the local host ip -- 192.168.0.100
IPAddress ip = ipentry.AddressList[0];

// connects the server socket to client socket
IPEndPoint ipEndPoint = new(ip, 1234);

Console.WriteLine(ipEndPoint.ToString());
using Socket server = new(
    ipEndPoint.AddressFamily,
    SocketType.Stream,
    ProtocolType.Tcp
    );

server.Bind(ipEndPoint);
server.Listen();

Console.WriteLine("Server started Listening on port: 1234");

var handler = await server.AcceptAsync();

while (true)
{

    var buffer = new byte[1_234];

    //receive the message from the client but as Bytes
    var received = await handler.ReceiveAsync(buffer, SocketFlags.None);

    //convert bytes to string message
    var messageString = Encoding.UTF8.GetString(buffer, 0, received);

    if(messageString != null)
    {
        Console.WriteLine("Message  from client: {0}", messageString);
        var response = "Message Received!";

        var responseByte = Encoding.UTF8.GetBytes(response);

        await handler.SendAsync(responseByte, SocketFlags.None);
    }
}