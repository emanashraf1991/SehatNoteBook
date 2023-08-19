using System;
using System.Collections.Generic;
 

namespace SehatNotebook.Configuration.Messages
{
    public static class ErrorMessages 
    {
        public static class Generic 
        {
            public static string SomethingWentWrong ="Something went wrong, please try again later";
            public static string UnableToProcess ="Unable to process";
            public static string TypeBadRequest ="Bad Request";
            public static string InvalidPayload = "Invalid Payload";
            public static string InvalidRequest = "Invalid Request";
        }
        public static class Profile 
        {
            public static string UserNotFound = "User not found!"; 
            public static string ProfileNotFound = "Profile not found!"; 
        }
    }
}