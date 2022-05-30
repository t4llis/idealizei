
    function ValidarEmail() {

        let email = $('#edt_email').val();
        if (email.trim() === "") {
            Messages('warning', '', 'Favor informar seu e-mail.');
        }
        else {

            $.ajax(
                {
                    type: 'GET',
                    url: $URL_PRINCIPAL + 'Login/ValidarEmailInformado?email=' + email,
                    async: true,
                    contentType: "application/json; charset=utf-8",
                    datatype: 'json',
                    beforeSend: function (xhr) {
                    },
                    success: function (data) {

                        if (data === 'True') {
                            Messages('warning', '', 'E-mail já cadastrado.');
                        }
                        else {
                            return true;
                        }
                    },
                    error: function () {
                        Messages('error');
                    }
                }
            );

        }
    }

    function ValidarSenha() {

        let pass = $('#edt_senha').val();
        if (pass.trim() === "") {
            Messages('warning', '', 'Favor informar sua senha.');
        }
    }

    function ValidarNome() {

        let nome = $('#edt_nome').val();
        if (nome.trim() === "") {
            Messages('warning', '', 'Favor informar seu nome.');
        }
    }