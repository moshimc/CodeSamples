// ------------------------------------------------------------
// Project: ExportListsForImportActivity
// Purpose: Downloads file information into CSV
// Author: Matthew Collard
// ------------------------------------------------------------

// ------------------------------------------------------------
// Imports
// ------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using FuelSDK;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Web;
using System.IO;
using System.Net.Mime;
using System.Net.Mail;
using System.Configuration;
using System.Collections;
using CExactTarget.Utilities;


namespace ExportListsForImportActivity
{
    // ------------------------------------------------------------
    // Class: NewsletterPublicationListBooleanUpdater
    // Abstract: Call functions for exact target
    // ------------------------------------------------------------
    class ExportListsForImportActivity
    {


        // ------------------------------------------------------------
        // Properties
        // ------------------------------------------------------------
        private static OleDbConnection m_conAdministrator = null;
        private static Dictionary<string, string> dctListSubscribers = new Dictionary<string, string>();
        private static Dictionary<string, string> dctAllSubscribers = new Dictionary<string, string>();
        private static string fileLocation = "";
        private static string m_strDate = "";
        private static string m_strLoggingInfo = "";


        // ------------------------------------------------------------
        // Name: Main
        // Abstract: Where the magic happens
        // ------------------------------------------------------------
        static void Main(string[] args)
        {
            try
            {
                DateTime dtmNow = DateTime.Now;
                m_strDate = dtmNow.ToString("yyyy-MM-dd");
                Console.WriteLine(m_strDate);

                String strNewsletterFile = "NewsletterListForImportActivity_" + m_strDate + ".csv";
                String strRemindersFile = "RemindersListForImportActivity_" + m_strDate + ".csv";

                // Test Database Connection
                m_conAdministrator = CUtilities.OpenDatabaseConnection();
                m_conAdministrator.Open();

                // Get general array of subscriber key and email
                dctAllSubscribers = CExactTargetUtilities.GetSubscriberEmails();

                // Newsletter List
                fileLocation = "C:\\Users\\TD Programmer\\reports\\" + strNewsletterFile;

                // Export a list
                dctListSubscribers = CExactTargetUtilities.PrepareListForCSV("2726");

                // Create the CSV file
                CreateExcelReport(dctListSubscribers);
                
                // Reminders List
                fileLocation = "C:\\Users\\TD Programmer\\reports\\" + strRemindersFile;

                dctListSubscribers = null;
                // Export a list
                dctListSubscribers = CExactTargetUtilities.PrepareListForCSV("2727");

                // Create the CSV file
                CreateExcelReport(dctListSubscribers);
                
                // After saving, upload
                CUtilities.UploadFileToFTP("USERNAME", "PASSWORD", "ftp://ftp.s7.exacttarget.com:21", "C:/Users/TD Programmer/reports/" + strNewsletterFile, "Import/" + strNewsletterFile);
                CUtilities.UploadFileToFTP("USERNAME", "PASSWORD", "ftp://ftp.s7.exacttarget.com:21", "C:/Users/TD Programmer/reports/" + strRemindersFile, "Import/" + strRemindersFile);

                // Activate import activities 
                CExactTargetUtilities.StartImport("9CDAA8B6-89F5-4FEA-96CF-F26A2F527C58");
                CExactTargetUtilities.StartImport("8E157FAD-C220-4310-87DC-FB033DEEBF48");
                
            }
            catch (Exception excError)
            {
                m_strLoggingInfo += "Error: " + excError.ToString();
            }
            finally
            {
                // Close the connection if it's open.
                if (m_conAdministrator != null)
                {
                    m_conAdministrator.Close();
                }
            }
        }


        // ------------------------------------------------------------
        // Name: CreateExcelReport
        // Abstract: create an excel report
        // ------------------------------------------------------------
        private static void CreateExcelReport(Dictionary<string, string> dctSubscribers)
        {
            try
            {
                // String builder
                StringBuilder csv = new StringBuilder();
                string strFormattedLine = "";
                string strSubscriberKey = "";
                string strStatus = "";
                string strEmail = "";

                // Headers
                csv.AppendLine("Email Address, Subscriber Key, Status");

                foreach (KeyValuePair<string, string> entry in dctSubscribers)
                {
                    // Add our data
                    strSubscriberKey = entry.Key;
                    strStatus = entry.Value;
                    strEmail = dctAllSubscribers[strSubscriberKey];

                    strFormattedLine = string.Format("{0},{1},{2}", strEmail, strSubscriberKey, strStatus);
                    csv.AppendLine(strFormattedLine);
                }

                // Create our CSV here
                File.WriteAllText(fileLocation, csv.ToString());

            }
            catch (Exception excError)
            {
                // Display Error Message
                m_strLoggingInfo += "Error: " + excError.ToString();
            }
        }




    }
}
