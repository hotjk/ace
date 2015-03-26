using EasyNetQ;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE
{
    public class ProtoBufSerializer : ISerializer
    {
        private readonly ITypeNameSerializer typeNameSerializer;

        public ProtoBufSerializer(ITypeNameSerializer typeNameSerializer)
        {
            this.typeNameSerializer = typeNameSerializer;
        }

        public object BytesToMessage(string typeName, byte[] bytes)
        {
            var type = typeNameSerializer.DeSerialize(typeName);
            throw new NotImplementedException();
        }

        public T BytesToMessage<T>(byte[] bytes)
        {
            using(Stream stream = new MemoryStream(bytes))
            {

            }
            throw new NotImplementedException();
        }

        public byte[] MessageToBytes<T>(T message) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
