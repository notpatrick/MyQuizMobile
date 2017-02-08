using System;

namespace MyQuizMobile.Helpers {
    public class TimeHelper {
        public static DateTime UnixTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long GetUnixTimeDifferenceToNow() { return (long)DateTime.Now.Subtract(UnixTime).TotalSeconds; }

        public static DateTime ConvertFromUnixTimestamp(long timestamp) {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return origin.AddSeconds(timestamp);
        }

        public static long ConvertToUnixTimestamp(DateTime date) {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var diff = date.ToUniversalTime() - origin;
            return (long)diff.TotalSeconds;
        }
    }
}