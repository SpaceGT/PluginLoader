﻿using ProtoBuf;
using Sandbox.Graphics.GUI;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;

namespace avaness.PluginLoader.Data
{
    [ProtoContract]
    public class ModPlugin : PluginData, ISteamItem
    {
        public override string Source => "Mod";

        [XmlIgnore]
        public ulong WorkshopId { get; private set; }

        public override string Id
        {
            get
            {
                return base.Id;
            }
            set
            {
                base.Id = value;
                WorkshopId = ulong.Parse(Id);
            }
        }

        public ModPlugin()
        { }

        public override Assembly GetAssembly()
        {
            return null;
        }

        public override bool TryLoadAssembly(out Assembly a)
        {
            a = null;
            return false;
        }

        public override void Show()
        {
            MyGuiSandbox.OpenUrl("https://steamcommunity.com/workshop/filedetails/?id=" + Id, UrlOpenMode.SteamOrExternalWithConfirm);
        }

        private string modLocation;
        public string ModLocation
        {
            get
            {
                if (modLocation != null)
                    return modLocation;
                modLocation = Path.Combine(Path.GetFullPath(@"..\..\..\workshop\content\244850\"), WorkshopId.ToString());
                return modLocation;
            }
        }

        public bool Exists => Directory.Exists(ModLocation);

        public MyObjectBuilder_Checkpoint.ModItem GetModItem()
        {
            var modItem = new MyObjectBuilder_Checkpoint.ModItem(WorkshopId, "Steam");
            modItem.SetModData(new WorkshopItem(ModLocation));
            return modItem;
        }

        class WorkshopItem : VRage.GameServices.MyWorkshopItem
        {
            public WorkshopItem(string folder)
            {
                Folder = folder;
            }
        }

        public MyModContext GetModContext()
        {
            MyModContext modContext = new MyModContext();
            modContext.Init(new MyObjectBuilder_Checkpoint.ModItem(WorkshopId, "Steam"));
            modContext.Init(WorkshopId.ToString(), null, ModLocation);
            return modContext;
        }
    }
}
