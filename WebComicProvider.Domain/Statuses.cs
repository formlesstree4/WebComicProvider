using System.ComponentModel;

namespace WebComicProvider.Domain
{
    public enum Statuses
    {
        [Description("Active")]
        ComicActive = 1,

        [Description("Inactive")]
        ComicInactive = 2,

        [Description("Completed")]
        ComicCompleted = 3,


        [Description("Active")]
        IssueActive = 4,

        [Description("Inactive")]
        IssueInactive = 5,

        [Description("Completed")]
        IssueCompleted = 6,


        [Description("Active")]
        PageActive = 7,

        [Description("Inactive")]
        PageInactive = 8,

        [Description("Completed")]
        PageCompleted = 9
    }
}
