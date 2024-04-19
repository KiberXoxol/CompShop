using System;
using ComputerShop.Properties;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Security.RightsManagement;

namespace ComputerShop
{
    public class DataBaseConnection
    {
        public string connectionUrl = "data source=6.tcp.eu.ngrok.io, 16136;" +
            "Password=dmitriy_001;" +
            "Database=Computer_Shop;" +
            "TrustServerCertificate=True;" +
            "User Id=DmitriyMakeev;";
        public int readDatabase()
        {
            int UserID = 0;
            Console.WriteLine("call table");
            using (SqlConnection conn = new SqlConnection(connectionUrl))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"
                    Use Computer_Shop
                    SELECT *
                    FROM Пользователь;
                    ";
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        UserID = Convert.ToInt32(("" + dr["ID Пользователя"]).Trim()) + 1;
                    }
                    dr.Close();
                }
                conn.Close();
            }
            return UserID;
        }
        public Dictionary<string, Dictionary<string, string>> displayItems(string category)
        {
            Dictionary<string, Dictionary<string, string>> ItemsInformation = new Dictionary<string, Dictionary<string, string>>();

            Console.WriteLine("display table");
            using (SqlConnection conn = new SqlConnection(connectionUrl))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = $@"
                    Use Computer_Shop
                    SELECT *
                    FROM Товары
                    Where Категория = N'{category}'
                    ";
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Dictionary<string, string> information = new Dictionary<string, string>()
                        {
                            {"Название",""}, {"Цена",""}, {"Количество",""}, {"Оценка",""}, {"Фотография",""}, {"Компания",""}
                        };

                        information["Название"] = ("" + dr["Наименование"]).Trim();
                        information["Цена"] = ("" + dr["Цена"]).Trim();
                        information["Количество"] = ("" + dr["Количество (шт)"]).Trim();
                        information["Оценка"] = ("" + dr["Средняя оценка"]).Trim();
                        information["Фотография"] = ("" + dr["Фотография"]).Trim();
                        information["Компания"] = ("" + dr["Компания"]).Trim();
                        ItemsInformation["" + dr["Артикул"]] = information;
                    }
                    dr.Close();
                }
                conn.Close();
            }

            return ItemsInformation;
        }

        public void UserRegistration(Dictionary<string, string> info)
        {
            int id = readDatabase();
            using (SqlConnection connection = new SqlConnection(connectionUrl))
            {

                connection.Open();
                SqlCommand commandToAddInformationFromTable = new SqlCommand($@"
                Use Computer_Shop
                INSERT INTO Пользователь
                VALUES({id}, N'{info["Имя"]} {info["Фамилия"]}',
                N'{info["Пароль"]}', N'{info["Телефон"]}',
                N'{info["Почта"]}')");
                commandToAddInformationFromTable.Connection = connection;
                commandToAddInformationFromTable.ExecuteNonQuery();
            }

            Settings.Default["UserID"] = Convert.ToString(id);
            Settings.Default.Save();
        }

        public bool UserAuth(Dictionary<string, string> info)
        {
            using (SqlConnection conn = new SqlConnection(connectionUrl))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = $@"
                    Use Computer_Shop
                    SELECT [ID Пользователя]
                    FROM Пользователь
                    Where Телефон = {info["Телефон"]} and Пароль = N'{info["Пароль"]}';
                    ";

                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        int ID = Convert.ToInt32(dr["ID Пользователя"]);
                        Settings.Default["UserID"] = Convert.ToString(ID);
                        Settings.Default.Save();
                        return true;
                    }
                    dr.Close();
                }
                conn.Close();
            }
            return false;
        }
    }
}