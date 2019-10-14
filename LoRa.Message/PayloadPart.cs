using System;

namespace LoRa.Message
{
    public class PayloadPart<TParent> : PayloadPartBase<TParent> where TParent : IPayloadPart
    {
        protected int Start { get; }
        protected int Length { get; }
        public override Span<byte> RawData => Parent.RawData.Slice(Start, Length);
        public PayloadPart(IPayloadPart parent, int start, int lengt) : base(parent)
        {
            Start = start;
            Length = lengt;
        }
    }
}