using System;

namespace ESS.Domain.Common.Association.Domain
{
    public enum AssociationRule
    {
        None,
        //For example, when a task can be substituted for another task
        Substitution,
        //For example, when a specific project is not allowed to be part of a specific program, or a
        //specific task is “excluded” from being part of a specific phase or project
        Exclusion,
        //Where two work efforts must happen at the same time
        Concurrent,
        //Where one work effort needs to be completed (partially or fully) before another work effort can start
        Precedent,

        /// <summary>
        ///     When work efforts help each other, for example, when two projects may help each
        ///     other, such as a metadata project helping a data governance project and vice versa. In this example,
        ///     the more progress that is accomplished on the metadata project, the more progress for the data
        ///     governance project, and vice versa.
        /// </summary>
        Complementary
    }
}
