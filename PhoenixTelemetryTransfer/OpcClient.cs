// <copyright file="OpcClient.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PhoenixTelemetryTransfer
{
    using System;
    using System.Collections.Generic;
    using Opc;
    using Opc.Da;

    public sealed class OpcClient : IOpcClient, IDisposable
    {
        private readonly string opcUrl;
        private Subscription groupWrite;
        private SubscriptionState opcGroup;
        private Opc.Da.Server server;
        private OpcCom.Factory fact = new OpcCom.Factory();
        private List<Item> itemsList = new List<Item>();
        private List<ItemValue> itemValuesList = new List<ItemValue>();
        private bool disposed;

        public OpcClient(string opcUrl)
        {
            this.opcUrl = opcUrl;
        }

        public string OpcUrl { get => this.opcUrl; }

        public void Connect()
        {
            this.server?.Dispose();
            this.server = new Opc.Da.Server(this.fact, null);
            this.server.Url = new URL(this.opcUrl);
            this.server.Connect();
        }

        public void CreateOPC_Group()
        {
            this.opcGroup = new SubscriptionState();
            this.opcGroup.Name = "WriteGroup";
            this.opcGroup.Active = false;
            this.groupWrite?.Dispose();
            this.groupWrite = (Opc.Da.Subscription)this.server.CreateSubscription(this.opcGroup);
        }

        public void AddTagValue(string name, double value)
        {
            Item itemname = new Item();
            itemname.ItemName = name;

            this.itemsList.Add(itemname);
            ItemValue itemValue = new ItemValue(itemname);
            itemValue.Value = value;
            this.itemValuesList.Add(itemValue);
        }

        public void AddTimeStamp(string name, DateTime time)
        {
            Item itemname = new Item();
            itemname.ItemName = name;

            this.itemsList.Add(itemname);
            ItemValue itemValue = new ItemValue(itemname);
            itemValue.Value = time;
            this.itemValuesList.Add(itemValue);
        }

        public void WriteGroupData()
        {
            if (this.itemsList.Count != null && this.itemsList.Count != 0)
            {
                this.groupWrite.AddItems(this.itemsList.ToArray());


                for (int i = 0; i < this.itemValuesList.Count; i++)
                {
                    this.itemValuesList[i].ServerHandle = this.groupWrite.Items[i].ServerHandle;
                }
                this.groupWrite.Write(this.itemValuesList.ToArray());
                this.groupWrite.RemoveItems(this.groupWrite.Items);
                this.itemsList.Clear();
                this.itemValuesList.Clear();
            }
    }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
            this.groupWrite?.Dispose();
            this.server?.Dispose();
            this.fact.Dispose();
        }

        private void ThrowIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }
    }
}
