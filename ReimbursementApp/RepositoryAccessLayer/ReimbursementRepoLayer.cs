using Models;
using System.Data.SqlClient;
//using System.Runtime.InteropServices;

namespace RepositoryAccessLayer
{
    public class ReimbursementRepoLayer
    {
        public async Task<List<ReimbursementReq>> RequestsAsync(int type)

        {
            SqlConnection conn = new SqlConnection("Server=tcp:dbpro.database.windows.net,1433;Initial Catalog=Project1;Persist Security Info=False;User ID=mominur;Password=A123456789a;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            using (SqlCommand command = new SqlCommand($"SELECT * FROM ReimbursementApp WHERE Status = @type", conn))
            {

                command.Parameters.AddWithValue("@type", type);
                conn.Open();
                SqlDataReader? ret = await command.ExecuteReaderAsync();
                List<ReimbursementReq> rList = new List<ReimbursementReq>();
                while (ret.Read())
                {
                    ReimbursementReq r = new ReimbursementReq((Guid)ret[0], (Guid)ret[1], ret.GetString(2), ret.GetDecimal(3), ret.GetInt32(4));
                    rList.Add(r);
                }
                conn.Close();
                return rList;

            }
        }

     

        public async Task<ReimbursementApplication> AddEmployeeApplicationAsync(ReimbursementApplication application)
        {
           // throw new NotImplementedException();

             SqlConnection conn = new SqlConnection("Server=tcp:dbpro.database.windows.net,1433;Initial Catalog=Project1;Persist Security Info=False;User ID=mominur;Password=A123456789a;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            using (SqlCommand command = new SqlCommand($"INSERT INTO Employees (EmployeeID,FirstName,LastName,IsManager,Email, Password) VALUES(@id, @f,@l,@i,@e,@p);", conn))

            {
                command.Parameters.AddWithValue("@id", application.EmployeeID);
                command.Parameters.AddWithValue("@f", application.FirstName);
                 command.Parameters.AddWithValue("@l", application.LastName);
                command.Parameters.AddWithValue("@i", application.IsManager);
                 command.Parameters.AddWithValue("@e", application.Email);
                command.Parameters.AddWithValue("@p", application.Password);

                conn.Open();
                int ret = await command.ExecuteNonQueryAsync();
                if (ret == 1)
                {
                    //conn.Close();
                    return application;
                }

                conn.Close();
                return null;
            }
        }

   

        public async Task<UpdateReRequestDto> UpdateRequestAsync(Guid reimbursementId, int status)
        {
            SqlConnection conn = new SqlConnection("Server=tcp:dbpro.database.windows.net,1433;Initial Catalog=Project1;Persist Security Info=False;User ID=mominur;Password=A123456789a;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            using (SqlCommand command = new SqlCommand($"UPDATE ReimbursementApp SET Status = @status WHERE ReimbursementID = @id", conn))
            {
                command.Parameters.AddWithValue("@id", reimbursementId);
                command.Parameters.AddWithValue("@status", status);
                conn.Open();
                int ret = await command.ExecuteNonQueryAsync();
                if (ret == 1)
                {
                    conn.Close();
                    //call the requestByid()
                    // created 2 neew methods to get the request by 
                    // ID And get the employee by id. Thes are methods that would be useful and reusable
                    // Mark does not want to do 80% for the project

                    //call the updaterequesttByid() . This method will join to return the employee name 
                    // along with the relavent detail and return DTO


                    UpdateReRequestDto urbi = await this.UpdateRequestByIdAsync(reimbursementId);
                   // UpdateReRequestDto urbi = await this.UpdateRequestAsync(reimbursementId, status);

                    // return.Close();
                    return urbi;

                }

                conn.Close();
                return null;
            }

        }

        

        public async Task<bool> IsManagerAsync(Guid employeeID)
        {
            SqlConnection conn = new SqlConnection("Server=tcp:dbpro.database.windows.net,1433;Initial Catalog=Project1;Persist Security Info=False;User ID=mominur;Password=A123456789a;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            using (SqlCommand command = new SqlCommand($"SELECT IsManager FROM Employees WHERE EmployeeID = @id", conn))
            {

                command.Parameters.AddWithValue("@id", employeeID);
                conn.Open();
                SqlDataReader? ret = await command.ExecuteReaderAsync();
                if (ret.Read() && ret.GetBoolean(0))

                {
                    conn.Close();
                    return true;
                }
                conn.Close();
                return false;
            }
        }


        public async Task<UpdateReRequestDto> UpdateRequestByIdAsync(Guid reimbursementId)
        {
            SqlConnection conn = new SqlConnection("Server=tcp:dbpro.database.windows.net,1433;Initial Catalog=Project1;Persist Security Info=False;User ID=mominur;Password=A123456789a;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            using (SqlCommand command = new SqlCommand($"SELECT ReimbursementID,FirstName, LastName, Status FROM Employees LEFT JOIN  ReimbursementApp ON EmployeeID = FK_EmployeeId WHERE ReimbursementID = @reimbursementId", conn))
            {

                command.Parameters.AddWithValue("@reimbursementId", reimbursementId);

                conn.Open();
                SqlDataReader? ret = await command.ExecuteReaderAsync();

                if (ret.Read())
                {
                    UpdateReRequestDto r = new UpdateReRequestDto(ret.GetGuid(0), ret.GetString(1), ret.GetString(2), ret.GetInt32(3));
                    conn.Close();

                    return r;
                }
            }

            conn.Close();
            return null;
        }

        public async Task<Ticket> TicketAsync(Ticket ticket)
        {
           // throw new NotImplementedException();

             SqlConnection conn = new SqlConnection("Server=tcp:dbpro.database.windows.net,1433;Initial Catalog=Project1;Persist Security Info=False;User ID=mominur;Password=A123456789a;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            using (SqlCommand command = new SqlCommand($"INSERT INTO ReimbursementApp (ReimbursementID,FK_EmployeeId,Details,Amount) VALUES(@id,@fk,@d,@a);", conn))

            {
                command.Parameters.AddWithValue("@id", ticket.ReimbursementID);
                command.Parameters.AddWithValue("@fk", ticket.FK_EmployeeId);
                 command.Parameters.AddWithValue("@d", ticket.Details);
                command.Parameters.AddWithValue("@a", ticket.Amount);
    

                conn.Open();
                int ret = await command.ExecuteNonQueryAsync();
                if (ret == 1)
                {
                    //conn.Close();
                     
                    return ticket;
                }

                conn.Close();
                return null;
            }

        }

 

        public async Task<List<AllTicket>> AllTicketAsync(int type)

        {
            SqlConnection conn = new SqlConnection("Server=tcp:dbpro.database.windows.net,1433;Initial Catalog=Project1;Persist Security Info=False;User ID=mominur;Password=A123456789a;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            using (SqlCommand command = new SqlCommand($"SELECT * FROM ReimbursementApp", conn))
            {

                //command.Parameters.AddWithValue("@type", type);
                conn.Open();
                SqlDataReader? ret = await command.ExecuteReaderAsync();
                List<AllTicket> rList = new List<AllTicket>();
                while (ret.Read())
                {
                    AllTicket r = new AllTicket((Guid)ret[0], (Guid)ret[1], ret.GetString(2), ret.GetDecimal(3), ret.GetInt32(4));
                    rList.Add(r);
                }
                conn.Close();
                return rList;

            }
        }




   
        public async Task<Login> LoginAsync(Login login)
        {
        
        
            SqlConnection conn = new SqlConnection("Server=tcp:dbpro.database.windows.net,1433;Initial Catalog=Project1;Persist Security Info=False;User ID=mominur;Password=A123456789a;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            using (SqlCommand command = new SqlCommand($"SELECT Email, Password FROM Employees WHERE Email = @email AND Password = @password", conn))
            {

                command.Parameters.AddWithValue("@email", login.Email);
                command.Parameters.AddWithValue("@password", login.Password);
                conn.Open();
                SqlDataReader? ret = await command.ExecuteReaderAsync();

                Login? log = null;
                if(ret.Read()){
                    log = new Login(ret.GetString(0), ret.GetString(1));
                    return log;
                }
                conn.Close();
                return null; 
                
                
            }
            
        }



    }

} 

 