
using System;
using System.IO;
using System.Windows.Forms;

namespace LUCKY_COFFEE_ORDER
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        Random rand = new Random();

        int customerCount = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void btncalculate_Click(object sender, EventArgs e)
        {
            string customername = txtname.Text;

            if (customername.Trim() == "")
            {
                MessageBox.Show("Please enter customer name.");
                return;
            }

            int quantity;

            if (!int.TryParse(txtquantity.Text, out quantity) || quantity < 1)
            {
                MessageBox.Show("Please enter a valid quantity.");
                return;
            }

            double price = 0;
            string drink = "";

            if (rdotea.Checked)
            {
                price = 1;
                drink = "Tea";
            }
            else if (rdocoffee.Checked)
            {
                price = 2;
                drink = "Coffee";
            }
            else if (rdojuice.Checked)
            {
                price = 3;
                drink = "Juice";
            }
            else
            {
                MessageBox.Show("Please select a drink.");
                return;
            }

            double total = price * quantity;
            double discount = 0;

            if (chkstudent.Checked)
            {
                discount = total * 0.10;
            }

            double finaltotal = total - discount;

            bool wonPrize = false;

            customerCount++;

            OrderList.Items.Add("========== ORDER " + customerCount + " ==========");

            OrderList.Items.Add(customerCount + ") " + customername + " - " + drink);

            OrderList.Items.Add("Customer Name: " + customername);

            OrderList.Items.Add("Drink: " + drink + " ($" + price.ToString("0.00") + ")");

            OrderList.Items.Add("Quantity: " + quantity);

            if (chkstudent.Checked)
            {
                OrderList.Items.Add("Member Discount: 10%");
            }
            else
            {
                OrderList.Items.Add("Member Discount: No");
            }

            OrderList.Items.Add("Total Before Discount: $" + total.ToString("0.00"));

            OrderList.Items.Add("Total Discount: $" + discount.ToString("0.00"));

            OrderList.Items.Add("Final Total: $" + finaltotal.ToString("0.00"));

            OrderList.Items.Add("Lucky Numbers:");

            for (int i = 1; i <= quantity; i++)
            {
                int luckynumber = rand.Next(1, 51);

                OrderList.Items.Add(i + ") Lucky Number: " + luckynumber);

                if (luckynumber == 7 || luckynumber == 17)
                {
                    wonPrize = true;
                }
            }

            if (wonPrize)
            {
                OrderList.Items.Add("Lucky Message: You got a Free Cookie!");
            }
            else
            {
                OrderList.Items.Add("Lucky Message: No cookies this time.");
            }

            OrderList.Items.Add("----------------------");

            lbltotal.Text = "Total Price: $" + finaltotal.ToString("0.00");

            if (wonPrize)
            {
                lbllucky.Text = "You got a Free Cookie!";
            }
            else
            {
                lbllucky.Text = "No cookies this time.";
            }

            MessageBox.Show("Order added successfully!");
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                if (OrderList.Items.Count == 0)
                {
                    MessageBox.Show("No order to save.");
                    return;
                }

                using (StreamWriter writer = new StreamWriter("orders.txt", false))
                {
                    for (int i = 0; i < OrderList.Items.Count; i++)
                    {
                        writer.WriteLine(OrderList.Items[i].ToString());
                    }
                }

                MessageBox.Show("All orders saved successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnload_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists("orders.txt"))
                {
                    MessageBox.Show("No file found.");
                    return;
                }

                OrderList.Items.Clear();

                customerCount = 0;

                using (StreamReader reader = File.OpenText("orders.txt"))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();

                        OrderList.Items.Add(line);

                        if (line.StartsWith("= ORDER = "))
                        {
                            customerCount++;
                        }
                    }
                }

                MessageBox.Show("All orders loaded successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            txtname.Clear();
            txtquantity.Clear();

            OrderList.Items.Clear();

            lbltotal.Text = "Total Price: $0.00";

            lbllucky.Text = "Lucky Message:";

            chkstudent.Checked = false;

            rdotea.Checked = false;
            rdocoffee.Checked = false;
            rdojuice.Checked = false;

            customerCount = 0;

            txtname.Focus();
        }
    }
}
