using AutoMapper;
using DataAccess_Layer.Models;
using Service_Layer.DTO;

namespace Service_Layer.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDTO, User>().ForMember(dest => dest.Role, opt => opt.MapFrom(src => MapRole(src.Role))).ReverseMap();
            CreateMap<RegisterDTO, User>().ForMember(dest => dest.Sex, opt => opt.MapFrom(src => MapSex(src.Sex))).ReverseMap();

            CreateMap<RegisterPatientDTO, User>().ForMember(dest => dest.Role, opt => opt.MapFrom(src => MapRole(src.Role))).ReverseMap();
            CreateMap<RegisterPatientDTO, User>().ForMember(dest => dest.Sex, opt => opt.MapFrom(src => MapSex(src.Sex))).ReverseMap();

            // Mapping for AppointmentDTO to AppointmentDetails
            CreateMap<AppointmentDTO, AppointmentDetails>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => MapStatus(src.Status.ToString())))
                .ReverseMap();
        }

        private Role MapRole(string role)
        {
            switch (role.ToLower())
            {
                case "nurse":
                    return Role.Nurse;
                case "doctor":
                    return Role.Doctor;
                case "receptionist":
                    return Role.Receptionist;
                case "patient":
                    return Role.Patient;
                default:
                    throw new ArgumentException($"Invalid role: {role}");
            }
        }

        private Sex MapSex(string sex)
        {
            switch (sex.ToLower())
            {
                case "male":
                    return Sex.Male;
                case "female":
                    return Sex.Female;
                case "other":
                    return Sex.Other;
                default:
                    throw new ArgumentException($"Invalid sex: {sex}");
            }
        }

        private Status MapStatus(string status)
        {
            switch (status.ToLower())
            {
                case "scheduled":
                    return Status.Scheduled;
                case "cancelled":
                    return Status.Cancelled;
                case "rescheduled":
                    return Status.Rescheduled;
                default:
                    throw new ArgumentException($"Invalid sex: {status}");
            }
        }
    }
}
