using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DosingApp.Droid
{
    public class PermissonManager
    {
        public enum PermissionsIdentifier
        {
            Storage // Here you can add more identifiers.
        }

        private static string[] GetPermissionsRequired(PermissionsIdentifier identifier)
        {
            string[] permissions = null;
            if (identifier == PermissionsIdentifier.Storage)
                permissions = PermissionExternalStorage;
            return permissions;
        }

        private static Int32 GetRequestId(PermissionsIdentifier identifier)
        {
            Int32 requestId = -1;
            if (identifier == PermissionsIdentifier.Storage)
                requestId = ExternalStorageRequestId;
            return requestId;
        }

        public static bool PermissionTCS;
        public static readonly String[] PermissionExternalStorage = new String[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage };
        public const Int32 ExternalStorageRequestId = 2;

        public static Boolean GetPermission(PermissionsIdentifier identifier)
        {
            Boolean isPermitted = false;
            if ((Int32)Build.VERSION.SdkInt < 23)
                isPermitted = true;
            else
                isPermitted = GetPermissionOnSdk23OrAbove(GetPermissionsRequired(identifier), GetRequestId(identifier));
            return isPermitted;
        }

        private static Boolean GetPermissionOnSdk23OrAbove(String[] permissions, Int32 requestId)
        {
            bool PermissionTCS = false;
            if (MainApplication.CurrentContext.CheckSelfPermission(permissions[0]) == (Int32)Permission.Granted)
                PermissionTCS = true;
            else
                ActivityCompat.RequestPermissions((Activity)MainApplication.CurrentContext, permissions, requestId);
            return PermissionTCS;

        }

        public static void OnRequestPermissionsResult(Permission[] grantResults)
        {
            PermissionTCS = grantResults[0] == Permission.Granted;
        }
    }
}