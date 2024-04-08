﻿using DataAccess_Layer.Models;

namespace Service_Layer.Interface
{
    public interface IPatientService
    {
        Task<List<AppointmentDetails>> GetCurrentAppointments(int patientId);
        Task<List<AppointmentDetails>> GetPreviousAppointments(int patientId);
    }
}