﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

/// <author>Ryan Haire</author>
/// <summary>
/// Code Behind File for Patient.aspx
/// </summary>

namespace Caregiver.Web_Pages {
    public partial class Patient : System.Web.UI.Page {

        //private Classes.Patient patient;

        /// <summary>
        /// On page load, the form values are filled with the patients data from the database
        /// </summary>
        protected void Page_Load(object sender, EventArgs e) {        
            if (!IsPostBack) {
                //if (!(bool)Session["IsRegisteredUser"]) {
                //    Server.Transfer("Login.aspx");
                //}

                SetPatient();
            }

            SetEnabled(false);
            btnEdit.Style.Add("display", "inline");
            btnSave.Style.Add("display", "none");
        }

        private void SetPatient() {
            Classes.Patient patient = (Classes.Patient) Session["SelectedPatient"];

            tbFirstName.Text = patient.FirstName;
            tbLastName.Text = patient.LastName;
            if (patient.Sex == 'M') {
                rdbSex.SelectedIndex = 0;
                imgUser.ImageUrl = "/Images/Male.png";
            } else {
                rdbSex.SelectedIndex = 1;
                imgUser.ImageUrl = "/Images/Female.png";
            }

            tbDob.Text = patient.Dob;
            tbAddress.Text = patient.Address;
            tbCity.Text = patient.City;

            for (int i = 0; i < ddlProvince.Items.Count; i++) {
                if (ddlProvince.Items[i].Text == patient.Province) {
                    ddlProvince.SelectedIndex = i;
                }
            }

            tbPostalCode.Text = patient.PostalCode;
            tbPhoneNum.Text = patient.PhoneNum;        

            for (int i = 0; i < cblHistory.Items.Count; i++) {
                for (int j = 0; j < patient.History.Count; j++) {
                    if (cblHistory.Items[i].Text == patient.History[j]) {
                        cblHistory.Items[i].Selected = true;
                    }
                }
                
            }

            for (int i = 0; i < cblSymptom.Items.Count; i++) {
                for (int j = 0; j < patient.Symptoms.Count; j++) {
                    if (cblSymptom.Items[i].Text == patient.Symptoms[j]) {
                        cblSymptom.Items[i].Selected = true;
                    }
                }
            }

        }

        /// <author>Stefano</author>
        /// <summary>
        /// 
        /// </summary>
        private void SetEnabled(bool value) {
            tbFirstName.Enabled = value;
            tbLastName.Enabled = value;
            rdbSex.Enabled = value;
            tbDob.Enabled = value;
            cblHistory.Enabled = value;
            tbAddress.Enabled = value;
            tbCity.Enabled = value;
            ddlProvince.Enabled = value;
            tbPostalCode.Enabled = value;
            tbPhoneNum.Enabled = value;
            cblSymptom.Enabled = value;
        }

        /// <author>Stefano</author>
        /// <summary>
        /// 
        /// </summary>
        protected void lbReturn_Click(object sender, EventArgs e) {
            Server.Transfer("ViewAllPatients.aspx");
        }

