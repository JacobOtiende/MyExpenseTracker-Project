using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Transactions;

namespace MyExpenseTracker.Pages.Transactions
{
    public class EditModel : PageModel
    {
        public TransactionInfo transactionInfo = new TransactionInfo();
        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                String connectionString = "Data Source=CONSOLE-USER;Initial Catalog=MyTracker;Integrated Security=True;trustservercertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM tracker WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {                                
                                transactionInfo.id = "" + reader.GetInt32(0);
                                transactionInfo.emoji = reader.GetString(2);
                                transactionInfo.description = reader.GetString(3);
                                transactionInfo.amount = "" + reader.GetDecimal(4);
                                transactionInfo.category = reader.GetString(5);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost() 
        {
            if (!string.IsNullOrEmpty(Request.Form["id"]) ||
                !string.IsNullOrEmpty(Request.Form["emoji"]) ||
                !string.IsNullOrEmpty(Request.Form["description"]) ||
                !string.IsNullOrEmpty(Request.Form["amount"]) ||
                !string.IsNullOrEmpty(Request.Form["category"]))
            {
                transactionInfo.id = Request.Form["id"];
                transactionInfo.emoji = Request.Form["emoji"];
                transactionInfo.description = Request.Form["description"];
                transactionInfo.amount = Request.Form["amount"];
                transactionInfo.category = Request.Form["category"];
            }
            else
            {
                errorMessage = "All fields are required";
                return;
            }

            try
            {
                String connectionString = "Data Source=CONSOLE-USER;Initial Catalog=MyTracker;Integrated Security=True;trustservercertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE tracker " +
              "SET emoji = @emoji, description = @description, amount = @amount, category = @category " +
              "WHERE id = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@emoji", transactionInfo.emoji);
                        command.Parameters.AddWithValue("@description", transactionInfo.description);
                        command.Parameters.AddWithValue("@amount", transactionInfo.amount);
                        command.Parameters.AddWithValue("@category", transactionInfo.category);
                        command.Parameters.AddWithValue("@id", transactionInfo.id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Transactions/Index");
        }
    }
}
