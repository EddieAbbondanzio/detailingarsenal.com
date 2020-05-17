using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain {
    public class HoursOfOperation : Entity<HoursOfOperation>, IUserEntity {
        public Guid UserId { get; set; } = Guid.Empty;

        /// <summary>
        /// Days that the business is open.
        /// </summary>
        public List<HoursOfOperationDay> Days { get; set; } = new List<HoursOfOperationDay>();
    }
}