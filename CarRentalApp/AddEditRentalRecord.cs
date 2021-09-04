using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class AddEditRentalRecord : Form
    {
        private bool isEditMode;
        private readonly CarRentalEntities carRentalEntities;
        public AddEditRentalRecord()
        {
            InitializeComponent();
            lblTitle.Text = "Add New Rental";
            this.Text = "Add New Rental";
            isEditMode = false;
            carRentalEntities = new CarRentalEntities();
        }
        public AddEditRentalRecord(CarRentalRecord recordToEdit)
        {
            InitializeComponent();
            lblTitle.Text = "Edit Rental Record";
            this.Text = "Edit Rental Record";
            if (recordToEdit == null)
            {
                MessageBox.Show("Please ensure that you selected a valid record to edit");
                Close();
            }
            else
            {
                isEditMode = true;
                carRentalEntities = new CarRentalEntities();
                PopulateFields(recordToEdit);
            }

        }

        private void PopulateFields(CarRentalRecord recordToEdit)
        {
            CustomerName.Text = recordToEdit.CustomerName;
            DateRented.Value = (DateTime)recordToEdit.DateRented;
            DateReturned.Value=(DateTime)recordToEdit.DateReturned;
            Cost.Text= recordToEdit.Cost.ToString();
            lblRecordId.Text = recordToEdit.id.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //This is a LINQ Query to fetch data from the database
            //var cars = carRentalEntities.TypesOfCars.ToList();

            var cars = carRentalEntities.TypesOfCars.Select
                (q => new {Id = q.id, Name = q.Make + " " + q.Model }).ToList();
            TypeofCar.DisplayMember = "Name";
            TypeofCar.ValueMember = "Id";
            TypeofCar.DataSource = cars;
        }

        private void submit_Click(object sender, EventArgs e)
        {
            try
            {
                string cname = CustomerName.Text;
                var daterented = DateRented.Value;
                var datereturned = DateReturned.Value;
                double cost = Convert.ToDouble(Cost.Text);

                var cartype = TypeofCar.Text;
                var isValid = true;
                var errorMessage = "";

                if (string.IsNullOrWhiteSpace(cname) || string.IsNullOrWhiteSpace(cartype))
                {
                    isValid = false;
                    errorMessage += "Error: Please Enter missing data.\n\r";
                }
                if (daterented > datereturned)
                {
                    isValid = false;
                    errorMessage += "Error: Illegal Date Selection\n\r";
                }
                if (isValid)
                {
                    //Declare and object of the record to be added
                    var rentalRecord = new CarRentalRecord();
                    if (isEditMode)
                    {
                        //If it is in Edit mode, then get the ID and retrieve the record from the database and place
                        // the result in the record object
                        var id = int.Parse(lblRecordId.Text);
                        rentalRecord = carRentalEntities.CarRentalRecords.FirstOrDefault(q => q.id == id);
                    }
                    //Populate the record object with value from the form
                        rentalRecord.CustomerName = cname;
                        rentalRecord.DateRented = daterented;
                        rentalRecord.DateReturned = datereturned;
                        rentalRecord.Cost = (decimal)cost;
                        //int against the selected value of combo box on basis of id
                        rentalRecord.TypeOfCarId = (int)TypeofCar.SelectedValue;
                   //If not in edit mode, then add the record object to the database
                    if(!isEditMode)
                    {
                        carRentalEntities.CarRentalRecords.Add(rentalRecord); 
                    }
                    //Save changes made to the entity
                    carRentalEntities.SaveChanges();

                   MessageBox.Show($"Thankyou for Renting {cname}\n\r" +
                       $"the car {cartype}\n\r" +
                       $"from {daterented}\n\r" +
                       $"till {datereturned}\n\r" +
                       $"Cost {cost}\n\r" +
                       $"Thankyou! for your Booking");

                    Close();
                 }
                else
                {
                    MessageBox.Show(errorMessage);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