        /// <summary>
        /// This function is for calculating the diagnosis of the patient by checking what symptoms
        /// and history they have compared to the disease that contains the respective symptoms and diseases
        /// Then outputs the diagnosis
        /// </summary>
        protected void btn_Diagnose(object sender, EventArgs e) {
            string result = "";
            Classes.Patient patient = (Classes.Patient)Session["SelectedPatient"];

            int coronaryArteryDiseaseChance = patient.CalculateCoronaryArteryChance();
            int strokeChance = patient.CalculateStrokeChance();
            int fluChance = patient.CalculateFluChance();
            int kidneyDiseaseChance = patient.CalculateKidneyDiseaseChance();


            //**********************************************************************to be deleted after
            lbl1.Style.Add("display", "inline");
            lbl2.Style.Add("display", "inline");
            lbl3.Style.Add("display", "inline");
            lbl4.Style.Add("display", "inline");
            lblDiagnosis.Style.Add("display", "inline");

            if (cblSymptom.SelectedIndex == -1) {
                result = "No diagnosis.";
                lbl1.Text = "";
                lbl2.Text = "";
                lbl3.Text = "";
                lbl4.Text = "";
            } else {

                // check if all chances are equal
                if (coronaryArteryDiseaseChance == strokeChance && coronaryArteryDiseaseChance == fluChance && coronaryArteryDiseaseChance == kidneyDiseaseChance) {
                    result = "Probable chance of Flu(Influenza)";
                } // check if coronaryArteryDiseaseChance is greater than all the others 

                // if one chance is greater than the others
                if (coronaryArteryDiseaseChance > strokeChance && coronaryArteryDiseaseChance > fluChance && coronaryArteryDiseaseChance > kidneyDiseaseChance) {
                    result = "Probable chance of Coronary Artery Disease";
                }// check if strokeChance is greater than all the others  
                else if (strokeChance > coronaryArteryDiseaseChance && strokeChance > fluChance && strokeChance > kidneyDiseaseChance) {
                    result = "Probable chance of Stroke";
                }// check if fluChance is greater than all the others  
                else if (fluChance > coronaryArteryDiseaseChance && fluChance > strokeChance && fluChance > kidneyDiseaseChance) {
                    result = "Probable chance of Flu(Influenza)";
                } // check if kidneyDiseaseChance is greater than all the others 
                else if (kidneyDiseaseChance > coronaryArteryDiseaseChance && kidneyDiseaseChance > fluChance && kidneyDiseaseChance > strokeChance) {
                    result = "Probable chance of Kidney Disease";
                }

                // check if any 2 are equal and greater than the other 2 -- if
                if (coronaryArteryDiseaseChance == strokeChance && strokeChance > fluChance && strokeChance > kidneyDiseaseChance) {
                    result = "Probable chance of Stroke";
                } else if (coronaryArteryDiseaseChance == fluChance && fluChance > strokeChance && fluChance > kidneyDiseaseChance) {
                    result = "Probable chance of Flu(Influenza)";
                } else if (coronaryArteryDiseaseChance == kidneyDiseaseChance && kidneyDiseaseChance > fluChance && kidneyDiseaseChance > strokeChance) {
                    result = "Probable chance of Kidney Disease";
                } else if (strokeChance == fluChance && fluChance > coronaryArteryDiseaseChance && fluChance > kidneyDiseaseChance) {
                    result = "Probable chance of Flu(Influenza)";
                } else if (strokeChance == kidneyDiseaseChance && strokeChance > coronaryArteryDiseaseChance && strokeChance > fluChance) {
                    result = "Probable chance of stroke";
                } else if (fluChance == kidneyDiseaseChance && fluChance > coronaryArteryDiseaseChance && fluChance > strokeChance) {
                    result = "Probable chance of Flu(Influenza)";
                }

                // check if any 3 are equal && greater than the other one -- if 
                if (coronaryArteryDiseaseChance == strokeChance && coronaryArteryDiseaseChance == fluChance && fluChance > kidneyDiseaseChance) {
                    result = "Probable chance of Flu(Influenza)";
                } else if (coronaryArteryDiseaseChance == strokeChance && coronaryArteryDiseaseChance == kidneyDiseaseChance && strokeChance > fluChance) {
                    result = "Probable chance of Stroke";
                } else if (coronaryArteryDiseaseChance == fluChance && coronaryArteryDiseaseChance == kidneyDiseaseChance && fluChance > strokeChance) {
                    result = "Probable chance of Flu(Influenza)";
                } else if (strokeChance == fluChance && strokeChance == kidneyDiseaseChance && strokeChance > coronaryArteryDiseaseChance) {
                    result = "Probable chance of Flu(Influenza)";
                }

                lbl1.Text = "coronaryArteryDiseaseChance = " + coronaryArteryDiseaseChance.ToString();
                lbl2.Text = "strokeChance = " + strokeChance.ToString();
                lbl3.Text = "fluChance = " + fluChance.ToString();
                lbl4.Text = "kidneyDiseaseChance = " + kidneyDiseaseChance.ToString();
            }

            lblDiagnosis.Text = result;
        }

        protected void btnEdit_Click(object sender, EventArgs e) {
            SetEnabled(true);
            btnEdit.Style.Add("display", "none");
            btnSave.Style.Add("display", "inline");
            btnDiagnose.Style.Add("display", "none");

            //**********************************************************************to be deleted after
            lbl1.Style.Add("display", "none");
            lbl2.Style.Add("display", "none");
            lbl3.Style.Add("display", "none");
            lbl4.Style.Add("display", "none");
            lblDiagnosis.Style.Add("display", "none");
        }

