// ------------------------------------------------------------
// Class: CUtilities
// Abstract: Various utility procedures
// ------------------------------------------------------------


// ------------------------------------------------------------
// Imports
// ------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data.Odbc;
using FuelSDK;
using System.Collections.Specialized;
using System.Net;
using System.IO;

namespace CExactTarget.Utilities
{
    static class CUtilities
    {
        // ------------------------------------------------------------
        // Properties
        // ------------------------------------------------------------


        // ------------------------------------------------------------
        // Name: OpenDatabaseConnection
        // Abstract: Open a database connection
        // ------------------------------------------------------------
        public static OleDbConnection OpenDatabaseConnection()
        {
            OleDbConnection conDatabaseConnection = null;

            try
            {
                conDatabaseConnection = new OleDbConnection("Provider=PervasiveOLEDB;Data Source=TD;Location=10.1.100.140");
                //conDatabaseConnection = new OleDbConnection("Provider=PervasiveOLEDB;Data Source=10.1.100.140:1583;Initial Catalog=TD; User ID=; Password=");
                //conDatabaseConnection = new OleDbConnection("Provider=SQLOLEDB;Data Source=10.1.100.140:1583;Initial Catalog=TD; Integrated Security=SSPI");
            }
            catch (Exception excError)
            {
                Console.WriteLine("Error: " + excError.ToString());
            }

            return conDatabaseConnection;
        }


        // ------------------------------------------------------------
        // Name: OpenDatabaseConnection2
        // Abstract: Open a database connection
        // ------------------------------------------------------------
        public static OdbcConnection OpenDatabaseConnection2()
        {
            OdbcConnection conDatabaseConnection = null;

            try
            {
                conDatabaseConnection = new OdbcConnection("Driver={Pervasive ODBC Client Interface};ServerName=10.1.100.140:1583;dbq=@TD");
                //conDatabaseConnection = new OleDbConnection("Provider=PervasiveOLEDB;Data Source=10.1.100.140:1583;Initial Catalog=TD; User ID=; Password=");
                //conDatabaseConnection = new OleDbConnection("Provider=SQLOLEDB;Data Source=10.1.100.140:1583;Initial Catalog=TD; Integrated Security=SSPI");
            }
            catch (Exception excError)
            {
                Console.WriteLine("Error: " + excError.ToString());
            }

            return conDatabaseConnection;
        }


        // ------------------------------------------------------------
        // Name: OpenDatabaseConnection2
        // Abstract: Open a database connection
        // ------------------------------------------------------------
        public static void UploadFileToFTP(String strUsername, String strPassword, String strHost, String strFileLocation, String strDestination)
        {
            FtpWebRequest ftpRequest = null;
            FtpWebResponse ftpResponse = null;
            Stream ftpStream = null;

            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(strHost + "/" + strDestination);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(strUsername, strPassword);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                /* Establish Return Communication with the FTP Server */

                // Upload
                StreamReader sourceStream = new StreamReader(strFileLocation);
                byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());

                //FileStream sourceStream = File.OpenRead(strFileLocation);
                //byte[] fileContents = new byte[sourceStream.Length];
                sourceStream.Close();
                ftpRequest.ContentLength = fileContents.Length;

                Stream requestStream = ftpRequest.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();

                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();

                Console.WriteLine("Uplaod File Complete, status {0}", ftpResponse.StatusDescription);

                ftpResponse.Close();
            }
            catch (Exception excError)
            {
                Console.WriteLine("Error: " + excError.ToString());
            }
        }
    }
}
