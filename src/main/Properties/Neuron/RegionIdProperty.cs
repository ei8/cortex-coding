﻿using System;

namespace ei8.Cortex.Coding.Properties.Neuron
{
    public class RegionIdProperty : INeuronProperty<Guid?>
    {
        public RegionIdProperty(Guid? value, string name)
        {
            this.Value = value;
            this.Name = name;
        }

        public Guid? Value { get; private set; }

        public string Name { get; }

        public object GetValue() => this.Value;
    }
}