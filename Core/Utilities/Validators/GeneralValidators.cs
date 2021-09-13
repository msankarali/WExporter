using System.IO;
using System.Linq;

namespace Core.Utilities.Validators
{
    public class GeneralValidators
    {
        public static bool ValidateExtensionFormat(
            string allowedExtensions,
            params string[] fileNames)
        {
            return fileNames.All(fn =>
                allowedExtensions.Split(",").All(ae =>
                    Path.GetExtension(fn).Contains(ae)
                    )
                );
        }

        public static bool ValidateEmailFormat(string email)
        {
            try
            {
                var address = new System.Net.Mail.MailAddress(email);
                return address.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}