(function (EmailsInput) {
    'use strict'

    document.addEventListener('DOMContentLoaded', function () {        
        // Input e-mails laboratório de ideias
        const inputContainerNode = document.querySelector('#InputTextoEmails');
        const emailsInput = EmailsInput(inputContainerNode, { triggerKeyCodes: [13, 32], placeholder: 'Informe os emails...' });

        window.emailsInput = emailsInput;  

        // Input e-mails feedbacks
        const inputEmailFeedback = document.querySelector('#input_email_compartilhar');
        const emailFeedback = EmailsInput(inputEmailFeedback, { triggerKeyCodes: [13, 32], placeholder: 'Informe os emails...' });

        window.emailFeedback = emailFeedback;  
    })

}(window.lib.EmailsInput))