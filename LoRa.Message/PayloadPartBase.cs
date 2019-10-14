using System;
using System.Diagnostics;

namespace LoRa.Message
{
    [DebuggerDisplay("{debuggerDisplay,nq}")]
    public abstract class PayloadPartBase<TParent> : IPayloadPart where TParent : IPayloadPart
    {
        public TParent Parent { get; }

        public abstract Span<byte> RawData { get; }

        private string debuggerDisplay => BitConverter.ToString(RawData.ToArray());

        protected PayloadPartBase(IPayloadPart parent)
        {
            Parent = (TParent)parent;
        }
    }
}