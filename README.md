# Sterling_Hospital
Hospital Management System

Technical requirement:
1. Please develop API in .net core for below requirement.
2. The architecture should be a multi-tiered service based.
3. You can use the entity framework code first approach but it is not mandatory.
4. Please provide compliable and functioning code.
We have a Sterling Hospital who has doctors, nurses & Receptionists. We want to maintain 
all the appointment schedules for doctors with patients & duties of nurses on a daily basis.
Functional requirement:
1). There should be 4 categories of users in the application.
 Doctor
 Nurse
 Receptionist
 Patients
2). Following are the minimum fields for all the users.
 First name
 Last name
 Email (Optional)
 Phone number
 Date of Birth
 Sex
 Address
 Postal Code (Optional)
3). There can be only 3 doctors who are experts in brain surgery, physiotherapist & Eye 
specialist. 10 Nurses and 2 Receptionists.
4). Following are the minimum fields for Appointment details.
 PatientId
 ScheduleStartTime
 ScheduleEndTime
 PatientProblem
 Description (Optional)
 Status (Scheduled/ Cancelled/ Rescheduled)
 ConsultDoctor (brain surgery, physiotherapist & Eye specialist)
 IsOnlineConsult
5). Doctors should have the following functionality.
 He/she should be able to login using email id and password.
 He/she can check all the appointments on the dashboard & can reschedule or 
cancel as per availability.
 He/she diagnoses the Patient problem and assigns duties to nurses to admit cases.
6). Nurses should have the following functionality.
 He/she should be able to login using email id and password.
 He/she can see all duties on the dashboard with each admitted patient's respective 
time.
7). Receptionists should have the following functionality.
 He/she should be able to login using email id and password.
 He/she has rights to create a patient profile with patient id & password.
 Patient Id should have Sterling_PatientNumber and Password should have 
FirstNameBirthDate
 He/she has rights to schedule every patient appointment as per consult doctor 
inquiry & availability slot by call/hospital visit.
8). Patients should have the following functionality.
 He/she should be able to login using email id and password.
 He/she should be able to track current appointment schedules with status on 
Dashboard.
 He/she can see all previous appointments history.
