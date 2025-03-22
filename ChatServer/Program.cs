using System.Net;
using System.Net.Sockets;
using ChatServer.NET.IO;

namespace ChatServer
{
    class Program
    {
        static List<Client> _users;
        static TcpListener _listener;



        static void Main(string[] args)
        {
            _users = new List<Client>();
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7891);
            _listener.Start();



            while (true)
            {
                var client = new Client(_listener.AcceptTcpClient());
                _users.Add(client);

                // broadcast the connection to everyone on the server

                BroadcastConnection();
            }

            static void BroadcastConnection()
            {
                foreach (var user in _users)
                {
                    foreach (var usr in _users)
                    {
                        var broadcastPacket = new PacketBuilder();
                        broadcastPacket.WriteOpCode(1);
                        broadcastPacket.WriteMessage(usr.Username);
                        broadcastPacket.WriteMessage(usr.UID.ToString());
                        user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
                    }
                }
            }
        }


        public static void BroadcastMessage(string message)
        {
            foreach (var user in _users)
            {

                var broadcastPacket = new PacketBuilder();
                broadcastPacket.WriteOpCode(5);
                broadcastPacket.WriteMessage(message);
                user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
            }
        }

        public static void BroadcastDisconnect(string UID)
        {
             var disconnectedUser = _users.Find(x => x.UID.ToString() == UID);
             _users.Remove(disconnectedUser);
             foreach (var user in _users)
             {

                var broadcastPacket = new PacketBuilder();
                broadcastPacket.WriteOpCode(10);
                broadcastPacket.WriteMessage(UID);
                user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
             }
             BroadcastMessage($"{disconnectedUser.Username} has disconnected");
        }
    }
}
