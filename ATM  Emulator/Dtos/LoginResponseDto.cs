﻿namespace ATM__Emulator.Dtos
{
    public class LoginResponseDto
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public decimal Balance { get; set; }

        public string AccessToken { get; set; }
    }
}
