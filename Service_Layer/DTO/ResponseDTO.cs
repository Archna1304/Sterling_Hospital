﻿namespace Service_Layer.DTO
{
    public class ResponseDTO
    {
        public int Status { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
    }
}
