using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyExpenseTracker.Pages.Transactions
{
    public class CreateModel : PageModel
    {
        public TransactionInfo transactionInfo = new TransactionInfo();
        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {
        }

        public void OnPost() 
        {
            if(!string.IsNullOrEmpty(Request.Form["emoji"]) ||
               !string.IsNullOrEmpty(Request.Form["description"]) ||
               !string.IsNullOrEmpty(Request.Form["amount"]) ||
               !string.IsNullOrEmpty(Request.Form["category"]))
            {
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

            //saving new trnsactiion to the database
            try
            {
                String connectionString = "Data Source=CONSOLE-USER;Initial Catalog=MyTracker;Integrated Security=True;trustservercertificate=True";
                using (SqlConnection  connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO tracker" +
                                "(emoji, description, amount, category) VALUES" +
                                "(@emoji, @description, @amount, @category);";
                    using(SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@emoji", transactionInfo.emoji);
                        command.Parameters.AddWithValue("@description", transactionInfo.description);
                        command.Parameters.AddWithValue("@amount", transactionInfo.amount);
                        command.Parameters.AddWithValue("@category", transactionInfo.category);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            transactionInfo.emoji = ""; transactionInfo.description = ""; transactionInfo.amount = ""; transactionInfo.category="";
            successMessage = "New Transaction Added";

            Response.Redirect("/Transactions/Index");

        }

    }
}
