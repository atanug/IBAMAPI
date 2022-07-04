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
using Microsoft.EntityFrameworkCore;



using IBAM.API.Models;
using IBAM.API.Data;
using IBAM.API.Controllers;

namespace IBAM.API
{
    public  class Function
    {
        
        private readonly DataContext _context;
        public Function(DataContext context)
        {
            _context = context;
        }

          

       

        // [FunctionName("GetMembers")]
        // public static async Task<IActionResult> Run(
        //     [HttpTrigger(AuthorizationLevel.Function, "get", Route = "members")] HttpRequest req,
        //     ILogger log)
        // {
        //     List<MemberDisplay> MemberList = new List<MemberDisplay>();  
        //     try  
        //     {  
        //         using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("SqlConnectionString")))  
        //         {  
        //             connection.Open();  
        //             var query = @"Select * from Members a 
        //                             inner join State b on a.stateid = b.stateid 
        //                             inner join country c on b.countryId = c.countryId";  
        //             SqlCommand command = new SqlCommand(query, connection);  
        //             var reader = await command.ExecuteReaderAsync();  
        //             while (reader.Read())  
        //             {  
                        

        //                 MemberDisplay memberdisplay = new MemberDisplay()  
        //                 {  
        //                     MemberId = (int)reader["MemberId"],  
        //                     FirstName = reader["FirstName"].ToString(),   
        //                     LastName = reader["LastName"].ToString(),
        //                     StreetAddress1 = reader["StreetAddress1"].ToString(),
        //                     StreetAddress2 = reader["StreetAddress2"].ToString(),
        //                     City = reader["City"].ToString(),
        //                     StateName = reader["StateName"].ToString(),
        //                     CountryName= reader["CountryName"].ToString(),
        //                     PostalCode = reader["PostalCode"].ToString(),
        //                     EmailAddress = reader["EmailAddress"].ToString(),
        //                     PhoneNumber=reader["PhoneNumber"].ToString(),
        //                     IsActivemember=(Boolean)reader["IsActiveMember"]
        //                 };  
        //                 MemberList.Add(memberdisplay);  
        //             }  
        //         }  
        //     }  
        //     catch (Exception e)  
        //     {  
        //         log.LogError(e.ToString());  
        //     }  
        //     if(MemberList.Count > 0)  
        //     {  
        //         return new OkObjectResult(MemberList);  
        //     }  
        //     else  
        //     {  
        //         return new NotFoundResult();  
        //     }  
        // }

        // [FunctionName("CreateMember")]  
        // public static async Task<IActionResult> CreateMember(  
        //     [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "members")] HttpRequest req, ILogger log)  
        // {  
        //     string requestBody = await new StreamReader(req.Body).ReadToEndAsync();  
        //     var input = JsonConvert.DeserializeObject<CreateMember>(requestBody);  
        //     try  
        //     {  
        //         using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("SqlConnectionString")))  
        //         {  
        //             connection.Open();  
        //             if(! String.IsNullOrEmpty(input.FirstName))  
        //             {  
        //                 var query = @"INSERT INTO [Members] (FirstName,LastName,StreetAddress1, StreetAddress2,
        //                 City, StateId, CountryId, PostalCode, PhoneNumber, EmailAddress, CreatedOn, 
        //                 UpdatedOn) VALUES(@FirstName, @LastName , @StreetAddress1,@StreetAddress2,
        //                 @City,@StateId,@CountryId,@PostalCode,@PhoneNumber,@EmailAddress,getdate(),getdate())"; 

        //                 SqlCommand dbCommand = new SqlCommand(query, connection); 

        //                 dbCommand.Parameters.Add(new SqlParameter("@FirstName", input.FirstName)); 
        //                 dbCommand.Parameters.Add(new SqlParameter("@LastName", input.LastName));
        //                 dbCommand.Parameters.Add(new SqlParameter("@StreetAddress1", input.StreetAddress1));
        //                 if (input.StreetAddress2==null){
        //                     dbCommand.Parameters.Add(new SqlParameter("@StreetAddress2",""));
        //                 }
        //                 else{
        //                     dbCommand.Parameters.Add(new SqlParameter("@StreetAddress2", input.StreetAddress2));
        //                 }
                        
        //                 dbCommand.Parameters.Add(new SqlParameter("@City", input.City));
        //                 dbCommand.Parameters.Add(new SqlParameter("@StateId", input.StateId));
        //                 dbCommand.Parameters.Add(new SqlParameter("@CountryId", input.CountryId));
        //                 dbCommand.Parameters.Add(new SqlParameter("@PostalCode", ""));
        //                 dbCommand.Parameters.Add(new SqlParameter("@PhoneNumber", input.PhoneNumber)); 
        //                 dbCommand.Parameters.Add(new SqlParameter("@EmailAddress", input.EmailAddress));        

        //                 log.LogError(query);
                         
        //                 dbCommand.ExecuteNonQuery();  
        //             }  
        //         }  
        //     }  
        //     catch (Exception e)  
        //     {  
        //         log.LogError(e.ToString());  
        //         return new BadRequestResult();  
        //     }  
        //     return new OkResult();  
        // }

        
    }

     

}
