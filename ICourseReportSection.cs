using System.Collections.ObjectModel;

namespace Milestone
{
    public interface ICourseReportSection
    {
        Task LoadAsync(LocalDbService db);
    }

    public class ActiveCourseReport : ICourseReportSection
    {
        private readonly ObservableCollection<Course> _target;

        public ActiveCourseReport(ObservableCollection<Course> target)
        {
            _target = target;
        }

        public async Task LoadAsync(LocalDbService db)
        {
            _target.Clear();
            var data = await db.GetActiveCourses();
            foreach (var course in data)
                _target.Add(course);
        }
    }

    public class CompletedCourseReport : ICourseReportSection
    {
        private readonly ObservableCollection<Course> _target;

        public CompletedCourseReport(ObservableCollection<Course> target)
        {
            _target = target;
        }

        public async Task LoadAsync(LocalDbService db)
        {
            _target.Clear();
            var data = await db.GetCompletedCourses();
            foreach (var course in data)
                _target.Add(course);
        }
    }
}
