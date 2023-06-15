using System;
using Unity.Netcode;

namespace Shakalaka
{
    [Serializable]
    public struct ClientBoardData : INetworkSerializable
    {
        public PileData playerHandData;
        public PileData opponentHandData;
        public PileData playerAreaData;
        public PileData opponentAreaData;
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            if (serializer.IsReader)
            {
                serializer.SerializeValue(ref playerHandData);
                serializer.SerializeValue(ref opponentHandData);
                serializer.SerializeValue(ref playerAreaData);
                serializer.SerializeValue(ref opponentAreaData);
            }
            else
            {
                serializer.SerializeValue(ref playerHandData);
                serializer.SerializeValue(ref opponentHandData);
                serializer.SerializeValue(ref playerAreaData);
                serializer.SerializeValue(ref opponentAreaData);
            }
        }
    }
}