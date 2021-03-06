﻿using ESS.Framework.CQRS.Event;

namespace ESS.Domain.Common.Category.Events
{
    public class CategoryTypeSchemeCreated : Event
    {
        public string Name;
        public string Code;
        public bool IsSystem;

    }

    public class CategoryTypeSchemeNameChanged : Event
    {
        public string Name;
    }
    public class CategoryTypeSchemeDeleted : Event
    {
    }
}
