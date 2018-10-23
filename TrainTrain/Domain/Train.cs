﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace TrainTrain.Domain
{
    public class Train
    {
        private readonly string _id;
        private readonly IList<Seat> _seats;
        private readonly List<Coach> _coaches;
        private const decimal CapacityRatioThreshold = 0.7M;


        public Train(string id, IList<Seat> seats)
        {
            _id = id;
            _seats = seats;
            _coaches = seats.GroupBy(seat => seat.CoachName).Select(coach => new Coach(coach.Key, coach)).ToList();

        }

        public DomainEvent TryToReserve(int seatsRequestedCount, string bookingReference)
        {
            if (!CanReserve(seatsRequestedCount))
            {
                return new SeatsReservedFailedBecauseNotEnoughAvailableSeats(_id);
            }

            var reservation = FindAvailableSeats(seatsRequestedCount);
            if (reservation.Success())
            {
                return new SeatsReserved(_id, reservation.Seats(), bookingReference);
            }
            return new SeatsReservedFailedBecauseNotEnoughAvailableSeats(_id);
        }

        private bool CanReserve(int seatsRequestedCount)
        {
            return ReservedSeats + seatsRequestedCount <= GetAvailableSeatsForReservation();
        }

        private Reservation FindAvailableSeats(int seatsRequestedCount)
        {
            return _coaches
                .Select(coach => coach.TryReserve(seatsRequestedCount))
                .FirstOrDefault(reservation => reservation.Success()) ?? new FailReservation();
        }

        private int GetNbSeats()
        {
            return _seats.Count;
        }

        private int ReservedSeats
        {
            get { return _seats.Count(seat => !seat.IsNotReserved()); }
        }

        private int GetAvailableSeatsForReservation()
        {
            return (int)Math.Floor(CapacityRatioThreshold * GetNbSeats());
        }
    }
}