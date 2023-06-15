using System;
using Unity.Netcode;

namespace Shakalaka
{
    [Serializable]
    public struct PileData : INetworkSerializable
    {
        public PileVisibility visibility;
        public int[] cards;
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            if (serializer.IsReader)
            {
                var reader = serializer.GetFastBufferReader();
                reader.ReadValueSafe(out visibility);
                reader.ReadValueSafe(out cards);
            }
            else
            {
                var writer = serializer.GetFastBufferWriter();
                writer.WriteValueSafe(visibility);
                writer.WriteValueSafe(cards);
            }
        }
    }
}