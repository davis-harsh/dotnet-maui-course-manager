namespace Milestone
{
    public static class AppSettings
    {
        private const string CourseNotificationsKey = "course_notifications_enabled";
        private const string AssessmentNotificationsKey = "assessment_notifications_enabled";

        public static bool CourseNotificationsEnabled
        {
            get => Preferences.Get(CourseNotificationsKey, true);
            set => Preferences.Set(CourseNotificationsKey, value);
        }

        public static bool AssessmentNotificationsEnabled
        {
            get => Preferences.Get(AssessmentNotificationsKey, true);
            set => Preferences.Set(AssessmentNotificationsKey, value);
        }
    }

}
