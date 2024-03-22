using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyExpenseTracker.Pages.Transactions
{
    public class IndexModel : PageModel
    {
        public List<TransactionInfo> transaction = new List<TransactionInfo>();

        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=CONSOLE-USER;Initial Catalog=MyTracker;Integrated Security=True;trustservercertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM tracker";
                    using(SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TransactionInfo info = new TransactionInfo();
                                info.id = "" + reader.GetInt32(0);
                                info.created_at = reader.GetDateTime(1).ToString();
                                info.emoji = reader.GetString(2);
                                info.description = reader.GetString(3);
                                info.amount = "" + reader.GetDecimal(4);
                                info.category = reader.GetString(5);

                                transaction.Add(info);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
           
        }
    }
    public class TransactionInfo
    {
        public String id;
        public String created_at;
        public String emoji;
        public String description;
        public String amount;
        public String category;
    }
}
