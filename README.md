# Sterling Hospital Management System

## Technical Requirements:

1. Develop API in .NET Core.
2. Implement a multi-tiered service-based architecture.
3. Entity Framework code-first approach is preferred but not mandatory.
4. Provide compliable and functioning code.

## Overview:

Sterling Hospital requires a comprehensive Hospital Management System to manage appointment schedules for doctors, nurse duties, and patient information efficiently.

## Functional Requirements:

1. **User Categories:**
   - Doctor
   - Nurse
   - Receptionist
   - Patient

2. **User Information:**
   - First Name
   - Last Name
   - Email (Optional)
   - Phone Number
   - Date of Birth
   - Sex
   - Address
   - Postal Code (Optional)

3. **Staff Distribution:**
   - 3 Doctors specialized in:
     - Brain Surgery
     - Physiotherapy
     - Eye Care
   - 10 Nurses
   - 2 Receptionists

4. **Appointment Details:**
   - Patient ID
   - Schedule Start Time
   - Schedule End Time
   - Patient Problem
   - Description (Optional)
   - Status (Scheduled/Cancelled/Rescheduled)
   - Consult Doctor
   - Is Online Consultation

5. **Doctor Functionality:**
   - Login using email and password
   - View all appointments on the dashboard
   - Reschedule or cancel appointments as necessary
   - Diagnose patient problems and assign nurse duties

6. **Nurse Functionality:**
   - Login using email and password
   - View duties on the dashboard with patient times

7. **Receptionist Functionality:**
   - Login using email and password
   - Create patient profiles with ID and password
   - Patient ID format: Sterling_PatientNumber
   - Password format: FirstNameBirthDate
   - Schedule patient appointments based on doctor availability

8. **Patient Functionality:**
   - Login using email and password
   - Track current appointment schedules and statuses on the dashboard
   - View previous appointment history

For further development and implementation details, refer to the technical requirements and functional specifications outlined above.
