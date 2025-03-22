using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.NET.IO
{
    public class PacketBuilder
    {
        MemoryStream _ms;
        public PacketBuilder()
        {
            _ms = new MemoryStream();
        }

        public void WriteOpCode(byte opcode)
        {
            _ms.WriteByte(opcode);
        }

        public void WriteMessage(string msg)
        {
            byte[] msgBytes = Encoding.UTF8.GetBytes(msg); // Преобразуем строку в байты
            int msgLength = msgBytes.Length; // Определяем длину строки в байтах\

            byte[] lengthBytes = BitConverter.GetBytes(msgLength); // Преобразуем длину в 4 байта
            _ms.Write(lengthBytes, 0, lengthBytes.Length); // Записываем длину
            _ms.Write(msgBytes, 0, msgBytes.Length); // Записываем саму строку
        }

        public byte[] GetPacketBytes()
        {
            return _ms.ToArray();
        }
    }
}
