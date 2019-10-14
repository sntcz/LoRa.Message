using System;

namespace LoRa.Message
{
    /// <summary>
    /// Message type (MType bit field). 
    /// The LoRaWAN distinguishes between six different MAC message types: 
    /// join request, join accept, unconfirmed data up/down, and confirmed data up/down.
    /// </summary>
    public enum MType
    {
        /// <summary>
        /// 000 - Join Request
        /// </summary>
        JoinRequest,
        /// <summary>
        /// 001 - Join Accept
        /// </summary>
        JoinAccept,
        /// <summary>
        /// 010 - Unconfirmed Data Up
        /// </summary>
        UnconfirmedDataUp,
        /// <summary>
        /// 011 - Unconfirmed Data Down
        /// </summary>
        UnconfirmedDataDown,
        /// <summary>
        /// 100 - Confirmed Data Up
        /// </summary>
        ConfirmedDataUp,
        /// <summary>
        /// 101 - Confirmed Data Down
        /// </summary>
        ConfirmedDataDown,
        /// <summary>
        /// 110 - RFU
        /// </summary>
        RejoinRequest,
        /// <summary>
        /// 111 - Proprietary
        /// </summary>
        Proprietary
    }
}