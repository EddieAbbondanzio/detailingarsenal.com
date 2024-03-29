using System;
using System.Collections.Generic;
using System.Linq;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Calendar {
    public class Appointment : Aggregate<Appointment>, IUserEntity {
        public const int NotesMaxLength = 1024;

        public Guid UserId { get; set; }

        /// <summary>
        /// Id of the service being performed.
        /// </summary>
        public Guid ServiceId { get; set; }

        public Guid ClientId { get; set; }

        /// <summary>
        /// Price of the appointment in USD.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// How long the appointment will take in minutes.
        /// </summary>
        public int Duration => Blocks.Select(b => b.Duration).Sum();

        /// <summary>
        /// Misc. notes.
        /// </summary>
        public string? Notes {
            get => notes;
            set {
                if (notes?.Length > NotesMaxLength) {
                    throw new ArgumentOutOfRangeException();
                }

                notes = value;
            }
        }

        public List<AppointmentBlock> Blocks { get; set; } = new List<AppointmentBlock>();

        private string? notes;

        public static Appointment Create(Guid userId, Guid serviceId, Guid clientId, decimal price, string? notes) {
            return new Appointment() {
                Id = Guid.NewGuid(),
                UserId = userId,
                ServiceId = serviceId,
                ClientId = clientId,
                Price = price
            };
        }
    }
}