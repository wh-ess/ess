#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper;
using ESS.Domain.Foundation.EntityConfig.Events;
using ESS.Framework.Common.Utilities;
using ESS.Framework.CQRS;
using ESS.Framework.CQRS.Event;
using ESS.Framework.Data;

#endregion

namespace ESS.Domain.Foundation.EntityConfig.ReadModels
{
    public class FieldView : ISubscribeTo<FieldsSaved>
    {
        public readonly string Path = Framework.Common.Utilities.Path.ServerPath() + "/Fields";
        private readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof(List<FieldItem>));
        private readonly IRepository<FieldItem, Guid> _repository;

        public FieldView(IRepository<FieldItem, Guid> repository)
        {
            _repository = repository;
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
        }

        /// <summary>
        /// 单个模块,动作
        /// </summary>
        /// <param name="moduleNo"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public IEnumerable<FieldItem> FieldList(string moduleNo, string actionName)
        {
            var filePath = Path + "/" + moduleNo + "-" + actionName + ".xml";

            if (!File.Exists(filePath))
            {
                return new List<FieldItem>();
            }
            var reader = XmlReader.Create(filePath);
            var list = (List<FieldItem>)_xmlSerializer.Deserialize(reader);
            reader.Close();
            return list.OrderBy(c => c.Index);
        }

        /// <summary>
        /// 用于选择
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FieldItem> FieldList()
        {
            DirectoryInfo folder = new DirectoryInfo(Path);
            var list = new List<FieldItem>();
            //遍历文件
            foreach (FileInfo file in folder.GetFiles())
            {
                if (File.Exists(file.FullName))
                {
                    var reader = XmlReader.Create(file.FullName);
                    list.AddRange((List<FieldItem>)_xmlSerializer.Deserialize(reader));
                    reader.Close();
                }
            }
            return list.Distinct().OrderBy(c => c.Index);
        }

        #region handle

        public void Handle(FieldsSaved e)
        {
            var filePath = Path + "/" + e.ModuleNo + "-" + e.ActionName + ".xml";
            var writer = XmlWriter.Create(filePath);
            _xmlSerializer.Serialize(writer, e.Fields);
            writer.Close();
        }

        #endregion
    }

    [Serializable]
    public class FieldItem
    {
        public Guid Id;
        public string Name;
        public string Text;
        public FieldType Type;
        public FieldShowType ShowIn;
        public FieldFeatureType Feature;
        public int Index;
        public int ListWidth;
        public string SourceIdField;
        public string SourceName;
        public string SourceParentIdField;
        public string SourceTextField;
        public string Align;
        public string Group;
        public string HelpText;
        public string Condition;
        public string Format;
    }

    public enum FieldType
    {
        Text,
        Date,
        Select,
        SelectMulti,
        DateTime,
        Time,
        Month,
        Checkbox,
        CheckList,
        Radio,
        Number,
        Currency,
        Digits,
        Hidden,
        Label,
        Password,
        Icon,
        Tree,
        SelectPage
    }

    [Flags]
    public enum FieldShowType
    {
        InForm = 1,
        InList = 2,
        InSearch = 4,
    }

    [Flags]
    public enum FieldFeatureType
    {
        Required = 1,
        IsForeignKey = 2,
        IsPrimaryKey = 4,
        IsTreeColumn = 8,
        IsAutoKey = 16,
    }
}