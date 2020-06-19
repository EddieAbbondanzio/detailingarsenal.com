using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.Settings {
    public class HoursOfOperation : Aggregate<HoursOfOperation>, IUserEntity {
        public Guid UserId { get; set; } = Guid.Empty;

        /// <summary>
        /// Days that the business is open.
        /// </summary>
        public List<HoursOfOperationDay> Days { get; set; } = new List<HoursOfOperationDay>();

        public static HoursOfOperation Create(Guid userId) {
            return new HoursOfOperation() {
                Id = Guid.NewGuid(),
                UserId = userId
            };
        }
    }
}