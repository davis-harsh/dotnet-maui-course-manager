using Plugin.LocalNotification;

namespace Milestone
{
    public static class Notifications
    {
        public static void AssessmentNotifications(Assessment assessment)
        {
            if (!AppSettings.AssessmentNotificationsEnabled)
                return;

            if (assessment == null || assessment.AssessmentId <= 0)
                return;

            // Instant notification
            var startDateScheduled = new NotificationRequest
            {
                NotificationId = assessment.AssessmentId * 100,
                Title = "Assessment Scheduled",
                Description = $"Assessment \"{assessment.Name}\" has been scheduled for {assessment.StartDate:MMM dd}.",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(5),
                    NotifyRepeatInterval = TimeSpan.Zero
                }
            };

            // Day-of notification
            var startDateNotification = new NotificationRequest
            {
                NotificationId = assessment.AssessmentId * 100 + 1,
                Title = "Assessment Begins Now",
                Description = $"Assessment \"{assessment.Name}\" is today! Good luck!",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = assessment.StartDate.Date.AddHours(0).AddSeconds(5),
                    NotifyRepeatInterval = TimeSpan.Zero
                }
            };
            // Instant notification
            var endDateScheduled = new NotificationRequest
            {
                NotificationId = assessment.AssessmentId * 100 + 2,
                Title = "Assessment Due Date",
                Description = $"Assessment \"{assessment.Name}\" is due by {assessment.EndDate:MMM dd}.",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(5),
                    NotifyRepeatInterval = TimeSpan.Zero
                }
            };

            // Day-of notification
            var endDateNotification = new NotificationRequest
            {
                NotificationId = assessment.AssessmentId * 100 + 3,
                Title = "Assessment Due Today",
                Description = $"Assessment \"{assessment.Name}\" is due today!",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = assessment.EndDate.Date.AddHours(0).AddSeconds(5),
                    NotifyRepeatInterval = TimeSpan.Zero
                }
            };

            LocalNotificationCenter.Current.Show(startDateScheduled);
            LocalNotificationCenter.Current.Show(startDateNotification);
            LocalNotificationCenter.Current.Show(endDateScheduled);
            LocalNotificationCenter.Current.Show(endDateNotification);
        }


        public static void CourseNotifications(Course course)
        {
            if (!AppSettings.CourseNotificationsEnabled)
                return;

            if (course == null || course.CourseId <= 0)
                return;

            // Instant notification
            var startDateScheduled = new NotificationRequest
            {
                NotificationId = course.CourseId * 1000,
                Title = "Start Date Set",
                Description = $"\"{course.Name}\" will begin on {course.StartDate:MMM dd}.",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(5),
                    NotifyRepeatInterval = TimeSpan.Zero
                }
            };

            // Day-of notification
            var courseStartNotification = new NotificationRequest
            {
                NotificationId = course.CourseId * 1000 + 1,
                Title = "Course Starts Today",
                Description = $"\"{course.Name}\" starts now!",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = course.StartDate.Date.AddHours(0).AddSeconds(5),
                    NotifyRepeatInterval = TimeSpan.Zero
                }
            };

            // Instant notification
            var endDateScheduled = new NotificationRequest
            {
                NotificationId = course.CourseId * 1000 + 2,
                Title = "End Date Set",
                Description = $"\"{course.Name}\" ends on {course.EndDate:MMM dd}.",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(5),
                    NotifyRepeatInterval = TimeSpan.Zero
                }
            };

            // Day-of notification
            var courseEndNotification = new NotificationRequest
            {
                NotificationId = course.CourseId * 1000 + 3,
                Title = "Course Ends Today",
                Description = $"\"{course.Name}\" wraps up today. Great job!",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = course.EndDate.Date.AddHours(0).AddSeconds(5),
                    NotifyRepeatInterval = TimeSpan.Zero
                }
            };

            LocalNotificationCenter.Current.Show(startDateScheduled);
            LocalNotificationCenter.Current.Show(courseStartNotification);
            LocalNotificationCenter.Current.Show(endDateScheduled);
            LocalNotificationCenter.Current.Show(courseEndNotification);
        }
    }

}
