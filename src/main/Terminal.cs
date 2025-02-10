using System;

namespace ei8.Cortex.Coding
{
    public class Terminal : INetworkItem
    {
        public Terminal(Guid id, Guid presynapticNeuronId, Guid postsynapticNeuronId, NeurotransmitterEffect effect, float strength) : this(id, false, presynapticNeuronId, postsynapticNeuronId, effect, strength)
        {
        }

        public Terminal(Guid id, bool isTransient, Guid presynapticNeuronId, Guid postsynapticNeuronId, NeurotransmitterEffect effect, float strength)
        {
            Id = id;
            IsTransient = isTransient;
            PresynapticNeuronId = presynapticNeuronId;
            PostsynapticNeuronId = postsynapticNeuronId;
            Effect = effect;
            Strength = strength;
        }

        public static Terminal CreateTransient(Guid presynapticNeuronId, Guid postsynapticNeuronId) => 
            new Terminal(Guid.NewGuid(), true, presynapticNeuronId, postsynapticNeuronId, NeurotransmitterEffect.Excite, 1f);

        public static Terminal CloneAsPersistent(Terminal value) =>
            new Terminal(value.Id, value.PresynapticNeuronId, value.PostsynapticNeuronId, value.Effect, value.Strength);

        public Guid Id { get; private set; }
        public bool IsTransient { get; }
        public Guid PresynapticNeuronId { get; private set; }
        public Guid PostsynapticNeuronId { get; private set; }
        public NeurotransmitterEffect Effect { get; private set; }
        public float Strength { get; private set; }
    }
}
