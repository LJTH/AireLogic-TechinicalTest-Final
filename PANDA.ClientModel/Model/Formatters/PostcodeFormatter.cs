using System.Text.RegularExpressions;

namespace PANDA.ClientModel.Model.Formatters
{
    internal static class PostcodeFormatter
    {
        public static string FormatPostcode(string postcode)
        {
            if (string.IsNullOrWhiteSpace(postcode))
            {
                return string.Empty;
            }

            // Remove extra spaces and convert to uppercase
            postcode = postcode.ToUpper().Replace(" ", "");

            // Match valid UK postcode formats
            var match = Regex.Match(postcode,
                @"^([A-Z]{1,2}[0-9][0-9A-Z]?)([0-9][A-Z]{2})$");

            if (!match.Success)
            {
                return "Invalid Postcode";
            }

            // Reformat with a single space
            return $"{match.Groups[1].Value} {match.Groups[2].Value}";
        }

    }
}
