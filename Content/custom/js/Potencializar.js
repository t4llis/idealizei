$(document).ready(function () {

    if ((window.location.href === $URL_PRINCIPAL + 'Potencializar/') ||
        (window.location.href === $URL_PRINCIPAL + 'Potencializar')) {
        $('.potencializar-menu').addClass("active");
        $('.potencializar-menu').find('ul').attr('style', 'display: block;');
        
    }

    $('.btn-submit-orcamento').on('click', function () {
        console.log('click funcionou');
        let nome = $('#nome').val();
        let email = $('#email').val();
        let assunto = $('#assunto option:selected').val();
        let mensagem = $('#mensagem').val();

        if (nome.trim() === "")
        {
            Messages('warning', 'ATENÇÃO!', 'Favor informar um nome.');
        }
        else if (email.trim() === "")
        {
            Messages('warning', 'ATENÇÃO!', 'Favor informar seu e-mail.');
        }
        else if (assunto.trim() === "")
        {
            Messages('warning', 'ATENÇÃO!', 'Favor informar o assunto.');
        }
        else if (mensagem.trim() === "")
        {
            Messages('warning', 'ATENÇÃO!', 'Favor informar um texto na mensagem.');
        }
        else
        {

                let ObjEmail = '{';

                ObjEmail += '"NOME":"' + nome + '",';
                ObjEmail += '"EMAIL":"' + email + '",';
                ObjEmail += '"ASSUNTO":"' + assunto + '",';
                ObjEmail += '"MENSAGEM":"' + mensagem + '"';

                ObjEmail += '}';

                $.ajax(
                    {
                        type: 'POST',
                        url: $URL_PRINCIPAL + 'Potencializar/Contatar',
                        data: ObjEmail,
                        async: true,
                        contentType: "application/json; charset=utf-8",
                        datatype: 'json',
                        beforeSend: function (xhr) {
                        },
                        success: function (data) {
                            if (data) {
                                Messages('success', 'SUCESSO!', 'Mensagem enviada com sucesso!');
                                location.reload();
                            }
                            else
                                Messages('warning', 'ATENÇÃO!', 'Tivemos problemas ao tentar enviar sua mensagem.');
                        },
                        error: function () {
                            Messages('error');
                        }
                    });

             }
    });
});