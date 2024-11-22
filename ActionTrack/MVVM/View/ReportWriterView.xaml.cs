using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Syncfusion.Pdf;
using Syncfusion.Windows.PdfViewer;
using Syncfusion.SfSkinManager;

namespace ActionTrack.MVVM.View
{
    /// <summary>
    /// Interaction logic for ReportWriter.xaml
    /// </summary>
    public partial class ReportWriterView : UserControl
    {
        
        public ReportWriterView()
        {
            SfSkinManager.SetTheme(this, new Theme("MaterialDark"));
            InitializeComponent();
            
            CreateNewReport();
        }

        private void CreateNewReport()
        {
            // Get the path to the Application Data folder (AppData\Local or AppData\Roaming)
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Define the path to your 'ActionTrack' folder
            string actionTrackFolderPath = System.IO.Path.Combine(appDataPath, "ActionTrack");

            // Check if the folder exists, if not, create it
            if (!Directory.Exists(actionTrackFolderPath))
            {
                Directory.CreateDirectory(actionTrackFolderPath);
            }

            // Define the template file path (ensure this is the correct file name)
            string templatePath = System.IO.Path.Combine(actionTrackFolderPath, "report_template.pdf");

            // Check if the template file exists
            if (!File.Exists(templatePath))
            {
                MessageBox.Show("Template file not found in ActionTrack folder.");
                return;
            }

            // Generate a new report file path
            string newReportPath = System.IO.Path.Combine(actionTrackFolderPath, $"report_{Guid.NewGuid()}.pdf");

            // Copy the template PDF to the new report path
            File.Copy(templatePath, newReportPath);

            // Now load the new report
            LoadPdfIntoViewer(newReportPath);
        }

        private void LoadPdfIntoViewer(string reportPath)
        {
            // Check if pdfViewer is initialized and then load the PDF
            if (pdfViewer != null)
            {
                
                pdfViewer.Load(reportPath);
            }
            else
            {
                MessageBox.Show("PDF Viewer not initialized.");
            }
        }

        // Optionally, if you want to save the edited PDF
        private void SaveEditedPdf(string filePath)
        {
            // Save the edited PDF to the specified path
            if (pdfViewer != null)
            {
                pdfViewer.Save(filePath);
            }
            else
            {
                MessageBox.Show("PDF Viewer not initialized.");
            }
        }


    }
}
