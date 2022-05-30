$(document).ready(function () {

    $('.btn_verificar_email').on('click', function () {
        Loading.start();

        let email = $('#email_recuperar').val();
        
        if (email.trim() === "") {
            Messages('warning', 'ATENÇÃO!', 'Favor informar seu e-mail.');
            Loading.done();
        }
        else {

            $.ajax(
                {
                    type: 'GET',
                    url: $URL_PRINCIPAL + 'Login/ValidarEmailInformado?email=' + email,
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    datatype: 'json',
                    beforeSend: function (xhr) {
                    },
                    success: function (data) {

                        if (data === 'True') {

                            $.ajax(
                                {
                                    type: 'GET',
                                    url: $URL_PRINCIPAL + 'Login/EnviarLinkRecuperacao?email=' + email + '&urlSite=' + $URL_PRINCIPAL + 'Login/RedefinirSenhaAtual/',
                                    async: false,
                                    contentType: "application/json; charset=utf-8",
                                    datatype: 'json',
                                    beforeSend: function (xhr) {
                                    },
                                    success: function (data) {
                                        if (data === 'True') {
                                            Messages('success', 'SUCESSO!', 'Acesse o seu e-mail!');
                                            Loading.done();
                                        }
                                        else {
                                            Messages('error', 'ATENÇÃO!', 'Esse e-mail não esta cadastrado.');
                                            Loading.done();
                                        }
                                    },
                                    error: function () {
                                        Messages('error');
                                        Loading.done();
                                    }
                                });

                        }
                        else {
                            Messages('error', 'ATENÇÃO!', 'Esse e-mail não esta cadastrado, informe uma conta válida.');
                            Loading.done();
                        }
                    },
                    error: function () {
                        Messages('error');
                        Loading.done();
                    }
                });

        }
    });




    $('.btn_salvar_nova_senha').on('click', function () {
        Loading.start();

        let email = $(this).attr("data-emailUser");
        let senha1 = $("#senha1_recuperar").val();
        let senha2 = $("#senha2_recuperar").val();

        if (senha1 != senha2) {
            Messages('error', 'ATENÇÃO!', 'Senhas não conferem.');
            Loading.done();
        } else {

            $.ajax(
                {
                    type: 'GET',
                    url: $URL_PRINCIPAL + 'Login/AlterarSenha?email=' + email + '&senha=' + senha1,
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    datatype: 'json',
                    beforeSend: function (xhr) {
                    },
                    success: function (data) {
                        if (data === 'True') {
                            Messages('success', 'SUCESSO!', 'Senha alterada com sucesso !');
                            Loading.done();
                            window.location.href = $URL_PRINCIPAL + "Login/";
                        }
                        else {
                            Messages('error', 'ATENÇÃO!', 'Tivemos problemas, tente novamente mais tarde.');
                            Loading.done();
                        }
                    },
                    error: function () {
                        Messages('error');
                        Loading.done();
                    }
                });
        }
    });
});