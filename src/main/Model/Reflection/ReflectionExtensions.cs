using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ei8.Cortex.Coding.Model.Properties;
using ei8.Cortex.Coding.Model.Properties.Neuron;

namespace ei8.Cortex.Coding.Model.Reflection
{
    public static class ReflectionExtensions
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
                    result = GetNeuronPropertyData(
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

                    string propertyValue = null;

                    if (instance != null)
                    {
                        var propInitValue = property.GetValue(instance);
                        if (propInitValue != null)
                        {
                            if (property.PropertyType == typeof(DateTimeOffset))
                                // ei8.Cortex.Coding.d23.neurULization.Implementation.neurULizer.DeneurULize
                                // uses "o" format-specifier (RoundtripKind)
                                propertyValue = ((DateTimeOffset) propInitValue).ToUniversalTime().ToString("o");
                            else if (propInitValue != null)
                                propertyValue = propInitValue.ToString();
                        }
                    }

                    string propertyKey = property.ToKeyString();

                    var classKey = string.Empty;

                    // if classAttribute was specified
                    if (classAttribute != null)
                    {
                        // and type was specified
                        if (classAttribute.Type != null)
                            // use classAttribute type
                            classKey = classAttribute.Type.ToKeyString();
                        else
                            classKey = string.Empty;
                    }
                    else
                        // otherwise, use property type
                        classKey = property.PropertyType.ToKeyString();

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
                case nameof(Neuron.MirrorUrl):
                    neuronProperty = NeuronPropertyBase<string>.Create<MirrorUrlProperty>(propertyValue, propertyName); 
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

        /// <summary>
        /// Gets Mirror keys from the specified Types and the applicable members thereof.
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static IEnumerable<object> GetMirrorKeys(params Type[] types)
        {
            var result = new List<object>();

            foreach (var type in types)
            {
                result.Add(type);
                result.AddRange(
                    type.GetProperties()
                        .Where(prop => !prop.GetCustomAttributes<neurULNeuronPropertyAttribute>().Any())
                );
            }

            return result;
        }

        public static string ToKeyString(this Enum value) => value.ToString();

        public static string ToKeyString(this MemberInfo value)
        {
            // get neurULKeyAttribute of root type
            var erka = value.GetCustomAttributes<neurULKeyAttribute>().SingleOrDefault();
            string key;
            // if attribute exists
            if (erka != null)
                key = erka.Key;
            else if (value is PropertyInfo pi)
                key = $"{pi.ReflectedType.ToKeyString()}{Constants.TypeNamePropertyNameSeparator}{pi.Name}";
            else if (value is Type t)
                // assembly qualified name 
                key = Nullable.GetUnderlyingType(t) != null ? Nullable.GetUnderlyingType(t).FullName : t.FullName;
            else
                throw new ArgumentOutOfRangeException(nameof(value));
            return key;
        }
    }
}
