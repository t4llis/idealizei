$(document).ready(function () {

    $('.btn-enviar_ideia_email').on('click', function () {

        Loading.start();
        
        let ObjEmail = '{';
        let caminho_compartilhar = $URL_PRINCIPAL + $('.caminho_compartilhar').html();

        const emails = window.emailFeedback.getValue();

        ObjEmail += '"EMAIL_DESTINATARIO":"' + emails + '",';
        let id_ideia = $(this).attr("data-ideia");
        let tipo_pagina = $(this).attr("data-tipoPagina");
       
        if (shareEnum == "COCRIACAO") {

            ObjEmail += '"ASSUNTO":"'    +"Link cocriador Idealizei" + '",';
            ObjEmail += '"ID_IDEIA":"'   + id_ideia + '",';
            ObjEmail += '"TIPO_ENVIO":"' + tipo_pagina + '",';
            ObjEmail += '"MENSAGEM":"' + "Olá, \n Você foi escolhido por {0} para cocriar uma ideia na Plataforma(?) Idealizei. \n "+
                                         "Sua ajuda é muito importante para {0}.\n " +
                                         "Acesse  " + caminho_compartilhar + " e ajude a idealizar essa ideia. \n " +
                                         "E-mail automático, não retorne." + '"';
        } else {
            ObjEmail += '"ASSUNTO":"' + "Link Avaliador Idealizei" + '",';
            ObjEmail += '"ID_IDEIA":"' + id_ideia + '",';
            ObjEmail += '"TIPO_ENVIO":"' + tipo_pagina + '",';
            ObjEmail += '"MENSAGEM":"' + "Olá, \n Você foi escolhido por {0} para cocriar uma ideia na Plataforma(?) Idealizei. \n " +
                                         "Sua ajuda é muito importante para {0}. \n " +
                                         "Acesse  " + caminho_compartilhar + " e ajude a avaliar essa ideia. \n " + 
                                         "E-mail automático, não retorne." + '"';
        }        
        
        ObjEmail += '}';

        $.ajax(
            {
                type: 'POST',
                url: $URL_PRINCIPAL + 'Ideia/EnviarEmail',
                data: ObjEmail,
                async: true,
                contentType: "application/json; charset=utf-8",
                datatype: 'json',
                beforeSend: function (xhr) {
                },
                success: function (data) {
                    Loading.done();
                    if (data) {
                        Messages('success', 'SUCESSO!', 'E-mail enviado com sucesso!');                        
                    }
                    else
                        Messages('warning', 'ATENÇÃO!', 'Tivemos problemas ao tentar enviar seu e-mail, verifique o destinatario.');
                },
                error: function () {
                    Loading.done();
                    Messages('error');
                }
            });

    });


    $('.btn_copy_link').on('click', function () {
        let link_share = $URL_PRINCIPAL + $('.caminho_compartilhar').html();
        console.log(link_share);
        $('#copy_link_value').val(link_share);
        $('#copy_link_value').select();
        document.execCommand('copy');
    });




    $('.btn_copy_link').on('click', function () {
        let link_share = $URL_PRINCIPAL + $('.caminho_compartilhar').html();
        console.log(link_share);
        $('#copy_link_value').val(link_share);
        $('#copy_link_value').select();
        document.execCommand('copy');

    });


});

