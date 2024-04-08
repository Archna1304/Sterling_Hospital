﻿using DataAccess_Layer.Models;

namespace DataAccess_Layer.Interface
{
    public interface IPatientRepo
    {
        Task<List<AppointmentDetails>> GetCurrentAppointments(int patientId);
        Task<List<AppointmentDetails>> GetPreviousAppointments(int patientId);
    }
}