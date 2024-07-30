using System;

namespace ei8.Cortex.Coding
{
    public class Neuron : IEnsembleItem
    {
        public Neuron(
            Guid id, 
            string tag, 
            string externalReferenceUrl, 
            Guid? regionId,
            string regionTag,
            DateTimeOffset? creationTimestamp,
            Guid creationAuthorId,
            string creationAuthorTag,
            DateTimeOffset? unifiedLastModificationTimestamp,
            Guid? unifiedLastModificationAuthorId,
            string unifiedLastModificationAuthorTag,
            string url,
            int version
            ) : this(id, false, tag, externalReferenceUrl, regionId)
        {
            this.RegionTag = regionTag;
            this.CreationTimestamp = creationTimestamp;
            this.CreationAuthorId = creationAuthorId;
            this.CreationAuthorTag = creationAuthorTag;
            this.UnifiedLastModificationTimestamp = unifiedLastModificationTimestamp;
            this.UnifiedLastModificationAuthorId = unifiedLastModificationAuthorId;
            this.UnifiedLastModificationAuthorTag = unifiedLastModificationAuthorTag;
            this.Url = url;
            this.Version = version;
        }

        private Neuron(Guid id, bool isTransient, string tag, string externalReferenceUrl, Guid? regionId)
        {
            this.IsTransient = isTransient;
            this.Id = id;
            this.Tag = tag;
            this.ExternalReferenceUrl = externalReferenceUrl;
            this.RegionId = regionId;
        }

        public static Neuron CreateTransient(
            string tag,
            string externalReferenceUrl,
            Guid? regionId
            ) => Neuron.CreateTransient(Guid.NewGuid(), tag, externalReferenceUrl, regionId);

        public static Neuron CreateTransient(
            Guid id, 
            string tag, 
            string externalReferenceUrl, 
            Guid? regionId
            ) => new Neuron(id, true, tag, externalReferenceUrl, regionId);

        public bool IsTransient { get; }
        
        public Guid Id { get; private set; }
        public string Tag { get; set; }
        public string ExternalReferenceUrl { get; set; }
        public Guid? RegionId { get; set; }
        public string RegionTag { get; private set; }
        public DateTimeOffset? CreationTimestamp { get; private set; }
        public Guid CreationAuthorId { get; private set; }
        public string CreationAuthorTag { get; private set; }
        public DateTimeOffset? UnifiedLastModificationTimestamp { get; private set; }
        public Guid? UnifiedLastModificationAuthorId { get; private set; }
        public string UnifiedLastModificationAuthorTag { get; private set; }
        public string Url { get; }
        public int Version { get; private set; }
    }
}
