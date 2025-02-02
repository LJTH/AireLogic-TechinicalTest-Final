namespace PANDA.Service.Validation
{
    internal static class NhsNumberValidator
    {
        public static bool IsValidNHSNumber(string nhsNumber)
        {
            // Ensure the NHS number is exactly 10 digits
            if (string.IsNullOrWhiteSpace(nhsNumber) || nhsNumber.Length != 10 || !nhsNumber.All(char.IsDigit))
            {
                return false;
            }

            // Convert string to integer array
            int[] digits = nhsNumber.Select(c => c - '0').ToArray();

            // Compute checksum using the first 9 digits
            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                sum += digits[i] * (10 - i);
            }

            int remainder = sum % 11;
            int checksum = (11 - remainder) % 11; // Handles the special case where remainder is 0

            // If the checksum is 10, the NHS number is invalid
            if (checksum == 10)
            {
                return false;
            }

            // The 10th digit should match the calculated checksum
            return digits[9] == checksum;
        }

        public static void Main()
        {
            string nhsNumber = "9434765919"; // Example NHS number
            Console.WriteLine($"NHS Number {nhsNumber} is {(IsValidNHSNumber(nhsNumber) ? "valid" : "invalid")}");
        }
    }

}