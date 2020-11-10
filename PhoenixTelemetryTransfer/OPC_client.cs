using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opc;
using Opc.Da;

namespace PhoenixTelemetryTransfer
{
    public class OPC_client
    {
        private Subscription groupWrite;
        private SubscriptionState OPC_Group;
        private Opc.Da.Server server;
        private string opcUrl;
        private OpcCom.Factory fact = new OpcCom.Factory();
        private List<Item> itemsList = new List<Item>();

        public OPC_client()
        {

        }

        public void Connect()
        {
            this.server = new Opc.Da.Server(this.fact, null);
            this.server.Url = new Opc.URL(opcUrl); 
            this.server.Connect();
        }

        public void CreateOPC_Group()
        {
            this.OPC_Group = new Opc.Da.SubscriptionState();
            this.OPC_Group.Name = "WriteGroup";
            this.OPC_Group.Active = false;
            this.groupWrite = (Opc.Da.Subscription)this.server.CreateSubscription(this.OPC_Group);
        }
        public void WriteData(string itemName, double value)
        {
            groupWrite.RemoveItems(groupWrite.Items);
            List<Item> writeList = new List<Item>();
            List<ItemValue> valueList = new List<ItemValue>();

            Item itemToWrite = new Item();
            itemToWrite.ItemName = itemName;
            ItemValue itemValue = new ItemValue(itemToWrite);
            itemValue.Value = value;

            writeList.Add(itemToWrite);
            valueList.Add(itemValue);

            groupWrite.AddItems(writeList.ToArray());

            for (int i = 0; i < valueList.Count; i++)
                valueList[i].ServerHandle = groupWrite.Items[i].ServerHandle;

            groupWrite.Write(valueList.ToArray());
        }

        public string OpcUrl { get => this.opcUrl; set => this.opcUrl = value; }

        // public string TimeStamp { get => this.timeStamp; set => this.timeStamp = value; }
    }
}
