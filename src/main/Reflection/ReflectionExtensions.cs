﻿using System;
using System.Linq;
using System.Reflection;
using ei8.Cortex.Coding.Properties;
using ei8.Cortex.Coding.Properties.Neuron;

namespace ei8.Cortex.Coding.Reflection
{
    internal static class ReflectionExtensions
    {
        internal static PropertyData ToPropertyData(this PropertyInfo property, object instance = null)
        {
            PropertyData result = null;
            var ignore = property.GetCustomAttributes<neurULIgnoreAttribute>().SingleOrDefault();
            if (ignore == null)
            {
                var neuronPropertyAttribute = property.GetCustomAttributes<neurULNeuronPropertyAttribute>().SingleOrDefault();
                if (neuronPropertyAttribute != null)
                {
                    result = ReflectionExtensions.GetNeuronPropertyData(
                        neuronPropertyAttribute, 
                        property.Name,
                        instance != null ? property.GetValue(instance) : null
                    );
                }
                else
                {
                    var classAttribute = property.GetCustomAttributes<neurULClassAttribute>().SingleOrDefault();
                    // if property type is Guid and property is decorated by classAttribute
                    var matchBy = property.PropertyType == typeof(Guid) && classAttribute != null ?
                            // match by id
                            ValueMatchBy.Id :
                            // otherwise, match by tag
                            ValueMatchBy.Tag;
                    var propertyValue = instance != null ?
                        property.GetValue(instance)?.ToString() :
                        null;
                    string propertyKey = ExternalReference.ToKeyString(property);

                    // if classAttribute was specified
                    var classKey = string.Empty;

                    if (classAttribute != null)
                    {
                        if (classAttribute.Type != null)
                            // use classAttribute type
                            classKey = ExternalReference.ToKeyString(classAttribute.Type);
                        else
                            classKey = string.Empty;
                    }
                    else
                        // otherwise, use property type
                        classKey = ExternalReference.ToKeyString(property.PropertyType);

                    result = new PropertyData(
                        propertyKey,
                        classKey,
                        propertyValue,
                        matchBy
                    );
                }
            }

            return result;
        }

        private static PropertyData GetNeuronPropertyData(neurULNeuronPropertyAttribute neuronPropertyAttribute, string propertyName, object propertyValue)
        {
            PropertyData result = null;

            var neuronPropertyName = neuronPropertyAttribute.PropertyName ?? propertyName;
            INeuronProperty neuronProperty;
            switch (neuronPropertyName)
            {
                case nameof(Neuron.Id):
                    neuronProperty = NeuronPropertyBase<Guid>.Create<IdProperty>(propertyValue, propertyName);
                    break;
                case nameof(Neuron.Tag):
                    neuronProperty = NeuronPropertyBase<string>.Create<TagProperty>(propertyValue, propertyName);
                    break;
                case nameof(Neuron.ExternalReferenceUrl):
                    neuronProperty = NeuronPropertyBase<string>.Create<ExternalReferenceUrlProperty>(propertyValue, propertyName); 
                    break;
                case nameof(Neuron.RegionId):
                    neuronProperty = NeuronPropertyBase<Guid?>.Create<RegionIdProperty>(propertyValue, propertyName); 
                    break;
                case nameof(Neuron.RegionTag):
                    neuronProperty = NeuronPropertyBase<string>.Create<RegionTagProperty>(propertyValue, propertyName);
                    break;
                case nameof(Neuron.CreationTimestamp):
                    neuronProperty = NeuronPropertyBase<DateTimeOffset?>.Create<CreationTimestampProperty>(propertyValue, propertyName); 
                    break;
                case nameof(Neuron.CreationAuthorId):
                    neuronProperty = NeuronPropertyBase<Guid>.Create<CreationAuthorIdProperty>(propertyValue, propertyName); 
                    break;
                case nameof(Neuron.CreationAuthorTag):
                    neuronProperty = NeuronPropertyBase<string>.Create<CreationAuthorTagProperty>(propertyValue, propertyName); 
                    break;
                case nameof(Neuron.UnifiedLastModificationTimestamp):
                    neuronProperty = NeuronPropertyBase<DateTimeOffset?>.Create<UnifiedLastModificationTimestampProperty>(propertyValue, propertyName);
                    break;
                case nameof(Neuron.UnifiedLastModificationAuthorId):
                    neuronProperty = NeuronPropertyBase<Guid?>.Create<UnifiedLastModificationAuthorIdProperty>(propertyValue, propertyName); 
                    break;
                case nameof(Neuron.UnifiedLastModificationAuthorTag):
                    neuronProperty = NeuronPropertyBase<string>.Create<UnifiedLastModificationAuthorTagProperty>(propertyValue, propertyName); 
                    break;
                case nameof(Neuron.Url):
                    neuronProperty = NeuronPropertyBase<string>.Create<UrlProperty>(propertyValue, propertyName); 
                    break;
                case nameof(Neuron.Version):
                    neuronProperty = NeuronPropertyBase<int>.Create<VersionProperty>(propertyValue, propertyName);
                    break;
                default:
                    throw new NotImplementedException($"Neuron Property '{neuronPropertyName}' not yet implemented.");
            }

            if (neuronProperty != null)
                result = new PropertyData(neuronProperty);
        
            return result;
        }
    }
}