        protected void btnSave_Click(object sender, EventArgs e) {
            btnEdit.Style.Add("display", "inline");
            btnSave.Style.Add("display", "none");
            btnDiagnose.Style.Add("display", "inline");

            //**********************************************************************to be deleted after
            lbl1.Style.Add("display", "none");
            lbl2.Style.Add("display", "none");
            lbl3.Style.Add("display", "none");
            lbl4.Style.Add("display", "none");
            lblDiagnosis.Style.Add("display", "none");

            using (SqlConnection conn = new SqlConnection()) {
                conn.ConnectionString = "server=(local);database=Caregiver;Integrated Security=SSPI";
                try {
                    using (SqlCommand cmd = new SqlCommand()) {
                        conn.Open();
                        cmd.Connection = conn;

                        Classes.Patient patient = (Classes.Patient)Session["SelectedPatient"];

                        cmd.CommandText = "DELETE FROM PatientHistory WHERE PatientId=@id;"
                                        + "DELETE FROM PatientSymptom WHERE PatientId=@id";
                        cmd.Parameters.AddWithValue("@id", patient.Id);
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "UPDATE Patient SET FirstName=@FirstName,LastName=@LastName,Sex=@Sex,Birthday=@Birthday," +
                                          " Address=@Address,City=@City,Province=@Province,PostalCode=@PostalCode,PhoneNum=@PhoneNum" +
                                          " WHERE PatientId = @id2;";
                        cmd.Parameters.AddWithValue("@FirstName", tbFirstName.Text);
                        cmd.Parameters.AddWithValue("@LastName", tbLastName.Text);
                        cmd.Parameters.AddWithValue("@Sex", rdbSex.SelectedValue);
                        cmd.Parameters.AddWithValue("@Birthday", Convert.ToDateTime(tbDob.Text));
                        cmd.Parameters.AddWithValue("@Address", tbAddress.Text);
                        cmd.Parameters.AddWithValue("@City", tbCity.Text);
                        cmd.Parameters.AddWithValue("@Province", ddlProvince.SelectedItem.Value);
                        cmd.Parameters.AddWithValue("@PostalCode", tbPostalCode.Text.ToUpper());
                        cmd.Parameters.AddWithValue("@PhoneNum", tbPhoneNum.Text);
                        cmd.Parameters.AddWithValue("@id2", patient.Id);
                        cmd.ExecuteNonQuery();

                        patient.FirstName = tbFirstName.Text;
                        patient.LastName = tbLastName.Text;
                        patient.Sex = rdbSex.SelectedValue[0];
                        patient.Dob = tbDob.Text;
                        patient.Address = tbAddress.Text;
                        patient.City = tbCity.Text;
                        patient.Province = ddlProvince.SelectedItem.Value;
                        patient.PostalCode = tbPostalCode.Text.ToUpper();
                        patient.PhoneNum = tbPhoneNum.Text;

                        List<string> history = new List<string>();
                        patient.History.Clear();
                        foreach (ListItem item in cblHistory.Items) {
                            if (item.Selected) {
                                history.Add(item.Value);
                                patient.History.Add(item.Text);
                            }
                        }

                        List<string> symptoms = new List<string>();
                        patient.Symptoms.Clear();
                        foreach (ListItem item in cblSymptom.Items) {
                            if (item.Selected) {
                                symptoms.Add(item.Value);
                                patient.Symptoms.Add(item.Text);
                            }
                        }

                        foreach (string item in history) {
                            cmd.CommandText = "INSERT INTO PatientHistory VALUES(@PatientID,@HistoryId)";
                            cmd.Parameters.AddWithValue("@PatientId", patient.Id);
                            cmd.Parameters.AddWithValue("@HistoryId", item);
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }

                        foreach (string item in symptoms) {
                            cmd.CommandText = "INSERT INTO PatientSymptom VALUES(@PatientID,@SymptomId)";
                            cmd.Parameters.AddWithValue("@PatientId", patient.Id);
                            cmd.Parameters.AddWithValue("@SymptomId", item);
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }

                        lblUpdateResult.Text = "Update successfull!";
                        Session["SelectedPatient"] = patient;
                        conn.Close();
                    }
                } catch (SqlException ex) {
                    lblUpdateResult.Text = ex.Message;
                }
            }
        }
    }
}