// <copyright file="OpcClient.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;

namespace PhoenixTelemetryTransfer
{
    public interface IOpcClient
    {
        string OpcUrl { get; }

        void AddTimeStamp(string name, DateTime time);
        void AddTagValue(string name, double value);
        void Connect();
        void CreateOPC_Group();
        void Dispose();
        void WriteGroupData();
    }
}