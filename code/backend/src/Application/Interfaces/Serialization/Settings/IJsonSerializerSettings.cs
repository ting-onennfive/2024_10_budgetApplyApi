﻿using Newtonsoft.Json;

namespace budgetApplyApi.Application.Interfaces.Serialization.Settings
{
    public interface IJsonSerializerSettings
    {
        /// <summary>
        /// Settings for <see cref="Newtonsoft.Json"/>.
        /// </summary>
        public JsonSerializerSettings JsonSerializerSettings { get; }
    }
}