using Android.App;
using Android.Content;
using Android.OS;
using Android.Print;
using Android.Print.Pdf;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DosingApp.Droid
{
    public class CustomPrintDocumentAdapter : PrintDocumentAdapter
    {
        private string filePath;

        public CustomPrintDocumentAdapter(string filePath)
        {
            this.filePath = filePath;
        }

        public override void OnLayout(PrintAttributes oldAttributes, PrintAttributes newAttributes, CancellationSignal cancellationSignal, LayoutResultCallback callback, Bundle extras)
        {
            if (cancellationSignal.IsCanceled)
            {
                callback.OnLayoutCancelled();
                return;
            }

            PrintDocumentInfo pdi = new PrintDocumentInfo.Builder(filePath).SetContentType(PrintContentType.Document).Build();

            callback.OnLayoutFinished(pdi, true);
        }

        public override void OnWrite(PageRange[] pages, ParcelFileDescriptor destination, CancellationSignal cancellationSignal, WriteResultCallback callback)
        {
            InputStream input = null;
            OutputStream output = null;

            try
            {
                //Create FileInputStream object from the given file
                input = new FileInputStream(filePath);
                //Create FileOutputStream object from the destination FileDescriptor instance
                output = new FileOutputStream(destination.FileDescriptor);

                byte[] buf = new byte[1024];
                int bytesRead;

                while ((bytesRead = input.Read(buf)) > 0)
                {
                    //Write the contents of the given file to the print destination
                    output.Write(buf, 0, bytesRead);
                }

                callback.OnWriteFinished(new PageRange[] { PageRange.AllPages });

            }
            catch (FileNotFoundException ex)
            {
                //Catch exception
                System.Diagnostics.Debug.WriteLine(ex);
            }
            catch (Exception e)
            {
                //Catch exception
                System.Diagnostics.Debug.WriteLine(e);
            }
            finally
            {
                try
                {
                    input.Close();
                    output.Close();
                }
                catch (IOException e)
                {
                    e.PrintStackTrace();
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }
        }
    }
}