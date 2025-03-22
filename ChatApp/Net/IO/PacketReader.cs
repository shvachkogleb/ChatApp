using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Net.IO
{
    public class PacketReader : BinaryReader
    {
        private NetworkStream _ns;
        public PacketReader(NetworkStream ns) : base(ns)
        {
            _ns = ns;
        }

        public string ReadMessage()
        {
            var Length = ReadInt32(); // Читаем длину сообщения
            byte[] msgBuffer = new byte[Length]; // Создаем буфер для сообщения
            _ns.Read(msgBuffer, 0, Length); // Читаем сообщение

            var msg = Encoding.ASCII.GetString(msgBuffer); // Преобразуем байты в строку

            return msg;
        }
    }
}
