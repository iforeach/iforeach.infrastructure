﻿using System;

using org.iForeach.Environment.Extensions.Features;

namespace org.iForeach.Environment.Extensions
{
    /// <summary>
    /// An implementation of this service is able to provide the <see cref="Feature"/> that
    /// any services was harvested from.
    /// </summary>
    public interface ITypeFeatureProvider
    {
        IFeatureInfo GetFeatureForDependency(Type dependency);
        void TryAdd(Type type, IFeatureInfo feature);
    }
}