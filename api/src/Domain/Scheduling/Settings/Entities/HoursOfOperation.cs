using System;
using System.Collections.Generic;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Settings {
    public class HoursOfOperation : Aggregate<HoursOfOperation>, IUserEntity {
        public Guid UserId { get; set; } = Guid.Empty;

        /// <summary>
        /// Days that the business is open.
        /// </summary>
        public List<HoursOfOperationDay> Days { get; set; } = new List<HoursOfOperationDay>();

        public static HoursOfOperation Create(Guid userId) {
            var days = new List<HoursOfOperationDay>();

            // Default to Mon - Fri 8AM to 5PM
            for (int d = 1; d <= 6; d++) {
                days.Add(
                    HoursOfOperationDay.Create(
                        d,
                        8 * 60,
                        17 * 60
                    )
                );
            }

            return new HoursOfOperation() {
                Id = Guid.NewGuid(),
                UserId = userId,
                Days = days
            };
        }
    }
}