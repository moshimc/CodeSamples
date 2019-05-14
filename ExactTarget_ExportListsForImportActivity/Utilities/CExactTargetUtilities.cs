// ------------------------------------------------------------
// Class: CExactTargetUtilities
// Purpose: API procedures
// Author: Matthew Collard
// ------------------------------------------------------------


// ------------------------------------------------------------
// Imports
// ------------------------------------------------------------
using FuelSDK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CExactTarget.Utilities
{
    static class CExactTargetUtilities
    {
        // ------------------------------------------------------------
        // Properties
        // ------------------------------------------------------------
        private static ET_Client m_etcTDClient = null;
        private static ET_Client m_etcTDClientShared = null;


        // ------------------------------------------------------------
        // Name: CUtilities
        // Abstract: Constructor
        // ------------------------------------------------------------
        static CExactTargetUtilities()
        {
            // Initialize our ET Client
            // Pass ClientID/ClientSecret
            NameValueCollection parameters = new NameValueCollection();
            //parameters.Add("clientId", "<InsertClientId>");
            //parameters.Add("clientSecret", "<InsertClientSecret>");

            // WORKS FOR Customer Unit Lists
            parameters.Add("clientId", "<InsertClientId>");
            parameters.Add("clientSecret", "<InsertClientSecret>");

            // WORKS FOR main subscribers and shared data extensions!!
            //parameters.Add("clientId", "<InsertClientId>");
            //parameters.Add("clientSecret", "<InsertClientSecret>");
            Console.WriteLine("ID: " + parameters.Get("clientID").ToString());
            m_etcTDClient = new ET_Client(parameters);

            // for shared data extensions
            NameValueCollection parametersShared = new NameValueCollection();
            parametersShared.Add("clientId", "<InsertClientId>");
            parametersShared.Add("clientSecret", "<InsertClientSecret>");
            m_etcTDClientShared = new ET_Client(parametersShared);
            Console.WriteLine("OK");
        }


        // ------------------------------------------------------------
        // Name: TestAPI
        // Abstract: Test the API out
        // ------------------------------------------------------------
        public static void TestAPI()
        {
            //ET_Client myclient = new ET_Client();
            ET_List list = new ET_List();
            list.AuthStub = m_etcTDClient;
            GetReturn getFR = list.Get();
            Console.WriteLine("Get Status: " + getFR.Status.ToString());
            Console.WriteLine("Message: " + getFR.Message.ToString());
            Console.WriteLine("Code: " + getFR.Code.ToString());
            Console.WriteLine("Results Length: " + getFR.Results.Length);
            foreach (ET_List ResultList in getFR.Results)
            {
                Console.WriteLine("--ID: " + ResultList.ID + ", Name: " + ResultList.ListName + ", Description: " + ResultList.Description);
            }
        }


        // ------------------------------------------------------------
        // Name: SetBouncesToUnsubscribed
        // Abstract: Retrieve new customers
        // ------------------------------------------------------------
        public static void SetBouncesToUnsubscribed()
        {
            try
            {
                ET_Subscriber GetSubscriber = new ET_Subscriber();
                GetSubscriber.AuthStub = m_etcTDClient;
                GetSubscriber.Props = new string[] { "SubscriberKey", "EmailAddress", "Status" };
                //GetSubscriber.SearchFilter = new SimpleFilterPart() { Property = "EmailAddress", SimpleOperator = SimpleOperators.equals, Value = new string[] { "matthew.collard@tirediscounters.com" } };
                GetSubscriber.Attributes = new FuelSDK.ET_ProfileAttribute[] { new ET_ProfileAttribute() { Name = "Bounce Count", Value = "3" } };
                GetReturn GetResponse = GetSubscriber.Get();
                Console.WriteLine("Get Status: " + GetResponse.Status.ToString());
                Console.WriteLine("Message: " + GetResponse.Message.ToString());
                Console.WriteLine("Code: " + GetResponse.Code.ToString());
                Console.WriteLine("Results Length: " + GetResponse.Results.Length);

                foreach (ET_Subscriber sub in GetResponse.Results)
                {
                    Console.WriteLine("--EmailAddress: " + sub.EmailAddress + " Status: " + sub.Status.ToString());
                }
            }
            catch (Exception excError)
            {
                Console.WriteLine("Error: " + excError.ToString());
            }
        }


        // ------------------------------------------------------------
        // Name: Test_AddSubscriberToList
        // Abstract: Test -- Add subscriber to list
        // ------------------------------------------------------------
        public static void Test_AddSubscriberToList()
        {

            string NewListName = "CSharpSDKAddSubscriberToList";
            string SubscriberTestEmail = "AddSubToListExample@bh.exacttarget.com";
            String strSubscriberKey = "TestTestTest";

            Console.WriteLine("--- Testing AddSubscriberToList ---");

            Console.WriteLine("\n Create List");
            ET_List postList = new ET_List();
            postList.AuthStub = m_etcTDClient;
            postList.ListName = NewListName;

            // Create the new list in ET
            PostReturn prList = postList.Post();

            if (prList.Status && prList.Results.Length > 0)
            {
                // Get List ID
                int newListID = prList.Results[0].Object.ID;

                Console.WriteLine("\n Create Subscriber on List");
                FuelReturn hrAddSub = m_etcTDClient.AddSubscribersToList(SubscriberTestEmail, strSubscriberKey, new List<int>() { newListID });
                Console.WriteLine("Helper Status: " + hrAddSub.Status.ToString());
                Console.WriteLine("Message: " + hrAddSub.Message.ToString());
                Console.WriteLine("Code: " + hrAddSub.Code.ToString());

                Console.WriteLine("\n Retrieve all Subscribers on the List");
                ET_List_Subscriber getListSub = new ET_List_Subscriber();
                getListSub.AuthStub = m_etcTDClient;
                getListSub.Props = new string[] { "ObjectID", "SubscriberKey", "CreatedDate", "Client.ID", "Client.PartnerClientKey", "ListID", "Status" };
                getListSub.SearchFilter = new SimpleFilterPart() { Property = "ListID", SimpleOperator = SimpleOperators.equals, Value = new string[] { newListID.ToString() } };
                GetReturn getResponse = getListSub.Get();
                Console.WriteLine("Get Status: " + getResponse.Status.ToString());
                Console.WriteLine("Message: " + getResponse.Message.ToString());
                Console.WriteLine("Code: " + getResponse.Code.ToString());
                Console.WriteLine("Results Length: " + getResponse.Results.Length);
                foreach (ET_List_Subscriber ResultListSub in getResponse.Results)
                {
                    Console.WriteLine("--ListID: " + ResultListSub.ListID + ", SubscriberKey(EmailAddress): " + ResultListSub.SubscriberKey);
                }

                Console.WriteLine("\n Delete List");
                postList.ID = newListID;

                // Deletes the list
                DeleteReturn drList = postList.Delete();
                Console.WriteLine("Delete Status: " + drList.Status.ToString());

                // Delete the subscriber
                ET_Subscriber deleteSubscriber = new ET_Subscriber();
                deleteSubscriber.AuthStub = m_etcTDClient;
                deleteSubscriber.EmailAddress = SubscriberTestEmail;
                deleteSubscriber.SubscriberKey = strSubscriberKey;
                //Console.WriteLine("Subscriber's Customer Key: " + deleteSubscriber.CustomerKey.ToString() + "; Subscriber Key: " + deleteSubscriber.SubscriberKey);
                DeleteReturn deleteResponse = deleteSubscriber.Delete();
                Console.WriteLine("Delete Status: " + deleteResponse.Status.ToString());
                Console.WriteLine("Message: " + deleteResponse.Message.ToString());
                Console.WriteLine("Code: " + deleteResponse.Code.ToString());
                Console.WriteLine("Results Length: " + deleteResponse.Results.Length);
            }
        }

        // ------------------------------------------------------------
        // Name: Test_AddSubscriberToList
        // Abstract: Test -- Add subscriber to list
        // ------------------------------------------------------------
        public static void Test_AddNewSubscriberToDataExtension()
        {
            /*
            Console.WriteLine("--- Testing Subscriber ---");
            string SubscriberTestEmail = "CSharpSDKExample223@bh.exacttarget.com";

            Console.WriteLine("\n Create Subscriber");
            ET_Subscriber postSub = new ET_Subscriber();
            postSub.AuthStub = m_etcTDClient;
            postSub.EmailAddress = SubscriberTestEmail;
            postSub.Attributes = new FuelSDK.ET_ProfileAttribute[] { new ET_ProfileAttribute() { Name = "First Name", Value = "ExactTarget Example" } };
            PostReturn postResponse = postSub.Post();
            Console.WriteLine("Post Status: " + postResponse.Status.ToString());
            Console.WriteLine("Message: " + postResponse.Message.ToString());
            Console.WriteLine("Code: " + postResponse.Code.ToString());
            Console.WriteLine("Results Length: " + postResponse.Results.Length);

            if (postResponse.Results.Length > 0)
            {
                Console.WriteLine("--NewID: " + postResponse.Results[0].NewID.ToString());
                foreach (ET_ProfileAttribute attr in ((ET_Subscriber)postResponse.Results[0].Object).Attributes)
                {
                    Console.WriteLine("Name: " + attr.Name + ", Value: " + attr.Value);
                }
            }

            Console.WriteLine("\n Add a row to a data extension (using CustomerKey)");
            ET_DataExtensionRow deRowPost = new ET_DataExtensionRow();
            deRowPost.AuthStub = m_etcTDClient;
            deRowPost.DataExtensionCustomerKey = NameOfTestDataExtension;
            deRowPost.ColumnValues.Add("Name", "Example Name");
            deRowPost.ColumnValues.Add("OtherColumn", "Different Example Text");
            PostReturn prRowResponse = deRowPost.Post();
            Console.WriteLine("Post Status: " + prRowResponse.Status.ToString());
            Console.WriteLine("Message: " + prRowResponse.Message.ToString());
            Console.WriteLine("Code: " + prRowResponse.Code.ToString());
            Console.WriteLine("Results Length: " + prRowResponse.Results.Length);

            Console.WriteLine("\n Add a row to a data extension (using Name)");
            ET_DataExtensionRow deRowPost2 = new ET_DataExtensionRow();
            deRowPost2.AuthStub = m_etcTDClient;
            deRowPost2.DataExtensionName = NameOfTestDataExtension;
            deRowPost2.ColumnValues.Add("Name", "Example Name3");
            deRowPost2.ColumnValues.Add("OtherColumn", "Different Example Text");
            PostReturn prRowResponse2 = deRowPost2.Post();
            Console.WriteLine("Post Status: " + prRowResponse2.Status.ToString());
            Console.WriteLine("Message: " + prRowResponse2.Message.ToString());
            Console.WriteLine("Code: " + prRowResponse2.Code.ToString());
            Console.WriteLine("Results Length: " + prRowResponse2.Results.Length);

            Console.WriteLine("\n Retrieve All Rows from DataExtension");
            ET_DataExtensionRow deRowGet = new ET_DataExtensionRow();
            deRowGet.AuthStub = m_etcTDClient;
            deRowGet.DataExtensionName = NameOfTestDataExtension;
            deRowGet.Props = new string[] { "Name", "OtherColumn" };
            GetReturn grRow = deRowGet.Get();
            Console.WriteLine("Post Status: " + grRow.Status.ToString());
            Console.WriteLine("Message: " + grRow.Message.ToString());
            Console.WriteLine("Code: " + grRow.Code.ToString());
            Console.WriteLine("Results Length: " + grRow.Results.Length);

            if (getColumnResponse.Status)
            {
                foreach (ET_DataExtensionRow column in grRow.Results)
                {
                    Console.WriteLine("--Name: " + column.ColumnValues["Name"] + " - OtherColumn: " + column.ColumnValues["OtherColumn"]);
                }
            }

            Console.WriteLine("\n Update a row in  a data extension");
            ET_DataExtensionRow deRowPatch = new ET_DataExtensionRow();
            deRowPatch.AuthStub = m_etcTDClient;
            deRowPatch.DataExtensionCustomerKey = NameOfTestDataExtension;
            deRowPatch.ColumnValues.Add("Name", "Example Name");
            deRowPatch.ColumnValues.Add("OtherColumn", "New Value for First Column");
            PatchReturn patchRowResponse = deRowPatch.Patch();
            Console.WriteLine("Post Status: " + patchRowResponse.Status.ToString());
            Console.WriteLine("Message: " + patchRowResponse.Message.ToString());
            Console.WriteLine("Code: " + patchRowResponse.Code.ToString());
            Console.WriteLine("Results Length: " + patchRowResponse.Results.Length);

            Console.WriteLine("\n Retrieve only updated row");
            ET_DataExtensionRow deRowGetSingle = new ET_DataExtensionRow();
            deRowGetSingle.AuthStub = m_etcTDClient;
            deRowGetSingle.DataExtensionName = NameOfTestDataExtension;
            deRowGetSingle.Props = new string[] { "Name", "OtherColumn" };
            deRowGetSingle.SearchFilter = new SimpleFilterPart() { Property = "Name", SimpleOperator = SimpleOperators.equals, Value = new string[] { "Example Name" } };
            GetReturn grSingleRow = deRowGetSingle.Get();
            Console.WriteLine("Post Status: " + grSingleRow.Status.ToString());
            Console.WriteLine("Message: " + grSingleRow.Message.ToString());
            Console.WriteLine("Code: " + grSingleRow.Code.ToString());
            Console.WriteLine("Results Length: " + grSingleRow.Results.Length);

            if (getColumnResponse.Status)
            {
                foreach (ET_DataExtensionRow column in grSingleRow.Results)
                {
                    Console.WriteLine("--Name: " + column.ColumnValues["Name"] + " - OtherColumn: " + column.ColumnValues["OtherColumn"]);
                }
            }

            Console.WriteLine("\n Delete a row from a data extension)");
            ET_DataExtensionRow deRowDelete = new ET_DataExtensionRow();
            deRowDelete.AuthStub = m_etcTDClient;
            deRowDelete.DataExtensionCustomerKey = NameOfTestDataExtension;
            deRowDelete.ColumnValues.Add("Name", "Example Name");
            DeleteReturn drRowResponse = deRowDelete.Delete();
            Console.WriteLine("Post Status: " + drRowResponse.Status.ToString());
            Console.WriteLine("Message: " + drRowResponse.Message.ToString());
            Console.WriteLine("Code: " + drRowResponse.Code.ToString());
            Console.WriteLine("Results Length: " + drRowResponse.Results.Length);
            */
        }


        // ------------------------------------------------------------
        // Name: RetrieveSubscribersFromList
        // Abstract: Retrieve subscribers from a list
        // ------------------------------------------------------------
        public static void RetrieveLists()
        {
            try
            {
                Console.WriteLine("\n Retrieve all Subscribers on the List");
                ET_List getList = new ET_List();
                getList.AuthStub = m_etcTDClient;
                GetReturn getResponse = getList.Get();
                Console.WriteLine("Get Status: " + getResponse.Status.ToString());
                Console.WriteLine("Message: " + getResponse.Message.ToString());
                Console.WriteLine("Code: " + getResponse.Code.ToString());
                Console.WriteLine("Results Length: " + getResponse.Results.Length);
                foreach (ET_List ResultListSub in getResponse.Results)
                {
                    Console.WriteLine("--ListID: " + ResultListSub.ID + ", CustomerKey: " + ResultListSub.CustomerKey);
                }
            }
            catch (Exception excError)
            {
                // Display Error
                Console.WriteLine("Error: " + excError.ToString());
            }
        }


        // ------------------------------------------------------------
        // Name: RetrieveSubscribersFromList
        // Abstract: Retrieve subscribers from a list
        // ------------------------------------------------------------
        public static Dictionary<string, string> RetrieveSubscribersFromList()
        {
            Dictionary<string, string> dctSubscribers = new Dictionary<string, string>();

            try
            {
                int intIndex = 0;
                string strSubscriberKey = "";

                Console.WriteLine("\n Retrieve all Subscribers on the List");
                ET_List_Subscriber getListSub = new ET_List_Subscriber();
                getListSub.AuthStub = m_etcTDClient;
                getListSub.Props = new string[] { "ObjectID", "SubscriberKey", "CreatedDate", "Client.ID", "Client.PartnerClientKey", "ListID", "Status" };
                getListSub.SearchFilter = new SimpleFilterPart() { Property = "ListID", SimpleOperator = SimpleOperators.equals, Value = new string[] { "3075" } };
                GetReturn getResponse = getListSub.Get();
                Console.WriteLine("Get Status: " + getResponse.Status.ToString());
                Console.WriteLine("Message: " + getResponse.Message.ToString());
                Console.WriteLine("Code: " + getResponse.Code.ToString());
                Console.WriteLine("Results Length: " + getResponse.Results.Length);

                ET_Subscriber getSub = new ET_Subscriber();
                getSub.AuthStub = m_etcTDClient;
                getSub.Props = new string[] { "SubscriberKey", "EmailAddress", "Status" };

                // Loop through subscribers on the list and add their keys to our array
                foreach (ET_List_Subscriber ResultListSub in getResponse.Results)
                {
                    // Subscriber Key
                    strSubscriberKey = ResultListSub.SubscriberKey;

                    // Get information on that subscriber
                    getSub.SearchFilter = new SimpleFilterPart() { Property = "SubscriberKey", SimpleOperator = SimpleOperators.equals, Value = new string[] { strSubscriberKey } };
                    GetReturn getSubResponse = getSub.Get();

                    foreach (ET_Subscriber sub in getSubResponse.Results)
                    {
                        if (dctSubscribers.TryGetValue(sub.EmailAddress.ToUpper(), out strSubscriberKey) == false && dctSubscribers.ContainsKey(sub.EmailAddress.ToUpper()) == false)
                        {
                            intIndex += 1;
                            if (intIndex == 1323)
                            {
                                Console.WriteLine("EHY");
                            }
                            dctSubscribers.Add(sub.EmailAddress.ToUpper(), ResultListSub.SubscriberKey);
                            Console.WriteLine("Count: " + intIndex.ToString() + "; Subscriber Key: " + ResultListSub.SubscriberKey);
                        }
                    }
                }



            }
            catch (Exception excError)
            {
                // Display Error
                Console.WriteLine("Error: " + excError.ToString());
            }

            return dctSubscribers;
        }


        // ------------------------------------------------------------
        // Name: UpdateConvertedProspects
        // Abstract: Retrieve subscribers from a list
        // ------------------------------------------------------------
        public static Dictionary<string, string> UpdateConvertedProspects(Dictionary<string, string> dctSubscriberList, Dictionary<string, string> dctSubscriberKeys,
                                                                          List<string> alstrConvertedKey, List<string> alstrConvertedEmail, List<string> alstrConvertedSubscriberKey)
        {
            Dictionary<string, string> dctSubscribers = new Dictionary<string, string>();

            try
            {
                string strSubscriberKey = "";
                string strEmail = "";
                string strCustomerNumber = "";

                // Loop through each converted prospect
                foreach (KeyValuePair<string, string> entry in dctSubscriberList)
                {
                    // Data
                    DateTime dtmCreatedDate = DateTime.Now;

                    // Get customer number and email
                    strCustomerNumber = entry.Key;
                    strEmail = entry.Value;

                    // Get subscriber key based on that email
                    strSubscriberKey = dctSubscriberKeys[strEmail];

                    // Get subscriber dates
                    ET_Subscriber getSub = new ET_Subscriber();
                    getSub.AuthStub = m_etcTDClient;
                    getSub.Props = new string[] { "SubscriberKey", "EmailAddress", "Status", "CreatedDate" };
                    getSub.SearchFilter = new SimpleFilterPart() { Property = "SubscriberKey", SimpleOperator = SimpleOperators.equals, Value = new string[] { strSubscriberKey } };
                    GetReturn getResponse = getSub.Get();
                    Console.WriteLine("Get Status: " + getResponse.Status.ToString());
                    Console.WriteLine("Message: " + getResponse.Message.ToString());
                    Console.WriteLine("Code: " + getResponse.Code.ToString());
                    Console.WriteLine("Results Length: " + getResponse.Results.Length);

                    foreach (ET_Subscriber sub in getResponse.Results)
                    {
                        Console.WriteLine("--EmailAddress: " + sub.EmailAddress + " Status: " + sub.Status.ToString());
                        dtmCreatedDate = sub.CreatedDate;
                    }

                    // Create subscriber with same information but correct SubscriberKey
                    Console.WriteLine("\n Create Subscriber");
                    ET_Subscriber postSub = new ET_Subscriber();
                    postSub.AuthStub = m_etcTDClient;
                    postSub.EmailAddress = strEmail;
                    postSub.SubscriberKey = strCustomerNumber;
                    postSub.CreatedDate = dtmCreatedDate;
                    PostReturn postResponse = postSub.Post();
                    Console.WriteLine("Post Status: " + postResponse.Status.ToString());
                    Console.WriteLine("Message: " + postResponse.Message.ToString());
                    Console.WriteLine("Code: " + postResponse.Code.ToString());
                    Console.WriteLine("Results Length: " + postResponse.Results.Length);

                    // Add Subscriber to Newsletter and Reminders List
                    // Newsletter ListID: 2726; Reminders ListID: 2727  
                    FuelReturn hrAddSub = m_etcTDClient.AddSubscribersToList(strEmail, strCustomerNumber, new List<int>() { 2726 });
                    Console.WriteLine("Helper Status: " + hrAddSub.Status.ToString());
                    Console.WriteLine("Message: " + hrAddSub.Message.ToString());
                    Console.WriteLine("Code: " + hrAddSub.Code.ToString());

                    hrAddSub = m_etcTDClient.AddSubscribersToList(strEmail, strCustomerNumber, new List<int>() { 2727 });
                    Console.WriteLine("Helper Status: " + hrAddSub.Status.ToString());
                    Console.WriteLine("Message: " + hrAddSub.Message.ToString());
                    Console.WriteLine("Code: " + hrAddSub.Code.ToString());

                    // Delete old subscriber
                    ET_Subscriber deleteSub = new ET_Subscriber();
                    deleteSub.AuthStub = m_etcTDClient;
                    deleteSub.EmailAddress = strEmail;
                    deleteSub.SubscriberKey = strSubscriberKey;
                    DeleteReturn deleteResponse = deleteSub.Delete();
                    Console.WriteLine("Delete Status: " + deleteResponse.Status.ToString());
                    Console.WriteLine("Message: " + deleteResponse.Message.ToString());
                    Console.WriteLine("Code: " + deleteResponse.Code.ToString());
                    Console.WriteLine("Results Length: " + deleteResponse.Results.Length);

                    // Add to array list
                    alstrConvertedKey.Add(strCustomerNumber);
                    alstrConvertedEmail.Add(strEmail);
                    alstrConvertedSubscriberKey.Add(strSubscriberKey);
                }

            }
            catch (Exception excError)
            {
                // Display Error
                Console.WriteLine("Error: " + excError.ToString());
            }

            return dctSubscribers;
        }


        // ------------------------------------------------------------
        // Name: UpdateConvertedProspects
        // Abstract: Retrieve subscribers from a list
        // ------------------------------------------------------------
        public static Dictionary<string, string> GetDuplicateSubscribers()
        {
            Dictionary<string, string> dctDuplicateSubscribers = new Dictionary<string, string>();
            Dictionary<string, string> dctSubscribers = new Dictionary<string, string>();
            Dictionary<string, string> dctAllSubscribers = new Dictionary<string, string>();
            try
            {
                string strCustomerNumber = "";
                string strSubscriberKey = "";
                string strEmail = "";

                // Get subscriber dates
                ET_Subscriber getSub = new ET_Subscriber();
                getSub.AuthStub = m_etcTDClient;
                getSub.Props = new string[] { "SubscriberKey", "EmailAddress" };
                GetReturn getResponse = getSub.Get();
                Console.WriteLine("Get Status: " + getResponse.Status.ToString());
                Console.WriteLine("Message: " + getResponse.Message.ToString());
                Console.WriteLine("Code: " + getResponse.Code.ToString());
                Console.WriteLine("Results Length: " + getResponse.Results.Length);
                while (getResponse.MoreResults == true)
                {

                    foreach (ET_Subscriber sub in getResponse.Results)
                    {
                        Console.WriteLine("SubscriberKey: " + sub.SubscriberKey);

                        // Add to our list
                        dctAllSubscribers.Add(sub.SubscriberKey, sub.EmailAddress);
                    }

                    getResponse = getSub.GetMoreResults();
                }

                foreach (KeyValuePair<string, string> entry in dctAllSubscribers)
                {
                    strSubscriberKey = entry.Key;
                    strEmail = entry.Value;
                    // Add to duplicates if email already exists
                    if (dctSubscribers.ContainsValue(strEmail) == true)
                    {
                        // Get customer number from duplicate if duplicate hasn't been logged
                        if (dctDuplicateSubscribers.ContainsValue(strEmail) == false)
                        {
                            strCustomerNumber = dctSubscribers.FirstOrDefault(x => x.Value == strEmail).Key;

                            // Add (both) duplicate entries
                            dctDuplicateSubscribers.Add(strSubscriberKey, strEmail);
                            dctDuplicateSubscribers.Add(strCustomerNumber, strEmail);
                        }
                        else
                        {
                            dctDuplicateSubscribers.Add(strSubscriberKey, strEmail);
                        }
                    }
                    else
                    {
                        // Add to our list
                        dctSubscribers.Add(strSubscriberKey, strEmail);
                    }
                }

            }
            catch (Exception excError)
            {
                // Display Error
                Console.WriteLine("Error: " + excError.ToString());
            }

            return dctDuplicateSubscribers;
        }


        // ------------------------------------------------------------
        // Name: UpdateConvertedProspects
        // Abstract: Retrieve subscribers from a list
        // ------------------------------------------------------------
        public static void SetVerifiedEmailStatus()
        {
            Dictionary<string, string> dctAllSubscribers = new Dictionary<string, string>();
            Dictionary<string, string> dctDataExtensionSubscribers = new Dictionary<string, string>();
            Dictionary<string, string> dctNewsletterListSubscribers = new Dictionary<string, string>();
            Dictionary<string, string> dctDENewsletterSubscribers = new Dictionary<string, string>();
            Dictionary<string, string> dctRemindersListSubscribers = new Dictionary<string, string>();
            Dictionary<string, string> dctDERemindersSubscribers = new Dictionary<string, string>();

            List<string> alstrValidSubscriberKeys = new List<string>();

            try
            {
                string strSubscriberKey = "";
                string strEmail = "";
                string strCustomerNumber = "";
                string strStatus = "";
                string strDEStatus = "";
                int intUpdateRecordCount = 0;
                // Data
                DateTime dtmCreatedDate = DateTime.Now;

                
                // Get subscriber dates
                ET_Subscriber getSub = new ET_Subscriber();
                getSub.AuthStub = m_etcTDClientShared;
                getSub.Props = new string[] { "SubscriberKey", "EmailAddress", "Status", "CreatedDate" };
                GetReturn getResponse = getSub.Get();
                
                Console.WriteLine("Get Status: " + getResponse.Status.ToString());
                Console.WriteLine("Message: " + getResponse.Message.ToString());
                Console.WriteLine("Code: " + getResponse.Code.ToString());
                Console.WriteLine("Results Length: " + getResponse.Results.Length);
                int intResults = getResponse.Results.Length;

                // Get all subscriber keys associated with active subscribers
                while (getResponse.MoreResults == true || intResults > 0)
                {
                    foreach (ET_Subscriber sub in getResponse.Results)
                    {
                        strStatus = sub.Status.ToString();
                        dctAllSubscribers.Add(sub.SubscriberKey, strStatus);
                        Console.WriteLine("Added EmailAddress: " + sub.EmailAddress + " Status: " + strStatus);
                        
                    }

                    getResponse = getSub.GetMoreResults();
                    intResults = getResponse.Results.Length;
                }
                

                // Get all customers and their 'VerifiedEmail' field from CustomerBaseDatabase
                Console.WriteLine("\n Retrieve All Rows from DataExtension");
                ET_DataExtensionRow deRowGet = new ET_DataExtensionRow();
                deRowGet.AuthStub = m_etcTDClientShared;
                deRowGet.DataExtensionName = "CustomerDatabase";
                deRowGet.Props = new string[] { "SubscriberKey", "VerifiedEmail" };
                GetReturn grRow = deRowGet.Get();
                Console.WriteLine("Get Status: " + grRow.Status.ToString());
                Console.WriteLine("Message: " + grRow.Message.ToString());
                Console.WriteLine("Code: " + grRow.Code.ToString());
                Console.WriteLine("Results Length: " + grRow.Results.Length);
                intResults = grRow.Results.Length;
                string x;
                while (grRow.MoreResults == true || intResults > 0)
                {
                    foreach (ET_DataExtensionRow column in grRow.Results)
                    {
                        strSubscriberKey = column.ColumnValues["SubscriberKey"];
                        strStatus = column.ColumnValues["VerifiedEmail"];

                        if (dctAllSubscribers.TryGetValue(strSubscriberKey, out x) == true)
                        {
                            dctDataExtensionSubscribers.Add(strSubscriberKey, strStatus);
                            Console.WriteLine("Added customer: " + strSubscriberKey);
                        }

                    }

                    grRow = deRowGet.GetMoreResults();
                    intResults = grRow.Results.Length;
                }            

                // Update statuses
                ET_DataExtensionRow deRowPatch = null;
                foreach (KeyValuePair<string, string> entry in dctDataExtensionSubscribers)
                {
                    strSubscriberKey = entry.Key;
                    strDEStatus = entry.Value;

                    // if our status = active, set to true
                    strStatus = dctAllSubscribers[strSubscriberKey];
                    if (strStatus.Equals("Active") == true)
                    {
                        strStatus = "True";
                    }
                    else
                    {
                        strStatus = "False";
                    }

                    // Is the status of our subscriber the same on the All Subscribers list as it is on the Data Extension?
                    if (strDEStatus.Equals(strStatus) == false)
                    {
                        intUpdateRecordCount += 1;
                        // No, update the verified email field
                        deRowPatch = new ET_DataExtensionRow();
                        deRowPatch.AuthStub = m_etcTDClientShared;
                        deRowPatch.DataExtensionCustomerKey = "CustomerDBKey";
                        deRowPatch.ColumnValues.Add("SubscriberKey", strSubscriberKey);
                        deRowPatch.ColumnValues.Add("VerifiedEmail", strStatus);
                        PatchReturn patchRowResponse = deRowPatch.Patch();
                        Console.WriteLine("Post Status: " + patchRowResponse.Status.ToString());
                        Console.WriteLine("Count: " + intUpdateRecordCount.ToString());
                    }
                }

                Console.WriteLine("Done editing email statuses!");
   
            }
            catch (Exception excError)
            {
                // Display Error
                Console.WriteLine("Error: " + excError.ToString());
            }
        }


        // ------------------------------------------------------------
        // Name: UpdateConvertedProspects
        // Abstract: Retrieve subscribers from a list
        // ------------------------------------------------------------
        public static void SetPublicationListBooleans()
        {
            Dictionary<string, string> dctNewsletterListSubscribers = new Dictionary<string, string>();
            Dictionary<string, string> dctDENewsletterSubscribers = new Dictionary<string, string>();
            Dictionary<string, string> dctRemindersListSubscribers = new Dictionary<string, string>();
            Dictionary<string, string> dctDERemindersSubscribers = new Dictionary<string, string>();

            List<string> alstrValidSubscriberKeys = new List<string>();

            try
            {
                string strSubscriberKey = "";
                string strStatus = "";
                string strDEStatus = "";
                int intUpdateRecordCount = 0;
                int intResults = 0;
                string x;

                // Data
                DateTime dtmCreatedDate = DateTime.Now;


                // Newsletter Publication List
                // Get subscriber dates
                ET_List_Subscriber getListSub = new ET_List_Subscriber();
                getListSub.AuthStub = m_etcTDClient;
                getListSub.Props = new string[] { "SubscriberKey", "Status", "CreatedDate" };
                getListSub.SearchFilter = new SimpleFilterPart() { Property = "ListID", SimpleOperator = SimpleOperators.equals, Value = new string[] { "2726" } };
                GetReturn getLSResponse = getListSub.Get();
                Console.WriteLine("Get Status: " + getLSResponse.Status.ToString());
                Console.WriteLine("Message: " + getLSResponse.Message.ToString());
                Console.WriteLine("Code: " + getLSResponse.Code.ToString());
                Console.WriteLine("Results Length: " + getLSResponse.Results.Length);
                intResults = getLSResponse.Results.Length;

                // Get all subscriber keys associated with active subscribers
                while (getLSResponse.MoreResults == true || intResults > 0)
                {
                    foreach (ET_List_Subscriber sub in getLSResponse.Results)
                    {
                        strStatus = sub.Status.ToString();
                        dctNewsletterListSubscribers.Add(sub.SubscriberKey, strStatus);
                        Console.WriteLine("Added Subscriber: " + sub.SubscriberKey + " Status: " + strStatus);

                    }

                    getLSResponse = getListSub.GetMoreResults();
                    intResults = getLSResponse.Results.Length;
                }

                Console.WriteLine("Retrieved Newsletter List Subscription Status");

                // Get all customers and their 'NewsletterPublicationList' field from CustomerBaseDatabase
                Console.WriteLine("\n Retrieve All Rows from DataExtension");
                ET_DataExtensionRow deRowGetNewsletter = new ET_DataExtensionRow();
                deRowGetNewsletter.AuthStub = m_etcTDClientShared;
                deRowGetNewsletter.DataExtensionName = "CustomerDatabase";
                deRowGetNewsletter.Props = new string[] { "SubscriberKey", "NewsletterPublicationList" };
                GetReturn grRowNewsletter = deRowGetNewsletter.Get();
                Console.WriteLine("Get Status: " + grRowNewsletter.Status.ToString());
                Console.WriteLine("Message: " + grRowNewsletter.Message.ToString());
                Console.WriteLine("Code: " + grRowNewsletter.Code.ToString());
                Console.WriteLine("Results Length: " + grRowNewsletter.Results.Length);
                intResults = grRowNewsletter.Results.Length;

                while (grRowNewsletter.MoreResults == true || intResults > 0)
                {
                    foreach (ET_DataExtensionRow column in grRowNewsletter.Results)
                    {
                        strSubscriberKey = column.ColumnValues["SubscriberKey"];
                        strStatus = column.ColumnValues["NewsletterPublicationList"];

                        if (dctNewsletterListSubscribers.TryGetValue(strSubscriberKey, out x) == true)
                        {
                            dctDENewsletterSubscribers.Add(strSubscriberKey, strStatus);
                            Console.WriteLine("Added NPL customer: " + strSubscriberKey);
                        }

                    }

                    grRowNewsletter = deRowGetNewsletter.GetMoreResults();
                    intResults = grRowNewsletter.Results.Length;
                }

                // Update statuses
                ET_DataExtensionRow deRowPatchNewsletter = null;
                foreach (KeyValuePair<string, string> entry in dctDENewsletterSubscribers)
                {
                    strSubscriberKey = entry.Key;
                    strDEStatus = entry.Value;

                    // if our status = active, set to true
                    strStatus = dctNewsletterListSubscribers[strSubscriberKey];
                    if (strStatus.Equals("Active") == true)
                    {
                        strStatus = "True";
                    }
                    else
                    {
                        strStatus = "False";
                    }

                    // Is the status of our subscriber the same on the All Subscribers list as it is on the Data Extension?
                    if (strDEStatus.Equals(strStatus) == false)
                    {
                        intUpdateRecordCount += 1;
                        // No, update the verified email field
                        deRowPatchNewsletter = new ET_DataExtensionRow();
                        deRowPatchNewsletter.AuthStub = m_etcTDClientShared;
                        deRowPatchNewsletter.DataExtensionCustomerKey = "CustomerDBKey";
                        deRowPatchNewsletter.ColumnValues.Add("SubscriberKey", strSubscriberKey);
                        deRowPatchNewsletter.ColumnValues.Add("NewsletterPublicationList", strStatus);
                        PatchReturn patchRowResponse = deRowPatchNewsletter.Patch();
                        Console.WriteLine("Post Status: " + patchRowResponse.Status.ToString());
                        Console.WriteLine("Count: " + intUpdateRecordCount.ToString());
                        Console.WriteLine("Changed newsletter boolean for subscriber: " + strSubscriberKey + " from " + strDEStatus + " to " + strStatus);
                    }
                }
                
                Console.WriteLine("Completed Newsletter Boolean Update");

                // Reminders Publication List
                // Get subscriber dates
                ET_List_Subscriber getListSubReminders = new ET_List_Subscriber();
                getListSubReminders.AuthStub = m_etcTDClient;
                getListSubReminders.Props = new string[] { "SubscriberKey", "Status", "CreatedDate" };
                getListSubReminders.SearchFilter = new SimpleFilterPart() { Property = "ListID", SimpleOperator = SimpleOperators.equals, Value = new string[] { "2727" } };
                GetReturn getLSResponseReminders = getListSubReminders.Get();
                Console.WriteLine("Get Status: " + getLSResponseReminders.Status.ToString());
                Console.WriteLine("Message: " + getLSResponseReminders.Message.ToString());
                Console.WriteLine("Code: " + getLSResponseReminders.Code.ToString());
                Console.WriteLine("Results Length: " + getLSResponseReminders.Results.Length);
                intResults = getLSResponseReminders.Results.Length;

                // Get all subscriber keys associated with active subscribers
                while (getLSResponseReminders.MoreResults == true || intResults > 0)
                {
                    foreach (ET_List_Subscriber sub in getLSResponseReminders.Results)
                    {
                        strStatus = sub.Status.ToString();
                        dctRemindersListSubscribers.Add(sub.SubscriberKey, strStatus);
                        Console.WriteLine("Added Subscriber: " + sub.SubscriberKey + " Status: " + strStatus);

                    }

                    getLSResponseReminders = getListSubReminders.GetMoreResults();
                    intResults = getLSResponseReminders.Results.Length;
                }

                Console.WriteLine("Retrieved Reminders List Subscription Status");

                // Get all customers and their 'ReminderssPublicationList' field from CustomerBaseDatabase
                Console.WriteLine("\n Retrieve All Rows from DataExtension");
                ET_DataExtensionRow deRowGetReminders = new ET_DataExtensionRow();
                deRowGetReminders.AuthStub = m_etcTDClientShared;
                deRowGetReminders.DataExtensionName = "CustomerDatabase";
                deRowGetReminders.Props = new string[] { "SubscriberKey", "RemindersPublicationList" };
                GetReturn grRowReminders = deRowGetReminders.Get();
                Console.WriteLine("Get Status: " + grRowReminders.Status.ToString());
                Console.WriteLine("Message: " + grRowReminders.Message.ToString());
                Console.WriteLine("Code: " + grRowReminders.Code.ToString());
                Console.WriteLine("Results Length: " + grRowReminders.Results.Length);
                intResults = grRowReminders.Results.Length;

                while (grRowReminders.MoreResults == true || intResults > 0)
                {
                    foreach (ET_DataExtensionRow column in grRowReminders.Results)
                    {
                        strSubscriberKey = column.ColumnValues["SubscriberKey"];
                        strStatus = column.ColumnValues["RemindersPublicationList"];

                        if (dctRemindersListSubscribers.TryGetValue(strSubscriberKey, out x) == true)
                        {
                            dctDERemindersSubscribers.Add(strSubscriberKey, strStatus);
                            Console.WriteLine("Added RPL customer: " + strSubscriberKey);
                        }

                    }

                    grRowReminders = deRowGetReminders.GetMoreResults();
                    intResults = grRowReminders.Results.Length;
                }

                intUpdateRecordCount = 0;

                // Update statuses
                ET_DataExtensionRow deRowPatchReminders = null;
                foreach (KeyValuePair<string, string> entry in dctDERemindersSubscribers)
                {
                    strSubscriberKey = entry.Key;
                    strDEStatus = entry.Value;


                    // if our status = active, set to true
                    strStatus = dctRemindersListSubscribers[strSubscriberKey];
                    if (strStatus.Equals("Active") == true)
                    {
                        strStatus = "True";
                    }
                    else
                    {
                        strStatus = "False";
                    }

                    // Is the status of our subscriber the same on the All Subscribers list as it is on the Data Extension?
                    if (strDEStatus.Equals(strStatus) == false)
                    {
                        intUpdateRecordCount += 1;
                        // No, update the verified email field
                        deRowPatchReminders = new ET_DataExtensionRow();
                        deRowPatchReminders.AuthStub = m_etcTDClientShared;
                        deRowPatchReminders.DataExtensionCustomerKey = "CustomerDBKey";
                        deRowPatchReminders.ColumnValues.Add("SubscriberKey", strSubscriberKey);
                        deRowPatchReminders.ColumnValues.Add("RemindersPublicationList", strStatus);
                        PatchReturn patchRowResponse = deRowPatchReminders.Patch();
                        Console.WriteLine("Post Status: " + patchRowResponse.Status.ToString());
                        Console.WriteLine("Count: " + intUpdateRecordCount.ToString());
                        Console.WriteLine("Changed reminders boolean for subscriber: " + strSubscriberKey + " from " + strDEStatus + " to " + strStatus);
                    }
                }

                Console.WriteLine("Completed Reminders Boolean Update");
            }
            catch (Exception excError)
            {
                // Display Error
                Console.WriteLine("Error: " + excError.ToString());
            }
        }


        // ------------------------------------------------------------
        // Name: PrepareListForCSV
        // Abstract: Prepare a list to be used to create a CSV
        // ------------------------------------------------------------
        public static Dictionary<string, string> PrepareListForCSV(String strListID)
        {
            Dictionary<string, string> dctListSubscribers = new Dictionary<string, string>();

            try
            {
                String strStatus = "";

                // Newsletter Publication List
                // Get subscriber dates
                ET_List_Subscriber getListSub = new ET_List_Subscriber();
                getListSub.AuthStub = m_etcTDClient;
                getListSub.Props = new string[] { "SubscriberKey", "Status" };
                getListSub.SearchFilter = new SimpleFilterPart() { Property = "ListID", SimpleOperator = SimpleOperators.equals, Value = new string[] { strListID } };
                GetReturn getLSResponse = getListSub.Get();
                Console.WriteLine("Get Status: " + getLSResponse.Status.ToString());
                Console.WriteLine("Message: " + getLSResponse.Message.ToString());
                Console.WriteLine("Code: " + getLSResponse.Code.ToString());
                Console.WriteLine("Results Length: " + getLSResponse.Results.Length);
                int intResults = getLSResponse.Results.Length;

                // CREATE HEADERS: "Subscriber Key" & "Status"

                // Get all subscriber keys associated with active subscribers
                while (getLSResponse.MoreResults == true || intResults > 0)
                {
                    foreach (ET_List_Subscriber sub in getLSResponse.Results)
                    {
                        strStatus = sub.Status.ToString();
                        dctListSubscribers.Add(sub.SubscriberKey, strStatus);
                        Console.WriteLine("Added Subscriber: " + sub.SubscriberKey + " Status: " + strStatus);
                    }

                    getLSResponse = getListSub.GetMoreResults();
                    intResults = getLSResponse.Results.Length;
                }

                Console.WriteLine("Retrieved List Subscriber Statuses from ListID: " + strListID);

            }
            catch (Exception excError)
            {
                // Display Error
                Console.WriteLine("Error: " + excError.ToString());
            }

            return dctListSubscribers;
        }


        // ------------------------------------------------------------
        // Name: StartImport
        // Abstract: Start an import activity
        // ------------------------------------------------------------
        public static void StartImport(String strImportKey)
        {
            try
            {

                Console.WriteLine("\n Start Import To List");
                ET_Import startImport = new ET_Import();
                startImport.AuthStub = m_etcTDClientShared;
                startImport.CustomerKey = strImportKey;
                PerformReturn perListImport = startImport.Start();
                Console.WriteLine("Start Status: " + perListImport.Status.ToString());
                Console.WriteLine("Message: " + perListImport.Message.ToString());
                Console.WriteLine("Code: " + perListImport.Code.ToString());
                Console.WriteLine("Results Length: " + perListImport.Results.Length);

                if (perListImport.Status)
                {
                    Console.WriteLine("\n Check Status using the same instance of ET_Import as used for start");
                    string CurrentImportStatus = "";
                    while (CurrentImportStatus != "Error" && CurrentImportStatus != "Completed")
                    {
                        Console.WriteLine("Checking status in loop " + CurrentImportStatus);
                        //Wait a bit before checking the status to give it time to process
                        Thread.Sleep(15000);
                        GetReturn statusListImport = startImport.Status();
                        Console.WriteLine("Status Status: " + statusListImport.Status.ToString());
                        Console.WriteLine("Message: " + statusListImport.Message.ToString());
                        Console.WriteLine("Code: " + statusListImport.Code.ToString());
                        Console.WriteLine("Results Length: " + statusListImport.Results.Length);
                        CurrentImportStatus = ((ET_ImportResult)statusListImport.Results[0]).ImportStatus;
                    }
                    Console.WriteLine("Final Status: " + CurrentImportStatus);
                }
            }
            catch (Exception excError)
            {
                // Display Error
                Console.WriteLine("Error: " + excError.ToString());
            }
        }


        // ------------------------------------------------------------
        // Name: GetSubscriberEmails
        // Abstract: Get subscriber keys and emails
        // ------------------------------------------------------------
        public static Dictionary<string, string> GetSubscriberEmails()
        {
            Dictionary<string, string> dctAllSubscribers = new Dictionary<string, string>();

            try
            {
                ET_Subscriber getSub = new ET_Subscriber();
                getSub.AuthStub = m_etcTDClientShared;
                getSub.Props = new string[] { "SubscriberKey", "EmailAddress" };
                GetReturn getResponse = getSub.Get();

                Console.WriteLine("Get Status: " + getResponse.Status.ToString());
                Console.WriteLine("Message: " + getResponse.Message.ToString());
                Console.WriteLine("Code: " + getResponse.Code.ToString());
                Console.WriteLine("Results Length: " + getResponse.Results.Length);
                int intResults = getResponse.Results.Length;

                // Get all subscriber keys associated with active subscribers
                while (getResponse.MoreResults == true || intResults > 0)
                {
                    foreach (ET_Subscriber sub in getResponse.Results)
                    {
                        dctAllSubscribers.Add(sub.SubscriberKey, sub.EmailAddress);
                    }

                    getResponse = getSub.GetMoreResults();
                    intResults = getResponse.Results.Length;
                }
            }
            catch (Exception excError)
            {
                // Display Error
                Console.WriteLine("Error: " + excError.ToString());
            }

            return dctAllSubscribers;
        }
    }


    
}
