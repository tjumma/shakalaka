using System;
using Unity.Netcode;

namespace Shakalaka
{
    [Serializable]
    public struct ClientBoard : INetworkSerializable
    {
        public Pile playerPile;
        public Pile opponentPile;
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            if (serializer.IsReader)
            {
                serializer.SerializeValue(ref playerPile);
                serializer.SerializeValue(ref opponentPile);
            }
            else
            {
                serializer.SerializeValue(ref playerPile);
                serializer.SerializeValue(ref opponentPile);
            }
        }
    }
}