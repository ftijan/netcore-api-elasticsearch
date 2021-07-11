using Example.Elasticsearch.Api.Models;
using Nest;
using System;

namespace Example.Elasticsearch.Api.Extensions
{
    public static class ModelConfig
    {       
        public static ConnectionSettings WithModelConfig(this ConnectionSettings connectionSettings)
        {
            if (connectionSettings is null)
            {
                throw new ArgumentNullException(nameof(connectionSettings));
            }

            // Employee has a property with a name Id and Nest will attempt to
            // map it to internal _id field - disable it and allow Elasticsearch to
            // generate its own _id value
            return connectionSettings.DefaultMappingFor<Employee>(x => x.DisableIdInference());
        }
    }
}
