using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESS.Domain.JXC
{
    //poco 类 做文件夹内文件选取特殊处理用的，没任何意义，只不过是前台JSON解析必须这个结构
    public class DropdownList
    {
        public string id { get; set; }
        public string text { get; set; }
    }
}
