using System.Collections.ObjectModel;

namespace Milestone
{
    public class TermGroup : ObservableCollection<Course>
    {

        public string TermName { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }

        public TermGroup(string termName, DateTime start, DateTime end, IEnumerable<Course> courses)
            : base(courses)
        {
            TermName = termName;
            StartDate = start;
            EndDate = end;
        }
    }
}
