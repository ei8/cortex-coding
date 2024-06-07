using System;

namespace ei8.Cortex.Coding
{
    public class Neuron : IEnsembleItem
    {
        public Neuron(Guid id, string tag, string externalReferenceUrl, Guid? regionId) : this(id, false, tag, externalReferenceUrl, regionId)
        {
        }

        public Neuron(Guid id, bool isTransient, string tag, string externalReferenceUrl, Guid? regionId)
        {
            Id = id;
            IsTransient = isTransient;
            Tag = tag;
            ExternalReferenceUrl = externalReferenceUrl;
            RegionId = regionId;
        }

        public static Neuron CreateTransient(string tag, string externalReferenceUrl, Guid? regionId) => new Neuron(Guid.NewGuid(), true, tag, externalReferenceUrl, regionId);

        public string ExternalReferenceUrl { get; private set; }
        public Guid Id { get; private set; }
        public bool IsTransient { get; }
        public Guid? RegionId { get; private set; }
        public string Tag { get; private set; }
    }
}
