using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using IBAM.API.Models;

namespace IBAM.API
{
    public static class MemberFunctions
    {
        [FunctionName("GetMembers")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "members")] HttpRequest req,
            ILogger log)
        {
            List<MemberDisplay> MemberList = new List<MemberDisplay>();  
            try  
            {  
                using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("SqlConnectionString")))  
                {  
                    connection.Open();  
                    var query = @"Select * from Members a 
                                    inner join State b on a.stateid = b.stateid 
                                    inner join country c on b.countryId = c.countryId";  
                    SqlCommand command = new SqlCommand(query, connection);  
                    var reader = await command.ExecuteReaderAsync();  
                    while (reader.Read())  
                    {  
                        

                        MemberDisplay memberdisplay = new MemberDisplay()  
                        {  
                            MemberId = (int)reader["MemberId"],  
                            FirstName = reader["FirstName"].ToString(),   
                            LastName = reader["LastName"].ToString(),
                            StreetAddress1 = reader["StreetAddress1"].ToString(),
                            StreetAddress2 = reader["StreetAddress2"].ToString(),
                            City = reader["City"].ToString(),
                            StateName = reader["StateName"].ToString(),
                            CountryName= reader["CountryName"].ToString(),
                            PostalCode = reader["PostalCode"].ToString(),
                            EmailAddress = reader["EmailAddress"].ToString(),
                            PhoneNumber=reader["PhoneNumber"].ToString(),
                            IsActivemember=(Boolean)reader["IsActiveMember"]
                        };  
                        MemberList.Add(memberdisplay);  
                    }  
                }  
            }  
            catch (Exception e)  
            {  
                log.LogError(e.ToString());  
            }  
            if(MemberList.Count > 0)  
            {  
                return new OkObjectResult(MemberList);  
            }  
            else  
            {  
                return new NotFoundResult();  
            }  
        }
    }
}
