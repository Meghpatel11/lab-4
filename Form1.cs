using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab_4
{
    public partial class Form1 : Form
    {
        private List<Customer> customerList = new List<Customer>();
        // This flag is used to indicate whether the program is checking boxes as opposed to a human.
        private bool isAutomated = false;
        // Variable representing the current selected index in the ListView.
        // This is being used to simplify a few reference to indexes.
        private int selectedIndex = -1;

        public Form1()
        {
            InitializeComponent();
        }

        #region avoid

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #endregion


        #region events
        private void buttonEnter_Click(object sender, EventArgs e)
        {
            // Empty the error label; it will fill with NEW errors if anything is wrong.
            textBoxMessage.Text = String.Empty;

            // Check if the customer is valid.
            if (IsCustomerValid(textBoxMail.Text, textBoxFirst.Text, textBoxLast.Text, textBoxPhone.Text))
            {
                // Customer details are valid; create a customer object.
                Customer newCustomerToAdd = new Customer(textBoxMail.Text, textBoxFirst.Text, textBoxLast.Text, checkBox1.Checked ,textBoxPhone.Text);

                // If a customer is currently selected...
                if (selectedIndex >= 0)
                {
                    // Replace the old version of that customer with the new one!
                    customerList[selectedIndex] = newCustomerToAdd;
                }
                else
                {
                    // Otherwise, add a customer with the entered details to the end of the list.
                    customerList.Add(newCustomerToAdd);
                }

                // Refresh the entire listView.
                PopulateListView(customerList);

                // Reset the form to allow new entries.
                SetDefaults();
            }

        }

        /// <summary>
        /// Reset the form to its default state.
        /// </summary>
        private void buttonReset_Click(object sender, EventArgs e)
        {
            SetDefaults();
        }
       

        /// <summary>
        /// Me close form.
        /// </summary>
        

        private void buttonExit_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// When a customer in the ListView is selected, write that customer's properties into the input controls.
        /// </summary>
        private void CustomerSelected(object sender, EventArgs e)
        {
            // If the list is populated and something is selected...
            if (listView1.Items.Count > 0 && listView1.FocusedItem != null)
            {
                // ...fill in the entry fields with values based on the selected customer.
                
                checkBox1.Checked = listView1.FocusedItem.Checked;
                textBoxFirst.Text = listView1.FocusedItem.SubItems[1].Text;
                textBoxLast.Text = listView1.FocusedItem.SubItems[2].Text;
                textBoxMail.Text = listView1.FocusedItem.SubItems[4].Text;

                // Set the selectedIndex to match the listView.
                selectedIndex = listView1.FocusedItem.Index;
            }
            else
            {
                // If nothing is selected, set the selectedIndex to -1.
                selectedIndex = -1;
            }
        }

        /// <summary>
        /// When a checkbox in the ListView is checked, say no and don't let the user change it.
        /// </summary>
        private void PreventCheck(object sender, ItemCheckEventArgs e)
        {
            // Only prevent checking if it's being done by the user.
            if (!isAutomated)
            {
                // Change the new value of the checkbox equal to the old state of the checkbox.
                e.NewValue = e.CurrentValue;
            }
        }
        #endregion

        #region functions
        /// <summary>
        /// Converts the customer passed in to a ListViewItem and adds it to listViewEntries
        /// </summary>
        /// <param name="newCustomer"></param>
        private void PopulateListView(List<Customer> customerList)
        {
            // Clear the listView to start re-populating it.
            textBoxMessage.Clear();

            foreach (Customer newCustomer in customerList)
            {
                // Declare and instantiate a new ListViewItem.
                ListViewItem customerItem = new ListViewItem();

                // Allow the program to modify the ListView's checkboxes.
                isAutomated = true;

                customerItem.Checked = newCustomer.Checked;
                //customerItem.SubItems.Add(newCustomer.Checked);
                customerItem.SubItems.Add(newCustomer.FirstName);
                customerItem.SubItems.Add(newCustomer.LastName);
                customerItem.SubItems.Add(newCustomer.PhoneNumber);
                customerItem.SubItems.Add(newCustomer.Email);

                // Add the customerItem to the ListView.
                listView1.Items.Add(customerItem);

                // Disallow the user from modifying the ListView's checkboxes.
                isAutomated = false;
            }
        }

        /// <summary>
        /// Reset the form's input fields to their default states.
        /// </summary>
        private void SetDefaults()
        {
            // Resets fields to default state.
            //textBoxFirst.SelectedText = -1;

            textBoxMail.Clear();
            textBoxFirst.Clear();
            textBoxLast.Clear();
            textBoxMessage.Clear();
            textBoxPhone.Clear();
            checkBox1.Checked = false;
        
            listView1.SelectedItems.Clear();
            selectedIndex = -1;

            // Set a default focus.
            textBoxFirst.Focus();
        }

        /// <summary>
        /// Checks whether the input related to a customer is valid
        /// </summary>
        /// <param name="title">The customer's title as a string</param>
        /// <param name="firstName">The customer's first name as a string</param>
        /// <param name="lastName">The customer's last name as a string</param>
        /// <returns></returns>
        private bool IsCustomerValid(string email, string firstName, string lastName,string number)
        {
            // Assume the worker is valid.
            bool isValid = true;

            // Check the first input.
            if (email == String.Empty )
            {
                // If it's not valid, set isValid = false and write an error message.
                isValid &= false;
                textBoxMessage.Text += "You must enter proper email.\n";
            }
            // Check the second input.
            if (firstName == String.Empty)
            {
                // If it's not valid, set isValid = false and write an error message.
                isValid &= false;
                textBoxMessage.Text += "You must enter a first name.\n";
            }
            // Check the third input.
            if (lastName == String.Empty)
            {
                // If it's not valid, set isValid = false and write an error message.
                isValid &= false;
                textBoxMessage.Text += "You must enter a last name.\n";
            }

            if (number == String.Empty)
            {
                // If it's not valid, set isValid = false and write an error message.
                isValid &= false;
                textBoxMessage.Text += "You must enter a phone number.\n";
            }


            return isValid;
        }



        #endregion

       
    }
}
