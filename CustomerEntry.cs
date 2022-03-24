// Author:          Kyle Chapman
// Created:         March, 2021
// Last Modified:   March 15, 2022
// Description:
//  Demo program for NETD 2202 based loosely off of Class Exercise 4
//  in 2021 and modified for use in 2022.
//  Meant as an aid for Lab 4. Using an existing Customer class, this
//  Windows form can display a list of customers and allow the user
//  to add new customers to the list as well as edit existing customers
//  selected from a ListView.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CustomerEntry
{
    public partial class formCustomerEntry : Form
    {
        private List<Customer> customerList = new List<Customer>();
        // This flag is used to indicate whether the program is checking boxes as opposed to a human.
        private bool isAutomated = false;
        // Variable representing the current selected index in the ListView.
        // This is being used to simplify a few reference to indexes.
        private int selectedIndex = -1;

        public formCustomerEntry()
        {
            InitializeComponent();
        }

        #region "Event Handlers"

        /// <summary>
        /// When the form loads, instantiate some customers and add them to a list so they can be viewed later.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormLoad(object sender, EventArgs e)
        {
            // Declare and instantiate some example new customer objects.
            Customer kyle = new Customer("Mr.", "Kyle", "Chapman", true);
            Customer demoCustomer = new Customer("Ms.", "Francesca", "Lethbridge", true);
            Customer everyoneIsImportant = new Customer("Lady", "J", "Joe", false);
            
            // Add all of the new customer objects from above to the list.
            customerList.Add(kyle);
            customerList.Add(demoCustomer);
            customerList.Add(everyoneIsImportant);
            
            PopulateListView(customerList);
        }

        /// <summary>
        /// Validate all input fields, and if they are valid create the customer object, add the entered customer to the list, and add them to the ListView.
        /// </summary>
        private void ButtonEnterClick(object sender, EventArgs e)
        {
            // Empty the error label; it will fill with NEW errors if anything is wrong.
            labelError.Text = String.Empty;

            // Check if the customer is valid.
            if (IsCustomerValid(comboBoxTitle.Text, textBoxFirstName.Text, textBoxLastName.Text))
            {
                // Customer details are valid; create a customer object.
                Customer newCustomerToAdd = new Customer(comboBoxTitle.Text, textBoxFirstName.Text, textBoxLastName.Text, checkBoxVip.Checked);

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
        private void ButtonResetClick(object sender, EventArgs e)
        {
            SetDefaults();
        }

        /// <summary>
        /// Me close form.
        /// </summary>
        private void ButtonExitClick(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// When a customer in the ListView is selected, write that customer's properties into the input controls.
        /// </summary>
        private void CustomerSelected(object sender, EventArgs e)
        {
            // If the list is populated and something is selected...
            if (listViewEntries.Items.Count > 0 && listViewEntries.FocusedItem != null)
            {
                // ...fill in the entry fields with values based on the selected customer.
                comboBoxTitle.Text = listViewEntries.FocusedItem.SubItems[1].Text;
                textBoxFirstName.Text = listViewEntries.FocusedItem.SubItems[2].Text;
                textBoxLastName.Text = listViewEntries.FocusedItem.SubItems[3].Text;
                checkBoxVip.Checked = listViewEntries.FocusedItem.Checked;

                // Set the selectedIndex to match the listView.
                selectedIndex = listViewEntries.FocusedItem.Index;
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
        #region "Functions"

        /// <summary>
        /// Converts the customer passed in to a ListViewItem and adds it to listViewEntries
        /// </summary>
        /// <param name="newCustomer"></param>
        private void PopulateListView(List<Customer> customerList)
        {
            // Clear the listView to start re-populating it.
            listViewEntries.Items.Clear();

            foreach (Customer newCustomer in customerList)
            {
                // Declare and instantiate a new ListViewItem.
                ListViewItem customerItem = new ListViewItem();

                // Allow the program to modify the ListView's checkboxes.
                isAutomated = true;
                customerItem.Checked = newCustomer.VipStatus;

                customerItem.SubItems.Add(newCustomer.Title);
                customerItem.SubItems.Add(newCustomer.FirstName);
                customerItem.SubItems.Add(newCustomer.LastName);

                // Add the customerItem to the ListView.
                listViewEntries.Items.Add(customerItem);

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
            comboBoxTitle.SelectedIndex = -1;
            textBoxFirstName.Clear();
            textBoxLastName.Clear();
            checkBoxVip.Checked = false;
            listViewEntries.SelectedItems.Clear();
            labelError.Text = String.Empty;
            selectedIndex = -1;

            // Set a default focus.
            comboBoxTitle.Focus();
        }

        /// <summary>
        /// Checks whether the input related to a customer is valid
        /// </summary>
        /// <param name="title">The customer's title as a string</param>
        /// <param name="firstName">The customer's first name as a string</param>
        /// <param name="lastName">The customer's last name as a string</param>
        /// <returns></returns>
        private bool IsCustomerValid(string title, string firstName, string lastName)
        {
            // Assume the worker is valid.
            bool isValid = true;

            // Check the first input.
            if (title == String.Empty)
            {
                // If it's not valid, set isValid = false and write an error message.
                isValid &= false;
                labelError.Text += "You must select a title.\n";
            }
            // Check the second input.
            if (firstName == String.Empty)
            {
                // If it's not valid, set isValid = false and write an error message.
                isValid &= false;
                labelError.Text += "You must enter a first name.\n";
            }
            // Check the third input.
            if (lastName == String.Empty)
            {
                // If it's not valid, set isValid = false and write an error message.
                isValid &= false;
                labelError.Text += "You must enter a last name.";
            }

            return isValid;
        }

        #endregion

    }
}
