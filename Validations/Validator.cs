using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Snacks_App.Validations
{
    public class Validator : IValidator
    {
        public string NameError { get; set; } = "";
        public string EmailError { get; set; } = "";
        public string PhoneError { get; set; } = "";
        public string PasswordError { get; set; } = "";

        private const string NameEmptyErrorMsg = "Por favor, informe o seu nome.";
        private const string NameInvalidErrorMsg = "Por favor, informe um nome válido.";
        private const string EmailEmptyErrorMsg = "Por favor, informe um email.";
        private const string EmailInvalidErrorMsg = "Por favor, informe um email válido.";
        private const string PhoneEmptyErrorMsg = "Por favor, informe um telefone.";
        private const string PhoneInvalidErrorMsg = "Por favor, informe um telefone válido.";
        private const string PasswordEmptyErrorMsg = "Por favor, informe a senha.";
        private const string PasswordInvalidErrorMsg = "A senha deve conter pelo menos 8 caracteres, incluindo letras e números.";

        public Task<bool> Validate(string name, string email, string phone, string password)
        {
            var isNameValid = ValidateName(name);
            var isEmailValid = ValidateEmail(email);
            var isPhoneValid = ValidatePhone(phone);
            var isPasswordValid = ValidatePassword(password);

            return Task.FromResult(isNameValid && isEmailValid && isPhoneValid && isPasswordValid);
        }

        private bool ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                NameError = NameEmptyErrorMsg;
                return false;
            }

            if (name.Length < 3)
            {
                NameError = NameInvalidErrorMsg;
                return false;
            }

            NameError = "";
            return true;
        }

        private bool ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                EmailError = EmailEmptyErrorMsg;
                return false;
            }

            if (!Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                EmailError = EmailInvalidErrorMsg;
                return false;
            }

            EmailError = "";
            return true;
        }

        private bool ValidatePhone(string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                PhoneError = PhoneEmptyErrorMsg;
                return false;
            }

            if (phone.Length < 12)
            {
                PhoneError = PhoneInvalidErrorMsg;
                return false;
            }

            PhoneError = "";
            return true;
        }

        private bool ValidatePassword(string pass)
        {
            if (string.IsNullOrEmpty(pass))
            {
                PasswordError = PasswordEmptyErrorMsg;
                return false;
            }

            if (pass.Length < 8 || !Regex.IsMatch(pass, @"[a-zA-Z]") || !Regex.IsMatch(pass, @"\d"))
            {
                PasswordError = PasswordInvalidErrorMsg;
                return false;
            }

            PasswordError = "";
            return true;
        }

    }
}
