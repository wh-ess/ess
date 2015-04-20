#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using ESS.Domain.Foundation.EntityConfig.Events;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Domain.Foundation.EntityConfig.ReadModels
{
    public class ModuleView : ISubscribeTo<ModulesSaved>, ISubscribeTo<FieldsSaved>, ISubscribeTo<ActionSaved>
    {
        public readonly string Path = Framework.Common.Utilities.Path.ServerPath() + "/Module.xml";
        private readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof(List<ModuleItem>));

        public List<ModuleItem> ModuleConfig = new List<ModuleItem>();

        public ModuleView()
        {
            if (File.Exists(Path))
            {
                var reader = XmlReader.Create(Path);
                ModuleConfig = (List<ModuleItem>)_xmlSerializer.Deserialize(reader);
                reader.Close();
            }
        }

        public void Handle(ActionSaved e)
        {
            var item = ModuleConfig.FirstOrDefault(c => c.ModuleNo == e.ModuleNo);
            if (item == null)
            {
                throw new Exception();
            }
            item.Actions = e.Actions;
            SaveModule(ModuleConfig);
        }

        public void Handle(FieldsSaved e)
        {
            var item = ModuleConfig.FirstOrDefault(c => c.ModuleNo == e.ModuleNo);
            if (item == null)
            {
                throw new Exception("module not found");
            }
            var action = item.Actions.FirstOrDefault(c => c.Name == e.ActionName);
            if (action == null)
            {
                throw new Exception("action not found");
            }
            action.Fields = e.Fields.Select(c => c.Id)
                .ToList();
            SaveModule(ModuleConfig);
        }

        public void Handle(ModulesSaved e)
        {
            SaveModule(e.Modules);
        }

        private void SaveModule(List<ModuleItem> modules)
        {
            var writer = XmlWriter.Create(Path);
            _xmlSerializer.Serialize(writer, modules);
            writer.Close();
        }
    }


    [Serializable]
    public class ModuleItem
    {
        public string ModuleNo { get; set; }
        public string ParentModuleNo { get; set; }
        public List<ActionItem> Actions { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public int Order { get; set; }
        public bool IsMenu { get; set; }
    }

    [Serializable]
    public class ActionItem
    {
        public string Name { get; set; }
        public bool IsBatch { get; set; }
        public ActionType Type { get; set; }
        public string Text { get; set; }
        public string Action { get; set; }
        public List<Guid> Fields { get; set; }
        public string Icon { get; set; }
        public int Order { get; set; }
    }

    public enum ActionType
    {
        Button,
        Link,
        Query,
    }
}