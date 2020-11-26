using System;
using System.Collections.Generic;
using ReadyAppContacts.Models;
using MySql.Data.MySqlClient;


namespace ReadyAppContacts{

    public class ContactsDatabase{
        private  MySqlConnection conn;
        public ContactsDatabase(){
            initializeDatabase();
        }
        ~ContactsDatabase(){
            //Dispose(false);                     
        }

        private void initializeDatabase(){
        string connStr = "server=localhost;user=root;database=Contacts;port=3306;password=10";
        conn = new MySqlConnection(connStr);
        }



        #region getPhoneNumber
        public List<PhoneNumber> getPhoneNumber(string[] words){
            var query = "SELECT phone FROM Contacts @parameters";

            conn.Open();
            var results = new List<PhoneNumber>();
            var parameters = "";

            if(words.Length > 0){
                parameters = String.Format("WHERE {0}",String.Join(" OR ",words));

                using(var cmd = new MySqlCommand(query,conn)){
                    cmd.Parameters.AddWithValue("parameters",parameters);
                    var rdr = cmd.ExecuteReader();

                    while(rdr.Read()){
                        var phone = new PhoneNumber();
                        phone.Number = rdr["phone"] != DBNull.Value ? rdr["phone"].ToString() : null;
                        phone.Email = rdr["email"] != DBNull.Value ? rdr["email"].ToString() : null;
                        phone.Name = rdr["name"] != DBNull.Value ? rdr["name"].ToString() : null;
                        results.Add(phone);
                    }
                }
                
            }
            conn.Close();
            return results;
        }
        #endregion            



        #region getNumber
        public string[] getNumber(string name){
            var query = "SELECT phone FROM Contacts WHERE name SOUNDS LIKE @name";

            conn.Open();
            List<string> phone = new List<string>();
            using(var cmd = new MySqlCommand(query,conn)){
                cmd.Parameters.AddWithValue("name",name);
                var rdr = cmd.ExecuteReader();                
                while(rdr.Read()){
                    var phoneToList = rdr["phone"] != DBNull.Value ? Convert.ToString(rdr["phone"]) : null;
                    phone.Add(phoneToList);                    
                }
            }
            conn.Close();
            return phone.ToArray();
        }
        #endregion

        #region getEmail
        public string[] getEmail(string name){
            var query = "SELECT email FROM Contacts WHERE name SOUNDS LIKE @name";

            conn.Open();
            List<string> email = new List<string>();
            using(var cmd = new MySqlCommand(query,conn)){
                cmd.Parameters.AddWithValue("name",name);
                var rdr = cmd.ExecuteReader();            

                while(rdr.Read()){               
                    var emailToList = rdr["email"] != DBNull.Value ? Convert.ToString(rdr["email"]) : null;
                    email.Add(emailToList);                    
                }
            }
            conn.Close();
            return email.ToArray();
        }
        #endregion


        public string[] getContactsName(string name){
            var query = "SELECT name FROM Contacts WHERE name SOUNDS LIKE @name";

            conn.Open();
            List<string> responseName =  new List<string>();
            using(var cmd = new MySqlCommand(query,conn)){
                cmd.Parameters.AddWithValue("name",name);
                var rdr = cmd.ExecuteReader();

                while(rdr.Read()){
                    var nameToAdd = rdr["name"] != DBNull.Value ? Convert.ToString(rdr["name"]) : null;
                    responseName.Add(nameToAdd) ;                 
                }
            }
            conn.Close();
            return responseName.ToArray();
        }

        public bool deleteContact(string name){
            var query = @"DELETE FROM Contacts WHERE name = @name";

            conn.Open();
            try{
                using(var cmd = new MySqlCommand(query,conn)){
                    cmd.Parameters.AddWithValue("name",name);
                    var rdr = cmd.ExecuteReader();
                }
            }catch (System.Exception){
                conn.Close();
                return false;
            }
            conn.Close();
            return true;
        }

        public bool saveNewContact(string name, string number , string email){
            var query = @"INSERT INTO Contacts(name,phone,email)
            VALUES (@name, @phone, @email)";

            conn.Open();
            bool saved = false;

            try{
                using(var cmd = new MySqlCommand(query,conn)){
                    cmd.Parameters.AddWithValue("name",name);
                    cmd.Parameters.AddWithValue("phone",number);
                    cmd.Parameters.AddWithValue("email",email);
                    var rdr = cmd.ExecuteReader();
                }
            }
            catch (System.Exception ex){
                Console.WriteLine(ex.Message);
                conn.Close();
                return saved;
            }
            conn.Close();
            saved = true;
            return saved;
        }
    }
}