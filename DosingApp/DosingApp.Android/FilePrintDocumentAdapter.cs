using Android.App;
using Android.Content;
using Android.OS;
using Android.Print;
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
    public class FilePrintDocumentAdapter : PrintDocumentAdapter
    {
        private readonly string _fileName;
        private readonly string _filePath;

        public FilePrintDocumentAdapter(string fileName, string filePath)
        {
            _fileName = fileName;
            _filePath = filePath;
        }

        public override void OnLayout(PrintAttributes oldAttributes, PrintAttributes newAttributes, CancellationSignal cancellationSignal, LayoutResultCallback callback, Bundle extras)
        {
            if (cancellationSignal.IsCanceled)
            {
                callback.OnLayoutCancelled();
                return;
            }

            callback.OnLayoutFinished(new PrintDocumentInfo.Builder(_fileName)
                .SetContentType(PrintContentType.Document)
                .Build(), true);
        }

        public override void OnWrite(PageRange[] pages, ParcelFileDescriptor destination, CancellationSignal cancellationSignal, WriteResultCallback callback)
        {
            try
            {
                using (InputStream input = new FileInputStream(_filePath))
                {
                    using (OutputStream output = new FileOutputStream(destination.FileDescriptor))
                    {
                        var buf = new byte[1024];
                        int bytesRead;

                        while ((bytesRead = input.Read(buf)) > 0)
                        {
                            output.Write(buf, 0, bytesRead);
                        }
                    }
                }

                callback.OnWriteFinished(new[] { PageRange.AllPages });

            }
            catch (FileNotFoundException fileNotFoundException)
            {
                System.Diagnostics.Debug.WriteLine(fileNotFoundException);
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception);
            }
        }
    }
}